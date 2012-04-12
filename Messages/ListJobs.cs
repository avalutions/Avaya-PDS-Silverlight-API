using System.Collections.Generic;
using Dialer.Communication.Common;

namespace Dialer.Communication.Messages
{
    public class ListJobs : DialerMessage
    {
        public List<Job> Jobs { get; set; }

        #region DialerMessage Members

        internal override void Parse(List<string> segments)
        {
            Jobs = new List<Job>();
            foreach (string s in segments)
            {
                string[] parts = s.Split(',');
                Jobs.Add(new Job
                {
                    Type = (CallType)parts[0][0],
                    Name = parts[1],
                    Status = (JobStatus)parts[2][0]
                });
            }
        }

        #endregion
    }
}