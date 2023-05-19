namespace SiecaAPI.Errors
{
    [Serializable]
    public class GenericValidationException : Exception
    {
        public GenericValidationException() {
        }

        public GenericValidationException(string message)
            : base(message) { }

        public GenericValidationException(string message, Exception inner)
            : base(message, inner) { }

        protected GenericValidationException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
