namespace Butterfly.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SafeEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
    {
        private readonly IEnumerator<T> m_Inner;

        public SafeEnumerator(IEnumerator<T> inner)
        {
            this.m_Inner = inner;
        }

        public void Dispose()
        {
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

        public T Current
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

