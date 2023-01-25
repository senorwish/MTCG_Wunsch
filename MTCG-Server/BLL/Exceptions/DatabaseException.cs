using System.Runtime.Serialization;

namespace MTCGServer.BLL
{
    [Serializable]
    public class DatabaseException : Exception
    {
        public DatabaseException()
        {
        }
        public DatabaseException(string? message) : base(message)
        {
        }
        public DatabaseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
        protected DatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}