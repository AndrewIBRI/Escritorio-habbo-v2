namespace Butterfly.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public class SafeEnumeratorKvp<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
    {
        private readonly IEnumerator<KeyValuePair<TKey, TValue>> m_Inner;
        private readonly ReaderWriterLockSlim m_Lock;

        public SafeEnumeratorKvp(IEnumerator<KeyValuePair<TKey, TValue>> inner, ReaderWriterLockSlim @lock)
        {
            this.m_Inner = inner;
            this.m_Lock = @lock;
            this.m_Lock.EnterReadLock();
        }

        public void Dispose()
        {
            this.m_Lock.ExitReadLock();
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            return this.m_Inner.MoveNext();
        }

        public void Reset()
        {
            this.m_Inner.Reset();
        }

        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                return this.m_Inner.Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }
    }
}

