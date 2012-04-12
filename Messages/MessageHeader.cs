using System;
using System.Text.RegularExpressions;

namespace Dialer.Communication.Messages
{
    public class MessageHeader
    {
        public String Name { get; set; }
        public MessageType Type { get; set; }
        public String Client { get; set; }
        public String ProcID { get; set; }
        public String InvokeID { get; set; }
        public Int32 NumberOfSegments { get; set; }

        public override string ToString()
        {
            return string.Format("{0,-20}{1}{2,-20}{3,-6}{4,-4}{5,-4}",
                                   Name,
                                   (char)Type,
                                   Client,
                                   ProcID,
                                   InvokeID,
                                   NumberOfSegments);
        }

        public static MessageHeader FromString(String headerText)
        {
            MessageHeader result = null;
            Match match = Regex.Match(headerText, @"(?<Name>[A-Za-z\s]{20})(?<Type>[CDPRN])(?<Client>[A-Za-z\s]{20})(?<Proc>[\w\s]{6})(?<Invoke>[\w\s]{4})(?<Seg>[0-9\s]{4})");
            if (match.Success)
            {
                result = new MessageHeader();
                if (match.Groups["Name"].Success)
                    result.Name = match.Groups["Name"].Value.Trim();
                if (match.Groups["Type"].Success)
                    result.Type = (MessageType)((int)match.Groups["Type"].Value[0]);
                if (match.Groups["Client"].Success)
                    result.Client = match.Groups["Client"].Value.Trim();
                if (match.Groups["Proc"].Success)
                    result.ProcID = match.Groups["Proc"].Value.Trim();
                if (match.Groups["Invoke"].Success)
                    result.InvokeID = match.Groups["Invoke"].Value.Trim();
                if (match.Groups["Seg"].Success)
                    result.NumberOfSegments = int.Parse(match.Groups["Seg"].Value.Trim());
            }
            return result;
        }
    }
}
