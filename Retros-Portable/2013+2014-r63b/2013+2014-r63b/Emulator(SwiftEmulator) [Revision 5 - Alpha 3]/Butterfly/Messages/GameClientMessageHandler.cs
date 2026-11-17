namespace Butterfly.Messages
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Catalogs;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Games.SnowStorm;
    using Butterfly.HabboHotel.ChatMessageStorage;
    using Butterfly.HabboHotel.Habbo.Guilds;
    using Butterfly.HabboHotel.Habbo.SoundMachine;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Misc;
    using Butterfly.HabboHotel.Navigators;
    using Butterfly.HabboHotel.Pathfinding;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.Quests;
    using Butterfly.HabboHotel.RoomBots;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Support;
    using Butterfly.HabboHotel.Users.Badges;
    using Butterfly.HabboHotel.Users.Messenger;
    using Butterfly.Messages.StaticMessageHandlers;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;

    using HabboEvents;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Text.RegularExpressions;

    internal class GameClientMessageHandler
    {
        internal Room CurrentLoadingRoom;
        private int FloodCount;
        private DateTime FloodTime;
        private ClientMessage Request;
        private ServerMessage Response;
        private GameClient Session;

        internal GameClientMessageHandler(GameClient Session)
        {
            this.Session = Session;
            this.Response = new ServerMessage();
        }

        public void AcceptMember()
        {
            int guildId = this.Request.PopWiredInt32();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                int num2 = this.Request.PopWiredInt32();
                Habbo habbo = ButterflyEnvironment.getHabboForId((uint)num2);
                if (habbo != null)
                {
                    Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom((uint)guild.RoomId);
                    guild.Petitions.Remove((int)habbo.Id);
                    string str = "";
                    foreach (int num3 in guild.Petitions)
                    {
                        str = str + num3 + ";";
                    }
                    if (guild.Petitions.Count > 1)
                    {
                        str = str.Substring(0, str.Length - 1);
                    }
                    guild.Members.Add((int)habbo.Id,DateTime.Now.Day + " - " + DateTime.Now.Month + " -  " + DateTime.Now.Year);
                    if (habbo.FavoriteGuild == 0)
                    {
                        habbo.FavoriteGuild = guild.Id;
                        if (habbo.CurrentRoomId > 0)
                        {
                            if (room == null)
                            {
                                return;
                            }
                            List<RoomUser> list = new List<RoomUser>(ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(habbo.CurrentRoomId).GetRoomUserManager().UserList.Values);
                            ServerMessage message = new ServerMessage(Outgoing.SendGroup);
                            message.AppendInt32(1);
                            message.AppendInt32(guild.Id);
                            message.AppendString(guild.Image);
                            habbo.GetClient().SendMessage(message);
                            ServerMessage message2 = new ServerMessage(Outgoing.SetRoomUser);
                            message2.AppendInt32(1);
                            foreach (RoomUser user in list)
                            {
                                if (user.HabboId == habbo.Id)
                                {
                                    user.Serialize(message2, false);
                                }
                            }
                            room.SendMessage(message2);
                        }
                    }
                    habbo.ImGuilds.Add(guild.Id);
                    guild.GuildRanks.Add((int)habbo.Id, 2);
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET FavoriteGroup = '", habbo.FavoriteGuild, "' WHERE Id = '", habbo.Id, "'" }));
                        adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_groups VALUES (NULL, '", habbo.Username, "','", habbo.Id, "','", guild.Id, "','", DateTime.Now.Day, " - ", DateTime.Now.Month, " -  ", DateTime.Now.Year, "', '2')" }));
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE groups SET Petitions = '", str, "' WHERE Id = '", guild.Id, "'" }));
                        adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_stream VALUES (NULL, '", habbo.Id, "','", ButterflyEnvironment.LinkConfig, guild.Image, ".gif','10','", guild.Id, "','0','')" }));
                    }
                    //guild = ButterflyEnvironment.GetGame().GetGuilds().ReloadGuild(guildId);
                    
                    ServerMessage message3 = new ServerMessage(Outgoing.AddNewMember);
                    message3.AppendInt32(guildId);
                    message3.AppendInt32(guild.GuildRanks[num2]);
                    message3.AppendInt32(habbo.Id);
                    message3.AppendString(habbo.Username);
                    message3.AppendString(habbo.Look);
                    message3.AppendString(string.Concat(new object[] { DateTime.Now.Month, " ", DateTime.Now.Day, ", ", DateTime.Now.Year }));
                    this.Session.SendMessage(message3);

                    ServerMessage message4 = new ServerMessage(Outgoing.UpdatePetitionsGuild);
                    message4.AppendInt32(1);
                    message4.AppendInt32(guild.Id);
                    message4.AppendInt32(3);
                    message4.AppendString(guild.Name);
                    this.Session.SendMessage(message4);
                    if (ButterflyEnvironment.GetGame().GetClientManager().GetClient(habbo.Id) != null)
                    {
                        habbo.GetClient().SendMessage(message4);
                    }
                    if ((ButterflyEnvironment.GetGame().GetClientManager().GetClient(habbo.Id) != null) && (room != null))
                    {
                        ServerMessage message5 = new ServerMessage(Outgoing.SetRoomUser);
                        message5.AppendInt32(1);
                        room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id).Serialize(message5, false);
                        room.SendMessage(message5);
                    }
                    ServerMessage message6 = new ServerMessage(Outgoing.SendHtmlColors);
                    message6.AppendInt32(this.Session.GetHabbo().ImGuilds.Count);
                    foreach (int num4 in this.Session.GetHabbo().ImGuilds)
                    {
                        MyGuild guild2 = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(num4);
                        message6.AppendInt32(guild2.Id);
                        message6.AppendString(guild2.Name);
                        message6.AppendString(guild2.Image);
                        message6.AppendString(guild2.HtmlColor1);
                        message6.AppendString(guild2.HtmlColor2);
                        message6.AppendBoolean(guild2.Id == this.Session.GetHabbo().FavoriteGuild);
                    }
                    this.Session.SendMessage(message6);
                    RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData((uint)guild.RoomId);
                    if (ButterflyEnvironment.GetGame().GetClientManager().GetClient(habbo.Id) != null)
                    {
                        ServerMessage message7 = new ServerMessage(Outgoing.SendAdvGroupInit);
                        message7.AppendInt32(guild.Id);
                        message7.AppendBoolean(true);
                        message7.AppendInt32(guild.Type);
                        message7.AppendString(guild.Name);
                        message7.AppendString(guild.Description);
                        message7.AppendString(guild.Image);
                        message7.AppendInt32(data.Id);
                        message7.AppendString(data.Name);
                        if (guild.Petitions.Contains((int)habbo.Id))
                        {
                            message7.AppendInt32(2);
                        }
                        else if (!habbo.ImGuilds.Contains(guild.Id))
                        {
                            message7.AppendInt32(0);
                        }
                        else if (habbo.ImGuilds.Contains(guild.Id))
                        {
                            message7.AppendInt32(1);
                        }
                        message7.AppendInt32(guild.Members.Count);
                        message7.AppendBoolean(false);
                        message7.AppendString(guild.DateCreated);

                        message7.AppendBoolean(habbo.Id == guild.OwnerId);

                        if (habbo.ImGuilds.Contains(guild.Id))
                        {
                            if (guild.GuildRanks[(int)habbo.Id] < 2)
                                message7.AppendBoolean(true);
                            else message7.AppendBoolean(false);
                        }
                        else message7.AppendBoolean(false);
                        message7.AppendString(guild.OwnerName);
                        message7.AppendBoolean(true);
                        message7.AppendBoolean(true);
                        message7.AppendInt32(guild.Members.ContainsKey((int)habbo.Id) ? guild.Petitions.Count : 0);
                        habbo.GetClient().SendMessage(message7);
                    }
                }
            }
        }
        internal void PlaceBot()
        {
            Room Room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (Room == null || (!Room.AllowPets && !Room.CheckRights(Session, true)) || !Room.CheckRights(Session, true))
            {
                return;
            }
            uint PetId = Request.PopWiredUInt();
            RoomBot Bot = Session.GetHabbo().GetInventoryComponent().GetBot(PetId);
            if (Bot == null)
            {
                return;
            }

            int X = Request.PopWiredInt32();
            int Y = Request.PopWiredInt32();
            if (!Room.GetGameMap().CanWalk(X, Y, false))
            {
                return;
            }
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("UPDATE user_bots SET room_id = '" + Room.RoomId + "', x = '" + X + "', y = '" + Y + "' WHERE id = '" + PetId + "'");
            }
            Bot.RoomId = Room.RoomId;
            List<RandomSpeech> RndSpeechList = new List<RandomSpeech>();
            List<BotResponse> BotResponse = new List<Butterfly.HabboHotel.RoomBots.BotResponse>();
            RoomUser PetUser = Room.GetRoomUserManager().DeployBot(new RoomBot(PetId, Bot.RoomId, AIType.Generic, "freeroam", Bot.Name, Bot.Motto, Bot.Look, X, Y, 0, 0, 0, 0, 0, 0, ref RndSpeechList, ref BotResponse, Bot.OwnerId), null);

            Session.GetHabbo().GetInventoryComponent().MoveBotToRoom(PetId);
            Session.SendMessage(Session.GetHabbo().GetInventoryComponent().SerializeBotInventory());
        }
        internal void PickBot()
        {

            uint BotId = Request.PopWiredUInt();
            Room Room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            RoomUser BotUser = Room.GetRoomUserManager().GetBot(BotId);
            uint ownerID = 0;  //I really don't like doing this, but Bots were bad coded and NOT by me, so its not my fault.
            using(IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                try
                {
                    adapter.setQuery("SELECT user_id FROM user_bots WHERE id = " + BotId);
                    ownerID = Convert.ToUInt32((adapter.getInteger()));
                }
                catch(Exception ex)
                {
                    Logging.LogQueryError(ex,"Error @ Bot pickup for Bot:" + BotId);
                }
            }

            if ((Session == null) || (Session.GetHabbo() == null) || (Session.GetHabbo().GetInventoryComponent() == null) || (BotUser == null) || (this.Session.GetHabbo().Id != ownerID))
                return;

            BotUser.BotData.RoomId = 0;
            
            Session.GetHabbo().GetInventoryComponent().AddBot(BotUser.BotData);

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                dbClient.runFastQuery("UPDATE user_bots SET room_id = '0' WHERE id = '" + BotId + "' LIMIT 1");

            Room.GetRoomUserManager().RemoveBot(BotUser.VirtualId, false);
            Session.SendMessage(Session.GetHabbo().GetInventoryComponent().SerializeBotInventory());

        }

        internal void HandleBotSpeechList()
        {
            int botId = this.Request.PopWiredInt32();
            int skillType = this.Request.PopWiredInt32();
            switch (skillType)
            {
                case 2:
                    {
                        string truncatedText = "";
                        using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            DataTable dt;
                            DataRow speakingRow;
                            dbClient.setQuery("SELECT * FROM bots_speech WHERE bot_id = " + botId);
                            dt = dbClient.getTable();
                            if (dt != null)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    truncatedText += (dr["text"] + "\n");
                                }
                                dbClient.setQuery("SELECT * FROM user_bots WHERE id = " + botId);
                                speakingRow = dbClient.getRow();
                                truncatedText += ";#;";
                                truncatedText += Convert.ToString(speakingRow["automatic_chat"]);
                                truncatedText += ";#;";
                                truncatedText += Convert.ToString(speakingRow["speaking_interval"]);
                            }
                        }
                        ServerMessage message = new ServerMessage(Outgoing.SerializeSpeechList);
                        message.AppendInt32(botId);
                        message.AppendInt32(skillType);
                        message.AppendString(truncatedText);
                        this.Response = message;
                        this.SendResponse();
                        break;
                    }
                case 5:
                    {
                        
                        ServerMessage message = new ServerMessage(Outgoing.SerializeSpeechList);
                        message.AppendInt32(botId);
                        message.AppendInt32(skillType);
                        using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            dbClient.setQuery("SELECT name FROM user_bots WHERE id = " + botId);
                            message.AppendString(dbClient.getString());
                        }
                        this.Response = message;
                        this.SendResponse();
                        break;
                    }
                default:
                    return;
                        
            }

        }

        internal void GetOwnBots()
        {
            //if (Session.GetHabbo().GetInventoryComponent() == null)
            //{
            //    return;
            //}

            Session.SendMessage(Session.GetHabbo().GetInventoryComponent().SerializeBotInventory());
        }
        internal void BotEditRequest()
        {

            Room Room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            int BotId = Request.PopWiredInt32();
            int ActionType = Request.PopWiredInt32();
            string Data = ButterflyEnvironment.FilterInjectionChars(Request.PopFixedString());
            RoomUser BotUser = Room.GetRoomUserManager().GetBot((uint)BotId);
            DataTable BotSpeech;
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT text, shout, bot_id FROM bots_speech;");
                BotSpeech = dbClient.getTable();
            }
            List<RandomSpeech> RndSpeechList = new List<RandomSpeech>();
            foreach (DataRow Row2 in BotSpeech.Rows)
            {
                RndSpeechList.Add(new RandomSpeech((string)Row2["text"], ButterflyEnvironment.EnumToBool(Row2["shout"].ToString()), Convert.ToUInt32(Row2["bot_id"])));
            }

            List<BotResponse> BotResponse = new List<BotResponse>();

            int CurrentX = BotUser.X;
            int CurrentY = BotUser.Y;
            int CurrentRot = BotUser.RotBody;

            DataRow BotData;

            switch (ActionType)
            {
                case 1:

                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.runFastQuery("UPDATE user_bots SET look = '" + Session.GetHabbo().Look + "' WHERE id = '" + BotId + "'");
                    }


                    Room.GetRoomUserManager().RemoveBot(BotUser.VirtualId, false);

                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.setQuery("SELECT * FROM user_bots WHERE id = '" + BotId + "'");
                        BotData = dbClient.getRow();
                    }


                    Room.GetRoomUserManager().DeployBot(new RoomBot((uint)BotData["id"], (uint)BotData["room_id"], AIType.Generic, "freeroam", (string)BotData["name"], (string)BotData["motto"], (string)BotData["look"], CurrentX, CurrentY, 0, CurrentRot, 0, 0, 0, 0, ref RndSpeechList, ref BotResponse, (int)Session.GetHabbo().Id), null);
                    break;
                case 2:
                    try
                    {
                        string[] firstdata = Data.Split(';');
                        string[] toinendata = firstdata[0].Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        string automaticChat = firstdata[2];
                        string speakingInterval = firstdata[4];  //seconds

                        if (String.IsNullOrEmpty(speakingInterval) || Convert.ToInt32(speakingInterval) <= 0)
                            speakingInterval = "7";
                        using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            dbClient.runFastQuery("DELETE FROM bots_speech WHERE bot_id = '" + BotId + "'");
                        }
                        for (int i = 0; i <= toinendata.Length - 1; i++)
                        {
                            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {

                                dbClient.setQuery("INSERT INTO `bots_speech` (`bot_id`, `text`) VALUES ('" + BotId + "', @data)");
                                dbClient.addParameter("data", toinendata[i]);
                                dbClient.runQuery();
                                dbClient.runFastQuery("UPDATE user_bots SET automatic_chat='" + automaticChat + "',speaking_interval=" + Convert.ToInt32(speakingInterval) + " WHERE id = " + BotId);
                            }
                        }

                        CurrentX = BotUser.X;
                        CurrentY = BotUser.Y;
                        CurrentRot = BotUser.RotBody;
                        Room.GetRoomUserManager().RemoveBot(BotUser.VirtualId, false);

                        using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            dbClient.setQuery("SELECT * FROM user_bots WHERE id = '" + BotId + "'");
                            BotData = dbClient.getRow();

                        }


                        foreach (DataRow Row2 in BotSpeech.Rows)
                        {
                            RndSpeechList.Add(new RandomSpeech((string)Row2["text"], ButterflyEnvironment.EnumToBool(Row2["shout"].ToString()), Convert.ToUInt32(Row2["bot_id"])));
                        }

                        Room.GetRoomUserManager().DeployBot(new RoomBot((uint)BotData["id"], (uint)BotData["room_id"], AIType.Generic, "freeroam", (string)BotData["name"], (string)BotData["motto"], (string)BotData["look"], CurrentX, CurrentY, 0, CurrentRot, 0, 0, 0, 0, ref RndSpeechList, ref BotResponse, (int)Session.GetHabbo().Id), null);
                    }
                    catch
                    {
                        return;
                    }

                    break;
                case 3:
                    // Stand
                    break;
                case 4:
                    // Dance
                    if (BotUser.DanceId > 0)
                    {
                        BotUser.DanceId = 0;
                    }
                    else
                    {
                        Random rnd = new Random();
                        BotUser.DanceId = rnd.Next(1, 4);
                    }
                    ServerMessage message = new ServerMessage(Outgoing.Dance);
                    message.AppendInt32(BotUser.VirtualId);
                    message.AppendInt32(BotUser.DanceId);
                    Session.GetHabbo().CurrentRoom.SendMessage(message);
                    break;
                case 5:
                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.setQuery("UPDATE user_bots SET name = @data WHERE id = '" + BotId + "'");
                        dbClient.addParameter("data", Data);
                        dbClient.runQuery();
                    }

                    CurrentX = BotUser.X;
                    CurrentY = BotUser.Y;
                    CurrentRot = BotUser.RotBody;
                    Room.GetRoomUserManager().RemoveBot(BotUser.VirtualId, false);

                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.setQuery("SELECT * FROM user_bots WHERE id = '" + BotId + "'");
                        BotData = dbClient.getRow();
                    }

                    Room.GetRoomUserManager().RemoveBot(BotUser.VirtualId, false);
                    Room.GetRoomUserManager().DeployBot(new RoomBot((uint)BotData["id"], Session.GetHabbo().CurrentRoomId, AIType.Generic, "freeroam", (string)BotData["name"], (string)BotData["motto"], (string)BotData["look"], CurrentX, CurrentY, 0, CurrentRot, 0, 0, 0, 0, ref RndSpeechList, ref BotResponse, (int)Session.GetHabbo().Id), null);

                    break;

                default:

                    break;
            }

        }

        internal void SetRelationshipsStatus()
        {
            uint requester = this.Request.PopWiredUInt();
            uint newStatus = this.Request.PopWiredUInt();
            if (this.Session.GetHabbo().GetRelationshipComposer().GetRelationshipList.ContainsKey(requester))
            {
                if (this.Session.GetHabbo().GetRelationshipComposer().GetRelationshipList[requester] != newStatus)
                {
                    this.Response = this.Session.GetHabbo().GetRelationshipComposer().SerializeRelationshipUpdate(requester, newStatus);
                    this.SendResponse();
                }
            }
            else if ((!this.Session.GetHabbo().GetRelationshipComposer().GetRelationshipList.ContainsKey(requester)) && (newStatus != 0))
            {
                this.Response = this.Session.GetHabbo().GetRelationshipComposer().SerializeRelationshipUpdate(requester, newStatus);
                this.SendResponse();
            }
            else
                return;
        }

        internal void GetRelationshipsProfile()
        {
            uint userId = this.Request.PopWiredUInt();
            Habbo habbo = ButterflyEnvironment.getHabboForId(userId);
            if (habbo != null)
                this.Response = habbo.GetRelationshipComposer().SerializeRelationshipsProfile();
            else
            {
                ServerMessage message = new ServerMessage(Outgoing.ProfileRelationships);
                message.AppendInt32(userId);
                message.AppendInt32(0);
                this.Response = message;
            }
            this.SendResponse();
        }

        internal void AcceptRequest()
        {
            if (this.Session.GetHabbo().GetMessenger() != null)
            {
                int num = this.Request.PopWiredInt32();
                for (int i = 0; i < num; i++)
                {
                    uint senderID = this.Request.PopWiredUInt();
                    MessengerRequest request = this.Session.GetHabbo().GetMessenger().GetRequest(senderID);
                    if (request != null)
                    {
                        if (request.To != this.Session.GetHabbo().Id)
                        {
                            break;
                        }
                        if (!this.Session.GetHabbo().GetMessenger().FriendshipExists(request.To))
                        {
                            this.Session.GetHabbo().GetMessenger().CreateFriendship(request.From);
                            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_stream VALUES (NULL, '", this.Session.GetHabbo().Id, "', '','0','", request.From, "','0','')" }));
                                adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_stream VALUES (NULL, '", request.From, "', '','0','", this.Session.GetHabbo().Id, "','0','')" }));
                            }
                        }
                        this.Session.GetHabbo().GetMessenger().HandleRequest(senderID);
                    }
                }
            }
        }

        internal void AcceptTrade()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CanTradeInRoom)
            {
                Trade userTrade = room.GetUserTrade(this.Session.GetHabbo().Id);
                if (userTrade != null)
                {
                    userTrade.Accept(this.Session.GetHabbo().Id);
                }
            }
        }

        public void ActiveEndedQuest()
        {
            ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().ActiveEndQuest(this.Session);
        }

        internal void AddFavorite()
        {
            if (this.Session.GetHabbo() != null)
            {
                uint roomId = this.Request.PopWiredUInt();
                RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomId);
                if ((((data == null) || (this.Session.GetHabbo().FavoriteRooms.Count >= 30)) || this.Session.GetHabbo().FavoriteRooms.Contains(roomId)) || (data.Type == "public"))
                {
                    return;
                }
                else
                {
                    this.GetResponse().Init(Outgoing.FavsUpdate);
                    this.GetResponse().AppendInt32(roomId);
                    this.GetResponse().AppendBoolean(true);
                    this.SendResponse();
                    this.Session.GetHabbo().FavoriteRooms.Add(roomId);
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_favorites (user_id,room_id) VALUES (", this.Session.GetHabbo().Id, ",", roomId, ")" }));
                    }
                }
            }
        }

        public void AddNewCdToJuke()
        {
            if (((this.Session != null) && (this.Session.GetHabbo() != null)) && (this.Session.GetHabbo().CurrentRoom != null))
            {
                Room currentRoom = this.Session.GetHabbo().CurrentRoom;
                if (currentRoom.CheckRights(this.Session, true))
                {
                    RoomMusicController roomMusicController = currentRoom.GetRoomMusicController();
                    if (roomMusicController.PlaylistSize < roomMusicController.PlaylistCapacity)
                    {
                        int num = this.Request.PopWiredInt32();
                        UserItem item = this.Session.GetHabbo().GetInventoryComponent().GetItem(Convert.ToUInt32(num));
                        if ((item != null) && (item.GetBaseItem().InteractionType == InteractionType.musicdisc))
                        {
                            SongItem diskItem = new SongItem(item);
                            if (roomMusicController.AddDisk(diskItem) >= 0)
                            {
                                diskItem.SaveToDatabase((int)currentRoom.RoomId);
                                this.Session.GetHabbo().GetInventoryComponent().RemoveItem((uint)num, true);
                                this.Session.SendMessage(JukeboxDiscksComposer.Compose(roomMusicController.PlaylistCapacity, roomMusicController.Playlist.Values.ToList<SongInstance>()));
                            }
                        }
                    }
                }
            }
        }

        internal void AddSaddle()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (((room != null) && !room.IsPublic) && (room.AllowPets || room.CheckRights(this.Session, true)))
            {
                uint pId = this.Request.PopWiredUInt();
                RoomItem item = room.GetRoomItemHandler().GetItem(pId);
                if (item != null)
                {
                    uint petId = this.Request.PopWiredUInt();
                    RoomUser pet = room.GetRoomUserManager().GetPet(petId);
                    if (((pet != null) && (pet.PetData != null)) && (pet.PetData.OwnerId == this.Session.GetHabbo().Id))
                    {
                        string str;
                        IQueryAdapter adapter;
                        room.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                        if (item.GetBaseItem().Name.Contains("horse_hairdye"))
                        {
                            str = item.GetBaseItem().Name.Split(new char[] { '_' })[2];
                            int num3 = 0x30;
                            num3 += int.Parse(str);
                            pet.PetData.HairDye = num3;
                            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.runFastQuery(string.Concat(new object[] { "UPDATE user_pets SET hairdye = '", pet.PetData.HairDye, "' WHERE id = ", pet.PetData.PetId }));
                            }
                        }
                        else
                        {
                            int num4;
                            if (item.GetBaseItem().Name.Contains("horse_dye"))
                            {
                                string s = item.GetBaseItem().Name.Split(new char[] { '_' })[2];
                                num4 = int.Parse(s);
                                int num5 = (2 + (num4 * 4)) - 4;
                                switch (num4)
                                {
                                    case 13:
                                        num5 = 0x3d;
                                        break;

                                    case 14:
                                        num5 = 0x41;
                                        break;

                                    case 15:
                                        num5 = 0x45;
                                        break;

                                    case 0x10:
                                        num5 = 0x49;
                                        break;
                                }
                                pet.PetData.Race = num5.ToString();
                                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE user_pets SET race = '", pet.PetData.Race, "' WHERE id = ", pet.PetData.PetId }));
                                }
                            }
                            else if (item.GetBaseItem().Name.Contains("horse_hairstyle"))
                            {
                                str = item.GetBaseItem().Name.Split(new char[] { '_' })[2];
                                num4 = 100;
                                num4 += int.Parse(str);
                                pet.PetData.PetHair = num4;
                                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE user_pets SET pethair = '", pet.PetData.PetHair, "' WHERE id = ", pet.PetData.PetId }));
                                }
                            }
                            else
                            {
                                string str3 = item.GetBaseItem().Name.Replace("horse_saddle", "");
                                //if (string.IsNullOrEmpty(str3)) str3 = "1";
                                pet.PetData.HaveSaddle = int.Parse(str3);
                                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE user_pets SET have_saddle = '", str3, "' WHERE id = ", pet.PetData.PetId }));
                                }
                            }
                        }
                        ServerMessage message = new ServerMessage(Outgoing.SerializeMontura);
                        message.AppendInt32(pet.PetData.VirtualId);
                        message.AppendInt32(pet.PetData.PetId);
                        message.AppendInt32(pet.PetData.Type);
                        message.AppendInt32(int.Parse(pet.PetData.Race));
                        message.AppendString(pet.PetData.Color.ToLower());
                        if (pet.PetData.HaveSaddle == 2)
                        {
                            message.AppendInt32(1);
                            message.AppendInt32(3);
                            message.AppendInt32(4);
                            message.AppendInt32(10);
                            message.AppendInt32(0);
                            message.AppendInt32(2);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                            message.AppendInt32(3);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                        }
                        else if (pet.PetData.HaveSaddle == 1)
                        {
                            message.AppendInt32(1);
                            message.AppendInt32(3);
                            message.AppendInt32(2);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                            message.AppendInt32(3);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                            message.AppendInt32(4);
                            message.AppendInt32(9);
                            message.AppendInt32(0);
                        }
                        else
                        {
                            message.AppendInt32(1);
                            message.AppendInt32(2);
                            message.AppendInt32(2);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                            message.AppendInt32(3);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                        }
                        message.AppendBoolean(pet.PetData.HaveSaddle != 0);
                        message.AppendBoolean(pet.montandoBol);
                        room.SendMessage(message);
                    }
                }
            }
        }

        internal ServerMessage AddToNewGame(Butterfly.HabboHotel.Games.SnowStorm.SnowStorm War)
        {
            War.WarUsers.Add(this.Session.GetHabbo());
            this.Session.GetHabbo().SnowWar = War;
            ServerMessage message = new ServerMessage(Outgoing.AddToNewGame);
            message.AppendInt32(0);
            message.AppendString("SnowStorm level " + War.WarLevel);
            message.AppendInt32(0);
            message.AppendInt32(War.WarLevel);
            message.AppendInt32(2);
            message.AppendInt32(War.MaxUsers);
            message.AppendString(War.WarOwner.Username);
            message.AppendInt32(0x11);
            message.AppendInt32(War.WarUsers.Count);
            foreach (Habbo habbo in War.WarUsers)
            {
                message.AppendInt32(habbo.Id);
                message.AppendString(habbo.Username);
                message.AppendString(habbo.Look);
                message.AppendString(habbo.Gender.ToLower());
                message.AppendInt32(-1);
                message.AppendInt32(habbo.SnowLevel);
                message.AppendInt32(habbo.SnowPoints);
                message.AppendInt32(1);
            }
            message.AppendInt32(0);
            message.AppendInt32(120);
            return message;
        }

        internal void AnswerDoorbell()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session))
            {
                string username = this.Request.PopFixedString();
                bool flag = this.Request.PopWiredBoolean();
                GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(username);
                if (clientByUsername != null)
                {
                    if (flag)
                    {
                        clientByUsername.GetHabbo().LoadingChecksPassed = true;
                        clientByUsername.GetMessageHandler().Response.Init(Outgoing.ValidDoorBell);
                        clientByUsername.GetMessageHandler().Response.AppendString("");
                        clientByUsername.GetMessageHandler().SendResponse();
                    }
                    else
                    {
                        clientByUsername.GetMessageHandler().Response.Init(Outgoing.DoorBellNoPerson);
                        clientByUsername.GetMessageHandler().Response.AppendString("");
                        clientByUsername.GetMessageHandler().SendResponse();
                    }
                }
            }
        }

        internal void AnswerInfobusPoll()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            int num = this.Request.PopWiredInt32();
            int currentPollId = room.CurrentPollId;
            DataTable table = null;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM infobus_answers WHERE question_id = '" + currentPollId + "'");
                table = adapter.getTable();
                adapter.setQuery(string.Concat(new object[] { "INSERT INTO `infobus_results` (`question_id`, `answer_id`) VALUES ('", currentPollId, "', '", table.Rows[num - 1]["id"], "')" }));
                adapter.insertQuery();
            }
        }

        internal void ApplyEffect()
        {
            this.Session.GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(this.Request.PopWiredInt32());
        }

        internal void ApplyRoomEffect()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                UserItem item = this.Session.GetHabbo().GetInventoryComponent().GetItem(this.Request.PopWiredUInt());
                if (item != null)
                {
                    string s = "floor";
                    if (item.GetBaseItem().Name.ToLower().Contains("wallpaper"))
                    {
                        s = "wallpaper";
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("landscape"))
                    {
                        s = "landscape";
                    }
                    string str2 = s;
                    if (str2 != null)
                    {
                        if (!(str2 == "floor"))
                        {
                            if (str2 == "wallpaper")
                            {
                                room.Wallpaper = item.ExtraData;
                                room.RoomData.Wallpaper = item.ExtraData;
                            }
                            else if (str2 == "landscape")
                            {
                                room.Landscape = item.ExtraData;
                                room.RoomData.Landscape = item.ExtraData;
                            }
                        }
                        else
                        {
                            room.Floor = item.ExtraData;
                            room.RoomData.Floor = item.ExtraData;
                        }
                    }
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery(string.Concat(new object[] { "UPDATE rooms SET ", s, " = @extradata WHERE id = ", room.RoomId }));
                        adapter.addParameter("extradata", item.ExtraData);
                        adapter.runQuery();
                    }
                    this.Session.GetHabbo().GetInventoryComponent().RemoveItem(item.Id, false);
                    ServerMessage message = new ServerMessage(Outgoing.RoomDecoration);
                    message.AppendString(s);
                    message.AppendString(item.ExtraData);
                    room.SendMessage(message);
                }
            }
        }

        internal void BanUser()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                uint pId = this.Request.PopWiredUInt();
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(pId);
                if (((roomUserByHabbo != null) && !roomUserByHabbo.IsBot)&& (!roomUserByHabbo.GetClient().GetHabbo().HasFuse("fuse_mod") && !roomUserByHabbo.GetClient().GetHabbo().HasFuse("fuse_no_kick")))
                {
                    room.AddBan(pId);
                    room.GetRoomUserManager().RemoveUserFromRoom(roomUserByHabbo.GetClient(), true, true);
                    this.Session.CurrentRoomUserID = -1;
                }
            }
        }

        public void BuyGroup()
        {
            if ((this.Session != null) && (this.Session.GetHabbo().Credits >= 10))
            {
                List<int> gStates = new List<int>();
                string name = this.Request.PopFixedString();
                string description = this.Request.PopFixedString();
                int roomid = this.Request.PopWiredInt32();
                int color = this.Request.PopWiredInt32();
                int num3 = this.Request.PopWiredInt32();
                this.Request.PopWiredInt32();
                int guildBase = this.Request.PopWiredInt32();
                int guildBaseColor = this.Request.PopWiredInt32();
                int num6 = this.Request.PopWiredInt32();

                for (int i = 0; i < (num6 * 3); i++)
                {
                    int item = this.Request.PopWiredInt32();
                    gStates.Add(item);
                }

                string image = ButterflyEnvironment.GetGame().GetGuilds().GenerateGuildImage(guildBase, guildBaseColor, gStates);
                string htmlColor = ButterflyEnvironment.GetGame().GetGuilds().GetHtmlColor(color);
                string str5 = ButterflyEnvironment.GetGame().GetGuilds().GetHtmlColor(num3);
                string datecreated = DateTime.Now.ToShortDateString();
                
                int id = (int)this.Session.GetHabbo().Id;
                
                string username = this.Session.GetHabbo().Username;
                
                Dictionary<int,string> members = new Dictionary<int,string>();
                members.Add(id,DateTime.Now.Day + " - " + DateTime.Now.Month + " -  " + DateTime.Now.Year);
                

                
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().LoadRoom(Convert.ToUInt32(roomid));
                
                if (room != null)
                {
                    MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().AddGuild(0, name, id, username, description, roomid, image, color, num3, guildBase, guildBaseColor, gStates, htmlColor, str5, datecreated, members, new List<int>(), 0, 0);
                    
                    room.GroupId = guild.Id;
                    room.GenerateGuild(guild);
                    room.RoomData.GroupId = guild.Id;
                    
                    this.Session.GetHabbo().ImGuilds.Add(guild.Id);
                    this.Session.GetHabbo().FavoriteGuild = guild.Id;
                    
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET FavoriteGroup = '", guild.Id, "' WHERE Id = '", guild.OwnerId, "'" }));
                        adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_groups VALUES (NULL, '", this.Session.GetHabbo().Username, "','", this.Session.GetHabbo().Id, "','", guild.Id, "','", DateTime.Now.Day, " - ", DateTime.Now.Month, " -  ", DateTime.Now.Year, "', '0')" }));
                        adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_stream VALUES (NULL, '", this.Session.GetHabbo().Id, "','", ButterflyEnvironment.LinkConfig, guild.Image, ".gif','10','", guild.Id, "','0','')" }));
                    }
                    
                    ServerMessage message = new ServerMessage(Outgoing.SerializePurchaseInformation);
                    
                    message.AppendInt32(0x1815);
                    message.AppendString("CREATE_GUILD");
                    message.AppendInt32(10);
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendBoolean(true);
                    message.AppendInt32(0);
                    message.AppendInt32(2);
                    message.AppendBoolean(false);
                    
                    Session.SendMessage(message);
                    
                    Habbo habbo = this.Session.GetHabbo();
                    habbo.Credits -= 10;
                    
                    Session.GetHabbo().UpdateCreditsBalance();

                    ServerMessage message2 = new ServerMessage(Outgoing.SendHtmlColors);
                    message2.AppendInt32(this.Session.GetHabbo().ImGuilds.Count);
                    
                    foreach (int num10 in this.Session.GetHabbo().ImGuilds)
                    {
                        MyGuild guild2 = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(num10);
                        message2.AppendInt32(guild2.Id);
                        message2.AppendString(guild2.Name);
                        message2.AppendString(guild2.Image);
                        message2.AppendString(guild2.HtmlColor1);
                        message2.AppendString(guild2.HtmlColor2);
                        message2.AppendBoolean(guild2.Id == Session.GetHabbo().FavoriteGuild);
                    }
                    
                    Session.SendMessage(message2);

                    if ((ButterflyEnvironment.GetGame().GetClientManager().GetClient(habbo.Id) != null) && (room != null))
                    {
                        ServerMessage message3 = new ServerMessage(Outgoing.SetRoomUser);
                        message3.AppendInt32(1);
                        room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id).Serialize(message3, false);

                        room.SendMessage(message3);
                    }

                    ServerMessage message4 = new ServerMessage(Outgoing.UpdateRoom);
                    message4.AppendInt32(guild.RoomId);
                    this.Session.SendMessage(message4);

                    ServerMessage message5 = new ServerMessage(Outgoing.ConfigureWallandFloor);
                    message5.AppendBoolean(room.Hidewall);
                    message5.AppendInt32(room.WallThickness);
                    message5.AppendInt32(room.FloorThickness);
                    this.Session.SendMessage(message5);

                    ServerMessage message6 = new ServerMessage(Outgoing.SendRoomAndGroup);
                    message6.AppendInt32(guild.RoomId);
                    message6.AppendInt32(guild.Id);
                    this.Session.SendMessage(message6);

                    ServerMessage message7 = new ServerMessage(Outgoing.RoomData);
                    message7.AppendBoolean(true);
                    message7.AppendInt32(guild.RoomId);
                    message7.AppendString(room.Name);
                    message7.AppendBoolean(true);
                    message7.AppendInt32(room.OwnerId);
                    message7.AppendString(room.Owner);
                    message7.AppendInt32(room.State);
                    message7.AppendInt32(room.UsersNow);
                    message7.AppendInt32(room.UsersMax);
                    message7.AppendString(room.Description);
                    message7.AppendInt32(0);
                    message7.AppendInt32((room.Category == 0x34) ? 2 : 0);
                    message7.AppendInt32(room.Score);
                    message7.AppendInt32(0);
                    message7.AppendInt32(room.Category);
                    
                    if (room.GetRoomGuild() == null)
                    {
                        message7.AppendInt32(0);
                        message7.AppendInt32(0);
                    }
                    else
                    {
                        message7.AppendInt32(guild.Id);
                        message7.AppendString(guild.Name);
                        message7.AppendString(guild.Image);
                    }
                    
                    message7.AppendString("");
                    message7.AppendInt32(room.TagCount);
                    
                    foreach (string str8 in room.Tags.ToArray())
                    {
                        message7.AppendString(str8);
                    }
                    
                    message7.AppendInt32(0);
                    message7.AppendInt32(0);
                    message7.AppendInt32(0);
                    message7.AppendBoolean(true);
                    message7.AppendBoolean(true);
                    message7.AppendInt32(0);
                    message7.AppendInt32(0);
                    message7.AppendBoolean(false);
                    message7.AppendBoolean(false);
                    message7.AppendBoolean(false);
                    message7.AppendInt32(0);
                    message7.AppendInt32(0);
                    message7.AppendInt32(0);
                    message7.AppendBoolean(false);
                    message7.AppendBoolean(true);
                    room.SendMessage(message7);
                }
            }
        }

        internal void CancelMountOnPet()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    uint petId = this.Request.PopWiredUInt();
                    RoomUser pet = room.GetRoomUserManager().GetPet(petId);
                    if ((pet != null) && (pet.PetData != null))
                    {
                        roomUserByHabbo.montandoBol = false;
                        roomUserByHabbo.montandoID = 0;
                        pet.montandoBol = false;
                        pet.montandoID = 0;
                        roomUserByHabbo.MoveTo(roomUserByHabbo.X + 1, roomUserByHabbo.Y + 1);
                        roomUserByHabbo.ApplyEffect(-1);
                    }
                }
            }
        }

        internal void CanCreateRoom()
        {
            this.Response.Init(Outgoing.CanCreateRoom);
            this.Response.AppendInt32(0);
            this.Response.AppendInt32(0x1869f);
            this.SendResponse();
        }

        internal void CanGift()
        {
            uint itemId = this.Request.PopWiredUInt();
            if (ButterflyEnvironment.GetGame().GetCatalog().FindItem(itemId) == null)
            {
            }
        }

        internal void ChangeLook()
        {
            if (this.Session.GetHabbo().MutantPenalty)
            {
                this.Session.SendNotif("Because of a penalty or restriction on your account, you are not allowed to change your look.");
            }
            else
            {
                string gender = this.Request.PopFixedString().ToUpper();
                string look = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
                if (AntiMutant.ValidateLook(look, gender))
                {
                    ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("CHANGE_FIGURE", this.Session);
                    this.Session.GetHabbo().Look = ButterflyEnvironment.FilterFigure(look);
                    this.Session.GetHabbo().Gender = gender.ToLower();
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery("UPDATE users SET look = @look, gender = @gender WHERE id = " + this.Session.GetHabbo().Id);
                        adapter.addParameter("look", look);
                        adapter.addParameter("gender", gender);
                        adapter.runQuery();
                    }
                    ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(this.Session, "ACH_AvatarLooks", 1);
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
        }

        internal void ChangeMotto()
        {
            string query = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
            if (query != this.Session.GetHabbo().Motto)
            {
                this.Session.GetHabbo().Motto = query;
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("UPDATE users SET motto = @motto WHERE id = '" + this.Session.GetHabbo().Id + "'");
                    adapter.addParameter("motto", query);
                    adapter.runQuery();
                }
                ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("CHANGE_MOTTO", this.Session);
                if (this.Session.GetHabbo().InRoom)
                {
                    Room currentRoom = this.Session.GetHabbo().CurrentRoom;
                    if (currentRoom == null)
                    {
                        return;
                    }
                    RoomUser roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                    if (roomUserByHabbo == null)
                    {
                        return;
                    }
                    ServerMessage message = new ServerMessage(Outgoing.UpdateUserInformation);
                    message.AppendInt32(roomUserByHabbo.VirtualId);
                    message.AppendString(this.Session.GetHabbo().Look);
                    message.AppendString(this.Session.GetHabbo().Gender.ToLower());
                    message.AppendString(this.Session.GetHabbo().Motto);
                    message.AppendInt32(this.Session.GetHabbo().AchievementPoints);
                    currentRoom.SendMessage(message);
                }
                ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(this.Session, "ACH_Motto", 1);
            }
        }

        internal Butterfly.HabboHotel.Games.SnowStorm.SnowStorm CheckAGame()
        {
            foreach (Butterfly.HabboHotel.Games.SnowStorm.SnowStorm storm in ButterflyEnvironment.GetGame().GetStormWars().Wars.Values)
            {
                if ((storm.WarUsers.Count < 10) && (storm.WarStarted < 1))
                {
                    return storm;
                }
            }
            return null;
        }

        internal void CheckArenaStatic(ServerMessage Message, Butterfly.HabboHotel.Games.SnowStorm.SnowStorm War)
        {
            if (War.WarLevel == 9)
            {
                Message.AppendInt32((int)(20 + War.WarUsers.Count));
                Message.AppendInt32(3);
                Message.AppendInt32(0);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0xaf00);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0);
                Message.AppendInt32(3);
                Message.AppendInt32(1);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x1770a);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(1);
                Message.AppendInt32(3);
                Message.AppendInt32(2);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(3);
                Message.AppendInt32(3);
                Message.AppendInt32(3);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0x11f80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(5);
                Message.AppendInt32(3);
                Message.AppendInt32(4);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x15180);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(6);
                Message.AppendInt32(3);
                Message.AppendInt32(5);
                Message.AppendInt32(0xc800);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(7);
                Message.AppendInt32(3);
                Message.AppendInt32(6);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0x16a80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(10);
                Message.AppendInt32(3);
                Message.AppendInt32(7);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0xe100);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(14);
                Message.AppendInt32(3);
                Message.AppendInt32(8);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0x12c0a);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(15);
                Message.AppendInt32(3);
                Message.AppendInt32(9);
                Message.AppendInt32(0x15e0a);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x10);
                Message.AppendInt32(3);
                Message.AppendInt32(10);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0xd480);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x11);
                Message.AppendInt32(3);
                Message.AppendInt32(11);
                Message.AppendInt32(0x1c20a);
                Message.AppendInt32(0xbb80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x12);
                Message.AppendInt32(3);
                Message.AppendInt32(12);
                Message.AppendInt32(0x12c0a);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(20);
                Message.AppendInt32(3);
                Message.AppendInt32(13);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0x11f80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x15);
                Message.AppendInt32(3);
                Message.AppendInt32(14);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0xbb80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x18);
                Message.AppendInt32(3);
                Message.AppendInt32(15);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0x1450a);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x19);
                Message.AppendInt32(3);
                Message.AppendInt32(0x10);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0x15180);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x1b);
                Message.AppendInt32(3);
                Message.AppendInt32(0x11);
                Message.AppendInt32(0x1130a);
                Message.AppendInt32(0xed80);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x1d);
                Message.AppendInt32(3);
                Message.AppendInt32(0x12);
                Message.AppendInt32(0x6400);
                Message.AppendInt32(0xfa00);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(30);
                Message.AppendInt32(3);
                Message.AppendInt32(0x13);
                Message.AppendInt32(0xfa00);
                Message.AppendInt32(0x11080);
                Message.AppendInt32(12);
                Message.AppendInt32(12);
                Message.AppendInt32(0x23);
            }
        }

        public void CheckNameToChange()
        {
            string userName = this.Request.PopFixedString();
            Habbo habbo = ButterflyEnvironment.getHabboForName(userName);
            ServerMessage message = new ServerMessage(Outgoing.CheckName);
            if (userName.Length < 4)
            {
                message.AppendInt32(2);
                message.AppendString(userName);
                message.AppendInt32(3);
                message.AppendString("Jefa" + userName);
                message.AppendString("Con" + userName);
                message.AppendString(userName + "Risa");
            }
            else if (habbo != null)
            {
                message.AppendInt32(5);
                message.AppendString(userName);
                message.AppendInt32(3);
                message.AppendString("Jefa" + userName);
                message.AppendString("Con" + userName);
                message.AppendString(userName + "Risa");
            }
            else
            {
                message.AppendInt32(0);
                message.AppendString(userName);
                message.AppendInt32(0);
            }
            this.Session.SendMessage(message);
        }

        internal void CheckPetName()
        {
            string petName = this.Request.PopFixedString();
            this.Session.GetMessageHandler().GetResponse().Init(Outgoing.CheckPetName);
            this.Session.GetMessageHandler().GetResponse().AppendInt32(Catalog.CheckPetName(petName) ? 0 : 2);
            this.Session.GetMessageHandler().GetResponse().AppendString(petName);
            this.Session.GetMessageHandler().SendResponse();
        }

        internal void CheckRelease()
        {
            //Console.WriteLine("Client connected.. " + this.Request.PopFixedString());
            this.Session.bannertimmer = DateTime.Now;            
        }

        internal void ClearRoomLoading()
        {
            this.Session.GetHabbo().LoadingRoom = 0;
            this.Session.GetHabbo().LoadingChecksPassed = false;
        }

        internal void CommandsPet()
        {
            uint petId = this.Request.PopWiredUInt();
            RoomUser pet = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId).GetRoomUserManager().GetPet(petId);
            if ((pet != null) && (pet.PetData != null))
            {
                this.GetResponse().Init(0x25d);
                this.GetResponse().AppendInt32(petId);
                int level = pet.PetData.Level;
                this.GetResponse().AppendInt32(0x12);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(1);
                this.GetResponse().AppendInt32(2);
                this.GetResponse().AppendInt32(3);
                this.GetResponse().AppendInt32(4);
                this.GetResponse().AppendInt32(0x11);
                this.GetResponse().AppendInt32(5);
                this.GetResponse().AppendInt32(6);
                this.GetResponse().AppendInt32(7);
                this.GetResponse().AppendInt32(8);
                this.GetResponse().AppendInt32(9);
                this.GetResponse().AppendInt32(10);
                this.GetResponse().AppendInt32(11);
                this.GetResponse().AppendInt32(12);
                this.GetResponse().AppendInt32(13);
                this.GetResponse().AppendInt32(14);
                this.GetResponse().AppendInt32(15);
                this.GetResponse().AppendInt32(0x10);
                int i = 0;
                while (level > i)
                {
                    i++;
                    this.GetResponse().AppendInt32(i);
                }
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(1);
                this.GetResponse().AppendInt32(2);
                this.SendResponse();
            }
        }

        internal void CompleteTrade()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CanTradeInRoom)
            {
                Trade userTrade = room.GetUserTrade(this.Session.GetHabbo().Id);
                if (userTrade != null)
                {
                    userTrade.CompleteTrade(this.Session.GetHabbo().Id);
                }
            }
        }

        internal void CreateRoom()
        {
            string name = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
            string model = this.Request.PopFixedString();
            RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().CreateRoom(this.Session, name, model);
            if (data != null)
            {
                this.Response.Init(Outgoing.OnCreateRoomInfo);
                this.Response.AppendInt32(data.Id);
                this.Response.AppendString(data.Name);
                this.SendResponse();
            }
        }

        internal void CreateStream()
        {
            string str = this.Request.PopFixedString();
        }

        internal void Dance()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.Unidle();
                    int i = this.Request.PopWiredInt32();
                    if (((i < 0) || (i > 4)) || (!this.Session.GetHabbo().GetSubscriptionManager().HasSubscription("club_habbo") && (i > 1)))
                    {
                        i = 0;
                    }
                    if ((i > 0) && (roomUserByHabbo.CarryItemID > 0))
                    {
                        roomUserByHabbo.CarryItem(0);
                    }
                    roomUserByHabbo.DanceId = i;
                    ServerMessage message = new ServerMessage(Outgoing.Dance);
                    message.AppendInt32(roomUserByHabbo.VirtualId);
                    message.AppendInt32(i);
                    room.SendMessage(message);
                    ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("DANCE", this.Session);
                }
            }
        }

        internal void Sit()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            int tries = 0;
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.Unidle();

                    if (!roomUserByHabbo.Statusses.ContainsKey("sit") && !roomUserByHabbo.Statusses.ContainsKey("lay"))
                    {
                        if ((roomUserByHabbo.RotBody % 2) == 0)
                        {
                            if (roomUserByHabbo == null)
                            {
                                return;
                            }

                            try
                            {
                                roomUserByHabbo.Statusses.Add("sit", "1.0");
                                roomUserByHabbo.Z -= 0.35;
                                roomUserByHabbo.acostadoBol = true;
                                roomUserByHabbo.UpdateNeeded = true;
                            }
                            catch { }

                            return;

                        }

                        else
                        {
                            if (tries <= 1)
                            {
                                roomUserByHabbo.RotBody--;
                                tries++;
                                Sit();
                            }
                            else
                            {
                                Session.SendNotif("You cannot sit diagonally, try it again please");
                                tries = 0;
                            }

                        }

                    }

                    else if (roomUserByHabbo.acostadoBol == true)
                    {

                        roomUserByHabbo.Z += 0.35;
                        roomUserByHabbo.Statusses.Remove("sit");
                        roomUserByHabbo.Statusses.Remove("1.0");
                        roomUserByHabbo.acostadoBol = false;
                        roomUserByHabbo.UpdateNeeded = true;

                    }
                }
            }
        }

        internal void DeclineRequest()
        {
            if (this.Session.GetHabbo().GetMessenger() != null)
            {
                int num = this.Request.PopWiredInt32();
                int num2 = this.Request.PopWiredInt32();
                if ((num == 0) && (num2 == 1))
                {
                    uint sender = this.Request.PopWiredUInt();
                    this.Session.GetHabbo().GetMessenger().HandleRequest(sender);
                }
                else
                {
                    this.Session.GetHabbo().GetMessenger().HandleAllRequests();
                }
            }
        }

        internal void DeletePendingCFH()
        {
            if (ButterflyEnvironment.GetGame().GetModerationTool().UsersHasPendingTicket(this.Session.GetHabbo().Id))
            {
                ButterflyEnvironment.GetGame().GetModerationTool().DeletePendingTicketForUser(this.Session.GetHabbo().Id);
                this.GetResponse().Init(320);
                this.SendResponse();
            }
        }

        internal void DeletePostit()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                RoomItem item = room.GetRoomItemHandler().GetItem(this.Request.PopWiredUInt());
                if ((item != null) && (item.GetBaseItem().InteractionType == InteractionType.postit))
                {
                    room.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                }
            }
        }

        internal void DeleteRoom()
        {
            Func<RoomData, bool> predicate = null;
            uint RoomId = this.Request.PopWiredUInt();
            if (((this.Session != null) && (this.Session.GetHabbo() != null)) && (this.Session.GetHabbo().UsersRooms != null))
            {
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
                if ((room != null) && ((room.Owner == this.Session.GetHabbo().Username) || (this.Session.GetHabbo().Rank > 6)))
                {
                    if (this.Session.GetHabbo().GetInventoryComponent() != null)
                    {
                        List<RoomItem> roomItemsList = room.GetRoomItemHandler().RemoveAllFurniture(this.Session);
                        room.GetRoomItemHandler().RemoveItemsByOwner(ref roomItemsList, ref this.Session);
                    }
                    RoomData roomData = room.RoomData;
                    ButterflyEnvironment.GetGame().GetRoomManager().UnloadRoom(room);
                    ButterflyEnvironment.GetGame().GetRoomManager().QueueVoteRemove(roomData);
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery("DELETE FROM rooms WHERE id = " + RoomId);
                        adapter.runFastQuery("DELETE FROM user_favorites WHERE room_id = " + RoomId);
                        adapter.runFastQuery("DELETE items, items_extradata, items_rooms FROM items_rooms INNER JOIN items ON (items.item_id = items_rooms.item_id) LEFT JOIN items_extradata ON (items_extradata.item_id = items.item_id) WHERE items_rooms.room_id = " + RoomId);
                        adapter.runFastQuery("DELETE FROM room_rights WHERE room_id = " + RoomId);
                        adapter.runFastQuery("UPDATE users SET home_room = '0' WHERE home_room = " + RoomId);
                    }
                    if ((this.Session.GetHabbo().Rank > 5) && (this.Session.GetHabbo().Username != roomData.Owner))
                    {
                        ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(this.Session.GetHabbo().Username, roomData.Name, "Room deletion", string.Format("Deleted room ID {0}", roomData.Id));
                    }
                    if (predicate == null)
                    {
                        predicate = p => p.Id == RoomId;
                    }
                    RoomData item = this.Session.GetHabbo().UsersRooms.Where<RoomData>(predicate).SingleOrDefault<RoomData>();
                    if (item != null)
                    {
                        this.Session.GetHabbo().UsersRooms.Remove(item);
                    }
                }
            }
        }

        internal void Destroy()
        {
            this.Session = null;
        }

        public void EditAjustes()
        {
            int guildId = this.Request.PopWiredInt32();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Convert.ToUInt32(guild.RoomId));
                if (room != null)
                {
                    guild.Type = this.Request.PopWiredInt32();
                    guild.Petitions.Clear();
                    guild.RightsType = this.Request.PopWiredInt32();
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE groups SET Typee = '", guild.Type, "', RightsType = '", guild.RightsType, "', Petitions = '' WHERE Id = '", guild.Id, "'" }));
                    }
                    this.SaveSettingsPackets(guild, room);
                }
            }
        }

        public void EditColores()
        {
            int guildId = this.Request.PopWiredInt32();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Convert.ToUInt32(guild.RoomId));
                if (room != null)
                {
                    guild.CustomColor1 = this.Request.PopWiredInt32();
                    guild.CustomColor2 = this.Request.PopWiredInt32();
                    guild.HtmlColor1 = ButterflyEnvironment.GetGame().GetGuilds().GetHtmlColor(guild.CustomColor1);
                    guild.HtmlColor2 = ButterflyEnvironment.GetGame().GetGuilds().GetHtmlColor(guild.CustomColor2);
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE groups SET CustomColor1 = '", guild.CustomColor1, "', CustomColor2 = '", guild.CustomColor2, "', HtmlColor1 = '", guild.HtmlColor1, "', HtmlColor2 = '", guild.HtmlColor2, "' WHERE Id = '", guild.Id, "'" }));
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE items SET group_data = '", guildId, ";", guild.HtmlColor1, ";", guild.HtmlColor2, "' WHERE group_data LIKE '", guildId, ";%'" }));
                    }
                    this.SaveSettingsPackets(guild, room);

                    //Session.SendNotif(guild.CustomColor1.ToString() + " " + guild.HtmlColor1.ToString() + guild.CustomColor2.ToString() + " " + guild.HtmlColor2.ToString());
                    

                }
            }
        }

        public void EditGuild()
        {
            if (!this.Session.GetHabbo().ColorsSended)
            {
                this.OpenGuildPage();
                this.Session.GetHabbo().ColorsSended = true;
            }
            int guildId = this.Request.PopWiredInt32();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            ServerMessage message = new ServerMessage(Outgoing.SendGestionGroup);
            message.AppendInt32((int)(this.Session.GetHabbo().UsersRooms.Count - this.GetMyRoomsGuilds()));
            foreach (RoomData data in this.Session.GetHabbo().UsersRooms)
            {
                if (data.GroupId == 0)
                {
                    message.AppendInt32(data.Id);
                    message.AppendString(data.Name);
                    message.AppendBoolean(false);
                }
            }
            message.AppendBoolean(true);
            message.AppendInt32(guild.Id);
            message.AppendString(guild.Name);
            message.AppendString(guild.Description);
            message.AppendInt32(guild.RoomId);
            message.AppendInt32(guild.CustomColor1);
            message.AppendInt32(guild.CustomColor2);
            message.AppendInt32(guild.Type);
            message.AppendInt32(guild.RightsType);
            message.AppendBoolean(false);
            message.AppendString("");
            message.AppendInt32(5);
            message.AppendInt32(guild.GuildBase);
            message.AppendInt32(guild.GuildBaseColor);
            message.AppendInt32(4);
            message.AppendInt32(guild.GuildStates[0]);
            message.AppendInt32(guild.GuildStates[1]);
            message.AppendInt32(guild.GuildStates[2]);
            message.AppendInt32(guild.GuildStates[3]);
            message.AppendInt32(guild.GuildStates[4]);
            message.AppendInt32(guild.GuildStates[5]);
            message.AppendInt32(guild.GuildStates[6]);
            message.AppendInt32(guild.GuildStates[7]);
            message.AppendInt32(guild.GuildStates[8]);
            message.AppendInt32(guild.GuildStates[9]);
            message.AppendInt32(guild.GuildStates[10]);
            message.AppendInt32(guild.GuildStates[11]);
            message.AppendString(guild.Image);
            message.AppendInt32(guild.Members.Count);
            message.AppendInt32(guild.Petitions.Count);
            this.Session.SendMessage(message);
        }

        public void EditIdentidad()
        {
            int guildId = this.Request.PopWiredInt32();
            string str = this.Request.PopFixedString();
            string str2 = this.Request.PopFixedString();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom((uint)guild.RoomId);
                if (room != null)
                {
                    guild.Name = str;
                    guild.Description = str2;
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery(string.Concat(new object[] { "UPDATE groups SET Name = @groupname, Description = @groupdesc WHERE Id = '", guild.Id, "'" }));
                        adapter.addParameter("groupname", guild.Name);
                        adapter.addParameter("groupdesc", guild.Description);
                        adapter.runQuery();
                    }
                    this.SaveSettingsPackets(guild, room);
                }
            }
        }

        public void EditPlaca()
        {
            int guildId = this.Request.PopWiredInt32();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom((uint)guild.RoomId);
                if (room != null)
                {
                    this.Request.PopWiredInt32();
                    guild.GuildBase = this.Request.PopWiredInt32();
                    guild.GuildBaseColor = this.Request.PopWiredInt32();
                    this.Request.PopWiredInt32();
                    guild.GuildStates.Clear();
                    string str = "";
                    for (int i = 0; i < 12; i++)
                    {
                        int item = this.Request.PopWiredInt32();
                        guild.GuildStates.Add(item);
                        str = str + item + ";";
                    }
                    str = str.Substring(0, str.Length - 1);
                    guild.Image = ButterflyEnvironment.GetGame().GetGuilds().GenerateGuildImage(guild.GuildBase, guild.GuildBaseColor, guild.GuildStates);
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE groups SET Image = '", guild.Image, "', GuildBase = '", guild.GuildBase, "', GuildBaseColor = '", guild.GuildBaseColor, "', GuildStates = '", str, "' WHERE Id = '", guild.Id, "'" }));
                    }
                    this.SaveSettingsPackets(guild, room);
                    ServerMessage message;
                    message = new ServerMessage(Outgoing.SendAdvGroupInit);
                    message.AppendInt32(guild.Id);
                    message.AppendBoolean(true);
                    message.AppendInt32(guild.Type);
                    message.AppendString(guild.Name);
                    message.AppendString(guild.Description);
                    message.AppendString(guild.Image);
                    message.AppendInt32(room.RoomId);
                    message.AppendString(room.Name);
                    if (guild.Petitions.Contains((int)this.Session.GetHabbo().Id))
                    {
                        message.AppendInt32(2);
                    }
                    else if (!this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                    {
                        message.AppendInt32(0);
                    }
                    else if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                    {
                        message.AppendInt32(1);
                    }
                    message.AppendInt32(guild.Members.Count);
                    message.AppendBoolean(false);
                    message.AppendString(guild.DateCreated);

                    message.AppendBoolean(this.Session.GetHabbo().Id == guild.OwnerId);

                    if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                    {
                        if (guild.GuildRanks[(int)this.Session.GetHabbo().Id] < 2)
                            message.AppendBoolean(true);
                        else message.AppendBoolean(false);
                    }
                    else message.AppendBoolean(false);
                    message.AppendString(guild.OwnerName);
                    message.AppendBoolean(false);
                    message.AppendBoolean(false);
                    message.AppendInt32(guild.Petitions.Count);
                    this.Session.SendMessage(message);

                }
            }
        }

        internal void EnableEffect()
        {
            this.Session.GetHabbo().GetAvatarEffectsInventoryComponent().EnableEffect(this.Request.PopWiredInt32());
        }

        public void EndConfirmBuy()
        {
            int guildId = this.Request.PopWiredInt32();
            bool flag = this.Request.PopWiredBoolean();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData((uint)guild.RoomId);
                if (data != null)
                {
                    ServerMessage message;
                    if (!flag)
                    {
                        try
                        {
                            message = new ServerMessage(Outgoing.SendAdvGroupInit);
                            message.AppendInt32(guild.Id);
                            message.AppendBoolean(true);
                            message.AppendInt32(guild.Type);
                            message.AppendString(guild.Name);
                            message.AppendString(guild.Description);
                            message.AppendString(guild.Image);
                            message.AppendInt32(data.Id);
                            message.AppendString(data.Name);
                            if (guild.Petitions.Contains((int)this.Session.GetHabbo().Id))
                            {
                                message.AppendInt32(2);
                            }
                            else if (!this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                            {
                                message.AppendInt32(0);
                            }
                            else if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                            {
                                message.AppendInt32(1);
                            }
                            message.AppendInt32(guild.Members.Count);
                            message.AppendBoolean(false);
                            message.AppendString(guild.DateCreated);

                            message.AppendBoolean(this.Session.GetHabbo().Id == guild.OwnerId);

                            if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                            {
                                if (guild.GuildRanks[(int)this.Session.GetHabbo().Id] < 2)
                                    message.AppendBoolean(true);
                                else message.AppendBoolean(false);
                            }
                            else message.AppendBoolean(false);
                            message.AppendString(guild.OwnerName);
                            message.AppendBoolean(false);
                            message.AppendBoolean(false);
                            message.AppendInt32(guild.Petitions.Count);
                            this.Session.SendMessage(message);
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            message = new ServerMessage(Outgoing.SendAdvGroupInit);
                            message.AppendInt32(guild.Id);
                            message.AppendBoolean(false);
                            message.AppendInt32(guild.Type);
                            message.AppendString(guild.Name);
                            message.AppendString(guild.Description);
                            message.AppendString(guild.Image);
                            message.AppendInt32(data.Id);
                            message.AppendString(data.Name);
                            if (guild.Petitions.Contains((int)this.Session.GetHabbo().Id))
                            {
                                Console.WriteLine((guild.Petitions.Contains((int)this.Session.GetHabbo().Id)).ToString());
                                message.AppendInt32(2);
                            }
                            else if (!this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                            {
                                message.AppendInt32(0);
                            }
                            else if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                            {
                                message.AppendInt32(1);
                            }
                            message.AppendInt32(guild.Members.Count);
                            message.AppendBoolean(false);
                            message.AppendString(guild.DateCreated);

                            message.AppendBoolean(this.Session.GetHabbo().Id == guild.OwnerId);

                            if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                            {
                                if (guild.GuildRanks[(int)this.Session.GetHabbo().Id] < 2)
                                    message.AppendBoolean(true);
                                else message.AppendBoolean(false);
                            }
                            else message.AppendBoolean(false);
                            message.AppendString(guild.OwnerName);
                            message.AppendBoolean(true);
                            message.AppendBoolean(true);
                            message.AppendInt32(guild.Members.ContainsKey((int)this.Session.GetHabbo().Id) ? guild.Petitions.Count : 0);
                            this.Session.SendMessage(message);
                        }
                        catch(Exception ex)
                        {
                            Butterfly.Core.Logging.LogThreadException(ex.ToString(), "GUILD PROCESS THREAD ERROR: ");
                        }
                    }
                }
                else
                    Console.WriteLine("Roomdata for room: " + guild.RoomId + " was null");

            }
            else
                Console.WriteLine("Null group");

        }

        public void EndQuiz()
        {
            this.Session.GetHabbo().PassedQuiz = true;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE users SET passed_quiz = '1' WHERE id = '" + this.Session.GetHabbo().Id + "'");
            }
            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(this.Session, "ACH_SafetyQuizGraduate", 1);
            this.GetResponse().Init(Outgoing.EndTalentPractise);
            this.GetResponse().AppendString("citizenship");
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendString("A1 KUMIANKKA");
            this.GetResponse().AppendInt32(0);
            this.SendResponse();
            this.GetResponse().Init(Outgoing.Allowances);
            this.GetResponse().AppendInt32(7);
            this.GetResponse().AppendString("VOTE_IN_COMPETITIONS");
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString("TRADE");
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString("CITIZEN");
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString("SAFE_CHAT");
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString("FULL_CHAT");
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString("CALL_ON_HELPERS");
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString("USE_GUIDE_TOOL");
            this.GetResponse().AppendBoolean(false);
            this.GetResponse().AppendString("requirement.unfulfilled.helper_level_4");
            this.GetResponse().AppendString("JUDGE_CHAT_REVIEWS");
            this.GetResponse().AppendBoolean(false);
            this.GetResponse().AppendString("requirement.unfulfilled.helper_level_6");
            this.SendResponse();
        }

        public void EndQuizQuestions()
        {
            string str = this.Request.PopFixedString();
            int num = this.Request.PopWiredInt32();
            int num2 = this.Request.PopWiredInt32();
            int num3 = this.Request.PopWiredInt32();
            int num4 = this.Request.PopWiredInt32();
            int num5 = this.Request.PopWiredInt32();
            int num6 = this.Request.PopWiredInt32();
            List<int> list = new List<int>();
            if (num2 != 3)
            {
                list.Add(5);
            }
            if (num3 != 3)
            {
                list.Add(7);
            }
            if (num4 != 2)
            {
                list.Add(0);
            }
            if (num5 != 1)
            {
                list.Add(1);
            }
            if (num6 != 1)
            {
                list.Add(6);
            }
            ServerMessage message = new ServerMessage(Outgoing.CheckQuiz);
            message.AppendString("HabboWay1");
            message.AppendInt32(list.Count);
            foreach (int num7 in list)
            {
                message.AppendInt32(num7);
            }
            this.Session.SendMessage(message);
            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(this.Session, "ACH_HabboWayGraduate", 1);
        }

        internal void EnterInfobus()
        {
            this.Response.Init(0x51);
            this.Response.AppendString(LanguageLocale.GetValue("user.enterinfobus"));
            this.SendResponse();
        }

        internal void EnterInquiredRoom()
        {
        }

        internal void enterOnRoom()
        {
            uint id = this.Request.PopWiredUInt();
            string password = this.Request.PopFixedString();
            this.PrepareRoomForUser(id, password);
        }

        public void ExitGuild()
        {
            int guildId = this.Request.PopWiredInt32();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                Habbo habbo = ButterflyEnvironment.getHabboForId((uint)this.Request.PopWiredInt32());
                if (habbo != null)
                {
                    guild.Members.Remove((int)habbo.Id);
                    if (habbo.FavoriteGuild == guild.Id)
                    {
                        habbo.FavoriteGuild = 0;
                        if (habbo.CurrentRoomId > 0)
                        {
                            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom((uint)guild.RoomId);
                            if (room == null)
                            {
                                return;
                            }
                            List<RoomUser> list = new List<RoomUser>(ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(habbo.CurrentRoomId).GetRoomUserManager().UserList.Values);
                            ServerMessage message = new ServerMessage(Outgoing.SendGroup);
                            message.AppendInt32(0);
                            habbo.GetClient().SendMessage(message);
                            ServerMessage message2 = new ServerMessage(Outgoing.SetRoomUser);
                            message2.AppendInt32(1);
                            foreach (RoomUser user in list)
                            {
                                if (user.HabboId == habbo.Id)
                                {
                                    user.Serialize(message2, false);
                                    user.RemoveStatus("flatctrl 1");
                                    user.AddStatus("flatctrl 0", "");
                                    user.UpdateNeeded = true;
                                }
                            }
                            room.SendMessage(message2);
                        }
                    }
                    habbo.ImGuilds.Remove(guild.Id);
                    guild.GuildRanks.Remove((int)habbo.Id);

                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET FavoriteGroup = '", habbo.FavoriteGuild, "' WHERE Id = '", habbo.Id, "'" }));
                        adapter.runFastQuery("DELETE FROM user_groups WHERE GroupId = '" + guildId + "' AND userid = " + habbo.Id);
                    }
                    guild = ButterflyEnvironment.GetGame().GetGuilds().ReloadGuild(guildId);
                    if (habbo.Id != this.Session.GetHabbo().Id)
                        LoadMembersPetitions(0, guildId, 0, "");

                    else
                    {
                        RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData((uint)guild.RoomId);
                        ServerMessage message;
                    
                        message = new ServerMessage(Outgoing.SendAdvGroupInit);
                        message.AppendInt32(guild.Id);
                        message.AppendBoolean(true);
                        message.AppendInt32(guild.Type);
                        message.AppendString(guild.Name);
                        message.AppendString(guild.Description);
                        message.AppendString(guild.Image);
                        message.AppendInt32(data.Id);
                        message.AppendString(data.Name);
                        if (guild.Petitions.Contains((int)this.Session.GetHabbo().Id))
                        {
                            message.AppendInt32(2);
                        }
                        else if (!this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                        {
                            message.AppendInt32(0);
                        }
                        else if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                        {
                            message.AppendInt32(1);
                        }
                        message.AppendInt32(guild.Members.Count);
                        message.AppendBoolean(false);
                        message.AppendString(guild.DateCreated);
                        message.AppendBoolean(this.Session.GetHabbo().Id == guild.OwnerId);

                        if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                        {
                            if (guild.GuildRanks[(int)this.Session.GetHabbo().Id] < 2)
                                message.AppendBoolean(true);
                            else message.AppendBoolean(false);
                        }
                        else message.AppendBoolean(false);
                        message.AppendString(guild.OwnerName);
                        message.AppendBoolean(true);
                        message.AppendBoolean(true);
                        message.AppendInt32(guild.Members.ContainsKey((int)this.Session.GetHabbo().Id) ? guild.Petitions.Count : 0);
                        habbo.GetClient().SendMessage(message);

                    }

                }
            }
        }

        internal void ExtraVipWindow()
        {
            CatalogPage page = ButterflyEnvironment.GetGame().GetCatalog().GetPage(5);
            if ((((page != null) && page.Enabled) && (page.Visible && !page.ComingSoon)) && (page.MinRank <= this.Session.GetHabbo().Rank))
            {
                int i = this.Request.PopWiredInt32();
                if (page.Layout.Equals("club_buy"))
                {
                    ServerMessage message = new ServerMessage(Outgoing.ClubComposer);
                    message.AppendInt32(page.Items.Values.Count);
                    foreach (CatalogItem item in page.Items.Values)
                    {
                        item.SerializeClub(message, this.Session);
                    }
                    message.AppendInt32(i);
                    this.Session.SendMessage(message);
                }
            }
        }

        internal void FollowBuddy()
        {
            uint userID = this.Request.PopWiredUInt();
            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(userID);
            if (((clientByUserID != null) && (clientByUserID.GetHabbo() != null)) && clientByUserID.GetHabbo().InRoom)
            {
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(clientByUserID.GetHabbo().CurrentRoomId);
                Room room2 = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
                if (room != null)
                {
                    uint roomId = room.RoomId;
                    if (ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomId) != null)
                    {
                        this.GetResponse().Init(Outgoing.FollowBuddy);
                        this.GetResponse().AppendBoolean(room.IsPublic);
                        this.GetResponse().AppendInt32(clientByUserID.GetHabbo().CurrentRoomId);
                        this.SendResponse();
                        this.GetResponse().Init(Outgoing.RoomData);
                        this.GetResponse().AppendBoolean(false);
                        this.GetResponse().AppendInt32(room.RoomId);
                        this.GetResponse().AppendString(room.Name);
                        this.GetResponse().AppendBoolean(true);
                        this.GetResponse().AppendInt32(room.OwnerId);
                        this.GetResponse().AppendString(room.Owner);
                        this.GetResponse().AppendInt32(room.State);
                        this.GetResponse().AppendInt32(room.UsersNow);
                        this.GetResponse().AppendInt32(room.UsersMax);
                        this.GetResponse().AppendString(room.Description);
                        this.GetResponse().AppendInt32(0);
                        this.GetResponse().AppendInt32((room.Category == 0x34) ? 2 : 0);
                        this.GetResponse().AppendInt32(room.Score);
                        this.GetResponse().AppendInt32(0);
                        this.GetResponse().AppendInt32(room.Category);
                        if (room.GroupId == 0)
                        {
                            this.GetResponse().AppendInt32(0);
                            this.GetResponse().AppendInt32(0);
                        }
                        else
                        {
                            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(room.GroupId);
                            this.GetResponse().AppendInt32(guild.Id);
                            this.GetResponse().AppendString(guild.Name);
                            this.GetResponse().AppendString(guild.Image);
                        }
                        this.GetResponse().AppendString("");
                        this.GetResponse().AppendInt32(room.TagCount);
                        foreach (string str in room.Tags)
                        {
                            this.GetResponse().AppendString(str);
                        }
                        this.GetResponse().AppendInt32(0);
                        this.GetResponse().AppendInt32(0);
                        this.GetResponse().AppendInt32(0);
                        this.GetResponse().AppendBoolean(true);
                        this.GetResponse().AppendBoolean(true);
                        this.GetResponse().AppendInt32(0);
                        this.GetResponse().AppendInt32(0);
                        this.GetResponse().AppendBoolean(false);
                        this.GetResponse().AppendBoolean(false);
                        this.GetResponse().AppendBoolean(false);
                        this.GetResponse().AppendInt32(0);
                        this.GetResponse().AppendInt32(0);
                        this.GetResponse().AppendInt32(0);
                        this.GetResponse().AppendBoolean(false);
                        this.GetResponse().AppendBoolean(true);
                        this.SendResponse();
                    }
                }
            }
        }

        internal void FriendsListUpdate()
        {
            if (this.Session.GetHabbo().GetMessenger() == null)
            {
            }
        }

        internal void GetAchievements()
        {
            ButterflyEnvironment.GetGame().GetAchievementManager().GetList(this.Session, this.Request);
        }

        internal void GetBadges()
        {
            this.Session.SendMessage(this.Session.GetHabbo().GetBadgeComponent().Serialize());
        }

        internal void GetBalance()
        {
            this.Session.GetHabbo().UpdateCreditsBalance();
            this.Session.GetHabbo().UpdateActivityPointsBalance(false);
        }

        internal void GetCataData1()
        {
            this.GetResponse().Init(Outgoing.ShopData1);
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendInt32(0x2710);
            this.GetResponse().AppendInt32(0x30);
            this.GetResponse().AppendInt32(7);
            this.SendResponse();

            this.GetResponse().Init(Outgoing.RecyclePrizeList);
            Dictionary<uint, List<EcotronReward>> list;

            list = ButterflyEnvironment.GetGame().GetCatalog().GetEcotronRewards();
            this.GetResponse().AppendInt32(list.Count);

            foreach (var reward2 in list)
            {

                this.GetResponse().AppendInt32(reward2.Key);

                if (reward2.Key == 1)
                    this.GetResponse().AppendInt32(0);
                if (reward2.Key == 2)
                    this.GetResponse().AppendInt32(4);
                if (reward2.Key == 3)
                    this.GetResponse().AppendInt32(40);
                if (reward2.Key == 4)
                    this.GetResponse().AppendInt32(200);
                if (reward2.Key == 5)
                    this.GetResponse().AppendInt32(2000);
                Console.WriteLine(reward2.Key);
                this.GetResponse().AppendInt32(reward2.Value.Count);
                foreach (EcotronReward reward in reward2.Value)
                {
                    this.GetResponse().AppendString(reward.GetBaseItem().Name);
                    this.GetResponse().AppendInt32(1);
                    this.GetResponse().AppendString(reward.GetBaseItem().Type.ToString().ToLower());
                    this.GetResponse().AppendInt32(reward.GetBaseItem().SpriteId);
                }
            }
            this.SendResponse();
        }

        internal void BotsInventary() 
        {
            this.GetResponse().Init(Outgoing.ShopData1);
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendInt32(0x2710);
            this.GetResponse().AppendInt32(0x30);
            this.GetResponse().AppendInt32(7);
            this.SendResponse();
        }

        internal void GetCataData2()
        {
            int num;
            this.GetResponse().Init(Outgoing.ShopData2);
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendInt32(10);
            for (num = 0xd2c; num < 0xd36; num++)
            {
                this.GetResponse().AppendInt32(num);
            }
            this.GetResponse().AppendInt32(7);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendInt32(2);
            this.GetResponse().AppendInt32(3);
            this.GetResponse().AppendInt32(4);
            this.GetResponse().AppendInt32(5);
            this.GetResponse().AppendInt32(6);
            this.GetResponse().AppendInt32(11);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendInt32(2);
            this.GetResponse().AppendInt32(3);
            this.GetResponse().AppendInt32(4);
            this.GetResponse().AppendInt32(5);
            this.GetResponse().AppendInt32(6);
            this.GetResponse().AppendInt32(7);
            this.GetResponse().AppendInt32(8);
            this.GetResponse().AppendInt32(9);
            this.GetResponse().AppendInt32(10);
            this.GetResponse().AppendInt32(7);
            for (num = 0xbb; num < 0xc2; num++)
            {
                this.GetResponse().AppendInt32(num);
            }
            this.SendResponse();
            this.GetResponse().Init(Outgoing.Offer);
            this.GetResponse().AppendInt32(100);
            this.GetResponse().AppendInt32(6);
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendInt32(1);
            this.GetResponse().AppendInt32(2);
            this.GetResponse().AppendInt32(40);
            this.GetResponse().AppendInt32(0x63);
            this.SendResponse();
        }

        internal void GetCatalogIndex()
        {
            this.Session.SendMessage(ButterflyEnvironment.GetGame().GetCatalog().GetIndexMessageForRank(this.Session.GetHabbo().Rank));
        }

        internal void GetRoomAdsRooms()
        {
            Habbo habbo = this.Session.GetHabbo();

            ServerMessage MyRooms = new ServerMessage(Outgoing.SendRoomForCata);
            MyRooms.AppendBoolean(true);//?
            MyRooms.AppendInt32((int)habbo.UsersRooms.Count);//count

            foreach (RoomData data in habbo.UsersRooms)
            {
                MyRooms.AppendInt32(data.Id);
                MyRooms.AppendString(data.Name);
                MyRooms.AppendBoolean(false);

            }

            Session.SendMessage(MyRooms);
        }

        internal void GetCatalogPage()
        {
            CatalogPage page = ButterflyEnvironment.GetGame().GetCatalog().GetPage(this.Request.PopWiredInt32());
            if ((((page != null) && page.Enabled) && (page.Visible && !page.ComingSoon)) && (page.MinRank <= this.Session.GetHabbo().Rank))
            {
                //if (page.Caption == "Ecotron")
                //{
                //    this.Session.SendNotif("El Ecotr\x00f3n est\x00e1 fuera de servicio actualmente.");
                //}
                if (!(!page.ClubOnly || this.Session.GetHabbo().GetSubscriptionManager().HasSubscription("club_habbo")))
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("catalog.missingclubmembership"));
                }
                /*else if (page.Layout == "roomads")
                {
                    ServerMessage MyRooms = new ServerMessage(Outgoing.SendRoomForCata);
                    MyRooms.AppendBoolean(true);//?
                    MyRooms.AppendInt32(1);//count

                    MyRooms.AppendInt32(1);//room id
                    MyRooms.AppendString("Test");//room name
                    MyRooms.AppendBoolean(false);//boh

                    Session.SendMessage(page.GetMessage);
                    Session.SendMessage(MyRooms);
                }*/
                else
                {
                    //if (page.Layout == "recycler")
                    //{
                    //    this.Session.SendNotif("El ecotron no se encuentra disponible por el momento, \x00a1esp\x00e9ralo pronto!");
                    //}
                    this.Session.SendMessage(page.GetMessage);
                }
            }
        }

        internal void RecycleAgain()
        {
            GetResponse().Init(Outgoing.Recycle);
            GetResponse().AppendInt32(1);
            GetResponse().AppendInt32(0);
            SendResponse();
        }

        internal void GetEvents()
        {
            if (this.Session.GetHabbo() != null)
            {
                int category = int.Parse(this.Request.PopFixedString());
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeEventListing(category));
            }
        }

        internal void GetFavoriteRooms()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeFavoriteRooms(this.Session));
            }
        }

        internal void GetFlatCats()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeFlatCategories(this.Session));
            }
        }

        internal void GetFriendsRooms()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeNavigator(this.Session, -4));
            }
        }

        internal void GetGame()
        {
            int i = this.Request.PopWiredInt32();
            int num2 = this.Request.PopWiredInt32();
            if (this.Session.GetHabbo().CurrentRoomId > 0)
            {
                ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId).GetRoomUserManager().RemoveUserFromRoom(this.Session, false, false);
            }
            ServerMessage message = new ServerMessage(Outgoing.GetRankInGame);
            message.AppendInt32(i);
            if (this.Session.GetHabbo().GetSubscriptionManager().HasSubscription("club_habbo"))
            {
                message.AppendInt32(-1);
            }
            else
            {
                message.AppendInt32(3);
            }
            message.AppendInt32(0);
            this.Session.SendMessage(message);
        }

        internal void GetGames()
        {
            if ((this.Session.GetHabbo() != null) && ((this.Session.GetHabbo().InRoom && (this.Session.GetHabbo().CurrentRoomId > 0)) && (this.Session.CurrentRoomUserID > 0)))
            {
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
                if (room != null)
                {
                    room.GetRoomUserManager().RemoveUserFromRoom(this.Session, true, false);
                }
                this.Session.CurrentRoomUserID = -1;
                ServerMessage message = new ServerMessage(Outgoing.SendGroup);
                message.AppendInt32(0);
                this.Session.SendMessage(message);
            }
            
            this.GetResponse().Init(Outgoing.OpenGamesTab);
            this.GetResponse().AppendInt32(5);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendString("snowwar");
            this.GetResponse().AppendString("93d4f3");
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString(ButterflyEnvironment.HotelLink + "swfs/c_images/gamecenter_snowwar/");
            this.GetResponse().AppendInt32(3);
            this.GetResponse().AppendString("basejump");
            this.GetResponse().AppendString("68bbd2");
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString(ButterflyEnvironment.HotelLink + "swfs/c_images/gamecenter_basejump/");
            this.GetResponse().AppendInt32(4);
            this.GetResponse().AppendString("slotcar");
            this.GetResponse().AppendString("4a95df");
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString(ButterflyEnvironment.HotelLink + "swfs/c_images/gamecenter_slotcar/");
            this.GetResponse().AppendInt32(8);
            this.GetResponse().AppendString("Sulake_bestofturku");
            this.GetResponse().AppendString("0092b7");
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString(ButterflyEnvironment.HotelLink + "swfs/c_images/gamecenter_Sulake_bestofturku/");
            this.GetResponse().AppendInt32(9);
            this.GetResponse().AppendString("ChatRepublic_ssk");
            this.GetResponse().AppendString("fdc50d");
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString(ButterflyEnvironment.HotelLink + "swfs/c_images/gamecenter_ChatRepublic_ssk/");
            this.SendResponse();
        }

        internal void GetGuildFavorite()
        {
            int guildId = this.Request.PopWiredInt32();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                this.Session.GetHabbo().FavoriteGuild = guildId;
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET FavoriteGroup = '", guildId, "' WHERE Id = '", this.Session.GetHabbo().Id, "'" }));
                }
                ServerMessage message = new ServerMessage(Outgoing.UpdatePetitionsGuild);
                message.AppendInt32(0);
                message.AppendInt32(guild.Id);
                message.AppendInt32(2);
                message.AppendString(guild.Name);
                if (this.Session.GetHabbo().CurrentRoomId > 0)
                {
                    ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId).SendMessage(message);
                }
                else
                {
                    this.Session.SendMessage(message);
                }
                ServerMessage message2 = new ServerMessage(Outgoing.SendGroup);
                message2.AppendInt32(1);
                message2.AppendInt32(guild.Id);
                message2.AppendString(guild.Image);
                if (this.Session.GetHabbo().CurrentRoomId > 0)
                {
                    ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId).SendMessage(message2);
                }
                else
                {
                    this.Session.SendMessage(message2);
                }
                ServerMessage message3 = new ServerMessage(Outgoing.RemoveGuildFavorite);
                message3.AppendInt32((int)this.Session.GetHabbo().Id);
                this.Session.SendMessage(message3);
            }
        }

        internal void GetHelpCategories()
        {
            this.Session.SendMessage(ButterflyEnvironment.GetGame().GetHelpTool().SerializeIndex());
        }

        internal void GetHighRatedRooms()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeNavigator(this.Session, -2));
            }
        }

        internal void SaveBranding()
        {
            uint ItemId = Request.PopWiredUInt();
            uint Data = Request.PopWiredUInt();
            string BrandData = "state" + Convert.ToChar(9) + "0";
            for (int i = 1; i <= Data; i++)
            {
                BrandData = BrandData + Convert.ToChar(9) + Request.PopFixedString();
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            RoomItem Item = Room.GetRoomItemHandler().GetItem(ItemId);
            Item.ExtraData = BrandData;
            Room.GetRoomItemHandler().SetFloorItem(Session, Item, Item.GetX, Item.GetY, Item.Rot, false, false, true);

        }

        internal void GetInventory()
        {
            QueuedServerMessage message = new QueuedServerMessage(this.Session.GetConnection());
            message.appendResponse(this.Session.GetHabbo().GetInventoryComponent().SerializeFloorItemInventory());
            message.appendResponse(this.Session.GetHabbo().GetInventoryComponent().SerializeWallItemInventory());
            message.sendResponse();
        }

        internal void GetMoodlight()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                if (room.MoodlightData == null)
                {
                    foreach (RoomItem item in room.GetRoomItemHandler().mWallItems.Values)
                    {
                        if (item.GetBaseItem().InteractionType == InteractionType.dimmer)
                        {
                            room.MoodlightData = new MoodlightData(item.Id);
                        }
                    }
                }
                if (room.MoodlightData != null)
                {
                    this.Response.Init(Outgoing.DimmerData);
                    this.Response.AppendInt32(room.MoodlightData.Presets.Count);
                    this.Response.AppendInt32(room.MoodlightData.CurrentPreset);
                    int i = 0;
                    foreach (MoodlightPreset preset in room.MoodlightData.Presets)
                    {
                        i++;
                        this.Response.AppendInt32(i);
                        this.Response.AppendInt32((int)(int.Parse(ButterflyEnvironment.BoolToEnum(preset.BackgroundOnly)) + 1));
                        this.Response.AppendString(preset.ColorCode);
                        this.Response.AppendInt32(preset.ColorIntensity);
                    }
                    this.SendResponse();
                }
            }
        }

        public void GetMusicData()
        {
            try
            {
                int num = this.Request.PopWiredInt32();
                List<SongData> songs = new List<SongData>();
                for (int i = 0; i < num; i++)
                {
                    SongData song = SongManager.GetSong(this.Request.PopWiredInt32());
                    if (song != null)
                    {
                        songs.Add(song);
                    }
                }
                this.Session.SendMessage(JukeboxDiscksComposer.Compose(songs));
                songs.Clear();
                songs = null;
            }
            catch
            {
            }
        }

        public int GetMyRoomsGuilds()
        {
            int num = 0;
            foreach (RoomData data in this.Session.GetHabbo().UsersRooms)
            {
                if (data.GroupId != 0)
                {
                    num++;
                }
            }
            return num;
        }

        internal void GetOwnRooms()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeNavigator(this.Session, -3));
            }
        }

        internal void GetPetInfo()
        {
            if ((this.Session.GetHabbo() != null) && (this.Session.GetHabbo().CurrentRoom != null))
            {
                RoomUser pet = this.Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetPet(this.Request.PopWiredUInt());
                if ((pet == null) || (pet.PetData == null))
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("user.petinfoerror"));
                }
                else
                {
                    this.Session.SendMessage(pet.PetData.SerializeInfo());
                }
            }
        }

        internal void GetPetsInventory()
        {
            if (this.Session.GetHabbo().GetInventoryComponent() != null)
            {
                this.Session.SendMessage(this.Session.GetHabbo().GetInventoryComponent().SerializePetInventory());
            }
        }

        internal int GetPetTrainNumber(int Level)
        {
            switch (Level)
            {
                case 0:
                    return 2;

                case 1:
                    return 4;

                case 2:
                    return 5;

                case 3:
                    return 6;

                case 4:
                    return 7;

                case 5:
                    return 8;

                case 6:
                    return 9;

                case 7:
                    return 10;

                case 8:
                    return 11;

                case 9:
                    return 12;

                case 10:
                    return 13;

                case 11:
                    return 13;

                case 12:
                    return 14;

                case 13:
                    return 14;

                case 14:
                    return 15;

                case 15:
                    return 15;

                case 0x10:
                    return 0x10;

                case 0x11:
                    return 0x10;

                case 0x12:
                    return 0x11;

                case 0x13:
                    return 0x11;

                case 20:
                    return 0x11;
            }
            return 0x11;
        }

        internal void GetPopularRooms()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeNavigator(this.Session, int.Parse(this.Request.PopFixedString())));
            }
        }

        internal void GetPopularTags()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializePopularRoomTags());
            }
        }

        internal void GetPubs()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializePublicRooms());
            }
        }

        internal void GetRecentRooms()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeRecentRooms(this.Session));
            }
        }

        internal ServerMessage GetResponse()
        {
            return this.Response;
        }

        internal void GetRoomData2()
        {
            try
            {
                QueuedServerMessage message = new QueuedServerMessage(this.Session.GetConnection());
                if ((this.Session.GetHabbo().LoadingRoom > 0) && (this.CurrentLoadingRoom != null))
                {
                    RoomData roomData = this.CurrentLoadingRoom.RoomData;
                    if (roomData != null)
                    {
                        if (roomData.Model == null)
                        {
                            this.Session.SendNotif(LanguageLocale.GetValue("room.missingmodeldata"));
                            this.Session.SendMessage(new ServerMessage(Outgoing.OutOfRoom));
                            this.ClearRoomLoading();
                        }
                        else
                        {
                            message.appendResponse(this.CurrentLoadingRoom.GetGameMap().Model.GetHeightmap());
                            message.appendResponse(this.CurrentLoadingRoom.GetGameMap().Model.SerializeRelativeHeightmap());
                            message.sendResponse();
                        }
                    }
                    
                }
            }
            catch (Exception exception)
            {
                Logging.LogException(string.Concat(new object[] { "Unable to load room ID [", this.Session.GetHabbo().LoadingRoom, "] ", exception.ToString() }));
                this.Session.SendNotif(LanguageLocale.GetValue("room.roomdataloaderror"));
            }
        }

        internal void GetRoomData3()
        {
            if (((this.Session.GetHabbo().LoadingRoom > 0) && this.Session.GetHabbo().LoadingChecksPassed) && (this.CurrentLoadingRoom != null))
            {
                if (!(((this.CurrentLoadingRoom.UsersNow + 1) <= this.CurrentLoadingRoom.UsersMax) || this.Session.GetHabbo().HasFuse("fuse_enter_full_rooms")))
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("room.fullerror"));
                }
                else
                {
                    this.ClearRoomLoading();
                    QueuedServerMessage message = new QueuedServerMessage(this.Session.GetConnection());
                    if ((this.CurrentLoadingRoom.Type == "private") || (this.CurrentLoadingRoom.Type == "public"))
                    {
                        RoomItem[] source = this.CurrentLoadingRoom.GetRoomItemHandler().mFloorItems.Values.ToArray<RoomItem>();
                        RoomItem[] itemArray2 = this.CurrentLoadingRoom.GetRoomItemHandler().mWallItems.Values.ToArray<RoomItem>();
                        this.Response.Init(Outgoing.SerializeFloorItems);
                        if (source.Count<RoomItem>() > 0)
                        {
                            this.Response.AppendInt32(1);
                            this.Response.AppendInt32(this.CurrentLoadingRoom.OwnerId);
                            this.Response.AppendString(this.CurrentLoadingRoom.Owner);
                        }
                        else
                        {
                            this.Response.AppendInt32(0);
                        }
                        this.Response.AppendInt32(source.Length);
                        foreach (RoomItem item in source)
                        {
                            item.Serialize(this.Response, (int)item.placedBy);
                        }
                        message.appendResponse(this.GetResponse());
                        this.CurrentLoadingRoom.MovedThinksObH = true;
                        this.Response.Init(Outgoing.SerializeWallItems);
                        if (itemArray2.Count<RoomItem>() > 0)
                        {
                            this.Response.AppendInt32(1);
                            this.Response.AppendInt32(this.CurrentLoadingRoom.OwnerId);
                            this.Response.AppendString(this.CurrentLoadingRoom.Owner);
                        }
                        else
                        {
                            this.Response.AppendInt32(0);
                        }
                        this.Response.AppendInt32(itemArray2.Length);
                        foreach (RoomItem item in itemArray2)
                        {
                            item.Serialize(this.Response, (int)item.placedBy);
                        }
                        message.appendResponse(this.GetResponse());
                        
                        Array.Clear(source, 0, source.Length);
                        Array.Clear(itemArray2, 0, itemArray2.Length);
                        source = null;
                        itemArray2 = null;
                    }
                    this.CurrentLoadingRoom.GetRoomUserManager().AddUserToRoom(this.Session, false);
                    message.sendResponse();
                }
            }
        }

        internal void GetRoomEditData()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                this.GetResponse().Init(Outgoing.RoomDataEdit);
                this.GetResponse().AppendInt32(room.RoomId);
                this.GetResponse().AppendString(room.Name);
                this.GetResponse().AppendString(room.Description);
                this.GetResponse().AppendInt32(room.State);
                this.GetResponse().AppendInt32(room.Category);
                this.GetResponse().AppendInt32(room.UsersMax);
                this.GetResponse().AppendInt32(((room.RoomData.Model.MapSizeX * room.RoomData.Model.MapSizeY) > 100) ? 50 : 0x19);
                this.GetResponse().AppendInt32(room.TagCount);
                foreach (string str in room.Tags.ToArray())
                {
                    this.GetResponse().AppendString(str);
                }
                this.GetResponse().AppendInt32(room.UsersWithRights.Count);
                this.GetResponse().AppendInt32(room.AllowPets ? 1 : 0);
                this.GetResponse().AppendInt32(room.AllowPetsEating ? 1 : 0);
                this.GetResponse().AppendInt32(room.AllowWalkthrough ? 1 : 0);
                this.GetResponse().AppendInt32(room.Hidewall ? 1 : 0);
                this.GetResponse().AppendInt32(room.WallThickness);
                this.GetResponse().AppendInt32(room.FloorThickness);
                this.GetResponse().AppendInt32(0);//muto penso
                this.GetResponse().AppendInt32(0);//muto penso
                this.GetResponse().AppendInt32(0);//muto penso
                this.SendResponse();
                if (room.UsersWithRights.Count > 0)
                {
                    this.GetResponse().Init(Outgoing.GetPowerList);
                    this.GetResponse().AppendInt32(room.RoomData.Id);
                    this.GetResponse().AppendInt32(room.UsersWithRights.Count);
                    foreach (uint num in room.UsersWithRights)
                    {
                        Habbo habbo = ButterflyEnvironment.getHabboForId(num);
                        this.GetResponse().AppendInt32(habbo.Id);
                        this.GetResponse().AppendString(habbo.Username);
                    }
                    this.SendResponse();
                }
            }
        }


        internal void GetRoomsWithFriends()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeNavigator(this.Session, -5));
            }
        }

        internal void GetSubscriptionData()
        {
            this.GetResponse().Init(Outgoing.SerializeClub);
            this.GetResponse().AppendString("club_habbo");
            if (this.Session.GetHabbo().GetSubscriptionManager().HasSubscription("club_habbo"))
            {
                double expireTime = this.Session.GetHabbo().GetSubscriptionManager().GetSubscription("club_habbo").ExpireTime;
                double num2 = expireTime - ButterflyEnvironment.GetUnixTimestamp();
                int num3 = (int)Math.Ceiling((double)(num2 / 86400.0));
                int i = num3 / 0x1f;
                if (i >= 1)
                {
                    i--;
                }
                this.GetResponse().AppendInt32((int)(num3 - (i * 0x1f)));
                this.GetResponse().AppendInt32(2);//2
                this.GetResponse().AppendInt32(i);
                this.GetResponse().AppendInt32(1);
                this.GetResponse().AppendBoolean(true);
                this.GetResponse().AppendBoolean(true);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0x1ef);
            }
            else
            {
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendBoolean(false);
                this.GetResponse().AppendBoolean(true);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
            }
            this.SendResponse();
        }

        internal void GetTopicsInCategory()
        {
            uint categoryId = this.Request.PopWiredUInt();
            HelpCategory category = ButterflyEnvironment.GetGame().GetHelpTool().GetCategory(categoryId);
            if (category != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetHelpTool().SerializeCategory(category));
            }
        }

        internal void GetTrainerPanel()
        {
            uint petId = this.Request.PopWiredUInt();
            Pet pet = null;
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            if ((currentRoom != null) && ((pet = currentRoom.GetRoomUserManager().GetPet(petId).PetData) != null))
            {
                int level = pet.Level;
                pet = null;
                this.GetResponse().Init(Outgoing.PetTrainPanel);
                this.GetResponse().AppendInt32(petId);
                this.GetResponse().AppendInt32(0x11);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(14);
                this.GetResponse().AppendInt32(0x2b);
                this.GetResponse().AppendInt32(0x2c);
                this.GetResponse().AppendInt32(2);
                this.GetResponse().AppendInt32(3);
                this.GetResponse().AppendInt32(6);
                this.GetResponse().AppendInt32(7);
                this.GetResponse().AppendInt32(10);
                this.GetResponse().AppendInt32(12);
                this.GetResponse().AppendInt32(11);
                this.GetResponse().AppendInt32(15);
                this.GetResponse().AppendInt32(0x10);
                this.GetResponse().AppendInt32(0x18);
                this.GetResponse().AppendInt32(0x19);
                this.GetResponse().AppendInt32(0x1a);
                this.GetResponse().AppendInt32(0x2d);
                int petTrainNumber = this.GetPetTrainNumber(level);
                this.GetResponse().AppendInt32(petTrainNumber);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(14);
                if (level > 0)
                {
                    this.GetResponse().AppendInt32(0x2b);
                    this.GetResponse().AppendInt32(0x2c);
                }
                if (level > 1)
                {
                    this.GetResponse().AppendInt32(2);
                }
                if (level > 2)
                {
                    this.GetResponse().AppendInt32(3);
                }
                if (level > 3)
                {
                    this.GetResponse().AppendInt32(6);
                }
                if (level > 4)
                {
                    this.GetResponse().AppendInt32(7);
                }
                if (level > 5)
                {
                    this.GetResponse().AppendInt32(10);
                }
                if (level > 6)
                {
                    this.GetResponse().AppendInt32(12);
                }
                if (level > 7)
                {
                    this.GetResponse().AppendInt32(11);
                }
                if (level > 8)
                {
                    this.GetResponse().AppendInt32(15);
                }
                if (level > 9)
                {
                    this.GetResponse().AppendInt32(0x10);
                }
                if (level > 12)
                {
                    this.GetResponse().AppendInt32(0x18);
                }
                if (level > 13)
                {
                    this.GetResponse().AppendInt32(0x19);
                }
                if (level > 15)
                {
                    this.GetResponse().AppendInt32(0x1a);
                }
                if (level > 0x11)
                {
                    this.GetResponse().AppendInt32(0x2d);
                }
                this.SendResponse();
            }
        }

        internal void GetUserBadges()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Request.PopWiredUInt());
                if (((roomUserByHabbo != null) && !roomUserByHabbo.IsBot) && (roomUserByHabbo.GetClient() != null))
                {
                    this.Response.Init(Outgoing.GetUserBadges);
                    this.Response.AppendInt32(roomUserByHabbo.GetClient().GetHabbo().Id);
                    this.Response.AppendInt32(roomUserByHabbo.GetClient().GetHabbo().GetBadgeComponent().EquippedCount);
                    foreach (Badge badge in roomUserByHabbo.GetClient().GetHabbo().GetBadgeComponent().BadgeList.Values)
                    {
                        if (badge.Slot > 0)
                        {
                            this.Response.AppendInt32(badge.Slot);
                            this.Response.AppendString(badge.Code);
                        }
                    }
                    this.Response.AppendInt32(10);
                    this.Response.AppendInt32(10);
                    this.Response.AppendInt32(10);
                    
                    this.SendResponse();
                }
            }
        }

        internal void GetUserInfo()
        {

            this.Response.Init(Outgoing.HabboInfomation);
            this.Response.AppendInt32(this.Session.GetHabbo().Id);
            this.Response.AppendString(this.Session.GetHabbo().Username);
            this.Response.AppendString(this.Session.GetHabbo().Look);
            this.Response.AppendString(this.Session.GetHabbo().Gender.ToUpper());
            this.Response.AppendString(this.Session.GetHabbo().Motto);
            this.Response.AppendString(this.Session.GetHabbo().RealName);
            this.Response.AppendBoolean(false);//false
            this.Response.AppendInt32(this.Session.GetHabbo().Respect);
            this.Response.AppendInt32(this.Session.GetHabbo().DailyRespectPoints);
            this.Response.AppendInt32(this.Session.GetHabbo().DailyPetRespectPoints);
            this.Response.AppendBoolean(true);
            this.Response.AppendString(this.Session.GetHabbo().LastOnline);
            this.Response.AppendBoolean(false);//this.Session.GetHabbo().CanChangeName
            this.Response.AppendBoolean(false);//false
            Console.WriteLine(this.Response.ToString());
            this.SendResponse();

            this.GetResponse().Init(Outgoing.Allowances);
            this.GetResponse().AppendInt32(7);//this.Session.GetHabbo().PassedQuiz ? 7 : 5);
            this.GetResponse().AppendString("VOTE_IN_COMPETITIONS");
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString("TRADE");
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString("CITIZEN");
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendString("");
            //if (this.Session.GetHabbo().PassedQuiz)
            //{
                this.GetResponse().AppendString("SAFE_CHAT");
                this.GetResponse().AppendBoolean(true);
                this.GetResponse().AppendString("");
                this.GetResponse().AppendString("FULL_CHAT");
                this.GetResponse().AppendBoolean(true);
                this.GetResponse().AppendString("");
            //}
            this.GetResponse().AppendString("CALL_ON_HELPERS");
            this.GetResponse().AppendBoolean(true);
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString("USE_GUIDE_TOOL");
            this.GetResponse().AppendBoolean(false);
            this.GetResponse().AppendString("requirement.unfulfilled.helper_level_4");
            this.GetResponse().AppendString("JUDGE_CHAT_REVIEWS");
            this.GetResponse().AppendBoolean(false);
            this.GetResponse().AppendString("requirement.unfulfilled.helper_level_6");
            this.SendResponse();
            this.GetResponse().Init(Outgoing.AchievementPoints);
            this.GetResponse().AppendInt32(this.Session.GetHabbo().AchievementPoints);
            this.SendResponse();
            this.InitMessenger();
            this.SendAllowances();
             

        }

        internal void GetUserTags()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Request.PopWiredUInt());
                if ((roomUserByHabbo != null) && !roomUserByHabbo.IsBot)
                {
                    this.Response.Init(Outgoing.GetUserTags);
                    this.Response.AppendInt32(roomUserByHabbo.GetClient().GetHabbo().Id);
                    this.Response.AppendInt32(roomUserByHabbo.GetClient().GetHabbo().Tags.Count);
                    foreach (string str in roomUserByHabbo.GetClient().GetHabbo().Tags)
                    {
                        this.Response.AppendString(str);
                    }
                    this.SendResponse();
                }
            }
        }

        internal void GetWardrobe()
        {
            Session.GetHabbo().GetAvatarEffectsInventoryComponent().Serialize();//ok
            this.GetResponse().Init(Outgoing.WardrobeData);
            this.GetResponse().AppendInt32(this.Session.GetHabbo().GetSubscriptionManager().HasSubscription("club_habbo") ? 1 : 0);
            if (this.Session.GetHabbo().GetSubscriptionManager().HasSubscription("club_habbo"))
            {
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("SELECT slot_id, look, gender FROM user_wardrobe WHERE user_id = " + this.Session.GetHabbo().Id);
                    DataTable table = adapter.getTable();
                    if (table == null)
                    {
                        this.GetResponse().AppendInt32(0);
                    }
                    else
                    {
                        this.GetResponse().AppendInt32(table.Rows.Count);
                        foreach (DataRow row in table.Rows)
                        {
                            this.GetResponse().AppendInt32(Convert.ToUInt32(row["slot_id"]));
                            this.GetResponse().AppendString((string)row["look"]);
                            this.GetResponse().AppendString(row["gender"].ToString().ToUpper());
                        }
                    }
                }
                this.SendResponse();
            }
        }

        internal void GiveHanditem()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    RoomUser user2 = room.GetRoomUserManager().GetRoomUserByHabbo(this.Request.PopWiredUInt());
                    if ((user2 != null) && ((roomUserByHabbo.CarryItemID > 0) && (roomUserByHabbo.CarryTimer > 0)))
                    {
                        user2.CarryItem(roomUserByHabbo.CarryItemID);
                        roomUserByHabbo.CarryItem(0);
                    }
                }
            }
        }

        internal void GiveRespect()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && (this.Session.GetHabbo().DailyRespectPoints > 0))
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Request.PopWiredUInt());
                if (((roomUserByHabbo != null) && (roomUserByHabbo.GetClient().GetHabbo().Id != this.Session.GetHabbo().Id)) && !roomUserByHabbo.IsBot)
                {
                    ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("GIVE_RESPECT", this.Session);
                    ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(this.Session, "ACH_RespectGiven", 1);
                    ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(roomUserByHabbo.GetClient(), "ACH_RespectEarned", 1);
                    Habbo habbo = this.Session.GetHabbo();
                    habbo.DailyRespectPoints--;
                    Habbo habbo2 = roomUserByHabbo.GetClient().GetHabbo();
                    habbo2.Respect++;
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery("UPDATE users SET respect = respect + 1 WHERE id = " + roomUserByHabbo.GetClient().GetHabbo().Id);
                        adapter.runFastQuery("UPDATE users SET daily_respect_points = daily_respect_points - 1 WHERE id = " + this.Session.GetHabbo().Id);
                    }
                    ServerMessage message = new ServerMessage(Outgoing.Action);
                    message.AppendInt32(room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id).VirtualId);
                    message.AppendInt32(7);
                    room.SendMessage(message);
                    ServerMessage message2 = new ServerMessage(Outgoing.GiveRespect);
                    message2.AppendInt32(roomUserByHabbo.GetClient().GetHabbo().Id);
                    message2.AppendInt32(roomUserByHabbo.GetClient().GetHabbo().Respect);
                    room.SendMessage(message2);
                }
            }
        }

        internal void GiveRights(uint uid = 0)
        {
           uint pId;

           if (uid == 0)
               pId = this.Request.PopWiredUInt();
           else
               pId = uid;

           Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
           if (room != null)
           {
               Habbo habbo = ButterflyEnvironment.getHabboForId(pId);
               if ((((room != null) && room.CheckRights(this.Session, true)) && (habbo != null)))
               {
                   if (room.UsersWithRights.Contains(pId))
                   {
                       this.Session.SendNotif(LanguageLocale.GetValue("user.giverights.error"));
                   }
                   else
                   {
                       room.UsersWithRights.Add(pId);
                       try
                       {
                           using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                           {
                               adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO room_rights (room_id,user_id) VALUES (", room.RoomId, ",", pId, ")" }));
                           }
                       }
                       catch
                       {
                       }
                       this.Response.Init(Outgoing.GivePowers);
                       this.Response.AppendInt32(room.RoomId);
                       this.Response.AppendInt32(pId);
                       this.Response.AppendString(habbo.Username);
                       this.SendResponse();

                       RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(pId);
                       if (roomUserByHabbo != null)
                       {
                           ServerMessage message = new ServerMessage(Outgoing.RoomRightsLevel);
                           message.AppendInt32(1);
                           roomUserByHabbo.GetClient().SendMessage(message);
                           roomUserByHabbo.RemoveStatus("flatctrl 0");
                           roomUserByHabbo.AddStatus("flatctrl 1", "");
                           roomUserByHabbo.UpdateNeeded = true;
                       }
                   }
                }
            }
        }

        internal void GoToHotelView()
        {
            try
            {
                if ((this.Session.GetHabbo() != null) && ((this.Session.GetHabbo().InRoom && (this.Session.GetHabbo().CurrentRoomId > 0)) && (this.Session.CurrentRoomUserID > 0)))
                {
                    Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
                    if (room != null)
                    {
                        room.GetRoomUserManager().RemoveUserFromRoom(this.Session, true, false);
                    }
                    this.Session.CurrentRoomUserID = -1;
                }
            }
            catch
            {
            }
        }

        internal void HandlePurchase()
        {
            int pageId = this.Request.PopWiredInt32();
            uint itemId = this.Request.PopWiredUInt();
            string extraData = this.Request.PopFixedString();
            int priceAmount = this.Request.PopWiredInt32();
            ButterflyEnvironment.GetGame().GetCatalog().HandlePurchase(this.Session, pageId, itemId, extraData, priceAmount, false, "", "", 0, 0, 0, false);
        }

        internal void HandleRequest(ClientMessage request)
        {
            if (ButterflyEnvironment.diagPackets)
            {
                DateTime now = DateTime.Now;
                this.Request = request;
                StaticClientMessageHandler.HandlePacket(this, request);
                TimeSpan span = (TimeSpan)(DateTime.Now - now);
                if (span.TotalMilliseconds > ButterflyEnvironment.timeout)
                {
                    Console.WriteLine(string.Concat(new object[] { "Packet ", request.Id, " took ", span.Milliseconds, "ms to run. Packetdata: ", request.ToString() }));
                }
            }
            else
            {
                this.Request = request;
                StaticClientMessageHandler.HandlePacket(this, request);
            }
        }

        internal void IgnoreUser()
        {
            if (this.Session.GetHabbo().CurrentRoom != null)
            {
                string username = this.Request.PopFixedString();
                Habbo habbo = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(username).GetHabbo();
                if ((habbo != null) && !this.Session.GetHabbo().MutedUsers.Contains(habbo.Id))
                {
                    this.Session.GetHabbo().MutedUsers.Add(habbo.Id);
                    this.Response.Init(Outgoing.UpdateIgnoreStatus);
                    this.Response.AppendInt32(1);
                    this.Response.AppendString(username);
                    this.SendResponse();
                }
            }
        }

        internal void InitCrypto()
        {
            this.Session.TimePingedReceived = DateTime.Now;
            this.Response.Init(Outgoing.SendBannerMessageComposer);
            this.Response.AppendString("12f449917de4f94a8c48dbadd92b6276");
            this.Response.AppendBoolean(false);
            this.SendResponse();
        }

        internal void InitHelpTool()
        {
            this.Session.SendMessage(ButterflyEnvironment.GetGame().GetHelpTool().SerializeFrontpage());
        }

        internal void InitMessenger()
        {
            this.Session.GetHabbo().InitMessenger();
        }

        internal void InitSecretKey()
        {
            this.Session.TimePingedReceived = DateTime.Now;
            this.Response.Init(Outgoing.SecretKeyComposer);
            this.Response.AppendString(ButterflyEnvironment.secretKey);
            this.SendResponse();
        }

        internal void InitStream()
        {
            int id = this.Request.PopWiredInt32();
            DataTable table = null;
            if (id <= 1)
            {
                id = (int)this.Session.GetHabbo().Id;
            }
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM user_stream WHERE UserId = '" + id + "' ORDER BY Id DESC LIMIT 13");
                table = adapter.getTable();
            }
            ServerMessage message = new ServerMessage(Outgoing.InitStream);
            message.AppendInt32((ButterflyEnvironment.HotelStreamMessage == "") ? (table.Rows.Count + 1) : (table.Rows.Count + 2));
            foreach (DataRow row in table.Rows)
            {
                Habbo habbo = ButterflyEnvironment.getHabboForId(Convert.ToUInt32(row["UserId"]));
                message.AppendInt32((int)row["Id"]);
                message.AppendInt32((int)row["Type"]);
                message.AppendString(habbo.Id.ToString());
                message.AppendString(habbo.Username);
                message.AppendString(habbo.Gender.ToLower());
                if (((int)row["Type"]) == 0)
                {
                    string im = ButterflyEnvironment.StreamLink + ButterflyEnvironment.GetGame().GetClientManager().GetLookById(Convert.ToUInt32(row["FriendId"])) + ".gif";
                    message.AppendString((string)im);
                }
                else
                {
                    message.AppendString((string)row["Image"]);
                }
                message.AppendInt32(10);
                message.AppendInt32(5);
                message.AppendInt32((int)row["Likes"]);
                message.AppendBoolean(true);
                message.AppendBoolean(true);
                message.AppendBoolean(false);
                if (((int)row["Type"]) == 0)
                {
                    Habbo habbo2 = ButterflyEnvironment.getHabboForId(Convert.ToUInt32(row["FriendId"]));
                    message.AppendString(habbo2.Id.ToString());
                    message.AppendString(habbo2.Username);
                }
                else if (((int)row["Type"]) == 10)
                {
                    MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(Convert.ToInt32(row["FriendId"]));
                    message.AppendString(guild.RoomId.ToString());
                    message.AppendString(guild.Id.ToString());
                    message.AppendString(ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData((uint)guild.RoomId).Name);
                    message.AppendString(guild.Name.ToString());
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                }
            }
            if (ButterflyEnvironment.HotelStreamMessage != "")
            {
                message.AppendInt32(0);
                message.AppendInt32(4);
                message.AppendString("-1");
                message.AppendString("");
                message.AppendString("null");
                message.AppendString("");
                message.AppendInt32(0);
                message.AppendInt32(6);
                message.AppendInt32(0);
                message.AppendBoolean(true);
                message.AppendBoolean(false);
                message.AppendBoolean(false);
                message.AppendString(ButterflyEnvironment.HotelStreamMessage);
                message.AppendString(ButterflyEnvironment.HotelLink);
            }
            message.AppendInt32(0);
            message.AppendInt32(4);
            message.AppendString("-1");
            message.AppendString("");
            message.AppendString("null");
            message.AppendString("");
            message.AppendInt32(0);
            message.AppendInt32(6);
            message.AppendInt32(0);
            message.AppendBoolean(true);
            message.AppendBoolean(false);
            message.AppendBoolean(false);
            message.AppendString("Swift Emulator - AKllX's @ hotelweblink.com");
            message.AppendString("http://google.it.com/");
            this.Session.SendMessage(message);
        }

        internal void InitTrade()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CanTradeInRoom)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                RoomUser roomUserByVirtualId = room.GetRoomUserManager().GetRoomUserByVirtualId(this.Request.PopWiredInt32());
                if (((roomUserByVirtualId != null) && (roomUserByVirtualId.GetClient() != null)) && (roomUserByVirtualId.GetClient().GetHabbo() != null))
                {
                    bool flag = false;
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery("SELECT block_trade FROM users WHERE id = " + roomUserByVirtualId.GetClient().GetHabbo().Id);
                        flag = ButterflyEnvironment.EnumToBool(adapter.getString());
                    }
                    if (flag)
                    {
                        this.Session.SendNotif(LanguageLocale.GetValue("user.tradedisabled"));
                    }
                    else
                    {
                        room.TryStartTrade(roomUserByHabbo, roomUserByVirtualId);
                    }
                }
            }
        }

        internal void KickBot()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                RoomUser roomUserByVirtualId = room.GetRoomUserManager().GetRoomUserByVirtualId(this.Request.PopWiredInt32());
                if ((roomUserByVirtualId != null) && roomUserByVirtualId.IsBot)
                {
                    room.GetRoomUserManager().RemoveBot(roomUserByVirtualId.VirtualId, true);
                }
            }
        }

        internal void KickUser()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session))
            {
                uint pId = this.Request.PopWiredUInt();
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(pId);
                if (((roomUserByHabbo != null) && !roomUserByHabbo.IsBot) && ((!room.CheckRights(roomUserByHabbo.GetClient(), true) && !roomUserByHabbo.GetClient().GetHabbo().HasFuse("fuse_mod")) && !roomUserByHabbo.GetClient().GetHabbo().HasFuse("fuse_no_kick")))
                {
                    room.GetRoomUserManager().RemoveUserFromRoom(roomUserByHabbo.GetClient(), true, true);
                    roomUserByHabbo.GetClient().CurrentRoomUserID = -1;
                }
            }
        }

        internal void LeaveGame()
        {
            if (((this.Session != null) || (this.Session.GetHabbo() != null)) && ((this.Session.GetHabbo().SnowWar != null) && this.Session.GetHabbo().SnowWar.WarUsers.Contains(this.Session.GetHabbo())))
            {
                Butterfly.HabboHotel.Games.SnowStorm.SnowStorm snowWar = this.Session.GetHabbo().SnowWar;
                ServerMessage packet = new ServerMessage(Outgoing.LeaveGame);
                packet.AppendInt32(this.Session.GetHabbo().Id);
                this.Session.GetHabbo().SnowWar.SendToStorm(packet, true, this.Session.GetHabbo().Id);
                snowWar.WarUsers.Remove(this.Session.GetHabbo());
                if ((snowWar.WarOwner == this.Session.GetHabbo()) && (snowWar.WarUsers.Count > 0))
                {
                    snowWar.WarOwner = snowWar.WarUsers[0];
                }
                this.Session.GetHabbo().SnowWar = null;
            }
        }

        internal void stageReadyMessage()
        {
            Console.WriteLine("Stage ready....");
        }

        internal void LoadArena(Butterfly.HabboHotel.Games.SnowStorm.SnowStorm War)
        {
            this.GetResponse().Init(Outgoing.Game2StageStartingMessageEvent);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendString("snowwar_arena_0");
            this.GetResponse().AppendInt32(5);
            this.CheckArenaStatic(this.GetResponse(), War);
            foreach (Habbo habbo in War.WarUsers)
            {
                this.GetResponse().AppendInt32(5);
                this.GetResponse().AppendInt32(habbo.SnowUserId);
                this.GetResponse().AppendInt32(habbo.SnowX);
                this.GetResponse().AppendInt32(habbo.SnowY);
                this.GetResponse().AppendInt32((int)(habbo.SnowX / 0xc80));
                this.GetResponse().AppendInt32((int)(habbo.SnowY / 0xc80));
                this.GetResponse().AppendInt32(2);
                this.GetResponse().AppendInt32(5);
                this.GetResponse().AppendInt32(5);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32((int)(habbo.SnowX / 0xc80));
                this.GetResponse().AppendInt32((int)(habbo.SnowY / 0xc80));
                this.GetResponse().AppendInt32(habbo.SnowX);
                this.GetResponse().AppendInt32(habbo.SnowY);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(habbo.SnowTeam);
                this.GetResponse().AppendInt32(habbo.Id);
                this.GetResponse().AppendString(habbo.Username);
                this.GetResponse().AppendString(habbo.Motto);
                this.GetResponse().AppendString(habbo.Look);
                this.GetResponse().AppendString(habbo.Gender.ToLower());
            }
            War.SendToStorm(this.GetResponse(), false, 0);
            Thread.Sleep(5000);
            foreach (Habbo habbo in War.WarUsers)
            {
                this.GetResponse().Init(Outgoing.Game2PlayerExitedGameArenaMessageEvent);
                this.GetResponse().AppendInt32(habbo.Id);
                this.GetResponse().AppendInt32(20);
                habbo.GetClient().SendMessage(this.GetResponse());
            }
            this.GetResponse().Init(Outgoing.Game2StageRunningMessageEvent);
            this.GetResponse().AppendInt32(120);
            War.SendToStorm(this.GetResponse(), false, 0);
            War.SnowStormStart();
        }

        public void LoadInvSongs()
        {
            if (((this.Session != null) && (this.Session.GetHabbo() != null)) && (this.Session.GetHabbo().GetInventoryComponent() != null))
            {
                this.Session.SendMessage(JukeboxDiscksComposer.SerializeSongInventory(this.Session.GetHabbo().GetInventoryComponent().songDisks));
            }
        }

        public void LoadJukeSongs()
        {
            if (((this.Session != null) && (this.Session.GetHabbo() != null)) && (this.Session.GetHabbo().CurrentRoom != null))
            {
                Room currentRoom = this.Session.GetHabbo().CurrentRoom;
                if (currentRoom.CheckRights(this.Session, true) && currentRoom.GotMusicController())
                {
                    RoomMusicController roomMusicController = currentRoom.GetRoomMusicController();
                    this.Session.SendMessage(JukeboxDiscksComposer.Compose(roomMusicController.PlaylistCapacity, roomMusicController.Playlist.Values.ToList<SongInstance>()));
                }
            }
        }

        public void RejectGuildMember()
        {
            int guildId = this.Request.PopWiredInt32();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                int num2 = this.Request.PopWiredInt32();
                RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData((uint)guild.RoomId);
                
                Habbo habbo = ButterflyEnvironment.getHabboForId((uint)num2);
                if (habbo != null)
                {
                    Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom((uint)guild.RoomId);
                    guild.Petitions.Remove((int)habbo.Id);
                    string str = "";
                    foreach (int num3 in guild.Petitions)
                    {
                        str = str + num3 + ";";
                    }
                    if (guild.Petitions.Count > 1)
                    {
                        str = str.Substring(0, str.Length - 1);
                    }
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE groups SET Petitions = '", str, "' WHERE Id = '", guild.Id, "'" }));
                    }
                    ServerMessage message;
                    message = new ServerMessage(Outgoing.SendAdvGroupInit);
                    message.AppendInt32(guild.Id);
                    message.AppendBoolean(true);
                    message.AppendInt32(guild.Type);
                    message.AppendString(guild.Name);
                    message.AppendString(guild.Description);
                    message.AppendString(guild.Image);
                    message.AppendInt32(data.Id);
                    message.AppendString(data.Name);
                    if (guild.Petitions.Contains((int)habbo.Id))
                    {
                        message.AppendInt32(2);
                    }
                    else if (!habbo.ImGuilds.Contains(guild.Id))
                    {
                        message.AppendInt32(0);
                    }
                    else if (habbo.ImGuilds.Contains(guild.Id))
                    {
                        message.AppendInt32(1);
                    }
                    message.AppendInt32(guild.Members.Count);
                    message.AppendBoolean(false);
                    message.AppendString(guild.DateCreated);
                    message.AppendBoolean(habbo.Id == guild.OwnerId);

                    if (habbo.ImGuilds.Contains(guild.Id))
                    {
                        if (guild.GuildRanks[(int)habbo.Id] < 2)
                            message.AppendBoolean(true);
                        else message.AppendBoolean(false);
                    }
                    else message.AppendBoolean(false);
                    message.AppendString(guild.OwnerName);
                    message.AppendBoolean(true);
                    message.AppendBoolean(true);
                    message.AppendInt32(guild.Members.ContainsKey((int)habbo.Id) ? guild.Petitions.Count : 0);
                    habbo.GetClient().SendMessage(message);

                }
            }
            LoadMembersPetitions(2,guild.Id);
                
        }

        public void LoadMembersPetitions(int num=4, int guildid=0, int numb2=0, string st ="")
        {
            int guildId;
            int num2;
            int num3;
            string str;
            
            if (num == 4)
            {
                guildId = this.Request.PopWiredInt32();
                num2 = this.Request.PopWiredInt32();
                str = this.Request.PopFixedString();            
                num3 = this.Request.PopWiredInt32();
            }
            else
            {
                guildId = guildid;
                num2 = numb2;
                str = st;       
                num3 = num;
            }

            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                ServerMessage message;
                Habbo habbo;
                int memberCounter = 0;
                int results;
                if (num3 == 0)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        results = 0;
                        foreach (int num4 in guild.Members.Keys)
                        {
                            habbo = ButterflyEnvironment.getHabboForId((uint)num4);
                            if (habbo.Username.ToLower().Contains(str.ToLower()))
                                results++;   
                        }
                    }

                    else
                        results = guild.Members.Count();

                    message = new ServerMessage(Outgoing.SendMembersAndPetitions);
                    message.AppendInt32(guild.Id);
                    message.AppendString(guild.Name);
                    message.AppendInt32(guild.RoomId);
                    message.AppendString(guild.Image);
                    message.AppendInt32(results);
                    
                    if(num2==0)
                        message.AppendInt32(results);
                    else 
                        message.AppendInt32(results - (num2*14));     
           
                    foreach (int num4 in guild.Members.Keys)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            habbo = ButterflyEnvironment.getHabboForId((uint)num4);
                            if(habbo.Username.ToLower().Contains(str.ToLower()))
                            {
                                if (memberCounter >= ((num2) * 14))
                                {
                                    if (guild.OwnerId == habbo.Id)
                                    {
                                        message.AppendInt32(0);
                                    }
                                    else
                                    {
                                        message.AppendInt32(guild.GuildRanks[num4]);
                                    }
                                    message.AppendInt32(num4);
                                    message.AppendString(habbo.Username);
                                    message.AppendString(habbo.Look);
                                    message.AppendString(guild.Members[num4]);                                        
                                }
                            memberCounter++;
                            }
                        }

                        else
                        {
                            if (memberCounter >= (num2 * 14))
                            {
                                habbo = ButterflyEnvironment.getHabboForId((uint)num4);
                                if (guild.OwnerId == habbo.Id)
                                {
                                    message.AppendInt32(0);
                                }
                                else
                                {
                                    message.AppendInt32(guild.GuildRanks[num4]);
                                }
                                message.AppendInt32(num4);
                                message.AppendString(habbo.Username);
                                message.AppendString(habbo.Look);
                                message.AppendString(guild.Members[num4]);
                            }
                            memberCounter++;
                        }
                    }

                    if(this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                    {
                        if (guild.GuildRanks[(int)this.Session.GetHabbo().Id]<2)
                            message.AppendBoolean(true);
                        else message.AppendBoolean(false);
                    }

                    else message.AppendBoolean(false);
                    message.AppendInt32(14);
                    message.AppendInt32(num2);
                    message.AppendInt32(0);
                    message.AppendString(str);
                    this.Session.SendMessage(message);
                }
                else if (num3 == 1)
                {
                    int admins = 0;
                    
                    foreach (int num4 in guild.Members.Keys)
                    {
                        if (guild.GuildRanks[num4] < 2) admins++;
                    }

                    if (!string.IsNullOrEmpty(str))
                    {
                        results = 0;
                        foreach (int num4 in guild.Members.Keys)
                        {
                            habbo = ButterflyEnvironment.getHabboForId((uint)num4);
                            if ((habbo.Username.ToLower().Contains(str.ToLower())) && ((guild.GuildRanks[num4] < 2)))
                                results++;
                        }
                    }

                    else
                        results = admins;

                    message = new ServerMessage(Outgoing.SendMembersAndPetitions);
                    message.AppendInt32(guild.Id);
                    message.AppendString(guild.Name);
                    message.AppendInt32(guild.RoomId);
                    message.AppendString(guild.Image);
                    message.AppendInt32(results);

                    if (num2 == 0)
                        message.AppendInt32(results);

                    else message.AppendInt32(results - (num2 * 14));
                    memberCounter = 0;
                    foreach (int num4 in guild.Members.Keys)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            habbo = ButterflyEnvironment.getHabboForId((uint)num4);
                            if (habbo.Username.ToLower().Contains(str.ToLower()))
                            {
                                if (guild.GuildRanks[(int)num4] < 2)
                                {
                                    if (memberCounter >= ((num2) * 14))
                                    {
                                        if (guild.OwnerId == habbo.Id)
                                        {
                                            message.AppendInt32(0);
                                        }
                                        else
                                        {
                                            message.AppendInt32(guild.GuildRanks[num4]);
                                        }
                                        message.AppendInt32(num4);
                                        message.AppendString(habbo.Username);
                                        message.AppendString(habbo.Look);
                                        message.AppendString(guild.Members[num4]);
                                    }
                                }
                                memberCounter++;
                            }
                        }

                        else
                        {
                            if (memberCounter >= (num2 * 14))
                            {
                                habbo = ButterflyEnvironment.getHabboForId((uint)num4);

                                if (guild.GuildRanks[(int)num4] < 2)
                                {
                                    if (guild.OwnerId == habbo.Id)
                                    {
                                        message.AppendInt32(0);
                                    }
                                    else
                                    {
                                        message.AppendInt32(guild.GuildRanks[num4]);
                                    }
                                    message.AppendInt32(num4);
                                    message.AppendString(habbo.Username);
                                    message.AppendString(habbo.Look);
                                    message.AppendString(guild.Members[num4]);
                                }
                            }
                            memberCounter++;
                        }
                    }
                    if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                    {
                        if (guild.GuildRanks[(int)this.Session.GetHabbo().Id] < 2)
                            message.AppendBoolean(true);
                        else message.AppendBoolean(false);
                    }
                    else message.AppendBoolean(false);
                    message.AppendInt32(14);
                    message.AppendInt32(num2);
                    message.AppendInt32(1);
                    message.AppendString(str);
                    this.Session.SendMessage(message);
                }

                else if (num3 == 2)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        results = 0;
                        foreach (int num4 in guild.Petitions)
                        {
                            habbo = ButterflyEnvironment.getHabboForId((uint)num4);
                            if (habbo.Username.ToLower().Contains(str.ToLower()))
                                results++;
                        }
                    }

                    else
                        results = guild.Petitions.Count();

                    message = new ServerMessage(Outgoing.SendMembersAndPetitions);
                    message.AppendInt32(guild.Id);
                    message.AppendString(guild.Name);
                    message.AppendInt32(guild.RoomId);
                    message.AppendString(guild.Image);
                    message.AppendInt32(results);
                    if (num2 == 0)
                        message.AppendInt32(results);

                    else message.AppendInt32(results - (num2 * 14));
                    memberCounter = 0;
                    foreach (int num4 in guild.Petitions)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            habbo = ButterflyEnvironment.getHabboForId((uint)num4);
                            if (habbo.Username.ToLower().Contains(str.ToLower()))
                            {
                                if (memberCounter >= ((num2) * 14))
                                {
                                    message.AppendInt32(3);
                                    message.AppendInt32(num4);
                                    message.AppendString(habbo.Username);
                                    message.AppendString(habbo.Look);
                                    message.AppendString("");
                                }
                                memberCounter++;
                            }
                        }

                        else
                        {
                            if (memberCounter >= (num2 * 14))
                            {
                                habbo = ButterflyEnvironment.getHabboForId((uint)num4);
                                message.AppendInt32(3);
                                message.AppendInt32(num4);
                                message.AppendString(habbo.Username);
                                message.AppendString(habbo.Look);
                                message.AppendString("");
                            }
                            memberCounter++;
                        }
                    }
                    if (guild.GuildRanks[(int)this.Session.GetHabbo().Id] < 2)
                        message.AppendBoolean(true);
                    else message.AppendBoolean(false);
                    message.AppendInt32(14);
                    message.AppendInt32(num2);
                    message.AppendInt32(2);
                    message.AppendString(str);
                    this.Session.SendMessage(message);
                }
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        internal void LoadProfile()
        {
            try
            {
                int num = this.Request.PopWiredInt32();
                bool flag = this.Request.PopWiredBoolean();
                Habbo habbo = ButterflyEnvironment.getHabboForId((uint)num);
                if (habbo == null)
                {
                    Console.WriteLine("can't get data for profile with userid = " + num);
                }
                else
                {
                    DataRow Info;
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery("SELECT `reg_timestamp` FROM `user_info` WHERE user_id = '" + habbo.Id + "'");
                        Info = adapter.getRow();
                    }
                    TimeSpan span = (TimeSpan)(DateTime.Now - Convert.ToDateTime(habbo.LastOnline));
                    List<MyGuild> list = new List<MyGuild>();
                    foreach (int num2 in habbo.ImGuilds)
                    {
                        MyGuild item = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(num2);
                        if (!((item == null) || list.Contains(item)))
                        {
                            list.Add(item);
                        }
                    }
                    this.Response.Init(Outgoing.ProfileInformation);
                    this.Response.AppendInt32(habbo.Id);
                    this.Response.AppendString(habbo.Username);
                    this.Response.AppendString(habbo.Look);
                    this.Response.AppendString(habbo.Motto);
                    this.Response.AppendString(UnixTimeStampToDateTime((double)Info["reg_timestamp"]).ToShortDateString());
                    this.Response.AppendInt32(habbo.AchievementPoints);
                    this.Response.AppendInt32(habbo.GetMessenger().myFriends);
                    this.Response.AppendBoolean(habbo.Id != this.Session.GetHabbo().Id);
                    this.Response.AppendBoolean(false);
                    this.Response.AppendBoolean(ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(habbo.Id) != null);
                    this.Response.AppendInt32(list.Count);
                    foreach (MyGuild guild2 in list)
                    {
                        this.Response.AppendInt32(guild2.Id);
                        this.Response.AppendString(guild2.Name);
                        this.Response.AppendString(guild2.Image);
                        this.Response.AppendString(guild2.HtmlColor1);
                        this.Response.AppendString(guild2.HtmlColor2);
                        this.Response.AppendBoolean(habbo.FavoriteGuild == guild2.Id);
                    }
                    //this.Response.AppendInt32((int)span.TotalSeconds);
                    //this.Response.AppendBoolean(true);
                    this.Response.AppendBoolean(false);
                    this.Response.AppendInt32((int)span.TotalSeconds);
                    this.Response.AppendBoolean(true);
                    this.SendResponse();
                }
            }
            catch
            {
            }
        }

        internal QueuedServerMessage LoadRoomForUser()
        {
            Room currentLoadingRoom = this.CurrentLoadingRoom;
            QueuedServerMessage message = new QueuedServerMessage(this.Session.GetConnection());
            if ((currentLoadingRoom != null) && this.Session.GetHabbo().LoadingChecksPassed)
            {
                this.Response.Init(Outgoing.InitialRoomInformation);
                this.Response.AppendString(currentLoadingRoom.ModelName);
                this.Response.AppendInt32(currentLoadingRoom.RoomId);
                message.appendResponse(this.GetResponse());
                if ((currentLoadingRoom.Type == "private") || (currentLoadingRoom.Type == "public"))
                {
                    if (currentLoadingRoom.Wallpaper != "0.0")
                    {
                        this.Response.Init(Outgoing.RoomDecoration);
                        this.Response.AppendString("wallpaper");
                        this.Response.AppendString(currentLoadingRoom.Wallpaper);
                        message.appendResponse(this.GetResponse());
                    }
                    if (currentLoadingRoom.Floor != "0.0")
                    {
                        this.Response.Init(Outgoing.RoomDecoration);
                        this.Response.AppendString("floor");
                        this.Response.AppendString(currentLoadingRoom.Floor);
                        message.appendResponse(this.GetResponse());
                    }
                    this.Response.Init(Outgoing.RoomDecoration);
                    this.Response.AppendString("landscape");
                    this.Response.AppendString(currentLoadingRoom.Landscape);
                    message.appendResponse(this.GetResponse());
                    
                    if (currentLoadingRoom.CheckRights(this.Session, true))
                    {
                        this.Response.Init(Outgoing.RoomRightsLevel);
                        this.Response.AppendInt32(4);
                        message.appendResponse(this.GetResponse());
                        this.Response.Init(Outgoing.HasOwnerRights);
                        message.appendResponse(this.GetResponse());
                    }

                    else if ((currentLoadingRoom.GroupId>0)&&(currentLoadingRoom.checkGroupRights(this.Session)))
                    {
                        this.Response.Init(Outgoing.RoomRightsLevel);
                        this.Response.AppendInt32(1);
                        message.appendResponse(this.GetResponse());
                        this.Response.Init(Outgoing.HasOwnerRights);
                        message.appendResponse(this.GetResponse());
                    }
                    
                    else if (currentLoadingRoom.CheckRights(this.Session))
                    {
                        this.Response.Init(Outgoing.RoomRightsLevel);
                        this.Response.AppendInt32(1);
                        message.appendResponse(this.GetResponse());
                    }
                    
                    else
                    {
                        this.Response.Init(Outgoing.RoomRightsLevel);
                        this.Response.AppendInt32(0);
                        message.appendResponse(this.GetResponse());
                    }


                    this.Response.Init(Outgoing.ScoreMeter);
                    this.Response.AppendInt32(currentLoadingRoom.Score);
                    this.Response.AppendBoolean(!this.Session.GetHabbo().RatedRooms.Contains(currentLoadingRoom.RoomId) && !currentLoadingRoom.CheckRights(this.Session, true));
                    message.appendResponse(this.GetResponse());
                }
                List<MyGuild> list = new List<MyGuild>();
                MyGuild item = null;
                foreach (RoomUser user in currentLoadingRoom.GetRoomUserManager().UserList.Values)
                {
                    if ((!user.IsPet && !user.IsBot) && (user.GetClient().GetHabbo().FavoriteGuild > 0))
                    {
                        MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(user.GetClient().GetHabbo().FavoriteGuild);
                        if ((guild != null) && !list.Contains(guild))
                        {
                            lock (list)
                            {
                                list.Add(guild);
                            }
                        }
                    }
                }
                if (this.Session.GetHabbo().FavoriteGuild > 0)
                {
                    item = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(this.Session.GetHabbo().FavoriteGuild);
                    if ((item != null) && !list.Contains(item))
                    {
                        lock (list)
                        {
                            list.Add(item);
                        }
                    }
                }
                this.Response.Init(Outgoing.SendGroup);
                this.Response.AppendInt32(list.Count);
                foreach (MyGuild guild2 in list)
                {
                    this.Response.AppendInt32(guild2.Id);
                    this.Response.AppendString(guild2.Image);
                }
                message.appendResponse(this.GetResponse());
                try
                {
                    if (item != null)
                    {
                        ServerMessage message2 = new ServerMessage(Outgoing.SendGroup);
                        message2.AppendInt32(1);
                        message2.AppendInt32(item.Id);
                        message2.AppendString(item.Image);
                        currentLoadingRoom.SendMessage(message2);
                    }
                }
                catch
                {
                }
            }
            return message;
        }

        public void LoadTalents()
        {
            bool flag = false;
            Dictionary<string, int> achievementGroup = new Dictionary<string, int>();
            List<string> badge = new List<string>();
            achievementGroup.Add("ACH_AvatarLooks", 1);
            badge.Add("ACH_AvatarLooks1");
            achievementGroup.Add("ACH_RespectGiven", 1);
            badge.Add("ACH_RespectGiven1");
            achievementGroup.Add("ACH_AllTimeHotelPresence", 1);
            badge.Add("ACH_AllTimeHotelPresence1");
            achievementGroup.Add("ACH_RoomEntry", 1);
            badge.Add("ACH_RoomEntry1");
            if (ButterflyEnvironment.GetGame().GetAchievementManager().ContainsGroupAch(achievementGroup, this.Session) || this.Session.GetHabbo().GetBadgeComponent().HasBadgeList(badge))
            {
                flag = true;
            }
            bool flag2 = false;
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
            List<string> list2 = new List<string>();
            dictionary2.Add("ACH_GuideAdvertisementReader", 1);
            list2.Add("ACH_GuideAdvertisementReader1");
            dictionary2.Add("ACH_RegistrationDuration", 1);
            list2.Add("ACH_RegistrationDuration1");
            dictionary2.Add("ACH_AllTimeHotelPresence", 2);
            list2.Add("ACH_AllTimeHotelPresence2");
            dictionary2.Add("ACH_RoomEntry", 2);
            list2.Add("ACH_RoomEntry2");
            if (ButterflyEnvironment.GetGame().GetAchievementManager().ContainsGroupAch(dictionary2, this.Session) || this.Session.GetHabbo().GetBadgeComponent().HasBadgeList(list2))
            {
                flag2 = true;
            }
            bool flag3 = false;
            Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
            List<string> list3 = new List<string>();
            dictionary3.Add("ACH_GuideAdvertisementReader", 1);
            list3.Add("ACH_GuideAdvertisementReader1");
            dictionary3.Add("ACH_RegistrationDuration", 2);
            list3.Add("ACH_RegistrationDuration2");
            dictionary3.Add("ACH_AllTimeHotelPresence", 3);
            list3.Add("ACH_AllTimeHotelPresence3");
            dictionary3.Add("ACH_RoomEntry", 3);
            list3.Add("ACH_RoomEntry3");
            if (ButterflyEnvironment.GetGame().GetAchievementManager().ContainsGroupAch(dictionary3, this.Session) || this.Session.GetHabbo().GetBadgeComponent().HasBadgeList(list3))
            {
                flag3 = true;
            }
            ServerMessage message = new ServerMessage(Outgoing.CitizenshipProgress);
            message.AppendString("citizenship");
            message.AppendInt32(5);
            message.AppendInt32(0);
            message.AppendInt32(this.Session.GetHabbo().PassedQuiz ? 2 : 1);
            message.AppendInt32(1);
            message.AppendInt32(0x7d);
            message.AppendInt32(1);
            message.AppendString("ACH_SafetyQuizGraduate1");
            message.AppendInt32(this.Session.GetHabbo().PassedQuiz ? 2 : 1);
            message.AppendInt32(this.Session.GetHabbo().PassedQuiz ? 1 : 0);
            message.AppendInt32(1);
            message.AppendInt32(0);
            message.AppendInt32(1);
            message.AppendString("A1 KUMIANKKA");
            message.AppendInt32(0);
            message.AppendInt32(1);
            message.AppendInt32(flag ? 2 : (this.Session.GetHabbo().PassedQuiz ? 1 : 0));
            message.AppendInt32(4);
            message.AppendInt32(6);
            message.AppendInt32(1);
            message.AppendString("ACH_AvatarLooks1");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_AvatarLooks", 1, this.Session) || this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AvatarLooks1")) ? 2 : (this.Session.GetHabbo().PassedQuiz ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(1);
            message.AppendInt32(0x12);
            message.AppendInt32(1);
            message.AppendString("ACH_RespectGiven1");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_RespectGiven", 1, this.Session) || this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RespectGiven1")) ? 2 : (this.Session.GetHabbo().PassedQuiz ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(2);
            message.AppendInt32(0x13);
            message.AppendInt32(1);
            message.AppendString("ACH_AllTimeHotelPresence1");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_AllTimeHotelPresence", 1, this.Session) || this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AllTimeHotelPresence1")) ? 2 : (this.Session.GetHabbo().PassedQuiz ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(30);
            message.AppendInt32(8);
            message.AppendInt32(1);
            message.AppendString("ACH_RoomEntry1");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_RoomEntry", 1, this.Session) || this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RoomEntry1")) ? 2 : (this.Session.GetHabbo().PassedQuiz ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(5);
            message.AppendInt32(0);
            message.AppendInt32(1);
            message.AppendString("A1 KUMIANKKA");
            message.AppendInt32(flag2 ? 2 : (flag ? 1 : 0));
            message.AppendInt32(2);
            message.AppendInt32(0);
            message.AppendInt32(4);
            message.AppendInt32(0x91);
            message.AppendInt32(1);
            message.AppendString("ACH_GuideAdvertisementReader1");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_GuideAdvertisementReader", 1, this.Session) || (this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_GuideAdvertisementReader1") && flag)) ? 2 : (flag ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(1);
            message.AppendInt32(11);
            message.AppendInt32(1);
            message.AppendString("ACH_RegistrationDuration1");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_RegistrationDuration", 1, this.Session) || (this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RegistrationDuration1") && flag)) ? 2 : (flag ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(1);
            message.AppendInt32(0x13);
            message.AppendInt32(2);
            message.AppendString("ACH_AllTimeHotelPresence2");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_AllTimeHotelPresence", 2, this.Session) || (this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AllTimeHotelPresence2") && flag)) ? 2 : (flag ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(60);
            message.AppendInt32(8);
            message.AppendInt32(2);
            message.AppendString("ACH_RoomEntry2");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_RoomEntry", 2, this.Session) || (this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RoomEntry2") && flag)) ? 2 : (flag ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(20);
            message.AppendInt32(0);
            message.AppendInt32(1);
            message.AppendString("A1 KUMIANKKA");
            message.AppendInt32(flag3 ? 2 : (flag2 ? 1 : 0));
            message.AppendInt32(3);
            message.AppendInt32(0);
            message.AppendInt32(4);
            message.AppendInt32(11);
            message.AppendInt32(2);
            message.AppendString("ACH_RegistrationDuration2");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_RegistrationDuration", 2, this.Session) || (this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_RegistrationDuration2") && flag2)) ? 2 : (flag2 ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(3);
            message.AppendInt32(0x5e);
            message.AppendInt32(1);
            message.AppendString("ACH_HabboWayGraduate1");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_HabboWayGraduate", 1, this.Session) || (this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_HabboWayGraduate1") && flag2)) ? 2 : (flag2 ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(1);
            message.AppendInt32(0x13);
            message.AppendInt32(3);
            message.AppendString("ACH_AllTimeHotelPresence3");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_AllTimeHotelPresence", 3, this.Session) || (this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_AllTimeHotelPresence3") && flag2)) ? 2 : (flag2 ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(120);
            message.AppendInt32(0x8e);
            message.AppendInt32(1);
            message.AppendString("ACH_FriendListSize1");
            message.AppendInt32((ButterflyEnvironment.GetGame().GetAchievementManager().ContainsAchievement("ACH_FriendListSize", 1, this.Session) || (this.Session.GetHabbo().GetBadgeComponent().HasBadge("ACH_FriendListSize1") && flag2)) ? 2 : (flag2 ? 1 : 0));
            message.AppendInt32(0);
            message.AppendInt32(2);
            message.AppendInt32(1);
            message.AppendString("TRADE");
            message.AppendInt32(1);
            message.AppendString("A1 KUMIANKKA");
            message.AppendInt32(flag3 ? 1 : 0);
            message.AppendInt32(4);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendInt32(1);
            message.AppendString("CITIZEN");
            message.AppendInt32(2);
            message.AppendString("A1 KUMIANKKA");
            message.AppendInt32(0);
            message.AppendString("HABBO_CLUB_CITIZENSHIP_VIP_REWARD");
            message.AppendInt32(7);
            this.Session.SendMessage(message);
        }

        public void LoadUser2()
        {
            int num = this.Request.PopWiredInt32();
            int num2 = this.Request.PopWiredInt32();
            int num3 = this.Request.PopWiredInt32();
            if ((num2 == 1) && (num3 == 0))
            {
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom((uint)num);
                if ((room != null) && (room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id) == null))
                {
                }
            }
            else if ((num2 != 0) || (num3 != 0))
            {
                RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData((uint)num);
                if (data != null)
                {
                    ServerMessage message = new ServerMessage(Outgoing.RoomData);
                    message.AppendBoolean(false);
                    data.Serialize(message, false);
                    message.AppendBoolean(true);
                    message.AppendBoolean(false);
                    message.AppendBoolean(true);
                    message.AppendBoolean(true);
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendBoolean(true);
                    this.Session.SendMessage(message);
                    //Console.WriteLine("aa");
                }
            }
        }

        internal void LookAt()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.Unidle();
                    int num = this.Request.PopWiredInt32();
                    int num2 = this.Request.PopWiredInt32();
                    if ((num != roomUserByHabbo.X) || (num2 != roomUserByHabbo.Y))
                    {
                        int rotation = Rotation.Calculate(roomUserByHabbo.X, roomUserByHabbo.Y, num, num2);
                        roomUserByHabbo.SetRot(rotation, false);
                        roomUserByHabbo.UpdateNeeded = true;
                    }
                }
            }


        }

        public void MarketplaceCanSell()
        {
            ServerMessage message = new ServerMessage(Outgoing.CanSell);
            message.AppendInt32(1);
            message.AppendInt32(0);
            this.Session.SendMessage(message);
        }

        internal void MarketplaceClaimCredits()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT asking_price FROM catalog_marketplace_offers WHERE user_id = " + this.Session.GetHabbo().Id + " AND state = 2");
                DataRow row = adapter.getRow();
                if (row.Table.Rows.Count > 0)
                {
                    int num = 0;
                    foreach (DataRow row2 in row.Table.Rows)
                    {
                        num += (int)row2["asking_price"];
                    }
                    if (num >= 1)
                    {
                        Habbo habbo = this.Session.GetHabbo();
                        habbo.Credits += num;
                        this.Session.GetHabbo().UpdateCreditsBalance();
                    }
                    adapter.runFastQuery("DELETE FROM catalog_marketplace_offers WHERE user_id = " + this.Session.GetHabbo().Id + " AND state = 2");
                }
            }
        }

        internal void MarketplaceGetOffers()
        {
            int minCost = this.Request.PopWiredInt32();
            int maxCost = this.Request.PopWiredInt32();
            string searchQuery = this.Request.PopFixedString();
            int filterMode = this.Request.PopWiredInt32();
            ServerMessage message = Marketplace.SerializeOffers(minCost, maxCost, searchQuery, filterMode);
            this.Session.SendMessage(message);
        }

        public void MarketplaceGetOwnOffers()
        {
            this.Session.SendMessage(Marketplace.SerializeOwnOffers((int)this.Session.GetHabbo().Id));
        }

        public void MarketplacePostItem()
        {
            if (this.Session.GetHabbo().GetInventoryComponent() != null)
            {
                int sellingPrice = this.Request.PopWiredInt32();
                int num2 = this.Request.PopWiredInt32();
                int num3 = this.Request.PopWiredInt32();
                UserItem item = this.Session.GetHabbo().GetInventoryComponent().GetItem((uint)num3);
                if ((item != null) && item.GetBaseItem().AllowTrade)
                {
                    Marketplace.SellItem(this.Session, (int)item.Id, sellingPrice);
                }
            }
        }

        internal void MarketplacePurchase()
        {
            int i = this.Request.PopWiredInt32();
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM catalog_marketplace_offers WHERE offer_id = " + i + " ");
                DataRow row = adapter.getRow();
                if (((row.Table.Rows.Count <= 0) || (((int)row["state"]) != 1)) || (((double)row["timestamp"]) <= Marketplace.FormatTimestamp()))
                {
                    this.Session.SendNotif("La oferta est\x00e1 expirada");
                }
                else
                {
                    Item item = ButterflyEnvironment.GetGame().GetItemManager().GetItem(Convert.ToUInt32(row["item_id"]));
                    if (item != null)
                    {
                        int num2 = (int)row["total_price"];
                        if (((int)row["total_price"]) >= 1)
                        {
                            Habbo habbo = this.Session.GetHabbo();
                            habbo.Credits -= num2;
                            this.Session.GetHabbo().UpdateCreditsBalance();
                        }
                        List<UserItem> list = ButterflyEnvironment.GetGame().GetCatalog().DeliverItems(this.Session, item, 1, (string)row["extra_data"], 0, 0);
                        this.Session.GetHabbo().GetInventoryComponent().RunDBUpdate();
                        adapter.runFastQuery("UPDATE catalog_marketplace_offers SET state = 2 WHERE offer_id = " + i);
                        ServerMessage message = new ServerMessage(Outgoing.BuyItemMarket);
                        message.AppendInt32(1);
                        message.AppendInt32(i);
                        message.AppendInt32(2);
                        message.AppendInt32(i);
                        this.Session.SendMessage(message);
                        ServerMessage message2 = new ServerMessage(Outgoing.SendPurchaseAlert);
                        message2.AppendInt32(1);
                        message2.AppendInt32((int)row["item_type"]);
                        message2.AppendInt32(list.Count);
                        foreach (UserItem item2 in list.ToArray())
                        {
                            message2.AppendInt32(item2.Id);
                        }
                        this.Session.SendMessage(message2);
                        this.Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                        ServerMessage message3 = Marketplace.SerializeOffers(-1, -1, "", 1);
                        this.Session.SendMessage(message3);
                    }
                }
            }
        }

        public void MarketplaceTakeBack()
        {
            int i = this.Request.PopWiredInt32();
            DataRow row = null;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM catalog_marketplace_offers WHERE offer_id = " + i + " LIMIT 1");
                row = adapter.getRow();
                if ((row.Table.Rows.Count > 0) && (Convert.ToInt32(row["user_id"]) == this.Session.GetHabbo().Id))
                {
                    Item item = ButterflyEnvironment.GetGame().GetItemManager().GetItem(Convert.ToUInt32(row["item_id"]));
                    if (item != null)
                    {
                        ButterflyEnvironment.GetGame().GetCatalog().DeliverItems(this.Session, item, 1, (string)row["extra_data"], 0, 0);
                        adapter.runFastQuery("DELETE FROM catalog_marketplace_offers WHERE offer_id = " + i);
                        ServerMessage message = new ServerMessage(Outgoing.QuitMarket);
                        message.AppendInt32(i);
                        message.AppendBoolean(true);
                        this.Session.SendMessage(message);
                        this.Session.SendMessage(new ServerMessage(Outgoing.AddMarket2));
                    }
                }
            }
        }

        internal void ModBanUser()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_ban"))
            {
                uint userId = this.Request.PopWiredUInt();
                string message = this.Request.PopFixedString();
                int length = this.Request.PopWiredInt32() * 0xe10;
                ModerationTool.BanUser(this.Session, userId, length, message);
            }
        }

        internal void ModCloseTicket()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_mod"))
            {
                int result = this.Request.PopWiredInt32();
                int num2 = this.Request.PopWiredInt32();
                uint ticketId = this.Request.PopWiredUInt();
                ButterflyEnvironment.GetGame().GetModerationTool().CloseTicket(this.Session, ticketId, result);
            }
        }

        internal void ModGetRoomChatlog()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_chatlogs"))
            {
                int num = this.Request.PopWiredInt32();
                uint roomID = this.Request.PopWiredUInt();
                if (ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(roomID) != null)
                {
                    this.Session.SendMessage(ModerationTool.SerializeRoomChatlog(roomID));
                }
            }
        }

        internal void ModGetRoomTool()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_mod"))
            {
                uint roomId = this.Request.PopWiredUInt();
                RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(roomId);
                this.Session.SendMessage(ModerationTool.SerializeRoomTool(data));
            }
        }

        internal void ModGetRoomVisits()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_mod"))
            {
                uint userId = this.Request.PopWiredUInt();
                this.Session.SendMessage(ModerationTool.SerializeRoomVisits(userId));
            }
        }

        internal void ModGetTicketChatlog()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_mod"))
            {
                SupportTicket ticket = ButterflyEnvironment.GetGame().GetModerationTool().GetTicket(this.Request.PopWiredUInt());
                if (ticket != null)
                {
                    RoomData roomData = ButterflyEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(ticket.RoomId);
                    if (roomData != null)
                    {
                        this.Session.SendMessage(ModerationTool.SerializeTicketChatlog(ticket, roomData, ticket.Timestamp));
                    }
                }
            }
        }

        internal void ModGetUserChatlog()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_chatlogs"))
            {
                this.Session.SendMessage(ModerationTool.SerializeUserChatlog(this.Request.PopWiredUInt()));
            }
        }

        internal void ModGetUserInfo()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_mod"))
            {
                uint id = this.Request.PopWiredUInt();
                if (ButterflyEnvironment.GetGame().GetClientManager().GetNameById(id) != "Unknown User")
                {
                    this.Session.SendMessage(ModerationTool.SerializeUserInfo(id));
                }
                else
                {
                    this.Session.SendNotif(LanguageLocale.GetValue("user.loadusererror"));
                }
            }
        }

        internal void ModKickUser()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_kick"))
            {
                uint userId = this.Request.PopWiredUInt();
                string message = this.Request.PopFixedString();
                ModerationTool.KickUser(this.Session, userId, message, false);
            }
        }

        internal void ModPerformRoomAction()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_mod"))
            {
                uint roomId = this.Request.PopWiredUInt();
                bool lockRoom = this.Request.PopWiredInt32() == 1;
                bool inappropriateRoom = this.Request.PopWiredInt32() == 1;
                bool kickUsers = this.Request.PopWiredInt32() == 1;
                ModerationTool.PerformRoomAction(this.Session, roomId, kickUsers, lockRoom, inappropriateRoom);
            }
        }

        internal void ModPickTicket()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_mod"))
            {
                int num = this.Request.PopWiredInt32();
                uint ticketId = this.Request.PopWiredUInt();
                ButterflyEnvironment.GetGame().GetModerationTool().PickTicket(this.Session, ticketId);
            }
        }

        internal void ModReleaseTicket()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_mod"))
            {
                int num = this.Request.PopWiredInt32();
                for (int i = 0; i < num; i++)
                {
                    uint ticketId = this.Request.PopWiredUInt();
                    ButterflyEnvironment.GetGame().GetModerationTool().ReleaseTicket(this.Session, ticketId);
                }
            }
        }

        internal void ModSendRoomAlert()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_alert"))
            {
                int num = this.Request.PopWiredInt32();
                int num2 = this.Request.PopWiredInt32();
                string s = this.Request.PopFixedString();
                ServerMessage message = new ServerMessage(Outgoing.SendNotif);
                message.AppendString(s);
                message.AppendString("");
                this.Session.GetHabbo().CurrentRoom.SendMessage(message);
            }
        }

        internal void ModSendUserCaution()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_alert"))
            {
                uint userId = this.Request.PopWiredUInt();
                string message = this.Request.PopFixedString();
                ModerationTool.AlertUser(this.Session, userId, message, true);
            }
        }

        internal void ModSendUserMessage()
        {
            if (this.Session.GetHabbo().HasFuse("fuse_alert"))
            {
                uint userId = this.Request.PopWiredUInt();
                string message = this.Request.PopFixedString();
                ModerationTool.AlertUser(this.Session, userId, message, false);
            }
        }

        internal void MountOnPet()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    uint petId = this.Request.PopWiredUInt();
                    bool flag = this.Request.PopWiredBoolean();
                    RoomUser pet = room.GetRoomUserManager().GetPet(petId);
                    if ((((pet != null) && (pet.PetData != null)) && (!pet.montandoBol || roomUserByHabbo.montandoBol)) && ((pet.PetData.OwnerId == roomUserByHabbo.HabboId) || pet.PetData.CanMountAllPeople))
                    {
                        if (flag)
                        {
                            pet.Statusses.Remove("sit");
                            pet.Statusses.Remove("lay");
                            pet.Statusses.Remove("snf");
                            pet.Statusses.Remove("eat");
                            pet.Statusses.Remove("ded");
                            pet.Statusses.Remove("jmp");
                            int x = roomUserByHabbo.X;
                            int y = roomUserByHabbo.Y;
                            pet.ClearMovement(true);
                            roomUserByHabbo.montandoBol = true;
                            pet.montandoBol = true;
                            pet.montandoID = (uint)roomUserByHabbo.VirtualId;
                            roomUserByHabbo.montandoID = Convert.ToUInt32(pet.VirtualId);
                            pet.PetData.AddExpirience(10);
                            room.SendMessage(room.GetRoomItemHandler().UpdateUserOnRoller(pet, new Point(x, y), 0, room.GetGameMap().SqAbsoluteHeight(x, y)));
                            room.GetRoomUserManager().UpdateUserStatus(pet, false);
                            room.SendMessage(room.GetRoomItemHandler().UpdateUserOnRoller(roomUserByHabbo, new Point(x, y), 0, room.GetGameMap().SqAbsoluteHeight(x, y) + 1.0));
                            room.GetRoomUserManager().UpdateUserStatus(roomUserByHabbo, false);
                            if (pet.PetData.HaveSaddle == 1)
                            {
                                roomUserByHabbo.ApplyEffect(0x4d);
                            }
                            else if (pet.PetData.HaveSaddle == 2)
                            {
                                roomUserByHabbo.ApplyEffect(0x67);
                            }
                            roomUserByHabbo.MoveTo(x + 1, y + 1);
                        }
                        else
                        {
                            pet.Statusses.Remove("sit");
                            pet.Statusses.Remove("lay");
                            pet.Statusses.Remove("snf");
                            pet.Statusses.Remove("eat");
                            pet.Statusses.Remove("ded");
                            pet.Statusses.Remove("jmp");
                            roomUserByHabbo.montandoBol = false;
                            roomUserByHabbo.montandoID = 0;
                            pet.montandoBol = false;
                            pet.montandoID = 0;
                            roomUserByHabbo.MoveTo(roomUserByHabbo.X + 1, roomUserByHabbo.Y + 1);
                            roomUserByHabbo.ApplyEffect(-1);
                        }
                        ServerMessage message = new ServerMessage(Outgoing.SerializeMontura);
                        message.AppendInt32(pet.PetData.VirtualId);
                        message.AppendInt32(pet.PetData.PetId);
                        message.AppendInt32(pet.PetData.Type);
                        message.AppendInt32(int.Parse(pet.PetData.Race));
                        message.AppendString(pet.PetData.Color.ToLower());
                        if (pet.PetData.HaveSaddle == 2)
                        {
                            message.AppendInt32(1);
                            message.AppendInt32(3);
                            message.AppendInt32(4);
                            message.AppendInt32(10);
                            message.AppendInt32(0);
                            message.AppendInt32(2);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                            message.AppendInt32(3);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                        }
                        else if (pet.PetData.HaveSaddle == 1)
                        {
                            message.AppendInt32(1);
                            message.AppendInt32(3);
                            message.AppendInt32(2);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                            message.AppendInt32(3);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                            message.AppendInt32(4);
                            message.AppendInt32(9);
                            message.AppendInt32(0);
                        }
                        else
                        {
                            message.AppendInt32(1);
                            message.AppendInt32(2);
                            message.AppendInt32(2);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                            message.AppendInt32(3);
                            message.AppendInt32(pet.PetData.PetHair);
                            message.AppendInt32(pet.PetData.HairDye);
                        }
                        message.AppendBoolean(pet.PetData.HaveSaddle != 0);
                        message.AppendBoolean(pet.montandoBol);
                        room.SendMessage(message);
                    }
                }
            }
        }

        internal void Move()
        {
            Room currentRoom = this.Session.GetHabbo().CurrentRoom;
            if (currentRoom != null)
            {
                RoomUser roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if ((roomUserByHabbo != null) && roomUserByHabbo.CanWalk)
                {
                    int pX = this.Request.PopWiredInt32();
                    int pY = this.Request.PopWiredInt32();
                    if ((pX != roomUserByHabbo.X) || (pY != roomUserByHabbo.Y))
                    {
                        roomUserByHabbo.MoveTo(pX, pY);
                    }
                }
            }
        }

        internal void MoveItem()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            uint theitem = this.Request.PopWiredUInt();
            
            if ((room != null) && room.CheckRights(this.Session))
            {
                RoomItem item = room.GetRoomItemHandler().GetItem(theitem);
                if (item != null)
                {
                    IQueryAdapter adapter;
                    if (item.wiredHandler != null)
                    {
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            item.wiredHandler.DeleteFromDatabase(adapter);
                            item.wiredHandler.Dispose();
                            room.GetWiredHandler().RemoveFurniture(item);
                        }
                        item.wiredHandler = null;
                    }
                    if (item.wiredCondition != null)
                    {
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            item.wiredCondition.DeleteFromDatabase(adapter);
                            item.wiredCondition.Dispose();
                            room.GetWiredHandler().conditionHandler.ClearTile(item.Coordinate);
                        }
                        item.wiredCondition = null;
                    }

                    int newX = this.Request.PopWiredInt32();
                    int newY = this.Request.PopWiredInt32();
                    int newRot = this.Request.PopWiredInt32();
                    int num4 = this.Request.PopWiredInt32();
                    int num5 = this.Request.PopWiredInt32();

                    bool flag = false;
                    
                    if (item.GetBaseItem().InteractionType == InteractionType.teleport)
                    {
                        flag = true;
                    }
                    
                    room.GetRoomItemHandler().SetFloorItem(this.Session, item, newX, newY, newRot, false, false, true);
                    item.UpdateState();
                    if (flag)
                    {
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            room.GetRoomItemHandler().SaveFurniture(adapter);
                        }
                    }

                }
            }
        }

        internal void MoveWallItem()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session))
            {
                uint pId = this.Request.PopWiredUInt();
                string str = this.Request.PopFixedString();
                RoomItem item = room.GetRoomItemHandler().GetItem(pId);
                if (item != null)
                {
                    try
                    {
                        WallCoordinate coordinate = new WallCoordinate(":" + str.Split(new char[] { ':' })[1]);
                        item.wallCoord = coordinate;
                    }
                    catch
                    {
                        return;
                    }
                    room.GetRoomItemHandler().UpdateItem(item);
                    ServerMessage message = new ServerMessage(Outgoing.UpdateWallItemOnRoom);
                    item.Serialize(message, (int)item.placedBy);
                    room.SendMessage(message);
                }
            }
        }

        internal void OfferTradeItem()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CanTradeInRoom)
            {
                Trade userTrade = room.GetUserTrade(this.Session.GetHabbo().Id);
                UserItem item = this.Session.GetHabbo().GetInventoryComponent().GetItem(this.Request.PopWiredUInt());
                if ((userTrade != null) && (item != null))
                {
                    userTrade.OfferItem(this.Session.GetHabbo().Id, item);
                }
            }
        }

        internal void OnRoomUserAdd()
        {
            QueuedServerMessage message = new QueuedServerMessage(this.Session.GetConnection());
            List<RoomUser> list = new List<RoomUser>();
            if (this.CurrentLoadingRoom != null)
            {
                foreach (RoomUser user in this.CurrentLoadingRoom.GetRoomUserManager().UserList.Values)
                {
                    if (!user.IsSpectator)
                    {
                        list.Add(user);
                    }
                }
                this.Response.Init(Outgoing.SetRoomUser);
                this.Response.AppendInt32(list.Count);
                foreach (RoomUser user in list)
                {
                    user.Serialize(this.Response, this.CurrentLoadingRoom.GetGameMap().gotPublicPool);
                }
                message.appendResponse(this.GetResponse());
                this.Response.Init(Outgoing.ConfigureWallandFloor);
                this.GetResponse().AppendBoolean(this.CurrentLoadingRoom.Hidewall);
                this.GetResponse().AppendInt32(this.CurrentLoadingRoom.WallThickness);
                this.GetResponse().AppendInt32(this.CurrentLoadingRoom.FloorThickness);
                message.appendResponse(this.GetResponse());
                message.appendResponse(this.GetResponse());
                if (this.CurrentLoadingRoom.Type == "public")
                {
                    this.Response.Init(Outgoing.ValidRoom);
                    this.Response.AppendBoolean(true);
                    this.Response.AppendInt32(this.CurrentLoadingRoom.RoomId);
                    this.Response.AppendBoolean(false);
                    message.appendResponse(this.GetResponse());
                }
                else if (this.CurrentLoadingRoom.Type == "private")
                {
                    this.Response.Init(Outgoing.ValidRoom);
                    this.Response.AppendBoolean(true);
                    this.Response.AppendInt32(this.CurrentLoadingRoom.RoomId);
                    this.Response.AppendBoolean(this.CurrentLoadingRoom.CheckRights(this.Session, true));
                    message.appendResponse(this.GetResponse());
                }
                this.Response.Init(Outgoing.RoomData);
                this.Response.AppendBoolean(true);
                this.Response.AppendInt32(this.CurrentLoadingRoom.RoomId);
                this.Response.AppendString(this.CurrentLoadingRoom.Name);
                this.Response.AppendBoolean(true);
                this.Response.AppendInt32(this.CurrentLoadingRoom.OwnerId);
                this.Response.AppendString(this.CurrentLoadingRoom.Owner);
                this.Response.AppendInt32(this.CurrentLoadingRoom.State);
                this.Response.AppendInt32(this.CurrentLoadingRoom.UsersNow);
                this.Response.AppendInt32(this.CurrentLoadingRoom.UsersMax);
                this.Response.AppendString(this.CurrentLoadingRoom.Description);
                this.Response.AppendInt32((this.CurrentLoadingRoom.Category == 0x34) ? 2 : 0);
                this.Response.AppendInt32(0);
                this.Response.AppendInt32(this.CurrentLoadingRoom.Score);
                this.Response.AppendInt32(0);
                this.Response.AppendInt32(this.CurrentLoadingRoom.Category);
                if (this.CurrentLoadingRoom.GroupId == 0)
                {
                    this.Response.AppendInt32(0);
                    this.Response.AppendInt32(0);
                }
                else
                {
                    MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(this.CurrentLoadingRoom.GroupId);
                    this.Response.AppendInt32(guild.Id);
                    this.Response.AppendString(guild.Name);
                    this.Response.AppendString(guild.Image);
                }
                this.Response.AppendString("");
                this.Response.AppendInt32(this.CurrentLoadingRoom.TagCount);
                foreach (string str in this.CurrentLoadingRoom.Tags)
                {
                    this.Response.AppendString(str);
                }
                this.Response.AppendInt32(0);
                this.Response.AppendInt32(0);
                this.Response.AppendInt32(0);
                this.Response.AppendBoolean(true);
                this.Response.AppendBoolean(true);
                this.Response.AppendInt32(0);
                this.Response.AppendInt32(0);
                this.Response.AppendBoolean(false);
                this.Response.AppendBoolean(false);
                this.Response.AppendBoolean(false);
                this.Response.AppendInt32(0);
                this.Response.AppendInt32(0);
                this.Response.AppendInt32(0);
                this.Response.AppendBoolean(false);
                this.Response.AppendBoolean(true);
                this.Response.AppendBoolean(true);//forse
                message.appendResponse(this.GetResponse());
                //Console.WriteLine("bb");
                if (((this.CurrentLoadingRoom.Type != "public") && (this.CurrentLoadingRoom.UsersWithRights.Count > 0)) && this.CurrentLoadingRoom.CheckRights(this.Session, true))
                {
                    Habbo habbo;
                    this.GetResponse().Init(Outgoing.GetPowerList);
                    this.GetResponse().AppendInt32(this.CurrentLoadingRoom.RoomData.Id);
                    this.GetResponse().AppendInt32(this.CurrentLoadingRoom.UsersWithRights.Count);
                    foreach (uint num in this.CurrentLoadingRoom.UsersWithRights)
                    {
                        habbo = ButterflyEnvironment.getHabboForId(num);
                        this.GetResponse().AppendInt32(habbo.Id);
                        this.GetResponse().AppendString(habbo.Username);
                    }
                    message.appendResponse(this.GetResponse());
                    foreach (uint num in this.CurrentLoadingRoom.UsersWithRights)
                    {
                        habbo = ButterflyEnvironment.getHabboForId(num);
                        this.GetResponse().Init(Outgoing.GivePowers);
                        this.GetResponse().AppendInt32(this.CurrentLoadingRoom.RoomId);
                        this.GetResponse().AppendInt32(habbo.Id);
                        this.GetResponse().AppendString(habbo.Username);
                        message.appendResponse(this.GetResponse());
                    }
                }
                ServerMessage message2 = this.CurrentLoadingRoom.GetRoomUserManager().SerializeStatusUpdates(true);
                if (message2 != null)
                {
                    message.appendResponse(message2);
                }
                foreach (RoomUser user in this.CurrentLoadingRoom.GetRoomUserManager().UserList.Values)
                {
                    if (!user.IsSpectator)
                    {
                        if (user.IsDancing)
                        {
                            this.Response.Init(Outgoing.Dance);
                            this.Response.AppendInt32(user.VirtualId);
                            this.Response.AppendInt32(user.DanceId);
                            message.appendResponse(this.GetResponse());
                        }
                        if (user.IsAsleep)
                        {
                            this.Response.Init(Outgoing.IdleStatus);
                            this.Response.AppendInt32(user.VirtualId);
                            this.Response.AppendBoolean(true);
                            message.appendResponse(this.GetResponse());
                        }
                        if ((user.CarryItemID > 0) && (user.CarryTimer > 0))
                        {
                            this.Response.Init(Outgoing.ApplyCarryItem);
                            this.Response.AppendInt32(user.VirtualId);
                            this.Response.AppendInt32(user.CarryTimer);
                            message.appendResponse(this.GetResponse());
                        }
                        if (!user.IsBot)
                        {
                            try
                            {
                                if ((((user.GetClient() != null) && (user.GetClient().GetHabbo() != null)) && (user.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent() != null)) && (user.CurrentEffect >= 1))
                                {
                                    this.Response.Init(Outgoing.ApplyEffects);
                                    this.Response.AppendInt32(user.VirtualId);
                                    this.Response.AppendInt32(user.CurrentEffect);
                                    this.Response.AppendInt32(0);
                                    message.appendResponse(this.GetResponse());
                                }
                            }
                            catch (Exception exception)
                            {
                                Logging.HandleException(exception, "Rooms.SendRoomData3");
                            }
                        }
                    }
                }
                if (this.Session.GetHabbo().QuestActivate > 0)
                {
                    QuestManager questById = ButterflyEnvironment.GetGame().GetQuestManager().GetQuestById(this.Session.GetHabbo().QuestActivate);
                    ServerMessage message3 = new ServerMessage(Outgoing.ActivateQuest);
                    message3.AppendString(questById.Category);
                    message3.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().MyQuestsCompleted(questById.Category, this.Session.GetHabbo()).Count);
                    message3.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().QuestsInCategory(questById.Category).Count);
                    message3.AppendInt32(-1);
                    message3.AppendInt32(questById.Id);
                    message3.AppendBoolean(ButterflyEnvironment.GetGame().GetQuestManager().IsActivated(questById.Id, this.Session.GetHabbo()));
                    message3.AppendString(questById.Type);
                    message3.AppendString(questById.ExtraValue);
                    message3.AppendInt32(questById.Award);
                    message3.AppendString(questById.SecondType);
                    if (this.Session.GetHabbo().QuestsStates.ContainsKey(questById.Id))
                    {
                        message3.AppendInt32(this.Session.GetHabbo().QuestsStates[questById.Id]);
                    }
                    else
                    {
                        message3.AppendInt32(0);
                    }
                    message3.AppendInt32(questById.CountFurnis);
                    message3.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().GetIntValue(questById.Category));
                    message3.AppendString("set_kuurna");
                    message3.AppendString("MAIN_CHAIN");
                    message3.AppendBoolean(true);
                    this.Session.SendMessage(message3);
                }
                message.sendResponse();
                //Console.WriteLine("cc");
                this.CurrentLoadingRoom = null;


            }
        }

        internal void OpenFlat()
        {
            if (this.Session.GetHabbo() != null)
            {
                uint roomId = this.Request.PopWiredUInt();
                string password = this.Request.PopFixedString();
                int num2 = this.Request.PopWiredInt32();
                Logging.WriteLine("Loading room [" + roomId + "]");
                RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomId);
                if ((data != null) && !(data.Type != "private"))
                {
                    this.PrepareRoomForUser(roomId, password);
                }
            }
        }

        public void OpenGuildPage()
        {
            Habbo habbo = this.Session.GetHabbo();
            
                ServerMessage message = new ServerMessage(Outgoing.SendGuildParts);
                message.AppendInt32(10);
                message.AppendInt32((int)(habbo.UsersRooms.Count - this.GetMyRoomsGuilds()));
                foreach (RoomData data in habbo.UsersRooms)
                {
                    if (data.GroupId == 0)
                    {
                        message.AppendInt32(data.Id);
                        message.AppendString(data.Name);
                        message.AppendBoolean(false);
                    }
                }
                message.AppendInt32(5);
                message.AppendInt32(10);
                message.AppendInt32(3);
                message.AppendInt32(4);
                message.AppendInt32(0x19);
                message.AppendInt32(0x11);
                message.AppendInt32(5);
                message.AppendInt32(0x19);
                message.AppendInt32(0x11);
                message.AppendInt32(3);
                message.AppendInt32(0x1d);
                message.AppendInt32(11);
                message.AppendInt32(4);
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendInt32(0);
                this.Session.SendMessage(message);
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetCatalog().mGuildPageBuy);
                this.Session.GetHabbo().ColorsSended = true;
            
        }

        internal void OpenPostit()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomItem item = room.GetRoomItemHandler().GetItem(this.Request.PopWiredUInt());
                if ((item != null) && (item.GetBaseItem().InteractionType == InteractionType.postit))
                {
                    this.Response.Init(Outgoing.OpenPostIt);
                    this.Response.AppendString(item.Id.ToString());
                    this.Response.AppendString(item.ExtraData);
                    this.SendResponse();
                }
            }
        }

        internal void OpenPresent()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                uint pId = this.Request.PopWiredUInt();
                RoomItem item = room.GetRoomItemHandler().GetItem(pId);
                if (item != null)
                {
                    IQueryAdapter adapter;
                    item.MagicRemove = true;
                    ServerMessage message = new ServerMessage(Outgoing.UpdateItemOnRoom);
                    item.Serialize(message, room.OwnerId);
                    room.SendMessage(message);
                    DataRow row = null;
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery("SELECT base_id,amount,extra_data FROM user_presents WHERE item_id = " + item.Id);
                        row = adapter.getRow();
                    }
                    if (row == null)
                    {
                        room.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                    }
                    else
                    {
                        Item item2 = ButterflyEnvironment.GetGame().GetItemManager().GetItem(Convert.ToUInt32(row["base_id"]));
                        if (item2 == null)
                        {
                            room.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                        }
                        else if (item2.Type.ToString().ToLower().Equals("s") && (item2.InteractionType != InteractionType.teleport))
                        {
                            room.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.runFastQuery(string.Concat(new object[] { "UPDATE items SET base_id = '", row["base_id"], "' WHERE item_id = ", item.Id }));
                                adapter.runFastQuery("UPDATE items_extradata SET data = '' WHERE item_id = " + item.Id);
                                adapter.runFastQuery("DELETE FROM user_presents WHERE item_id = " + item.Id);
                            }
                            string s = item.GetBaseItem().Type.ToString().ToLower();
                            string extraData = item.ExtraData;
                            item.BaseItem = Convert.ToUInt32(row["base_id"]);
                            item.refreshItem();
                            item.ExtraData = "";
                            if (!room.GetRoomItemHandler().SetFloorItem(this.Session, item, item.GetX, item.GetY, item.Rot, true, false, true))
                            {
                                this.Session.SendNotif("Failed to create your gift!");
                            }
                            else
                            {
                                this.Response.Init(Outgoing.OpenGift);
                                this.Response.AppendString(item2.Type.ToString());
                                this.Response.AppendInt32(item2.SpriteId);
                                this.Response.AppendString(item2.Name);
                                this.Response.AppendInt32(item.Id);
                                this.Response.AppendString(s);
                                this.Response.AppendBoolean(true);
                                this.Response.AppendString(extraData);
                                this.SendResponse();
                            }
                        }
                        else
                        {
                            room.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.runFastQuery("DELETE FROM user_presents WHERE item_id = " + item.Id);
                            }
                            this.Session.GetMessageHandler().GetResponse().Init(Outgoing.SendPurchaseAlert);
                            this.Session.GetMessageHandler().GetResponse().AppendInt32(1);
                            int i = 2;
                            if (item2.Type.ToString().ToLower().Equals("s"))
                            {
                                if (item2.InteractionType == InteractionType.pet)
                                {
                                    i = 3;
                                }
                                else
                                {
                                    i = 1;
                                }
                            }
                            this.Session.GetMessageHandler().GetResponse().AppendInt32(i);
                            List<UserItem> list = ButterflyEnvironment.GetGame().GetCatalog().DeliverItems(this.Session, item2, (int)row["amount"], (string)row["extra_data"], 0, 0);
                            this.Session.GetMessageHandler().GetResponse().AppendInt32(list.Count);
                            foreach (UserItem item3 in list)
                            {
                                this.Session.GetMessageHandler().GetResponse().AppendInt32(item3.Id);
                            }
                            this.Session.GetMessageHandler().SendResponse();
                            this.Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                        }
                    }
                }
            }
        }

        public void OpenQuests()
        {
            ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().LoadQuests(this.Session);
        }

        internal void PerformSearch()
        {
            if (this.Session.GetHabbo() != null)
            {
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeSearchResults(this.Request.PopFixedString()));
            }
        }

        internal void PerformSearch2()
        {
            if (this.Session.GetHabbo() != null)
            {
                int num = this.Request.PopWiredInt32();
                this.Session.SendMessage(ButterflyEnvironment.GetGame().GetNavigator().SerializeSearchResults(this.Request.PopFixedString()));
            }
        }

        internal void PetRaces()
        {
            string str = this.Request.PopFixedString();
            int sRaceId = 0;
            this.GetResponse().Init(Outgoing.PetRace);
            switch (str)
            {
                case "a0 pet0":
                    this.GetResponse().AppendString("a0 pet0");
                    sRaceId = 0;
                    break;

                case "a0 pet1":
                    this.GetResponse().AppendString("a0 pet1");
                    sRaceId = 1;
                    break;

                case "a0 pet2":
                    this.GetResponse().AppendString("a0 pet2");
                    sRaceId = 2;
                    break;

                case "a0 pet3":
                    this.GetResponse().AppendString("a0 pet3");
                    sRaceId = 3;
                    break;

                case "a0 pet4":
                    this.GetResponse().AppendString("a0 pet4");
                    sRaceId = 4;
                    break;

                case "a0 pet5":
                    this.GetResponse().AppendString("a0 pet5");
                    sRaceId = 5;
                    break;

                case "a0 pet6":
                    this.GetResponse().AppendString("a0 pet6");
                    sRaceId = 6;
                    break;

                case "a0 pet7":
                    this.GetResponse().AppendString("a0 pet7");
                    sRaceId = 7;
                    break;

                case "a0 pet8":
                    this.GetResponse().AppendString("a0 pet8");
                    sRaceId = 8;
                    break;

                case "a0 pet9":
                    this.GetResponse().AppendString("a0 pet9");
                    sRaceId = 9;
                    break;

                case "a0 pet10":
                    this.GetResponse().AppendString("a0 pet10");
                    sRaceId = 10;
                    break;

                case "a0 pet11":
                    this.GetResponse().AppendString("a0 pet11");
                    sRaceId = 11;
                    break;

                case "a0 pet12":
                    this.GetResponse().AppendString("a0 pet12");
                    sRaceId = 12;
                    break;

                case "a0 pet13":
                    this.GetResponse().AppendString("a0 pet13");
                    sRaceId = 13;
                    break;

                case "a0 pet14":
                    this.GetResponse().AppendString("a0 pet14");
                    sRaceId = 14;
                    break;

                case "a0 pet15":
                    this.GetResponse().AppendString("a0 pet15");
                    sRaceId = 15;
                    break;

                case "a0 pet16":
                    this.GetResponse().AppendString("a0 pet16");
                    sRaceId = 0x10;
                    break;

                case "a0 pet17":
                    this.GetResponse().AppendString("a0 pet17");
                    sRaceId = 0x11;
                    break;

                case "a0 pet18":
                    this.GetResponse().AppendString("a0 pet18");
                    sRaceId = 0x12;
                    break;

                case "a0 pet19":
                    this.GetResponse().AppendString("a0 pet19");
                    sRaceId = 0x13;
                    break;

                case "a0 pet20":
                    this.GetResponse().AppendString("a0 pet20");
                    sRaceId = 20;
                    break;

                case "a0 pet21":
                    this.GetResponse().AppendString("a0 pet21");
                    sRaceId = 0x15;
                    break;

                case "a0 pet22":
                    this.GetResponse().AppendString("a0 pet22");
                    sRaceId = 0x16;
                    break;
            }
            if (PetRace.RaceGotRaces(sRaceId))
            {
                List<PetRace> racesForRaceId = PetRace.GetRacesForRaceId(sRaceId);
                this.GetResponse().AppendInt32(racesForRaceId.Count);
                foreach (PetRace race in racesForRaceId)
                {
                    this.GetResponse().AppendInt32(sRaceId);
                    this.GetResponse().AppendInt32(race.Color1);
                    this.GetResponse().AppendInt32(race.Color2);
                    this.GetResponse().AppendBoolean(race.Has1Color);
                    this.GetResponse().AppendBoolean(race.Has2Color);
                }
            }
            else
            {
                this.Session.SendNotif("\x00a1Ha ocurrido un error cuando ibas a ver esta mascota, rep\x00f3rtalo a un administrador!");
                this.GetResponse().AppendInt32(0);
            }
            this.SendResponse();
        }

        internal void PickUpPet()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((((this.Session != null) && (this.Session.GetHabbo() != null)) && (this.Session.GetHabbo().GetInventoryComponent() != null)) && (((room != null) && !room.IsPublic) && (room.AllowPets || room.CheckRights(this.Session, true))))
            {
                uint petId = this.Request.PopWiredUInt();
                RoomUser pet = room.GetRoomUserManager().GetPet(petId);
                if (pet != null)
                {
                    if (pet.montandoBol)
                    {
                        RoomUser roomUserByVirtualId = room.GetRoomUserManager().GetRoomUserByVirtualId(Convert.ToInt32(pet.montandoID));
                        if (roomUserByVirtualId != null)
                        {
                            roomUserByVirtualId.montandoBol = false;
                            roomUserByVirtualId.ApplyEffect(-1);
                            roomUserByVirtualId.MoveTo(new Point(roomUserByVirtualId.X + 1, roomUserByVirtualId.Y + 1));
                        }
                    }
                    if (pet.PetData.DBState != DatabaseUpdateState.NeedsInsert)
                    {
                        pet.PetData.DBState = DatabaseUpdateState.NeedsUpdate;
                    }
                    pet.PetData.RoomId = 0;
                    this.Session.GetHabbo().GetInventoryComponent().AddPet(pet.PetData);
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        room.GetRoomUserManager().SavePets(adapter);
                    }
                    room.GetRoomUserManager().RemoveBot(pet.VirtualId, false);
                    this.Session.SendMessage(this.Session.GetHabbo().GetInventoryComponent().SerializePetInventory());
                }
            }
        }

        internal void Ping()
        {
            Habbo habbo = this.Session.GetHabbo();
            habbo.PointsOnline++;
            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(this.Session, "ACH_AllTimeHotelPresence", 1);
            if ((this.Session.GetHabbo().SnowWar != null) && (this.Session.GetHabbo().SnowStep == 2))
            {
                this.Session.GetHabbo().SnowPing = true;
            }
        }

        internal void PlaceItem()
        {
            Room pRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            
            if ((pRoom != null) && (pRoom.CheckRights(this.Session)))
            {
                string str = this.Request.PopFixedString();
                string[] strArray = str.Split(new char[] { ' ' });
                uint id = uint.Parse(strArray[0].Replace("-", ""));
                UserItem item = this.Session.GetHabbo().GetInventoryComponent().GetItem(id);
                if (item != null)
                {
                    RoomItem item2;
                    if (item.GetBaseItem().InteractionType == InteractionType.dimmer)
                    {
                        MoodlightData moodlightData = pRoom.MoodlightData;
                        if ((moodlightData != null) && (pRoom.GetRoomItemHandler().GetItem(moodlightData.ItemId) != null))
                        {
                            this.Session.SendNotif(LanguageLocale.GetValue("user.maxmoodlightsreached"));
                        }
                    }

                    if (strArray[1].StartsWith(":"))
                    {
                        try
                        {
                            WallCoordinate wallCoord = new WallCoordinate(":" + str.Split(new char[] { ':' })[1]);
                            item2 = new RoomItem(item.Id, pRoom.RoomId, item.BaseItem, item.ExtraData, wallCoord, pRoom, item.Group_data, item.RareId, item.placedBy);
                            if (pRoom.GetRoomItemHandler().SetWallItem(this.Session, item2))
                            {
                                this.Session.GetHabbo().GetInventoryComponent().RemoveItem(id, true);
                            }
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            int x = int.Parse(strArray[1]);
                            int y = int.Parse(strArray[2]);
                            int rot = int.Parse(strArray[3]);
                            if (this.Session.GetHabbo().forceRot > -1)
                            {
                                rot = this.Session.GetHabbo().forceRot;
                            }
                            item2 = new RoomItem(item.Id, pRoom.RoomId, item.BaseItem, item.ExtraData, x, y, 0.0, rot, pRoom, item.Group_data, item.RareId, item.placedBy);
                            if (pRoom.GetRoomItemHandler().SetFloorItem(this.Session, item2, x, y, rot, true, false, true))
                            {
                                this.Session.GetHabbo().GetInventoryComponent().RemoveItem(id, true);
                            }
                            if (WiredUtillity.TypeIsWired(item.GetBaseItem().InteractionType))
                            {
                                WiredSaver.HandleDefaultSave(item.Id, pRoom);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.LogCriticalException(ex.ToString());
                        }
                    }
                }
            }
        }

        internal void PlacePet()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && (room.AllowPets || room.CheckRights(this.Session, true)))
            {
                uint id = this.Request.PopWiredUInt();
                Pet petData = this.Session.GetHabbo().GetInventoryComponent().GetPet(id);
                if ((petData != null) && !petData.PlacedInRoom)
                {
                    int x = this.Request.PopWiredInt32();
                    int y = this.Request.PopWiredInt32();
                    if (room.GetGameMap().CanWalk(x, y, false))
                    {
                        RoomUser pet = room.GetRoomUserManager().GetPet(id);
                        if (pet != null)
                        {
                            room.GetRoomUserManager().RemoveBot(pet.VirtualId, false);
                        }
                        petData.PlacedInRoom = true;
                        petData.RoomId = room.RoomId;
                        List<RandomSpeech> speeches = new List<RandomSpeech>();
                        List<BotResponse> responses = new List<BotResponse>();
                        RoomUser user2 = room.GetRoomUserManager().DeployBot(new RoomBot(petData.PetId, petData.RoomId, AIType.Pet, "freeroam", petData.Name, "", petData.Look, x, y, 0, 0, 0, 0, 0, 0, ref speeches, ref responses, 0), petData);
                        this.Session.GetHabbo().GetInventoryComponent().MovePetToRoom(petData.PetId);
                        if (petData.DBState != DatabaseUpdateState.NeedsInsert)
                        {
                            petData.DBState = DatabaseUpdateState.NeedsUpdate;
                        }
                        using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            room.GetRoomUserManager().SavePets(adapter);
                        }
                        this.Session.SendMessage(this.Session.GetHabbo().GetInventoryComponent().SerializePetInventory());
                    }
                }
            }
        }

        internal void PlacePostIt()
        {
            Room pRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((pRoom != null) && pRoom.CheckRights(this.Session))
            {
                uint id = this.Request.PopWiredUInt();
                string str = this.Request.PopFixedString();
                UserItem item = this.Session.GetHabbo().GetInventoryComponent().GetItem(id);
                if ((item != null) && (pRoom != null))
                {
                    try
                    {
                        WallCoordinate wallCoord = new WallCoordinate(":" + str.Split(new char[] { ':' })[1]);
                        RoomItem item2 = new RoomItem(item.Id, pRoom.RoomId, item.BaseItem, item.ExtraData, wallCoord, pRoom, item.Group_data, item.RareId, item.placedBy);
                        if (pRoom.GetRoomItemHandler().SetWallItem(this.Session, item2))
                        {
                            this.Session.GetHabbo().GetInventoryComponent().RemoveItem(id, true);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error while placing a post-it");
                    }
                }
            }
        }

        internal void Pong()
        {
            this.Session.TimePingedReceived = DateTime.Now;
        }

        public void PopularGuilds()
        {
            ServerMessage message = new ServerMessage(Outgoing.NavigatorPacket);
            message.AppendInt32(14);
            message.AppendString("");
            KeyValuePair<RoomData, int>[] guildRooms = ButterflyEnvironment.GetGame().GetRoomManager().GetGuildRooms();
            if (guildRooms != null)
            {
                message.AppendInt32(guildRooms.Length);
                for (int i = 0; i < guildRooms.Length; i++)
                {
                    guildRooms[i].Key.Serialize(message, false);
                }
            }
            else
            {
                message.AppendInt32(0);
            }
            message.AppendBoolean(false);
            this.Session.SendMessage(message);
        }

        internal void PrepareRoomForUser(uint Id, string Password)
        {
            this.ClearRoomLoading();
            QueuedServerMessage message = new QueuedServerMessage(this.Session.GetConnection());
            if (ButterflyEnvironment.ShutdownStarted)
            {
                this.Session.SendNotif(LanguageLocale.GetValue("shutdown.alert"));
            }
            else
            {
                if (this.Session.GetHabbo().InRoom)
                {
                    Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
                    if (room != null)
                    {
                        room.GetRoomUserManager().RemoveUserFromRoom(this.Session, false, false);
                        this.Session.CurrentRoomUserID = -1;
                    }
                }
                Room room2 = ButterflyEnvironment.GetGame().GetRoomManager().LoadRoom(Id);
                if (((room2.UserCount + 1) >= room2.UsersMax) && (this.Session.GetHabbo().Rank < 5))
                {
                    ServerMessage message2 = new ServerMessage(Outgoing.RoomErrorToEnter);
                    message2.AppendInt32(1);
                    this.Session.SendMessage(message2);
                    ServerMessage message3 = new ServerMessage(Outgoing.OutOfRoom);
                    this.Session.SendMessage(message3);
                }
                else if ((((room2 != null) && (this.Session != null)) && (this.Session.GetHabbo() != null)) && (!this.Session.GetHabbo().IsTeleporting || (this.Session.GetHabbo().TeleportingRoomID == Id)))
                {
                    this.Session.GetHabbo().LoadingRoom = Id;
                    this.CurrentLoadingRoom = room2;
                    if (!this.Session.GetHabbo().HasFuse("fuse_enter_any_room") && room2.UserIsBanned(this.Session.GetHabbo().Id))
                    {
                        if (room2.HasBanExpired(this.Session.GetHabbo().Id))
                        {
                            room2.RemoveBan(this.Session.GetHabbo().Id);
                        }
                        else
                        {
                            this.Response.Init(Outgoing.RoomErrorToEnter);
                            this.Response.AppendInt32(4);
                            message.appendResponse(this.GetResponse());
                            this.Response.Init(Outgoing.OutOfRoom);
                            message.appendResponse(this.GetResponse());
                            message.sendResponse();
                            return;
                        }
                    }
                    if (((room2.UsersNow >= room2.UsersMax) && !this.Session.GetHabbo().HasFuse("fuse_enter_full_rooms")) && !ButterflyEnvironment.GetGame().GetRoleManager().RankHasRight(this.Session.GetHabbo().Rank, "fuse_enter_full_rooms"))
                    {
                        this.Response.Init(Outgoing.RoomErrorToEnter);
                        this.Response.AppendInt32(1);
                        message.appendResponse(this.GetResponse());
                        this.Response.Init(Outgoing.OutOfRoom);
                        message.appendResponse(this.GetResponse());
                        message.sendResponse();
                    }
                    else
                    {
                        if (room2.Type == "private")
                        {
                            this.Response.Init(Outgoing.PrepareRoomForUsers);
                            message.appendResponse(this.GetResponse());
                            if ((!this.Session.GetHabbo().HasFuse("fuse_enter_any_room") && !room2.CheckRights(this.Session, true)) && !this.Session.GetHabbo().IsTeleporting)
                            {
                                if (room2.State == 1)
                                {
                                    if (room2.UserCount == 0)
                                    {
                                        this.Response.Init(Outgoing.DoorBellNoPerson);
                                        message.appendResponse(this.GetResponse());
                                    }
                                    else
                                    {
                                        this.Response.Init(Outgoing.Doorbell);
                                        this.Response.AppendString("");
                                        message.appendResponse(this.GetResponse());
                                        ServerMessage message4 = new ServerMessage(Outgoing.Doorbell);
                                        message4.AppendString(this.Session.GetHabbo().Username);
                                        room2.SendMessageToUsersWithRights(message4);
                                    }
                                    message.sendResponse();
                                    return;
                                }
                                if ((room2.State == 2) && (Password.ToLower() != room2.Password.ToLower()))
                                {
                                    this.Response.Init(Outgoing.RoomError);
                                    this.Response.AppendInt32(-100002);
                                    message.appendResponse(this.GetResponse());
                                    this.Response.Init(Outgoing.OutOfRoom);
                                    message.appendResponse(this.GetResponse());
                                    message.sendResponse();
                                    return;
                                }
                            }
                        }
                        this.Session.GetHabbo().LoadingChecksPassed = true;
                        message.addBytes(this.LoadRoomForUser().getPacket);
                        message.sendResponse();
                    }
                }
                
            }
        }



        internal void PurchaseGift()
        {
            int pageId = this.Request.PopWiredInt32();
            uint itemId = this.Request.PopWiredUInt();
            string extraData = this.Request.PopFixedString();
            string giftUser = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
            string giftMessage = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
            int giftSpriteId = this.Request.PopWiredInt32();
            int giftLazo = this.Request.PopWiredInt32();
            int giftColor = this.Request.PopWiredInt32();
            bool undef = this.Request.PopWiredBoolean();
            Console.WriteLine(pageId);
            Console.WriteLine(itemId);
            Console.WriteLine(extraData);
            Console.WriteLine(giftUser);
            Console.WriteLine(giftMessage);
            Console.WriteLine(giftSpriteId);
            Console.WriteLine(giftLazo);
            Console.WriteLine(giftColor);
            Console.WriteLine(undef);
            ButterflyEnvironment.GetGame().GetCatalog().HandlePurchase(this.Session, pageId, itemId, extraData, 1, true, giftUser, giftMessage, giftSpriteId, giftLazo, giftColor, undef);
        }

        internal void SerializeRateRoom(ref Room room)
        {
            ButterflyEnvironment.GetGame().GetRoomManager().QueueVoteAdd(room.RoomData);
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "UPDATE rooms SET score = ", room.Score, " WHERE id = ", room.RoomId }));
            }
            this.Session.GetHabbo().RatedRooms.Add(room.RoomId);
            this.Response.Init(Outgoing.ScoreMeter);
            this.Response.AppendInt32(room.Score);
            this.SendResponseWithOwnerParam();
        }
        internal void RateRoom()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (((room != null) && !this.Session.GetHabbo().RatedRooms.Contains(room.RoomId)) && !room.CheckRights(this.Session, true))
            {
                switch (this.Request.PopWiredInt32())
                {
                    case -1:
                        {
                            room.Score--;
                            RoomData roomData = room.RoomData;
                            roomData.Score--;
                            this.SerializeRateRoom(ref room);
                            break;
                        }
                    case 1:
                        {
                            room.Score++;
                            RoomData data2 = room.RoomData;
                            data2.Score++;
                            this.SerializeRateRoom(ref room);
                            break;
                        }
                    default:
                        return;
                }
            }
        }

        internal void RecycleItems()
        {
            if (this.Session.GetHabbo().InRoom)
            {
                int num = this.Request.PopWiredInt32();
                if (num > 0)
                {
                    for (int i = 0; i < num; i++)
                    {
                        UserItem item = this.Session.GetHabbo().GetInventoryComponent().GetItem(this.Request.PopWiredUInt());
                        if ((item != null) && item.GetBaseItem().AllowRecycle)
                        {
                            this.Session.GetHabbo().GetInventoryComponent().RemoveItem(item.Id, false);
                        }
                        else
                        {
                            return;
                        }
                    }
                    EcotronReward randomEcotronReward = ButterflyEnvironment.GetGame().GetCatalog().GetRandomEcotronReward();
                    IQueryAdapter adapter;
                    uint num10;
                    uint base_id = 4692;
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery("SELECT item_id FROM items_base WHERE item_name = 'matic_box' LIMIT 1");
                        base_id = Convert.ToUInt32(adapter.getInteger());
                        if (adapter.dbType == DatabaseType.MSSQL)
                        {
                            adapter.setQuery(string.Concat(new object[] { "INSERT INTO items (base_id, rareid,placedBy) OUTPUT INSERTED.* VALUES ('",base_id,"','','",this.Session.GetHabbo().Id,"')" }));
                        }
                        else
                        {
                            adapter.setQuery(string.Concat(new object[] { "INSERT INTO items (base_id, rareid, placedBy) VALUES ('", base_id, "','0','", this.Session.GetHabbo().Id, "')" }));
                        }
                        num10 = (uint)adapter.insertQuery();
                        adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO items_users VALUES (", num10, ",", Session.GetHabbo().Id, ")" }));

                        adapter.setQuery("INSERT INTO items_extradata VALUES (" + num10 + ",@timestamp)");
                        adapter.addParameter("timestamp", DateTime.Now.ToLongDateString());

                        adapter.runQuery();

                        adapter.setQuery(string.Concat(new object[] { "INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES (", num10, ",", randomEcotronReward.BaseId, ",'1','')" }));
                        adapter.runQuery();
                    }
                    UserItem item3 = Session.GetHabbo().GetInventoryComponent().AddNewItem(num10, base_id, DateTime.Now.ToLongDateString(), false, false, 0, "", 0);
                    Session.GetHabbo().GetInventoryComponent().SendFloorInventoryUpdate();

                    Session.GetMessageHandler().GetResponse().Init(Outgoing.SendPurchaseAlert);
                    Session.GetMessageHandler().GetResponse().AppendInt32(1);
                    Session.GetMessageHandler().GetResponse().AppendInt32(1);
                    Session.GetMessageHandler().GetResponse().AppendInt32(1);
                    Session.GetMessageHandler().GetResponse().AppendInt32(item3.Id);
                    Session.GetMessageHandler().SendResponse();

                    Session.GetHabbo().GetInventoryComponent().RunDBUpdate();

                    this.Response.Init(Outgoing.RecycleState);
                    this.Response.AppendInt32(1);
                    this.Response.AppendInt32(item3.Id);
                    this.SendResponse();
                }
            }
        }
        internal void GetRecyclerStatus()
        {
            GetResponse().Init(Outgoing.Recycle);
            GetResponse().AppendInt32(1);
            GetResponse().AppendInt32(0);
            SendResponse();
        }
        internal void RedeemExchangeFurni()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                RoomItem item = room.GetRoomItemHandler().GetItem(this.Request.PopWiredUInt());
                if ((item != null) && (item.GetBaseItem().Name.StartsWith("CF_") || item.GetBaseItem().Name.StartsWith("CFC_")))
                {
                    int num = int.Parse(item.GetBaseItem().Name.Split(new char[] { '_' })[1]);
                    if (num > 0)
                    {
                        Habbo habbo = this.Session.GetHabbo();
                        habbo.Credits += num;
                        this.Session.GetHabbo().UpdateCreditsBalance();
                    }
                    room.GetRoomItemHandler().RemoveFurniture(null, item.Id);
                    this.Response.Init(Outgoing.UpdateInventary);
                    this.SendResponse();
                }
            }
        }

        internal void RedeemVoucher()
        {
            VoucherHandler.TryRedeemVoucher(this.Session, this.Request.PopFixedString());
        }

        internal void RemoveBuddy()
        {
            if (this.Session.GetHabbo().GetMessenger() != null)
            {
                int num = this.Request.PopWiredInt32();
                for (int i = 0; i < num; i++)
                {
                    this.Session.GetHabbo().GetMessenger().DestroyFriendship(this.Request.PopWiredUInt());
                }
            }
        }

        public void RemoveCdToJuke()
        {
            if (((this.Session != null) && (this.Session.GetHabbo() != null)) && (this.Session.GetHabbo().CurrentRoom != null))
            {
                Room currentRoom = this.Session.GetHabbo().CurrentRoom;
                if (currentRoom.CheckRights(this.Session, true) && currentRoom.GotMusicController())
                {
                    RoomMusicController roomMusicController = currentRoom.GetRoomMusicController();
                    SongItem item = roomMusicController.RemoveDisk(this.Request.PopWiredInt32());
                    if (item != null)
                    {
                        item.RemoveFromDatabase();
                        this.Session.GetHabbo().GetInventoryComponent().AddNewItem((uint)item.itemID, item.baseItem.ItemId, item.songID.ToString(), true, true, 0, "",Session.GetHabbo().Id, (uint)item.songID);
                        this.Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                        this.Session.SendMessage(JukeboxDiscksComposer.SerializeSongInventory(this.Session.GetHabbo().GetInventoryComponent().songDisks));
                        this.Session.SendMessage(JukeboxDiscksComposer.Compose(roomMusicController.PlaylistCapacity, roomMusicController.Playlist.Values.ToList<SongInstance>()));
                    }
                }
            }
        }

        internal void RemoveFavorite()
        {
            if (this.Session.GetHabbo() != null)
            {
                uint num = this.Request.PopWiredUInt();
                this.Session.GetHabbo().FavoriteRooms.Remove(num);
                this.GetResponse().Init(Outgoing.FavsUpdate);
                this.GetResponse().AppendInt32(num);
                this.GetResponse().AppendBoolean(false);
                this.SendResponse();
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "DELETE FROM user_favorites WHERE user_id = ", this.Session.GetHabbo().Id, " AND room_id = ", num }));
                }
            }
        }



        internal void RemoveGuildFavorite()
        {
            this.Session.GetHabbo().FavoriteGuild = 0;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE users SET FavoriteGroup = '0' WHERE Id = '" + this.Session.GetHabbo().Id + "'");
            }
            ServerMessage message = new ServerMessage(Outgoing.UpdatePetitionsGuild);
            message.AppendInt32(0);
            message.AppendInt32(-1);
            message.AppendInt32(-1);
            message.AppendString("");
            if (this.Session.GetHabbo().CurrentRoomId > 0)
            {
                ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId).SendMessage(message);
            }
            else
            {
                this.Session.SendMessage(message);
            }
            ServerMessage message2 = new ServerMessage(Outgoing.RemoveGuildFavorite);
            message2.AppendInt32((int)this.Session.GetHabbo().Id);
            this.Session.SendMessage(message2);
        }

        internal void RemoveHanditem()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if ((roomUserByHabbo != null) && ((roomUserByHabbo.CarryItemID > 0) && (roomUserByHabbo.CarryTimer > 0)))
                {
                    roomUserByHabbo.CarryItem(0);
                }
            }
        }

        internal void RemoveSaddle()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (((room != null) && !room.IsPublic) && (room.AllowPets || room.CheckRights(this.Session, true)))
            {
                uint petId = this.Request.PopWiredUInt();
                RoomUser pet = room.GetRoomUserManager().GetPet(petId);
                if (((pet != null) && (pet.PetData != null)) && (pet.PetData.OwnerId == this.Session.GetHabbo().Id))
                {
                    if (pet.PetData.HaveSaddle == 2)
                    {
                        ButterflyEnvironment.GetGame().GetCatalog().DeliverItems(this.Session, ButterflyEnvironment.GetGame().GetItemManager().GetItem(0xfa1), 1, "", 0, 0);
                    }
                    else if (pet.PetData.HaveSaddle == 1)
                    {
                        ButterflyEnvironment.GetGame().GetCatalog().DeliverItems(this.Session, ButterflyEnvironment.GetGame().GetItemManager().GetItem(0xaf4), 1, "", 0, 0);
                    }
                    pet.PetData.HaveSaddle = 0;
                    this.Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery("UPDATE user_pets SET have_saddle = 0 WHERE id = " + pet.PetData.PetId);
                    }
                    ServerMessage message = new ServerMessage(Outgoing.SerializeMontura);
                    message.AppendInt32(pet.PetData.VirtualId);
                    message.AppendInt32(pet.PetData.PetId);
                    message.AppendInt32(pet.PetData.Type);
                    message.AppendInt32(int.Parse(pet.PetData.Race));
                    message.AppendString(pet.PetData.Color.ToLower());
                    message.AppendInt32(1);
                    message.AppendInt32(2);
                    message.AppendInt32(2);
                    message.AppendInt32(pet.PetData.PetHair);
                    message.AppendInt32(pet.PetData.HairDye);
                    message.AppendInt32(3);
                    message.AppendInt32(pet.PetData.PetHair);
                    message.AppendInt32(pet.PetData.HairDye);
                    message.AppendBoolean(pet.PetData.HaveSaddle != 0);
                    message.AppendBoolean(pet.montandoBol);
                    room.SendMessage(message);
                }
            }
        }

        internal void ReqLoadRoomForUser()
        {
            this.LoadRoomForUser().sendResponse();
        }

        internal void RequestBuddy()
        {
            if ((this.Session.GetHabbo().GetMessenger() != null) && this.Session.GetHabbo().GetMessenger().RequestBuddy(this.Request.PopFixedString()))
            {
                ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("REQUEST_FRIEND", this.Session);
            }
        }

        internal void RespectPet()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (((room != null) && !room.IsPublic) && (room.AllowPets || room.CheckRights(this.Session, true)))
            {
                uint petId = this.Request.PopWiredUInt();
                RoomUser pet = room.GetRoomUserManager().GetPet(petId);
                if (((pet != null) && (pet.PetData != null)) && (pet.PetData.OwnerId == this.Session.GetHabbo().Id))
                {
                    pet.PetData.OnRespect();
                    Habbo habbo = this.Session.GetHabbo();
                    habbo.DailyPetRespectPoints--;
                    ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(this.Session, "ACH_PetRespectGiver", 1);
                    ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(pet.PetData.OwnerId), "ACH_PetRespectReceiver", 1);
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery("UPDATE users SET daily_pet_respect_points = daily_pet_respect_points - 1 WHERE id = " + this.Session.GetHabbo().Id);
                    }
                }
            }
        }

        internal void SaveMannequin()
        {
            try
            {
                Room Room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);


                if (Room == null)
                {
                    return;
                }


                int ID = Request.PopWiredInt32();


                RoomItem Item = Room.GetRoomItemHandler().GetItem(Convert.ToUInt32(ID));


                if (Item == null || Item.GetBaseItem().InteractionType != Butterfly.HabboHotel.Items.InteractionType.mannequin)
                {
                    return;
                }

                Item.ExtraData = Item.ExtraData.Split(Convert.ToChar(9))[0] + Convert.ToChar(9) + Session.GetHabbo().Gender + Convert.ToChar(9) + Session.GetHabbo().Look;

                ServerMessage Message = new ServerMessage(0);
                Message.Init(Outgoing.UpdateFloorItemExtraData);
                Message.AppendString(Item.Id.ToString());
                Message.AppendInt32(1);
                Message.AppendInt32(3);
                Message.AppendString("GENDER");
                Message.AppendString(Item.ExtraData.Split(Convert.ToChar(9))[1]);
                Message.AppendString("FIGURE");
                Message.AppendString(Item.ExtraData.Split(Convert.ToChar(9))[2]);
                Message.AppendString("OUTFIT_NAME");
                Message.AppendString(Item.ExtraData.Split(Convert.ToChar(9))[0]);
                Room.SendMessage(Message);
            }
            catch
            {
            }
        }


        internal void SaveMannequinName()
        {
            try
            {
                Room Room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);


                if (Room == null)
                {
                    return;
                }


                uint ID = Request.PopWiredUInt();


                RoomItem Item = Room.GetRoomItemHandler().GetItem(ID);


                if (Item == null || Item.GetBaseItem().InteractionType != Butterfly.HabboHotel.Items.InteractionType.mannequin)
                {
                    return;
                }


                string Name = Request.PopFixedString();


                Item.ExtraData = Name + Convert.ToChar(9) + Item.ExtraData.Split(Convert.ToChar(9))[1] + Convert.ToChar(9) + Item.ExtraData.Split(Convert.ToChar(9))[2];


                ServerMessage Message = new ServerMessage(0);
                Message.Init(Outgoing.UpdateFloorItemExtraData);
                Message.AppendString(Item.Id.ToString());
                Message.AppendInt32(1);
                Message.AppendInt32(3);
                Message.AppendString("GENDER");
                Message.AppendString(Item.ExtraData.Split(Convert.ToChar(9))[1]);
                Message.AppendString("FIGURE");
                Message.AppendString(Item.ExtraData.Split(Convert.ToChar(9))[2]);
                Message.AppendString("OUTFIT_NAME");
                Message.AppendString(Item.ExtraData.Split(Convert.ToChar(9))[0]);
                Room.SendMessage(Message);
            }
            catch
            {
            }
        }

        internal string GetHair(string _Figure)
        {
            string FigurePartHair = _Figure;
            string GetHairPart;

            GetHairPart = System.Text.RegularExpressions.Regex.Split(_Figure, "hr")[1];
            FigurePartHair = GetHairPart.Split('.')[0];
            string FigurePartBody = _Figure;
            string GetBodyPart;

            GetBodyPart = System.Text.RegularExpressions.Regex.Split(_Figure, "hd")[1];
            FigurePartBody = GetBodyPart.Split('.')[0];

            string _Uni = Convert.ToString("hr" + FigurePartHair + "." + "hd" + FigurePartBody + ".");

            return _Uni;
        }

        public void SaveNameToChange()
        {
            string s = this.Request.PopFixedString();
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                string username = this.Session.GetHabbo().Username;
                this.Session.GetHabbo().Username = s;
                this.Session.GetHabbo().CanChangeName = false;
                foreach (RoomData data in this.Session.GetHabbo().UsersRooms)
                {
                    data.Owner = s;
                }
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET username = '", s, "', canchangename = '0' WHERE id = '", this.Session.GetHabbo().Id, "'" }));
                    adapter.runFastQuery("UPDATE rooms SET owner = '" + s + "' WHERE owner = '" + username + "'");
                }
                ServerMessage message = new ServerMessage(Outgoing.GetName);
                message.AppendInt32(0);
                message.AppendString(s);
                message.AppendInt32(0);
                this.Session.SendMessage(message);
                ServerMessage message2 = new ServerMessage(Outgoing.ChangePublicName);
                message2.AppendInt32(this.Session.GetHabbo().Id);
                message2.AppendInt32(this.Session.CurrentRoomUserID);
                message2.AppendString(s);
                room.SendMessage(message2);
                if (this.Session.GetHabbo().Id == room.OwnerId)
                {
                    ServerMessage message3 = new ServerMessage(Outgoing.UpdateRoom);
                    message3.AppendInt32(room.RoomId);
                    room.SendMessage(message3);
                }
                ServerMessage message4 = new ServerMessage(Outgoing.UpdateUserInformation);
                message4.AppendInt32(-1);
                message4.AppendString(this.Session.GetHabbo().Look);
                message4.AppendString(this.Session.GetHabbo().Gender.ToLower());
                message4.AppendString(this.Session.GetHabbo().Motto);
                message4.AppendInt32(this.Session.GetHabbo().AchievementPoints);
                this.Session.SendMessage(message4);
                ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(this.Session, "ACH_Name", 1);
                if (this.Session.GetHabbo().InRoom)
                {
                    ServerMessage message5 = new ServerMessage(Outgoing.UpdateUserInformation);
                    message5.AppendInt32(room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id).VirtualId);
                    message5.AppendString(this.Session.GetHabbo().Look);
                    message5.AppendString(this.Session.GetHabbo().Gender.ToLower());
                    message5.AppendString(this.Session.GetHabbo().Motto);
                    message5.AppendInt32(this.Session.GetHabbo().AchievementPoints);
                    room.SendMessage(message5);
                }
                ServerMessage message6 = new ServerMessage(Outgoing.RoomData);
                message6.AppendBoolean(true);
                message6.AppendInt32(room.RoomId);
                message6.AppendString(room.Name);
                message6.AppendBoolean(true);
                message6.AppendInt32(room.OwnerId);
                message6.AppendString(room.Owner);
                message6.AppendInt32(room.State);
                message6.AppendInt32(room.UsersNow);
                message6.AppendInt32(room.UsersMax);
                message6.AppendString(room.Description);
                message6.AppendInt32(0);
                message6.AppendInt32((room.Category == 0x34) ? 2 : 0);
                message6.AppendInt32(room.Score);
                message6.AppendInt32(0);
                message6.AppendInt32(room.Category);
                if (room.GetRoomGuild() == null)
                {
                    message6.AppendInt32(0);
                    message6.AppendInt32(0);
                }
                else
                {
                    message6.AppendInt32(room.GetRoomGuild().Id);
                    message6.AppendString(room.GetRoomGuild().Name);
                    message6.AppendString(room.GetRoomGuild().Image);
                }
                message6.AppendString("");
                message6.AppendInt32(room.TagCount);
                foreach (string str3 in room.Tags.ToArray())
                {
                    message6.AppendString(str3);
                }
                message6.AppendInt32(0);
                message6.AppendInt32(0);
                message6.AppendInt32(0);
                message6.AppendBoolean(true);
                message6.AppendBoolean(true);
                message6.AppendInt32(0);
                message6.AppendInt32(0);
                message6.AppendBoolean(false);
                message6.AppendBoolean(false);
                message6.AppendBoolean(false);
                message6.AppendInt32(0);
                message6.AppendInt32(0);
                message6.AppendInt32(0);
                message6.AppendBoolean(false);
                message6.AppendBoolean(true);
                room.SendMessage(message6);
            }
        }

        internal void SavePostit()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomItem item = room.GetRoomItemHandler().GetItem(this.Request.PopWiredUInt());
                if ((item != null) && (item.GetBaseItem().InteractionType == InteractionType.postit))
                {
                    string str = this.Request.PopFixedString();
                    string str2 = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString(), true);
                    if (room.CheckRights(this.Session) || str2.StartsWith(item.ExtraData))
                    {
                        string str3 = str;
                        if ((str3 != null) && ((((str3 == "FFFF33") || (str3 == "FF9CFF")) || (str3 == "9CCEFF")) || (str3 == "9CFF9C")))
                        {
                            item.ExtraData = str + " " + str2;
                            item.UpdateState(true, true);
                        }
                    }
                }
            }
        }

        internal void SaveRoomData()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            RoomData roomData = room.RoomData;
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                int num = this.Request.PopWiredInt32();
                string str = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
                string str2 = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
                int num2 = this.Request.PopWiredInt32();
                string str3 = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
                int num3 = this.Request.PopWiredInt32();
                int id = this.Request.PopWiredInt32();
                int num5 = this.Request.PopWiredInt32();
                List<string> tags = new List<string>();
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < num5; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(",");
                    }
                    string item = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString().ToLower());
                    tags.Add(item);
                    builder.Append(item);
                }
                int num7 = this.Request.PopWiredInt32();
                bool k = this.Request.PopWiredBoolean();
                bool flag2 = this.Request.PopWiredBoolean();
                bool flag3 = this.Request.PopWiredBoolean();
                bool flag4 = this.Request.PopWiredBoolean();
                int num8 = this.Request.PopWiredInt32();
                int num9 = this.Request.PopWiredInt32();
                if ((num8 < -2) || (num8 > 1))
                {
                    num8 = 0;
                }
                if ((num9 < -2) || (num9 > 1))
                {
                    num9 = 0;
                }
                if (((str.Length >= 1) && ((num2 >= 0) && (num2 <= 2))) && ((num3 >= 10) && (num3 <= 50)))
                {
                    FlatCat flatCat = ButterflyEnvironment.GetGame().GetNavigator().GetFlatCat(id);
                    if (flatCat != null)
                    {
                        if (flatCat.MinRank > this.Session.GetHabbo().Rank)
                        {
                            this.Session.SendNotif(LanguageLocale.GetValue("user.roomdata.rightserror"));
                            id = 0;
                        }
                        if (num5 <= 2)
                        {
                            room.AllowPets = k;
                            room.AllowPetsEating = flag2;
                            room.AllowWalkthrough = flag3;
                            room.Hidewall = flag4;
                            room.RoomData.AllowPets = k;
                            room.RoomData.AllowPetsEating = flag2;
                            room.RoomData.AllowWalkthrough = flag3;
                            room.RoomData.Hidewall = flag4;
                            room.Name = str;
                            room.State = num2;
                            room.Description = str2;
                            room.Category = id;
                            room.Password = str3;
                            room.RoomData.Name = str;
                            room.RoomData.State = num2;
                            room.RoomData.Description = str2;
                            room.RoomData.Category = id;
                            room.RoomData.Password = str3;
                            room.ClearTags();
                            room.AddTagRange(tags);
                            room.UsersMax = num3;
                            room.RoomData.Tags.Clear();
                            room.RoomData.Tags.AddRange(tags);
                            room.RoomData.UsersMax = num3;
                            room.WallThickness = num8;
                            room.FloorThickness = num9;
                            room.RoomData.WallThickness = num8;
                            room.RoomData.FloorThickness = num9;
                            string str5 = "open";
                            if (room.State == 1)
                            {
                                str5 = "locked";
                            }
                            else if (room.State > 1)
                            {
                                str5 = "password";
                            }
                            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.setQuery(string.Concat(new object[] { 
                                    "UPDATE rooms SET caption = @caption, description = @description, password = @password, category = ", id, ", state = '", str5, "', tags = @tags, users_max = ", num3, ", allow_pets = ", TextHandling.BooleanToInt(k), ", allow_pets_eat = ", TextHandling.BooleanToInt(flag2), ", allow_walkthrough = ", TextHandling.BooleanToInt(flag3), ", allow_hidewall = ", TextHandling.BooleanToInt(room.Hidewall), ", floorthickness = ", room.FloorThickness, 
                                    ", wallthickness = ", room.WallThickness, " WHERE id = ", room.RoomId
                                 }));
                                adapter.addParameter("caption", room.Name);
                                adapter.addParameter("description", room.Description);
                                adapter.addParameter("password", room.Password);
                                adapter.addParameter("tags", builder.ToString());
                                adapter.runQuery();
                            }
                            
                            this.GetResponse().Init(Outgoing.UpdateRoomOne);
                            this.GetResponse().AppendInt32(room.RoomId);
                            
                            this.SendResponse();
                            
                            this.GetResponse().Init(Outgoing.ConfigureWallandFloor);
                            this.GetResponse().AppendBoolean(room.Hidewall);
                            this.GetResponse().AppendInt32(room.WallThickness);
                            this.GetResponse().AppendInt32(room.FloorThickness);
                            
                            this.Session.GetHabbo().CurrentRoom.SendMessage(this.GetResponse());
                            
                            RoomData data2 = room.RoomData;
                            
                            this.GetResponse().Init(Outgoing.RoomData);
                            this.GetResponse().AppendBoolean(false);
                            this.GetResponse().AppendInt32(room.RoomId);
                            this.GetResponse().AppendString(room.Name);
                            this.GetResponse().AppendBoolean(true);
                            this.GetResponse().AppendInt32(room.OwnerId);
                            this.GetResponse().AppendString(room.Owner);
                            this.GetResponse().AppendInt32(room.State);
                            this.GetResponse().AppendInt32(room.UsersNow);
                            this.GetResponse().AppendInt32(room.UsersMax);
                            this.GetResponse().AppendString(room.Description);
                            this.GetResponse().AppendInt32(0);
                            this.GetResponse().AppendInt32((room.Category == 0x34) ? 2 : 0);
                            this.GetResponse().AppendInt32(room.Score);
                            this.GetResponse().AppendInt32(0);
                            this.GetResponse().AppendInt32(room.Category);
                            
                            if (room.GroupId == 0)
                            {
                                this.GetResponse().AppendInt32(0);
                                this.GetResponse().AppendInt32(0);
                            }
                            else
                            {
                                MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(room.GroupId);
                                this.GetResponse().AppendInt32(guild.Id);
                                this.GetResponse().AppendString(guild.Name);
                                this.GetResponse().AppendString(guild.Image);
                            }

                            this.GetResponse().AppendString("");
                            this.GetResponse().AppendInt32(room.TagCount);
                            
                            foreach (string str6 in room.Tags)
                            {
                                this.GetResponse().AppendString(str6);
                            }
                            
                            this.GetResponse().AppendInt32(0);
                            this.GetResponse().AppendInt32(0);
                            this.GetResponse().AppendInt32(0);
                            this.GetResponse().AppendBoolean(true);
                            this.GetResponse().AppendBoolean(true);
                            this.GetResponse().AppendInt32(0);
                            this.GetResponse().AppendInt32(0);
                            
                            this.Response.AppendBoolean(false);
                            this.Response.AppendBoolean(false);
                            this.Response.AppendBoolean(false);
                            
                            this.GetResponse().AppendInt32(0);
                            this.GetResponse().AppendInt32(0);
                            this.GetResponse().AppendInt32(0);
                            this.GetResponse().AppendBoolean(false);
                            this.GetResponse().AppendBoolean(true);
                            this.Session.GetHabbo().CurrentRoom.SendMessage(this.GetResponse());
                        }
                    }
                }
            }
        }

        public void SaveSettingsPackets(MyGuild Guild, Room Room)
        {
            ServerMessage message = new ServerMessage(Outgoing.ConfigureWallandFloor);
            message.AppendBoolean(Room.Hidewall);
            message.AppendInt32(Room.WallThickness);
            message.AppendInt32(Room.FloorThickness);
            this.Session.SendMessage(message);
            ServerMessage message2 = new ServerMessage(Outgoing.RoomData);
            message2.AppendBoolean(false);
            message2.AppendInt32(Room.RoomId);
            message2.AppendString(Room.Name);
            message2.AppendBoolean(true);
            message2.AppendInt32(Room.OwnerId);
            message2.AppendString(Room.Owner);
            message2.AppendInt32(Room.State);
            message2.AppendInt32(Room.UsersNow);
            message2.AppendInt32(Room.UsersMax);
            message2.AppendString(Room.Description);
            message2.AppendInt32(0);
            message2.AppendInt32((Room.Category == 0x34) ? 2 : 0);
            message2.AppendInt32(Room.Score);
            message2.AppendInt32(0);
            message2.AppendInt32(Room.Category);
            if (Room.GetRoomGuild() == null)
            {
                message2.AppendInt32(0);
                message2.AppendInt32(0);
            }
            else
            {
                message2.AppendInt32(Guild.Id);
                message2.AppendString(Guild.Name);
                message2.AppendString(Guild.Image);
            }
            message2.AppendString("");
            message2.AppendInt32(Room.TagCount);
            foreach (string str in Room.Tags.ToArray())
            {
                message2.AppendString(str);
            }
            message2.AppendInt32(0);
            message2.AppendInt32(0);
            message2.AppendInt32(0);
            message2.AppendBoolean(true);
            message2.AppendBoolean(true);
            message2.AppendInt32(0);
            message2.AppendInt32(0);
            message2.AppendBoolean(false);
            message2.AppendBoolean(false);
            message2.AppendBoolean(false);
            message2.AppendInt32(0);
            message2.AppendInt32(0);
            message2.AppendInt32(0);
            message2.AppendBoolean(false);
            message2.AppendBoolean(true);
            Room.SendMessage(message2);
        }

        internal void SaveWardrobe()
        {
            uint num = this.Request.PopWiredUInt();
            string look = this.Request.PopFixedString();
            string gender = this.Request.PopFixedString();
            if (AntiMutant.ValidateLook(look, gender))
            {
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery(string.Concat(new object[] { "SELECT null FROM user_wardrobe WHERE user_id = ", this.Session.GetHabbo().Id, " AND slot_id = ", num }));
                    adapter.addParameter("look", look);
                    adapter.addParameter("gender", gender.ToUpper());
                    if (adapter.getRow() != null)
                    {
                        adapter.setQuery(string.Concat(new object[] { "UPDATE user_wardrobe SET look = @look, gender = @gender WHERE user_id = ", this.Session.GetHabbo().Id, " AND slot_id = ", num, ";" }));
                        adapter.addParameter("look", look);
                        adapter.addParameter("gender", gender.ToUpper());
                        adapter.runQuery();
                    }
                    else
                    {
                        adapter.setQuery(string.Concat(new object[] { "INSERT INTO user_wardrobe (user_id,slot_id,look,gender) VALUES (", this.Session.GetHabbo().Id, ",", num, ",@look,@gender)" }));
                        adapter.addParameter("look", look);
                        adapter.addParameter("gender", gender.ToUpper());
                        adapter.runQuery();
                    }
                }
            }
        }

        internal void SaveWired()
        {
            uint itemID = this.Request.PopWiredUInt();
            WiredSaver.HandleSave(this.Session, itemID, this.Session.GetHabbo().CurrentRoom, this.Request);
        }

        internal void SaveWiredConditions()
        {
            WiredSaver.HandleConditionSave(this.Request.PopWiredUInt(), this.Session.GetHabbo().CurrentRoom, this.Request, this.Session);
        }

        internal void SearchHabbo()
        {
            if (this.Session.GetHabbo().GetMessenger() != null)
            {
                this.Session.SendMessage(this.Session.GetHabbo().GetMessenger().PerformSearch(this.Request.PopFixedString()));
            }
        }

        internal void SearchHelpTopics()
        {
            string query = this.Request.PopFixedString();
            if (query.Length >= 3)
            {
                this.Session.SendMessage(HelpTool.SerializeSearchResults(query));
            }
        }

        public void SendAllowances()
        {
            this.GetResponse().Init(Outgoing.WelcomeUser);
            this.GetResponse().AppendInt32(0);
            this.SendResponse();
            this.GetResponse().Init(Outgoing.SendAllowances);
            this.GetResponse().AppendString("2012-11-09 19:00,hstarsa;2012-11-30 12:00,");
            this.GetResponse().AppendString("hstarsa");
            this.SendResponse();
            this.GetResponse().Init(Outgoing.SendAllowances);
            this.GetResponse().AppendString("2012-11-09 15:00,hstarsbots;2012-11-16 18:00,diarare;2012-11-19 12:00,xmasghost1;2012-11-22 20:00,xmasghost2;2012-11-22 20:45,xmasghost1;2012-11-25 21:00,xmasghost2;2012-11-25 21:45,xmasghost1;2012-11-28 22:00,xmasghost2;2012-11-28 22:45,xmasghost1;2012-11-30 14:00,");
            this.GetResponse().AppendString("xmasghost1");
            this.SendResponse();
            this.GetResponse().Init(Outgoing.SendAllowances);
            this.GetResponse().AppendString("2012-11-23 18:00,hstarssubmit2;2012-11-26 11:00,;2012-11-26 14:00,hstarsvote2;2012-11-28 11:00,");
            this.GetResponse().AppendString("hstarsvote2");
            this.SendResponse();
            this.GetResponse().Init(Outgoing.SendAllowances);
            this.GetResponse().AppendString("2012-11-09 18:00,hspeedway;2012-11-15 15:00,hstarsdiamonds;2012-11-30 12:00,");
            this.GetResponse().AppendString("hstarsdiamonds");
            this.SendResponse();
            this.GetResponse().Init(Outgoing.SendAllowances);
            this.GetResponse().AppendString("");
            this.GetResponse().AppendString("");
            this.SendResponse();
            if (!this.Session.GetHabbo().PassedQuiz)
            {
                this.GetResponse().Init(Outgoing.CitizenshipPanel);
                this.GetResponse().AppendString("citizenship");
                this.GetResponse().AppendInt32(1);
                this.GetResponse().AppendInt32(4);
                this.SendResponse();
            }
        }

        internal void SendFurniGuild()
        {
            ServerMessage message = new ServerMessage(Outgoing.SendHtmlColors);
            message.AppendInt32(this.Session.GetHabbo().ImGuilds.Count);
            foreach (int num in this.Session.GetHabbo().ImGuilds)
            {
                MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(num);
                message.AppendInt32(guild.Id);
                message.AppendString(guild.Name);
                message.AppendString(guild.Image);
                message.AppendString(guild.HtmlColor1);
                message.AppendString(guild.HtmlColor2);
                message.AppendBoolean(guild.Id == this.Session.GetHabbo().FavoriteGuild);
            }
            this.Session.SendMessage(message);
        }

        internal void SendInstantInvite()
        {
            int num = this.Request.PopWiredInt32();
            List<uint> list = new List<uint>();
            for (int i = 0; i < num; i++)
            {
                list.Add(this.Request.PopWiredUInt());
            }
            string s = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString(), true);
            ServerMessage message = new ServerMessage(Outgoing.InstantInvite);
            message.AppendInt32(this.Session.GetHabbo().Id);
            message.AppendString(s);
            foreach (uint num3 in list)
            {
                if (this.Session.GetHabbo().GetMessenger().FriendshipExists(num3))
                {
                    GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(num3);
                    if (clientByUserID == null)
                    {
                        break;
                    }
                    clientByUserID.SendMessage(message);
                }
            }
        }

        internal void SendInstantMessenger()
        {
            try
            {
                if (!ButterflyEnvironment.SystemMute)
                {
                    uint toId = this.Request.PopWiredUInt();
                    string message = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
                    Habbo habbo = ButterflyEnvironment.getHabboForId(toId);

                    if ((this.Session.GetHabbo().GetMessenger() != null) && (habbo.GetClient() != null))
                    {
                        if (this.Session.GetHabbo().GetMessenger().FriendshipExists(toId))
                        {
                            this.Session.GetHabbo().GetMessenger().SendInstantMessage(toId, message);
                        }
                        else this.Session.SendNotif("You are no longer friends with " + habbo.Username);
                    }
                }
            }
            catch 
            {
            
            }
        }

        public void SendRequestGuild()
        {
            int guildId = this.Request.PopWiredInt32();
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData((uint)guild.RoomId);
                if (data != null)
                {
                    IQueryAdapter adapter;
                    ServerMessage message4;
                    if (guild.Type == 0)
                    {
                        if (!this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                        {
                            guild.Members.Add((int)this.Session.GetHabbo().Id, DateTime.Now.Day + " - " + DateTime.Now.Month + " -  " + DateTime.Now.Year);
                            if (this.Session.GetHabbo().FavoriteGuild == 0)
                            {
                                this.Session.GetHabbo().FavoriteGuild = guild.Id;
                                if (this.Session.GetHabbo().CurrentRoomId > 0)
                                {
                                    Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom((uint)guild.RoomId);
                                    if (data == null)
                                    {
                                        return;
                                    }
                                    List<RoomUser> list = new List<RoomUser>(ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId).GetRoomUserManager().UserList.Values);
                                    ServerMessage message = new ServerMessage(Outgoing.SendGroup);
                                    message.AppendInt32(1);
                                    message.AppendInt32(guild.Id);
                                    message.AppendString(guild.Image);
                                    this.Session.SendMessage(message);
                                    ServerMessage message2 = new ServerMessage(Outgoing.SetRoomUser);
                                    message2.AppendInt32(1);
                                    foreach (RoomUser user in list)
                                    {
                                        if (user.HabboId == this.Session.GetHabbo().Id)
                                        {
                                            user.Serialize(message2, false);
                                        }
                                    }
                                    room.SendMessage(message2);
                                }
                            }
                            this.Session.GetHabbo().ImGuilds.Add(guild.Id);
                            guild.GuildRanks.Add((int)this.Session.GetHabbo().Id, 2);
                            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET FavoriteGroup = '", this.Session.GetHabbo().FavoriteGuild, "' WHERE Id = '", this.Session.GetHabbo().Id, "'" }));
                                adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_groups VALUES (NULL, '", this.Session.GetHabbo().Username, "','", this.Session.GetHabbo().Id, "','", guildId, "','", DateTime.Now.Day, " - ", DateTime.Now.Month, " -  ", DateTime.Now.Year, "', '2')" }));
                                adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_stream VALUES (NULL, '", this.Session.GetHabbo().Id, "','", ButterflyEnvironment.LinkConfig, guild.Image, ".gif','10','", guild.Id, "','0','')" }));
                            }
                            //guild = ButterflyEnvironment.GetGame().GetGuilds().ReloadGuild(guildId);
                            ServerMessage message3 = new ServerMessage(Outgoing.SendHtmlColors);
                            message3.AppendInt32(this.Session.GetHabbo().ImGuilds.Count);
                            foreach (int num2 in this.Session.GetHabbo().ImGuilds)
                            {
                                MyGuild guild2 = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(num2);
                                message3.AppendInt32(guild2.Id);
                                message3.AppendString(guild2.Name);
                                message3.AppendString(guild2.Image);
                                message3.AppendString(guild2.HtmlColor1);
                                message3.AppendString(guild2.HtmlColor2);
                                message3.AppendBoolean(guild2.Id == this.Session.GetHabbo().FavoriteGuild);
                            }
                            this.Session.SendMessage(message3);
                            message4 = new ServerMessage(Outgoing.SendAdvGroupInit);
                            message4.AppendInt32(guild.Id);
                            message4.AppendBoolean(true);
                            message4.AppendInt32(guild.Type);
                            message4.AppendString(guild.Name);
                            message4.AppendString(guild.Description);
                            message4.AppendString(guild.Image);
                            message4.AppendInt32(data.Id);
                            message4.AppendString(data.Name);
                            if (guild.Petitions.Contains((int)this.Session.GetHabbo().Id))
                            {
                                message4.AppendInt32(2);
                            }
                            else if (!this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                            {
                                message4.AppendInt32(0);
                            }
                            else if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                            {
                                message4.AppendInt32(1);
                            }
                            message4.AppendInt32(guild.Members.Count);
                            message4.AppendBoolean(false);
                            message4.AppendString(guild.DateCreated);
                            message4.AppendBoolean(this.Session.GetHabbo().Id == guild.OwnerId);
                            message4.AppendBoolean(this.Session.GetHabbo().FavoriteGuild == guild.Id);
                            message4.AppendString(guild.OwnerName);
                            message4.AppendBoolean(true);
                            message4.AppendBoolean(true);
                            message4.AppendInt32(guild.Members.ContainsKey((int)this.Session.GetHabbo().Id) ? guild.Petitions.Count : 0);
                            this.Session.SendMessage(message4);
                        }
                    }
                    else if ((guild.Type == 1) && !guild.Petitions.Contains((int)this.Session.GetHabbo().Id))
                    {
                        guild.Petitions.Add((int)this.Session.GetHabbo().Id);
                        string str = "";
                        foreach (int num3 in guild.Petitions)
                        {
                            str = str + num3 + ";";
                        }
                        if (str.Length > (this.Session.GetHabbo().Id.ToString().Length + 1))
                        {
                            str = str.Substring(0, str.Length - 1);
                        }
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.runFastQuery(string.Concat(new object[] { "UPDATE groups SET Petitions = '", str, "' WHERE Id = '", guild.Id, "'" }));
                        }
                        message4 = new ServerMessage(Outgoing.SendAdvGroupInit);
                        message4.AppendInt32(guild.Id);
                        message4.AppendBoolean(true);
                        message4.AppendInt32(guild.Type);
                        message4.AppendString(guild.Name);
                        message4.AppendString(guild.Description);
                        message4.AppendString(guild.Image);
                        message4.AppendInt32((int)data.Id);
                        message4.AppendString(data.Name);
                        if (guild.Petitions.Contains((int)this.Session.GetHabbo().Id))
                        {
                            message4.AppendInt32(2);
                        }
                        else if (!this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                        {
                            message4.AppendInt32(0);
                        }
                        else if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                        {
                            message4.AppendInt32(1);
                        }
                        message4.AppendInt32(guild.Members.Count);
                        message4.AppendBoolean(false);
                        message4.AppendString(guild.DateCreated);
                        message4.AppendBoolean(this.Session.GetHabbo().Id == guild.OwnerId);
                        message4.AppendBoolean(this.Session.GetHabbo().FavoriteGuild == guild.Id);
                        message4.AppendString(guild.OwnerName);
                        message4.AppendBoolean(true);
                        message4.AppendBoolean(true);
                        message4.AppendInt32(guild.Members.ContainsKey((int)this.Session.GetHabbo().Id) ? guild.Petitions.Count : 0);
                        this.Session.SendMessage(message4);
                    }
                }
            }
        }

        internal void SendResponse()
        {
            if (((this.Response != null) && (this.Response.Id > 0)) && (this.Session.GetConnection() != null))
            {
                this.Session.GetConnection().SendData(this.Response.GetBytes());
                //Console.WriteLine("Inviato -> " + Response.Id);
            }
        }

        internal void SendResponseWithOwnerParam()
        {
            if (((this.Response != null) && (this.Response.Id > 0)) && (this.Session.GetConnection() != null))
            {
                this.Response.AppendBoolean(this.Session.GetHabbo().CurrentRoom.CheckRights(this.Session, true));
                this.Session.GetConnection().SendData(this.Response.GetBytes());                
            }
        }

        internal void setClientVars()
        {
            
            string str = this.Request.PopFixedString();
            string str2 = this.Request.PopFixedString();
            
        }

        internal void SetHomeRoom()
        {
            uint roomId = this.Request.PopWiredUInt();
            RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomId);
            if ((roomId == 0) || ((data != null)))
            {
                this.Session.GetHabbo().HomeRoom = roomId;
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET home_room = ", roomId, " WHERE id = ", this.Session.GetHabbo().Id }));
                }
                this.Response.Init(Outgoing.HomeRoom);
                this.Response.AppendInt32(roomId);
                this.Response.AppendInt32(roomId);
                this.SendResponse();
            }
        }

        internal void SetLookTransfer()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session))
            {
                uint key = this.Request.PopWiredUInt();
                string str = this.Request.PopFixedString().ToUpper();
                string figure = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
                RoomItem item = room.GetRoomItemHandler().mFloorItems.GetValue(key);
                if ((str.Length <= 1) && (!(str != "M") || !(str != "F")))
                {
                    item.Figure = ButterflyEnvironment.FilterFigure(figure);
                    item.Gender = str;
                    item.ExtraData = str + ":" + figure;
                }
            }
        }

        internal void setUniqueIDToClient()
        {
            string str = this.Request.PopFixedString();
            this.Session.MachineId = str;
        }

        internal void Shout()
        {
            if (!ButterflyEnvironment.SystemMute)
            {
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
                if (room != null)
                {
                    RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                    if (roomUserByHabbo != null)
                    {
                        roomUserByHabbo.Chat(this.Session, ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString()), true, this.Request.PopWiredInt32());
                    }
                }
            }
        }

        internal void Sign()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.Unidle();
                    int num = this.Request.PopWiredInt32();
                    roomUserByHabbo.AddStatus("sign", Convert.ToString(num));
                    roomUserByHabbo.UpdateNeeded = true;
                }
            }
        }

        internal void SSOLogin()
        {
            if (this.Session.GetHabbo() == null)
            {
                this.Session.tryLogin(this.Request.PopFixedString());
                this.Session.TimePingedReceived = DateTime.Now;
            }
            else
            {
                this.Session.SendNotif(LanguageLocale.GetValue("user.allreadylogedon"));
            }
        }

        internal void StartGame(Butterfly.HabboHotel.Games.SnowStorm.SnowStorm War)
        {
            War.AssignTeams();
            War.WarStarted = 1;
            this.GetResponse().Init(Outgoing.SetStep1);
            this.GetResponse().AppendInt32(War.WarId);
            this.GetResponse().AppendString("SnowStorm level " + War.WarLevel);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(War.WarLevel);
            this.GetResponse().AppendInt32(2);
            this.GetResponse().AppendInt32(War.MaxUsers);
            this.GetResponse().AppendString(War.WarOwner.Username);
            this.GetResponse().AppendInt32(15);
            this.GetResponse().AppendInt32(War.WarUsers.Count);
            foreach (Habbo habbo in War.WarUsers)
            {
                this.GetResponse().AppendInt32(habbo.Id);
                this.GetResponse().AppendString(habbo.Username);
                this.GetResponse().AppendString(habbo.Look);
                this.GetResponse().AppendString((habbo.Gender.ToUpper() == "M") ? "M" : "f");
                this.GetResponse().AppendInt32(habbo.SnowTeam);
                this.GetResponse().AppendInt32(habbo.SnowLevel);
                this.GetResponse().AppendInt32(habbo.SnowPoints);
                this.GetResponse().AppendInt32(ButterflyEnvironment.GetGame().GetStormWars().LevelScore[habbo.SnowLevel]);
            }
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(120);
            War.SendToStorm(this.GetResponse(), false, 0);
            this.GetResponse().Init(Outgoing.Game2EnterArenaMessageEvent);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(War.WarLevel);
            this.GetResponse().AppendInt32(2);
            this.GetResponse().AppendInt32(0);
            this.GetResponse().AppendInt32(50);
            this.GetResponse().AppendInt32(50);
            this.GetResponse().AppendString(War.Model.SerializeHeightMap());
            this.GetResponse().AppendInt32(War.Model.SnowItems.Count);
            foreach (SnowItems items in War.Model.SnowItems)
            {
                items.SerializeItem(this.GetResponse());
            }
            War.SendToStorm(this.GetResponse(), false, 0);
            foreach (Habbo habbo in War.WarUsers)
            {
                this.GetResponse().Init(Outgoing.Game2ArenaEnteredMessageEvent);
                this.GetResponse().AppendInt32(habbo.Id);
                this.GetResponse().AppendString(habbo.Username);
                this.GetResponse().AppendString(habbo.Look);
                this.GetResponse().AppendString(habbo.Gender.ToLower());
                this.GetResponse().AppendInt32(habbo.SnowTeam);
                War.SendToStorm(this.GetResponse(), false, 0);
            }
            this.GetResponse().Init(Outgoing.Game2StageLoadMessageEvent);
            this.GetResponse().AppendInt32(0);
            War.SendToStorm(this.GetResponse(), false, 0);
            Action<object> action = delegate(object obj)
            {
                Thread.Sleep(0x1388);
                this.GetResponse().Init(Outgoing.Game2StageStillLoadingMessageEvent);
                this.GetResponse().AppendInt32(0);
                this.GetResponse().AppendInt32(0);
                War.SendToStorm(this.GetResponse(), false, 0);
                this.LoadArena(War);
            };
            new Task(action, "pingsession").Start();
        }

        internal void StartPanel()
        {
            Action<object> action2 = null;
            Butterfly.HabboHotel.Games.SnowStorm.SnowStorm War = this.CheckAGame();
            if (War == null)
            {
                War = new Butterfly.HabboHotel.Games.SnowStorm.SnowStorm(this.Session.GetHabbo());
                ServerMessage message = new ServerMessage(Outgoing.CreateWar);
                message.AppendInt32(-1);
                message.AppendString("SnowStorm level " + War.WarLevel);
                message.AppendInt32(0);
                message.AppendInt32(War.WarLevel);
                message.AppendInt32(2);
                message.AppendInt32(War.MaxUsers);
                message.AppendString(War.WarOwner.Username);
                message.AppendInt32(0x11);
                message.AppendInt32(1);
                message.AppendInt32(War.WarOwner.Id);
                message.AppendString(War.WarOwner.Username);
                message.AppendString(War.WarOwner.Look);
                message.AppendString(War.WarOwner.Gender.ToLower());
                message.AppendInt32(-1);
                message.AppendInt32(War.WarOwner.SnowLevel);
                message.AppendInt32(War.WarOwner.SnowPoints);
                message.AppendInt32(1);
                message.AppendInt32(0);
                message.AppendInt32(120);
                this.Session.SendMessage(message);
            }
            War.SendToStorm(this.AddToNewGame(War), false, 0);
            if (War.WarUsers.Count >= 1)
            {
                ServerMessage packet = new ServerMessage(Outgoing.StartCounter);
                packet.AppendInt32(15);
                War.SendToStorm(packet, false, 0);
                if (action2 == null)
                {
                    action2 = delegate(object obj)
                    {
                        Thread.Sleep(0x3a98);
                        this.StartGame(War);
                    };
                }
                Action<object> action = action2;
                new Task(action, "break").Start();
            }
        }

        public void StartQuest()
        {
            ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().ActiveQuest(this.Session, this.Request);
        }

        public void StartQuiz()
        {
            if (this.Request.PopFixedString() == "SafetyQuiz1")
            {
                this.EndQuiz();
            }
            else
            {
                ServerMessage message = new ServerMessage(Outgoing.LoadQuiz);
                message.AppendString("HabboWay1");
                message.AppendInt32(5);
                message.AppendInt32(5);
                message.AppendInt32(7);
                message.AppendInt32(0);
                message.AppendInt32(1);
                message.AppendInt32(6);
                this.Session.SendMessage(message);
            }
        }

        internal void StartTyping()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    ServerMessage message = new ServerMessage(Outgoing.TypingStatus);
                    message.AppendInt32(roomUserByHabbo.VirtualId);
                    message.AppendInt32(1);
                    room.SendMessage(message);
                }
            }
        }

        public void StopQuest()
        {
            ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().CancelQuest(this.Session);
        }

        internal void StopTrade()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CanTradeInRoom)
            {
                room.TryStopTrade(this.Session.GetHabbo().Id);
            }
        }

        internal void StopTyping()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    ServerMessage message = new ServerMessage(Outgoing.TypingStatus);
                    message.AppendInt32(roomUserByHabbo.VirtualId);
                    message.AppendInt32(0);
                    room.SendMessage(message);
                }
            }
        }

        internal void StreamLike()
        {
            int num = this.Request.PopWiredInt32();
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE user_stream SET Likes=Likes + 1 WHERE Id = '" + num + "'");
            }
        }

        internal void SubmitHelpTicket()
        {
            bool flag = false;
            if (ButterflyEnvironment.GetGame().GetModerationTool().UsersHasPendingTicket(this.Session.GetHabbo().Id))
            {
                flag = true;
            }
            if (!flag)
            {
                string message = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
                int num = this.Request.PopWiredInt32();
                int category = this.Request.PopWiredInt32();
                uint reportedUser = this.Request.PopWiredUInt();
                ButterflyEnvironment.GetGame().GetModerationTool().SendNewTicket(this.Session, category, reportedUser, message);
                ServerMessage TicketSended = new ServerMessage(Outgoing.HelpRequestAlert);
                TicketSended.AppendInt32(0);
                Session.SendMessage(TicketSended);
            }
        }

        internal void SwitchMoodlightStatus()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (((room != null) && room.CheckRights(this.Session, true)) && (room.MoodlightData != null))
            {
                RoomItem item = room.GetRoomItemHandler().GetItem(room.MoodlightData.ItemId);
                if ((item != null) && (item.GetBaseItem().InteractionType == InteractionType.dimmer))
                {
                    if (room.MoodlightData.Enabled)
                    {
                        room.MoodlightData.Disable();
                    }
                    else
                    {
                        room.MoodlightData.Enable();
                    }
                    item.ExtraData = room.MoodlightData.GenerateExtraData();
                    item.UpdateState();
                }
            }
        }

        internal void TakeAllRights()
        {
            Room room;
            try
            {
                room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            }
            catch
            {
                room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Request.PopWiredUInt());
            }
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                foreach (uint num in room.UsersWithRights)
                {
                    RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(num);
                    this.Response.Init(Outgoing.RemovePowers);
                    this.Response.AppendInt32(room.RoomId);
                    this.Response.AppendInt32(num);
                    this.SendResponse();
                    roomUserByHabbo.UpdateNeeded = true;

                    ServerMessage message = new ServerMessage(Outgoing.QuitRights);
                    roomUserByHabbo.GetClient().SendMessage(message);
                    roomUserByHabbo.RemoveStatus("flatctrl 1");
                    roomUserByHabbo.AddStatus("flatctrl 0", "");
                    roomUserByHabbo.UpdateNeeded = true;
                }
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery("DELETE FROM room_rights WHERE room_id = " + room.RoomId);
                }
                room.UsersWithRights.Clear();
            }
        }

        internal void TakeBackTradeItem()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CanTradeInRoom)
            {
                Trade userTrade = room.GetUserTrade(this.Session.GetHabbo().Id);
                UserItem item = this.Session.GetHabbo().GetInventoryComponent().GetItem(this.Request.PopWiredUInt());
                if ((userTrade != null) && (item != null))
                {
                    userTrade.TakeBackItem(this.Session.GetHabbo().Id, item);
                }
            }
        }

        internal void TakeItem()
        {
            try
            {
                int num = this.Request.PopWiredInt32();
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
                RoomItem item = room.GetRoomItemHandler().GetItem(this.Request.PopWiredUInt());

                if ((room != null) && ((room.CheckRights(this.Session, true)) || (item.placedBy == this.Session.GetHabbo().Id)))
                {
                    if (item.placedBy == 0) 
                        item.placedBy = this.Session.GetHabbo().Id;

                    Habbo habbo = ButterflyEnvironment.getHabboForId(item.placedBy);

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

                            room.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                        }

                        else
                        {
                            room.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                            room.GetRoomItemHandler().RemoveFurniture(habbo.GetClient(), item.Id);
                            habbo.GetInventoryComponent().AddNewItem(item.Id, item.BaseItem, item.ExtraData, true, true, item.RareId, item.Guilds_Data_String,habbo.Id, 0);
                            habbo.GetInventoryComponent().UpdateItems(false);
                        }
                    }
                }
            }
            
            catch 
            {
                
            }
        }

        internal void EjectFurni()
        {
            try
            {
                int num = this.Request.PopWiredInt32();
                Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
                if ((room != null) && ((room.CheckRights(this.Session, true)) || (room.checkGroupRights(this.Session))))
                {
                    RoomItem item = room.GetRoomItemHandler().GetItem(this.Request.PopWiredUInt());
                    Habbo habbo = ButterflyEnvironment.getHabboForId(item.placedBy);

                    if ((item != null) && (item.GetBaseItem().InteractionType != InteractionType.postit) && (item.placedBy != room.OwnerId) && (item.placedBy != 0))
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

                            room.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                        }

                        else
                        {
                            room.GetRoomItemHandler().RemoveFurniture(this.Session, item.Id);
                            room.GetRoomItemHandler().RemoveFurniture(habbo.GetClient(), item.Id);
                            habbo.GetInventoryComponent().AddNewItem(item.Id, item.BaseItem, item.ExtraData, true, true, item.RareId, item.Guilds_Data_String,item.placedBy, 0);
                            habbo.GetInventoryComponent().UpdateItems(false);
                        }
                    }
                }
            }

            catch 
            {
             
            }
        }

        internal void TakeRights()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CheckRights(this.Session, true))
            {
                StringBuilder builder = new StringBuilder();
                int num = this.Request.PopWiredInt32();
                for (int i = 0; i < num; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(" OR ");
                    }
                    uint item = this.Request.PopWiredUInt();
                    room.UsersWithRights.Remove(item);
                    builder.Append(string.Concat(new object[] { "room_id = '", room.RoomId, "' AND user_id = '", item, "'" }));
                    RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(item);
                    this.Response.Init(Outgoing.RemovePowers);
                    this.Response.AppendInt32(room.RoomId);
                    this.Response.AppendInt32(item);
                    this.SendResponse();
                    
                    ServerMessage message = new ServerMessage(Outgoing.QuitRights);
                    roomUserByHabbo.GetClient().SendMessage(message);
                    roomUserByHabbo.RemoveStatus("flatctrl 1");
                    roomUserByHabbo.AddStatus("flatctrl 0", "");
                    roomUserByHabbo.UpdateNeeded = true;
                }
                if (builder.ToString().Length > 0)
                {
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery("DELETE FROM room_rights WHERE " + builder.ToString());
                    }
                }
            }
        }

        internal void Talk()
        {
            try
            {
                if (!ButterflyEnvironment.SystemMute)
                {
                    Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
                    if (room != null)
                    {
                        RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                        if (roomUserByHabbo != null)
                        {
                            roomUserByHabbo.Chat(this.Session, ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString()), false, this.Request.PopWiredInt32());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal void TalkGame()
        {
            if (((this.Session != null) || (this.Session.GetHabbo() != null)) && (this.Session.GetHabbo().SnowWar != null))
            {
                Butterfly.HabboHotel.Games.SnowStorm.SnowStorm snowWar = this.Session.GetHabbo().SnowWar;
                string s = this.Request.PopFixedString();
                this.GetResponse().Init(Outgoing.Game2GameChatFromPlayerMessageEvent);
                this.GetResponse().AppendInt32(this.Session.GetHabbo().Id);
                this.GetResponse().AppendString(s);
                snowWar.SendToStorm(this.GetResponse(), false, 0);
            }
        }

        internal void TriggerItem()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                uint pId = this.Request.PopWiredUInt();
                RoomItem item = room.GetRoomItemHandler().GetItem(pId);
                if (item != null)
                {
                    if (item.GetBaseItem().Name.ToLower().Contains("bw_lgchair") && (this.Session.GetHabbo().QuestActivate == 10))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("bw_sboard") && (this.Session.GetHabbo().QuestActivate == 12))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("bw_van") && (this.Session.GetHabbo().QuestActivate == 13))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("party_floor") && (this.Session.GetHabbo().QuestActivate == 14))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("party_ball") && (this.Session.GetHabbo().QuestActivate == 15))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("jukebox") && (this.Session.GetHabbo().QuestActivate == 0x10))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("bb_gate_b") && (this.Session.GetHabbo().QuestActivate == 0x11))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("bb_patch") && (this.Session.GetHabbo().QuestActivate == 0x12))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("bb_rnd_tele") && (this.Session.GetHabbo().QuestActivate == 0x13))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("es_gate_r") && (this.Session.GetHabbo().QuestActivate == 20))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("es_score_r") && (this.Session.GetHabbo().QuestActivate == 0x15))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("es_exit") && (this.Session.GetHabbo().QuestActivate == 0x16))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("es_tagging") && (this.Session.GetHabbo().QuestActivate == 0x18))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("bb_counter") && (this.Session.GetHabbo().QuestActivate == 0x1a))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("bb_score_b") && (this.Session.GetHabbo().QuestActivate == 0x1b))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("es_tile") && (this.Session.GetHabbo().QuestActivate == 0x31))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    else if (item.GetBaseItem().Name.ToLower().Contains("es_box") && (this.Session.GetHabbo().QuestActivate == 50))
                    {
                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("FIND_STUFF", this.Session);
                    }
                    bool userHasRights = false;
                    
                    if (room.CheckRights(this.Session) || (item.GetBaseItem().InteractionType == InteractionType.dice))
                    {
                        userHasRights = true;
                    }
                    string extraData = item.ExtraData;
                    int request = this.Request.PopWiredInt32();
                    item.Interactor.OnTrigger(this.Session, item, request, userHasRights);
                    item.OnTrigger(room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id));
                    if (room.GotWired() && !WiredUtillity.TypeIsWired(item.GetBaseItem().InteractionType))
                    {
                        bool flag2 = false;
                        int getX = item.GetX;
                        int getY = item.GetY;
                        Point coord = new Point(getX, getY + 1);
                        Point point2 = new Point(getX + 1, getY);
                        Point point3 = new Point(getX, getY - 1);
                        Point point4 = new Point(getX - 1, getY);
                        foreach (RoomItem item2 in room.GetGameMap().GetCoordinatedItems(coord))
                        {
                            if (WiredHandler.TypeIsWire(item2.GetBaseItem().InteractionType))
                            {
                                flag2 = true;
                            }
                        }
                        foreach (RoomItem item2 in room.GetGameMap().GetCoordinatedItems(point2))
                        {
                            if (WiredHandler.TypeIsWire(item2.GetBaseItem().InteractionType))
                            {
                                flag2 = true;
                            }
                        }
                        foreach (RoomItem item2 in room.GetGameMap().GetCoordinatedItems(point3))
                        {
                            if (WiredHandler.TypeIsWire(item2.GetBaseItem().InteractionType))
                            {
                                flag2 = true;
                            }
                        }
                        foreach (RoomItem item2 in room.GetGameMap().GetCoordinatedItems(point4))
                        {
                            if (WiredHandler.TypeIsWire(item2.GetBaseItem().InteractionType))
                            {
                                flag2 = true;
                            }
                        }
                        if (flag2)
                        {
                            room.GetWiredHandler().TriggerOnWire(item.Coordinate);
                        }
                        else
                        {
                            room.GetWiredHandler().RemoveWiredItem(item.Coordinate);
                        }
                    }
                }
            }
        }

        internal void TriggerItemDiceSpecial()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomItem item = room.GetRoomItemHandler().GetItem(this.Request.PopWiredUInt());
                if (item != null)
                {
                    bool userHasRights = false;
                    if (room.CheckRights(this.Session))
                    {
                        userHasRights = true;
                    }
                    item.Interactor.OnTrigger(this.Session, item, -1, userHasRights);
                    item.OnTrigger(room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id));
                }
            }
        }

        internal void UnacceptTrade()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if ((room != null) && room.CanTradeInRoom)
            {
                Trade userTrade = room.GetUserTrade(this.Session.GetHabbo().Id);
                if (userTrade != null)
                {
                    userTrade.Unaccept(this.Session.GetHabbo().Id);
                }
            }
        }

        internal void UnignoreUser()
        {
            if (this.Session.GetHabbo().CurrentRoom != null)
            {
                string username = this.Request.PopFixedString();
                Habbo habbo = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(username).GetHabbo();
                if ((habbo != null) && this.Session.GetHabbo().MutedUsers.Contains(habbo.Id))
                {
                    this.Session.GetHabbo().MutedUsers.Remove(habbo.Id);
                    this.Response.Init(Outgoing.UpdateIgnoreStatus);
                    this.Response.AppendInt32(3);
                    this.Response.AppendString(username);
                    this.SendResponse();
                }
            }
        }

        internal void UpdateAllMount()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (((room != null) && !room.IsPublic) && (room.AllowPets || room.CheckRights(this.Session, true)))
            {
                uint petId = this.Request.PopWiredUInt();
                RoomUser pet = room.GetRoomUserManager().GetPet(petId);
                if (((pet != null) && (pet.PetData != null)) && (pet.PetData.OwnerId == this.Session.GetHabbo().Id))
                {
                    IQueryAdapter adapter;
                    if (pet.PetData.CanMountAllPeople)
                    {
                        pet.PetData.CanMountAllPeople = false;
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.runFastQuery("UPDATE user_pets SET can_all = 0 WHERE id = " + pet.PetData.PetId);
                        }
                    }
                    else
                    {
                        pet.PetData.CanMountAllPeople = true;
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.runFastQuery("UPDATE user_pets SET can_all = 1 WHERE id = " + pet.PetData.PetId);
                        }
                    }
                    this.Session.SendMessage(pet.PetData.SerializeInfo());
                }
            }
        }

        internal void UpdateBadges()
        {
            IQueryAdapter adapter;
            this.Session.GetHabbo().GetBadgeComponent().ResetSlots();
            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE user_badges SET badge_slot = 0 WHERE user_id = " + this.Session.GetHabbo().Id);
            }
            for (int i = 0; i < 5; i++)
            {
                int num2 = this.Request.PopWiredInt32();
                string str = this.Request.PopFixedString();
                if (str.Length != 0)
                {
                    if ((!this.Session.GetHabbo().GetBadgeComponent().HasBadge(str) || (num2 < 1)) || (num2 > 5))
                    {
                        return;
                    }
                    this.Session.GetHabbo().GetBadgeComponent().GetBadge(str).Slot = num2;
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE user_badges SET badge_slot = ", num2, " WHERE badge_id = '", str, "' AND user_id = ", this.Session.GetHabbo().Id }));
                    }
                }
            }
            ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("WEAR_BADGE", this.Session);
            ServerMessage message = new ServerMessage(Outgoing.UpdateBadges);
            message.AppendInt32(this.Session.GetHabbo().Id);
            message.AppendInt32(this.Session.GetHabbo().GetBadgeComponent().EquippedCount);
            foreach (Badge badge in this.Session.GetHabbo().GetBadgeComponent().BadgeList.Values)
            {
                if (badge.Slot > 0)
                {
                    message.AppendInt32(badge.Slot);
                    message.AppendString(badge.Code);
                }
            }
            if (this.Session.GetHabbo().InRoom && (ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId) != null))
            {
                ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId).SendMessage(message);
            }
            else
            {
                this.Session.SendMessage(message);
            }
        }

        internal void UpdateMoodlight()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (((room != null) && room.CheckRights(this.Session, true)) && (room.MoodlightData != null))
            {
                RoomItem item = room.GetRoomItemHandler().GetItem(room.MoodlightData.ItemId);
                if ((item != null) && (item.GetBaseItem().InteractionType == InteractionType.dimmer))
                {
                    int preset = this.Request.PopWiredInt32();
                    int num2 = this.Request.PopWiredInt32();
                    string color = this.Request.PopFixedString();
                    int intensity = this.Request.PopWiredInt32();
                    bool bgOnly = false;
                    if (num2 >= 2)
                    {
                        bgOnly = true;
                    }
                    room.MoodlightData.Enabled = true;
                    room.MoodlightData.CurrentPreset = preset;
                    room.MoodlightData.UpdatePreset(preset, color, intensity, bgOnly);
                    item.ExtraData = room.MoodlightData.GenerateExtraData();
                    item.UpdateState();
                }
            }
        }

        public void UpdateUserFromRankGuild()
        {
            int guildId = this.Request.PopWiredInt32();
            int UserId = this.Request.PopWiredInt32();
        }

        public void UpdateUserToRankGuild()
        {
            int guildId = this.Request.PopWiredInt32();
            
            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(guildId);
            if (guild != null)
            {
                int num2 = this.Request.PopWiredInt32();
                
                Habbo habbo = ButterflyEnvironment.getHabboForId((uint)num2);
                if (habbo != null)
                {
                    RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData((uint)guild.RoomId); guild.GuildRanks[num2] = (guild.GuildRanks[num2] == 1) ? 2 : 1;
                    
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE user_groups SET Rank = '", guild.GuildRanks[num2], "' WHERE UserId = '", num2, "' AND GroupId = '", guildId, "'" }));
                    }

                    try
                    {
                        ServerMessage message = new ServerMessage(Outgoing.SendHtmlColors);
                        message.AppendInt32(habbo.ImGuilds.Count);
                        foreach (int num3 in habbo.ImGuilds)
                        {
                            MyGuild guild2 = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(num3);
                            message.AppendInt32(guild2.Id);
                            message.AppendString(guild2.Name);
                            message.AppendString(guild2.Image);
                            message.AppendString(guild2.HtmlColor1);
                            message.AppendString(guild2.HtmlColor2);
                            message.AppendBoolean(guild2.Id == habbo.FavoriteGuild);
                        }

                        habbo.GetClient().SendMessage(message);
                        ServerMessage message1;

                        message1 = new ServerMessage(Outgoing.SendAdvGroupInit);
                        message1.AppendInt32(guild.Id);
                        message1.AppendBoolean(true);
                        message1.AppendInt32(guild.Type);
                        message1.AppendString(guild.Name);
                        message1.AppendString(guild.Description);
                        message1.AppendString(guild.Image);
                        message1.AppendInt32(data.Id);
                        message1.AppendString(data.Name);
                        if (guild.Petitions.Contains((int)habbo.Id))
                        {
                            message1.AppendInt32(2);
                        }
                        else if (!habbo.ImGuilds.Contains(guild.Id))
                        {
                            message1.AppendInt32(0);
                        }
                        else if (habbo.ImGuilds.Contains(guild.Id))
                        {
                            message1.AppendInt32(1);
                        }
                        message1.AppendInt32(guild.Members.Count);
                        message1.AppendBoolean(false);
                        message1.AppendString(guild.DateCreated);
                        message1.AppendBoolean(habbo.Id == guild.OwnerId);

                        if (this.Session.GetHabbo().ImGuilds.Contains(guild.Id))
                        {
                            if (guild.GuildRanks[(int)habbo.Id] < 2)
                                message1.AppendBoolean(true);
                            else message1.AppendBoolean(false);
                        }
                        else message1.AppendBoolean(false);
                        message1.AppendString(guild.OwnerName);
                        message1.AppendBoolean(true);
                        message1.AppendBoolean(true);
                        message1.AppendInt32(guild.Members.ContainsKey((int)habbo.Id) ? guild.Petitions.Count : 0);
                        habbo.GetClient().SendMessage(message);
                    }
                    catch 
                    {
                    
                    }
                    LoadMembersPetitions(0, guildId, 0, "");
                }
            }
        }

        internal void ViewHelpTopic()
        {
            uint topicId = this.Request.PopWiredUInt();
            HelpTopic topic = ButterflyEnvironment.GetGame().GetHelpTool().GetTopic(topicId);
            if (topic != null)
            {
                this.Session.SendMessage(HelpTool.SerializeTopic(topic));
            }
        }

        internal void EventTracker()
        {
            //Toolbar messages
            string arg1 = this.Request.PopFixedString();
            string arg2 = this.Request.PopFixedString();
            string arg3 = this.Request.PopFixedString();
            Console.WriteLine(arg1 + "-" +arg2+"-"+arg3);
            //if ((arg1 == "Navigation") && (arg3 == "category.view"))

        }

        internal void volumeControl()
        {
            //volume controls are being save but loading of the volumes need to be coded yet
            int num = this.Request.PopWiredInt32();
            int num1 = this.Request.PopWiredInt32();
            int num2 = this.Request.PopWiredInt32();

            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MySQL)
                {
                    adapter.runFastQuery("REPLACE INTO user_volume (userid, system_volume, furni_volume, trax_volume) VALUES (" + this.Session.GetHabbo().Id + "," + num + "," + num1 + ", " + num2 + " )");
                }

            }

        }

        internal void WalkGame()
        {
            //int num = this.Request.PopWiredInt32();
            //int num2 = this.Request.PopWiredInt32();
            //Console.WriteLine(string.Concat(new object[] { "X: ", num, " ;Y:", num2 }));

            
        }

        internal void Wave()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.Unidle();
                    int i = this.Request.PopWiredInt32();
                    roomUserByHabbo.DanceId = 0;
                    ServerMessage message = new ServerMessage(Outgoing.Action);
                    message.AppendInt32(roomUserByHabbo.VirtualId);
                    message.AppendInt32(i);
                    room.SendMessage(message);
                    if (i == 5)
                    {
                        roomUserByHabbo.IsAsleep = true;
                        ServerMessage message2 = new ServerMessage(Outgoing.IdleStatus);
                        message2.AppendInt32(roomUserByHabbo.VirtualId);
                        message2.AppendBoolean(roomUserByHabbo.IsAsleep);
                        room.SendMessage(message2);
                    }
                    ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("WAVE", this.Session);
                }
            }
        }

        internal void Whisper()
        {
            if (!ButterflyEnvironment.SystemMute && ((this.Session != null) && (this.Session.GetHabbo() != null)))
            {
                Room currentRoom = this.Session.GetHabbo().CurrentRoom;
                if (currentRoom != null)
                {
                    if (this.Session.GetHabbo().Muted)
                    {
                        this.Session.SendNotif(LanguageLocale.GetValue("user.ismuted"));
                    }
                    else if (!currentRoom.RoomMuted)
                    {
                        string str = ButterflyEnvironment.FilterInjectionChars(this.Request.PopFixedString());
                        string pName = str.Split(new char[] { ' ' })[0];
                        string s = LanguageLocale.FilterSwearwords(str.Substring(pName.Length + 1));
                        RoomUser roomUserByHabbo = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(this.Session.GetHabbo().Id);
                        RoomUser user2 = currentRoom.GetRoomUserManager().GetRoomUserByHabbo(pName);
                        int i = this.Request.PopWiredInt32();
                        ServerMessage message = new ServerMessage();
                        message.Init(Outgoing.Whisp);
                        message.AppendInt32(roomUserByHabbo.VirtualId);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(i);
                        message.AppendInt32(0);
                        message.AppendInt32(-1);
                        if (!((roomUserByHabbo == null) || roomUserByHabbo.IsBot))
                        {
                            roomUserByHabbo.GetClient().SendMessage(message);
                        }
                        roomUserByHabbo.Unidle();
                        if ((((user2 != null) && !user2.IsBot) && (user2.userID != roomUserByHabbo.userID)) && !user2.GetClient().GetHabbo().MutedUsers.Contains(this.Session.GetHabbo().Id))
                        {
                            user2.GetClient().SendMessage(message);
                        }
                        TimeSpan span = (TimeSpan)(DateTime.Now - this.FloodTime);
                        if (span.Seconds > 4)
                        {
                            this.FloodCount = 0;
                        }
                        if (((span.Seconds < 4) && (this.FloodCount > 5)) && (this.Session.GetHabbo().Rank < 5))
                        {
                            ServerMessage message2 = new ServerMessage(Outgoing.FloodFilter);
                            message2.AppendInt32(30);
                            roomUserByHabbo.GetClient().SendMessage(message2);
                        }
                        else
                        {
                            this.FloodTime = DateTime.Now;
                            this.FloodCount++;
                            List<RoomUser> roomUserByRank = currentRoom.GetRoomUserManager().GetRoomUserByRank(4);
                            if (roomUserByRank.Count > 0)
                            {
                                message = new ServerMessage();
                                message.Init(Outgoing.Whisp);
                                message.AppendInt32(roomUserByHabbo.VirtualId);
                                message.AppendString(LanguageLocale.GetValue("moderation.whisper") + pName + ": " + s);
                                message.AppendInt32(0);
                                message.AppendInt32(i);
                                message.AppendInt32(0);
                                message.AppendInt32(-1);
                                foreach (RoomUser user3 in roomUserByRank)
                                {
                                    if (((user3 != null) && ((user3.HabboId != user2.HabboId) && (user3.HabboId != roomUserByHabbo.HabboId))) && (user3.GetClient() != null))
                                    {
                                        user3.GetClient().SendMessage(message);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

