namespace Butterfly.HabboHotel.Users.Inventory
{
    using Butterfly;
    using Butterfly.Collections;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Catalogs;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.RoomIvokedItems;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.RoomBots;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using Butterfly.Messages;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    internal class InventoryComponent
    {
        private Hashtable discs;
        private Hashtable floorItems;
        private SafeDictionary<UInt32, RoomBot> InventoryBots;
        private SafeDictionary<uint, Pet> InventoryPets;
        private bool isUpdated;
        private Hashtable mAddedItems;
        private GameClient mClient;
        private ArrayList mRemovedItems;
        private bool userAttatched = false;
        internal uint UserId;
        private Hashtable wallItems;

        internal InventoryComponent(uint UserId, GameClient Client, UserData UserData)
        {
            this.mClient = Client;
            this.UserId = UserId;
            this.floorItems = new Hashtable();
            this.wallItems = new Hashtable();
            this.discs = new Hashtable();
            foreach (UserItem item in UserData.inventory)
            {
                if (item.GetBaseItem().InteractionType == InteractionType.musicdisc)
                {
                    this.discs.Add(item.Id, item);
                }
                if (item.isWallItem)
                {
                    this.wallItems.Add(item.Id, item);
                }
                else
                {
                    this.floorItems.Add(item.Id, item);
                }
            }
            this.InventoryPets = new SafeDictionary<uint, Pet>(UserData.pets);
            this.InventoryBots = new SafeDictionary<UInt32, RoomBot>(UserData.bots);
            this.mAddedItems = new Hashtable();
            this.mRemovedItems = new ArrayList();
            this.isUpdated = false;
        }

        internal void AddItem(RoomItem item)
        {
            this.AddNewItem(item.Id, item.BaseItem, item.ExtraData, true, true, item.RareId, item.Guilds_Data_String,item.placedBy, 0);
        }
        internal RoomBot GetBot(uint Id)
        {
            if (InventoryBots.ContainsKey(Id))
                return InventoryBots[Id] as RoomBot;
            return null;
        }
        internal ServerMessage SerializeBotInventory()
        {
            ServerMessage Message = new ServerMessage(Outgoing.BotInventory);
           
            Message.AppendInt32(InventoryBots.Count);

            foreach (RoomBot Bot in InventoryBots.Values)
            {
                Bot.SerializeInventory(Message);
            }
           
            return Message;
        }
        internal bool RemoveBot(uint PetId)
        {
            isUpdated = false;
            InventoryBots.Remove(PetId);
            return true;
        }

        internal void MoveBotToRoom(UInt32 PetId)
        {
            isUpdated = false;
            RemoveBot(PetId);
        }
        internal void AddBot(RoomBot Bot)
        {
            isUpdated = false;
            if (Bot == null || InventoryBots.ContainsKey(Bot.BotId))
                return;

            //Bot.PlacedInRoom = false;
            Bot.RoomId = 0;

            InventoryBots.Add(Bot.BotId, Bot);
        }

        internal void AddItemArray(List<RoomItem> RoomItemList)
        {
            foreach (RoomItem item in RoomItemList)
            {
                this.AddItem(item);
            }
        }

        internal UserItem AddNewItem(uint Id, uint BaseItem, string ExtraData, bool insert, bool fromRoom, uint RareId, string GroupString,uint placedBy, uint songID = 0)
        {
            this.isUpdated = false;
            if (insert)
            {
                IQueryAdapter adapter;
                if (fromRoom)
                {
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        if (adapter.dbType == DatabaseType.MSSQL)
                        {
                            adapter.setQuery("DELETE FROM items_users WHERE item_id = " + Id);
                            adapter.setQuery(string.Concat(new object[] { "INSERT INTO items_users VALUES (", Id, ",", placedBy, ")" }));
                        }
                        else
                        {
                            adapter.runFastQuery(string.Concat(new object[] { "REPLACE INTO items_users VALUES (", Id, ",", placedBy, ")" }));
                        }
                    }
                    Item item = ButterflyEnvironment.GetGame().GetItemManager().GetItem(BaseItem);
                    if ((item != null) && (item.InteractionType == InteractionType.musicdisc))
                    {
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.runFastQuery("DELETE FROM room_items_songs WHERE item_id = " + Id);
                        }
                    }
                }
                else
                {
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        if (adapter.dbType == DatabaseType.MSSQL)
                        {
                            adapter.setQuery(string.Concat(new object[] { "INSERT INTO items (base_id, rareid, group_data, placedBy) OUTPUT INSERTED.* VALUES (", BaseItem, ",", RareId, ",'", GroupString, "',", placedBy,")" }));
                        }
                        else
                        {
                            adapter.setQuery(string.Concat(new object[] { "INSERT INTO items (base_id, rareid, group_data, placedBy) VALUES (", BaseItem, ",", RareId, ",'", GroupString, "',", placedBy,")" }));
                        }
                        Id = (uint) adapter.insertQuery();
                        if (!string.IsNullOrEmpty(ExtraData))
                        {
                            adapter.setQuery("INSERT INTO items_extradata VALUES (" + Id + ",@extradata)");
                            adapter.addParameter("extradata", ExtraData);
                            adapter.runQuery();
                        }
                        adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO items_users VALUES (", Id, ",", placedBy, ")" }));
                    }
                }
            }
            UserItem item2 = new UserItem(Id, BaseItem, ExtraData, RareId, GroupString, placedBy);
            if (this.UserHoldsItem(Id))
            {
                this.RemoveItem(Id, false);
            }
            if (item2.GetBaseItem().InteractionType == InteractionType.musicdisc)
            {
                this.discs.Add(item2.Id, item2);
            }
            if (item2.isWallItem)
            {
                this.wallItems.Add(item2.Id, item2);
            }
            else
            {
                this.floorItems.Add(item2.Id, item2);
            }
            if (this.mRemovedItems.Contains(Id))
            {
                this.mRemovedItems.Remove(Id);
            }
            if (!this.mAddedItems.ContainsKey(Id))
            {
                this.mAddedItems.Add(Id, item2);
            }
            return item2;
        }

        internal void AddPet(Pet Pet)
        {
            this.isUpdated = false;
            if ((Pet != null) && !this.InventoryPets.ContainsKey(Pet.PetId))
            {
                Pet.PlacedInRoom = false;
                Pet.RoomId = 0;
                this.InventoryPets.Add(Pet.PetId, Pet);

                this.SerializePetInventory();

                
            }
        }

        internal void ClearItems()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("DELETE FROM items_users WHERE user_id = " + this.UserId);
            }
            this.mAddedItems.Clear();
            this.mRemovedItems.Clear();
            this.floorItems.Clear();
            this.wallItems.Clear();
            this.discs.Clear();
            this.InventoryPets.Clear();
            this.isUpdated = true;
            this.mClient.GetMessageHandler().GetResponse().Init(Outgoing.UpdateInventary);
            this.GetClient().GetMessageHandler().SendResponse();
        }

        private GameClient GetClient()
        {
            return ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(this.UserId);
        }

        internal UserItem GetItem(uint Id)
        {
            this.isUpdated = false;
            if (this.floorItems.ContainsKey(Id))
            {
                return (UserItem) this.floorItems[Id];
            }
            if (this.wallItems.ContainsKey(Id))
            {
                return (UserItem) this.wallItems[Id];
            }
            return null;
        }

        internal Pet GetPet(uint Id)
        {
            if (this.InventoryPets.ContainsKey(Id))
            {
                return this.InventoryPets[Id];
            }
            return null;
        }

        internal List<Pet> GetPets()
        {
            List<Pet> list = new List<Pet>();
            foreach (Pet pet in this.InventoryPets.Values)
            {
                list.Add(pet);
            }
            return list;
        }

        internal void LoadInventory()
        {
            DataTable table;
            IQueryAdapter adapter;
            DataTable table2;
            
            this.floorItems.Clear();
            this.wallItems.Clear();
            this.discs.Clear();
            this.InventoryPets.Clear();

            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MySQL)
                {
                    adapter.setQuery("CALL getuseritems(@userid)");
                }
                else
                {
                    adapter.setQuery("EXECUTE getuseritems @userid");
                }
                adapter.addParameter("userid", (int) this.UserId);
                table = adapter.getTable();
            }
            foreach (DataRow row in table.Rows)
            {
                string str;
                uint id = Convert.ToUInt32(row[0]);
                uint baseItem = Convert.ToUInt32(row[1]);
                if (!DBNull.Value.Equals(row[2]))
                {
                    str = (string) row[2];
                }
                else
                {
                    str = string.Empty;
                }
                uint rareid = Convert.ToUInt32(row[3]);
                string groupstring = (string) row[4];
                UserItem item = new UserItem(id, baseItem, str, rareid, groupstring, (uint)this.UserId);
                if (item.GetBaseItem().InteractionType == InteractionType.musicdisc)
                {
                    this.discs.Add(id, item);
                }
                if (item.isWallItem)
                {
                    this.wallItems.Add(id, item);
                }
                else
                {
                    this.floorItems.Add(id, item);
                }
            }
            
            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM user_pets WHERE user_id = " + this.UserId + " AND room_id = 0");
                table2 = adapter.getTable();
            }
            if (table2 != null)
            {
                foreach (DataRow row in table2.Rows)
                {
                    Pet pet = Catalog.GeneratePetFromRow(row);
                    this.InventoryPets.Add(pet.PetId, pet);
                }
            }
        }

        internal void MovePetToRoom(uint PetId)
        {
            this.isUpdated = false;
            this.RemovePet(PetId);
        }

        internal void RemoveItem(uint Id, bool PlacedInroom)
        {
            this.isUpdated = false;
            this.GetClient().GetMessageHandler().GetResponse().Init(Outgoing.RemoveObjectFromInventory);
            this.GetClient().GetMessageHandler().GetResponse().AppendInt32(Id);
            this.GetClient().GetMessageHandler().SendResponse();
            if (this.mAddedItems.ContainsKey(Id))
            {
                this.mAddedItems.Remove(Id);
            }
            if (!this.mRemovedItems.Contains(Id))
            {
                this.discs.Remove(Id);
                this.floorItems.Remove(Id);
                this.wallItems.Remove(Id);
                this.mRemovedItems.Add(Id);
            }
        }

        internal bool RemovePet(uint PetId)
        {
            this.isUpdated = false;
            ServerMessage message = new ServerMessage(0x25c);
            message.AppendInt32(PetId);
            this.GetClient().SendMessage(message);
            this.InventoryPets.Remove(PetId);
            return true;
        }

        internal void RunCycleUpdate()
        {
            this.isUpdated = true;
            this.RunDBUpdate();
        }

        internal void RunDBUpdate()
        {
            try
            {
                if (((this.mRemovedItems.Count > 0) || (this.mAddedItems.Count > 0)) || (this.InventoryPets.Count > 0))
                {
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        if (this.mAddedItems.Count > 0)
                        {
                            foreach (UserItem item in this.mAddedItems.Values)
                            {
                                adapter.runFastQuery(string.Concat(new object[] { "UPDATE items_users SET user_id = ", this.UserId, " WHERE item_id = ", item.Id }));
                            }
                            this.mAddedItems.Clear();
                        }
                        if (this.mRemovedItems.Count > 0)
                        {
                            foreach (uint num in this.mRemovedItems.ToArray())
                            {
                                adapter.runFastQuery(string.Concat(new object[] { "DELETE FROM items_users WHERE item_id=", num, " AND user_id=", this.UserId }));
                            }
                            this.mRemovedItems.Clear();
                        }
                        foreach (Pet pet in this.InventoryPets.Values)
                        {
                            if (pet.DBState == DatabaseUpdateState.NeedsUpdate)
                            {
                                adapter.runFastQuery(string.Concat(new object[] { 
                                    "UPDATE user_pets SET room_id = ", pet.RoomId, ", name = @name, race = @race, color = @color, type = ", pet.Type, ", expirience = ", pet.Expirience, ", energy = ", pet.Energy, ", nutrition = ", pet.Nutrition, 
                                    ", respect = ", pet.Respect, ", createstamp = '", pet.CreationStamp, "', x = ", pet.X, ", Y = ", pet.Y, ", Z = ", pet.Z, " WHERE id = ", pet.PetId
                                 }));
                                adapter.addParameter("name", pet.Name);
                                adapter.addParameter("race", pet.Race);
                                adapter.addParameter("color", pet.Color);
                                
                            }
                            pet.DBState = DatabaseUpdateState.Updated;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.LogCacheError("FATAL ERROR DURING USER INVENTORY DB UPDATE: " + exception.ToString());
            }
        }

        internal void SendFloorInventoryUpdate()
        {
            this.mClient.SendMessage(this.SerializeFloorItemInventory());
        }

        internal ServerMessage SerializeFloorItemInventory()
        {
            ServerMessage message = new ServerMessage(Outgoing.Inventory);
            message.AppendInt32(1);
            message.AppendInt32(0);
            message.AppendInt32((int)(this.floorItems.Count + this.discs.Count + this.wallItems.Count));
            foreach (UserItem item in this.floorItems.Values)
            {
                item.SerializeFloor(message, true);
            }
            foreach (UserItem item in this.discs.Values)
            {
                item.SerializeFloor(message, true);
            }
            foreach (UserItem item in this.wallItems.Values)
            {
                item.SerializeWall(message, true);
            }
            return message;
        }


        internal ServerMessage SerializePetInventory()
        {
            ServerMessage message = new ServerMessage(Outgoing.PetInventory);
            message.AppendInt32(1);
            message.AppendInt32(1);
            message.AppendInt32(this.InventoryPets.Count);


            foreach (Pet pet in this.InventoryPets.Values)
            {
                pet.SerializeInventory(message);
            }
            
            return message;
        }

        internal ServerMessage SerializeWallItemInventory()
        {
            ServerMessage message = new ServerMessage(Outgoing.Inventory + 12345);
            //message.AppendString("I");
            message.AppendInt32(1);
            message.AppendInt32(1);
            message.AppendInt32(this.wallItems.Count);
            foreach (UserItem item in this.wallItems.Values)
            {
                item.SerializeWall(message, true);
            }
            return message;
            //return null;
        }

        internal void SetActiveState(GameClient client)
        {
            this.mClient = client;
            this.userAttatched = true;
        }

        internal void SetIdleState()
        {
            this.userAttatched = false;
            this.mClient = null;
        }

        internal void UpdateItems(bool FromDatabase)
        {
            if (FromDatabase)
            {
                this.RunDBUpdate();
                this.LoadInventory();
            }
            if (((this.mClient != null) && (this.mClient.GetMessageHandler() != null)) && (this.mClient.GetMessageHandler().GetResponse() != null))
            {
                this.mClient.GetMessageHandler().GetResponse().Init(Outgoing.UpdateInventary);
                this.mClient.GetMessageHandler().SendResponse();
            }
        }

        private bool UserHoldsItem(uint itemID)
        {
            return (this.discs.ContainsKey(itemID) || (this.floorItems.ContainsKey(itemID) || this.wallItems.ContainsKey(itemID)));
        }

        public bool isInactive
        {
            get
            {
                return !this.userAttatched;
            }
        }

        internal bool NeedsUpdate
        {
            get
            {
                return (!this.userAttatched && !this.isUpdated);
            }
        }

        internal Hashtable songDisks
        {
            get
            {
                return this.discs;
            }
        }
    }
}

