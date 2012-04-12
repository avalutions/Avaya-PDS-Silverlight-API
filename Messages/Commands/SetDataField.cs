/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       SetDataField.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class SetDataField : Command
    {
				public String ListType { get; set; }
				public String FieldName { get; set; }
			    public SetDataField(string ListType, string FieldName) : base("AGTSetDataField")
	    {
        Arguments.Enqueue(ListType);Arguments.Enqueue(FieldName);        	Header.NumberOfSegments = 2;
        }
	}
}
