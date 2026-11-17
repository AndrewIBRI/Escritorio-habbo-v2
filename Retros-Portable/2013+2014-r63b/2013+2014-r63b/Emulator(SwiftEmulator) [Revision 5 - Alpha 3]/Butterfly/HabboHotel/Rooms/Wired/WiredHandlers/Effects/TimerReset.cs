namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    internal class TimerReset : IWiredEffect, IWiredTrigger, IWiredCycleable
    {
        private int cycles;
        private int delay;
        private bool disposed;
        private WiredHandler handler;
        private uint itemID = 0;
        private List<RoomItem> items;
        private Room room;

        public TimerReset(Room room, WiredHandler handler, List<RoomItem> items, int delay, uint itemID)
        {
            this.room = room;
            this.handler = handler;
            this.items = items;
            this.delay = delay;
            this.cycles = 0;
            this.disposed = false;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.itemID + "'");
            dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.itemID + "'");
        }

        public void Dispose()
        {
            this.disposed = true;
            this.room = null;
            this.handler = null;
            if (this.items != null)
            {
                this.items.Clear();
            }
            this.items = null;
        }

        public bool Disposed()
        {
            return this.disposed;
        }

        public bool Handle(RoomUser user, Team team, RoomItem item)
        {
            if (this.delay > 0)
            {
                this.cycles = 0;
                this.handler.RequestCycle(this);
            }
            else
            {
                return this.ResetTimers();
            }
            return false;
        }

        public bool IsSpecial(out SpecialEffects function)
        {
            function = SpecialEffects.None;
            return false;
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.itemID);
            DataRow row = dbClient.getRow();
            if (row != null)
            {
                this.delay = Convert.ToInt32(row[0].ToString());
            }
            else
            {
                this.delay = 20;
            }
            dbClient.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = " + this.itemID);
            DataTable table = dbClient.getTable();
            foreach (DataRow row2 in table.Rows)
            {
                RoomItem item = insideRoom.GetRoomItemHandler().GetItem(Convert.ToUInt32(row2[0]));
                if ((item != null) && !this.items.Contains(item))
                {
                    this.items.Add(item);
                }
            }
        }

        public bool OnCycle()
        {
            if (this.cycles > this.delay)
            {
                this.ResetTimers();
                return false;
            }
            this.cycles++;
            return true;
        }

        private bool ResetTimers()
        {
            this.handler.OnEvent(this.itemID);
            bool flag = false;
            foreach (RoomItem item in this.items)
            {
                if (item.wiredHandler != null)
                {
                    IWiredTimer wiredHandler = item.wiredHandler as IWiredTimer;
                    if (wiredHandler != null)
                    {
                        wiredHandler.ResetTimer();
                        flag = true;
                    }
                }
            }
            return flag;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.itemID, "integer", string.Empty, this.delay.ToString(), false);
            lock (this.items)
            {
                dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.itemID + "'");
                foreach (RoomItem item in this.items)
                {
                    WiredUtillity.SaveTrigger(dbClient, (int) this.itemID, (int) item.Id);
                }
            }
        }
    }
}

