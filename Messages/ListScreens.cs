using System.Collections.Generic;

namespace Dialer.Communication.Messages
{
    public class ListScreens : DialerMessage
    {
        public List<string> Screens { get; private set; }

        internal override void Parse(List<string> segments)
        {
            Screens = new List<string>(segments);
        }
    }
}
