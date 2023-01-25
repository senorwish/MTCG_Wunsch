using System.Runtime.Serialization;

namespace MTCGServer.BLL.Exceptions
{
    [Serializable]
    public class TooLessMoneyException : Exception
    {
        public TooLessMoneyException()
        {
        }
        public TooLessMoneyException(string? message) : base(message)
        {
        }
        public TooLessMoneyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
        protected TooLessMoneyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}