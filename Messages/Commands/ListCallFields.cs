/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ListCallFields.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class ListCallFields : Command
    {
				public String ListName { get; set; }
			    public ListCallFields(string ListName) : base("AGTListCallFields")
	    {
        Arguments.Enqueue(ListName);        	Header.NumberOfSegments = 1;
        }
	}
}
