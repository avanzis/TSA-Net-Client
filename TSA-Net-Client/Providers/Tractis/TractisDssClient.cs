using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using TSA_Net_Client.Providers.Helpers;

namespace TSA_Net_Client.Providers.Tractis
{
    public class TractisDssClient : ITimeStampProviderClient
    {
        public TractisDssClientProperties Configuration { get; }

        public TractisDssClient(string username, string password)
        {
            Configuration = new TractisDssClientProperties
            {
                ApiUserName = username,
                ApiPassword = password
            };
        }

        public TimeStampProviderApiResult GenerateTimeStamp(string contentToStamp)
        {
            if (string.IsNullOrEmpty(contentToStamp)) throw new ArgumentNullException(nameof(contentToStamp));

            var result = new TimeStampProviderApiResult { IsSuccess = false };

            var dataXml = string.Format(RequestTemplates.StampRequest, HashHelper.EncodeTo64(contentToStamp));

            var apiResponse = SendRequest(dataXml, Configuration.ApiTimeStampUrl);

            if (!string.IsNullOrEmpty(apiResponse))
            {
                result.ApiResponse = new XmlDocument();
                result.ApiResponse.LoadXml(apiResponse);

                var tractisDssNamespace = new XmlNamespaceManager(result.ApiResponse.NameTable);
                tractisDssNamespace.AddNamespace("dss", "urn:oasis:names:tc:dss:1.0:core:schema");
                tractisDssNamespace.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                tractisDssNamespace.AddNamespace("ns3", "urn:oasis:names:tc:dss:1.0:core:schema");

                result.IsSuccess = result.ApiResponse.SelectSingleNode("//dss:ResultMajor", tractisDssNamespace).InnerText.ToLower().Contains("success");
                result.Signature = ExtractSignatureString(apiResponse);
                result.TimeStampDateTime = DateTime.Parse(result.ApiResponse.SelectSingleNode("//ns3:SignResponse/ns3:OptionalOutputs/ns3:SigningTimeInfo/ns3:SigningTime", tractisDssNamespace).InnerText);
                result.OriginalContent = contentToStamp;

                var certificateText = result.ApiResponse.SelectSingleNode("//ds:X509Certificate", tractisDssNamespace).InnerText;
                var certificate = new X509Certificate(HashHelper.StrToByteArray(certificateText));

                result.TimeStampExpirationDateTime = DateTime.Parse(certificate.GetExpirationDateString());
            }

            return result;
        }

        private static string ExtractSignatureString(string apiResponse)
        {
            // This is done this way to avoid any manipulation of the original XML response

            var signatureStartPosition = apiResponse.IndexOf("<ds:Signature");
            var signatureEndPosition = apiResponse.IndexOf("</ns3:SignatureObject>");

            return apiResponse.Substring(signatureStartPosition, apiResponse.Length - signatureStartPosition - (apiResponse.Length - signatureEndPosition));
        }

        public TimeStampProviderApiResult VerifyTimeStamp(string contentToVerify, string signature)
        {
            if (string.IsNullOrEmpty(contentToVerify)) throw new ArgumentNullException(nameof(contentToVerify));
            if (string.IsNullOrEmpty(signature)) throw new ArgumentNullException(nameof(signature));

            var result = new TimeStampProviderApiResult { IsSuccess = false };

            var dataXml = string.Format(RequestTemplates.VerifyRequest, HashHelper.EncodeTo64(HashHelper.HashString(contentToVerify)), signature);

            var apiResponse = SendRequest(dataXml, Configuration.ApiVerifyUrl);

            if (!string.IsNullOrEmpty(apiResponse))
            {
                result.ApiResponse = new XmlDocument();
                result.ApiResponse.LoadXml(apiResponse);

                var tractisDssNamespace = new XmlNamespaceManager(result.ApiResponse.NameTable);
                tractisDssNamespace.AddNamespace("dss", "urn:oasis:names:tc:dss:1.0:core:schema");
                tractisDssNamespace.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

                result.IsSuccess =
                    result.ApiResponse.SelectSingleNode("//dss:ResultMajor", tractisDssNamespace).InnerText.ToLower().Contains("success") &&
                    result.ApiResponse.SelectSingleNode("//dss:ResultMinor", tractisDssNamespace).InnerText.ToLower().Contains("onalldocuments");

                result.OriginalContent = contentToVerify;
            }

            return result;
        }

        private string SendRequest(string dataToSend, string url)
        {
            var httpWebRequest = CreateHttpWebRequest(dataToSend, url);

            using (var response = httpWebRequest.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private HttpWebRequest CreateHttpWebRequest(string dataToSend, string url)
        {
            byte[] dataXmlByteArray = HashHelper.StrToByteArray(dataToSend);

            WebRequest request = WebRequest.Create(url);


            var httpWebRequest = request as HttpWebRequest;
            if (httpWebRequest != null)
            {
                httpWebRequest.ContentType = "text/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "*/*";

                var authorization = BuildAuthorization(Configuration.ApiUserName, Configuration.ApiPassword);
                request.Headers.Add(HttpRequestHeader.Authorization, authorization);

                httpWebRequest.ContentLength = dataXmlByteArray.Length;

                using (var dataStream = httpWebRequest.GetRequestStream())
                {
                    dataStream.Write(dataXmlByteArray, 0, dataXmlByteArray.Length);
                }
            }

            return httpWebRequest;
        }

        private static string BuildAuthorization(string username, string password)
        {
            var credentials = $"{username}:{password}";
            var bytes = Encoding.ASCII.GetBytes(credentials);
            var base64 = Convert.ToBase64String(bytes);
            return "Basic " + base64;
        }
    }
}
