using System;

namespace PkoAnalizer.Core
{
    public class PkoAnalizerException : Exception
    {
        public PkoAnalizerException() { }
        public PkoAnalizerException(string message) : base(message) { }
        public PkoAnalizerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
