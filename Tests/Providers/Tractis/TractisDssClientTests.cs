using System;
using NUnit.Framework;
using TSA_Net_Client.Providers.Tractis;

namespace Tests.Providers.Tractis
{
    [TestFixture]
    public class TractisDssClientTests
    {
        private TractisDssClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new TractisDssClient();
        }

        [Test]
        public void TractisDssClient_Overwrite_Properties_Overwrites_Properties()
        {
            var overwritedProperties = new TractisDssClientProperties();
            overwritedProperties.ApiPassword = "test";
            overwritedProperties.ApiTimeStampUrl = "url";
            overwritedProperties.ApiUserName = "test";

            _client.TractisDssClientProperties = overwritedProperties;

            Assert.That(_client.TractisDssClientProperties.ApiPassword, Is.EqualTo(overwritedProperties.ApiPassword));
            Assert.That(_client.TractisDssClientProperties.ApiTimeStampUrl, Is.EqualTo(overwritedProperties.ApiTimeStampUrl));
            Assert.That(_client.TractisDssClientProperties.ApiUserName, Is.EqualTo(overwritedProperties.ApiUserName));
        }

        [Test]
        public void GenerateTimeStamp_Null_Content_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _client.GenerateTimeStamp(null));
        }

        [Test]
        public void GenerateTimeStamp_Empty_Content_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _client.GenerateTimeStamp(string.Empty));
        }


        [Test]
        public void VerifyTimeStamp_Null_Content_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _client.VerifyTimeStamp(null, "signature"));
        }

        [Test]
        public void VerifyTimeStamp_Empty_Content_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _client.VerifyTimeStamp(string.Empty, "signature"));
        }


        [Test]
        public void VerifyTimeStamp_Null_Signature_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _client.VerifyTimeStamp("content", null));
        }

        [Test]
        public void VerifyTimeStamp_Empty_Signature_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _client.VerifyTimeStamp("content", string.Empty));
        }

    }
}
