using System;
using System.Runtime.Serialization;

namespace HatTrick.BLL.Exceptions
{
    [Serializable]
    public class InternalException : Exception
    {
        public InternalException() :
            base()
        {
        }

        public InternalException(
            string? message
        ) :
            base(message)
        {
        }

        public InternalException(
            string? message,
            Exception? innerException
        ) :
            base(message, innerException)
        {
        }

        protected InternalException(
            SerializationInfo info,
            StreamingContext context
        ) :
            base(info, context)
        {
        }
    }
}
