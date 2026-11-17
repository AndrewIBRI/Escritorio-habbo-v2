namespace Butterfly.Collections
{
    using System;
    using System.Collections;
    using System.Reflection;

    internal class DefinedHashtable<T, V> : Hashtable
    {
        public void Add(T key, T value)
        {
            base.Add(key, value);
        }

        public override void Clear()
        {
            base.Clear();
        }

        public override bool Contains(object key)
        {
            return base.Contains(key);
        }

        public bool ContainsKey(T key)
        {
            return base.ContainsKey(key);
        }

        public bool ContainsValue(V value)
        {
            return base.ContainsValue(value);
        }

        public void Remove(T key)
        {
            base.Remove(key);
        }

        internal V this[T key]
        {
            get
            {
                if (base.ContainsKey(key))
                {
                    return (V) base[key];
                }
                return default(V);
            }
        }

        public override ICollection Values
        {
            get
            {
                return base.Values;
            }
        }
    }
}

