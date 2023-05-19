namespace SiecaAPI.Errors
{
    [Serializable]
    public class InvalidLoginException: Exception
    {
        public InvalidLoginException() {
        }

        public InvalidLoginException(string message)
            : base(message) { }

        public InvalidLoginException(string message, Exception inner)
            : base(message, inner) { }

        protected InvalidLoginException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
