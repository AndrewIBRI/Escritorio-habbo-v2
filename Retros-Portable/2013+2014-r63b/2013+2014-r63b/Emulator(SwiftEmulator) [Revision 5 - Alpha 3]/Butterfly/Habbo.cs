namespace Butterfly.HabboHotel.Habbo.Users
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Achievements;
    using Butterfly.HabboHotel.ChatMessageStorage;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Games.SnowStorm;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Users.Badges;
    using Butterfly.HabboHotel.Users.Inventory;
    using Butterfly.HabboHotel.Users.Messenger;
    using Butterfly.HabboHotel.Users.Subscriptions;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using Butterfly.Messages;
    using Butterfly.ServerManager;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    internal class Habbo
    {
        internal int AchievementPoints;
        internal Dictionary<string, UserAchievement> Achievements;
        public bool ActiveQuestNow = false;
        internal int ActivityPoints;
        private Butterfly.HabboHotel.Users.Inventory.AvatarEffectsInventoryComponent AvatarEffectsInventoryComponent;
        private Butterfly.HabboHotel.Users.Badges.BadgeComponent BadgeComponent;
        private Butterfly.HabboHotel.Users.Relationship.RelationshipComposer RelationshipComposer;
        internal int buyItemLoop = 1;
        internal bool CalledGuideBot;
        internal bool CanChangeName;
        private ChatMessageManager chatMessageManager;
        public bool ColorsSended = false;
        internal int Credits;
        internal int Crystals;
        internal uint CurrentRoomId;
        internal int DailyPetRespectPoints;
        internal int DailyRespectPoints;
        internal bool Disconnected;
        public int FavoriteGuild;
        internal ArrayList FavoriteRooms;
        internal int forceRot = -1;
        internal string Gender;
        private bool HabboinfoSaved = false;
        internal bool HasFriendRequestsDisabled;
        internal uint HomeRoom;
        internal uint Id;
        public List<int> ImGuilds = new List<int>();
        private Butterfly.HabboHotel.Users.Inventory.InventoryComponent InventoryComponent;
        internal bool IsTeleporting;
        internal double LastActivityPointsUpdate;
        internal string LastOnline;
        public int LastQuestId = 0;
        internal bool LoadingChecksPassed;
        internal uint LoadingRoom;
        internal string Look;
        private GameClient mClient;
        private HabboMessenger Messenger;
        internal string Motto;
        internal bool MutantPenalty;
        internal bool Muted;
        internal List<uint> MutedUsers;
        public string MyQuests;
        internal bool PassedQuiz;
        internal int PointsOnline;
        public int QuestActivate = 0;
        public List<int> QuestsCompleted;
        public string QuestsS;
        public Dictionary<int, int> QuestsStates;
        internal uint Rank;
        internal List<uint> RatedRooms;
        internal string RealName;
        internal int Respect;
        public int SnowLevel = 0;
        public bool SnowPing = false;
        public int SnowPoints = 0;
        public int SnowStep = 0;
        public int SnowTeam = 0;
        public int SnowUserId;
        public Butterfly.HabboHotel.Games.SnowStorm.SnowStorm SnowWar = null;
        public int SnowX;
        public int SnowY;
        internal DateTime spamFloodTime;
        internal int spamProtectionAbuse = 0;
        internal bool spamProtectionBol = false;
        internal int spamProtectionCount = 1;
        internal int spamProtectionTime = 0;
        internal bool SpectatorMode;
        private Butterfly.HabboHotel.Users.Subscriptions.SubscriptionManager SubscriptionManager;
        internal List<string> Tags;
        internal uint TeleporterId;
        internal uint TeleportingRoomID;
        internal string tempGender;
        internal string tempLook;
        internal string Username;
        internal List<RoomData> UsersRooms;
        internal List<RoomData> visitedRooms = new List<RoomData>();

        internal Habbo(uint Id, string Username, string RealName, uint Rank, string Motto, string Look, string Gender, int Credits, int ActivityPoints, int Crystals, double LastActivityPointsUpdate, bool Muted, uint HomeRoom, int Respect, int DailyRespectPoints, int DailyPetRespectPoints, bool MutantPenalty, bool HasFriendRequestsDisabled, DataRow groupRow, int achievementPoints, string LastOnline, string quests, string queststates, bool canchangename, int favoriteGroup, bool PassedQuiz, int pointsOnline)
        {
            this.Id = Id;
            this.Username = Username;
            this.RealName = RealName;
            this.Rank = Rank;
            this.Motto = Motto;
            this.Look = ButterflyEnvironment.FilterFigure(Look.ToLower());
            this.Gender = Gender.ToLower();
            this.Credits = Credits;
            this.Crystals = Crystals;
            this.PointsOnline = pointsOnline;
            this.PassedQuiz = PassedQuiz;
            this.ActivityPoints = ActivityPoints;
            this.AchievementPoints = achievementPoints;
            this.LastActivityPointsUpdate = LastActivityPointsUpdate;
            this.Muted = Muted;
            this.LoadingRoom = 0;
            this.LoadingChecksPassed = false;
            this.CurrentRoomId = 0;
            this.HomeRoom = HomeRoom;
            this.FavoriteRooms = new ArrayList();
            this.MutedUsers = new List<uint>();
            this.Tags = new List<string>();
            this.Achievements = new Dictionary<string, UserAchievement>();
            this.RatedRooms = new List<uint>();
            this.Respect = Respect;
            this.DailyRespectPoints = DailyRespectPoints;
            this.DailyPetRespectPoints = DailyPetRespectPoints;
            this.CalledGuideBot = false;
            this.MutantPenalty = MutantPenalty;
            this.IsTeleporting = false;
            this.TeleporterId = 0;
            this.UsersRooms = new List<RoomData>();
            this.HasFriendRequestsDisabled = HasFriendRequestsDisabled;
            this.LastOnline = DateTime.Now.ToString();
            this.CanChangeName = canchangename;
            this.MyQuests = quests;
            this.QuestsS = queststates;
            this.QuestsStates = new Dictionary<int, int>();
            if (this.QuestsS.Length > 0)
            {
                foreach (string str in this.QuestsS.Split(new char[] { ';' }))
                {
                    try
                    {
                        string[] strArray = str.Split(new char[] { '-' });
                        this.QuestsStates.Add(int.Parse(strArray[0].ToString()), int.Parse(strArray[1].ToString()));
                    }
                    catch
                    {
                    }
                }
            }
            this.QuestsCompleted = new List<int>();
            if (this.MyQuests.Length > 0)
            {
                foreach (string str2 in this.MyQuests.Split(new char[] { ';' }))
                {
                    try
                    {
                        this.QuestsCompleted.Add(int.Parse(str2));
                    }
                    catch
                    {
                    }
                }
            }
            this.FavoriteGuild = favoriteGroup;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM user_groups WHERE UserId = '" + this.Id + "'");
                DataTable table = adapter.getTable();
                foreach (DataRow row in table.Rows)
                {
                    this.ImGuilds.Add((int) row["GroupId"]);
                }
            }
            if (this.DailyPetRespectPoints > 3)
            {
                this.DailyPetRespectPoints = 3;
            }
            if (this.DailyRespectPoints > 3)
            {
                this.DailyRespectPoints = 3;
            }
        }

        internal UserAchievement GetAchievementData(string p)
        {
            UserAchievement achievement = null;
            this.Achievements.TryGetValue(p, out achievement);
            return achievement;
        }

        internal Butterfly.HabboHotel.Users.Inventory.AvatarEffectsInventoryComponent GetAvatarEffectsInventoryComponent()
        {
            return this.AvatarEffectsInventoryComponent;
        }

        internal Butterfly.HabboHotel.Users.Badges.BadgeComponent GetBadgeComponent()
        {
            return this.BadgeComponent;
        }
        internal Butterfly.HabboHotel.Users.Relationship.RelationshipComposer GetRelationshipComposer()
        {
            return this.RelationshipComposer;
        }

        internal ChatMessageManager GetChatMessageManager()
        {
            return this.chatMessageManager;
        }

        internal GameClient GetClient()
        {
            return ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(this.Id);
        }

        internal Butterfly.HabboHotel.Users.Inventory.InventoryComponent GetInventoryComponent()
        {
            return this.InventoryComponent;
        }

        internal HabboMessenger GetMessenger()
        {
            return this.Messenger;
        }

        internal Butterfly.HabboHotel.Users.Subscriptions.SubscriptionManager GetSubscriptionManager()
        {
            return this.SubscriptionManager;
        }

        internal void GiveUserCrystals(int p)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET crystals = crystals + ", p, " WHERE id = '", this.Id, "'" }));
            }
        }

        internal bool HasFuse(string Fuse)
        {
            if (ButterflyEnvironment.GetGame().GetRoleManager().RankHasRight(this.Rank, Fuse))
            {
                return true;
            }
            foreach (string str in this.GetSubscriptionManager().SubList)
            {
                if (ButterflyEnvironment.GetGame().GetRoleManager().SubHasRight(str, Fuse))
                {
                    return true;
                }
            }
            return false;
        }

        internal void Init(GameClient client, UserData data)
        {
            this.mClient = client;
            this.SubscriptionManager = new Butterfly.HabboHotel.Users.Subscriptions.SubscriptionManager(this.Id, data);
            this.BadgeComponent = new Butterfly.HabboHotel.Users.Badges.BadgeComponent(this.Id, data);
            this.RelationshipComposer = new Butterfly.HabboHotel.Users.Relationship.RelationshipComposer(this.Id, data);
            this.InventoryComponent = InventoryGlobal.GetInventory(this.Id, client, data);
            this.InventoryComponent.SetActiveState(client);
            this.AvatarEffectsInventoryComponent = new Butterfly.HabboHotel.Users.Inventory.AvatarEffectsInventoryComponent(this.Id, client, data);
            this.chatMessageManager = new ChatMessageManager();
            ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().WhenLoadClient(client);
            this.Messenger = new HabboMessenger(this.Id);
            this.Messenger.Init(data.friends, data.requests);
            this.SpectatorMode = false;
            this.Disconnected = false;
            this.UsersRooms = data.rooms;
        }

        internal void InitInformation(UserData data)
        {
            this.SubscriptionManager = new Butterfly.HabboHotel.Users.Subscriptions.SubscriptionManager(this.Id, data);
            this.BadgeComponent = new Butterfly.HabboHotel.Users.Badges.BadgeComponent(this.Id, data);
            this.chatMessageManager = new ChatMessageManager();
            this.RelationshipComposer = new Butterfly.HabboHotel.Users.Relationship.RelationshipComposer(this.Id, data);
            this.Messenger = new HabboMessenger(this.Id);
            this.Messenger.Init(data.friends, data.requests);
            this.SpectatorMode = false;
            this.Disconnected = false;
            this.UsersRooms = data.rooms;
        }

        internal void InitMessenger()
        {
            GameClient client = this.GetClient();
            if (client != null)
            {
                this.Messenger.OnStatusChanged(false);
                client.SendMessage(this.Messenger.SerializeFriends());
                client.SendMessage(this.Messenger.SerializeRequests());
                if (this.Messenger.requests.Count > ButterflyEnvironment.friendRequestLimit)
                {
                    client.SendNotif(string.Concat(new object[] { "M\x00e1nximo de amigos atingido. Delete alguns.. !" }));
                }
            }
        }

        internal void LoadAchievements(Dictionary<string, UserAchievement> achievements)
        {
            this.Achievements = achievements;
        }

        internal void LoadData(UserData data)
        {
            this.LoadAchievements(data.achievements);
            this.LoadFavorites(data.favouritedRooms);
            this.LoadMutedUsers(data.ignores);
            this.LoadTags(data.tags);
        }

        internal void LoadFavorites(List<uint> roomID)
        {
            this.FavoriteRooms = new ArrayList();
            foreach (uint num in roomID)
            {
                this.FavoriteRooms.Add(num);
            }
        }

        internal void LoadMutedUsers(List<uint> usersMuted)
        {
            this.MutedUsers = usersMuted;
        }

        internal void LoadTags(List<string> tags)
        {
            this.Tags = tags;
        }

        internal void Mute()
        {
            if (!this.Muted)
            {
                this.GetClient().SendNotif(LanguageLocale.GetValue("moderation.muted"));
                this.Muted = true;
            }
        }

        internal void OnDisconnect()
        {
            if (!this.Disconnected)
            {
                this.Disconnected = true;
                ButterflyEnvironment.GetGame().GetClientManager().UnregisterClient(this.Id, this.Username);
                SessionManagement.IncreaseDisconnection();
                Logging.WriteLine(this.Username + " has logged out.");
                if (!this.HabboinfoSaved)
                {
                    this.HabboinfoSaved = true;
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET users.online = '0', users.points_online = '", this.PointsOnline, "', users.last_online = '", DateTime.Now.ToString(), "', activity_points = ", this.ActivityPoints, ", activity_points_lastupdate = '", this.LastActivityPointsUpdate, "', credits = ", this.Credits, ", achievement_points = ", this.AchievementPoints, " WHERE id = ", this.Id, " ;" }));
                    }
                }
                if (this.InRoom && (this.CurrentRoom != null))
                {
                    this.CurrentRoom.GetRoomUserManager().RemoveUserFromRoom(this.mClient, false, false);
                }
                if (this.Messenger != null)
                {
                    this.Messenger.AppearOffline = true;
                    this.Messenger.Destroy();
                }
                if (this.SubscriptionManager != null)
                {
                    this.SubscriptionManager.Clear();
                }
                if (this.AvatarEffectsInventoryComponent != null)
                {
                    this.AvatarEffectsInventoryComponent.Dispose();
                }
                if (this.InventoryComponent != null)
                {
                    this.InventoryComponent.SetIdleState();
                    this.InventoryComponent.RunDBUpdate();
                }
                this.mClient = null;
            }
        }

        internal void SerializeClub()
        {
            GameClient client = this.GetClient();
            ServerMessage message = new ServerMessage(Outgoing.SerializeClub);
            message.AppendString("club_habbo");
            if (this.GetSubscriptionManager().HasSubscription("club_habbo"))
            {
                double expireTime = this.GetSubscriptionManager().GetSubscription("club_habbo").ExpireTime;
                double num2 = expireTime - ButterflyEnvironment.GetUnixTimestamp();
                int num3 = (int) Math.Ceiling((double) (num2 / 86400.0));
                int i = num3 / 0x1f;
                if (i >= 1)
                {
                    i--;
                }
                message.AppendInt32((int) (num3 - (i * 0x1f)));
                message.AppendInt32(2);
                message.AppendInt32(i);
                message.AppendInt32(1);
                message.AppendBoolean(true);
                message.AppendBoolean(true);
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendInt32(500);
            }
            else
            {
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendInt32(1);
                message.AppendBoolean(false);
                message.AppendBoolean(true);
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendInt32(0);
            }
            client.SendMessage(message);
            ServerMessage message2 = new ServerMessage(Outgoing.Fuserights);
            if (this.GetSubscriptionManager().HasSubscription("club_habbo"))
            {
                message2.AppendInt32(2);//2
            }
            else if (this.GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                message2.AppendInt32(1);//1
            }
            else
            {
                message2.AppendInt32(0);
            }
            message2.AppendInt32(this.Rank);
            client.SendMessage(message2);
        }

        internal void Unmute()
        {
            if (this.Muted)
            {
                this.GetClient().SendNotif(LanguageLocale.GetValue("moderation.unmuted"));
                this.Muted = false;
            }
        }

        internal void UpdateActivityPointsBalance(bool InDatabase)
        {
            this.UpdateActivityPointsBalance(0);
        }

        internal void UpdateActivityPointsBalance(int NotifAmount)
        {
            if (((this.mClient != null) && (this.mClient.GetMessageHandler() != null)) && (this.mClient.GetMessageHandler().GetResponse() != null))
            {
                if (ActivityPoints > ButterflyEnvironment.MaxDucket)
                    ActivityPoints = ButterflyEnvironment.MaxDucket;

                this.mClient.GetMessageHandler().GetResponse().Init(Outgoing.ActivityPoints);
                this.mClient.GetMessageHandler().GetResponse().AppendInt32(2);
                this.mClient.GetMessageHandler().GetResponse().AppendInt32(0);
                this.mClient.GetMessageHandler().GetResponse().AppendInt32(this.ActivityPoints);
                this.mClient.GetMessageHandler().GetResponse().AppendInt32(105);
                this.mClient.GetMessageHandler().GetResponse().AppendInt32(this.Crystals);
                this.mClient.GetMessageHandler().SendResponse();
            }
        }

        internal void UpdateCreditsBalance()
        {
            this.mClient.GetMessageHandler().GetResponse().Init(Outgoing.CreditsBalance);
            this.mClient.GetMessageHandler().GetResponse().AppendString(this.Credits + ".0");
            this.mClient.GetMessageHandler().SendResponse();
        }

        internal Room CurrentRoom
        {
            get
            {
                if (this.CurrentRoomId <= 0)
                {
                    return null;
                }
                return ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.CurrentRoomId);
            }
        }

        internal string GetQueryString
        {
            get
            {
                this.HabboinfoSaved = true;
                return string.Concat(new object[] { "UPDATE users SET users.online = '0', users.last_online = '", DateTime.Now.ToString(), "', activity_points = '", this.ActivityPoints, "', activity_points_lastupdate = '", this.LastActivityPointsUpdate, "', credits = '", this.Credits, "', achievement_points = ", this.AchievementPoints, " WHERE id = '", this.Id, "'; DELETE FROM user_online WHERE userid = ", this.Id, "; " });
            }
        }

        internal bool InRoom
        {
            get
            {
                return (this.CurrentRoomId >= 1);
            }
        }
    }
}

