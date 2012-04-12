/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ReadyNextItem.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class ReadyNextItem : Command
    {
        public override string FinishedText { get { return "Waiting for a call."; } }

        public ReadyNextItem()
            : base("AGTReadyNextItem")
        {
            Header.NumberOfSegments = 0;
        }
    }
}
