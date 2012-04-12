/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       AttachJob.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class AttachJob : Command
    {
        public override string PendingText
        {
            get { return string.Format("Attaching to {0}", JobName); }
        }
        public String JobName { get; set; }
        public AttachJob(string JobName)
            : base("AGTAttachJob")
        {
            Arguments.Enqueue(JobName); Header.NumberOfSegments = 1;
        }
    }
}
