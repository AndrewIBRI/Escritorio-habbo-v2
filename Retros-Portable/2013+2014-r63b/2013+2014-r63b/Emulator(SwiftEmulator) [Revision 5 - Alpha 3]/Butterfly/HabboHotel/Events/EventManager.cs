namespace Butterfly.HabboHotel.Events
{
    using Butterfly.HabboHotel.Rooms;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal class EventManager
    {
        private Queue addQueue;
        private Dictionary<int, EventCategory> eventCategories = new Dictionary<int, EventCategory>();
        private Dictionary<RoomData, int> events = new Dictionary<RoomData, int>();
        private IOrderedEnumerable<KeyValuePair<RoomData, int>> orderedEventRooms;
        private Queue removeQueue;
        private Queue updateQueue;

        public EventManager()
        {
            this.orderedEventRooms = from t in this.events
                orderby t.Value descending
                select t;
            this.addQueue = new Queue();
            this.removeQueue = new Queue();
            this.updateQueue = new Queue();
            for (int i = 0; i < 30; i++)
            {
                this.eventCategories.Add(i, new EventCategory(i));
            }
        }

        internal KeyValuePair<RoomData, int>[] GetRoomsForCategory(int categoryID)
        {
            if (categoryID > 0)
            {
                return this.eventCategories[categoryID].GetActiveRooms();
            }
            return this.orderedEventRooms.ToArray<KeyValuePair<RoomData, int>>();
        }

        internal void onCycle()
        {
            this.workRemoveQueue();
            this.workAddQueue();
            this.workUpdate();
            this.SortCollection();
            foreach (EventCategory category in this.eventCategories.Values)
            {
                category.onCycle();
            }
        }

        internal void QueueAddEvent(RoomData data, int roomEventCategory)
        {
            lock (this.addQueue.SyncRoot)
            {
                this.addQueue.Enqueue(data);
            }
            this.eventCategories[roomEventCategory].QueueAddEvent(data);
        }

        internal void QueueRemoveEvent(RoomData data, int roomEventCategory)
        {
            lock (this.removeQueue.SyncRoot)
            {
                this.removeQueue.Enqueue(data);
            }
            this.eventCategories[roomEventCategory].QueueRemoveEvent(data);
        }

        internal void QueueUpdateEvent(RoomData data, int roomEventCategory)
        {
            lock (this.updateQueue.SyncRoot)
            {
                this.updateQueue.Enqueue(data);
            }
            this.eventCategories[roomEventCategory].QueueUpdateEvent(data);
        }

        private void SortCollection()
        {
            this.orderedEventRooms = from t in this.events.Take<KeyValuePair<RoomData, int>>(40)
                orderby t.Value descending
                select t;
        }

        private void workAddQueue()
        {
            if (this.addQueue.Count > 0)
            {
                lock (this.addQueue.SyncRoot)
                {
                    while (this.addQueue.Count > 0)
                    {
                        RoomData key = (RoomData) this.addQueue.Dequeue();
                        if (!this.events.ContainsKey(key))
                        {
                            this.events.Add(key, key.UsersNow);
                        }
                    }
                }
            }
        }

        private void workRemoveQueue()
        {
            if (this.removeQueue.Count > 0)
            {
                lock (this.removeQueue.SyncRoot)
                {
                    while (this.removeQueue.Count > 0)
                    {
                        RoomData key = (RoomData) this.removeQueue.Dequeue();
                        this.events.Remove(key);
                    }
                }
            }
        }

        private void workUpdate()
        {
            if (this.removeQueue.Count > 0)
            {
                lock (this.removeQueue.SyncRoot)
                {
                    while (this.removeQueue.Count > 0)
                    {
                        RoomData key = (RoomData) this.updateQueue.Dequeue();
                        if (this.events.ContainsKey(key))
                        {
                            this.events[key] = key.UsersNow;
                        }
                    }
                }
            }
        }
    }
}

