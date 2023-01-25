using System.Runtime.Serialization;

namespace MTCGServer.DAL
{
    [Serializable]
    public class DataUpdateException : Exception
    {
        public DataUpdateException()
        {
        }

        public DataUpdateException(string? message) : base(message)
        {
        }

        public DataUpdateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DataUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}