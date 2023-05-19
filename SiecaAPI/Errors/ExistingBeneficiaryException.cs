namespace SiecaAPI.Errors
{
    [Serializable]
    public class ExistingBeneficiaryException : Exception
    {
        public ExistingBeneficiaryException() {
        }

        public ExistingBeneficiaryException(string message)
            : base(message) { }

        public ExistingBeneficiaryException(string message, Exception inner)
            : base(message, inner) { }

        protected ExistingBeneficiaryException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
