namespace Butterfly.HabboHotel.Misc
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.RoomIvokedItems;
    using Butterfly.Messages;
    using Butterfly.Util;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Database_Manager.Database;

    internal class ChatCommandHandler
    {
        private string[] Params;
        private GameClient Session;

        public ChatCommandHandler(string[] input, GameClient session)
        {
            this.Params = input;
            this.Session = session;
        }

        internal void alert()
        {
            string username = null;
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            username = this.Params[1];
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(username);
            if (clientByUsername == null)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
            else
            {
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, clientByUsername.GetHabbo().Username, "Alert", "Alerted user with message [" + MergeParams(this.Params, 2) + "]");
                clientByUsername.SendNotif(MergeParams(this.Params, 2), this.Session.GetHabbo().HasFuse("fuse_admin"));
            }
        }

        internal void ban()
        {
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(this.Params[1]);
            if (clientByUsername == null)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
            else if (clientByUsername.GetHabbo().Rank >= this.Session.GetHabbo().Rank)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("ban.notallowed"));
            }
            else
            {
                int num = 0;
                try
                {
                    num = int.Parse(this.Params[2]);
                }
                catch (FormatException)
                {
                    return;
                }
                if (num <= 600)
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("ban.toolesstime"));
                }
                else
                {
                    ButterflyEnvironment.GetGame().GetBanManager().BanUser(clientByUsername, this.Session.GetHabbo().Username, (double) num, MergeParams(this.Params, 3), false);
                    ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, clientByUsername.GetHabbo().Username, "Ban", string.Concat(new object[] { "Ban for ", num, " seconds with message ", MergeParams(this.Params, 3) }));
                }
            }
        }

        internal void buyx()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            try
            {
                int num = int.Parse(this.Params[1]);
                if (this.Session.GetHabbo().Rank > 1)
                {
                    if (num <= 0)
                    {
                        this.Session.SendNotif(LanguageLocale.GetValue("buyx.maxminreached"));
                    }
                    else
                    {
                        this.Session.GetHabbo().buyItemLoop = num;
                    }
                }
                else if ((num <= 0) || (num >= 50))
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("buyx.maxminreached"));
                }
                else
                {
                    this.Session.GetHabbo().buyItemLoop = num;
                }
            }
            catch
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
            }
        }

        internal static void catarefresh()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                ButterflyEnvironment.GetGame().GetCatalog().Initialize(adapter);
            }
            ButterflyEnvironment.GetGame().GetCatalog().InitCache();
            ButterflyEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(new ServerMessage(Outgoing.UpdateShop));
        }

        internal void coins()
        {
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(this.Params[1]);
            if (clientByUsername != null)
            {
                int num;
                if (int.TryParse(this.Params[2], out num))
                {
                    clientByUsername.GetHabbo().Credits += num;
                    clientByUsername.GetHabbo().UpdateCreditsBalance();
                    clientByUsername.SendNotif(this.Session.GetHabbo().Username + LanguageLocale.GetValue("coins.awardmessage1") + num.ToString() + LanguageLocale.GetValue("coins.awardmessage2"));
                    this.Session.SendNotif(LanguageLocale.GetValue("coins.updateok"));
                }
                else
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
                }
            }
            else
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
        }

        internal void come()
        {
            if (this.Params.Length < 1)
            {
                this.Session.SendNotif("No use specified");
            }
            else
            {
                string username = this.Params[1];
                GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(username);
                if (clientByUsername == null)
                {
                    this.Session.SendNotif("User is  offline");
                }
                else
                {
                    Room currentRoom = this.Session.GetHabbo().CurrentRoom;
                    clientByUsername.GetMessageHandler().PrepareRoomForUser(currentRoom.RoomId, currentRoom.Password);
                }
            }
        }

        internal void commands()
        {
            ServerMessage message = new ServerMessage();
            message.Init(Outgoing.SetCommandsView);
            message.AppendInt32(1);
            message.AppendString(ChatCommandRegister.GenerateCommandList(this.Session));
            this.Session.SendMessage(message);
        }

        internal void coords()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            RoomUser roomUserByHabbo = null;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    this.Session.SendNotif(string.Concat(new object[] { "X: ", roomUserByHabbo.X, " - Y: ", roomUserByHabbo.Y, " - Z: ", roomUserByHabbo.Z, " - Rot: ", roomUserByHabbo.RotBody, ", sqState: ", currentRoom.GetGameMap().GameMap[roomUserByHabbo.X, roomUserByHabbo.Y].ToString() }));
                }
            }
        }

internal void refreshnavigator()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                ButterflyEnvironment.GetGame().GetNavigator().Initialize(adapter);
            }
            this.Session.SendNotif("Navigator and Public Rooms reloaded.");
            ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "RefreshNavigator", "Refreshed Navigator.");
        }

        internal void copylook()
        {
            string query = this.Params[1];
            bool flag = false;
            string str2 = null;
            string str3 = null;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT gender,look FROM users WHERE username = @username");
                adapter.addParameter("username", query);
                DataRow row = adapter.getRow();
                if (row != null)
                {
                    flag = true;
                    str2 = (string) row[0];
                    str3 = (string) row[1];
                    adapter.setQuery("UPDATE users SET gender = @gender, look = @look WHERE username = @username");
                    adapter.addParameter("gender", str2);
                    adapter.addParameter("look", str3);
                    adapter.addParameter("username", this.Session.GetHabbo().Username);
                    adapter.runQuery();
                }
            }
            if (flag)
            {
                this.Session.GetHabbo().Gender = str2;
                this.Session.GetHabbo().Look = str3;
                this.Session.GetMessageHandler().GetResponse().Init(Outgoing.UpdateUserInformation);
                this.Session.GetMessageHandler().GetResponse().AppendInt32(-1);
                this.Session.GetMessageHandler().GetResponse().AppendString(this.Session.GetHabbo().Look);
                this.Session.GetMessageHandler().GetResponse().AppendString(this.Session.GetHabbo().Gender.ToLower());
                this.Session.GetMessageHandler().GetResponse().AppendString(this.Session.GetHabbo().Motto);
                this.Session.GetMessageHandler().GetResponse().AppendInt32(this.Session.GetHabbo().AchievementPoints);
                this.Session.GetMessageHandler().SendResponse();
                if (this.Session.GetHabbo().InRoom)
                {
                    Room currentRoom = this.Session.GetHabbo().CurrentRoom;
                    if (currentRoom != null)
                    {
                        RoomUser roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                        if (roomUserByHabbo != null)
                        {
                            ServerMessage message = new ServerMessage(Outgoing.UpdateUserInformation);
                            message.AppendInt32(roomUserByHabbo.VirtualId);
                            message.AppendString(this.Session.GetHabbo().Look);
                            message.AppendString(this.Session.GetHabbo().Gender.ToLower());
                            message.AppendString(this.Session.GetHabbo().Motto);
                            message.AppendInt32(this.Session.GetHabbo().AchievementPoints);
                            currentRoom.SendMessage(message);
                        }
                    }
                }
            }
        }

        internal void dario()
        {
            if (!(this.Params[1] != "lol123"))
            {
            }
        }

        internal void deleteMission()
        {
            string username = this.Params[1];
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(username);
            if (clientByUsername == null)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
            else if (this.Session.GetHabbo().Rank <= clientByUsername.GetHabbo().Rank)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("user.notpermitted"));
            }
            else
            {
                clientByUsername.GetHabbo().Motto = LanguageLocale.GetValue("user.unacceptable_motto");
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, clientByUsername.GetHabbo().Username, "mission removal", "removed mission");
                Room room2 = clientByUsername.GetHabbo().CurrentRoom;
                if (room2 != null)
                {
                    RoomUser roomUserByHabbo = room2.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                    if (roomUserByHabbo != null)
                    {
                        ServerMessage message = new ServerMessage(Outgoing.UpdateUserInformation);
                        message.AppendInt32(roomUserByHabbo.VirtualId);
                        message.AppendString(this.Session.GetHabbo().Look);
                        message.AppendString(this.Session.GetHabbo().Gender.ToLower());
                        message.AppendString(this.Session.GetHabbo().Motto);
                        message.AppendInt32(this.Session.GetHabbo().AchievementPoints);
                        room2.SendMessage(message);
                    }
                }
            }
        }

        internal void disablediagonal()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            currentRoom = this.Session.GetHabbo().CurrentRoom;
            if (currentRoom.GetGameMap().DiagonalEnabled)
            {
                currentRoom.GetGameMap().DiagonalEnabled = false;
            }
            else
            {
                currentRoom.GetGameMap().DiagonalEnabled = true;
            }
        }

        internal void disablefriends()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE users SET block_newfriends = '1' WHERE id = " + this.Session.GetHabbo().Id);
            }
            this.Session.GetHabbo().HasFriendRequestsDisabled = true;
            this.Session.SendNotif(LanguageLocale.GetValue("friends.disabled"));
        }

        internal void disabletrade()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE users SET block_trade = '1' WHERE id = " + this.Session.GetHabbo().Id);
            }
            this.Session.SendNotif(LanguageLocale.GetValue("trade.disable"));
        }

        internal void disconnect()
        {
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(this.Params[1]);
            if (clientByUsername == null)
            {
                this.Session.SendNotif("User not found.");
            }
            else if (clientByUsername.GetHabbo().Rank >= this.Session.GetHabbo().Rank)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("disconnect.notallwed"));
            }
            else
            {
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, clientByUsername.GetHabbo().Username, "Disconnect", "User disconnected by user");
                clientByUsername.GetConnection().Dispose();
            }
        }

        internal void Dispose()
        {
            this.Session = null;
            Array.Clear(this.Params, 0, this.Params.Length);
        }

        internal void dumpmaps()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Session.GetHabbo().CurrentRoom.GetGameMap().GenerateMapDump());
            FileStream stream = new FileStream(@"Logs\mapdumps.txt", FileMode.Append, FileAccess.Write);
            byte[] bytes = Encoding.ASCII.GetBytes(builder.ToString() + "\r\n\r\n");
            stream.Write(bytes, 0, bytes.Length);
            stream.Dispose();
        }

        internal void ejectall()
        {
            Dictionary<uint, uint> rejects = new Dictionary<uint, uint>();

            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            foreach (RoomItem item in currentRoom.GetRoomItemHandler().mWallItems.Values)
            {
                if ((currentRoom.OwnerId == Session.GetHabbo().Id)&&(item.placedBy != Session.GetHabbo().Id))
                {
                    if ((item != null) && (item.GetBaseItem().InteractionType != InteractionType.postit))
                    {

                        rejects.Add(item.Id,item.placedBy);
                    }
                }
            }

            foreach (RoomItem item in currentRoom.GetRoomItemHandler().mFloorItems.Values)
            {
                if ((currentRoom.OwnerId == Session.GetHabbo().Id) && (item.placedBy != Session.GetHabbo().Id))
                {
                    if ((item != null) && (item.GetBaseItem().InteractionType != InteractionType.postit))
                    {
                        rejects.Add(item.Id, item.placedBy);
                    }
                }
            }

            foreach (KeyValuePair<uint, uint> eject in rejects)
            {
                Habbo habbo = ButterflyEnvironment.getHabboForId(eject.Value);
                RoomItem item = currentRoom.GetRoomItemHandler().GetItem(eject.Key);
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

                    currentRoom.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                }
                else
                {
                    currentRoom.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                    currentRoom.GetRoomItemHandler().RemoveFurniture(habbo.GetClient(), item.Id);
                    habbo.GetInventoryComponent().AddNewItem(item.Id, item.BaseItem, item.ExtraData, true, true, item.RareId, item.Guilds_Data_String, 0);
                    habbo.GetInventoryComponent().UpdateItems(false);
                }
            }
        }
        

        internal void empty()
        {
            if ((this.Params.Length > 1) && this.Session.GetHabbo().HasFuse("fuse_sysadmin"))
            {
                GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(this.Params[1]);
                if (clientByUsername != null)
                {
                    clientByUsername.GetHabbo().GetInventoryComponent().ClearItems();
                    this.Session.SendNotif(LanguageLocale.GetValue("empty.dbcleared"));
                }
                else
                {
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery("SELECT id FROM users WHERE username = @usrname");
                        adapter.addParameter("usrname", this.Params[1]);
                        adapter.runFastQuery("DELETE FROM items_users WHERE user_id = " + int.Parse(adapter.getString()));
                        this.Session.SendNotif(LanguageLocale.GetValue("empty.cachecleared"));
                    }
                }
            }
            else
            {
                this.Session.GetHabbo().GetInventoryComponent().ClearItems();
                this.Session.SendNotif(LanguageLocale.GetValue("empty.cleared"));
            }
        }

        internal void enable()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            int effectId = int.Parse(this.Params[1]);
            this.Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(effectId);
        }

        internal void enablefriends()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE users SET block_newfriends = '0' WHERE id = " + this.Session.GetHabbo().Id);
            }
            this.Session.GetHabbo().HasFriendRequestsDisabled = false;
            this.Session.SendNotif(LanguageLocale.GetValue("friends.enabled"));
        }

        internal void enablestatus()
        {
            this.Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Username).AddStatus(this.Params[0], string.Empty);
        }

        internal void enabletrade()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE users SET block_trade = '0' WHERE id = " + this.Session.GetHabbo().Id);
            }
            this.Session.SendNotif(LanguageLocale.GetValue("trade.enable"));
        }

        internal void everybodyactionnow()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            if (currentRoom != null)
            {
                int result = 0;
                if (int.TryParse(this.Params[1], out result))
                {
                    List<RoomUser> roomUsers = currentRoom.GetRoomUserManager().GetRoomUsers();
                    foreach (RoomUser user in roomUsers)
                    {
                        user.DanceId = 0;
                        ServerMessage message = new ServerMessage(Outgoing.Action);
                        message.AppendInt32(user.VirtualId);
                        message.AppendInt32(result);
                        currentRoom.SendMessage(message);
                        if (result == 5)
                        {
                            user.IsAsleep = true;
                            ServerMessage message2 = new ServerMessage(Outgoing.IdleStatus);
                            message2.AppendInt32(user.VirtualId);
                            message2.AppendBoolean(user.IsAsleep);
                            currentRoom.SendMessage(message2);
                        }
                    }
                }
            }
        }

        internal void everybodydancenow()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            if (currentRoom != null)
            {
                int result = 0;
                if (int.TryParse(this.Params[1], out result))
                {
                    List<RoomUser> roomUsers = currentRoom.GetRoomUserManager().GetRoomUsers();
                    foreach (RoomUser user in roomUsers)
                    {
                        user.DanceId = result;
                        ServerMessage message = new ServerMessage(Outgoing.Dance);
                        message.AppendInt32(user.VirtualId);
                        message.AppendInt32(result);
                        currentRoom.SendMessage(message);
                    }
                }
            }
        }

        internal void everybodysaysnow()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            if (currentRoom != null)
            {
                string message = MergeParams(this.Params, 1);
                if (message != "")
                {
                    List<RoomUser> roomUsers = currentRoom.GetRoomUserManager().GetRoomUsers();
                    foreach (RoomUser user in roomUsers)
                    {
                        user.Chat(user.GetClient(), message, false, 0);
                    }
                }
            }
        }

        internal void everybodyshoutsnow()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            if (currentRoom != null)
            {
                string message = MergeParams(this.Params, 1);
                if (message != "")
                {
                    List<RoomUser> roomUsers = currentRoom.GetRoomUserManager().GetRoomUsers();
                    foreach (RoomUser user in roomUsers)
                    {
                        user.Chat(user.GetClient(), message, false, 0);
                    }
                }
            }
        }

        internal void everybodysignnow()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            if (currentRoom != null)
            {
                int result = 0;
                if (int.TryParse(this.Params[1], out result))
                {
                    List<RoomUser> roomUsers = currentRoom.GetRoomUserManager().GetRoomUsers();
                    foreach (RoomUser user in roomUsers)
                    {
                        user.AddStatus("sign", Convert.ToString(result));
                        user.UpdateNeeded = true;
                    }
                }
            }
        }

        internal void faq()
        {
            DataTable table;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT question, answer FROM faq");
                table = adapter.getTable();
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(" - FAQ - \r\r");
            foreach (DataRow row in table.Rows)
            {
                builder.Append("Q: " + ((string) row["question"]) + "\r");
                builder.Append("A: " + ((string) row["answer"]) + "\r\r");
            }
            this.Session.SendNotif(builder.ToString());
        }

        internal void FinVotacion()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                if (!currentRoom.votacionEnCurso)
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("votation.nocourse"));
                }
                else if (!currentRoom.sbdyVoted)
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("votation.endednovote"));
                    currentRoom.votacionEnCurso = false;
                }
                else
                {
                    List<RoomUser> roomUsers = currentRoom.GetRoomUserManager().GetRoomUsers();
                    int votes = 0;
                    string username = "";
                    string str2 = "";
                    foreach (RoomUser user in roomUsers)
                    {
                        if (user.votes >= votes)
                        {
                            votes = user.votes;
                            username = user.GetUsername();
                        }
                        if (user.votes > 1)
                        {
                            object obj2 = str2;
                            str2 = string.Concat(new object[] { obj2, "\r", LanguageLocale.GetValue("votation.enduse"), user.GetUsername(), LanguageLocale.GetValue("votation.endvot"), user.votes });
                            user.votes = 0;
                            user.alreadyVoted = false;
                        }
                    }
                    ServerMessage message = new ServerMessage();
                    message.Init(Outgoing.BroadcastMessage);
                    message.AppendString(string.Concat(new object[] { LanguageLocale.GetValue("votation.endedin"), username, LanguageLocale.GetValue("votation.endedmi"), votes, "votos.\r", LanguageLocale.GetValue("votation.endparc"), str2 }));
                    message.AppendString("");
                    currentRoom.QueueRoomMessage(message);
                    currentRoom.votacionEnCurso = false;
                    currentRoom.sbdyVoted = false;
                }
            }
        }

        internal void Fly()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            RoomUser roomUserByHabbo = null;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.isFlying = true;
                    roomUserByHabbo.AllowOverride = true;
                }
            }
        }

        internal void forcerot()
        {
            try
            {
                int num = int.Parse(this.Params[1]);
                if ((num <= -1) || (num >= 7))
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("forcerot.inputerror"));
                }
                else
                {
                    this.Session.GetHabbo().forceRot = num;
                }
            }
            catch
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
            }
        }

        internal void freeze()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            RoomUser roomUserByHabbo = this.Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Params[1]);
            if (roomUserByHabbo != null)
            {
                roomUserByHabbo.Freezed = !roomUserByHabbo.Freezed;
            }
        }

        private static string GetInput(string[] Params)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str in Params)
            {
                builder.Append(str + " ");
            }
            return builder.ToString();
        }

        internal void giveBadge()
        {
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(this.Params[1]);
            if (clientByUsername != null)
            {
                clientByUsername.GetHabbo().GetBadgeComponent().GiveBadge(ButterflyEnvironment.FilterInjectionChars(this.Params[2]), true);
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, clientByUsername.GetHabbo().Username, "Badge", "Badge given to user [" + this.Params[2] + "]");
            }
            else
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
        }

        internal void giveCrystals()
        {
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(this.Params[1]);
            if (clientByUsername != null)
            {
                int num;
                if (int.TryParse(this.Params[2], out num))
                {
                    clientByUsername.GetHabbo().Crystals += num;
                    clientByUsername.GetHabbo().GiveUserCrystals(int.Parse(this.Params[2]));
                    clientByUsername.GetHabbo().UpdateDiamondsBalance();
                    clientByUsername.SendNotif(this.Session.GetHabbo().Username + LanguageLocale.GetValue("crystal.awardmessage1") + num.ToString() + LanguageLocale.GetValue("crystal.awardmessage2"));
                    this.Session.SendNotif(LanguageLocale.GetValue("crystal.updateok"));
                }
                else
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
                }
            }
            else
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
        }
        }

        internal void globalcredits()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            try
            {
                int amount = int.Parse(this.Params[1]);
                ButterflyEnvironment.GetGame().GetClientManager().QueueCreditsUpdate(amount);
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery("UPDATE users SET credits = credits + " + amount);
                }
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "Mass Credits", "Send [" + amount + "] credits to everyone in the database");
            }
            catch
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
            }
        }

        internal void handitem()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            RoomUser roomUserByHabbo = null;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    try
                    {
                        roomUserByHabbo.CarryItem(int.Parse(this.Params[1]));
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void hotelalert()
        {
            string str = GetInput(this.Params).Substring(4);
            ServerMessage message = new ServerMessage(Outgoing.BroadcastMessage);
            message.AppendString(LanguageLocale.GetValue("hotelallert.notice") + "\r\n" + str + "\r\n- " + this.Session.GetHabbo().Username);
            ButterflyEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(message);
            ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "HotelAlert", "Hotel alert [" + str + "]");
        }

        internal void info()
        {
            //Removing credits is disrespectful
            //Removing credits is disrespectful
            TimeSpan span = (TimeSpan)(DateTime.Now - ButterflyEnvironment.ServerStarted);
            //Removing credits is disrespectful
           this.Session.SendNotif(string.Concat("\rCredits: by AKllX @ forum.ragezone.com \r Status: Online \r ", LanguageLocale.GetValue("info.serveruptime"), span.Days, " day(s), ", span.Hours, " hour(s) and ", span.Minutes, " minute(s)\r", LanguageLocale.GetValue("info.onlineusers"), ButterflyEnvironment.GetGame().GetClientManager().ClientCount, "\r", LanguageLocale.GetValue("info.activerooms"), ButterflyEnvironment.GetGame().GetRoomManager().LoadedRoomsCount, "\r\rCreditos:\rMeth0d\rMatinmine\rCarlos D\rSuper0ca\rLeenster\rMoogly\rNiels\rAKllX"));
            //Removing credits is disrespectful

        }

        internal void invisible()
        {
            if (this.Session.GetHabbo().SpectatorMode)
            {
                this.Session.GetHabbo().SpectatorMode = false;
                this.Session.SendNotif(LanguageLocale.GetValue("invisible.enabled"));
            }
            else
            {
                this.Session.GetHabbo().SpectatorMode = true;
                this.Session.SendNotif(LanguageLocale.GetValue("invisible.disabled"));
            }
        }

        internal void kick()
        {
            string username = null;
            GameClient session = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            username = this.Params[1];
            session = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(username);
            if (session == null)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
            else if (this.Session.GetHabbo().Rank <= session.GetHabbo().Rank)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("kick.notallwed"));
            }
            else if (session.GetHabbo().CurrentRoomId < 1)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("kick.error"));
            }
            else
            {
                currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(session.GetHabbo().CurrentRoomId);
                if (currentRoom != null)
                {
                    currentRoom.GetRoomUserManager().RemoveUserFromRoom(session, true, false);
                    session.CurrentRoomUserID = -1;
                    if (this.Params.Length > 2)
                    {
                        session.SendNotif(LanguageLocale.GetValue("kick.withmessage") + MergeParams(this.Params, 2));
                    }
                    else
                    {
                        session.SendNotif(LanguageLocale.GetValue("kick.nomessage"));
                    }
                }
            }
        }

        internal void langban()
        {
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(this.Params[1]);
            if (clientByUsername == null)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
            else if (clientByUsername.GetHabbo().Rank >= this.Session.GetHabbo().Rank)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("ban.notallowed"));
            }
            else
            {
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, clientByUsername.GetHabbo().Username, "Ban", "Long ban for 31536000 seconds");
                ButterflyEnvironment.GetGame().GetBanManager().BanUser(clientByUsername, this.Session.GetHabbo().Username, 31536000.0, "This is an english hotel. Therefore, brazilian, portugeese and spanish users has to find their own retro, and not mess with an english one.", true);
            }
        }

        internal void language()
        {
            DataRow row;
            string query = this.Params[1];
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT country FROM users JOIN ip_cache ON (users.ip_last = ip_cache.ip) AND username = @username");
                adapter.addParameter("username", query);
                row = adapter.getRow();
            }
            this.Session.SendNotif(query + LanguageLocale.GetValue("language.notif") + ((string) row["country"]));
        }

        internal void lay()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            RoomUser roomUserByHabbo = null;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    try
                    {
                        if (roomUserByHabbo.sentadoBol)
                        {
                            roomUserByHabbo.sentadoBol = false;
                            roomUserByHabbo.RemoveStatus("sit");
                        }
                        if (!roomUserByHabbo.Statusses.ContainsKey("lay"))
                        {
                            if ((roomUserByHabbo.RotBody % 2) == 0)
                            {
                                roomUserByHabbo.AddStatus("lay", Convert.ToString((double) (currentRoom.GetGameMap().Model.SqFloorHeight[roomUserByHabbo.X, roomUserByHabbo.Y] + 0.55)).Replace(",", "."));
                                roomUserByHabbo.acostadoBol = true;
                                roomUserByHabbo.UpdateNeeded = true;
                            }
                            else
                            {
                                this.Session.SendNotif(LanguageLocale.GetValue("diag.noaction"));
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        internal void linkAlert()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            string s = this.Params[1];
            string str2 = MergeParams(this.Params, 2);
            ServerMessage message = new ServerMessage(Outgoing.SendNotif);
            message.AppendString(LanguageLocale.GetValue("hotelallert.notice") + "\r\n" + str2 + "\r\n-" + this.Session.GetHabbo().Username);
            message.AppendString(s);
            ButterflyEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(message);
        }

        internal void massbadge()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            ButterflyEnvironment.GetGame().GetClientManager().QueueBadgeUpdate(this.Params[1]);
            ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "Badge", "Mass badge with badge [" + this.Params[1] + "]");
        }

        public void masscredits()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            try
            {
                int amount = int.Parse(this.Params[1]);
                ButterflyEnvironment.GetGame().GetClientManager().QueueCreditsUpdate(amount);
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "Mass Credits", "Send [" + amount + "] credits to everyone online");
            }
            catch
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
            }
        }

        internal static string MergeParams(string[] Params, int Start)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Params.Length; i++)
            {
                if (i >= Start)
                {
                    if (i > Start)
                    {
                        builder.Append(" ");
                    }
                    builder.Append(Params[i]);
                }
            }
            return builder.ToString();
        }

        internal void moonwalk()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            if (currentRoom != null)
            {
                RoomUser roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                roomUserByHabbo.moonwalkEnabled = !roomUserByHabbo.moonwalkEnabled;
                if (roomUserByHabbo.moonwalkEnabled)
                {
                    this.Session.SendNotif("Moonwalk enabled");
                }
                else
                {
                    this.Session.SendNotif("Moonwalk disabled");
                }
            }
        }

        internal void mordi()
        {
            this.Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(60);
        }

        internal void mute()
        {
            string username = null;
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            username = this.Params[1];
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(username);
            if ((clientByUsername == null) || (clientByUsername.GetHabbo() == null))
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
            else if (clientByUsername.GetHabbo().Rank >= this.Session.GetHabbo().Rank)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("mute.notallowed"));
            }
            else
            {
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, clientByUsername.GetHabbo().Username, "Mute", "Muted user");
                clientByUsername.GetHabbo().Mute();
            }
        }

        internal void nuevaVotacion()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                if (currentRoom.votacionEnCurso)
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("votation.incourse"));
                }
                else
                {
                    string str = MergeParams(this.Params, 1);
                    ServerMessage message = new ServerMessage();
                    message.Init(Outgoing.SendNotif);
                    message.AppendInt32(1);
                    message.AppendString(LanguageLocale.GetValue("votation.introduction") + str + LanguageLocale.GetValue("votation.ending"));
                    currentRoom.QueueRoomMessage(message);
                    currentRoom.votacionEnCurso = true;
                    currentRoom.sbdyVoted = false;
                }
            }
        }

        internal void openroom()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            try
            {
                uint id = uint.Parse(this.Params[1]);
                this.Session.GetMessageHandler().PrepareRoomForUser(id, "");
            }
            catch
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
            }
        }

        internal void overridee()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            RoomUser roomUserByHabbo = null;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    if (roomUserByHabbo.AllowOverride)
                    {
                        roomUserByHabbo.AllowOverride = false;
                        this.Session.SendNotif(LanguageLocale.GetValue("override.disabled"));
                    }
                    else
                    {
                        roomUserByHabbo.AllowOverride = true;
                        this.Session.SendNotif(LanguageLocale.GetValue("override.enabled"));
                    }
                    currentRoom.GetGameMap().GenerateMaps(true);
                }
            }
        }

        internal void pickall()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            try
            {
                if ((room != null) && (room.OwnerId == this.Session.GetHabbo().Id))
                {
                    List<RoomItem> roomItemList = room.GetRoomItemHandler().RemoveAllFurniture(this.Session);
                    room.GetRoomItemHandler().RemoveItemsByOwner(ref roomItemList, ref this.Session);
                }
            }
            catch
            {
            }
        }


        internal void pixels()
        {
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(this.Params[1]);
            if (clientByUsername != null)
            {
                int num;
                if (int.TryParse(this.Params[2], out num))
                {
                    clientByUsername.GetHabbo().ActivityPoints += num;
                    clientByUsername.GetHabbo().UpdateActivityPointsBalance(true);
                    clientByUsername.SendNotif(this.Session.GetHabbo().Username + LanguageLocale.GetValue("pixels.awardmessage1") + num.ToString() + LanguageLocale.GetValue("pixels.awardmessage2"));
                    this.Session.SendNotif(LanguageLocale.GetValue("pixels.updateok"));
                }
                else
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
                }
            }
            else
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
        }

        internal void powerlevels()
        {
            this.Session.SendNotif("Powerlevel: " + ButterflyEnvironment.GetRandomNumber(0x2329, 0x2710) + " (Over the FUCKING 9000)");
        }

        internal void pull()
        {
            Room room = Session.GetHabbo().CurrentRoom;
            if (room == null)
                return;


            RoomUser roomuser = room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (roomuser == null)
                return;

            if (Params.Length == 1)
            {
                Point squareInFront = CoordinationUtil.GetPointInFront(roomuser.Coordinate, roomuser.RotBody);
                Point squareInFrontInFront = CoordinationUtil.GetPointBehind(squareInFront, CoordinationUtil.RotationIverse(roomuser.RotBody));
                List<RoomUser> users = room.GetGameMap().GetRoomUsers(squareInFrontInFront);

                foreach (RoomUser user in users)
                {
                    user.MoveTo(squareInFront);
                }
            }
            else
            {
                RoomUser roomuserTarget = room.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
                if (roomuserTarget == null)
                    return;

                Point closestSquare = CoordinationUtil.GetPointBehind(roomuserTarget.Coordinate, roomuserTarget.RotBody);

                Point a = new Point(closestSquare.X, closestSquare.Y++);
                if (CoordinationUtil.GetDistance(closestSquare, a) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = a;

                Point b = new Point(closestSquare.X, closestSquare.Y--);
                if (CoordinationUtil.GetDistance(closestSquare, b) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = b;

                Point c = new Point(closestSquare.X++, closestSquare.Y);
                if (CoordinationUtil.GetDistance(closestSquare, c) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = c;

                Point d = new Point(closestSquare.X--, closestSquare.Y++);
                if (CoordinationUtil.GetDistance(closestSquare, d) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = d;

                Point e = new Point(closestSquare.X++, closestSquare.Y--);
                if (CoordinationUtil.GetDistance(closestSquare, e) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = e;

                Point f = new Point(closestSquare.X--, closestSquare.Y);
                if (CoordinationUtil.GetDistance(closestSquare, f) < CoordinationUtil.GetDistance(closestSquare, roomuserTarget.Coordinate))
                    closestSquare = f;

                roomuserTarget.MoveTo(closestSquare);
            }
        }

        internal void push()
        {
            Room room = Session.GetHabbo().CurrentRoom;
            if (room == null)
                return;

            RoomUser roomuser = room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (roomuser == null)
                return;

            if (Params.Length == 1)
            {

                Point squareInfront = CoordinationUtil.GetPointInFront(roomuser.Coordinate, roomuser.RotBody);
                List<RoomUser> users = room.GetGameMap().GetRoomUsers(squareInfront);

                Point squareInFrontOfUserInFront = CoordinationUtil.GetPointInFront(squareInfront, roomuser.RotBody);
                if (room.GetGameMap().CanWalk(squareInFrontOfUserInFront.X, squareInFrontOfUserInFront.Y, false) == false)
                {
                    return;
                }

                foreach (RoomUser user in users)
                {
                    user.MoveTo(squareInFrontOfUserInFront);
                }
            }
            else
            {
                RoomUser roomuserTarget = room.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
                if (roomuserTarget == null)
                    return;

                Point furtherstSquare = CoordinationUtil.GetPointBehind(roomuserTarget.Coordinate, roomuserTarget.RotBody);

                Point a = new Point(furtherstSquare.X, furtherstSquare.Y++);
                if (CoordinationUtil.GetDistance(furtherstSquare, a) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = a;

                Point b = new Point(furtherstSquare.X, furtherstSquare.Y--);
                if (CoordinationUtil.GetDistance(furtherstSquare, b) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = b;

                Point c = new Point(furtherstSquare.X++, furtherstSquare.Y);
                if (CoordinationUtil.GetDistance(furtherstSquare, c) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = c;

                Point d = new Point(furtherstSquare.X--, furtherstSquare.Y++);
                if (CoordinationUtil.GetDistance(furtherstSquare, d) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = d;

                Point e = new Point(furtherstSquare.X++, furtherstSquare.Y--);
                if (CoordinationUtil.GetDistance(furtherstSquare, e) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = e;

                Point f = new Point(furtherstSquare.X--, furtherstSquare.Y);
                if (CoordinationUtil.GetDistance(furtherstSquare, f) > CoordinationUtil.GetDistance(furtherstSquare, roomuserTarget.Coordinate))
                    furtherstSquare = f;

                roomuserTarget.MoveTo(furtherstSquare);
            }
        }

        internal void refreshitems()
        {
            ButterflyEnvironment.GetGame().reloaditems();
            this.Session.SendNotif("Item definition reloaded.");
            ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "RefreshItems", "Refreshed items definitions.");
        }

        internal void registerIRC()
        {
        }

        internal void roomalert()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                string s = MergeParams(this.Params, 1);
                ServerMessage message = new ServerMessage();
                message.Init(Outgoing.SetCommandsView);
                message.AppendInt32(1);
                message.AppendString(s);
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "Alert", "Room alert with message [" + s + "]");
                currentRoom.QueueRoomMessage(message);
            }
        }

        internal void roombadge()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            if (this.Session.GetHabbo().CurrentRoom != null)
            {
                currentRoom.QueueRoomBadge(this.Params[1]);
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "Badge", string.Concat(new object[] { "Roombadge in room [", currentRoom.RoomId, "] with badge [", this.Params[1], "]" }));
            }
        }

        internal void roomkick()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                string allert = MergeParams(this.Params, 1);
                RoomKick kick = new RoomKick(allert, (int) this.Session.GetHabbo().Rank);
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "Room kick", "Kicked the whole room");
                currentRoom.QueueRoomKick(kick);
            }
        }

        internal void roommute()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            if (this.Session.GetHabbo().CurrentRoom.RoomMuted)
            {
                this.Session.GetHabbo().CurrentRoom.RoomMuted = false;
            }
            else
            {
                this.Session.GetHabbo().CurrentRoom.RoomMuted = true;
            }
            ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "Room Mute", "Room muted");
        }

        internal void seteffect()
        {
            this.Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(int.Parse(this.Params[1]));
        }

        internal void setmax()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            currentRoom = this.Session.GetHabbo().CurrentRoom;
            uint roomId = currentRoom.RoomId;
            try
            {
                int maxUsers = int.Parse(this.Params[1]);
                if (((maxUsers > 100) && (this.Session.GetHabbo().Rank == 1)) || (maxUsers > 200))
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("setmax.maxusersreached"));
                }
                else
                {
                    currentRoom.SetMaxUsers(maxUsers);
                }
            }
            catch
            {
            }
        }

        internal void setspeed()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            currentRoom = this.Session.GetHabbo().CurrentRoom;
            if ((currentRoom != null) && currentRoom.CheckRights(this.Session, true))
            {
                try
                {
                    this.Session.GetHabbo().CurrentRoom.GetRoomItemHandler().SetSpeed(int.Parse(this.Params[1]));
                }
                catch
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("input.intonly"));
                }
            }
        }

        internal void shutdown()
        {
            Logging.LogCriticalException("User " + this.Session.GetHabbo().Username + " shut down the server " + DateTime.Now.ToString());
            new Task(new Action(ButterflyEnvironment.PreformShutDown)).Start();
        }

        internal void sit()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            RoomUser roomUserByHabbo = null;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    try
                    {
                        if (roomUserByHabbo.acostadoBol)
                        {
                            roomUserByHabbo.acostadoBol = false;
                            roomUserByHabbo.RemoveStatus("lay");
                        }
                        if (!roomUserByHabbo.Statusses.ContainsKey("sit"))
                        {
                            if ((roomUserByHabbo.RotBody % 2) == 0)
                            {
                                roomUserByHabbo.AddStatus("sit", Convert.ToString((double) (currentRoom.GetGameMap().Model.SqFloorHeight[roomUserByHabbo.X, roomUserByHabbo.Y] + 0.55)).Replace(",", "."));
                                roomUserByHabbo.sentadoBol = true;
                                roomUserByHabbo.UpdateNeeded = true;
                            }
                            else
                            {
                                this.Session.SendNotif(LanguageLocale.GetValue("diag.noaction"));
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                foreach (RoomItem item in currentRoom.GetRoomItemHandler().mFloorItems.Values)
                {
                    if (item.GetBaseItem().InteractionType == InteractionType.gld_wall)
                        item.UpdateState();
                }
                
            }
        }

        internal void staffmessages()
        {
            string str = MergeParams(this.Params, 1);
            ServerMessage message = new ServerMessage(Outgoing.BroadcastMessage);
            message.AppendString(LanguageLocale.GetValue("staffmessage.starting") + this.Session.GetHabbo().Username + LanguageLocale.GetValue("staffmessage.ending") + "\r\n" + str);
            ButterflyEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(message, "fuse_mod");
            ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, string.Empty, "Staff Message", "Sent staff message:" + str);
        }

        internal void stalk()
        {
            GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(this.Params[1]);
            if (((clientByUsername != null) && (clientByUsername.GetHabbo() != null)) && (clientByUsername.GetHabbo().CurrentRoom != null))
            {
                this.Session.GetMessageHandler().PrepareRoomForUser(clientByUsername.GetHabbo().CurrentRoom.RoomId, "");
            }
        }

        internal void startquestion()
        {
            try
            {
                string str;
                IQueryAdapter adapter;
                Room Room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
                DataTable table = null;
                int QuestionId = int.Parse(this.Params[1]);
                Room.CurrentPollId = QuestionId;
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("SELECT question FROM infobus_questions WHERE id = '" + QuestionId + "' LIMIT 1");
                    str = adapter.getString();
                }
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("SELECT * FROM infobus_answers WHERE question_id = '" + QuestionId + "'");
                    table = adapter.getTable();
                }
                ServerMessage message = new ServerMessage(Outgoing.PollQuestionStart);
                message.AppendString(str);
                message.AppendInt32(table.Rows.Count);
                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        message.AppendInt32((int) row["id"]);
                        message.AppendString((string) row["answer_text"]);
                    }
                }
                Room.SendMessage(message);
                new Thread(() => Room.ShowResults(Room, QuestionId, this.Session)).Start();
            }
            catch
            {
                this.Session.SendNotif("Wrong syntax.");
            }
        }

        internal void superban()
        {
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(this.Params[1]);
            if (clientByUsername == null)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
            else if (clientByUsername.GetHabbo().Rank >= this.Session.GetHabbo().Rank)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("ban.notallowed"));
            }
            else
            {
                int num;
                try
                {
                    num = int.Parse(this.Params[2]);
                }
                catch (FormatException)
                {
                    return;
                }
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, clientByUsername.GetHabbo().Username, "Ban", "Long ban for " + num + " seconds");
                if (num <= 600)
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("ban.toolesstime"));
                }
                else
                {
                    ButterflyEnvironment.GetGame().GetBanManager().BanUser(clientByUsername, this.Session.GetHabbo().Username, (double) num, MergeParams(this.Params, 3), true);
                }
            }
        }

        internal void teleport()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            RoomUser roomUserByHabbo = null;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
            if (roomUserByHabbo != null)
            {
                roomUserByHabbo.TeleportEnabled = !roomUserByHabbo.TeleportEnabled;
                currentRoom.GetGameMap().GenerateMaps(true);
            }
        }

        internal void unbanUser()
        {
            if (this.Params.Length > 1)
            {
                ButterflyEnvironment.GetGame().GetBanManager().UnbanUser(this.Params[1]);
            }
        }

        internal void unload()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            ButterflyEnvironment.GetGame().GetRoomManager().UnloadRoom(currentRoom);
        }

        internal void unmute()
        {
            string username = null;
            GameClient clientByUsername = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            username = this.Params[1];
            clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(username);
            if ((clientByUsername == null) || (clientByUsername.GetHabbo() == null))
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.usernotfound"));
            }
            else
            {
                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, clientByUsername.GetHabbo().Username, "Mute", "Un Muted user");
                clientByUsername.GetHabbo().Unmute();
            }
        }

        internal void userinfo()
        {
            string str = this.Params[1];
            bool flag = true;
            if (string.IsNullOrEmpty(str))
            {
                this.Session.SendNotif(LanguageLocale.GetValue("input.userparammissing"));
            }
            else
            {
                GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(str);
                if ((clientByUsername == null) || (clientByUsername.GetHabbo() == null))
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("input.useroffline"));
                }
                else
                {
                    Habbo habbo = clientByUsername.GetHabbo();
                    StringBuilder builder = new StringBuilder();
                    if (habbo.CurrentRoom != null)
                    {
                        builder.Append(string.Concat(new object[] { " - ", LanguageLocale.GetValue("roominfo.title"), " [", habbo.CurrentRoom.RoomId, "] - \r" }));
                        builder.Append(LanguageLocale.GetValue("userinfo.owner") + habbo.CurrentRoom.Owner + "\r");
                        builder.Append(LanguageLocale.GetValue("userinfo.roomname") + habbo.CurrentRoom.Name + "\r");
                        builder.Append(string.Concat(new object[] { LanguageLocale.GetValue("userinfo.usercount"), habbo.CurrentRoom.UserCount, "/", habbo.CurrentRoom.UsersMax }));
                    }
                    this.Session.SendNotif(string.Concat(new object[] { 
                        LanguageLocale.GetValue("userinfo.userinfotitle"), str, ":\r", LanguageLocale.GetValue("userinfo.rank"), habbo.Rank, " \r", LanguageLocale.GetValue("userinfo.isonline"), flag.ToString(), " \r", LanguageLocale.GetValue("userinfo.userid"), habbo.Id, " \r", LanguageLocale.GetValue("userinfo.visitingroom"), habbo.CurrentRoomId, " \r", LanguageLocale.GetValue("userinfo.motto"), 
                        habbo.Motto, " \r", LanguageLocale.GetValue("userinfo.credits"), habbo.Credits, " \r", LanguageLocale.GetValue("userinfo.ismuted"), habbo.Muted.ToString(), "\r\r\r", builder.ToString()
                     }));
                }
            }
        }

        internal void votar()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (currentRoom != null)
            {
                if (!currentRoom.votacionEnCurso)
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("votation.nocourse"));
                }
                else
                {
                    string username = MergeParams(this.Params, 1);
                    RoomUser roomUserByHabbo = null;
                    RoomUser user2 = null;
                    GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(username);
                    if (((clientByUsername == null) || (clientByUsername.GetHabbo() == null)) || (this.Session.GetHabbo().CurrentRoomId != clientByUsername.GetHabbo().CurrentRoomId))
                    {
                        this.Session.SendNotif(LanguageLocale.GetValue("votation.distroom"));
                    }
                    else
                    {
                        roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(clientByUsername.GetHabbo().Id);
                        user2 = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                        if (user2.alreadyVoted)
                        {
                            this.Session.SendNotif(LanguageLocale.GetValue("votation.already"));
                        }
                        else
                        {
                            roomUserByHabbo.votes++;
                            user2.alreadyVoted = true;
                            user2.Chat(null, LanguageLocale.GetValue("votation.votmes"), false, 0);
                            currentRoom.sbdyVoted = true;
                        }
                    }
                }
            }
        }

        internal void warp()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            RoomUser roomUserByHabbo = null;
            currentRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
            if ((roomUserByHabbo != null) && currentRoom.CheckRights(this.Session, false))
            {
                roomUserByHabbo.TeleportEnabled = !roomUserByHabbo.TeleportEnabled;
                currentRoom.GetGameMap().GenerateMaps(true);
            }
        }

        internal bool WasExecuted()
        {
            ChatCommand command = ChatCommandRegister.GetCommand(this.Params[0].Substring(1).ToLower());
            if (command.UserGotAuthorization(this.Session))
            {
                ChatCommandRegister.InvokeCommand(this, command.commandID);
                this.Dispose();
                return true;
            }
            this.Dispose();
            return false;
        }

        internal void wheresmypet()
        {
        }

        internal void whosonline()
        {
        }
    }
}

