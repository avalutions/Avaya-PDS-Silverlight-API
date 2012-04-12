using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commands = Dialer.Communication.Messages.Commands;
using Dialer.Communication.Common;
using Dialer.Communication.Messages;

namespace Dialer.Communication
{
    public class DialerAgent : IDisposable, IAgent, IDialerInfo
    {
        private DialerConnection _connection;
        private AgentState? _waitingState;
        private int callNotifications = 0;
        private bool _disconnected = false;

        #region destructor

        /// <summary>
        /// Finalizer method
        /// </summary>
        ~DialerAgent()
        {
        }

        #endregion

        #region private methods

        #region command helpers

        /// <summary>
        /// Used to dial a single digit
        /// </summary>
        /// <param name="digit">Digit to dial</param>
        //public void Dial(int digit)
        //{
        //    //Create and call the DialDigit command
        //    new Commands.DialDigit(digit.ToString()).Send(_connection);
        //}

        #endregion

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool closeCall)
        {
            _disposed = true;
            if (_connection != null)
                _connection.Dispose();
        }
        private bool _disposed;

        #endregion

        #region IAgent Membersbe cl

        /// <summary>
        /// The job we are currently attached to or nothing if we are not attached
        /// </summary>
        public Common.Job CurrentJob { get; private set; }
        public AgentState State { get; set; }
        /// <summary>
        /// The username of the current agent
        /// </summary>
        public string Name { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// Holds the headset information such as volume
        /// </summary>
        private Headset _headset = new Headset();
        public Headset Headset { get { return _headset; } }
        /// <summary>
        /// The _connection to the dialer that handles sending commands and delivers messages/notifications
        /// </summary>
        public IDialerConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value as DialerConnection;
                if (_connection != null)
                {
                    _connection.Agent = this;
                    _connection.NewMessage += new EventHandler<MessageArgs>(_connection_NewMessage);
                }
            }
        }

        public void TransferCall(string destination)
        {
            _connection.SendCommand(new Commands.TransferCall(destination));
        }

        public void TransferCall(TransferAction action)
        {
            switch (action)
            {
                case TransferAction.Conference:
                    _connection.SendCommand(new Commands.TransferCall(""));
                    break;
                case TransferAction.Release:
                    MoveToState(AgentState.Updating);
                    break;
                default:
                    break;
            }
        }

        void _connection_NewMessage(object sender, MessageArgs e)
        {
            if (e.Message != null && e.Message is DialerMessage)
            {
                if ((e.Message as DialerMessage).Header.Name == "AGTCallNotify")
                {
                    if ((e.Message as DialerMessage).Segments.Contains(Enum.GetName(typeof(Common.CallType), CurrentJob.Type).ToUpper()))
                    {
                        if((e.Message as DialerMessage).Segments.Count >= 5 && (e.Message as DialerMessage).Segments[4].Split(',').Count() >= 2)
                            CurrentCall.AccountNumber = (e.Message as DialerMessage).Segments[4].Split(',')[1];
                    }
                    else
                    {
                        var segs = (e.Message as DialerMessage).Segments.Skip(2).ToList();
                        if (segs.Count > 0)
                        {
                            CurrentCall.Screens.Clear();
                            CurrentCall.Screens.AddRange(CurrentJob.Screens);
                            for (int i = 0; i < segs.Count(); i++)
                            {
                                string[] pieces = segs[i].Split(',');

                                Screen screen = CurrentCall.Screens.FirstOrDefault(s => s.Fields.Count(f => f.Name == pieces[0]) > 0);
                                if (screen != null)
                                {
                                    DataField field = screen.Fields.OfType<DataField>().FirstOrDefault(f => f.Name == pieces[0]);
                                    if (field != null)
                                    {
                                        field.Value = pieces[1];
                                        field.IsDirty = false;
                                    }
                                }
                            }
                        }
                    }

                    if (++callNotifications == 3)
                    {
                        MoveToState(AgentState.OnCall);
                        callNotifications = 0;
                    }
                }
                else if ((e.Message as DialerMessage).Header.Name == "AGTAutoReleaseLine")
                    MoveToState(AgentState.Disconnected);
                else if ((e.Message as DialerMessage).Header.Name == "AGTJobTransLink")
                {
                    MoveToState(AgentState.LoggedOn);
                    Attach(new Job
                                {
                                    Name = (e.Message as DialerMessage).Segments[2],
                                    Status = JobStatus.Active,
                                    Type = CurrentJob.Type
                                });
                }
            }
            OnNewMessage(e);
        }

        private void OnNewMessage(MessageArgs e)
        {
            if (NewMessage != null)
                NewMessage(this, e);
        }

        private void OnCallDisconnected()
        {
            _disconnected = true;
            if (CallDisconnected != null)
                CallDisconnected(this, EventArgs.Empty);
        }

        public void CallBack(DateTime date)
        {
            _connection.SendCommand(new Commands.SetCallback(date.ToString("yyyy/MM/dd"), date.ToString("hh:mmt").ToLower(), "1", null, null));
            OnNewMessage(new MessageArgs { Agent = this, IsTerminating = false, Message = new GeneralMessage { IsError = false, Message = string.Format("Set callback date to {0:f}", date) } });
        }

        public Common.Call CurrentCall { get; private set; }

        public event EventHandler<MessageArgs> NewMessage;
        public event EventHandler<AgentStateChangedArgs> AgentStateChanged;
        public event EventHandler<AgentStateArgs> AgentStateChanging;
        public event EventHandler CallDisconnected;

        public void Attach(Common.Job jobToAttachTo)
        {
            CurrentCall = new Call();
            AgentState newState = AgentState.Available;
            AgentState oldState = State;

            OnStateChanging(oldState, newState);

            CurrentJob = jobToAttachTo;
            //Set the job class
            _connection.SendCommand(new Commands.SetWorkClass(((char)CurrentJob.Type).ToString()));
            //Attach to the job
            _connection.SendCommand(new Commands.AttachJob(CurrentJob.Name));
            //Build the screens for the current job
            BuildScreens();
            //
            _connection.SendCommand(new Commands.SetNotifyKeyField(((char)CurrentJob.Type).ToString(), "ACCTNUM"));
            _connection.SendCommand(new Commands.AvailWork());
            _connection.SendCommand(new Commands.ReadyNextItem());

            State = newState;
            OnStateChanged(oldState, newState, true);
        }

        public void Finish(Common.CompletionCode completionCode)
        {
            Finish(completionCode.Code);
            OnNewMessage(new MessageArgs { Agent = this, IsTerminating = false, Message = new GeneralMessage { IsError = false, Message = string.Format("Disposition set to \"{0}\"", completionCode.Description) } });
        }

        public void Finish(int completionCode)
        {
            AgentState newState = AgentState.Updating;
            AgentState oldState = State;

            OnStateChanging(oldState, newState);

            if (!_disconnected)
            {
                _connection.SendCommand(new Commands.HangupCall());
                OnCallDisconnected();
            }
            foreach (Screen screen in CurrentCall.Screens)
            {
                foreach (DataField field in screen.Fields.OfType<DataField>().Where(df => df.IsDirty))
                {
                    _connection.SendCommand(new Commands.UpdateField(((char)CurrentJob.Type).ToString(), field.Name, field.Value));
                    field.IsDirty = false;
                }
            }
            _connection.SendCommand(new Commands.FinishedItem(completionCode.ToString()));

            State = newState;
            OnStateChanged(oldState, newState, true);
        }

        public void MoveToState(AgentState agentState)
        {
            MoveToState(agentState, false);
        }

        public void MoveToState(AgentState agentState, bool force)
        {
            if (_waitingState.HasValue)
            {
                AgentState state = _waitingState.Value;
                _waitingState = null;
                MoveToState(state, force);
            }
            else
            {
                bool result = false;
                AgentState oldState = State;
                OnStateChanging(oldState, agentState);

                if (agentState < oldState)
                    result = MoveDownStates(agentState, force);
                else if (agentState > oldState)
                    result = MoveUpStates(agentState);
                else
                    result = true;

                if (result)
                    State = agentState;

                OnStateChanged(oldState, agentState, result);
            }
        }

        #endregion

        #region Helpers

        private bool MoveUpStates(AgentState newState)
        {
            bool result = true;
            for (int i = (int)State; i <= (int)newState; i++)
            {
                if (i == (int)AgentState.Available)
                {
                    if (newState != AgentState.OnCall)
                    {
                        _connection.SendCommand(new Commands.SetNotifyKeyField(((char)CurrentJob.Type).ToString(), "ACCTNUM"));
                        _connection.SendCommand(new Commands.AvailWork());
                        _connection.SendCommand(new Commands.ReadyNextItem());
                    }
                    result = true;
                }
                else if (i == (int)AgentState.LoggedOn)
                {
                    //Initializes our command with the username and password
                    Commands.Logon logon = new Commands.Logon(Name, Password);
                    //Send the command to log on
                    _connection.SendCommand(logon);
                    //If we were able to log on...
                    if (!logon.Response.IsError)
                    {
                        //Initialize the headset to keep track of
                        //Initialize our command with the extension
                        Commands.ReserveHeadset reserve = new Commands.ReserveHeadset(Headset.ID);
                        //Send the command to reserve the headset
                        _connection.SendCommand(reserve);
                        //If we successfully reserved the headset
                        if (!reserve.Response.IsError)
                        {
                            //Initialize our command with no arguments since we have already reserved a headset
                            Commands.ConnHeadset connect = new Commands.ConnHeadset();
                            //Send the command to connect the headset
                            //Connecting the headset involves the dialer calling the agent's headset, and the agent answering
                            _connection.SendCommand(connect);

                            //If the response is available and it's an error
                            if (connect.Response == null || connect.Response.IsError)
                            {
                                //Start the log off process
                                _connection.SendCommand(new Commands.Logoff());
                                result = false;
                            }
                            else
                                result = true;
                        }
                        //If we did not succeed
                        else
                        {
                            result = false;
                            //And log off
                            _connection.SendCommand(new Commands.Logoff());
                        }
                    }
                    //If we did not succeed
                    else
                    {
                        result = false;
                        //And log off
                        _connection.SendCommand(new Commands.Logoff());
                    }
                }
            }
            return result;
        }

        private bool MoveDownStates(AgentState newState, bool force)
        {
            for (int i = (int)State; i > (int)newState; i--)
            {
                if (i == (int)AgentState.OnCall)
                {
                    if (State == AgentState.OnCall)
                    {
                        if (force)
                            Finish(49);
                        else
                        {
                            _waitingState = newState;
                            StateMoveAttribute[] attributes = (StateMoveAttribute[])_waitingState.GetType().GetField(_waitingState.ToString()).GetCustomAttributes(typeof(StateMoveAttribute), false);
                            OnNewMessage(new MessageArgs 
                            { 
                                Agent = this, 
                                IsTerminating = false, 
                                Message = new GeneralMessage 
                                { 
                                    IsError = false, 
                                    Message = string.Format("You will {0} after finishing the current record.", attributes[0].Message)
                                } 
                            });
                            return false;
                        }
                    }
                }
                else if (i == (int)AgentState.Updating && newState >= AgentState.Available)
                {
                    _connection.SendCommand(new Commands.ReadyNextItem());
                    _disconnected = false;
                }
                else if (i == (int)AgentState.Available)
                {
                    _connection.SendCommand(new Commands.NoFurtherWork());
                }
                else if (i == (int)AgentState.OnBreak)
                {
                    _connection.SendCommand(new Commands.DetachJob());
                }
                else if (i == (int)AgentState.LoggedOn)
                {
                    _connection.SendCommand(new Commands.Logoff());
                }
                else if (i == (int)AgentState.LoggedOff)
                {
                    //Never will reach this since it is the lowest number and never reach our destination state
                }
            }
            return true;
        }

        private void OnStateChanging(AgentState oldState, AgentState newState)
        {
            if (AgentStateChanging != null)
                AgentStateChanging(this, new AgentStateArgs { OldState = oldState, NewState = newState });
        }

        private void OnStateChanged(AgentState oldState, AgentState newState, bool completed)
        {
            if (AgentStateChanged != null)
                AgentStateChanged(this, new AgentStateChangedArgs { OldState = oldState, NewState = newState, Completed = completed });
        }

        private void AddToCallFields(IEnumerable<Common.Field> fields)
        {
            foreach (Common.DataField field in fields.OfType<Common.DataField>())
                AddToCallFields(field);
        }

        private void AddToCallFields(Common.DataField field)
        {
            if (field != null)
            {
                Commands.SetDataField setDataField = new Commands.SetDataField(((char)CurrentJob.Type).ToString(), field.Name);
                _connection.SendCommand(setDataField);
            }
        }

        private void BuildScreens()
        {
            CurrentJob.Screens = new List<Screen>();
            Commands.ListScreens listScreens = null;
            listScreens = new Commands.ListScreens(((char)CurrentJob.Type).ToString());
            _connection.SendCommand(listScreens);
            ListScreens screens = listScreens.Response as ListScreens;
            if (screens != null && screens.Screens != null)
            {
                foreach (string screenName in screens.Screens)
                {
                    Commands.GetScreen getScreen = new Commands.GetScreen(screenName);
                    _connection.SendCommand(getScreen);
                    GetScreen screen = getScreen.Response as GetScreen;
                    CurrentJob.Screens.Add(screen.Screen);
                    AddToCallFields(screen.Screen.Fields);
                }
            }
        }

        #endregion

        #region IDialerInfo Members

        public List<CompletionCode> Keys
        {
            get
            {
                Commands.ListKeys listKeys = new Commands.ListKeys();
                _connection.SendCommand(listKeys);
                return (listKeys.Response as ListKeys).Codes;
            }
        }

        public List<Job> Jobs
        {
            get
            {
                //Initialize our command with our CallType
                Commands.ListJobs listJobs = new Commands.ListJobs("A");
                //Send the command
                _connection.SendCommand(listJobs);
                //Wait for the command to finish
                ListJobs result = listJobs.Response as ListJobs;
                //Return the results
                return result.Jobs;
            }
        }

        #endregion

    }
}
