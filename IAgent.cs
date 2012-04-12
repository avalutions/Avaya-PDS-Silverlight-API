using System;
using Dialer.Communication.Common;

namespace Dialer.Communication
{
    public interface IAgent
    {
        Job CurrentJob { get; }
        AgentState State { get; set; }
        String Name { get; set; }
        String Password { get; set; }
        Headset Headset { get; }
        IDialerConnection Connection { get; set; }
        Call CurrentCall { get; }

        event EventHandler<MessageArgs> NewMessage;
        event EventHandler<AgentStateChangedArgs> AgentStateChanged;
        event EventHandler<AgentStateArgs> AgentStateChanging;

        void Attach(Job jobToAttachTo);
        void Finish(CompletionCode completionCode);
        void Finish(int completionCode);
        void MoveToState(AgentState agentState);
        void MoveToState(AgentState agentState, bool force);
        void CallBack(DateTime callBackDate);
        void TransferCall(String destination);
        void TransferCall(TransferAction action);
    }
}
