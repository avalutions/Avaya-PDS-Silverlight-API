/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       LogonAcd.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class LogonAcd : Command
    {
				public String Extension { get; set; }
				public String PBXID { get; set; }
			    public LogonAcd(string Extension, string PBXID) : base("AGTLogonAcd")
	    {
        Arguments.Enqueue(Extension);Arguments.Enqueue(PBXID);        	Header.NumberOfSegments = 2;
        }
	}
}
