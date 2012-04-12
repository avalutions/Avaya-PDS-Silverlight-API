/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       DetachJob.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class DetachJob : Command
    {
        public override string PendingText { get { return "Attempting to detach from job"; } }

        public DetachJob()
            : base("AGTDetachJob")
        {
            Header.NumberOfSegments = 0;
        }
    }
}
