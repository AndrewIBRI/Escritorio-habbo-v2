namespace Butterfly.HabboHotel.Navigators
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.Messages;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    internal class Navigator
    {
        private Hashtable PrivateCategories = new Hashtable();
        private Dictionary<int, PublicItem> PublicItems = new Dictionary<int, PublicItem>();

        internal Navigator()
        {
        }

        internal FlatCat GetFlatCat(int Id)
        {
            if (this.PrivateCategories.ContainsKey(Id))
            {
                return (FlatCat) this.PrivateCategories[Id];
            }
            return null;
        }

        internal void Initialize(IQueryAdapter dbClient)
        {
            dbClient.setQuery("SELECT id,caption,min_rank FROM navigator_flatcats WHERE enabled = 2");
            DataTable table = dbClient.getTable();
            dbClient.setQuery("SELECT * FROM navigator_publics_new ORDER BY ordernum ASC");
            DataTable table2 = dbClient.getTable();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    this.PrivateCategories.Add((int) row["id"], new FlatCat((int) row["id"], (string) row["caption"], (int) row["min_rank"]));
                }
            }
            if (table2 != null)
            {
                foreach (DataRow row in table2.Rows)
                {
                    this.PublicItems.Add((int) row["id"], new PublicItem((int) row["id"], int.Parse(row["bannertype"].ToString()), (string) row["caption"], (string) row["description"], (string) row["image"], (row["image_type"].ToString().ToLower() == "internal") ? PublicImageType.INTERNAL : PublicImageType.EXTERNAL, Convert.ToUInt32(row["room_id"]), (int) row["category_id"], (int) row["category_parent_id"], ButterflyEnvironment.EnumToBool(row["recommended"].ToString()), (int) row["typeofdata"], (string) row["tag"]));
                }
            }
        }

        private ServerMessage SerializeActiveRooms(int category)
        {
            int active = 0;
            try
            {
                KeyValuePair<RoomData, int>[] activeRooms = ButterflyEnvironment.GetGame().GetRoomManager().GetActiveRooms();
                foreach (KeyValuePair<RoomData, int> pair in activeRooms)
                {
                    RoomData key = pair.Key;
                    if (key.Category.Equals(category))
                    {
                       active++;
                    }
                }
                Array.Clear(activeRooms, 0, activeRooms.Length);
                activeRooms = null;
            }
            catch
            {
                active = 0;
            }

            ServerMessage reply = new ServerMessage(Outgoing.NavigatorPacket);
            reply.AppendInt32(1);
            reply.AppendString(category.ToString());
            
            if (active!=0)
            {
                KeyValuePair<RoomData, int>[] activeRooms = ButterflyEnvironment.GetGame().GetRoomManager().GetActiveRooms();
                this.SerializeNavigatorPopularRooms(ref reply, activeRooms, category, active);
                Array.Clear(activeRooms, 0, activeRooms.Length);
                activeRooms = null;
            }
            else
            {
                reply.AppendInt32(0);
            }
            reply.AppendInt32(0);
            return reply;
        }

        internal ServerMessage SerializeEventListing(int Category)
        {
            ServerMessage message = new ServerMessage(0x1c3);
            message.AppendInt32(Category);
            message.AppendInt32(12);
            message.AppendString("");
            KeyValuePair<RoomData, int>[] eventRoomsForCategory = ButterflyEnvironment.GetGame().GetRoomManager().GetEventRoomsForCategory(Category);
            message.AppendInt32(eventRoomsForCategory.Length);
            foreach (KeyValuePair<RoomData, int> pair in eventRoomsForCategory)
            {
                pair.Key.Serialize(message, true);
            }
            return message;
        }

        internal ServerMessage SerializeFavoriteRooms(GameClient Session)
        {
            ServerMessage message = new ServerMessage(Outgoing.NavigatorPacket);
            message.AppendInt32(6);
            message.AppendString("");
            message.AppendInt32(Session.GetHabbo().FavoriteRooms.Count);
            foreach (uint num in Session.GetHabbo().FavoriteRooms.ToArray())
            {
                RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(num);
                if (data != null)
                {
                    data.Serialize(message, false);
                }
            }
            message.AppendBoolean(false);
            return message;
        }

        internal ServerMessage SerializeFlatCategories(GameClient Session)
        {
            ServerMessage message = new ServerMessage(Outgoing.FlatCats);
            message.AppendInt32(this.PrivateCategories.Count);
            foreach (FlatCat cat in this.PrivateCategories.Values)
            {
                message.AppendInt32(cat.Id);
                message.AppendString(cat.Caption);
                message.AppendBoolean(cat.MinRank <= Session.GetHabbo().Rank);
            }
            return message;
        }

        private void SerializeItemsFromCata(int Id, ServerMessage Message)
        {
            foreach (PublicItem item in this.PublicItems.Values)
            {
                if (item.ParentId == Id)
                {
                    item.Serialize(Message);
                }
            }
        }

        internal ServerMessage SerializeNavigator(GameClient session, int mode)
        {
            KeyValuePair<RoomData, int>[] votedRooms;
            if (mode >= 0)
            {
                return this.SerializeActiveRooms(mode);
            }
            ServerMessage reply = new ServerMessage(Outgoing.NavigatorPacket);
            switch (mode)
            {
                case -5:
                case -4:
                {
                    reply.AppendInt32((int) (mode * -1));
                    List<RoomData> rooms = (from p in session.GetHabbo().GetMessenger().GetActiveFriendsRooms()
                        orderby p.UsersNow descending
                        select p).ToList<RoomData>();
                    this.SerializeNavigatorRooms(ref reply, rooms);
                    return reply;
                }
                case -3:
                    reply.AppendInt32(5);
                    this.SerializeNavigatorRooms(ref reply, session.GetHabbo().UsersRooms);
                    return reply;

                case -2:
                    reply.AppendInt32(2);
                    try
                    {
                        votedRooms = ButterflyEnvironment.GetGame().GetRoomManager().GetVotedRooms();
                        this.SerializeNavigatorRooms(ref reply, votedRooms);
                        Array.Clear(votedRooms, 0, votedRooms.Length);
                        votedRooms = null;
                    }
                    catch
                    {
                        reply.AppendInt32(0);
                    }
                    reply.AppendInt32(0);
                    return reply;

                case -1:
                    reply.AppendInt32(1);
                    reply.AppendString("-1");
                    votedRooms = null;
                    try
                    {
                        votedRooms = ButterflyEnvironment.GetGame().GetRoomManager().GetActiveRooms();
                        this.SerializeNavigatorPopularRooms(ref reply, votedRooms);
                        Array.Clear(votedRooms, 0, votedRooms.Length);
                        votedRooms = null;
                    }
                    catch
                    {
                        reply.AppendInt32(0);
                        reply.AppendBoolean(false);
                    }
                    return reply;
            }
            return reply;
        }

        private void SerializeNavigatorPopularRooms(ref ServerMessage reply, KeyValuePair<RoomData, int>[] rooms)
        {
            reply.AppendInt32(rooms.Length);
            foreach (KeyValuePair<RoomData, int> pair in rooms)
            {
                pair.Key.Serialize(reply, false);
            }
            reply.AppendBoolean(false);
        }

        private void SerializeNavigatorPopularRooms(ref ServerMessage reply, KeyValuePair<RoomData, int>[] rooms, int Category, int activeRooms)
        {
            reply.AppendInt32(activeRooms);
            int num = 0;
            foreach (KeyValuePair<RoomData, int> pair in rooms)
            {
                RoomData key = pair.Key;
                if (key.Category.Equals(Category))
                {
                    key.Serialize(reply, false);
                    num++;
                }
            }
            /*if(num==0)
                reply.AppendInt32(0);*/
        }

        private void SerializeNavigatorRooms(ref ServerMessage reply, KeyValuePair<RoomData, int>[] rooms)
        {
            reply.AppendString(string.Empty);
            reply.AppendInt32(rooms.Length);
            foreach (KeyValuePair<RoomData, int> pair in rooms)
            {
                pair.Key.Serialize(reply, false);
            }
        }

        private void SerializeNavigatorRooms(ref ServerMessage reply, List<RoomData> rooms)
        {
            reply.AppendString("");
            reply.AppendInt32(rooms.Count);
            foreach (RoomData data in rooms)
            {
                data.Serialize(reply, false);
            }
            reply.AppendInt32(0);
            reply.AppendInt32(0);
            reply.AppendBoolean(false);
        }

        internal ServerMessage SerializePopularRoomTags()
        {
            Dictionary<string, int> collection = new Dictionary<string, int>();
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MySQL)
                {
                    adapter.setQuery("SELECT rooms.tags, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE roomtype = 'private' AND active_users > 0 ORDER BY active_users DESC LIMIT 50");
                }
                else
                {
                    adapter.setQuery("SELECT TOP 50 rooms.tags, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE roomtype = 'private' AND active_users > 0 ORDER BY active_users DESC");
                }
                DataTable table = adapter.getTable();
                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        int num;
                        if (!string.IsNullOrEmpty(row["active_users"].ToString()))
                        {
                            num = (int) row["active_users"];
                        }
                        else
                        {
                            num = 0;
                        }
                        List<string> list = new List<string>();
                        foreach (string str in row["tags"].ToString().Split(new char[] { ',' }))
                        {
                            list.Add(str);
                        }
                        foreach (string str in list)
                        {
                            if (collection.ContainsKey(str))
                            {
                                Dictionary<string, int> dictionary2;
                                string str2;
                                (dictionary2 = collection)[str2 = str] = dictionary2[str2] + num;
                            }
                            else
                            {
                                collection.Add(str, num);
                            }
                        }
                    }
                }
                List<KeyValuePair<string, int>> list2 = new List<KeyValuePair<string, int>>(collection);
                list2.Sort((Comparison<KeyValuePair<string, int>>) ((firstPair, nextPair) => firstPair.Value.CompareTo(nextPair.Value)));
                ServerMessage message = new ServerMessage(Outgoing.PopularTags);
                message.AppendInt32(list2.Count);
                foreach (KeyValuePair<string, int> pair in list2)
                {
                    message.AppendString(pair.Key);
                    message.AppendInt32(pair.Value);
                }
                return message;
            }
        }

        internal ServerMessage SerializePublicRooms()
        {
            ServerMessage message = new ServerMessage(Outgoing.SerializePublicRooms);
            message.AppendInt32(this.PublicItems.Count);
            foreach (PublicItem item in this.PublicItems.Values)
            {
                if (item.ParentId <= 0)
                {
                    item.Serialize(message);
                    if (item.itemType == PublicItemType.CATEGORY)
                    {
                        foreach (PublicItem item2 in this.PublicItems.Values)
                        {
                            if (item.Id == item2.ParentId)
                            {
                                item2.Serialize(message);
                            }
                        }
                    }
                }
            }
            message.AppendInt32(1);
            int num = new Random().Next(1, this.PublicItems.Count);
            num = 4;
            try
            {
                this.PublicItems[num].Serialize(message);
            }
            catch
            {
                this.PublicItems[1].Serialize(message);
            }
            message.AppendInt32(0);
            return message;
        }

        internal ServerMessage SerializeRecentRooms(GameClient Session)
        {
            ServerMessage message = new ServerMessage(Outgoing.NavigatorPacket);
            message.AppendInt32(7);
            message.AppendString("");
            message.AppendInt32(0);
            message.AppendBoolean(false);
            return message;
        }

        internal ServerMessage SerializeSearchResults(string SearchQuery)
        {
            DataTable table = new DataTable();
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (SearchQuery.Length > 0)
                {
                    if (SearchQuery.StartsWith("owner:"))
                    {
                        adapter.setQuery("SELECT * FROM rooms WHERE owner LIKE '" + SearchQuery.Replace("owner:", "") + "' ORDER BY id DESC LIMIT 50");
                    }
                    else if (adapter.dbType == DatabaseType.MySQL)
                    {
                        adapter.setQuery("SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE owner = @query AND roomtype = 'private' UNION ALL SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE caption = @query AND roomtype = 'private' ORDER BY active_users DESC LIMIT 50");
                    }
                    else
                    {
                        adapter.setQuery("SELECT TOP 50 rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE owner = @query AND roomtype = 'private' UNION ALL SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE caption = @query AND roomtype = 'private' ORDER BY active_users DESC");
                    }
                    adapter.addParameter("query", SearchQuery);
                    table = adapter.getTable();
                }
            }
            List<RoomData> list = new List<RoomData>();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    RoomData item = ButterflyEnvironment.GetGame().GetRoomManager().FetchRoomData(Convert.ToUInt32(row["id"]), row);
                    list.Add(item);
                }
            }
            ServerMessage message = new ServerMessage(Outgoing.NavigatorPacket);
            message.AppendInt32(8);
            message.AppendString(SearchQuery);
            message.AppendInt32(list.Count);
            foreach (RoomData data2 in list)
            {
                data2.Serialize(message, false);
            }
            message.AppendBoolean(false);
            return message;
        }
    }
}

