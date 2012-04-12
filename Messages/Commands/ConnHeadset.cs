/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ConnHeadset.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class ConnHeadset : Command
    {
        public override string PendingText { get { return "Connecting headset"; } }

        public override string FinishedText
        {
            get
            {
                return "Headset connected.  Please attach to a job.";
            }
        }

        public ConnHeadset()
            : base("AGTConnHeadset")
        {
            Header.NumberOfSegments = 0;
        }
    }
}
