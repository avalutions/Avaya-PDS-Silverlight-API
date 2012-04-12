/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       GetScreen.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialer.Communication.Messages.Commands
{
    public class GetScreen : Command
    {
        public GetScreen(string screen)
            : base("AGTGetScreen")
        {
            Arguments.Enqueue(screen);
            Header.NumberOfSegments = 1;
        }
    }
}
