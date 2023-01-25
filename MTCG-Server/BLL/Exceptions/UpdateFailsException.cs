using System.Runtime.Serialization;

namespace MTCGServer.BLL
{
    [Serializable]
    public class UpdateFailsException : Exception
    {
        public UpdateFailsException()
        {
        }
        public UpdateFailsException(string? message) : base(message)
        {
        }
        public UpdateFailsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
        protected UpdateFailsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}