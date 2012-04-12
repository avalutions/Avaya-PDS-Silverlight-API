using System;
using Dialer.Communication.Messages;

namespace Dialer.Communication
{
    public class MessageArgs : EventArgs
    {
        public IAgent Agent { get; set; }
        public IMessage Message { get; set; }
        public bool IsTerminating { get; set; }
    }
}
