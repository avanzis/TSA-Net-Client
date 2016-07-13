using System;
using TSANetClient.Interfaces;
using TSANetClient.Providers;

namespace TSANetClient.Services
{
    public class TimeStampService : ITimeStampService
    {
        private readonly ITimeStampProviderClient _timeStampClient;

        public TimeStampService(ITimeStampProviderClient timeStampClient)
        {
            _timeStampClient = timeStampClient;
        }

        public TimeStampResult GenerateTimeStamp(string content)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentNullException(nameof(content));

            var result = new TimeStampResult { IsSuccess = false };

            var apiResult = _timeStampClient.GenerateTimeStamp(content);

            if (apiResult.IsSuccess)
            {
                result.IsSuccess = true;
                result.TimeStampXml = apiResult.ApiResponse;
                result.Signature = apiResult.Signature;
                result.TimeStampDateTime = apiResult.TimeStampDateTime;
                result.TimeStampExpirationDateTime = apiResult.TimeStampExpirationDateTime;
                result.OriginalContent = apiResult.OriginalContent;
            }

            return result;
        }

        public TimeStampResult VerifyTimeStamp(string content, string signature)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrEmpty(signature)) throw new ArgumentNullException(nameof(signature));

            var result = new TimeStampResult { IsSuccess = false };
            var apiResult = _timeStampClient.VerifyTimeStamp(content, signature);

            if (apiResult.IsSuccess)
            {
                result.IsSuccess = true;
                result.TimeStampXml = apiResult.ApiResponse;
            }

            return result;
        }
    }
}
