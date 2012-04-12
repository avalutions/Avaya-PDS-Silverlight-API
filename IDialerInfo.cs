using System.Collections.Generic;
using Dialer.Communication.Common;

namespace Dialer.Communication
{
    public interface IDialerInfo
    {
        IDialerConnection Connection { get; set; }
        List<CompletionCode> Keys { get; }
        List<Job> Jobs { get; }
    }
}
