using System;

namespace Atom.Collections
{
    [Serializable]
    class LockTimeoutException : Exception
    {
        public LockTimeoutException(string exception)
            : base(exception)
        {
            //Mini weini
        }
    }
}
