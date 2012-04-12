/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       SetUnit.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class SetUnit : Command
    {
				public String UnitID { get; set; }
			    public SetUnit(string UnitID) : base("AGTSetUnit")
	    {
        Arguments.Enqueue(UnitID);        	Header.NumberOfSegments = 1;
        }
	}
}
