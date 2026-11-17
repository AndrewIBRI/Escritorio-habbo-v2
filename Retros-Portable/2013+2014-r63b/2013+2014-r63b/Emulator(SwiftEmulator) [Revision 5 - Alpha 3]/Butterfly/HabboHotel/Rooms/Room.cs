namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly;
    using Butterfly.Collections;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Catalogs;
    using Butterfly.HabboHotel.ChatMessageStorage;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Guilds;
    using Butterfly.HabboHotel.Habbo.SoundMachine;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.RoomBots;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.RoomIvokedItems;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.Messages;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Uber.HabboHotel.Rooms;

    public class Room
    {
        internal ArrayList ActiveTrades;
        internal bool AllowPets;
        internal bool AllowPetsEating;
        internal bool AllowWalkthrough;
        private Dictionary<uint, double> Bans;
        private BattleBanzai banzai;
        internal int Category;
        internal string CCTs;
        private ChatMessageManager chatMessageManager;
        private Queue chatMessageQueue;
        public int CurrentPollId;
        internal string Description;
        internal RoomEvent Event = null;
        internal bool EveryoneGotRights;
        internal string Floor;
        internal int FloorThickness;
        private Freeze freeze;
        private GameManager game;
        private GameItemHandler gameItemHandler;
        private Gamemap gamemap;
        internal int GroupId;
        internal bool guideBotIsCalled;
        private MyGuild guildRoom;
        internal bool Hidewall;
        private uint Id;
        private int IdleTime;
        private bool isCrashed = false;
        internal bool isCycling = false;
        internal string Landscape;
        internal DateTime lastTimerReset;
        private bool mCycleEnded;
        private bool mDisposed;
        internal bool mIsIdle = false;
        internal string ModelName;
        internal Butterfly.HabboHotel.Items.MoodlightData MoodlightData;
        internal bool MovedThinksObH = false;
        private Butterfly.HabboHotel.Rooms.RoomData mRoomData;
        private RoomMusicController musicController;
        internal RoomIcon myIcon;
        internal string Name;
        internal string Owner;
        internal int OwnerId;
        internal string Password;
        internal bool procesoEnCurso = false;
        internal int procesoIntentos = 0;
        internal Task procesoTask;
        private Random rnd;
        private Queue roomAlerts;
        private Queue roomBadge;
        private RoomItemHandling roomItemHandling;
        private Queue roomKick;
        private Queue roomMessages;
        internal bool RoomMuted;
        private Queue roomServerMessages;
        private RoomUserManager roomUserManager;
        internal bool sbdyVoted = false;
        internal int Score;
        private Soccer soccer;
        internal int State;
        private int tagCount;
        internal ArrayList Tags;
        internal TeamManager teambanzai;
        internal TeamManager teamfreeze;
        internal string Type;
        internal int UsersMax;
        internal int UsersNow;
        internal List<uint> UsersWithRights;
        internal bool votacionEnCurso = false;
        internal string Wallpaper;
        internal int WallThickness;
        private WiredHandler wiredHandler;
        private List<RoomBot> Bots;
        internal event RoomUserSaysDelegate OnUserSays;

        internal Room(Butterfly.HabboHotel.Rooms.RoomData Data)
        {
            this.InitializeFromRoomData(Data);
        }

        internal void AddBan(uint pId)
        {
            if (!this.Bans.ContainsKey(pId))
            {
                this.Bans.Add(pId, (double) ButterflyEnvironment.GetUnixTimestamp());
            }
        }

        internal void AddTagRange(List<string> tags)
        {
            this.tagCount += tags.Count;
            this.Tags.AddRange(tags);
        }

        internal bool AllowsShous(RoomUser user, string message)
        {
            bool messageHandled = false;
            if (this.OnUserSays != null)
            {
                this.OnUserSays(null, new UserSaysArgs(user, message), out messageHandled);
            }
            return !messageHandled;
        }

        private void BotLeaveMessage()
        {
            foreach (RoomUser user in this.roomUserManager.UserList.Values)
            {
                if (user.IsBot || user.IsPet)
                {
                    ServerMessage message = new ServerMessage(0x1d);
                    message.AppendString(user.VirtualId.ToString());
                    this.SendMessage(message);
                }
            }
        }

        internal bool CheckRights(GameClient Session)
        {
            return this.CheckRights(Session, false);
        }

        internal bool CheckRights(GameClient Session, bool RequireOwnership)
        {
            try
            {
                if ((Session == null) || (Session.GetHabbo() == null))
                {
                    return false;
                }

                if ((this.guildRoom != null) && (!RequireOwnership))
                {
                    MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(this.guildRoom.Id);

                    if ((guild.Members.ContainsKey((int)Session.GetHabbo().Id)) && (guild.RightsType == 0))
                    {
                        return true;
                    }

                    if ((guild.Members.ContainsKey((int)Session.GetHabbo().Id)) && (guild.GuildRanks[(int)Session.GetHabbo().Id] < 2) && (guild.RightsType == 1))
                    {
                        return true;
                    }
                }

                if (Session.GetHabbo().Username == this.Owner)
                {
                    return true;
                }

                if (Session.GetHabbo().HasFuse("fuse_admin") || Session.GetHabbo().HasFuse("fuse_any_room_controller"))
                {
                    return true;
                }
                if (!RequireOwnership)
                {
                    if (Session.GetHabbo().HasFuse("fuse_any_room_rights"))
                    {
                        return true;
                    }
                    if (this.UsersWithRights.Contains(Session.GetHabbo().Id))
                    {
                        return true;
                    }
                    if (this.EveryoneGotRights)
                    {
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.HandleException(exception, "Room.CheckRights");
            }
            return false;
        }

        internal bool checkGroupRights(GameClient Session)
        {
            try
            {

                if ((Session == null) || (Session.GetHabbo() == null))
                {
                    return false;
                }

                if (this.guildRoom != null)
                {
                    MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(this.guildRoom.Id);

                    if (!guild.Members.ContainsKey((int)Session.GetHabbo().Id))
                    {
                        return false;
                    }

                    if ((guild.Members.ContainsKey((int)Session.GetHabbo().Id)) && (guild.RightsType == 0))
                    {
                        return true;
                    }

                    if ((guild.Members.ContainsKey((int)Session.GetHabbo().Id)) && (guild.GuildRanks[(int)Session.GetHabbo().Id] < 2) && (guild.RightsType == 1))
                    {
                        return true;
                    }
                }

                else return false;
            }

            catch (Exception exception)
            {
                Logging.HandleException(exception, "Room.CheckRights");
            }

            return false;
        }

        internal void ClearTags()
        {
            this.Tags.Clear();
            this.tagCount = 0;
        }
        internal void InitUserBots()
        {
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT * FROM user_bots WHERE room_id = " + RoomId);
                DataTable Data = dbClient.getTable();

                if (Data == null)
                    return;

                foreach (DataRow Row in Data.Rows)
                {
                    dbClient.setQuery("SELECT text, shout, bot_id FROM bots_speech;");
                    DataTable BotSpeech = dbClient.getTable();


                    RoomBot Bot = Catalog.GenerateBotFromRow(Row);
                    List<RandomSpeech> RndSpeechList = new List<RandomSpeech>();
                    foreach (DataRow Row2 in BotSpeech.Rows)
                    {
                        RndSpeechList.Add(new RandomSpeech((string)Row2["text"], ButterflyEnvironment.EnumToBool(Row2["shout"].ToString()), Convert.ToUInt32(Row2["bot_id"])));
                    }
                    List<BotResponse> BotResponse = new List<RoomBots.BotResponse>();
                    roomUserManager.DeployBot(new RoomBot(Bot.BotId, RoomId, AIType.Generic, "freeroam", Bot.Name, Bot.Motto, Bot.Look, Bot.X, Bot.Y, (int)Bot.Z, 0, 0, 0, 0, 0, ref RndSpeechList, ref BotResponse, Bot.OwnerId), null);
                }
            }
        }
        internal RoomUser DeployBot(RoomBot Bot)
        {
            return this.roomUserManager.DeployBot(Bot, null);
        }

        internal void Destroy()
        {
            this.SendMessage(new ServerMessage(Outgoing.OutOfRoom));
            this.Dispose();
        }

        private void Dispose()
        {
            if (!this.mDisposed)
            {
                this.mDisposed = true;
                this.mCycleEnded = true;
                this.MovedThinksObH = false;
                ButterflyEnvironment.GetGame().GetRoomManager().QueueActiveRoomRemove(this.mRoomData);
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    this.GetRoomItemHandler().SaveFurniture(adapter);
                    adapter.runFastQuery("DELETE FROM room_active WHERE roomid = " + this.Id);
                }
                this.WorkRoomServerMessageThread();
                this.tagCount = 0;
                this.Tags.Clear();
                this.roomUserManager.UserList.Clear();
                this.UsersWithRights.Clear();
                this.Bans.Clear();
                foreach (RoomItem item in this.GetRoomItemHandler().mFloorItems.Values)
                {
                    item.Destroy();
                }
                foreach (RoomItem item in this.GetRoomItemHandler().mWallItems.Values)
                {
                    item.Destroy();
                }
                this.ActiveTrades.Clear();
                if (this.HasOngoingEvent)
                {
                    ButterflyEnvironment.GetGame().GetRoomManager().GetEventManager().QueueRemoveEvent(this.mRoomData, this.Event.Category);
                }
            }
        }

        internal void FlushSettings()
        {
            ServerMessage message;
            QueuedDictionary<uint, RoomItem> dictionary;
            List<ServerMessage> messages = new List<ServerMessage>();
            lock ((dictionary = this.GetRoomItemHandler().mFloorItems))
            {
                foreach (RoomItem item in this.GetRoomItemHandler().mFloorItems.Values)
                {
                    message = new ServerMessage(0x5e);
                    message.AppendString(item.Id.ToString());
                    message.AppendString("");
                    message.AppendBoolean(false);
                    messages.Add(message);
                }
            }
            lock ((dictionary = this.GetRoomItemHandler().mWallItems))
            {
                foreach (RoomItem item in this.GetRoomItemHandler().mWallItems.Values)
                {
                    message = new ServerMessage(0x54);
                    message.AppendString(item.Id.ToString());
                    message.AppendString("");
                    message.AppendBoolean(false);
                    messages.Add(message);
                }
            }
            this.SendMessage(messages);
            this.mCycleEnded = true;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                this.GetRoomItemHandler().SaveFurniture(adapter);
            }
            this.Tags.Clear();
            this.UsersWithRights.Clear();
            this.Bans.Clear();
            this.ActiveTrades.Clear();
            if (this.GotFreeze())
            {
                this.freeze = new Freeze(this);
            }
            if (this.GotBanzai())
            {
                this.banzai = new BattleBanzai(this);
            }
            if (this.GotSoccer())
            {
                this.soccer = new Soccer(this);
            }
            if (this.gameItemHandler != null)
            {
                this.gameItemHandler = new GameItemHandler(this);
            }
        }

        internal void GenerateGuild(MyGuild Guild)
        {
            this.guildRoom = Guild;
        }

        internal BattleBanzai GetBanzai()
        {
            if (this.banzai == null)
            {
                this.banzai = new BattleBanzai(this);
            }
            return this.banzai;
        }

        internal ChatMessageManager GetChatMessageManager()
        {
            return this.chatMessageManager;
        }

        internal Freeze GetFreeze()
        {
            if (this.freeze == null)
            {
                this.freeze = new Freeze(this);
            }
            return this.freeze;
        }

        internal GameItemHandler GetGameItemHandler()
        {
            if (this.gameItemHandler == null)
            {
                this.gameItemHandler = new GameItemHandler(this);
            }
            return this.gameItemHandler;
        }

        internal GameManager GetGameManager()
        {
            if (this.game == null)
            {
                this.game = new GameManager(this);
            }
            return this.game;
        }

        internal Gamemap GetGameMap()
        {
            return this.gamemap;
        }

        internal int GetRightsLevel(GameClient Session)
        {
            try
            {
                if ((Session == null) || (Session.GetHabbo() == null))
                {
                    return 0;
                }
                if (Session.GetHabbo().Username == this.Owner)
                {
                    return 4;
                }
                if (Session.GetHabbo().HasFuse("fuse_admin") || Session.GetHabbo().HasFuse("fuse_any_room_controller"))
                {
                    return 4;
                }
                if (Session.GetHabbo().HasFuse("fuse_any_room_rights"))
                {
                    return 3;
                }
                if (this.UsersWithRights.Contains(Session.GetHabbo().Id))
                {
                    return 1;
                }
                if (this.EveryoneGotRights)
                {
                    return 1;
                }
            }
            catch (Exception exception)
            {
                Logging.HandleException(exception, "GetRightsLevel");
            }
            return 0;
        }

        

        internal MyGuild GetRoomGuild()
        {
            return this.guildRoom;
        }

        internal RoomItemHandling GetRoomItemHandler()
        {
            return this.roomItemHandling;
        }

        internal RoomMusicController GetRoomMusicController()
        {
            if (this.musicController == null)
            {
                this.musicController = new RoomMusicController();
            }
            return this.musicController;
        }

        internal RoomUserManager GetRoomUserManager()
        {
            return this.roomUserManager;
        }

        internal Soccer GetSoccer()
        {
            if (this.soccer == null)
            {
                this.soccer = new Soccer(this);
            }
            return this.soccer;
        }

        internal TeamManager GetTeamManagerForBanzai()
        {
            if (this.teambanzai == null)
            {
                this.teambanzai = TeamManager.createTeamforGame("banzai");
            }
            return this.teambanzai;
        }

        internal TeamManager GetTeamManagerForFreeze()
        {
            if (this.teamfreeze == null)
            {
                this.teamfreeze = TeamManager.createTeamforGame("freeze");
            }
            return this.teamfreeze;
        }

        internal Trade GetUserTrade(uint UserId)
        {
            foreach (Trade trade in this.ActiveTrades.ToArray())
            {
                if (trade.ContainsUser(UserId))
                {
                    return trade;
                }
            }
            return null;
        }

        internal WiredHandler GetWiredHandler()
        {
            if (this.wiredHandler == null)
            {
                this.wiredHandler = new WiredHandler(this);
            }
            return this.wiredHandler;
        }

        internal bool GotBanzai()
        {
            return (this.banzai != null);
        }

        internal bool GotFreeze()
        {
            return (this.freeze != null);
        }

        internal bool GotMusicController()
        {
            return (this.musicController != null);
        }

        internal bool GotSoccer()
        {
            return (this.soccer != null);
        }

        internal bool GotWired()
        {
            return (this.wiredHandler != null);
        }

        internal bool HasActiveTrade(RoomUser User)
        {
            if (User.IsBot)
            {
                return false;
            }
            return this.HasActiveTrade(User.GetClient().GetHabbo().Id);
        }

        internal bool HasActiveTrade(uint UserId)
        {
            foreach (Trade trade in this.ActiveTrades.ToArray())
            {
                if (trade.ContainsUser(UserId))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool HasBanExpired(uint pId)
        {
            if (!this.UserIsBanned(pId))
            {
                return true;
            }
            double num = ButterflyEnvironment.GetUnixTimestamp() - this.Bans[pId];
            return (num > 900.0);
        }

        private void Initialize(uint Id, string Name, string Description, string Type, string Owner, int OwnerId, int Category, int State, int UsersMax, string ModelName, string CCTs, int Score, List<string> pTags, bool AllowPets, bool AllowPetsEating, bool AllowWalkthrough, bool Hidewall, RoomIcon Icon, string Password, string Wallpaper, string Floor, string Landscape, Butterfly.HabboHotel.Rooms.RoomData RoomData, bool RightOverride, int walltickness, int floorthickness, int groupid)
        {
            this.mDisposed = false;
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.Owner = Owner;
            this.OwnerId = OwnerId;
            this.Category = Category;
            this.Type = Type;
            this.State = State;
            this.UsersNow = 0;
            this.UsersMax = UsersMax;
            this.ModelName = ModelName;
            this.CCTs = CCTs;
            this.Score = Score;
            this.GroupId = groupid;
            this.tagCount = 0;
            this.Tags = new ArrayList();
            
            foreach (string str in pTags)
            {
                this.tagCount++;
                this.Tags.Add(str);
            }
            
            this.AllowPets = AllowPets;
            this.AllowPetsEating = AllowPetsEating;
            this.AllowWalkthrough = AllowWalkthrough;
            this.Hidewall = Hidewall;
            this.myIcon = Icon;
            this.Password = Password;
            this.Bans = new Dictionary<uint, double>();
            this.Wallpaper = Wallpaper;
            this.Floor = Floor;
            this.Landscape = Landscape;
            this.chatMessageManager = new ChatMessageManager();
            this.ActiveTrades = new ArrayList();
            this.mCycleEnded = false;
            this.mRoomData = RoomData;
            this.EveryoneGotRights = RightOverride;
            this.roomMessages = new Queue();
            this.chatMessageQueue = new Queue();
            this.rnd = new Random();
            this.roomMessages = new Queue();
            this.roomAlerts = new Queue();
            this.roomBadge = new Queue();
            this.roomKick = new Queue();
            this.roomServerMessages = new Queue();
            this.IdleTime = 0;
            this.RoomMuted = false;
            this.WallThickness = walltickness;
            this.FloorThickness = floorthickness;
            
            if (this.GroupId > 0)
            {
                this.guildRoom = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(this.GroupId);
            }
            
            this.gamemap = new Gamemap(this);
            this.roomItemHandling = new RoomItemHandling(this);
            this.roomUserManager = new RoomUserManager(this);
            this.wiredHandler = new WiredHandler(this);
            this.LoadRights();
            this.GetRoomItemHandler().LoadFurniture();
            this.wiredHandler.LoadWired();
            this.GetGameMap().GenerateMaps(true);
            this.LoadMusic();
            
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MySQL)
                {
                    adapter.runFastQuery("REPLACE INTO room_active VALUES (" + Id + ",1)");
                }
                else
                {
                    adapter.runFastQuery(string.Concat(new object[] { "IF EXISTS (SELECT roomid FROM room_active WHERE roomid = ", Id, ") UPDATE room_active SET active_users = 1 WHERE roomid = ", Id, " ELSE INSERT INTO room_active VALUES (", Id, ",1)" }));
                }
            }

            ButterflyEnvironment.GetGame().GetRoomManager().QueueActiveRoomAdd(this.mRoomData);
        }

        private void InitializeFromRoomData(Butterfly.HabboHotel.Rooms.RoomData Data)
        {
            this.Initialize(Data.Id, Data.Name, Data.Description, Data.Type, Data.Owner, Data.OwnerId, Data.Category, Data.State, Data.UsersMax, Data.ModelName, Data.CCTs, Data.Score, Data.Tags, Data.AllowPets, Data.AllowPetsEating, Data.AllowWalkthrough, Data.Hidewall, Data.Icon, Data.Password, Data.Wallpaper, Data.Floor, Data.Landscape, Data, Data.AllowRightsOverride, Data.WallThickness, Data.FloorThickness, Data.GroupId);
        }

        internal void InitPets()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM user_pets WHERE room_id = " + this.RoomId);
                DataTable table = adapter.getTable();
                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Pet petData = Catalog.GeneratePetFromRow(row);
                        List<RandomSpeech> speeches = new List<RandomSpeech>();
                        List<BotResponse> responses = new List<BotResponse>();
                        this.roomUserManager.DeployBot(new RoomBot(petData.PetId, this.RoomId, AIType.Pet, "freeroam", petData.Name, "", petData.Look, petData.X, petData.Y, (int) petData.Z, 0, 0, 0, 0, 0, ref speeches, ref responses, 0), petData);
                    }
                }
            }

            
        }

        internal void InitBots()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                Bots = new List<RoomBot>();
                
                adapter.setQuery("SELECT id, room_id, ai_type, walk_mode, name, motto, look, x, y, z, rotation, min_x, min_y, max_x, max_y FROM bots WHERE room_id="+RoomId+";");
                DataTable Data = adapter.getTable();

                adapter.setQuery("SELECT id, bot_id, keywords, response_text, mode, serve_id FROM bots_responses;");
                DataTable BotResponses = adapter.getTable();

                adapter.setQuery("SELECT text, shout, bot_id FROM bots_speech;");
                DataTable BotSpeech = adapter.getTable();

                List<BotResponse> Responses = new List<BotResponse>();
                List<RandomSpeech> Speeches = new List<RandomSpeech>();

                foreach (DataRow Row in BotResponses.Rows)
                {
                    Responses.Add(new BotResponse(Convert.ToUInt32(Row["bot_id"]), (string)Row["keywords"], (string)Row["response_text"], Row["mode"].ToString(), (int)Row["serve_id"]));
                }

                foreach (DataRow Row in BotSpeech.Rows)
                {
                    Speeches.Add(new RandomSpeech((string)Row["text"], ButterflyEnvironment.EnumToBool(Row["shout"].ToString()), Convert.ToUInt32(Row["bot_id"])));
                }


                if (Data == null)
                {
                    return;
                }

                foreach (DataRow Row in Data.Rows)
                {
                    
                    string BotAI = (string)Row["ai_type"];
                    AIType BotAIType;
                    switch (BotAI)
                    {
                        case "generic":
                            BotAIType = AIType.Generic;
                            break;
                        case "guide":
                            BotAIType = AIType.Guide;
                            break;
                        case "pet":
                            BotAIType = AIType.Pet;
                            break;
                        default:
                            BotAIType = AIType.Generic;
                            break;
                    }
                    
                    Bots.Add(new RoomBot(Convert.ToUInt32(Row["id"]), Convert.ToUInt32(Row["room_id"]), BotAIType, (string)Row["walk_mode"],
                        (String)Row["name"], (string)Row["motto"], (String)Row["look"], (int)Row["x"], (int)Row["y"], (int)Row["z"],
                        (int)Row["rotation"], (int)Row["min_x"], (int)Row["min_y"], (int)Row["max_x"], (int)Row["max_y"], ref Speeches, ref Responses, 0));
                }

            }
        }

        public void LoadBots()
        {
            InitBots();
            foreach (RoomBot bot in Bots)
            {
                RoomUser NewUser = DeployBot(bot);
                NewUser.SetPos(bot.X, bot.Y, bot.Z);
            }
        }

        internal void LoadMusic()
        {
            DataTable table;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM items_rooms_songs WHERE roomid = '" + this.RoomId + "'");
                table = adapter.getTable();
            }
            foreach (DataRow row in table.Rows)
            {
                int songID = (int) row["songid"];
                uint num2 = (uint) row["itemid"];
                int baseItem = (int) row["baseitem"];
                SongItem diskItem = new SongItem((int) num2, songID, baseItem);
                this.GetRoomMusicController().AddDisk(diskItem);
            }
        }

        internal void LoadRights()
        {
            this.UsersWithRights = new List<uint>();
            DataTable table = new DataTable();
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT room_rights.user_id FROM room_rights WHERE room_id = " + this.Id);
                table = adapter.getTable();
            }
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    this.UsersWithRights.Add(Convert.ToUInt32(row["user_id"]));
                }
            }
        }

        internal void onReload()
        {
            List<RoomUser> roomUsers = this.GetRoomUserManager().GetRoomUsers();
            Hashtable userID = this.GetRoomUserManager().usersByUsername.Clone() as Hashtable;
            Hashtable userName = this.GetRoomUserManager().usersByUsername.Clone() as Hashtable;
            int primaryCounter = 0;
            int secondaryCounter = 0;
            this.GetRoomUserManager().backupCounters(ref primaryCounter, ref secondaryCounter);
            this.FlushSettings();
            this.ReloadSettings();
            this.GetRoomUserManager().UpdateUserStats(roomUsers, userID, userName, primaryCounter, secondaryCounter);
            this.UpdateFurniture();
            this.GetGameMap().GenerateMaps(true);
        }

        private void OnRoomCrash(Exception e)
        {
            Logging.LogThreadException(e.ToString(), "Room cycle task for room " + this.RoomId);
            try
            {
                if (Debugger.IsAttached)
                {
                    foreach (RoomUser user in this.roomUserManager.UserList.Values)
                    {
                        if (!user.IsBot && !user.IsPet)
                        {
                            user.GetClient().SendNotif("Unhandled exception in room: " + e.ToString());
                            try
                            {
                                this.GetRoomUserManager().RemoveUserFromRoom(user.GetClient(), true, false);
                                user.GetClient().CurrentRoomUserID = -1;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            ButterflyEnvironment.GetGame().GetRoomManager().UnloadRoom(this);
            this.isCrashed = true;
        }

        internal void onRoomKick()
        {
            foreach (RoomUser user in this.roomUserManager.UserList.Values)
            {
                if (user.GetClient().GetHabbo().Rank < 4)
                {
                    this.GetRoomUserManager().RemoveUserFromRoom(user.GetClient(), true, true);
                    user.GetClient().CurrentRoomUserID = -1;
                }
            }
        }

        internal void OnUserSay(RoomUser User, string Message, bool Shout)
        {
            foreach (RoomUser user in this.roomUserManager.UserList.Values)
            {
                if (user.IsBot)
                {
                    if (Shout)
                    {
                        user.BotAI.OnUserShout(User, Message);
                    }
                    else
                    {
                        user.BotAI.OnUserSay(User, Message);
                    }
                }
            }
        }

        internal void ProcessRoom()
        {
            try
            {
                if (!this.isCrashed && !this.mDisposed)
                {
                    try
                    {
                        int idleCount = 0;
                        this.GetRoomItemHandler().OnCycle();
                        this.GetRoomUserManager().OnCycle(ref idleCount);
                        if (this.musicController != null)
                        {
                            this.musicController.Update(this);
                        }
                        if (idleCount > 0)
                        {
                            this.IdleTime++;
                        }
                        else
                        {
                            this.IdleTime = 0;
                        }
                        if (!this.mCycleEnded)
                        {
                            if (this.IdleTime >= 10)
                            {
                                ButterflyEnvironment.GetGame().GetRoomManager().UnloadRoom(this);
                                this.mIsIdle = false;
                                return;
                            }
                            ServerMessage message = this.GetRoomUserManager().SerializeStatusUpdates(false);
                            if (message != null)
                            {
                                this.SendMessage(message);
                            }
                        }
                        if (this.gameItemHandler != null)
                        {
                            this.gameItemHandler.OnCycle();
                        }
                        if (this.game != null)
                        {
                            this.game.OnCycle();
                        }
                        if (this.GotBanzai())
                        {
                            this.banzai.OnCycle();
                        }
                        if (this.GotSoccer())
                        {
                            this.soccer.OnCycle();
                        }
                        if (this.wiredHandler != null)
                        {
                            this.wiredHandler.OnCycle();
                        }
                        this.roomUserManager.UserList.OnCycle();
                        this.WorkRoomAlertQueue();
                        this.WorkRoomBadgeQueue();
                        this.WorkRoomKickQueue();
                        this.WorkChatQueue();
                        this.WorkRoomServerMessageThread();
                    }
                    catch (Exception exception1)
                    {
                        Exception e = exception1;
                        this.OnRoomCrash(e);
                    }
                }
            }
            catch (Exception exception2)
            {
                Logging.LogCriticalException("Sub crash in room cycle: " + exception2.ToString());
            }
        }

        internal void QueueChatMessage(InvokedChatMessage message)
        {
            lock (this.chatMessageQueue.SyncRoot)
            {
                this.chatMessageQueue.Enqueue(message);
            }
        }

        internal void QueueRoomAlert(RoomAlert alert)
        {
            lock (this.roomAlerts.SyncRoot)
            {
                this.roomAlerts.Enqueue(alert);
            }
        }

        internal void QueueRoomBadge(string badge)
        {
            lock (this.roomBadge.SyncRoot)
            {
                this.roomBadge.Enqueue(badge);
            }
        }

        internal void QueueRoomKick(RoomKick kick)
        {
            lock (this.roomKick.SyncRoot)
            {
                this.roomKick.Enqueue(kick);
            }
        }

        internal void QueueRoomMessage(ServerMessage message)
        {
            lock (this.roomServerMessages.SyncRoot)
            {
                this.roomServerMessages.Enqueue(message.GetBytes());
            }
        }

        internal void ReloadSettings()
        {
            Butterfly.HabboHotel.Rooms.RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(this.RoomId);
            this.InitializeFromRoomData(data);
            this.InitPets();
            this.InitUserBots();
            //this.LoadBots();
            
        }

        internal void RemoveBan(uint pId)
        {
            this.Bans.Remove(pId);
        }

        internal void RequestReload()
        {
        }

        internal void SendLeaveMessageForBots()
        {
            onCycleDoneDelegate function = new onCycleDoneDelegate(this.BotLeaveMessage);
            this.roomUserManager.UserList.QueueDelegate(function);
        }

        internal void SendMessage(ServerMessage Message)
        {
            try
            {
                if (Message != null)
                {
                    byte[] bytes = Message.GetBytes();
                    lock (this.roomServerMessages.SyncRoot)
                    {
                        this.roomServerMessages.Enqueue(bytes);
                        //Console.WriteLine("Inviato -> " + Message.Id);
                    }
                }
            }
            catch (InvalidOperationException exception)
            {
                Logging.HandleException(exception, "Room.SendMessage");
            }
        }

        internal void SendMessage(List<ServerMessage> Messages)
        {
            if (Messages.Count != 0)
            {
                try
                {
                    byte[] array = new byte[0];
                    int index = 0;
                    foreach (ServerMessage message in Messages)
                    {
                        byte[] bytes = message.GetBytes();
                        int newSize = array.Length + bytes.Length;
                        Array.Resize<byte>(ref array, newSize);
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            array[index] = bytes[i];
                            index++;
                        }
                        //Console.WriteLine("Inviato -> " + message.Id);
                    }
                    lock (this.roomServerMessages.SyncRoot)
                    {
                        this.roomServerMessages.Enqueue(array);
                    }
                }
                catch (Exception exception)
                {
                    Logging.HandleException(exception, "Room.SendMessage List<ServerMessage>");
                }
            }
        }

        internal void SendMessageToUsersWithRights(ServerMessage Message)
        {
            Exception exception;
            try
            {
                byte[] bytes = Message.GetBytes();
                foreach (RoomUser user in this.roomUserManager.UserList.Values)
                {
                    if (!user.IsBot)
                    {
                        GameClient session = user.GetClient();
                        if ((session != null) && this.CheckRights(session))
                        {
                            try
                            {
                                session.GetConnection().SendData(bytes);
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                                Logging.HandleException(exception, "Room.SendMessageToUsersWithRights");
                            }
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                Logging.HandleException(exception, "Room.SendMessageToUsersWithRights");
            }
        }

        internal void SetMaxUsers(int MaxUsers)
        {
            this.UsersMax = MaxUsers;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "UPDATE rooms SET users_max = ", MaxUsers, " WHERE id = ", this.RoomId }));
            }
        }

        public static void ShowResults(Room Room, int QuestionId, GameClient Session)
        {
            try
            {
                string str;
                IQueryAdapter adapter;
                int num2;
                Thread.Sleep(0x7530);
                DataTable table = null;
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
                ServerMessage message = new ServerMessage(0xbe2);
                message.AppendString(str);
                message.AppendInt32(table.Rows.Count);
                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        int num;
                        message.AppendInt32((int) row["id"]);
                        message.AppendString((string) row["answer_text"]);
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.setQuery(string.Concat(new object[] { "SELECT COUNT(*) FROM infobus_results WHERE answer_id = '", (int) row["id"], "' AND question_id = '", QuestionId, "'" }));
                            num = adapter.getInteger();
                        }
                        message.AppendInt32(num);
                    }
                }
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("SELECT COUNT(*) FROM infobus_results WHERE question_id = '" + QuestionId + "'");
                    num2 = adapter.getInteger();
                }
                message.AppendInt32(num2);
                Room.SendMessage(message);
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("DELETE FROM infobus_results WHERE question_id = '" + QuestionId + "'");
                    adapter.runQuery();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteLine("ERR: " + exception.Message);
            }
        }

        internal void TryStartTrade(RoomUser UserOne, RoomUser UserTwo)
        {
            if (((((UserOne != null) && (UserTwo != null)) && (!UserOne.IsBot && !UserTwo.IsBot)) && ((!UserOne.IsTrading && !UserTwo.IsTrading) && !this.HasActiveTrade(UserOne))) && !this.HasActiveTrade(UserTwo))
            {
                this.ActiveTrades.Add(new Trade(UserOne.GetClient().GetHabbo().Id, UserTwo.GetClient().GetHabbo().Id, this.RoomId));
            }
        }

        internal void TryStopTrade(uint UserId)
        {
            Trade userTrade = this.GetUserTrade(UserId);
            if (userTrade != null)
            {
                userTrade.CloseTrade(UserId);
                this.ActiveTrades.Remove(userTrade);
            }
        }

        internal void UpdateFurniture()
        {
            ServerMessage message;
            List<ServerMessage> messages = new List<ServerMessage>();
            RoomItem[] array = this.GetRoomItemHandler().mFloorItems.Values.ToArray<RoomItem>();
            foreach (RoomItem item in array)
            {
                message = new ServerMessage(0x5d);
                item.Serialize(message, (int)item.placedBy);
                messages.Add(message);
            }
            Array.Clear(array, 0, array.Length);
            array = null;
            RoomItem[] itemArray2 = this.GetRoomItemHandler().mWallItems.Values.ToArray<RoomItem>();
            foreach (RoomItem item in itemArray2)
            {
                message = new ServerMessage(0x53);
                item.Serialize(message, (int)item.placedBy);
                messages.Add(message);
            }
            Array.Clear(itemArray2, 0, itemArray2.Length);
            itemArray2 = null;
            this.SendMessage(messages);
        }

        internal ServerMessage UpdateRoomState()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomData);
            message.AppendBoolean(true);
            message.AppendInt32(this.Id);
            message.AppendString(this.Name);
            message.AppendBoolean(true);
            message.AppendInt32(this.OwnerId);
            message.AppendString(this.Owner);
            message.AppendInt32(this.State);
            message.AppendInt32(this.UsersNow);
            message.AppendInt32(this.UsersMax);
            message.AppendString(this.Description);
            message.AppendInt32(0);
            message.AppendInt32((this.Category == 0x34) ? 2 : 0);
            message.AppendInt32(this.Score);
            message.AppendInt32(0);
            message.AppendInt32(this.Category);
            if (this.GroupId == 0)
            {
                message.AppendInt32(0);
                message.AppendInt32(0);
            }
            else
            {
                MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(this.GroupId);
                message.AppendInt32(guild.Id);
                message.AppendString(guild.Name);
                message.AppendString(guild.Image);
            }
            message.AppendString("");
            message.AppendInt32(this.TagCount);
            foreach (string str in this.Tags.ToArray())
            {
                message.AppendString(str);
            }
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendBoolean(true);
            message.AppendBoolean(true);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendBoolean(false);
            message.AppendBoolean(false);
            message.AppendBoolean(false);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendBoolean(false);
            message.AppendBoolean(true);
            return message;
        }

        internal bool UserIsBanned(uint pId)
        {
            return this.Bans.ContainsKey(pId);
        }

        private void WorkChatQueue()
        {
            if (this.chatMessageQueue.Count > 0)
            {
                lock (this.chatMessageQueue.SyncRoot)
                {
                    while (this.chatMessageQueue.Count > 0)
                    {
                        InvokedChatMessage message = (InvokedChatMessage) this.chatMessageQueue.Dequeue();
                        message.user.OnChat(message);
                    }
                }
            }
        }

        private void WorkRoomAlertQueue()
        {
            if (this.roomAlerts.Count > 0)
            {
                lock (this.roomAlerts.SyncRoot)
                {
                    while (this.roomAlerts.Count > 0)
                    {
                        RoomAlert alert = (RoomAlert) this.roomAlerts.Dequeue();
                        foreach (RoomUser user in this.roomUserManager.UserList.Values)
                        {
                            if (!user.IsBot && (user.GetClient().GetHabbo().Rank < alert.minrank))
                            {
                                user.GetClient().SendNotif(alert.message, false);
                            }
                        }
                    }
                }
            }
        }

        private void WorkRoomBadgeQueue()
        {
            if (this.roomBadge.Count > 0)
            {
                lock (this.roomBadge.SyncRoot)
                {
                    while (this.roomBadge.Count > 0)
                    {
                        string badge = (string) this.roomBadge.Dequeue();
                        foreach (RoomUser user in this.roomUserManager.UserList.Values)
                        {
                            try
                            {
                                if ((!user.IsBot && (user.GetClient() != null)) && (user.GetClient().GetHabbo() != null))
                                {
                                    user.GetClient().GetHabbo().GetBadgeComponent().GiveBadge(badge, true);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        private void WorkRoomKickQueue()
        {
            if (this.roomKick.Count > 0)
            {
                lock (this.roomKick.SyncRoot)
                {
                    while (this.roomKick.Count > 0)
                    {
                        RoomKick kick = (RoomKick) this.roomKick.Dequeue();
                        List<RoomUser> list = new List<RoomUser>();
                        foreach (RoomUser user in this.roomUserManager.UserList.Values)
                        {
                            if (!user.IsBot && (user.GetClient().GetHabbo().Rank < kick.minrank))
                            {
                                if (kick.allert.Length > 0)
                                {
                                    user.GetClient().SendNotif(LanguageLocale.GetValue("roomkick.allert") + kick.allert);
                                }
                                list.Add(user);
                            }
                        }
                        foreach (RoomUser user2 in list)
                        {
                            this.GetRoomUserManager().RemoveUserFromRoom(user2.GetClient(), true, false);
                            user2.GetClient().CurrentRoomUserID = -1;
                        }
                    }
                }
            }
        }

        private void WorkRoomServerMessageThread()
        {
            if (this.roomServerMessages.Count > 0)
            {
                List<byte> list = new List<byte>();
                lock (this.roomServerMessages.SyncRoot)
                {
                    while (this.roomServerMessages.Count > 0)
                    {
                        list.AddRange((byte[]) this.roomServerMessages.Dequeue());
                    }
                }
                byte[] packet = list.ToArray();
                foreach (RoomUser user in this.roomUserManager.UserList.Values)
                {
                    if (!user.IsBot)
                    {
                        GameClient client = user.GetClient();
                        if ((client != null) && (client.GetConnection() != null))
                        {
                            client.GetConnection().SendData(packet);
                        }
                    }
                }
                list.Clear();
                packet = null;
                list = null;
            }
        }

        internal bool CanTradeInRoom
        {
            get
            {
                if (this.IsPublic)
                {
                    return false;
                }
                return true;
            }
        }

        internal bool HasOngoingEvent
        {
            get
            {
                return (this.Event != null);
            }
        }

        internal RoomIcon Icon
        {
            get
            {
                return this.myIcon;
            }
            set
            {
                this.myIcon = value;
            }
        }

        internal bool IsPublic
        {
            get
            {
                return (this.Type == "public");
            }
        }

        internal Butterfly.HabboHotel.Rooms.RoomData RoomData
        {
            get
            {
                return this.mRoomData;
            }
        }

        internal uint RoomId
        {
            get
            {
                return this.Id;
            }
        }

        internal int TagCount
        {
            get
            {
                return this.Tags.Count;
            }
        }

        internal int UserCount
        {
            get
            {
                return this.roomUserManager.GetRoomUserCount();
            }
        }
    }
}

