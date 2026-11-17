namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Runtime.InteropServices;

    internal class MoveRotate : MovementManagement, IWiredEffect, IWiredTrigger, IWiredCycleable
    {
        private int cycles;
        private int delay;
        private WiredHandler handler;
        private bool isDisposed;
        private uint itemID;
        private List<RoomItem> items;
        private MovementState movement;
        private Room room;
        private RotationState rotation;

        public MoveRotate(MovementState movement, RotationState rotation, List<RoomItem> items, int delay, Room room, WiredHandler handler, uint itemID)
        {
            this.movement = movement;
            this.rotation = rotation;
            this.items = items;
            this.delay = delay;
            this.room = room;
            this.handler = handler;
            this.cycles = 0;
            this.itemID = itemID;
            this.isDisposed = false;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.itemID + "'");
            dbClient.runFastQuery("DELETE FROM trigger_in_place WHERE original_trigger = '" + this.itemID + "'");
            dbClient.runFastQuery("DELETE FROM trigger_rotation WHERE item_id = '" + this.itemID + "'");
        }

        public void Dispose()
        {
            this.isDisposed = true;
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
            return this.isDisposed;
        }

        public bool Handle(RoomUser user, Team team, RoomItem item)
        {
            this.cycles = 0;
            if (this.delay == 0)
            {
                this.HandleItems();
            }
            else
            {
                this.handler.RequestCycle(this);
            }
            return false;
        }

        private bool HandleItems()
        {
            this.handler.OnEvent(this.itemID);
            bool flag = false;
            foreach (RoomItem item in this.items)
            {
                if (this.HandleMovement(item))
                {
                    flag = true;
                }
            }
            return flag;
        }

        private bool HandleMovement(RoomItem item)
        {
            Point point = base.HandleMovement(item.Coordinate, this.movement, item.Rot);
            int newRot = base.HandleRotation(item.Rot, this.rotation);
            return ((((point != item.Coordinate) || (newRot != item.Rot)) && this.room.GetGameMap().SquareIsOpen(point.X, point.Y, false)) && this.room.GetRoomItemHandler().SetFloorItem(null, item, point.X, point.Y, newRot, false, false, true));
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
            dbClient.setQuery("SELECT rotation_status, movement_status FROM trigger_rotation WHERE item_id = @id");
            dbClient.addParameter("id", (int) this.itemID);
            DataRow row = dbClient.getRow();
            if (row != null)
            {
                this.rotation = (RotationState) Convert.ToInt32(row[0]);
                this.movement = (MovementState) Convert.ToInt32(row[1]);
            }
            else
            {
                this.rotation = RotationState.none;
                this.movement = MovementState.none;
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
            if (this.room == null)
            {
                return false;
            }
            this.cycles++;
            if (this.cycles > this.delay)
            {
                this.cycles = 0;
                this.HandleItems();
                return false;
            }
            return true;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.itemID, "integer", string.Empty, this.delay.ToString(), false);
            if (dbClient.dbType == DatabaseType.MSSQL)
            {
                dbClient.runFastQuery("DELETE FROM trigger_rotation WHERE item_id = " + this.itemID);
                dbClient.setQuery("INSERT INTO trigger_rotation(item_id,rotation_status,movement_status) VALUES (@id,@rot_id,@mov_id)");
            }
            else
            {
                dbClient.setQuery("REPLACE INTO trigger_rotation SET item_id = @id, rotation_status = @rot_id,  movement_status = @mov_id");
            }
            dbClient.addParameter("id", (int) this.itemID);
            dbClient.addParameter("rot_id", (int) this.rotation);
            dbClient.addParameter("mov_id", (int) this.movement);
            dbClient.runQuery();
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

