namespace Butterfly.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal class QueuedDictionary<T, V>
    {
        private Queue addQueue;
        private Dictionary<T, V> collection;
        private EventHandler onAdd;
        private EventHandler onCycleDone;
        private Queue onCycleEventQueue;
        private EventHandler onRemove;
        private EventHandler onUpdate;
        private Queue removeQueue;
        private Queue updateQueue;

        public QueuedDictionary()
        {
            this.collection = new Dictionary<T, V>();
            this.addQueue = new Queue();
            this.updateQueue = new Queue();
            this.removeQueue = new Queue();
            this.onCycleEventQueue = new Queue();
        }

        public QueuedDictionary(EventHandler onAddItem, EventHandler onUpdate, EventHandler onRemove, EventHandler onCycleDone)
        {
            this.collection = new Dictionary<T, V>();
            this.addQueue = new Queue();
            this.updateQueue = new Queue();
            this.removeQueue = new Queue();
            this.onAdd = onAddItem;
            this.onUpdate = onUpdate;
            this.onRemove = onRemove;
            this.onCycleDone = onCycleDone;
            this.onCycleEventQueue = new Queue();
        }

        internal void Add(T key, V value)
        {
            KeyValuePair<T, V> pair = new KeyValuePair<T, V>(key, value);
            lock (this.addQueue.SyncRoot)
            {
                this.addQueue.Enqueue(pair);
            }
        }

        internal void Clear()
        {
            this.collection.Clear();
        }

        internal bool ContainsKey(T key)
        {
            return this.collection.ContainsKey(key);
        }

        internal void Destroy()
        {
            if (this.collection != null)
            {
                this.collection.Clear();
            }
            if (this.addQueue != null)
            {
                this.addQueue.Clear();
            }
            if (this.updateQueue != null)
            {
                this.updateQueue.Clear();
            }
            if (this.removeQueue != null)
            {
                this.removeQueue.Clear();
            }
            if (this.onCycleEventQueue != null)
            {
                this.onCycleEventQueue.Clear();
            }
            this.collection = null;
            this.addQueue = null;
            this.updateQueue = null;
            this.removeQueue = null;
            this.onCycleEventQueue = null;
            this.onAdd = null;
            this.onUpdate = null;
            this.onRemove = null;
            this.onCycleDone = null;
        }

        internal V GetValue(T key)
        {
            if (this.collection.ContainsKey(key))
            {
                return this.collection[key];
            }
            return default(V);
        }

        private void onAddItem(object sender, EventArgs args)
        {
        }

        internal void OnCycle()
        {
            this.WorkRemoveQueue();
            this.WorkAddQueue();
            this.WorkUpdateQueue();
            this.WorkOnEventDoneQueue();
            if (this.onCycleDone != null)
            {
                this.onCycleDone(null, new EventArgs());
            }
        }

        private void onCycleIsDone(object sender, EventArgs args)
        {
        }

        private void onRemoveItem(object sender, EventArgs args)
        {
        }

        private void onUpdateItem(object sender, EventArgs args)
        {
        }

        internal void QueueDelegate(onCycleDoneDelegate function)
        {
            lock (this.onCycleEventQueue.SyncRoot)
            {
                this.onCycleEventQueue.Enqueue(function);
            }
        }

        internal void Remove(T key)
        {
            lock (this.removeQueue.SyncRoot)
            {
                this.removeQueue.Enqueue(key);
            }
        }

        internal List<KeyValuePair<T, V>> ToList()
        {
            return this.collection.ToList<KeyValuePair<T, V>>();
        }

        internal void Update(T key, V value)
        {
            KeyValuePair<T, V> pair = new KeyValuePair<T, V>(key, value);
            lock (this.updateQueue.SyncRoot)
            {
                this.updateQueue.Enqueue(pair);
            }
        }

        private void WorkAddQueue()
        {
            if (this.addQueue.Count > 0)
            {
                lock (this.addQueue.SyncRoot)
                {
                    while (this.addQueue.Count > 0)
                    {
                        KeyValuePair<T, V> sender = (KeyValuePair<T, V>) this.addQueue.Dequeue();
                        if (this.collection.ContainsKey(sender.Key))
                        {
                            this.collection[sender.Key] = sender.Value;
                        }
                        else
                        {
                            this.collection.Add(sender.Key, sender.Value);
                        }
                        if (this.onAdd != null)
                        {
                            this.onAdd(sender, null);
                        }
                    }
                }
            }
        }

        private void WorkEventQueue()
        {
            if (this.onCycleEventQueue.Count > 0)
            {
                lock (this.onCycleEventQueue.SyncRoot)
                {
                    while (this.onCycleEventQueue.Count > 0)
                    {
                        onCycleDoneDelegate delegate2 = (onCycleDoneDelegate) this.onCycleEventQueue.Dequeue();
                        delegate2();
                    }
                }
            }
        }

        private void WorkOnEventDoneQueue()
        {
            if (this.onCycleEventQueue.Count > 0)
            {
                lock (this.onCycleEventQueue.SyncRoot)
                {
                    while (this.onCycleEventQueue.Count > 0)
                    {
                        onCycleDoneDelegate delegate2 = (onCycleDoneDelegate) this.onCycleEventQueue.Dequeue();
                        delegate2();
                    }
                }
            }
        }

        private void WorkRemoveQueue()
        {
            if (this.removeQueue.Count > 0)
            {
                lock (this.removeQueue.SyncRoot)
                {
                    List<T> list = new List<T>();
                    while (this.removeQueue.Count > 0)
                    {
                        T key = (T) this.removeQueue.Dequeue();
                        if (this.collection.ContainsKey(key))
                        {
                            V local2 = this.collection[key];
                            this.collection.Remove(key);
                            KeyValuePair<T, V> sender = new KeyValuePair<T, V>(key, local2);
                            if (this.onRemove != null)
                            {
                                this.onRemove(sender, null);
                            }
                        }
                        else
                        {
                            list.Add(key);
                        }
                    }
                    if (list.Count > 0)
                    {
                        foreach (T local in list)
                        {
                            this.removeQueue.Enqueue(local);
                        }
                    }
                }
            }
        }

        private void WorkUpdateQueue()
        {
            if (this.updateQueue.Count > 0)
            {
                lock (this.updateQueue.SyncRoot)
                {
                    while (this.updateQueue.Count > 0)
                    {
                        KeyValuePair<T, V> sender = (KeyValuePair<T, V>) this.addQueue.Dequeue();
                        if (this.collection.ContainsKey(sender.Key))
                        {
                            this.collection[sender.Key] = sender.Value;
                        }
                        else
                        {
                            this.collection.Add(sender.Key, sender.Value);
                        }
                        if (this.onUpdate != null)
                        {
                            this.onUpdate(sender, null);
                        }
                    }
                }
            }
        }

        internal Dictionary<T, V> Inner
        {
            get
            {
                return this.collection;
            }
            set
            {
                this.collection = value;
            }
        }

        internal Dictionary<T, V>.KeyCollection Keys
        {
            get
            {
                return this.collection.Keys;
            }
        }

        internal Dictionary<T, V>.ValueCollection Values
        {
            get
            {
                return this.collection.Values;
            }
        }
    }
}

