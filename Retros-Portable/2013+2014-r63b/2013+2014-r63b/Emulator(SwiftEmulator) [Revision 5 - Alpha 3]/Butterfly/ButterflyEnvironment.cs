namespace Butterfly
{
    using Butterfly.Core;
    using Butterfly.HabboHotel;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Guilds;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using Butterfly.Messages;
    using Butterfly.Messages.ClientMessages;
    using Butterfly.Messages.StaticMessageHandlers;
    using Butterfly.Net;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEncryption;
    using HabboEvents;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;

    internal static class ButterflyEnvironment
    {
        internal static bool Activated = false;
        private static readonly List<char> allowedchars = new List<char>(new char[] { 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', 
            '7', '8', '9', '0', '-', '.'
         });
        private static ConfigurationData Configuration;
        private static ConnectionHandeling ConnectionManager;
        internal static CultureInfo cultureInfo;
        private static BigInteger d = new BigInteger("59AE13E243392E89DED305764BDD9E92E4EAFA67BB6DAC7E1415E8C645B0950BCCD26246FD0D4AF37145AF5FA026C0EC3A94853013EAAE5FF1888360F4F9449EE023762EC195DFF3F30CA0B08B8C947E3859877B5D7DCED5C8715C58B53740B84E11FBC71349A27C31745FCEFEEEA57CFF291099205E230E0C7C27E8E1C0512B", 0x10);
        internal static DatabaseType dbType;
        private static Encoding DefaultEncoding;
        internal static bool diagPackets = false;
        internal static bool DisconectLog = false;
        private static BigInteger e = new BigInteger(3L);
        public static uint friendRequestLimit = 300;
        private static Butterfly.HabboHotel.Game Game;
        public static uint giftInt = 0x242;
        internal static HabboCrypto globalCrypto;
        internal static bool groupsEnabled;
        internal static string HotelStreamMessage = "";
        internal static string InteralLicense = "";
        internal static bool IrcEnabled;
        internal static bool isLive;
        internal static bool AllowPacketlog = true;
        internal static string LinkConfig;
        internal static string HotelLink;
        private static DatabaseManager manager;
        internal static MusSocket MusSystem;
        private static BigInteger n = new BigInteger("86851DD364D5C5CECE3C883171CC6DDC5760779B992482BD1E20DD296888DF91B33B936A7B93F06D29E8870F703A216257DEC7C81DE0058FEA4CC5116F75E6EFC4E9113513E45357DC3FD43D4EFAB5963EF178B78BD61E81A14C603B24C8BCCE0A12230B320045498EDC29282FF0603BC7B7DAE8FC1B05B52B2F301A9DC783B7", 0x10);
        internal const string PrettyVersion = "Swift";
        internal static string publicToken;
        internal static bool SeparatedTasksInGameClientManager = false;
        internal static bool SeparatedTasksInMainLoops = false;
        internal static int ServerRelease = 1;
        internal static DateTime ServerStarted;
        private static bool ShutdownInitiated = false;
        internal static bool spamBans = false;
        internal static int spamBans_limit = 20;
        internal static string StreamLink;
        internal static bool SystemMute;
        internal static int timeout = 500;
        private static Dictionary<uint, Habbo> usersCached = new Dictionary<uint, Habbo>();
        internal static bool useSSO;
        internal static string secretKey = "12844543231839046982589043811871065476910599239608903221675649771360705087933";//old: 24231219992253632572058933470468103090824667747608911151318774416044820318109
        internal static int MaxDucket = 6500;

        internal static void AppendTimeStampWithComment(ref StringBuilder builder, DateTime time, string text)
        {
            builder.AppendLine(text + " =>[" + TimeSpanToString((TimeSpan)(DateTime.Now - time)) + "]");
        }

        internal static string BoolToEnum(bool Bool)
        {
            if (Bool)
            {
                return "1";
            }
            return "0";
        }

        internal static void Destroy()
        {
            isLive = false;
            Logging.WriteLine("Destroying Butterfly environment...");
            if (GetGame() != null)
            {
                GetGame().Destroy();
                Game = null;
            }
            if (GetConnectionManager() != null)
            {
                Logging.WriteLine("Destroying connection manager.");
                GetConnectionManager().Destroy();
            }
            if (manager != null)
            {
                try
                {
                    Logging.WriteLine("Destroying database manager.");
                    manager.destroy();
                }
                catch
                {
                }
            }
            Logging.WriteLine("Uninitialized successfully. Closing.");
        }

        internal static bool EnumToBool(string Enum)
        {
            return (Enum == "1");
        }

        internal static string FilterFigure(string figure)
        {
            foreach (char ch in figure)
            {
                if (!isValid(ch))
                {
                    return "lg-3023-1335.hr-828-45.sh-295-1332.hd-180-4.ea-3168-89.ca-1813-62.ch-235-1332";
                }
            }
            return figure;
        }

        internal static string FilterInjectionChars(string Input)
        {
            Input = Input.Replace((char)1, ' ');
            Input = Input.Replace((char)2, ' ');
            Input = Input.Replace((char)3, ' ');
            Input = Input.Replace((char)9, ' ');
            return Input;
        }

        internal static string FilterInjectionChars(string Input, bool AllowLinebreaks)
        {
            if (Input == null)
            {
                return null;
            }


            return Regex.Replace(Input, @"[\r\n\x00\x1a\\'""]", @"\$0");
        }

        internal static ConfigurationData GetConfig()
        {
            return Configuration;
        }

        internal static ConnectionHandeling GetConnectionManager()
        {
            return ConnectionManager;
        }

        internal static DatabaseManager GetDatabaseManager()
        {
            return manager;
        }

        internal static Encoding GetDefaultEncoding()
        {
            return DefaultEncoding;
        }

        internal static Butterfly.HabboHotel.Game GetGame()
        {
            return Game;
        }

        internal static Habbo getHabboForId(uint UserId)
        {
            try
            {
                GameClient clientByUserID = GetGame().GetClientManager().GetClientByUserID(UserId);
                if (clientByUserID != null)
                {
                    Habbo habbo = clientByUserID.GetHabbo();
                    if ((habbo != null) && (habbo.Id > 0))
                    {
                        if (usersCached.ContainsKey(UserId))
                        {
                            usersCached.Remove(UserId);
                        }
                        return habbo;
                    }
                }
                else
                {
                    if (usersCached.ContainsKey(UserId))
                    {
                        return usersCached[UserId];
                    }
                    UserData userData = UserDataFactory.GetUserData((int)UserId);
                    Habbo user = userData.user;
                    if (user != null)
                    {
                        user.InitInformation(userData);
                        usersCached.Add(UserId, user);
                        return user;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        internal static Habbo getHabboForName(string UserName)
        {
            try
            {
                using (IQueryAdapter adapter = GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("SELECT id FROM users WHERE username = '" + UserName + "'");
                    int num = adapter.getInteger();
                    if (num > 0)
                    {
                        return getHabboForId((uint)num);
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        internal static int GetRandomNumber(int Min, int Max)
        {
            RandomBase base2 = new Quick();
            return base2.Next(Min, Max);
        }

        internal static int GetUnixTimestamp()
        {
            TimeSpan span = (TimeSpan)(DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 0, 0, 0));
            return (int)span.TotalSeconds;
        }

        internal static void Initialize()
        {
            Console.Clear();
            DateTime now = DateTime.Now;
            SystemMute = false;
            IrcEnabled = false;
            ServerStarted = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine();
            Console.WriteLine(@"      ___           ___                                              ");
            Console.WriteLine(@"     /  /\         /  /\           ___         ___           ___     ");
            Console.WriteLine(@"    /  /::\       /  /:/_         /__/\       /  /\         /__/\    ");
            Console.WriteLine(@"   /__/:/\:\     /  /:/ /\        \__\:\     /  /::\        \  \:\   ");
            Console.WriteLine(@"  _\_ \:\ \:\   /  /:/ /:/_       /  /::\   /  /:/\:\        \__\:\  ");
            Console.WriteLine(@" /__/\ \:\ \:\ /__/:/ /:/ /\   __/  /:/\/  /  /::\ \:\       /  /::\ ");
            Console.WriteLine(@" \  \:\ \:\_\/ \  \:\/:/ /:/  /__/\/:/~~  /__/:/\:\ \:\     /  /:/\:\");
            Console.WriteLine(@"  \  \:\_\:\    \  \::/ /:/   \  \::/     \__\/  \:\_\/    /  /:/__\/");
            Console.WriteLine(@"   \  \:\/:/     \  \:\/:/     \  \:\          \  \:\     /__/:/     ");
            Console.WriteLine(@"    \  \::/       \  \::/       \__\/           \__\/     \__\/      ");
            Console.WriteLine(@"     \__\/         \__\/                                             ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Revision 5 - Alpha 3 @ AKllX ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("For a full credits list acess :info");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Title = "Loading " + PrettyVersion;
            DefaultEncoding = Encoding.Default;
            //Logging.WriteLine("");
            cultureInfo = CultureInfo.CreateSpecificCulture("en-GB");
            LanguageLocale.Init();
            try
            {
                publicToken = new BigInteger(DiffieHellman.GenerateRandomHexString(15), 0x10).ToString();
                globalCrypto = new HabboCrypto(n, e, d);
                ChatCommandRegister.Init();
                PetCommandHandeler.Init();
                PetLocale.Init();
                Configuration = new ConfigurationData(Path.Combine(Application.StartupPath, "Settings/configuration.ini"), false);
                DateTime time2 = DateTime.Now;
                Logging.WriteLine("Connecting to database...");
                dbType = (GetConfig().data.ContainsKey("db.mssql") && (GetConfig().data["db.mssql"] == "true")) ? DatabaseType.MSSQL : DatabaseType.MySQL;
                HotelLink = GetConfig().data["hotel.link"];
                LinkConfig = GetConfig().data["guilds.link"];
                StreamLink = GetConfig().data["stream.link"];
                HotelStreamMessage = GetConfig().data["hotel.stream.message"];
                MaxDucket = Convert.ToInt32(GetConfig().data["ducket.upper.limit"]);
                AllowPacketlog = Convert.ToBoolean(GetConfig().data["allow.packetlog"]);

                manager = new DatabaseManager(uint.Parse(GetConfig().data["db.pool.maxsize"]), int.Parse(GetConfig().data["db.pool.minsize"]), dbType);
                manager.setServerDetails(GetConfig().data["db.hostname"], uint.Parse(GetConfig().data["db.port"]), GetConfig().data["db.username"], GetConfig().data["db.password"], GetConfig().data["db.name"]);
                manager.init();
                TimeSpan span = (TimeSpan)(DateTime.Now - time2);
                Logging.WriteLine(string.Concat(new object[] { "Connected to database! (", span.Seconds, " s, ", span.Milliseconds, " ms)" }));
                LanguageLocale.InitSwearWord();
                friendRequestLimit = (uint)int.Parse(GetConfig().data["client.maxrequests"]);
                GuildsPartsData.InitGroups();
                Game = new Butterfly.HabboHotel.Game(int.Parse(GetConfig().data["game.tcp.conlimit"]));
                Game.ContinueLoading();
                ConnectionManager = new ConnectionHandeling(int.Parse(GetConfig().data["game.tcp.port"]), int.Parse(GetConfig().data["game.tcp.conlimit"]), int.Parse(GetConfig().data["game.tcp.conperip"]), GetConfig().data["game.tcp.enablenagles"].ToLower() == "true");
                ConnectionManager.init();
                ConnectionManager.Start();
                StaticClientMessageHandler.Initialize();
                ClientMessageFactory.Init();
                string[] strArray = GetConfig().data["mus.tcp.allowedaddr"].Split(new char[] { Convert.ToChar(",") });
                MusSystem = new MusSocket(GetConfig().data["mus.tcp.bindip"], int.Parse(GetConfig().data["mus.tcp.port"]), strArray, 0);
                groupsEnabled = false;
                //if (Configuration.data.ContainsKey("groups.enabled") && (Configuration.data["groups.enabled"] == "true"))
                //{
                groupsEnabled = true;
                //}
                useSSO = true;
                //if (Configuration.data.ContainsKey("auth.ssodisabled") && (Configuration.data["auth.ssodisabled"] == "false"))
                //{
                //    useSSO = false;
                //}
                if (Configuration.data.ContainsKey("spambans.enabled") && (Configuration.data["spambans.enabled"] == "true"))
                {
                    spamBans = true;
                    spamBans_limit = Convert.ToInt32(Configuration.data["spambans.limit"]);
                    //Logging.WriteLine("Spam Bans enabled");
                }
                if (Configuration.data.ContainsKey("SeparatedTasksInMainLoops.enabled") && (Configuration.data["SeparatedTasksInMainLoops.enabled"] == "true"))
                {
                    SeparatedTasksInMainLoops = true;
                    //Logging.WriteLine("MultiTasking in MainLoop");
                }
                if (Configuration.data.ContainsKey("SeparatedTasksInGameClientManager.enabled") && (Configuration.data["SeparatedTasksInGameClientManager.enabled"] == "true"))
                {
                    SeparatedTasksInGameClientManager = true;
                    //Logging.WriteLine("MultiTasking in ClientManager");
                }
                TimeSpan span2 = (TimeSpan)(DateTime.Now - now);
                Logging.WriteLine(string.Concat(new object[] { PrettyVersion, " -> READY! (", span2.Seconds, " s, ", span2.Milliseconds, " ms)" }));
                using (IQueryAdapter adapter = GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE server_status SET bannerdata='", globalCrypto.Prime, ":", globalCrypto.Generator, "';" }));
                }
                //Logging.WriteLine("GlobalCrypto keys set in SQL.");
                isLive = true;
                //if (Debugger.IsAttached)
                //{
                //    Console.ForegroundColor = ConsoleColor.Yellow;
                //    Logging.WriteLine("Server is debugging: Console writing enabled");
                //    Console.ForegroundColor = ConsoleColor.White;
                //}
                //else
                //{
                //    Logging.WriteLine("Server is not debugging: Console writing disabled");
                //    Logging.DisablePrimaryWriting(false);
                //}
            }
            catch (KeyNotFoundException exception)
            {
                Logging.WriteLine("Please check your configuration file - some values appear to be missing.");
                Logging.WriteLine("Press any key to shut down ...");
                Logging.WriteLine(exception.ToString());
                Console.ReadKey(true);
                Destroy();
            }
            catch (InvalidOperationException exception2)
            {
                Logging.WriteLine("Failed to initialize ButterflyEmulator: " + exception2.Message);
                Logging.WriteLine("Press any key to shut down ...");
                Console.ReadKey(true);
                Destroy();
            }
            catch (Exception exception3)
            {
                Console.WriteLine("Fatal error during startup: " + exception3.ToString());
                Console.WriteLine("Press a key to exit");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        private static bool isValid(char character)
        {
            return allowedchars.Contains(character);
        }

        internal static bool IsValidAlphaNumeric(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            for (int i = 0; i < inputStr.Length; i++)
            {
                if (!(char.IsLetter(inputStr[i]) || char.IsNumber(inputStr[i])))
                {
                    return false;
                }
            }
            return true;
        }

        internal static void PreformShutDown()
        {
            PreformShutDown(true);
        }

        internal static void PreformShutDown(bool ExitWhenDone)
        {
            if (!ShutdownInitiated && isLive)
            {
                StringBuilder builder = new StringBuilder();
                DateTime now = DateTime.Now;
                DateTime time = DateTime.Now;
                ShutdownInitiated = true;
                SendMassMessage(LanguageLocale.GetValue("shutdown.alert"));
                AppendTimeStampWithComment(ref builder, time, "Hotel pre-warning");
                Game.StopGameLoop();
                Console.Write("Game loop stopped");
                DateTime time3 = DateTime.Now;
                Console.WriteLine("Server shutting down...");
                Console.Title = "<<- SERVER SHUTDOWN ->>";
                GetConnectionManager().Destroy();
                AppendTimeStampWithComment(ref builder, time3, "Socket close");
                DateTime time4 = DateTime.Now;
                GetGame().GetClientManager().CloseAll();
                AppendTimeStampWithComment(ref builder, time4, "Furni pre-save and connection close");
                DateTime time5 = DateTime.Now;
                Console.WriteLine("<<- SERVER SHUTDOWN ->> ROOM SAVE");
                Game.GetRoomManager().RemoveAllRooms();
                AppendTimeStampWithComment(ref builder, time5, "Room destructor");
                DateTime time6 = DateTime.Now;
                using (IQueryAdapter adapter = manager.getQueryreactor())
                {
                    adapter.runFastQuery("TRUNCATE TABLE user_online");
                    adapter.runFastQuery("TRUNCATE TABLE room_active");
                }
                AppendTimeStampWithComment(ref builder, time6, "Database pre-save");
                DateTime time7 = DateTime.Now;
                ConnectionManager.Destroy();
                AppendTimeStampWithComment(ref builder, time7, "Connection shutdown");
                DateTime time8 = DateTime.Now;
                Game.Destroy();
                AppendTimeStampWithComment(ref builder, time8, "Game destroy");
                DateTime time9 = DateTime.Now;
                try
                {
                    Console.WriteLine("Destroying database manager.");
                    manager.destroy();
                }
                catch
                {
                }
                AppendTimeStampWithComment(ref builder, time9, "Database shutdown");
                TimeSpan span = (TimeSpan)(DateTime.Now - now);
                builder.AppendLine("Total time on shutdown " + TimeSpanToString(span));
                builder.AppendLine("You have reached ==> [END OF SESSION]");
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine();
                Logging.LogShutdown(builder);
                Console.WriteLine("System disposed, goodbye!");
                if (ExitWhenDone)
                {
                    Environment.Exit(Environment.ExitCode);
                }
            }
        }

        internal static void SendMassMessage(string Message)
        {
            try
            {
                ServerMessage message = new ServerMessage(Outgoing.BroadcastMessage);
                message.AppendString(Message);
                GetGame().GetClientManager().QueueBroadcaseMessage(message);
            }
            catch (Exception exception)
            {
                Logging.HandleException(exception, "ButterflyEnvironment.SendMassMessage");
            }
        }

        internal static string TimeSpanToString(TimeSpan span)
        {
            return string.Concat(new object[] { span.Seconds, " s, ", span.Milliseconds, " ms" });
        }

        internal static bool ShutdownStarted
        {
            get
            {
                return ShutdownInitiated;
            }
        }
    }
}

