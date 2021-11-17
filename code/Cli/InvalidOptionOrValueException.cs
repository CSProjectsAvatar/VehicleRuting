using System;
using System.Runtime.Serialization;

namespace Cli {
    [Serializable]
    internal class InvalidOptionOrValueException : Exception {
        public InvalidOptionOrValueException() {
        }

        public InvalidOptionOrValueException(string message) : base(message) {
        }

        public InvalidOptionOrValueException(string message, Exception innerException) : base(message, innerException) {
        }

        protected InvalidOptionOrValueException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}