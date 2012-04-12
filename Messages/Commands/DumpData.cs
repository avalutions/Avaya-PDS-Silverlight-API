/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       DumpData.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class DumpData : Command
    {
				public String FileName { get; set; }
			    public DumpData(string FileName) : base("AGTDumpData")
	    {
        Arguments.Enqueue(FileName);        	Header.NumberOfSegments = 1;
        }
	}
}
