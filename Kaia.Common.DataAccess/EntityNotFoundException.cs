using System;

namespace Kaia.Common.DataAccess
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base() { }

        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException(Exception innerException) :
            base(null, innerException)
        { }

        public EntityNotFoundException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }
}
