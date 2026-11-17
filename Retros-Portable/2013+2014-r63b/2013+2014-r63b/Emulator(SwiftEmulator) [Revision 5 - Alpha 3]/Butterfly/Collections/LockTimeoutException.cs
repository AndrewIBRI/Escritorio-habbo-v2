namespace Butterfly.Collections
{
    using System;

    [Serializable]
    internal class LockTimeoutException : Exception
    {
        public LockTimeoutException(string exception) : base(exception)
        {
        }
    }
}

