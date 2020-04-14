using System;

namespace BankAnalizer.Core
{
    public class BankAnalizerException : NonGenericException
    {
        public BankAnalizerException(string message) : base(message) { }
        public BankAnalizerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
