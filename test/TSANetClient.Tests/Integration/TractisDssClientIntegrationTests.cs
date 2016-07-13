using NUnit.Framework;
using TSANetClient.Providers.Tractis;

namespace TSANetClient.Tests.Integration
{
    [TestFixture]
    public class TractisDssClientIntegrationTests
    {
        private TractisDssClient _client;
        private string _contentToTest;

        [SetUp]
        public void SetUp()
        {
            _client = new TractisDssClient("username", "password");
            _contentToTest = "text to be timestamped";
        }

        [Test]
        public void GenerateTimeStamp_Generates_New_TimeStamp_And_Verify_It()
        {
            var result = _client.GenerateTimeStamp(_contentToTest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.ApiResponse, Is.Not.Null);
            Assert.That(result.Signature,Is.Not.Null);
            Assert.That(result.Signature,Is.Not.EqualTo(string.Empty));
            Assert.That(result.TimeStampDateTime.HasValue, Is.True);
            Assert.That(result.TimeStampExpirationDateTime.HasValue, Is.True);

            var verifyingResult = _client.VerifyTimeStamp(_contentToTest, result.Signature);

            Assert.That(verifyingResult, Is.Not.Null);
            Assert.That(verifyingResult.IsSuccess, Is.True);
            Assert.That(verifyingResult.ApiResponse, Is.Not.Null);
            Assert.That(verifyingResult.Signature, Is.Null);
            Assert.That(verifyingResult.TimeStampDateTime.HasValue, Is.False);
        }
    }
}
