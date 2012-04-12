using System;
using System.Collections.Generic;

namespace Dialer.Communication.Common
{
    public class Call
    {
        private List<Screen> _screens = new List<Screen>();
        public List<Screen> Screens { get { return _screens; } }
        public String AccountNumber { get; set; }
        public bool OnHold { get; set; }
    }
}
