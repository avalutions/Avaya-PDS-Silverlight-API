using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dialer.Communication.Common;

namespace Dialer.Communication.Messages
{
    public class ReadField : DialerMessage
    {
        public DataField Field { get; private set; }

        internal override void Parse(List<string> segments)
        {
            if (segments.Count > 0)
            {
                string[] paramaters = segments[0].Split(',');
                Field = new DataField
                {
                    Name = paramaters[0],
                    Type = paramaters[1],
                    Length = Convert.ToInt32(paramaters[2]),
                    Value = paramaters[3]
                };
            }
        }
    }
}
