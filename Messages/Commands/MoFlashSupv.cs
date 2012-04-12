/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       MoFlashSupv.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class MoFlashSupv : Command
    {
				public String JobName { get; set; }
			    public MoFlashSupv(string JobName) : base("AGTMoFlashSupv")
	    {
        Arguments.Enqueue(JobName);        	Header.NumberOfSegments = 1;
        }
	}
}
