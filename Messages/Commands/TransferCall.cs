/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       TransferCall.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class TransferCall : Command
    {
				public String PhoneNumber { get; set; }
			    public TransferCall(string PhoneNumber) : base("AGTTransferCall")
	    {
        Arguments.Enqueue(PhoneNumber);        	Header.NumberOfSegments = 1;
        }
	}
}
