/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       DialDigit.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class DialDigit : Command
    {
				public String Digit { get; set; }
			    public DialDigit(string Digit) : base("AGTDialDigit")
	    {
        Arguments.Enqueue(Digit);        	Header.NumberOfSegments = 1;
        }
	}
}
