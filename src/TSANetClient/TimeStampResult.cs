using System;
using System.Xml;

namespace TSANetClient
{
    public class TimeStampResult
    {
        public string OriginalContent { get; set; }
        public XmlDocument TimeStampXml { get; set; }
        public string Signature { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime? TimeStampDateTime { get; set; }
        public DateTime? TimeStampExpirationDateTime { get; set; }
    }
}
