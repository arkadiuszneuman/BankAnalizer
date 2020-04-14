using System;

namespace BankAnalizer.Core
{
    public class NonGenericException : CoreException
    {
        public NonGenericException(string message) : base(message) { }
        public NonGenericException(string message, Exception innerException) : base(message, innerException) { }
    }
}
