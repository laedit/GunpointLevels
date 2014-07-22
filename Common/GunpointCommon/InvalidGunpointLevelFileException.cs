using System;
using System.Runtime.Serialization;

namespace Gunpoint.Common
{
    [Serializable]
    public class InvalidGunpointLevelFileException : Exception
    {
        public InvalidGunpointLevelFileException() { }

        public InvalidGunpointLevelFileException(string message) : base(message) { }

        public InvalidGunpointLevelFileException(string message, Exception inner) : base(message, inner) { }

        protected InvalidGunpointLevelFileException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
