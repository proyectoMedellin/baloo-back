namespace SiecaAPI.Errors
{
    [Serializable]
    public class NoDataFoundException: Exception
    {
        public NoDataFoundException() {
        }

        public NoDataFoundException(string message)
            : base(message) { }

        public NoDataFoundException(string message, Exception inner)
            : base(message, inner) { }

        protected NoDataFoundException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
