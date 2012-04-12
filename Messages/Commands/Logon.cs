/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       Logon.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class Logon : Command
    {
        public override string PendingText { get { return "Attempting to log on"; } }

        public String AgentName { get; set; }
        public String Password { get; set; }
        public Logon(string AgentName, string Password)
            : base("AGTLogon")
        {
            Arguments.Enqueue(AgentName); Arguments.Enqueue(Password); Header.NumberOfSegments = 2;
        }
    }
}
