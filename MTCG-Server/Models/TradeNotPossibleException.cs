using System.Runtime.Serialization;

namespace MTCGServer.Models
{
    [Serializable]
    public class TradeNotPossibleException : Exception
    {
        public TradeNotPossibleException()
        {
        }

        public TradeNotPossibleException(string? message) : base(message)
        {
        }

        public TradeNotPossibleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TradeNotPossibleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}