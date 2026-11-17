namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly.HabboHotel.Items;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    internal class CoordItemSearch
    {
        private Hashtable items;

        public CoordItemSearch(Hashtable itemArray)
        {
            this.items = itemArray;
        }

        internal List<RoomItem> GetAllRoomItemForSquare(int pX, int pY)
        {
            Point key = new Point(pX, pY);
            List<RoomItem> list = new List<RoomItem>();
            if (this.items.ContainsKey(key))
            {
                List<RoomItem> list2 = (List<RoomItem>) this.items[key];
                foreach (RoomItem item in list2)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        internal List<RoomItem> GetRoomItemForSquare(int pX, int pY)
        {
            Point key = new Point(pX, pY);
            List<RoomItem> list = new List<RoomItem>();
            if (this.items.ContainsKey(key))
            {
                List<RoomItem> list2 = (List<RoomItem>) this.items[key];
                foreach (RoomItem item in list2)
                {
                    if ((item.Coordinate.X == key.X) && (item.Coordinate.Y == key.Y))
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        internal List<RoomItem> GetRoomItemForSquare(int pX, int pY, double minZ)
        {
            List<RoomItem> list = new List<RoomItem>();
            Point key = new Point(pX, pY);
            if (this.items.ContainsKey(key))
            {
                List<RoomItem> list2 = (List<RoomItem>) this.items[key];
                foreach (RoomItem item in list2)
                {
                    if ((item.GetZ > minZ) && ((item.GetX == pX) && (item.GetY == pY)))
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }
    }
}

