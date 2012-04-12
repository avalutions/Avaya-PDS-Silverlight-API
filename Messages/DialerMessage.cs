using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Dialer.Communication.Messages
{
    public class DialerMessage : IMessage
    {
        private string _message = null;

        internal MessageHeader Header { get; set; }
        internal List<string> Segments { get; set; }
        public String Message { get; set; }
        public bool IsError
        {
            get
            {
                if (Segments != null)
                    return Segments[1].StartsWith("E");
                else
                    return false;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        internal virtual void Parse(List<string> segments) { }

        public static DialerMessage FromRaw(String data)
        {
            MessageHeader header = MessageHeader.FromString(data.Substring(0, 55));
            DialerMessage result = null;
            String[] segments = data.Substring(56).Split((char)MessageSeperator.Delimiter);
            Array.ForEach(segments, a => a = a.Replace(((char)MessageSeperator.Delimiter).ToString(), ""));
            string typeName = "Dialer.Communication.Messages." + header.Name.Replace("AGT", "");
            if (Type.GetType(typeName) != null && header.Type == MessageType.Data)
            {
                ConstructorInfo ci = Type.GetType(typeName).GetConstructor(Type.EmptyTypes);
                result = ci.Invoke(null) as DialerMessage;
            }
            else
                result = new DialerMessage();
            result.Header = header;
            result.Message = data;
            result.Segments = segments.ToList();
            result.Parse(segments.Skip(2).ToList());
            return result;
        }
    }
}
