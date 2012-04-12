/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ListDataFields.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class ListDataFields : Command
    {
				public String ListType { get; set; }
			    public ListDataFields(string ListType) : base("AGTListDataFields")
	    {
        Arguments.Enqueue(ListType);        	Header.NumberOfSegments = 1;
        }
	}
}
