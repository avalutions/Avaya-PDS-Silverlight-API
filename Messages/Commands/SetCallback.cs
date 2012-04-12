/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       SetCallback.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class SetCallback : Command
    {
        public String Date { get; set; }
        public String Time { get; set; }
        public String PhoneIndex { get; set; }
        public String RecallName { get; set; }
        public String RecallNumber { get; set; }
        public SetCallback(string Date, string Time, string PhoneIndex, string RecallName, string RecallNumber)
            : base("AGTSetCallback")
        {
            Arguments.Enqueue(Date); 
            Arguments.Enqueue(Time); 
            Arguments.Enqueue(PhoneIndex); 
            if(!string.IsNullOrEmpty(RecallName))
                Arguments.Enqueue(RecallName);
            if (!string.IsNullOrEmpty(RecallNumber))
                Arguments.Enqueue(RecallNumber); 
            Header.NumberOfSegments = Arguments.Count;
        }
    }
}
