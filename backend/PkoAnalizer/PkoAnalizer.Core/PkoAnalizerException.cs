using System;

namespace PkoAnalizer.Core
{
    public class BankAnalizerException : Exception
    {
        public BankAnalizerException() { }
        public BankAnalizerException(string message) : base(message) { }
        public BankAnalizerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
