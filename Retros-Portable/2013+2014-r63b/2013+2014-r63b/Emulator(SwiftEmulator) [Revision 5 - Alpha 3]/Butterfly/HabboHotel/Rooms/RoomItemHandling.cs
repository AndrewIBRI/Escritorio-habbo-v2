namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly;
    using Butterfly.Collections;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.RoomIvokedItems;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.RoomBots;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using Butterfly.HabboHotel.Pathfinding;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.Messages;
    using Butterfly.Util;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    

    internal class RoomItemHandling
    {
        private Hashtable mAddedItems;
        internal QueuedDictionary<uint, RoomItem> mFloorItems;
        private bool mGotRollers;
        private Hashtable mMovedItems;
        private Hashtable mRemovedItems;
        internal QueuedDictionary<uint, RoomItem> mRollers;
        private int mRollerSpeed;
        private int mRoolerCycle;
        internal QueuedDictionary<uint, RoomItem> mWallItems;
        private List<uint> rollerItemsMoved;
        private List<ServerMessage> rollerMessages;
        private List<uint> rollerUsersMoved;
        private Room room;
        private Queue roomItemUpdateQueue;

        public RoomItemHandling(Room room)
        {
            this.room = room;
            this.mRemovedItems = new Hashtable();
            this.mMovedItems = new Hashtable();
            this.mAddedItems = new Hashtable();
            this.mRollers = new QueuedDictionary<uint, RoomItem>();
            this.mWallItems = new QueuedDictionary<uint, RoomItem>();
            this.mFloorItems = new QueuedDictionary<uint, RoomItem>();
            this.roomItemUpdateQueue = new Queue();
            this.mGotRollers = false;
            this.mRoolerCycle = 0;
            this.mRollerSpeed = 4;
            this.rollerItemsMoved = new List<uint>();
            this.rollerUsersMoved = new List<uint>();
            this.rollerMessages = new List<ServerMessage>();
        }

        internal void AddItem(RoomItem item)
        {
            if (this.mRemovedItems.ContainsKey(item.Id))
            {
                this.mRemovedItems.Remove(item.Id);
            }
            if (!(this.mMovedItems.ContainsKey(item.Id) || this.mAddedItems.ContainsKey(item.Id)))
            {
                this.mAddedItems.Add(item.Id, item);
            }
        }

        private void ClearRollers()
        {
            this.mRollers.Clear();
        }

        private List<ServerMessage> CycleRollers()
        {
            this.mRollers.OnCycle();
            if (this.mGotRollers)
            {
                if ((this.mRoolerCycle >= this.mRollerSpeed) || (this.mRollerSpeed == 0))
                {
                    this.rollerItemsMoved.Clear();
                    this.rollerUsersMoved.Clear();
                    this.rollerMessages.Clear();
                    foreach (RoomItem item in this.mRollers.Values)
                    {
                        Point squareInFront = item.SquareInFront;
                        List<RoomItem> list = this.room.GetGameMap().GetRoomItemForSquare(item.GetX, item.GetY, item.GetZ);
                        RoomUser userForSquare = this.room.GetRoomUserManager().GetUserForSquare(item.GetX, item.GetY);
                        if ((list.Count > 0) || (userForSquare != null))
                        {
                            List<RoomItem> coordinatedItems = this.room.GetGameMap().GetCoordinatedItems(squareInFront);
                            double nextZ = 0.0;
                            int num2 = 0;
                            bool flag = false;
                            double totalHeight = 0.0;
                            bool flag2 = true;
                            foreach (RoomItem item2 in coordinatedItems)
                            {
                                if (item2.IsRoller)
                                {
                                    flag = true;
                                    if (item2.TotalHeight > totalHeight)
                                    {
                                        totalHeight = item2.TotalHeight;
                                    }
                                }
                            }
                            if (flag)
                            {
                                foreach (RoomItem item2 in coordinatedItems)
                                {
                                    if (item2.TotalHeight > totalHeight)
                                    {
                                        flag2 = false;
                                    }
                                }
                            }
                            else
                            {
                                totalHeight += this.room.GetGameMap().GetHeightForSquareFromData(squareInFront);
                            }
                            nextZ = totalHeight;
                            bool flag3 = num2 > 0;
                            if (this.room.GetRoomUserManager().GetUserForSquare(squareInFront.X, squareInFront.Y) != null)
                            {
                                flag3 = true;
                            }
                            foreach (RoomItem item2 in list)
                            {
                                double num4 = item2.GetZ - item.TotalHeight;
                                if (((!this.rollerItemsMoved.Contains(item2.Id) && this.room.GetGameMap().CanRollItemHere(squareInFront.X, squareInFront.Y)) && (flag2 && (item.GetZ < item2.GetZ))) && (this.room.GetRoomUserManager().GetUserForSquare(squareInFront.X, squareInFront.Y) == null))
                                {
                                    this.rollerMessages.Add(this.UpdateItemOnRoller(item2, squareInFront, item.Id, totalHeight + num4));
                                    this.rollerItemsMoved.Add(item2.Id);
                                }
                            }
                            if ((((((userForSquare != null) && !userForSquare.IsWalking) && (flag2 && !flag3)) && this.room.GetGameMap().CanRollItemHere(squareInFront.X, squareInFront.Y)) && (this.room.GetGameMap().GetFloorStatus(squareInFront) != 0)) && !this.rollerUsersMoved.Contains(userForSquare.HabboId))
                            {
                                this.rollerMessages.Add(this.UpdateUserOnRoller(userForSquare, squareInFront, item.Id, nextZ));
                                this.rollerUsersMoved.Add(userForSquare.HabboId);
                            }
                        }
                    }
                    this.mRoolerCycle = 0;
                    return this.rollerMessages;
                }
                this.mRoolerCycle++;
            }
            return new List<ServerMessage>();
        }

        internal void Destroy()
        {
            this.mFloorItems.Clear();
            this.mWallItems.Clear();
            this.mRemovedItems.Clear();
            this.mMovedItems.Clear();
            this.mAddedItems.Clear();
            this.roomItemUpdateQueue.Clear();
            this.room = null;
            this.mFloorItems = null;
            this.mWallItems = null;
            this.mRemovedItems = null;
            this.mMovedItems = null;
            this.mAddedItems = null;
            this.mWallItems = null;
            this.roomItemUpdateQueue = null;
        }

        internal RoomItem GetFurniObject(int X, int Y)
        {
            foreach (RoomItem item in this.room.GetRoomItemHandler().mFloorItems.Values)
            {
                if ((item.GetX == X) && (item.GetY == Y))
                {
                    return item;
                }
            }
            return null;
        }

        internal List<RoomItem> GetFurniObjects(int X, int Y)
        {
            return this.room.GetGameMap().GetCoordinatedItems(new Point(X, Y));
        }

        internal RoomItem GetItem(uint pId)
        {
            if (this.mFloorItems.ContainsKey(pId))
            {
                return this.mFloorItems.GetValue(pId);
            }
            if (this.mWallItems.ContainsKey(pId))
            {
                return this.mWallItems.GetValue(pId);
            }
            return null;
        }

        internal void LoadFurniture()
        {
            this.mFloorItems.Clear();
            this.mWallItems.Clear();
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MySQL)
                {
                    adapter.setQuery("CALL getroomitems(@roomid)");
                    adapter.addParameter("roomid", this.room.RoomId);
                }
                else
                {
                    adapter.setQuery("EXECUTE getroomitems " + this.room.RoomId);
                }
                DataTable table = adapter.getTable();
                foreach (DataRow row in table.Rows)
                {
                    string str;
                    RoomItem item;
                    uint id = Convert.ToUInt32(row[0]);
                    decimal num2 = Convert.ToDecimal(row[1]);
                    decimal num3 = Convert.ToDecimal(row[2]);
                    sbyte n = Convert.ToSByte(row[3]);
                    uint baseItem = Convert.ToUInt32(row[4]);
                    if (DBNull.Value.Equals(row[5]))
                    {
                        str = string.Empty;
                    }
                    else
                    {
                        str = (string) row[5];
                        
                    }
                    uint placedBy = Convert.ToUInt32(row[8]);
                    uint rareid = Convert.ToUInt32(row[6]);
                    string str2 = (string) row[7];
                    
                    if (n > 6)
                    {
                        WallCoordinate wallCoord = new WallCoordinate((double) num2, (double) num3, n);
                        item = new RoomItem(id, this.room.RoomId, baseItem, str, wallCoord, this.room, str2, rareid, placedBy);
                        if (!this.mWallItems.ContainsKey(id))
                        {
                            this.mWallItems.Inner.Add(id, item);
                        }
                    }
                    else
                    {
                        int num7;
                        int num8;
                        TextHandling.Split((double) num2, out num7, out num8);
                        item = new RoomItem(id, this.room.RoomId, baseItem, str, num7, num8, (double) num3, n, this.room, str2, rareid, placedBy);
                        if ((item.Id != 0) && !this.mFloorItems.ContainsKey(id))
                        {
                            this.mFloorItems.Inner.Add(id, item);
                        }
                    }
                }
                foreach (RoomItem item2 in this.mFloorItems.Values)
                {
                    if (item2.IsRoller)
                    {
                        this.mGotRollers = true;
                    }
                    else if (item2.GetBaseItem().InteractionType == InteractionType.dimmer)
                    {
                        if (this.room.MoodlightData == null)
                        {
                            this.room.MoodlightData = new MoodlightData(item2.Id);
                        }
                    }
                    else if (WiredUtillity.TypeIsWired(item2.GetBaseItem().InteractionType))
                    {
                        WiredLoader.LoadWiredItem(item2, this.room, adapter);
                        this.room.GetWiredHandler().AddWire(item2, item2.Coordinate, item2.Rot, item2.GetBaseItem().InteractionType);
                    }
                }

                
            }
        }

        internal void OnCycle()
        {
            if (this.mGotRollers)
            {
                try
                {
                    this.room.SendMessage(this.CycleRollers());
                }
                catch (Exception exception)
                {
                    Logging.LogThreadException(exception.ToString(), "rollers for room with ID " + this.room.RoomId);
                    this.mGotRollers = false;
                }
            }
            if (this.roomItemUpdateQueue.Count > 0)
            {
                List<RoomItem> list = new List<RoomItem>();
                lock (this.roomItemUpdateQueue.SyncRoot)
                {
                    while (this.roomItemUpdateQueue.Count > 0)
                    {
                        RoomItem item = (RoomItem) this.roomItemUpdateQueue.Dequeue();
                        item.ProcessUpdates();
                        if (item.IsTrans || (item.UpdateCounter > 0))
                        {
                            list.Add(item);
                        }
                    }
                    foreach (RoomItem item in list)
                    {
                        this.roomItemUpdateQueue.Enqueue(item);
                    }
                }
            }

            this.mFloorItems.OnCycle();
            this.mWallItems.OnCycle();
        }

        internal void QueueRoomItemUpdate(RoomItem item)
        {
            lock (this.roomItemUpdateQueue.SyncRoot)
            {
                this.roomItemUpdateQueue.Enqueue(item);
            }
        }
        internal void RemoveItemsByOwner(ref List<RoomItem> roomItemList, ref GameClient session)
        {
            foreach (RoomItem item in roomItemList)
            {
                if (item.placedBy == 0)
                    item.placedBy = session.GetHabbo().Id;

                Habbo habbo = ButterflyEnvironment.getHabboForId(item.placedBy);
                uint num = habbo.Id;

                if ((item != null) && (item.GetBaseItem().InteractionType != InteractionType.postit))
                {
                    if (habbo.GetClient() == null)
                    {
                        IQueryAdapter adapter;
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            if (adapter.dbType == DatabaseType.MSSQL)
                            {
                                adapter.setQuery("DELETE FROM items_users WHERE item_id = " + item.Id);
                                adapter.setQuery(string.Concat(new object[] { "INSERT INTO items_users VALUES (", item.Id, ",", item.placedBy, ")" }));
                            }
                            else
                            {
                                adapter.runFastQuery(string.Concat(new object[] { "REPLACE INTO items_users VALUES (", item.Id, ",", item.placedBy, ")" }));
                            }
                        }

                        this.RemoveFurniture(session, item.Id);
                    }

                    else
                    {
                        this.RemoveFurniture(session, item.Id);
                        this.RemoveFurniture(habbo.GetClient(), item.Id);
                        habbo.GetInventoryComponent().AddNewItem(item.Id, item.BaseItem, item.ExtraData, true, true, item.RareId, item.Guilds_Data_String, habbo.Id, 0);
                        habbo.GetInventoryComponent().UpdateItems(false);

                    }
                }
            }
        }

        internal List<RoomItem> RemoveAllFurniture(GameClient Session)
        {
            ServerMessage message;

            List<RoomItem> list = new List<RoomItem>();

            foreach (RoomItem item in this.mFloorItems.Values.ToArray<RoomItem>())
            {
                item.Interactor.OnRemove(Session, item);
                message = new ServerMessage(Outgoing.PickUpFloorItem);
                message.AppendString(item.Id + string.Empty);
                message.AppendInt32(0);
                message.AppendInt32(item.placedBy);
                this.room.SendMessage(message);
                list.Add(item);                
            }

            foreach (RoomItem item in this.mWallItems.Values.ToArray<RoomItem>())
            {
                item.Interactor.OnRemove(Session, item);
                message = new ServerMessage(Outgoing.PickUpWallItem);
                message.AppendString(item.Id + string.Empty);
                message.AppendInt32(this.room.OwnerId);
                this.room.SendMessage(message);
                list.Add(item);
            }
            
            this.mWallItems.Clear();
            this.mFloorItems.Clear();
            this.mRemovedItems.Clear();
            this.mMovedItems.Clear();
            this.mAddedItems.Clear();            

            this.mRollers.QueueDelegate(new onCycleDoneDelegate(this.ClearRollers));
            
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("DELETE FROM items_rooms WHERE room_id = " + this.room.RoomId);
            }
            
            this.room.GetGameMap().GenerateMaps(true);
            this.room.GetRoomUserManager().UpdateUserStatusses();
            
            if (this.room.GotWired())
            {
                this.room.GetWiredHandler().OnPickall();
            }
            
            return list;
        }

        internal void RemoveFurniture(GameClient Session, uint pId)
        {
            RoomItem item = this.GetItem(pId);
            if (item != null)
            {
                IQueryAdapter adapter;
                
                if (item.GetBaseItem().InteractionType == InteractionType.fbgate)
                {
                    this.room.GetSoccer().UnRegisterGate(item);
                }
                
                if (item.GetBaseItem().InteractionType != InteractionType.gift)
                {
                    item.Interactor.OnRemove(Session, item);
                }
                
                this.RemoveRoomItem(item);
                
                if (item.wiredHandler != null)
                {
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        item.wiredHandler.DeleteFromDatabase(adapter);
                        item.wiredHandler.Dispose();
                        this.room.GetWiredHandler().RemoveFurniture(item);
                    }
                    item.wiredHandler = null;
                }
                
                if (item.wiredCondition != null)
                {
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        item.wiredCondition.DeleteFromDatabase(adapter);
                        item.wiredCondition.Dispose();
                        this.room.GetWiredHandler().conditionHandler.ClearTile(item.Coordinate);
                    }
                    item.wiredCondition = null;
                }
            }
        }

        internal void RemoveItem(RoomItem item)
        {
            if (this.mAddedItems.ContainsKey(item.Id))
            {
                this.mAddedItems.Remove(item.Id);
            }
            if (this.mMovedItems.ContainsKey(item.Id))
            {
                this.mMovedItems.Remove(item.Id);
            }
            if (!this.mRemovedItems.ContainsKey(item.Id))
            {
                this.mRemovedItems.Add(item.Id, item);
            }
            this.mRollers.Remove(item.Id);
        }

        private void RemoveRoomItem(RoomItem Item)
        {
            ServerMessage message;
            if (Item.IsWallItem)
            {
                message = new ServerMessage(Outgoing.PickUpWallItem);
                message.AppendString(Item.Id + string.Empty);
                message.AppendInt32(Item.placedBy);
                this.room.SendMessage(message);
            }

            else if (Item.IsFloorItem)
            {
                message = new ServerMessage(Outgoing.PickUpFloorItem);
                message.AppendString(Item.Id + string.Empty);
                message.AppendInt32(0);
                message.AppendInt32(Item.placedBy);
                this.room.SendMessage(message);
            }

            if (Item.IsWallItem)
            {
                this.mWallItems.Remove(Item.Id);
            }

            else
            {
                this.mFloorItems.Remove(Item.Id);
                this.mFloorItems.OnCycle();
                this.room.GetGameMap().RemoveFromMap(Item);
            }

            this.RemoveItem(Item);
            this.room.GetRoomUserManager().UpdateUserStatusses();
            
            if (WiredHandler.TypeIsWire(Item.GetBaseItem().InteractionType))
            {
                this.room.GetWiredHandler().RemoveWiredItem(Item.Coordinate);
            }
        }

        internal void SaveFurniture(IQueryAdapter dbClient)
        {
            if (dbClient.dbType == DatabaseType.MSSQL)
            {
                this.SaveFurnitureToMSSQL(dbClient);
            }
            else
            {
                try
                {
                    if ((((this.mAddedItems.Count > 0) || (this.mRemovedItems.Count > 0)) || (this.mMovedItems.Count > 0)) || (this.room.GetRoomUserManager().PetCount > 0))
                    {
                        double num;
                        QueryChunk chunk = new QueryChunk();
                        QueryChunk chunk2 = new QueryChunk("REPLACE INTO items_rooms (item_id,room_id,x,y,n) VALUES ");
                        QueryChunk chunk3 = new QueryChunk("REPLACE INTO items_extradata (item_id,data) VALUES ");
                        foreach (RoomItem item in this.mRemovedItems.Values)
                        {
                            chunk.AddQuery(string.Concat(new object[] { "DELETE FROM items_rooms WHERE item_id = ", item.Id, " AND room_id = ", this.room.RoomId }));
                        }
                        if (this.mAddedItems.Count > 0)
                        {
                            foreach (RoomItem item in this.mAddedItems.Values)
                            {
                                if (!string.IsNullOrEmpty(item.ExtraData))
                                {
                                    chunk3.AddQuery(string.Concat(new object[] { "(", item.Id, ",@data_id", item.Id, ")" }));
                                    chunk3.AddParameter("@data_id" + item.Id, item.ExtraData);
                                }
                                if (item.IsFloorItem)
                                {
                                    num = TextHandling.Combine(item.GetX, item.GetY);
                                    chunk2.AddQuery(string.Concat(new object[] { "(", item.Id, ",", item.RoomId, ",", TextHandling.GetString(num), ",", TextHandling.GetString(item.GetZ), ",", item.Rot, ")" }));
                                }
                                else
                                {
                                    chunk2.AddQuery(string.Concat(new object[] { "(", item.Id, ",", item.RoomId, ",", TextHandling.GetString(item.wallCoord.GetXValue()), ",", TextHandling.GetString(item.wallCoord.GetYValue()), ",", item.wallCoord.n(), ")" }));
                                }
                            }
                        }
                        foreach (RoomItem item in this.mMovedItems.Values)
                        {
                            if (!string.IsNullOrEmpty(item.ExtraData))
                            {
                                chunk.AddQuery(string.Concat(new object[] { "UPDATE items_extradata SET data = @data", item.Id, " WHERE item_id = ", item.Id }));
                                chunk.AddParameter("data" + item.Id, item.ExtraData);
                            }
                            if (item.IsWallItem)
                            {
                                chunk.AddQuery(string.Concat(new object[] { "UPDATE items_rooms SET x=", TextHandling.GetString(item.wallCoord.GetXValue()), ", y=", TextHandling.GetString(item.wallCoord.GetYValue()), ", n=", item.wallCoord.n(), " WHERE item_id = ", item.Id }));
                            }
                            else
                            {
                                num = TextHandling.Combine(item.GetX, item.GetY);
                                chunk.AddQuery(string.Concat(new object[] { "UPDATE items_rooms SET x=", TextHandling.GetString(num), ", y=", TextHandling.GetString(item.GetZ), ", n=", item.Rot, " WHERE item_id = ", item.Id }));
                            }
                        }
                        this.room.GetRoomUserManager().AppendPetsUpdateString(dbClient);
                        this.mAddedItems.Clear();
                        this.mRemovedItems.Clear();
                        this.mMovedItems.Clear();
                        chunk.Execute(dbClient);
                        chunk2.Execute(dbClient);
                        chunk3.Execute(dbClient);
                        chunk.Dispose();
                        chunk2.Dispose();
                        chunk3.Dispose();
                        chunk = null;
                        chunk2 = null;
                        chunk3 = null;
                    }
                }
                catch (Exception exception)
                {
                    Logging.LogCriticalException(string.Concat(new object[] { "Error during saving furniture for room ", this.room.RoomId, ". Stack: ", exception.ToString() }));
                }
            }
        }

        internal void SaveFurnitureToMSSQL(IQueryAdapter dbClient)
        {
            try
            {
                if ((((this.mAddedItems.Count > 0) || (this.mRemovedItems.Count > 0)) || (this.mMovedItems.Count > 0)) || (this.room.GetRoomUserManager().PetCount > 0))
                {
                    double num;
                    QueryChunk chunk = new QueryChunk();
                    QueryChunk chunk2 = new QueryChunk();
                    QueryChunk chunk3 = new QueryChunk();
                    foreach (RoomItem item in this.mRemovedItems.Values)
                    {
                        chunk.AddQuery(string.Concat(new object[] { "DELETE FROM items_rooms WHERE item_id = ", item.Id, " AND room_id = ", this.room.RoomId }));
                    }
                    if (this.mAddedItems.Count > 0)
                    {
                        foreach (RoomItem item in this.mAddedItems.Values)
                        {
                            if (!string.IsNullOrEmpty(item.ExtraData))
                            {
                                chunk3.AddQuery("DELETE FROM items_extradata WHERE item_id = " + item.Id);
                                chunk3.AddQuery(string.Concat(new object[] { "INSERT INTO items_extradata (item_id,data) VALUES (", item.Id, ",@data_id", item.Id, ")" }));
                                chunk3.AddParameter("@data_id" + item.Id, item.ExtraData);
                            }
                            chunk2.AddQuery("DELETE FROM items_rooms WHERE item_id = " + item.Id);
                            if (item.IsFloorItem)
                            {
                                num = TextHandling.Combine(item.GetX, item.GetY);
                                chunk2.AddQuery(string.Concat(new object[] { "INSERT INTO items_rooms (item_id,room_id,x,y,n) VALUES (", item.Id, ",", item.RoomId, ",", TextHandling.GetString(num), ",", TextHandling.GetString(item.GetZ), ",", item.Rot, ")" }));
                            }
                            else
                            {
                                chunk2.AddQuery(string.Concat(new object[] { "INSERT INTO items_rooms (item_id,room_id,x,y,n) VALUES (", item.Id, ",", item.RoomId, ",", TextHandling.GetString(item.wallCoord.GetXValue()), ",", TextHandling.GetString(item.wallCoord.GetYValue()), ",", item.wallCoord.n(), ")" }));
                            }
                        }
                    }
                    foreach (RoomItem item in this.mMovedItems.Values)
                    {
                        if (!string.IsNullOrEmpty(item.ExtraData))
                        {
                            chunk.AddQuery(string.Concat(new object[] { "UPDATE items_extradata SET data = @data", item.Id, " WHERE item_id = ", item.Id }));
                            chunk.AddParameter("data" + item.Id, item.ExtraData);
                        }
                        if (item.IsWallItem)
                        {
                            chunk.AddQuery(string.Concat(new object[] { "UPDATE items_rooms SET x=", TextHandling.GetString(item.wallCoord.GetXValue()), ", y=", TextHandling.GetString(item.wallCoord.GetYValue()), ", n=", item.wallCoord.n(), " WHERE item_id = ", item.Id }));
                        }
                        else
                        {
                            num = TextHandling.Combine(item.GetX, item.GetY);
                            chunk.AddQuery(string.Concat(new object[] { "UPDATE items_rooms SET x=", TextHandling.GetString(num), ", y=", TextHandling.GetString(item.GetZ), ", n=", item.Rot, " WHERE item_id = ", item.Id }));
                        }
                    }
                    this.room.GetRoomUserManager().AppendPetsUpdateString(dbClient);
                    this.mAddedItems.Clear();
                    this.mRemovedItems.Clear();
                    this.mMovedItems.Clear();
                    chunk.Execute(dbClient);
                    chunk2.Execute(dbClient);
                    chunk3.Execute(dbClient);
                    chunk.Dispose();
                    chunk2.Dispose();
                    chunk3.Dispose();
                    chunk = null;
                    chunk2 = null;
                    chunk3 = null;
                }
            }
            catch (Exception exception)
            {
                Logging.LogCriticalException(string.Concat(new object[] { "Error during saving furniture for room ", this.room.RoomId, ". Stack: ", exception.ToString() }));
            }
        }

        internal bool SetFloorItem(RoomItem Item, int newX, int newY, double newZ)
        {
            this.room.GetGameMap().RemoveFromMap(Item);
            Item.SetState(newX, newY, newZ, Gamemap.GetAffectedTiles(Item.GetBaseItem().Length, Item.GetBaseItem().Width, newX, newY, Item.Rot));
            this.UpdateItem(Item);
            this.room.GetGameMap().AddItemToMap(Item);
            return true;
        }

        internal bool SetFloorItem(GameClient Session, RoomItem Item, int newX, int newY, int newRot, bool newItem, bool OnRoller, bool sendMessage)
        {
            return this.SetFloorItem(Session, Item, newX, newY, newRot, newItem, OnRoller, sendMessage, true);
        }

        internal bool SetFloorItem(GameClient Session, RoomItem Item, int newX, int newY, int newRot, bool newItem, bool OnRoller, bool sendMessage, bool updateRoomUserStatuses)
        {
            ServerMessage message;
            bool flag = false;
            if (!newItem)
            {
                flag = this.room.GetGameMap().RemoveFromMap(Item);
            }
            Dictionary<int, ThreeDCoord> tiles = Gamemap.GetAffectedTiles(Item.GetBaseItem().Length, Item.GetBaseItem().Width, newX, newY, newRot);
            if (!this.room.GetGameMap().ValidTile(newX, newY) || (this.room.GetGameMap().SquareHasUsers(newX, newY) && !Item.GetBaseItem().IsSeat))
            {
                if (flag)
                {
                    this.AddItem(Item);
                    this.room.GetGameMap().AddToMap(Item);
                }
                return false;
            }
            foreach (ThreeDCoord coord in tiles.Values)
            {
                if (!this.room.GetGameMap().ValidTile(coord.X, coord.Y) || (this.room.GetGameMap().SquareHasUsers(coord.X, coord.Y) && !Item.GetBaseItem().IsSeat))
                {
                    if (flag)
                    {
                        this.AddItem(Item);
                        this.room.GetGameMap().AddToMap(Item);
                    }
                    return false;
                }
            }
            double pZ = this.room.GetGameMap().Model.SqFloorHeight[newX, newY];
            if (!OnRoller)
            {
                if ((this.room.GetGameMap().Model.SqState[newX, newY] != SquareState.OPEN) && !Item.GetBaseItem().IsSeat)
                {
                    if (flag)
                    {
                        this.AddItem(Item);
                        this.room.GetGameMap().AddToMap(Item);
                    }
                    return false;
                }
                foreach (ThreeDCoord coord in tiles.Values)
                {
                    if ((this.room.GetGameMap().Model.SqState[coord.X, coord.Y] != SquareState.OPEN) && !Item.GetBaseItem().IsSeat)
                    {
                        if (flag)
                        {
                            this.AddItem(Item);
                            this.room.GetGameMap().AddToMap(Item);
                        }
                        return false;
                    }
                }
                if (!Item.GetBaseItem().IsSeat && !Item.IsRoller)
                {
                    foreach (ThreeDCoord coord in tiles.Values)
                    {
                        if (this.room.GetGameMap().GetRoomUsers(new Point(coord.X, coord.Y)).Count > 0)
                        {
                            if (flag)
                            {
                                this.AddItem(Item);
                                this.room.GetGameMap().AddToMap(Item);
                            }
                            return false;
                        }
                    }
                }
            }
            List<RoomItem> furniObjects = this.GetFurniObjects(newX, newY);
            List<RoomItem> collection = new List<RoomItem>();
            List<RoomItem> list3 = new List<RoomItem>();
            foreach (ThreeDCoord coord in tiles.Values)
            {
                List<RoomItem> list4 = this.GetFurniObjects(coord.X, coord.Y);
                if (list4 != null)
                {
                    collection.AddRange(list4);
                }
            }
            list3.AddRange(furniObjects);
            list3.AddRange(collection);
            
            if (!OnRoller)
            {
                foreach (RoomItem item in list3)
                {
                    if ((((item != null) && (item.Id != Item.Id)) && (item.GetBaseItem() != null)) && !item.GetBaseItem().Stackable)
                    {
                        if (flag)
                        {
                            this.AddItem(Item);
                            this.room.GetGameMap().AddToMap(Item);
                         }
                        return false;
                    }
                }
            }
            if (((Item.Rot != newRot) && (Item.GetX == newX)) && (Item.GetY == newY))
            {
                if (Item.GetBaseItem().InteractionType == InteractionType.gld_wall)
                {
                    newRot++;
                }
                pZ = Item.GetZ;

            }
            foreach (RoomItem item in list3)
            {
                if ((item.Id != Item.Id) && (item.TotalHeight > pZ))
                {
                    pZ = item.TotalHeight;
                }
            }
            if (Item.GetBaseItem().Name != "boutique_mannequin1")
            {
                if (newRot != 0 && newRot != 2 && newRot != 4 && newRot != 6 && newRot != 8)
                {
                    newRot = 0;
                }
            }
            else
            {
                if (newRot != 0 && newRot != 1 && newRot != 2 && newRot != 3 && newRot != 4 && newRot != 5 && newRot != 6 && newRot != 7 && newRot != 8 && newRot != 9 && newRot != 10 && newRot != 11 && newRot != 12) 
                {
                    newRot = 0;
                }
            }
            
            Item.Rot = newRot;
            int getX = Item.GetX;
            int getY = Item.GetY;
            Item.SetState(newX, newY, pZ, tiles);
            if (!(OnRoller || (Session == null)))
            {
                Item.Interactor.OnPlace(Session, Item);
            }
            if (newItem)
            {
                if (this.mFloorItems.ContainsKey(Item.Id))
                {
                    if (Session != null)
                    {
                        Session.SendNotif(LanguageLocale.GetValue("room.itemplaced"));
                    }
                    return true;
                }
                if (!(!Item.IsFloorItem || this.mFloorItems.ContainsKey(Item.Id)))
                {
                    this.mFloorItems.Add(Item.Id, Item);
                }
                else if (!(!Item.IsWallItem || this.mWallItems.ContainsKey(Item.Id)))
                {
                    this.mWallItems.Add(Item.Id, Item);
                }
                this.AddItem(Item);
                if (sendMessage)
                {
                    message = new ServerMessage(Outgoing.AddFloorItemToRoom);
                    Item.Serialize(message, (int)Item.placedBy);
                    message.AppendString(ButterflyEnvironment.GetGame().GetClientManager().GetNameById(Item.placedBy));
                    this.room.SendMessage(message);
                    Console.WriteLine(message.ToString());
                }
            }
            else
            {
                this.UpdateItem(Item);
                if (!(OnRoller || !sendMessage))
                {
                    message = new ServerMessage(Outgoing.UpdateItemOnRoom);
                    Item.Serialize(message, (int)Item.placedBy);
                    this.room.SendMessage(message);
                }
            }

            if (!newItem)
            {
                this.room.GetWiredHandler().RemoveWiredItem(new Point(getX, getY));
                if (WiredHandler.TypeIsWire(Item.GetBaseItem().InteractionType))
                {
                    this.room.GetWiredHandler().AddWire(Item, new Point(newX, newY), newRot, Item.GetBaseItem().InteractionType);
                }
            }

            else if (WiredHandler.TypeIsWire(Item.GetBaseItem().InteractionType))
            {
                this.room.GetWiredHandler().AddWire(Item, Item.Coordinate, newRot, Item.GetBaseItem().InteractionType);
            }
            this.room.GetGameMap().AddToMap(Item);
            if (Item.GetBaseItem().IsSeat)
            {
                updateRoomUserStatuses = true;
            }
            if (updateRoomUserStatuses)
            {
                this.room.GetRoomUserManager().UpdateUserStatusses();
            }
            if ((Item.GetBaseItem().InteractionType == InteractionType.obstacle_h) && ((Item.Rot == 2) || (Item.Rot == 6)))
            {
                if ((Item.GetX + 1) <= this.room.GetGameMap().StaticModel.MapSizeX)
                {
                    Item.SetState(Item.GetX + 1, Item.GetY, Item.GetZ, tiles);
                }
                else if (((Item.Rot == 0) || (Item.Rot == 4)) && ((Item.GetY + 1) <= this.room.GetGameMap().StaticModel.MapSizeY))
                {
                    Item.SetState(Item.GetX, Item.GetY + 1, Item.GetZ, tiles);
                }
            }
            return true;
        }

        internal void SetSpeed(int p)
        {
            this.mRollerSpeed = p;
        }

        internal bool SetWallItem(GameClient Session, RoomItem Item)
        {
            if (!(Item.IsWallItem && !this.mWallItems.ContainsKey(Item.Id)))
            {
                return false;
            }
            if (this.mFloorItems.ContainsKey(Item.Id))
            {
                Session.SendNotif(LanguageLocale.GetValue("room.itemplaced"));
                return true;
            }
            Item.Interactor.OnPlace(Session, Item);
            if ((Item.GetBaseItem().InteractionType == InteractionType.dimmer) && (this.room.MoodlightData == null))
            {
                this.room.MoodlightData = new MoodlightData(Item.Id);
                Item.ExtraData = this.room.MoodlightData.GenerateExtraData();
            }
            this.mWallItems.Add(Item.Id, Item);
            this.AddItem(Item);
            ServerMessage message = new ServerMessage(Outgoing.AddWallItemToRoom);
            Item.Serialize(message, (int)Item.placedBy);
            message.AppendString(ButterflyEnvironment.GetGame().GetClientManager().GetNameById(Item.placedBy));
            this.room.SendMessage(message);
            return true;
        }

        internal void UpdateItem(RoomItem item)
        {
            if (!this.mAddedItems.ContainsKey(item.Id))
            {
                if (this.mRemovedItems.ContainsKey(item.Id))
                {
                    this.mRemovedItems.Remove(item.Id);
                }
                if (!this.mMovedItems.ContainsKey(item.Id))
                {
                    this.mMovedItems.Add(item.Id, item);
                }
            }
        }

        private ServerMessage UpdateItemOnRoller(RoomItem pItem, Point NextCoord, uint pRolledID, double NextZ)
        {
            ServerMessage message = new ServerMessage();
            message.Init(Outgoing.ObjectOnRoller);
            message.AppendInt32(pItem.GetX);
            message.AppendInt32(pItem.GetY);
            message.AppendInt32(NextCoord.X);
            message.AppendInt32(NextCoord.Y);
            message.AppendInt32(1);
            message.AppendInt32(pItem.Id);
            message.AppendString(TextHandling.GetString(pItem.GetZ));
            message.AppendString(TextHandling.GetString(NextZ));
            message.AppendInt32(pRolledID);
            this.SetFloorItem(pItem, NextCoord.X, NextCoord.Y, NextZ);
            return message;
        }

        internal ServerMessage UpdateUserOnRoller(RoomUser pUser, Point pNextCoord, uint pRollerID, double NextZ)
        {
            ServerMessage message = new ServerMessage(Outgoing.ObjectOnRoller);
            message.AppendInt32(pUser.X);
            message.AppendInt32(pUser.Y);
            message.AppendInt32(pNextCoord.X);
            message.AppendInt32(pNextCoord.Y);
            message.AppendInt32(0);
            message.AppendInt32(pRollerID);
            message.AppendInt32(2);
            message.AppendInt32(pUser.VirtualId);
            message.AppendString(TextHandling.GetString(pUser.Z));
            message.AppendString(TextHandling.GetString(NextZ));
            this.room.GetGameMap().UpdateUserMovement(new Point(pUser.X, pUser.Y), new Point(pNextCoord.X, pNextCoord.Y), pUser);
            this.room.GetGameMap().GameMap[pUser.X, pUser.Y] = 1;
            pUser.X = pNextCoord.X;
            pUser.Y = pNextCoord.Y;
            pUser.Z = NextZ;
            this.room.GetGameMap().GameMap[pUser.X, pUser.Y] = 0;
            return message;
        }

        internal bool GotRollers
        {
            get
            {
                return this.mGotRollers;
            }
            set
            {
                this.mGotRollers = value;
            }
        }
    }
}

