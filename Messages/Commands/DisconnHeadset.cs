/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       DisconnHeadset.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class DisconnHeadset : Command
    {
			    public DisconnHeadset() : base("AGTDisconnHeadset")
	    {
                	Header.NumberOfSegments = 0;
        }
	}
}
