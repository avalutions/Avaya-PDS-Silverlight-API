/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ListJobs.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class ListJobs : Command
    {
				public String JobType { get; set; }
			    public ListJobs(string JobType) : base("AGTListJobs")
	    {
        Arguments.Enqueue(JobType);        	Header.NumberOfSegments = 1;
        }
	}
}
