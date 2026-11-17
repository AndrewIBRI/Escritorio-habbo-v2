namespace Butterfly.Collections
{
    using System;
    using System.Threading;

    public class AcquireReaderLock : IDisposable
    {
        private bool m_Disposed = false;
        private ReaderWriterLockSlim m_Lock = null;

        public AcquireReaderLock(ReaderWriterLockSlim rwl)
        {
            this.m_Lock = rwl;
            this.m_Lock.EnterReadLock();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.m_Disposed && disposing)
            {
                this.m_Lock.ExitReadLock();
            }
            this.m_Disposed = true;
        }
    }
}

