/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       ListScreens.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dialer.Communication.Messages.Commands
{
    public class ListScreens : Command
    {
        public ListScreens(String jobType)
            : base("AGTListScreens")
        {
            Arguments.Enqueue(jobType);
            Header.NumberOfSegments = 1;
        }
    }
}
