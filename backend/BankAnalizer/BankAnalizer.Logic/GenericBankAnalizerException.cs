using System;
using System.Runtime.Serialization;

namespace BankAnalizer.Core
{
    public class GenericBankAnalizerException : CoreException
    {
        public GenericBankAnalizerException()
        {
        }

        public GenericBankAnalizerException(string message) : base(message)
        {
        }

        public GenericBankAnalizerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GenericBankAnalizerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
