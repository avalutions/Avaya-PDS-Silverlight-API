/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       NoFurtherWork.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class NoFurtherWork : Command
    {
        public override string PendingText { get { return "Attempting to stop calls."; } }
        public override string FinishedText { get { return "No longer accepting calls."; } }
        public NoFurtherWork()
            : base("AGTNoFurtherWork")
        {
            Header.NumberOfSegments = 0;
        }
    }
}
