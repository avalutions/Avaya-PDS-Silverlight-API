using System;
using System.Collections.Generic;
using Dialer.Communication.Common;

namespace Dialer.Communication.Messages
{
    public class ListKeys : DialerMessage
    {
        public List<CompletionCode> Codes { get; set; }
        
        #region IMessage Members

        internal override void Parse(List<string> segments)
        {
            Codes = new List<CompletionCode>();
            foreach (string s in segments)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    string[] code = s.Split(',');
                    if (code.Length == 3 && !string.IsNullOrWhiteSpace(code[0]) && !string.IsNullOrWhiteSpace(code[1]))
                    {
                        Codes.Add(new CompletionCode
                        {
                            Code = Convert.ToInt32(code[0]),
                            Description = code[1],
                            Script = code[2]
                        });
                    }
                }
            }
        }

        #endregion
    }
}
