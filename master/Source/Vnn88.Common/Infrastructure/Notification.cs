namespace Vnn88.Common.Infrastructure
{
    /// <summary>
    /// Enum represent for type of messsage
    /// </summary>
    public enum NotifyType
    {
        Error,
        Info,
        Success,
        Warning
    }

    /// <summary>
    /// Class represnt for a message
    /// </summary>
    public class NotifyMessage
    {
        public string Message { get; set; }
        public NotifyType NotifyType { get; set; }
    }     
}
