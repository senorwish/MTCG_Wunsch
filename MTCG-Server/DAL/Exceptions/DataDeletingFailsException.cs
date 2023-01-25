using System.Runtime.Serialization;

namespace MTCGServer.DAL.Trading
{
    [Serializable]
    public class DataDeletingFailsException : Exception
    {
        public DataDeletingFailsException()
        {
        }

        public DataDeletingFailsException(string? message) : base(message)
        {
        }

        public DataDeletingFailsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DataDeletingFailsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}