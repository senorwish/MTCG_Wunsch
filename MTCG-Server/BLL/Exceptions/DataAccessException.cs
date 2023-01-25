﻿using System.Runtime.Serialization;

namespace MTCGServer.BLL.Exceptions
{
    [Serializable]
    public class DataAccessException : Exception
    {
        public DataAccessException()
        {
        }
        public DataAccessException(string? message) : base(message)
        {
        }
        public DataAccessException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
        protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}