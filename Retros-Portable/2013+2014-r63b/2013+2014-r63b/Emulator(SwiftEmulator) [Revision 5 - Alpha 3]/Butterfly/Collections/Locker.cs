namespace Butterfly.Collections
{
    using System;
    using System.Threading;

    public class Locker
    {
        internal int handle = 0;

        internal bool ExitLock()
        {
            return (Interlocked.Exchange(ref this.handle, 0) == 1);
        }

        internal bool TryEnterLock()
        {
            if (this.handle == 1)
            {
                return false;
            }
            return (Interlocked.Exchange(ref this.handle, 1) == 0);
        }
    }
}

