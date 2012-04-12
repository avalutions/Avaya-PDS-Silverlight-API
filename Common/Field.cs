using System;

namespace Dialer.Communication.Common
{
    public abstract class Field
    {
        public String Name { get; set; }
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Int32 Width { get; set; }
    }
}
