/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       MoFlashBlind.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class MoFlashBlind : Command
    {
				public String JobName { get; set; }
			    public MoFlashBlind(string JobName) : base("AGTMoFlashBlind")
	    {
        Arguments.Enqueue(JobName);        	Header.NumberOfSegments = 1;
        }
	}
}
