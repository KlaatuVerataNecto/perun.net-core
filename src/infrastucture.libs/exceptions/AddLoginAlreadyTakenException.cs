using System;

namespace infrastucture.libs.exceptions
{
    public class AddLoginAlreadyTakenException : Exception
    {
        public AddLoginAlreadyTakenException()
        {
        }

        public AddLoginAlreadyTakenException(string message)
        : base(message)
        {
        }

        public AddLoginAlreadyTakenException(string message, Exception inner)
        : base(message, inner)
    {
        }
    }
}
