/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       Logoff.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class Logoff : Command
    {
        public override string PendingText { get { return "Attempting to log off"; } }
        public override string FinishedText
        {
            get
            {
                return "Successfully logged off";
            }
        }

        public Logoff()
            : base("AGTLogoff")
        {
            Header.NumberOfSegments = 0;
        }
    }
}
