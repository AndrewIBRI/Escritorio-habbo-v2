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
    using System.Linq;
    using System.Runtime.InteropServices;

    internal class TeleportToItem : IWiredTrigger, IWiredCycleable, IWiredEffect
    {
        private int cycles;
        private int delay;
        private Queue delayedUsers;
        private bool disposed;
        private Gamemap gamemap;
        private WiredHandler handler;
        private uint itemID;
        private List<RoomItem> items;
        private Random rnd;

        public TeleportToItem(Gamemap gamemap, WiredHandler handler, List<RoomItem> items, int delay, uint itemID)
        {
            this.gamemap = gamemap;
            this.handler = handler;
            this.items = items;
            this.delay = delay + 2;
            this.itemID = itemID;
            this.cycles = 0;
            this.delayedUsers = new Queue();
            this.rnd = new Random();
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
            this.gamemap = null;
            this.handler = null;
            if (this.items != null)
            {
                this.items.Clear();
            }
            this.items = null;
            if (this.delayedUsers != null)
            {
                this.delayedUsers.Clear();
            }
        }

        public bool Disposed()
        {
            return this.disposed;
        }

        public bool Handle(RoomUser user, Team team, RoomItem item)
        {
            user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(4);
            this.cycles = 0;
            if ((this.delay == 0) && (user != null))
            {
                return this.TeleportUser(user);
            }
            lock (this.delayedUsers.SyncRoot)
            {
                this.delayedUsers.Enqueue(user);
            }
            this.handler.RequestCycle(this);
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
            this.delay = dbClient.getInteger();
            dbClient.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = " + this.itemID);
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

        public bool OnCycle()
        {
            this.cycles++;
            if (this.cycles > this.delay)
            {
                if (this.delayedUsers.Count > 0)
                {
                    lock (this.delayedUsers.SyncRoot)
                    {
                        while (this.delayedUsers.Count > 0)
                        {
                            RoomUser user = (RoomUser) this.delayedUsers.Dequeue();
                            this.TeleportUser(user);
                        }
                    }
                }
                return false;
            }
            return true;
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

        private bool TeleportUser(RoomUser user)
        {
            if ((user != null) && !this.disposed)
            {
                if (this.items.Count > 1)
                {
                    int num = 0;
                    for (int i = 0; i < this.items.Count; i++)
                    {
                        num = this.rnd.Next(0, this.items.Count);
                        RoomItem item = this.items[num];
                        if (item.Coordinate != user.Coordinate)
                        {
                            this.gamemap.TeleportToItem(user, item);
                            user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(0);
                            return true;
                        }
                    }
                }
                else if (this.items.Count == 1)
                {
                    this.gamemap.TeleportToItem(user, this.items.First<RoomItem>());
                    user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(0);
                    return true;
                }
            }
            return false;
        }
    }
}

