using System;
using System.Collections.Generic;

namespace Dialer.Communication.Messages
{
    public class ListCallLists : DialerMessage
    {
        public List<String> CallLists { get; private set; }

        internal override void Parse(List<string> segments)
        {
            CallLists = new List<string>(segments);
        }
    }
}
