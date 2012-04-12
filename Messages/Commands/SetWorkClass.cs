/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       SetWorkClass.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class SetWorkClass : Command
    {
				public String ClassID { get; set; }
			    public SetWorkClass(string ClassID) : base("AGTSetWorkClass")
	    {
        Arguments.Enqueue(ClassID);        	Header.NumberOfSegments = 1;
        }
	}
}
