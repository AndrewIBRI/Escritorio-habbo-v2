namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Conditions
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class TriggerUserIsOnFurni : IWiredCondition, IWiredTrigger
    {
        private bool isDisposed;
        private RoomItem item;
        private List<RoomItem> items;

        public TriggerUserIsOnFurni(RoomItem item, List<RoomItem> items)
        {
            this.item = item;
            this.items = items;
            this.isDisposed = false;
        }

        public bool AllowsExecution(RoomUser user)
        {
            foreach (RoomItem item in this.items)
            {
                if (item.Coordinate == user.Coordinate)
                {
                    return true;
                }
            }
            return false;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            this.isDisposed = true;
            this.item = null;
            if (this.items != null)
            {
                this.items.Clear();
            }
            this.items = null;
        }

        public bool Disposed()
        {
            return this.isDisposed;
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = " + this.item.Id);
            DataTable table = dbClient.getTable();
            foreach (DataRow row in table.Rows)
            {
                RoomItem item = insideRoom.GetRoomItemHandler().GetItem(Convert.ToUInt32(row[0]));
                if ((item != null) && !this.items.Contains(item))
                {
                    this.items.Add(item);
                }
            }
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
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

