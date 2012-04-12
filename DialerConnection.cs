using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dialer.Communication.Messages;
using System.IO;
using Dialer.Communication.Messages.Commands;
using System.Collections;
using System.Configuration;
using System.Xml.Linq;

namespace Dialer.Communication
{
    /// <summary>
    /// Class that utilizes the <typeparamref name="DialerSocket"/> to communicate with the dialer.
    /// When a message is received events are fired notifying subscribers of different states of the active user.
    /// </summary>
    public sealed class DialerConnection : IDialerConnection
    {
        internal event Action<string, string> NewCallData;
        internal event EventHandler CallReady;
        private XDocument _messagesDocument;
        private String _dialerAddress;

        #region IDialerConnection Members

        public string IPAddress { get; set; }
        public int Port { get; set; }
        public IAgent Agent { get; set; }

        /// <summary>
        /// Event to handle any new status (mainly the pipe for messages to bubble up).
        /// </summary>
        public event EventHandler<MessageArgs> NewMessage;

        #endregion

        private bool HandleError(DialerMessage response)
        {
            List<string> segments = new List<string>(response.Segments);
            string[] details = segments[1].Split(',');
            string[] data = new string[details.Length - 1];
            Array.Copy(details, 1, data, 0, data.Length);

            XElement message =
                (from m in _messagesDocument.Descendants("message")
                 where (string)m.Attribute("code") == details[0]
                 select m).FirstOrDefault();

            if (message != null)
            {
                response.Message = message.Descendants("source").Where(d => (string)d.Attribute("name") == response.Header.Name).Select(v => v.Value).FirstOrDefault();
                if (string.IsNullOrEmpty(response.Message))
                    response.Message = message.Descendants("source").Where(d => (string)d.Attribute("name") == "default").Select(v => v.Value).FirstOrDefault();
            }

            return message != null;
        }

        public void Reset()
        {
            _socket = new DialerSocket(IPAddress, Port, NewMessageReceived);
            //_socket.Send(_dialerAddress + Environment.NewLine);
        }

        //Used to keep track of tracing (writing messages to a file)
        private bool _isTracing;
        //The stream writer to use for logging
        private StreamWriter _log;
        //The actual DialerSocket that will notify us when new messages are received and allow us to send messages to the dialer
        private DialerSocket _socket;
        //List of commands that have been sent and are waiting for a response
        private List<Command> _waitingCommands;

        /// <summary>
        /// Default constructor for opening a connection to the dialer
        /// </summary>
        /// <param name="agent">The <typeparamref name="DialerAgent"/> opening the connection</param>
        public DialerConnection(string dialerAddress, string ipAddress, int port)
        {
            IPAddress = ipAddress;
            Port = port;
            _messagesDocument = XDocument.Load("Messages.xml");
            _dialerAddress = dialerAddress;
            //Read in our custom configuration section
            //Configuration.DialerConnectionConfigurationSection config = (Configuration.DialerConnectionConfigurationSection)ConfigurationManager.GetSection("dialerConnectionGroup/dialerConnectionSection");

            //If the agent is configured to log messages, set up the file for tracing
            //if (config.Tracing.Enabled)
            //{
            //    //Keep track of whether we are tracing or not
            //    _isTracing = true;

            //    //Get the path to the folder messages will be stored in
            //    string path = config.Tracing.Path;
            //    //Build a full filename using the path provided and the current time
            //    string output = System.IO.Path.Combine(path, string.Format("{0}_{1:yyyyMMdd_hhmmss}.txt", Agent.Name, DateTime.Now));

            //    //Initialize our stream writer with the new path we built
            //    _log = new StreamWriter(output);
            //    //Make sure the stream pushes any new data we send to it right to the file
            //    _log.AutoFlush = true;
            //}
            //Initialize our socket with the server and port specificed in the custom configuration section.  Also pass in our delegate for capturing new messages.
            _socket = new DialerSocket(IPAddress, Port, NewMessageReceived);
            //_socket.Send(_dialerAddress + Environment.NewLine);

            //Initialize the empty list of commands.
            _waitingCommands = new List<Command>();
        }

        /// <summary>
        /// A notification was received from the dialer so handle which kind.
        /// </summary>
        /// <param name="response">The structured data received from the dialer</param>
        /// <returns>Whether the notification was handled or not</returns>
        private bool HandleNotification(DialerMessage response)
        {
            bool handled = true;

            //The agent hung up the headset without logging off
            if (response.Header.Name == "AGTHeadsetConnBroken")
            {
                //Bubble the message to the presentation layer
                OnNewMessage("NOTICE: Headset connection broke.  Logoff process beginning.", false);
                //Start the logoff process
                Agent.MoveToState(AgentState.LoggedOff, true);
            }
            //An information message was received from the supervisor.  This is not a system message delivery mechanism
            else if (response.Header.Name == "AGTReceiveMessage")
            {
                //Bubble the message to the presentation layer
                OnNewMessage("NOTICE: New message from supervisor to follow.", false);
                //Pull out the actual message received
                OnNewMessage(response.Segments.ToList()[1], false);
            }
            //The job we are currently attached to is ending.  Begin detach process
            else if (response.Header.Name == "AGTJobEnd")
            {
                //Bubble the message to the presentation layer
                OnNewMessage("NOTICE: The current job has ended.", false);
                //Start the detach process
                Agent.MoveToState(AgentState.LoggedOn, true);
            }
            //System error received.  This is usually detramental
            else if (response.Header.Name == "AGTSystemError")
            {
                //Pull out the message the dialer is sending
                string message = response.Segments.ToList()[1];

                //Bubble the message to the user
                OnNewMessage("NOTICE: System error reported. Message to follow.", false);
                OnNewMessage(message, true);

                //Check to see if it was a command that failed.
                Command pendingCommand = null;
                lock (((ICollection)_waitingCommands).SyncRoot)
                    pendingCommand = _waitingCommands.FirstOrDefault(c => message.Contains(c.Header.Name));
                //If so, end the command
                if (pendingCommand != null)
                    pendingCommand.Finished.Set();
            }
            else if (response.Header.Name == "AGTAutoReleaseLine")
            {
                OnNewMessage("The line has been disconnected.", false);
                Agent.MoveToState(AgentState.Disconnected);
            }
            //A new call was received in the format expected
            else if (response.Header.Name == "AGTCallNotify")
            {
                OnNewMessage(response);
                if (response.Segments[1] == "M00000")
                    OnNewMessage("NOTICE: Call received", false);
            }
            //We dont have a method for handling the notification so indicate that
            else
                handled = false;

            //Return if we handled the notification or not
            return handled;
        }

        /// <summary>
        /// Set all commands to finished.  Mainly used to clear the road before logging off, etc.
        /// </summary>
        private void ClearCommands()
        {
            lock (((ICollection)_waitingCommands).SyncRoot)
            {
                //Set all waiting commands to finished
                _waitingCommands.ForEach(cmd => cmd.Finished.Set());
                //Clear all of the messages from our queue since they are technically done
                _waitingCommands.Clear();
            }
        }

        /// <summary>
        /// Handles new messages received from the <typeparamref name="DialerSocket"/>
        /// </summary>
        /// <param name="message">The raw message received from the dialer through our <typeparamref name="DialerSocket"/></param>
        internal void NewMessageReceived(string message)
        {
            //If we are tracing, log the raw message to the file
            if (_isTracing && _log.BaseStream.CanWrite)
                _log.WriteLine(string.Format("{0:MM/dd/yyyy} {1,10} << {2}", DateTime.Now, Agent.Name, message));

            //Populate our response from the raw message
            DialerMessage response = DialerMessage.FromRaw(message);

            bool handled = HandleError(response);

            //If we were able to parse the response and we have enough data to pass around, process the response
            if (response != null && response.Segments.Count >= 2)
            {
                //If it's a notification, handle the notification
                if (response.Header.Type == Messages.MessageType.Notification)
                    HandleNotification(response);
                //If it's a response to a command sent, handle it
                else if (response.Header.Type == Messages.MessageType.Response)
                {
                    Command command = null;
                    lock (((ICollection)_waitingCommands).SyncRoot)
                    {
                        //If we have commands waiting, and one of them is the command we received a response for...
                        if (_waitingCommands.Count > 0 && _waitingCommands.Count(h => h.Header.Name == response.Header.Name) > 0)
                        {
                            //Set our local variable with the found command
                            command = _waitingCommands.FirstOrDefault(h => h.Header.Name == response.Header.Name);
                            //Remove it from the pending queue since we have received a response for it
                            _waitingCommands.Remove(command);
                        }
                    }

                    //If we have found a command to finish...
                    if (command != null)
                    {
                        //If we have nothing set for the response (usually a data message if its a command to do so
                        if (command.Response == null)
                        {
                            DialerMessage newResponse = null;
                            if (_socket._messageQueue != null)
                            {
                                //Check to see if we missed a data message (since, technically, we could receive a response before our data because we're using asychronous invocation
                                List<DialerMessage> missedResponses = new List<DialerMessage>();
                                _socket._messageQueue.ToList().Where(m => m.Contains(command.Header.Name)).ToList().ForEach(r => missedResponses.Add(DialerMessage.FromRaw(r)));
                                //If there is a data message found, set it as our response
                                newResponse = missedResponses.FirstOrDefault(r => r.Header.Type == Messages.MessageType.Data);
                            }

                            //If we didn't find a data message to set as our response....
                            if (newResponse == null)
                                //Just set it to our response message
                                command.Response = response;
                            //If we did find a data message...
                            else
                                //Set it as our response5
                                command.Response = newResponse;
                        }

                        //If the response is an error and it's been handled (populated our message), kick off a new status
                        if (response.IsError && handled)
                            OnNewMessage(response.Message, true);

                        //If the command has overridden the FinishedText, send that as a message
                        if (!string.IsNullOrEmpty(command.FinishedText))
                            OnNewMessage(command.FinishedText, false);

                        //Inidicate our command is finished
                        command.Finished.Set();
                    }
                }
                //If we are dealing with a data message
                else if (response.Header.Type == Messages.MessageType.Data)
                {
                    //Find the command it belongs to
                    Command command = null;
                    lock (((ICollection)_waitingCommands).SyncRoot)
                        command = _waitingCommands.FirstOrDefault(h => h.Header.Name == response.Header.Name);

                    //And set the response to the data message
                    if (command != null)
                        command.Response = response;
                }
                //If we are dealing with a pending message
                else if (response.Header.Type == Messages.MessageType.Pending)
                {
                    //Find the command it belongs to
                    Command command = null;
                    lock (((ICollection)_waitingCommands).SyncRoot)
                        command = _waitingCommands.FirstOrDefault(h => h.Header.Name == response.Header.Name);

                    //And if the command has overriden the PendingText, kick off a new status
                    if (command != null && !string.IsNullOrEmpty(command.PendingText))
                        OnNewMessage(command.PendingText, false);
                }
                //Otherwise, if it's not been handled, and we can determine it's an error
                else if (response.IsError)
                    //Kick off a new status with the raw message
                    OnNewMessage(response);
            }
        }

        /// <summary>
        /// Wrapper to invoke the NewStatus event
        /// </summary>
        /// <param name="args">The message args to bubble</param>
        private void OnNewMessage(string message, bool isError)
        {
            OnNewMessage(new GeneralMessage { Message = message, IsError = isError }, isError);
        }

        private void OnNewMessage(IMessage message)
        {
            OnNewMessage(message, false);
        }

        private void OnNewMessage(IMessage message, bool isTerminating)
        {
            if (_isTracing && _log.BaseStream.CanWrite)
                _log.WriteLine(string.Format("{0:MM/dd/yyyy} {1,10} {2} {3}", DateTime.Now, Agent.Name, message is DialerMessage ? "<<" : "<-", message.Message));

            if (NewMessage != null)
                NewMessage(this, new MessageArgs { Agent = Agent, Message = message, IsTerminating = isTerminating });
        }

        /// <summary>
        /// Method used to send a command to the dialer
        /// </summary>
        /// <param name="command">The command to send to the dialer SYCHRONOUSLY</param>
        internal void SendCommand(Command command)
        {
            //Add the command to our list of pending commands
            lock (((ICollection)_waitingCommands).SyncRoot)
                _waitingCommands.Add(command);
            //Get the raw command text
            String commandText = command.ToString();
            //Send the raw command text to the socket
            _socket.Send(commandText);

            //If we are tracing, log the raw command text to the file
            if (_isTracing && _log.BaseStream != null && _log.BaseStream.CanWrite)
                _log.WriteLine(string.Format("{0:MM/dd/yyyy} {1,10} >> {2}", DateTime.Now, Agent.Name, commandText));

            command.Finished.WaitOne();
        }

        internal void SendCommand(Command command, Action<Command> responseCallback)
        {
            Action<Command> caller = new Action<Command>(SendCommand);
            caller.BeginInvoke(command, new AsyncCallback(ar => responseCallback(ar.AsyncState as Command)), command);
        }

        #region IDisposable

        /// <summary> 
        /// Finialize 
        /// </summary>
        ~DialerConnection()
        {
            if (!this._disposed)
                Dispose();
        }

        private bool _disposed;

        /// <summary>
        /// Called by the Garbage Collector (GC) to dispose of any objects needing to be released.
        /// </summary>
        public void Dispose()
        {
            try
            {
                // Flag that dispose has been called
                this._disposed = true;
                // Disconnect the client from the server
                _socket.Dispose();

                _log.Flush();
                _log.Close();
            }
            catch
            {
            }
        }

        #endregion
    }
}
