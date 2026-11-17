namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    public class SateChanged : IWiredTrigger, IWiredTimer, IWiredCycleable
    {
        private int cycleCount;
        private int delay;
        private OnItemTrigger delegateFunction;
        private bool disposed;
        private WiredHandler handler;
        private RoomItem item;
        private List<RoomItem> items;
        private Queue triggeringQueue;

        public SateChanged(WiredHandler handler, RoomItem item, List<RoomItem> items, int delay)
        {
            this.handler = handler;
            this.items = items;
            this.item = item;
            this.delay = delay;
            this.delegateFunction = new OnItemTrigger(this.itemTriggered);
            this.cycleCount = 0;
            this.triggeringQueue = new Queue();
            foreach (RoomItem item2 in items)
            {
                item2.itemTriggerEventHandler += this.delegateFunction;
            }
            this.disposed = true;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
            dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            this.disposed = true;
            this.handler = null;
            if (this.items != null)
            {
                foreach (RoomItem item in this.items)
                {
                    item.itemTriggerEventHandler -= this.delegateFunction;
                }
                this.items.Clear();
            }
            this.items = null;
        }

        public bool Disposed()
        {
            return this.disposed;
        }

        private void itemTriggered(object sender, ItemTriggeredArgs e)
        {
            if (this.delay > 0)
            {
                this.triggeringQueue.Enqueue(e);
                this.handler.RequestCycle(this);
            }
            else
            {
                this.onTrigger(e);
            }
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.item.Id);
            this.delay = dbClient.getInteger();
            dbClient.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = " + this.item.Id);
            DataTable table = dbClient.getTable();
            foreach (DataRow row in table.Rows)
            {
                RoomItem item = insideRoom.GetRoomItemHandler().GetItem(Convert.ToUInt32(row[0]));
                if ((item != null) && !this.items.Contains(item))
                {
                    item.itemTriggerEventHandler += this.delegateFunction;
                    this.items.Add(item);
                }
            }
        }

        public bool OnCycle()
        {
            if (this.cycleCount > this.delay)
            {
                if (this.triggeringQueue.Count > 0)
                {
                    lock (this.triggeringQueue.SyncRoot)
                    {
                        while (this.triggeringQueue.Count > 0)
                        {
                            ItemTriggeredArgs e = (ItemTriggeredArgs) this.triggeringQueue.Dequeue();
                            this.onTrigger(e);
                        }
                    }
                }
                return false;
            }
            this.cycleCount++;
            return true;
        }

        private void onTrigger(ItemTriggeredArgs e)
        {
            this.handler.RequestStackHandle(this.item.Coordinate, e.TriggeringItem, e.TriggeringUser, Team.none);
            this.handler.OnEvent(this.item.Id);
        }

        public void ResetTimer()
        {
            this.cycleCount = this.delay;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.item.Id, "integer", string.Empty, this.delay.ToString(), false);
            lock (this.items)
            {
                dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.item.Id + "'");
                foreach (RoomItem item in this.items)
                {
                    WiredUtillity.SaveTrigger(dbClient, (int) this.item.Id, (int) item.Id);
                }
            }
        }
    }
}

