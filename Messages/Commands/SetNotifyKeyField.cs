/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       SetNotifyKeyField.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class SetNotifyKeyField : Command
    {
				public String ListType { get; set; }
				public String FieldName { get; set; }
			    public SetNotifyKeyField(string ListType, string FieldName) : base("AGTSetNotifyKeyField")
	    {
        Arguments.Enqueue(ListType);Arguments.Enqueue(FieldName);        	Header.NumberOfSegments = 2;
        }
	}
}
