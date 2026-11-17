namespace Butterfly.Core
{
    using System;

    internal class MissingLocaleException : Exception
    {
        public MissingLocaleException(string message) : base(message)
        {
        }
    }
}

