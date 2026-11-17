namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly.Collections;
    using Butterfly.HabboHotel.Items;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class GameItemHandler
    {
        private QueuedDictionary<uint, RoomItem> banzaiPyramids;
        private QueuedDictionary<uint, RoomItem> banzaiTeleports;
        private Random rnd;
        private Room room;

        public GameItemHandler(Room room)
        {
            this.room = room;
            this.rnd = new Random();
            this.banzaiPyramids = new QueuedDictionary<uint, RoomItem>();
            this.banzaiTeleports = new QueuedDictionary<uint, RoomItem>();
        }

        internal void AddPyramid(RoomItem item, uint itemID)
        {
            if (this.banzaiPyramids.ContainsKey(itemID))
            {
                this.banzaiPyramids.Inner[itemID] = item;
            }
            else
            {
                this.banzaiPyramids.Add(itemID, item);
            }
        }

        internal void AddTeleport(RoomItem item, uint itemID)
        {
            if (this.banzaiTeleports.ContainsKey(itemID))
            {
                this.banzaiTeleports.Inner[itemID] = item;
            }
            else
            {
                this.banzaiTeleports.Add(itemID, item);
            }
        }

        private void CyclePyramids()
        {
            this.banzaiPyramids.OnCycle();
            Random random = new Random();
            foreach (RoomItem item in this.banzaiPyramids.Inner.Values)
            {
                if ((item.interactionCountHelper == 0) && (item.ExtraData == "1"))
                {
                    this.room.GetGameMap().RemoveFromMap(item, false);
                    item.interactionCountHelper = 1;
                }
                if (string.IsNullOrEmpty(item.ExtraData))
                {
                    item.ExtraData = "0";
                }
                if (random.Next(0, 30) == 15)
                {
                    if (item.ExtraData == "0")
                    {
                        item.ExtraData = "1";
                        item.UpdateState();
                        this.room.GetGameMap().RemoveFromMap(item, false);
                    }
                    else if (this.room.GetGameMap().itemCanBePlacedHere(item.GetX, item.GetY))
                    {
                        item.ExtraData = "0";
                        item.UpdateState();
                        this.room.GetGameMap().AddItemToMap(item, false);
                    }
                }
            }
        }

        private void CycleRandomTeleports()
        {
            this.banzaiTeleports.OnCycle();
        }

        internal void Destroy()
        {
            if (this.banzaiTeleports != null)
            {
                this.banzaiTeleports.Destroy();
            }
            if (this.banzaiPyramids != null)
            {
                this.banzaiPyramids.Clear();
            }
            this.banzaiPyramids = null;
            this.banzaiTeleports = null;
            this.room = null;
            this.rnd = null;
        }

        internal void OnCycle()
        {
            this.CyclePyramids();
            this.CycleRandomTeleports();
        }

        internal void onTeleportRoomUserEnter(RoomUser User, RoomItem Item)
        {
            IEnumerable<RoomItem> source = from p in this.banzaiTeleports.Inner.Values
                where p.Id != Item.Id
                select p;
            int maxValue = source.Count<RoomItem>();
            int num2 = this.rnd.Next(0, maxValue);
            int num3 = 0;
            if (maxValue != 0)
            {
                foreach (RoomItem item in source)
                {
                    if (item != null)
                    {
                        if (num3 == num2)
                        {
                            this.room.GetGameMap().TeleportToItem(User, item);
                            item.ExtraData = "1";
                            item.UpdateNeeded = true;
                            Item.ExtraData = "1";
                            Item.UpdateNeeded = true;
                            item.UpdateState();
                            Item.UpdateState();
                        }
                        num3++;
                    }
                }
            }
        }

        internal void RemovePyramid(uint itemID)
        {
            this.banzaiPyramids.Remove(itemID);
        }

        internal void RemoveTeleport(uint itemID)
        {
            this.banzaiTeleports.Remove(itemID);
        }
    }
}

