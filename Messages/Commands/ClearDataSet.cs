/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ClearDataSet.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class ClearDataSet : Command
    {
				public String ListType { get; set; }
			    public ClearDataSet(string ListType) : base("AGTClearDataSet")
	    {
        Arguments.Enqueue(ListType);        	Header.NumberOfSegments = 1;
        }
	}
}
