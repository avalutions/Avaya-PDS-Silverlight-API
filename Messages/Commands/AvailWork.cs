/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       AvailWork.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class AvailWork : Command
    {
        public override string FinishedText { get { return "Preparing to receive calls"; } }

        public AvailWork()
            : base("AGTAvailWork")
        {
            Header.NumberOfSegments = 0;
        }
    }
}
