using System;

namespace persistance.ef.exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException() { }
        public ObjectNotFoundException(string message) : base(message) { }
        public ObjectNotFoundException(string message, System.Exception inner) : base(message, inner) { }

    }
}
