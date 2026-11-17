namespace ConnectionManager.Socket_Exceptions
{
    using System;

    internal class SocketInitializationException : Exception
    {
        public SocketInitializationException(string message) : base(message)
        {
        }
    }
}

