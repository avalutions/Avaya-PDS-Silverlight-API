using System.ComponentModel;

namespace Dialer.Communication
{
    public enum AgentState
    {
        [StateMove("be logged off")]
        LoggedOff = 0,
        [StateMove("be logged on")]
        LoggedOn = 1,
        [StateMove("go on break")]
        OnBreak = 2,
        [StateMove("become available")]
        Available = 3,
        [StateMove("be updating")]
        Updating = 4,
        [StateMove("get on a call")]
        OnCall = 5,
        [StateMove("transfer the call")]
        Transferring = 6,
        [StateMove("be disconnected")]
        Disconnected = 7
    }
}
