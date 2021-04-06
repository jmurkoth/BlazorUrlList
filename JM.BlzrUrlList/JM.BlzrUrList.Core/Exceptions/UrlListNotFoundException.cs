using System;
using System.Runtime.Serialization;

namespace JM.BlzrUrlList.Exceptions
{
    [Serializable]
    public class UrlListNotFoundException : Exception
    {
        public UrlListNotFoundException()
        {
        }

        public UrlListNotFoundException(string message) : base($"{message} not found")
        {
        }

        public UrlListNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UrlListNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}