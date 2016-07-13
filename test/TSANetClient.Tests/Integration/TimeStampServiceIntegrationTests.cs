using NUnit.Framework;
using TSANetClient.Providers;
using TSANetClient.Providers.Tractis;
using TSANetClient.Services;

namespace TSANetClient.Tests.Integration
{
    [TestFixture]
    public class TimeStampServiceIntegrationTests
    {
        private ITimeStampProviderClient _timeStampProviderClient;
        private TimeStampService _timeStampService;
        private string _content;
        private string _timestampSignature;

        [SetUp]
        public void SetUp()
        {
            _timeStampProviderClient = new TractisDssClient("username", "password");
            _timeStampService = new TimeStampService(_timeStampProviderClient);
            _content = "anything";
            _timestampSignature = "incorrect signature";
        }

        [Test]
        public void Service_GetTimeStamp_Returns_TimeStamp()
        {
            var result = _timeStampService.GenerateTimeStamp(_content);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.TimeStampXml, Is.Not.Null);
            Assert.That(result.TimeStampDateTime.HasValue, Is.True);
            Assert.That(result.TimeStampExpirationDateTime.HasValue, Is.True);
            Assert.That(result.OriginalContent, Is.EqualTo(_content));
        }

        [Test]
        public void Service_VerifyTimeStamp_Returns_False_If_Signature_Is_Invalid()
        {
            var result = _timeStampService.VerifyTimeStamp(_content, _timestampSignature);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.TimeStampXml, Is.Null);
            Assert.That(result.Signature, Is.Null);
            Assert.That(result.TimeStampDateTime.HasValue, Is.False);
            Assert.That(result.TimeStampExpirationDateTime.HasValue, Is.False);

        }
    }
}
