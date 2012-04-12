using System;
using System.Collections.Generic;

namespace Dialer.Communication.Common
{
    public class Job
    {
        public CallType Type { get; set; }
        public String Name { get; set; }
        public JobStatus Status { get; set; }
        public List<Screen> Screens { get; set; }
    }
}
