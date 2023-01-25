using System.Runtime.Serialization;

namespace MTCGServer.BLL
{
    [Serializable]
    public class DeletingFailsException : Exception
    {
        public DeletingFailsException()
        {
        }
        public DeletingFailsException(string? message) : base(message)
        {
        }
        public DeletingFailsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
        protected DeletingFailsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}