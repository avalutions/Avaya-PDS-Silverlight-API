using System;
using System.Collections.Generic;
using Dialer.Communication.Common;

namespace Dialer.Communication.Messages
{
    public class ListCallFields : DialerMessage
    {
        public List<Field> Fields { get; private set; }

        internal override void Parse(List<string> segments)
        {
            Fields = new List<Field>();
            while (segments.Count > 0)
            {
                string[] raw = segments[0].Split(',');
                DataField field = new DataField
                {
                    Name = raw[0],
                    Length = Convert.ToInt32(raw[1]),
                    Type = raw[2]
                };
                Fields.Add(field);
            }
        }
    }
}
