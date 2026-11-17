using System;
using System.Threading.Tasks;
using Atom.Core;
using Atom.HabboHotel.Achievements;
using Atom.HabboHotel.Advertisements;
using Atom.HabboHotel.Catalogs;
using Atom.HabboHotel.GameClients;
using Atom.HabboHotel.Items;
using Atom.HabboHotel.Misc;
using Atom.HabboHotel.Navigators;
using Atom.HabboHotel.Roles;
using Atom.HabboHotel.RoomBots;
using Atom.HabboHotel.Rooms;
using Atom.HabboHotel.Support;
using Atom.HabboHotel.Pets;
using Atom.ServerManager;
using Atom.HabboHotel.Users.Inventory;
using Database_Manager.Database.Session_Details.Interfaces;
using System.Threading;
using Atom.HabboHotel.Quests;
using Atom.HabboHotel.SoundMachine;
using Atom.HabboHotel.Users.Competitions;

namespace Atom.HabboHotel
{
    class Game
    {
        #region Fields
        private GameClientManager ClientManager;
        private ModerationBanManager BanManager;
        private RoleManager RoleManager;
        private HelpTool HelpTool;
        private Catalog Catalog;
        private Navigator Navigator;
        private ItemManager ItemManager;
        private RoomManager RoomManager;
        private AdvertisementManager AdvertisementManager;
        private PixelManager PixelManager;
        private AchievementManager AchievementManager;
        private ModerationTool ModerationTool;
        private BotManager BotManager;
        //private Task StatisticsThread;
        //private Task ConsoleTitleTask;
        private InventoryGlobal globalInventory;
        private QuestManager questManager;
        //private SoundMachineManager soundMachineManager;

        private Thread gameLoop;
        private bool gameLoopActive;
        private const int gameLoopSleepTime = 25;

        // Tasks Main GameLoop
        private Task LowPriorityWorker_task, RoomManagerCycle_task, ClientManagerCycle_task;
        internal bool LowPriorityWorker_ended, RoomManagerCycle_ended, ClientManagerCycle_ended;
        #endregion

        #region Return values

        internal GameClientManager GetClientManager()
        {
            return ClientManager;
        }

        internal ModerationBanManager GetBanManager()
        {
            return BanManager;
        }

        internal RoleManager GetRoleManager()
        {
            return RoleManager;
        }

        internal HelpTool GetHelpTool()
        {
            return HelpTool;
        }

        internal Catalog GetCatalog()
        {
            return Catalog;
        }

        internal Navigator GetNavigator()
        {
            return Navigator;
        }

        internal ItemManager GetItemManager()
        {
            return ItemManager;
        }

        internal RoomManager GetRoomManager()
        {
            return RoomManager;
        }

        internal AdvertisementManager GetAdvertisementManager()
        {
            return AdvertisementManager;
        }

        internal PixelManager GetPixelManager()
        {
            return PixelManager;
        }

        internal AchievementManager GetAchievementManager()
        {
            return AchievementManager;
        }

        internal ModerationTool GetModerationTool()
        {
            return ModerationTool;
        }

        internal BotManager GetBotManager()
        {
            return BotManager;
        }

        internal InventoryGlobal GetInventory()
        {
            return globalInventory;
        }

        internal QuestManager GetQuestManager()
        {
            return questManager;
        }
        #endregion

        #region Boot
        internal Game(int conns)
        {
            ClientManager = new GameClientManager();

            //if (ButterflyEnvironment.GetConfig().data["client.ping.enabled"] == "1")
            //{
            //    ClientManager.StartConnectionChecker();
            //}

            
            using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
            {
                
                //ButterflyEnvironment.GameInstance = this;
                DateTime start = DateTime.Now;

                BanManager = new ModerationBanManager();
                RoleManager = new RoleManager();
                HelpTool = new HelpTool();
                Catalog = new Catalog();
                Navigator = new Navigator();
                ItemManager = new ItemManager();
                RoomManager = new RoomManager();
                AdvertisementManager = new AdvertisementManager();
                PixelManager = new PixelManager();
                
                ModerationTool = new ModerationTool();
                BotManager = new BotManager();
                questManager = new QuestManager();
                //soundMachineManager = new SoundMachineManager();

                TimeSpan spent = DateTime.Now - start;

                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Inicialización → Listo! (" + spent.Seconds + " s, " + spent.Milliseconds + " ms)");
            }
        }
        internal void ContinueLoading()
        {
            DateTime Start;
            TimeSpan TimeUsed;
            using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
            {
                Start = DateTime.Now;
                BanManager.LoadBans(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Prohibiciones → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                //RoleManager.LoadRoles(dbClient);
                RoleManager.LoadRights(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Permisos → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                HelpTool.LoadCategories(dbClient);
                HelpTool.LoadTopics(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Ayuda → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                Catalog.Initialize(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Cache → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                Navigator.Initialize(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Navegador → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                ItemManager.LoadItems(dbClient);
                globalInventory = new InventoryGlobal();
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Items → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                RoomManager.LoadModels(dbClient);
                RoomManager.InitRoomLinks(dbClient);
                RoomManager.InitVotedRooms(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Salas → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                Ranking.initRankings(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Clasificación → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                AdvertisementManager.LoadRoomAdvertisements(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Publicidad → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                AchievementManager = new AchievementManager(dbClient);
                questManager.Initialize(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Logros → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                ModerationTool.LoadMessagePresets(dbClient);
                ModerationTool.LoadPendingTickets(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Herramientas de Moderación → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                BotManager.LoadBots(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Bots → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                PetRace.Init(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Mascotas → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                Catalog.InitCache();
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Catálogo → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                Start = DateTime.Now;
                SongManager.Initialize();
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Sonido → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");


                Start = DateTime.Now;
                DatabaseCleanup(dbClient);
                LowPriorityWorker.Init(dbClient);
                TimeUsed = DateTime.Now - Start;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Limpieza MySQL → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");
            }

            StartGameLoop();
            TimeUsed = DateTime.Now - Start;
            Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Juegos → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

            DataSocket.SetupListener(42);
            DataSocket.Start();
        }

        #endregion

        #region Game loop

        internal void StartGameLoop()
        {
            gameLoopActive = true;
            gameLoop = new Thread(new ThreadStart(MainGameLoop));
            gameLoop.Start();

            //gameLoopSUBRooms = new Thread(new ThreadStart(RoomManager.OnCycle_RoomDedicatedThread));
            //gameLoopSUBRooms.Start();

        }

        internal void StopGameLoop()
        {
            gameLoopActive = false;

            while (LowPriorityWorker_ended == false || RoomManagerCycle_ended == false || ClientManagerCycle_ended == false)
            {
                Thread.Sleep(gameLoopSleepTime);
            }
        }

        internal byte GameLoopStatus = 0;

        internal static bool gameLoopEnabled = true;

        private void MainGameLoop_RoomDedicatedThread()
        {
            while (gameLoopActive)
            {
                if (gameLoopEnabled)
                {
                    try
                    {
                        LowPriorityWorker_ended = false;
                        ClientManagerCycle_ended = false;
                        if (AtomEnvironment.SeparatedTasksInMainLoops != true)
                        {
                            GameLoopStatus = 1;
                            LowPriorityWorker.Process(); //1 query
                            GameLoopStatus = 6;
                            ClientManager.OnCycle();
                            GameLoopStatus = 7;
                        }
                        else
                        {
                            if (LowPriorityWorker_task == null || LowPriorityWorker_task.IsCompleted == true)
                            {
                                LowPriorityWorker_task = new Task(LowPriorityWorker.Process);
                                LowPriorityWorker_task.Start();
                            }

                            if (ClientManagerCycle_task == null || ClientManagerCycle_task.IsCompleted == true)
                            {
                                ClientManagerCycle_task = new Task(ClientManager.OnCycle);
                                ClientManagerCycle_task.Start();
                            }

                        }


                    }
                    catch (Exception e)
                    {
                        Logging.LogCriticalException("INVALID MARIO BUG IN GAME LOOP: " + e.ToString());
                    }
                    GameLoopStatus = 8;
                }
                Thread.Sleep(gameLoopSleepTime);
            }

        }


        private void MainGameLoop()
        {
            while (gameLoopActive)
            {
                if (gameLoopEnabled)
                {
                    try
                    {
                        LowPriorityWorker_ended = false;
                        RoomManagerCycle_ended = false;
                        ClientManagerCycle_ended = false;
                        if (AtomEnvironment.SeparatedTasksInMainLoops != true)
                        {
                            GameLoopStatus = 1;
                            LowPriorityWorker.Process(); //1 query
                            GameLoopStatus = 5;
                            RoomManager.OnCycle(); // Queries for furni save
                            GameLoopStatus = 6;
                            ClientManager.OnCycle();
                            GameLoopStatus = 7;
                        }
                        else
                        {
                            if (LowPriorityWorker_task == null || LowPriorityWorker_task.IsCompleted == true)
                            {
                                LowPriorityWorker_task = new Task(LowPriorityWorker.Process);
                                LowPriorityWorker_task.Start();
                            }

                            if (RoomManagerCycle_task == null || RoomManagerCycle_task.IsCompleted == true)
                            {
                                RoomManagerCycle_task = new Task(RoomManager.OnCycle);
                                RoomManagerCycle_task.Start();
                            }

                            if (ClientManagerCycle_task == null || ClientManagerCycle_task.IsCompleted == true)
                            {
                                ClientManagerCycle_task = new Task(ClientManager.OnCycle);
                                ClientManagerCycle_task.Start();
                            }

                        }


                    }
                    catch (Exception e)
                    {
                        Logging.LogCriticalException("INVALID MARIO BUG IN GAME LOOP: " + e.ToString());
                    }
                    GameLoopStatus = 8;
                }
                Thread.Sleep(gameLoopSleepTime);
            }

        }
        #endregion
        #region Pasaje de parámetros a thread dedicado
        internal bool gameLoopEnabled_EXT
        {
            get
            {
                return gameLoopEnabled;
            }
        }

        internal bool gameLoopActive_EXT
        {
            get
            {
                return gameLoopActive;
            }
        }

        internal int gameLoopSleepTime_EXT
        {
            get
            {
                return gameLoopSleepTime;
            }
        }
        #endregion
        #region Shutdown
        internal static void DatabaseCleanup(IQueryAdapter dbClient)
        {
            //dbClient.runFastQuery("TRUNCATE TABLE user_tickets");
            dbClient.runFastQuery("TRUNCATE TABLE user_online");
            dbClient.runFastQuery("TRUNCATE TABLE room_active");
            dbClient.runFastQuery("UPDATE server_status SET status = 1, users_online = 0, rooms_loaded = 0, server_ver = '" + AtomEnvironment.PrettyVersion + "', stamp = '" + AtomEnvironment.GetUnixTimestamp() + "' ");
        }

        internal void Destroy()
        {

            //Stop game thread

            using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
            {
                DatabaseCleanup(dbClient);
            }
            if (GetClientManager() != null)
            {
                //GetClientManager().Clear();
                //GetClientManager().StopConnectionChecker();
            }

            Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] [INFO] → Destruido el servidor");
        }
        #endregion


        #region reload
        internal void reloaditems()
        {
            using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
            {
                ItemManager.LoadItems(dbClient);
                globalInventory = new InventoryGlobal();
            }
        }

        #endregion

    }
}
