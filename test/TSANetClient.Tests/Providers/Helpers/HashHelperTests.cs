using System;
using NUnit.Framework;
using TSANetClient.Providers.Helpers;

namespace TSANetClient.Tests.Providers.Helpers
{
    [TestFixture]
    public class HashHelperTests
    {
        private string _textToTest;
        private string _encodedResult;

        private byte[] _expectedBytes;
        private byte[] _expectedHashedBytes;

        private string _expectedHashedString;

        [SetUp]
        public void SetUp()
        {
            _textToTest = "test";
            _encodedResult = "dGVzdA==";

            _expectedBytes = new byte[] { 116, 101, 115, 116 };
            _expectedHashedBytes = new byte[]
                                       {
                                           169, 74, 143, 229, 204, 177, 155, 166, 28, 76, 8, 115, 211, 145, 233, 135, 152,
                                           47, 187, 211
                                       };

            _expectedHashedString = "qUqP5cyxm6YcTAhz05Hph5gvu9M=";
        }

        [Test]
        public void EncodeTo64_Null_String_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => HashHelper.EncodeTo64((string)null));
        }

        [Test]
        public void EncodeTo64_Empty_String_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => HashHelper.EncodeTo64(string.Empty));
        }

        [Test]
        public void EncodeTo64_Null_Bytes_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => HashHelper.EncodeTo64((byte[])null));
        }

        [Test]
        public void DecodeFrom64_Null_String_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => HashHelper.DecodeFrom64((string)null));
        }

        [Test]
        public void DecodeFrom64_Empty_String_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => HashHelper.DecodeFrom64(string.Empty));
        }

        [Test]
        public void EncodeTo64_Returns_Encoded64_String()
        {
            var result = HashHelper.EncodeTo64(_textToTest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(_encodedResult));
        }

        [Test]
        public void DecodeFrom64_Returns_Decoded4_String()
        {
            var result = HashHelper.DecodeFrom64(_encodedResult);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(_textToTest));
        }

        [Test]
        public void EncodeAndDecode_Returns_Base_String()
        {
            var encoded = HashHelper.EncodeTo64(_textToTest);
            var decoded = HashHelper.DecodeFrom64(encoded);

            Assert.That(_textToTest, Is.EqualTo(decoded));
        }

        [Test]
        public void StrToByteArray_Null_String_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => HashHelper.StrToByteArray(null));
        }

        [Test]
        public void StrToByteArray_Empty_String_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => HashHelper.StrToByteArray(string.Empty));
        }

        [Test]
        public void StrToByteArray_Return_Bytes_From_String()
        {
            var result = HashHelper.StrToByteArray(_textToTest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(_expectedBytes));

        }

        [Test]
        public void ByteArrayToStr_Null_Bytes_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => HashHelper.ByteArrayToStr(null));
        }

        [Test]
        public void ByteArrayToStr_Returns_String_From_Bytes()
        {
            var result = HashHelper.ByteArrayToStr(_expectedBytes);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(_textToTest));
        }

        [Test]
        public void HashString_Null_String_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => HashHelper.HashString(null));
        }

        [Test]
        public void HashString_Empty_String_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => HashHelper.HashString(string.Empty));
        }

        [Test]
        public void HashString_Returns_Hash_Of_String()
        {
            var result = HashHelper.HashString(_textToTest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(_expectedHashedBytes));
        }

        [Test]
        public void HashString_Returns_Same_Value_If_Hash_Several_Times()
        {
            var result = HashHelper.HashString(_textToTest);
            var result2 = HashHelper.HashString(_textToTest);
            var result3 = HashHelper.HashString(_textToTest);


            Assert.That(result, Is.EqualTo(result2));
            Assert.That(result, Is.EqualTo(result3));
        }

        [Test]
        public void HashHelper_Returns_Hashed_String()
        {
            var result = HashHelper.EncodeTo64(HashHelper.HashString(_textToTest));

            Assert.That(result,Is.Not.Null);
            Assert.That(result, Is.EqualTo(_expectedHashedString));
        }
    }
}
