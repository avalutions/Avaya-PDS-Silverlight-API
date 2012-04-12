using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialer.Communication
{
    public interface IDialerConnection
    {
        string IPAddress { get; set; }
        int Port { get; set; }
        IAgent Agent { get; set; }

        event EventHandler<MessageArgs> NewMessage;

        void Reset();
    }
}
