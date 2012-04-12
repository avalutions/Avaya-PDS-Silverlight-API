/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ListCallbackFmt.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class ListCallbackFmt : Command
    {
			    public ListCallbackFmt() : base("AGTListCallbackFmt")
	    {
                	Header.NumberOfSegments = 0;
        }
	}
}
