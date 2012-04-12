/*  ----------------------------------------------------------------------------
 *  Copyright (c) 2009-2010, Avalutions, LLC
 *  ----------------------------------------------------------------------------
 *  DialerCommandCenter
 *  ----------------------------------------------------------------------------
 *  File:       UpdateField.cs
 *  Author:     Benjamin D Thompson
 *  ----------------------------------------------------------------------------
 */

using System;

namespace Dialer.Communication.Messages.Commands
{
    public class UpdateField : Command
    {
        public override string FinishedText
        {
            get
            {
                return string.Format("Updated \"{0}\" to \"{1}\"", FieldName, NewValue);
            }
        }
        public String ListType { get; set; }
        public String FieldName { get; set; }
        public String NewValue { get; set; }
        public UpdateField(string ListType, string FieldName, string NewValue)
            : base("AGTUpdateField")
        {
            this.ListType = ListType;
            this.FieldName = FieldName;
            this.NewValue = NewValue;
            Arguments.Enqueue(ListType); Arguments.Enqueue(FieldName); Arguments.Enqueue(NewValue); Header.NumberOfSegments = 3;
        }
    }
}
