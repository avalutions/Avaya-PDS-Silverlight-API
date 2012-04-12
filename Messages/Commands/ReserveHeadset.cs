/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ReserveHeadset.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class ReserveHeadset : Command
    {
        public String HeadsetID { get; set; }
        public ReserveHeadset(string HeadsetID)
            : base("AGTReserveHeadset")
        {
            Arguments.Enqueue(HeadsetID); Header.NumberOfSegments = 1;
        }
    }
}
