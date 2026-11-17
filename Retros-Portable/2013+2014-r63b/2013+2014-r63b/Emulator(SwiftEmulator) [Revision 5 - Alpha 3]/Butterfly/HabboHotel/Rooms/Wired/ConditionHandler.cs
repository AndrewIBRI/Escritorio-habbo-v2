namespace Butterfly.HabboHotel.Rooms.Wired
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    internal class ConditionHandler
    {
        private Queue addQueue;
        private Queue removeQueue;
        private Room room;
        private Hashtable roomMatrix;

        public ConditionHandler(Room room)
        {
            this.room = room;
            this.roomMatrix = new Hashtable();
            this.addQueue = new Queue();
            this.removeQueue = new Queue();
        }

        internal void AddOrIgnoreRefferance(RoomItem item)
        {
            lock (this.addQueue.SyncRoot)
            {
                this.addQueue.Enqueue(item);
            }
        }

        private void AddOrUpdateRefferance(Point coordinate, IWiredCondition item)
        {
            List<IWiredCondition> list;
            if (this.roomMatrix.ContainsKey(coordinate))
            {
                list = (List<IWiredCondition>) this.roomMatrix[coordinate];
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
            else
            {
                list = new List<IWiredCondition>(1) {
                    item
                };
                this.roomMatrix.Add(coordinate, list);
            }
        }

        internal bool AllowsHandling(Point coordinate, RoomUser user)
        {
            if (this.roomMatrix.ContainsKey(coordinate))
            {
                List<IWiredCondition> list = (List<IWiredCondition>) this.roomMatrix[coordinate];
                foreach (IWiredCondition condition in list)
                {
                    if (!condition.AllowsExecution(user))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal void ClearTile(Point coordinate)
        {
            lock (this.removeQueue.SyncRoot)
            {
                this.removeQueue.Enqueue(coordinate);
            }
        }

        internal void OnCycle()
        {
            this.WorkAddQueue();
            this.WorkRemoveQueue();
        }

        private void WorkAddQueue()
        {
            lock (this.addQueue.SyncRoot)
            {
                while (this.addQueue.Count > 0)
                {
                    RoomItem item = (RoomItem) this.addQueue.Dequeue();
                    this.AddOrUpdateRefferance(item.Coordinate, item.wiredCondition);
                }
            }
        }

        private void WorkRemoveQueue()
        {
            lock (this.removeQueue.SyncRoot)
            {
                while (this.removeQueue.Count > 0)
                {
                    Point key = (Point) this.removeQueue.Dequeue();
                    this.roomMatrix.Remove(key);
                }
            }
        }
    }
}

