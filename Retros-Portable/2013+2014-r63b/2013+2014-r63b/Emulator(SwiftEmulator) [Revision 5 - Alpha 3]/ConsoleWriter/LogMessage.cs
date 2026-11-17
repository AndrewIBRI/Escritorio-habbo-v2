namespace ConsoleWriter
{
    using System;

    internal class LogMessage
    {
        internal string location;
        internal string message;

        public LogMessage(string message, string location)
        {
            this.message = message;
            this.location = location;
        }

        internal void Dispose()
        {
            this.message = null;
            this.location = null;
        }
    }
}

