/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       Command.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dialer.Communication.Common;
using System.Threading;
using Dialer.Communication.Messages;

namespace Dialer.Communication.Messages.Commands
{
    public abstract class Command : DialerMessage
    {
        public virtual String PendingText { get; set; }
        public virtual String FinishedText { get; set; }
        public Queue<String> Arguments { get; protected set; }
        public ManualResetEvent Finished { get; private set; }
        public DialerMessage Response { get; set; }
        public MessageHeader Header { get; protected set; }

        protected Command(String Name)
        {
            Arguments = new Queue<string>();
            Header = new MessageHeader
            {
                Name = Name,
                Type = MessageType.Command,
                Client = "Client",
                ProcID = "123456",
                InvokeID = "Invo",
                NumberOfSegments = Arguments.Count
            };
            Finished = new ManualResetEvent(false);
        }

        public override string ToString()
        {
            String[] arguments = new string[Arguments.Count];
            Arguments.CopyTo(arguments, 0);
            StringBuilder builder = new StringBuilder();
            builder.Append(Header.ToString());
            if (Arguments.Count > 0)
            {
                for(int i = 0; i < arguments.Length; i++)
                {
                    builder.Append((char)MessageSeperator.Delimiter);
                    builder.Append(arguments[i]);
                }
            }
            builder.Append((char)MessageSeperator.Terminator);

            return builder.ToString();
        }

        internal override void Parse(List<string> segments)
        {
        }
    }
}
