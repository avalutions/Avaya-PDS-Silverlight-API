/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       SetPassword.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class SetPassword : Command
    {
				public String UID { get; set; }
				public String Old { get; set; }
				public String New { get; set; }
			    public SetPassword(string UID, string Old, string New) : base("AGTSetPassword")
	    {
        Arguments.Enqueue(UID);Arguments.Enqueue(Old);Arguments.Enqueue(New);        	Header.NumberOfSegments = 3;
        }
	}
}
