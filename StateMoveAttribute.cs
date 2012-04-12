using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Dialer.Communication
{
    public class StateMoveAttribute : Attribute
    {
        public string Message { get; set; }
        public StateMoveAttribute(string Message)
        {
            this.Message = Message;
        }
    }
}
