namespace SiecaAPI.Errors
{
    [Serializable]
    public class MissingArgumentsException: Exception
    {
        public MissingArgumentsException() {
        }

        public MissingArgumentsException(string message)
            : base(message) { }

        public MissingArgumentsException(string message, Exception inner)
            : base(message, inner) { }

        protected MissingArgumentsException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
