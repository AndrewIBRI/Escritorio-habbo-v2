namespace Butterfly.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class SafeDictionary<T, V> : IDictionary<T, V>, ICollection<KeyValuePair<T, V>>, IEnumerable<KeyValuePair<T, V>>, IEnumerable, IDisposable
    {
        private readonly Dictionary<T, V> _inner;
        private readonly object lockObject;

        public SafeDictionary()
        {
            this._inner = new Dictionary<T, V>();
            this.lockObject = new object();
        }

        public SafeDictionary(Dictionary<T, V> inner)
        {
            this._inner = inner;
            this.lockObject = new object();
        }

        public void Add(KeyValuePair<T, V> item)
        {
            throw new NotImplementedException();
        }

        public void Add(T Item, V Value)
        {
            lock (this.lockObject)
            {
                this._inner.Add(Item, Value);
            }
        }

        public void Clear()
        {
            lock (this.lockObject)
            {
                this._inner.Clear();
            }
        }

        public bool Contains(KeyValuePair<T, V> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(T Item)
        {
            return this._inner.ContainsKey(Item);
        }

        public void CopyTo(KeyValuePair<T, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
            }
        }

        ~SafeDictionary()
        {
            this.Dispose(false);
        }

        public IEnumerator<KeyValuePair<T, V>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        internal Dictionary<T, V> getNonThreadSafeCollection()
        {
            return this._inner;
        }

        public V GetValue(T Item)
        {
            if (this._inner.ContainsKey(Item))
            {
                return this._inner[Item];
            }
            return default(V);
        }

        public bool Remove(KeyValuePair<T, V> item)
        {
            throw new NotImplementedException();
        }

        public void Remove(T Item)
        {
            lock (this.lockObject)
            {
                this._inner.Remove(Item);
            }
        }

        bool IDictionary<T, V>.Remove(T key)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(T key, out V value)
        {
            return this._inner.TryGetValue(key, out value);
        }

        public int Count
        {
            get
            {
                return this._inner.Count;
            }
        }

        internal Dictionary<T, V> getInner
        {
            get
            {
                return this._inner;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public V this[T Item]
        {
            get
            {
                return this._inner[Item];
            }
            set
            {
                this._inner[Item] = value;
            }
        }

        public ICollection<T> Keys
        {
            get
            {
                lock (this.lockObject)
                {
                    return new List<T>(this._inner.Keys);
                }
            }
        }

        public ICollection<V> Values
        {
            get
            {
                lock (this.lockObject)
                {
                    return new List<V>(this._inner.Values);
                }
            }
        }
    }
}

