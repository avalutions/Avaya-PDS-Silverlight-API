using System;

namespace Dialer.Communication
{
    public class AgentStateArgs : EventArgs
    {
        public AgentState OldState { get; set; }
        public AgentState NewState { get; set; }
    }
}
