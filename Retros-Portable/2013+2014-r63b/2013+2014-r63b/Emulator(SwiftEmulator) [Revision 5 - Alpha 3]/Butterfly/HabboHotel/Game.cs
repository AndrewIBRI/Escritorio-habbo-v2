namespace Butterfly.HabboHotel
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Achievements;
    using Butterfly.HabboHotel.Advertisements;
    using Butterfly.HabboHotel.Catalogs;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Games.SnowStorm;
    using Butterfly.HabboHotel.Habbo.Guilds;
    using Butterfly.HabboHotel.Habbo.SoundMachine;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Misc;
    using Butterfly.HabboHotel.Navigators;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.Quests;
    using Butterfly.HabboHotel.Roles;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Support;
    using Butterfly.HabboHotel.Users.Competitions;
    using Butterfly.HabboHotel.Users.Inventory;
    using Butterfly.ServerManager;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Game
    {
        private Butterfly.HabboHotel.Achievements.AchievementManager AchievementManager;
        private Butterfly.HabboHotel.Advertisements.AdvertisementManager AdvertisementManager;
        private ModerationBanManager BanManager;
        private Butterfly.HabboHotel.Catalogs.Catalog Catalog;
        private GameClientManager ClientManager = new GameClientManager();
        internal bool ClientManagerCycle_ended;
        private Task ClientManagerCycle_task;
        private Thread gameLoop;
        private bool gameLoopActive;
        internal static bool gameLoopEnabled = true;
        private const int gameLoopSleepTime = 0x19;
        internal byte GameLoopStatus = 0;
        private InventoryGlobal globalInventory;
        private Butterfly.HabboHotel.Habbo.Guilds.Guilds guilds;
        private Butterfly.HabboHotel.Support.HelpTool HelpTool;
        private Butterfly.HabboHotel.Items.ItemManager ItemManager;
        public int LastGuildId;
        internal bool LowPriorityWorker_ended;
        private Task LowPriorityWorker_task;
        private Butterfly.HabboHotel.Support.ModerationTool ModerationTool;
        private Butterfly.HabboHotel.Navigators.Navigator Navigator;
        private Butterfly.HabboHotel.Misc.PixelManager PixelManager;
        private QuestManager questManager;
        private Butterfly.HabboHotel.Roles.RoleManager RoleManager;
        private Butterfly.HabboHotel.Rooms.RoomManager RoomManager;
        internal bool RoomManagerCycle_ended;
        private Task RoomManagerCycle_task;
        internal bool SnowWarCycle_ended;
        //private Task SnowWarCycle_task;
        private Task GuildsCycle_taskAdd;
        internal bool GuildsCycleAdd_ended;
        //private Task GuildsCycle_taskRemove;
        internal bool GuildsCycleRemove_ended;
        
        private WarsData StormWars;

        internal Game(int conns)
        {
            using (ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                DateTime now = DateTime.Now;
                this.BanManager = new ModerationBanManager();
                this.RoleManager = new Butterfly.HabboHotel.Roles.RoleManager();
                this.HelpTool = new Butterfly.HabboHotel.Support.HelpTool();
                this.Catalog = new Butterfly.HabboHotel.Catalogs.Catalog();
                this.Navigator = new Butterfly.HabboHotel.Navigators.Navigator();
                this.ItemManager = new Butterfly.HabboHotel.Items.ItemManager();
                this.RoomManager = new Butterfly.HabboHotel.Rooms.RoomManager();
                this.AdvertisementManager = new Butterfly.HabboHotel.Advertisements.AdvertisementManager();
                this.PixelManager = new Butterfly.HabboHotel.Misc.PixelManager();
                this.StormWars = new WarsData();
                this.ModerationTool = new Butterfly.HabboHotel.Support.ModerationTool();
                this.questManager = new QuestManager();
                this.guilds = new Butterfly.HabboHotel.Habbo.Guilds.Guilds();
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Class initialization -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
            }
        }

        internal void ContinueLoading()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT max(Id) FROM groups");
                this.LastGuildId = adapter.getInteger();
                DateTime now = DateTime.Now;
                this.BanManager.LoadBans(adapter);
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Ban manager -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                this.RoleManager.LoadRights(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Role manager -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                this.HelpTool.LoadCategories(adapter);
                this.HelpTool.LoadTopics(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Help tool -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                this.Catalog.Initialize(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Catacache -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                this.Navigator.Initialize(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Navigator -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                this.ItemManager.LoadItems(adapter);
                this.globalInventory = new InventoryGlobal();
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Item manager -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                this.RoomManager.LoadModels(adapter);
                this.RoomManager.InitRoomLinks(adapter);
                this.RoomManager.InitVotedRooms(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Room manager -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                Ranking.initRankings(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "User rankings -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                this.AdvertisementManager.LoadRoomAdvertisements(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Adviserment manager -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                this.AchievementManager = new Butterfly.HabboHotel.Achievements.AchievementManager(adapter);
                this.questManager.Init(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Achievement manager -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                this.ModerationTool.LoadMessagePresets(adapter);
                this.ModerationTool.LoadPendingTickets(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Moderation tool -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                PetRace.Init(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Pet system -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                this.Catalog.InitCache();
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Catalogue manager -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                SongManager.Initialize();
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Sound manager -> READY! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                now = DateTime.Now;
                DatabaseCleanup(adapter);
                LowPriorityWorker.Init(adapter);
                span = (TimeSpan) (DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { "Database -> Cleanup performed! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
            }
            this.StartGameLoop();
            Logging.WriteLine("Game manager -> READY!");
            DataSocket.SetupListener(0x2a);
            DataSocket.Start();
        }

        internal static void DatabaseCleanup(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("TRUNCATE TABLE user_online");
            dbClient.runFastQuery("TRUNCATE TABLE room_active");
            dbClient.runFastQuery("UPDATE server_status SET status = 1, users_online = 0, rooms_loaded = 0, server_ver = '(anonymous group revision)', stamp = '" + ButterflyEnvironment.GetUnixTimestamp() + "' ");
        }

        internal void Destroy()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                DatabaseCleanup(adapter);
            }
            if (this.GetClientManager() != null)
            {
            }
            Console.WriteLine("Destroyed Habbo Hotel.");
        }

        internal Butterfly.HabboHotel.Achievements.AchievementManager GetAchievementManager()
        {
            return this.AchievementManager;
        }

        internal Butterfly.HabboHotel.Advertisements.AdvertisementManager GetAdvertisementManager()
        {
            return this.AdvertisementManager;
        }

        internal ModerationBanManager GetBanManager()
        {
            return this.BanManager;
        }

        internal Butterfly.HabboHotel.Catalogs.Catalog GetCatalog()
        {
            return this.Catalog;
        }

        internal GameClientManager GetClientManager()
        {
            return this.ClientManager;
        }

        public Butterfly.HabboHotel.Habbo.Guilds.Guilds GetGuilds()
        {
            return this.guilds;
        }

        internal Butterfly.HabboHotel.Support.HelpTool GetHelpTool()
        {
            return this.HelpTool;
        }

        internal InventoryGlobal GetInventory()
        {
            return this.globalInventory;
        }

        internal Butterfly.HabboHotel.Items.ItemManager GetItemManager()
        {
            return this.ItemManager;
        }

        internal Butterfly.HabboHotel.Support.ModerationTool GetModerationTool()
        {
            return this.ModerationTool;
        }

        internal Butterfly.HabboHotel.Navigators.Navigator GetNavigator()
        {
            return this.Navigator;
        }

        internal Butterfly.HabboHotel.Misc.PixelManager GetPixelManager()
        {
            return this.PixelManager;
        }

        internal QuestManager GetQuestManager()
        {
            return this.questManager;
        }

        internal Butterfly.HabboHotel.Roles.RoleManager GetRoleManager()
        {
            return this.RoleManager;
        }

        internal Butterfly.HabboHotel.Rooms.RoomManager GetRoomManager()
        {
            return this.RoomManager;
        }

        internal WarsData GetStormWars()
        {
            return this.StormWars;
        }

        private void MainGameLoop()
        {
            while (this.gameLoopActive)
            {
                if (gameLoopEnabled)
                {
                    try
                    {
                        this.LowPriorityWorker_ended = false;
                        this.RoomManagerCycle_ended = false;
                        this.ClientManagerCycle_ended = false;
                        this.GuildsCycleAdd_ended = false;
                        this.GuildsCycleRemove_ended = false;
                        if (!ButterflyEnvironment.SeparatedTasksInMainLoops)
                        {
                            this.GameLoopStatus = 1;
                            LowPriorityWorker.Process();
                            this.GameLoopStatus = 5;
                            this.RoomManager.OnCycle();
                            this.GameLoopStatus = 6;
                            this.ClientManager.OnCycle();
                            this.GameLoopStatus = 7;
                        }
                        else
                        {
                            if ((this.LowPriorityWorker_task == null) || this.LowPriorityWorker_task.IsCompleted)
                            {
                                this.LowPriorityWorker_task = new Task(new Action(LowPriorityWorker.Process));
                                this.LowPriorityWorker_task.Start();
                            }
                            if ((this.RoomManagerCycle_task == null) || this.RoomManagerCycle_task.IsCompleted)
                            {
                                this.RoomManagerCycle_task = new Task(new Action(this.RoomManager.OnCycle));
                                this.RoomManagerCycle_task.Start();
                            }
                            if ((this.ClientManagerCycle_task == null) || this.ClientManagerCycle_task.IsCompleted)
                            {
                                this.ClientManagerCycle_task = new Task(new Action(this.ClientManager.OnCycle));
                                this.ClientManagerCycle_task.Start();
                            }
                            if ((this.GuildsCycle_taskAdd == null) || this.GuildsCycle_taskAdd.IsCompleted)
                            {
                                this.GuildsCycle_taskAdd = new Task(new Action(this.guilds.OnCycle_GuildAdd));
                                this.GuildsCycle_taskAdd.Start();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.LogCriticalException("INVALID MARIO BUG IN GAME LOOP: " + exception.ToString());
                    }
                    this.GameLoopStatus = 8;
                }
                Thread.Sleep(0x19);
            }
        }

        private void MainGameLoop_RoomDedicatedThread()
        {
            while (this.gameLoopActive)
            {
                if (gameLoopEnabled)
                {
                    try
                    {
                        this.LowPriorityWorker_ended = false;
                        this.ClientManagerCycle_ended = false;
                        if (!ButterflyEnvironment.SeparatedTasksInMainLoops)
                        {
                            this.GameLoopStatus = 1;
                            LowPriorityWorker.Process();
                            this.GameLoopStatus = 6;
                            this.ClientManager.OnCycle();
                            this.GameLoopStatus = 7;
                        }
                        else
                        {
                            if ((this.LowPriorityWorker_task == null) || this.LowPriorityWorker_task.IsCompleted)
                            {
                                this.LowPriorityWorker_task = new Task(new Action(LowPriorityWorker.Process));
                                this.LowPriorityWorker_task.Start();
                            }
                            if ((this.ClientManagerCycle_task == null) || this.ClientManagerCycle_task.IsCompleted)
                            {
                                this.ClientManagerCycle_task = new Task(new Action(this.ClientManager.OnCycle));
                                this.ClientManagerCycle_task.Start();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.LogCriticalException("INVALID MARIO BUG IN GAME LOOP: " + exception.ToString());
                    }
                    this.GameLoopStatus = 8;
                }
                Thread.Sleep(0x19);
            }
        }

        internal void reloaditems()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                this.ItemManager.LoadItems(adapter);
                this.globalInventory = new InventoryGlobal();
            }
        }

        internal void StartGameLoop()
        {
            this.gameLoopActive = true;
            this.gameLoop = new Thread(new ThreadStart(this.MainGameLoop));
            this.gameLoop.Start();
        }

        internal void StopGameLoop()
        {
            this.gameLoopActive = false;
            while ((!this.LowPriorityWorker_ended || !this.RoomManagerCycle_ended) || !this.ClientManagerCycle_ended || !this.GuildsCycleAdd_ended)
            {
                Thread.Sleep(0x19);
            }
        }

        internal bool gameLoopActive_EXT
        {
            get
            {
                return this.gameLoopActive;
            }
        }

        internal bool gameLoopEnabled_EXT
        {
            get
            {
                return gameLoopEnabled;
            }
        }

        internal int gameLoopSleepTime_EXT
        {
            get
            {
                return 0x19;
            }
        }
    }
}

