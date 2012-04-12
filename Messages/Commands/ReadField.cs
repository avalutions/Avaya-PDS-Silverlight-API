/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ReadField.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class ReadField : Command
    {
				public String ListType { get; set; }
				public String FieldName { get; set; }
			    public ReadField(string ListType, string FieldName) : base("AGTReadField")
	    {
        Arguments.Enqueue(ListType);Arguments.Enqueue(FieldName);        	Header.NumberOfSegments = 2;
        }
	}
}
