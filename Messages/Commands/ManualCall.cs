/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ManualCall.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class ManualCall : Command
    {
				public String PhoneNumber { get; set; }
			    public ManualCall(string PhoneNumber) : base("AGTManualCall")
	    {
        Arguments.Enqueue(PhoneNumber);        	Header.NumberOfSegments = 1;
        }
	}
}
