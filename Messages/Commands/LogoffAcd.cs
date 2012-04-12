/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       LogoffAcd.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class LogoffAcd : Command
    {
			    public LogoffAcd() : base("AGTLogoffAcd")
	    {
                	Header.NumberOfSegments = 0;
        }
	}
}
