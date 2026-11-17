namespace Butterfly.HabboHotel.Users.UserDataManagement
{
    using Butterfly;
    using Butterfly.HabboHotel.Achievements;
    using Butterfly.HabboHotel.Catalogs;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Users.Authenticator;
    using Butterfly.HabboHotel.Users.Badges;
    using Butterfly.HabboHotel.Users.Relationship;
    using Butterfly.HabboHotel.RoomBots;
    using Butterfly.HabboHotel.Users.Inventory;
    using Butterfly.HabboHotel.Users.Messenger;
    using Butterfly.HabboHotel.Users.Subscriptions;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    internal class UserDataFactory
    {
        internal static UserData GetUserData(int UserId)
        {
            DataRow row;
            DataRow group = null;
            uint num;
            Dictionary<string, UserAchievement> achievements = new Dictionary<string, UserAchievement>();
            List<uint> favouritedRooms = new List<uint>();
            List<uint> ignores = new List<uint>();
            List<string> tags = new List<string>();
            Dictionary<string, Subscription> subscriptions = new Dictionary<string, Subscription>();
            List<Badge> badges = new List<Badge>();
            List<UserItem> inventory = new List<UserItem>();
            List<AvatarEffect> effects = new List<AvatarEffect>();
            Dictionary<uint, MessengerBuddy> friends = new Dictionary<uint, MessengerBuddy>();
            Dictionary<uint, MessengerRequest> requests = new Dictionary<uint, MessengerRequest>();
            List<RoomData> rooms = new List<RoomData>();
            Dictionary<uint, Pet> pets = new Dictionary<uint, Pet>();
            Dictionary<uint, int> quests = new Dictionary<uint, int>();
            Hashtable inventorySongs = new Hashtable();
            Dictionary<uint, RoomBot> bots = new Dictionary<uint, RoomBot>();
            List<Relationship> relationships = new List<Relationship>();
            Dictionary<uint, uint> tempRelationshipsDictionary = new Dictionary<uint, uint>();
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT users.* FROM users WHERE users.id = @id");
                adapter.addParameter("id", UserId);
                row = adapter.getRow();
                if (row == null)
                {
                    return null;
                }
                num = Convert.ToUInt32(row["id"]);
                if (ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(num) != null)
                {
                    return null;
                }
                if (ButterflyEnvironment.groupsEnabled)
                {
                    adapter.setQuery("SELECT groups_memberships.*, groups_details.* FROM groups_memberships LEFT JOIN groups_details ON groups_memberships.groupid = groups_details.id WHERE groups_memberships.userid = " + num + " AND groups_memberships.is_current = '1'");
                    group = adapter.getRow();
                }

                DataTable table;
                DataTable table2;
                //Profile Initialization - Might don't need all friends information, just the count but I didn't want to brake the system, so let them go..
                //Altrough this is not the best perfomance scenario
                adapter.setQuery("SELECT * FROM user_relationships WHERE requester_id = " + num);
                table = adapter.getTable();
                uint target, rstatus;
                foreach (DataRow row3 in table.Rows)
                {
                    target = Convert.ToUInt32(row3["target_id"]);
                    rstatus = Convert.ToUInt32(row3["relationshipstatus"]);
                    relationships.Add(new Relationship(target, rstatus));
                }

                adapter.setQuery(string.Concat(new object[] { "SELECT users.id,users.username,users.motto,users.look,users.last_online FROM users JOIN messenger_friendships ON users.id = messenger_friendships.sender WHERE messenger_friendships.receiver = ", num, " UNION ALL SELECT users.id,users.username,users.motto,users.look,users.last_online FROM users JOIN messenger_friendships ON users.id = messenger_friendships.receiver WHERE messenger_friendships.sender = ", num, " LIMIT 500" }));
                table2 = adapter.getTable();
                foreach (DataRow row3 in table2.Rows)
                {
                    uint num14 = Convert.ToUInt32(row3["id"]);
                    string pUsername = (string)row3["username"];
                    string pLook = (string)row3["look"];
                    string pMotto = (string)row3["motto"];
                    string pLastOnline = (string)row3["last_online"];
                    uint relation;
                    if (tempRelationshipsDictionary.ContainsKey(num14))
                        relation = tempRelationshipsDictionary[num14];
                    else
                        relation = 0;
                    if ((num14 != num) && !friends.ContainsKey(num14))
                    {
                        friends.Add(num14, new MessengerBuddy(num14, pUsername, pLook, pMotto, pLastOnline, relation));
                    }
                }
                string str = (string)row["username"];
                Habbo user = HabboFactory.GenerateHabbo(row, group);
                row = null;
                table = null;
                table2 = null;
                tempRelationshipsDictionary = null;

                return new UserData(num, achievements, favouritedRooms, ignores, tags, subscriptions, badges, inventory, effects, friends, requests, rooms, pets, quests, inventorySongs, user, bots, relationships);
            }
        }

        internal static UserData GetUserData(string sessionTicket, string ip, out byte errorCode)
        {
            DataRow row;
            DataTable table;
            DataTable table2;
            DataTable table3;
            DataTable table4;
            DataTable table5;
            DataTable table6;
            DataTable table7;
            DataTable table8;
            DataTable table9;
            DataTable table10;
            DataTable table11;
            DataTable table12;
            DataTable table13;
            DataTable table14;
            DataTable dBots;
            DataRow group = null;
            uint num;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (ButterflyEnvironment.useSSO)
                {
                    adapter.setQuery("SELECT * " +
                                        "FROM users " +
                                        "WHERE auth_ticket = @sso ");
                }
                else
                {
                    adapter.setQuery("SELECT * " +
                                        "FROM users " +
                                        "WHERE auth_ticket = @sso ");
                }

                adapter.addParameter("sso", sessionTicket);

                row = adapter.getRow();     
                if (row == null)
                {
                    errorCode = 1;
                    return null;
                }
                num = Convert.ToUInt32(row["id"]);
                if (ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(num) != null)
                {
                    errorCode = 2;
                    return null;
                }
                string str = (string) row["lastdailycredits"];
                string str2 = DateTime.Today.ToString("MM/dd");
                if (str != str2)
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET credits = credits + 3000, daily_respect_points = 3, lastdailycredits = '", str2, "' WHERE id = ", num }));
                    row["credits"] = ((int) row["credits"]) + 0xbb8;
                }
                adapter.setQuery("SELECT * FROM user_achievement WHERE userid = " + num);
                table = adapter.getTable();
                adapter.setQuery("SELECT room_id FROM user_favorites WHERE user_id = " + num);
                table2 = adapter.getTable();
                adapter.setQuery("SELECT ignore_id FROM user_ignores WHERE user_id = " + num);
                table3 = adapter.getTable();
                adapter.setQuery("SELECT * FROM user_bots WHERE user_id = " + num + " AND room_id = 0");
                dBots = adapter.getTable();
                adapter.setQuery("SELECT tag FROM user_tags WHERE user_id = " + num);
                table4 = adapter.getTable();
                adapter.setQuery("SELECT * FROM user_subscriptions WHERE user_id = " + num);
                table5 = adapter.getTable();
                adapter.setQuery("SELECT * FROM user_badges WHERE user_id = " + num);
                table6 = adapter.getTable();
                if (adapter.dbType == DatabaseType.MySQL)
                {
                    adapter.setQuery("CALL getuseritems(" + num + ")");
                }
                else
                {
                    adapter.setQuery("EXECUTE getuseritems " + num);
                }
                table7 = adapter.getTable();
                adapter.setQuery("SELECT * FROM user_effects WHERE user_id =  " + num);
                table8 = adapter.getTable();
                adapter.setQuery(string.Concat(new object[] { "SELECT users.id,users.username,users.motto,users.look,users.last_online FROM users JOIN messenger_friendships ON users.id = messenger_friendships.sender WHERE messenger_friendships.receiver = ", num, " UNION ALL SELECT users.id,users.username,users.motto,users.look,users.last_online FROM users JOIN messenger_friendships ON users.id = messenger_friendships.receiver WHERE messenger_friendships.sender = ", num, " LIMIT 500" }));
                table9 = adapter.getTable();
                adapter.setQuery("SELECT messenger_requests.sender,messenger_requests.receiver,users.username FROM users JOIN messenger_requests ON users.id = messenger_requests.sender WHERE messenger_requests.receiver = " + num + " LIMIT 150");
                table10 = adapter.getTable();
                adapter.setQuery("SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE owner = @name");
                adapter.addParameter("name", (string) row["username"]);
                table11 = adapter.getTable();
                adapter.setQuery("SELECT * FROM user_pets WHERE user_id = " + num + " AND room_id = 0");
                table12 = adapter.getTable();
                adapter.setQuery("SELECT * FROM user_quests WHERE user_id = " + num);
                table13 = adapter.getTable();
                if (ButterflyEnvironment.groupsEnabled)
                {
                    adapter.setQuery("SELECT groups_memberships.*, groups_details.* FROM groups_memberships LEFT JOIN groups_details ON groups_memberships.groupid = groups_details.id WHERE groups_memberships.userid = " + num + " AND groups_memberships.is_current = '1'");
                    group = adapter.getRow();
                }
                adapter.setQuery(string.Concat(new object[] { "UPDATE users SET ip_last = @ip WHERE id = ", num, "; UPDATE user_info SET login_timestamp = '", ButterflyEnvironment.GetUnixTimestamp(), "' WHERE user_id = ", num, " ; " }));
                adapter.addParameter("ip", ip);
                adapter.runQuery();
                if (adapter.dbType == DatabaseType.MySQL)
                {
                    adapter.runFastQuery("REPLACE INTO user_online VALUES (" + num + ")");
                }
                else
                {
                    adapter.runFastQuery(string.Concat(new object[] { "IF NOT EXISTS (SELECT userid FROM user_online WHERE userid = ", num, ") INSERT INTO user_online VALUES (", num, ")" }));
                }
                adapter.setQuery("SELECT * FROM user_relationships WHERE requester_id = " + num);
                table14 = adapter.getTable();
            }
                
            Dictionary<string, UserAchievement> achievements = new Dictionary<string, UserAchievement>();
            foreach (DataRow row3 in table.Rows)
            {
                string achievementGroup = (string) row3["groupname"];
                int level = (int) row3["level"];
                int progress = (int) row3["progress"];
                UserAchievement achievement = new UserAchievement(achievementGroup, level, progress);
                achievements.Add(achievementGroup, achievement);
            }
            List<uint> favouritedRooms = new List<uint>();
            foreach (DataRow row3 in table2.Rows)
            {
                uint item = Convert.ToUInt32(row3["room_id"]);
                favouritedRooms.Add(item);
            }
            List<uint> ignores = new List<uint>();
            foreach (DataRow row3 in table3.Rows)
            {
                uint num5 = Convert.ToUInt32(row3["ignore_id"]);
                ignores.Add(num5);
            }
            Dictionary<uint, RoomBot> bots = new Dictionary<uint, RoomBot>();
            RoomBot bot;
            foreach (DataRow dRow in dBots.Rows)
            {
                bot = Catalog.GenerateBotFromRow(dRow);
                bots.Add(bot.BotId, bot);
            }
            List<string> tags = new List<string>();
            foreach (DataRow row3 in table4.Rows)
            {
                string str4 = (string) row3["tag"];
                tags.Add(str4);
            }
            Dictionary<string, Subscription> subscriptions = new Dictionary<string, Subscription>();
            foreach (DataRow row3 in table5.Rows)
            {
                string key = (string) row3["subscription_id"];
                int timeExpire = (int) row3["timestamp_expire"];
                subscriptions.Add(key, new Subscription(key, timeExpire));
            }
            List<Badge> badges = new List<Badge>();
            foreach (DataRow row3 in table6.Rows)
            {
                string code = (string) row3["badge_id"];
                int slot = (int) row3["badge_slot"];
                badges.Add(new Badge(code, slot));
            }
            List<Relationship> relationships = new List<Relationship>();
            Dictionary<uint, uint> tempRelationshipsDictionary = new Dictionary<uint, uint>();
            uint target, rstatus;
            foreach (DataRow row3 in table14.Rows)
            {
                target = Convert.ToUInt32(row3["target_id"]);
                rstatus = Convert.ToUInt32(row3["relationshipstatus"]);
                relationships.Add(new Relationship(target, rstatus));
                if(!tempRelationshipsDictionary.ContainsKey(target))
                    tempRelationshipsDictionary.Add(target,rstatus);
            }

            List<UserItem> inventory = new List<UserItem>();
            foreach (DataRow row3 in table7.Rows)
            {
                string str7;
                uint id = Convert.ToUInt32(row3[0]);
                uint baseItem = Convert.ToUInt32(row3[1]);
                if (!DBNull.Value.Equals(row3[2]))
                {
                    str7 = (string) row3[2];
                }
                else
                {
                    str7 = string.Empty;
                }
                uint rareid = Convert.ToUInt32(row3[3]);
                string groupstring = (string) row3[4];
                inventory.Add(new UserItem(id, baseItem, str7, rareid, groupstring, num));
            }
            List<AvatarEffect> effects = new List<AvatarEffect>();
            foreach (DataRow row3 in table8.Rows)
            {
                int effectId = (int) row3["effect_id"];
                int totalDuration = (int) row3["total_duration"];
                bool activated = ButterflyEnvironment.EnumToBool((string) row3["is_activated"]);
                double activateTimestamp = (double) row3["activated_stamp"];
                effects.Add(new AvatarEffect(effectId, totalDuration, activated, activateTimestamp));
            }
            Dictionary<uint, MessengerBuddy> friends = new Dictionary<uint, MessengerBuddy>();
            string str9 = (string) row["username"];
            foreach (DataRow row3 in table9.Rows)
            {
                uint num14 = Convert.ToUInt32(row3["id"]);
                string pUsername = (string) row3["username"];
                string pLook = (string) row3["look"];
                string pMotto = (string) row3["motto"];
                string pLastOnline = (string) row3["last_online"];
                uint relation;
                if(tempRelationshipsDictionary.ContainsKey(num14))
                    relation = tempRelationshipsDictionary[num14];
                else
                    relation = 0;
                if ((num14 != num) && !friends.ContainsKey(num14))
                {
                    friends.Add(num14, new MessengerBuddy(num14, pUsername, pLook, pMotto, pLastOnline,relation));
                }
            }
            Dictionary<uint, MessengerRequest> requests = new Dictionary<uint, MessengerRequest>();
            foreach (DataRow row3 in table10.Rows)
            {
                uint num15 = Convert.ToUInt32(row3["sender"]);
                uint num16 = Convert.ToUInt32(row3["receiver"]);
                string str14 = (string) row3["username"];
                if (num15 != num)
                {
                    if (!requests.ContainsKey(num15))
                    {
                        requests.Add(num15, new MessengerRequest(num, num15, str14));
                    }
                }
                else if (!requests.ContainsKey(num16))
                {
                    requests.Add(num16, new MessengerRequest(num, num16, str14));
                }
            }
            List<RoomData> rooms = new List<RoomData>();
            foreach (DataRow row3 in table11.Rows)
            {
                uint roomId = Convert.ToUInt32(row3["id"]);
                rooms.Add(ButterflyEnvironment.GetGame().GetRoomManager().FetchRoomData(roomId, row3));
            }
            Dictionary<uint, Pet> pets = new Dictionary<uint, Pet>();
            foreach (DataRow row3 in table12.Rows)
            {
                Pet pet = Catalog.GeneratePetFromRow(row3);
                pets.Add(pet.PetId, pet);
            }
            Dictionary<uint, int> quests = new Dictionary<uint, int>();
            foreach (DataRow row3 in table13.Rows)
            {
                uint num18 = Convert.ToUInt32(row3["quest_id"]);
                int num19 = (int) row3["progress"];
                quests.Add(num18, num19);
            }
            Hashtable inventorySongs = new Hashtable();
            Habbo user = HabboFactory.GenerateHabbo(row, group);
            tempRelationshipsDictionary = null;
            row = null;
            table = null;
            table2 = null;
            table3 = null;
            table4 = null;
            table5 = null;
            table6 = null;
            table7 = null;
            table8 = null;
            table9 = null;
            table10 = null;
            table11 = null;
            table12 = null;
            table13 = null;
            table14 = null;
            dBots = null;
            errorCode = 0;
            return new UserData(num, achievements, favouritedRooms, ignores, tags, subscriptions, badges, inventory, effects, friends, requests, rooms, pets, quests, inventorySongs, user, bots, relationships);
        }
    }
}

