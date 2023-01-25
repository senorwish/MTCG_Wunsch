using System.Runtime.Serialization;

namespace MTCGServer.DAL.Exceptions
{
    [Serializable]
    public class DataAccessFailedException : Exception
    {
        public DataAccessFailedException()
        {
        }

        public DataAccessFailedException(string? message) : base(message)
        {
        }

        public DataAccessFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DataAccessFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}