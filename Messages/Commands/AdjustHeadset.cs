/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       AdjustHeadset.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class AdjustHeadset : Command
    {
				public String EarMouth { get; set; }
				public String Volume { get; set; }
			    public AdjustHeadset(string EarMouth, string Volume) : base("AGTAdjustHeadset")
	    {
        Arguments.Enqueue(EarMouth);Arguments.Enqueue(Volume);        	Header.NumberOfSegments = 2;
        }
	}
}
