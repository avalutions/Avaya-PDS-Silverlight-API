
namespace Dialer.Communication.Messages
{
    public sealed class GeneralMessage : IMessage
    {
        #region IMessage Members

        public bool IsError { get; set; }
        public string Message { get; set; }

        #endregion
    }
}
