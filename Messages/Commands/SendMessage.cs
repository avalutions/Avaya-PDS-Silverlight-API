/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       SendMessage.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class SendMessage : Command
    {
				public String Message { get; set; }
			    public SendMessage(string Message) : base("AGTSendMessage")
	    {
        Arguments.Enqueue(Message);        	Header.NumberOfSegments = 1;
        }
	}
}
