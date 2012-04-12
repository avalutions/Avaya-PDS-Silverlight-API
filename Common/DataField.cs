using System;
using System.Collections.Generic;

namespace Dialer.Communication.Common
{
    public class DataField : Field
    {
        private string _value;
        public String Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                IsDirty = true;
            }
        }
        public Int32 Length { get; set; }
        public String Type { get; set; }
        public List<String> PossibleValues { get; set; }
        public bool IsDirty { get; internal set; }
    }
}
