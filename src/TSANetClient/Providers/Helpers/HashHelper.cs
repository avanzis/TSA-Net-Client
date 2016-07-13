using System;
using System.Security.Cryptography;
using System.Text;

namespace TSANetClient.Providers.Helpers
{
    internal static class HashHelper
    {
        public static string EncodeTo64(string toEncode)
        {
            if (string.IsNullOrEmpty(toEncode)) throw new ArgumentNullException(nameof(toEncode));

            return EncodeTo64(Encoding.UTF8.GetBytes(toEncode));
        }

        public static string EncodeTo64(byte[] toEncode)
        {
            if (toEncode == null) throw new ArgumentNullException(nameof(toEncode));

            return Convert.ToBase64String(toEncode);
        }

        public static string DecodeFrom64(string toDecode)
        {
            if (string.IsNullOrEmpty(toDecode)) throw new ArgumentNullException(nameof(toDecode));

            return Encoding.UTF8.GetString(Convert.FromBase64String(toDecode));
        }

        public static byte[] StrToByteArray(string str)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));

            return Encoding.UTF8.GetBytes(str);
        }

        public static string ByteArrayToStr(byte[] bytesToConvert)
        {
            if (bytesToConvert == null) throw new ArgumentNullException(nameof(bytesToConvert));

            return Encoding.UTF8.GetString(bytesToConvert);
        }

        public static byte[] HashString(string str)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));

            using (var sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(Encoding.ASCII.GetBytes(str));
            }
        }
    }
}
