/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       HookflashLine.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class HookflashLine : Command
    {
				public String PhoneNumber { get; set; }
			    public HookflashLine(string PhoneNumber) : base("AGTHookflashLine")
	    {
        Arguments.Enqueue(PhoneNumber);        	Header.NumberOfSegments = 1;
        }
	}
}
