/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       FreeHeadset.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class FreeHeadset : Command
    {
			    public FreeHeadset() : base("AGTFreeHeadset")
	    {
                	Header.NumberOfSegments = 0;
        }
	}
}
