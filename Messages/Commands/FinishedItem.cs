/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       FinishedItem.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class FinishedItem : Command
    {
				public String CompCode { get; set; }
			    public FinishedItem(string CompCode) : base("AGTFinishedItem")
	    {
        Arguments.Enqueue(CompCode);        	Header.NumberOfSegments = 1;
        }
	}
}
