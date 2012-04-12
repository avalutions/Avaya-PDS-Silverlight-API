/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       GetHeadsetVol.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class GetHeadsetVol : Command
    {
			    public GetHeadsetVol() : base("AGTGetHeadsetVol")
	    {
                	Header.NumberOfSegments = 0;
        }
	}
}
