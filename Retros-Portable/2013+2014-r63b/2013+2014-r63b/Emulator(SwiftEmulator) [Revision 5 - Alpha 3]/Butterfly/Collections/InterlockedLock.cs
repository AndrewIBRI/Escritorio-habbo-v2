namespace Butterfly.Collections
{
    using System;
    using System.Threading;

    public class InterlockedLock : IDisposable
    {
        private bool disposed = false;
        private Locker locker;
        private const int timeout = 100;

        public InterlockedLock(Locker _locker)
        {
            this.locker = _locker;
            int num = 0;
            while (!this.locker.TryEnterLock())
            {
                num++;
                Thread.Sleep(1);
                if (num > 100)
                {
                    throw new LockTimeoutException("Unable to lock collection");
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.disposed = true;
                if (!this.locker.ExitLock())
                {
                    throw new LockTimeoutException("EH DARIO");
                }
            }
        }
    }
}

