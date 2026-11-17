namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    internal class WalksOffFurni : IWiredTrigger, IWiredCycleable, IWiredTimer
    {
        private int currentCycle;
        private UserWalksFurniDelegate delegateFunction;
        private bool disposed;
        private WiredHandler handler;
        private RoomItem item;
        private List<RoomItem> items;
        private Queue requestQueue;
        private int requiredCycles;

        public WalksOffFurni(RoomItem item, WiredHandler handler, List<RoomItem> targetItems, int requiredCycles)
        {
            this.item = item;
            this.handler = handler;
            this.items = targetItems;
            this.delegateFunction = new UserWalksFurniDelegate(this.targetItem_OnUserWalksOffFurni);
            this.currentCycle = 0;
            this.requiredCycles = requiredCycles;
            this.requestQueue = new Queue();
            foreach (RoomItem item2 in targetItems)
            {
                item2.OnUserWalksOffFurni += this.delegateFunction;
            }
            this.disposed = false;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
            dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            this.disposed = true;
            if (this.items != null)
            {
                foreach (RoomItem item in this.items)
                {
                    item.OnUserWalksOffFurni -= this.delegateFunction;
                }
                this.items.Clear();
            }
            this.items = null;
            this.item = null;
            this.handler = null;
        }

        public bool Disposed()
        {
            return this.disposed;
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.item.Id);
            DataRow row = dbClient.getRow();
            if (row != null)
            {
                this.requiredCycles = Convert.ToInt32(row[0].ToString());
            }
            else
            {
                this.requiredCycles = 0;
            }
            dbClient.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = " + this.item.Id);
            DataTable table = dbClient.getTable();
            foreach (DataRow row2 in table.Rows)
            {
                RoomItem item = insideRoom.GetRoomItemHandler().GetItem(Convert.ToUInt32(row2[0]));
                if ((item != null) && !this.items.Contains(item))
                {
                    item.OnUserWalksOffFurni += this.delegateFunction;
                    this.items.Add(item);
                }
            }
        }

        public bool OnCycle()
        {
            if (this.currentCycle > this.requiredCycles)
            {
                if (this.requestQueue.Count > 0)
                {
                    lock (this.requestQueue.SyncRoot)
                    {
                        while (this.requestQueue.Count > 0)
                        {
                            UserWalksFurniValue value2 = (UserWalksFurniValue) this.requestQueue.Dequeue();
                            this.handler.RequestStackHandle(this.item.Coordinate, value2.item, value2.user, Team.none);
                        }
                    }
                    this.handler.OnEvent(this.item.Id);
                }
                return false;
            }
            this.currentCycle++;
            return true;
        }

        public void ResetTimer()
        {
            this.currentCycle = this.requiredCycles;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.item.Id, "integer", string.Empty, this.requestQueue.ToString(), false);
            lock (this.items)
            {
                dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.item.Id + "'");
                foreach (RoomItem item in this.items)
                {
                    WiredUtillity.SaveTrigger(dbClient, (int) this.item.Id, (int) item.Id);
                }
            }
        }

        private void targetItem_OnUserWalksOffFurni(object sender, UserWalksOnArgs e)
        {
            if (this.requiredCycles > 0)
            {
                UserWalksFurniValue value2 = new UserWalksFurniValue(e.user, (RoomItem) sender);
                lock (this.requestQueue.SyncRoot)
                {
                    this.requestQueue.Enqueue(value2);
                }
                this.handler.RequestCycle(this);
            }
            else
            {
                this.handler.RequestStackHandle(this.item.Coordinate, (RoomItem) sender, e.user, Team.none);
                this.handler.OnEvent(this.item.Id);
            }
        }
    }
}

