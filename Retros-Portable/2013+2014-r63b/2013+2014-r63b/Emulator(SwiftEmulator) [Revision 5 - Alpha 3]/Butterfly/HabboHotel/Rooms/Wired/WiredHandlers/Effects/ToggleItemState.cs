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
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    internal class ToggleItemState : IWiredTrigger, IWiredCycleable, IWiredEffect
    {
        private int cycles;
        private int delay;
        private Queue delayedTriggeringUsers;
        private bool disposed;
        private Gamemap gamemap;
        private WiredHandler handler;
        private RoomItem item;
        private List<RoomItem> items;

        public ToggleItemState(Gamemap gamemap, WiredHandler handler, List<RoomItem> items, int delay, RoomItem Item)
        {
            this.item = Item;
            this.gamemap = gamemap;
            this.handler = handler;
            this.items = items;
            this.delay = delay;
            this.cycles = 0;
            this.delayedTriggeringUsers = new Queue();
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
            this.gamemap = null;
            this.handler = null;
            if (this.items != null)
            {
                this.items.Clear();
            }
            this.delayedTriggeringUsers.Clear();
        }

        public bool Disposed()
        {
            return this.disposed;
        }

        public bool Handle(RoomUser user, Team team, RoomItem i)
        {
            if (!this.disposed)
            {
                this.cycles = 0;
                if ((this.delay == 0) && (user != null))
                {
                    return this.ToggleItems(user);
                }
                lock (this.delayedTriggeringUsers.SyncRoot)
                {
                    this.delayedTriggeringUsers.Enqueue(user);
                }
                this.handler.RequestCycle(this);
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
            dbClient.addParameter("id", (int) this.item.Id);
            DataRow row = dbClient.getRow();
            if (row != null)
            {
                this.delay = Convert.ToInt32(row[0].ToString());
            }
            else
            {
                this.delay = 20;
            }
            dbClient.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = " + this.item.Id);
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
            if (this.disposed)
            {
                return false;
            }
            this.cycles++;
            if (this.cycles > this.delay)
            {
                if (this.delayedTriggeringUsers.Count > 0)
                {
                    lock (this.delayedTriggeringUsers.SyncRoot)
                    {
                        while (this.delayedTriggeringUsers.Count > 0)
                        {
                            RoomUser user = (RoomUser) this.delayedTriggeringUsers.Dequeue();
                            this.ToggleItems(user);
                        }
                    }
                }
                return false;
            }
            return true;
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

        private bool ToggleItems(RoomUser user)
        {
            if (this.disposed)
            {
                return false;
            }
            this.handler.OnEvent(this.item.Id);
            bool flag = false;
            foreach (RoomItem item in this.items)
            {
                if (item != null)
                {
                    if ((user != null) && (user.GetClient() != null))
                    {
                        item.Interactor.OnTrigger(user.GetClient(), item, 0, true);
                    }
                    else
                    {
                        item.Interactor.OnTrigger(null, item, 0, true);
                    }
                    flag = true;
                }
            }
            return flag;
        }
    }
}

