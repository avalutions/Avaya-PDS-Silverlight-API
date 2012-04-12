using System;

namespace Dialer.Communication.Messages
{
    public interface IMessage
    {
        bool IsError { get; set; }
        String Message { get; set; }
    }
}
