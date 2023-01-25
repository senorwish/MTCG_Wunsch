using System.Runtime.Serialization;

namespace MTCGServer.DAL.Trading
{
    [Serializable]
    public class TradeIdNotFoundException : Exception
    {
        public TradeIdNotFoundException()
        {
        }

        public TradeIdNotFoundException(string? message) : base(message)
        {
        }

        public TradeIdNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TradeIdNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}