using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using Atom.Core;
using Atom.HabboHotel;
using Atom.HabboHotel.Pets;
using Atom.Messages;
using Database_Manager.Database;
using Database_Manager.Database.Session_Details.Interfaces;
using Atom.Messages.StaticMessageHandlers;
using Atom.Messages.ClientMessages;
using Atom.Net;
using System.Globalization;
using Atom.HabboHotel.Users;
using Atom.HabboHotel.Users.UserDataManagement;
using System.Data;
using HabboEncryption;
using HabboEvents;
namespace Atom
{
    static class AtomEnvironment
    {
        private static ConfigurationData Configuration;
        private static ConfigurationData License;
        private static Encoding DefaultEncoding;
        private static ConnectionHandeling ConnectionManager;
        private static Game Game;
        internal static DateTime ServerStarted;
        private static DatabaseManager manager;
        //internal static IRCBot messagingBot;
        internal static bool IrcEnabled;
        internal static bool groupsEnabled;
        internal static bool SystemMute;
        internal static bool useSSO;
        internal static bool isLive;
        internal const string PrettyVersion = "Atom Emulator 0.4.9";
        internal static bool diagPackets = false;
        internal static int timeout = 500;
        internal static DatabaseType dbType;
        internal static MusSocket MusSystem;
        internal static CultureInfo cultureInfo;
        public static uint giftInt = 41260;
        public static uint friendRequestLimit = 300;

        // Multi Tasking (configurations.ini)
        internal static bool SeparatedTasksInMainLoops = false;
        internal static bool SeparatedTasksInGameClientManager = false;

        // Bans por spam de flood (configurations.ini)
        internal static bool spamBans = false;
        internal static int spamBans_limit = 20;

        internal static HabboCrypto globalCrypto;
        internal static string publicToken;
        private static BigInteger n = new BigInteger("86851DD364D5C5CECE3C883171CC6DDC5760779B992482BD1E20DD296888DF91B33B936A7B93F06D29E8870F703A216257DEC7C81DE0058FEA4CC5116F75E6EFC4E9113513E45357DC3FD43D4EFAB5963EF178B78BD61E81A14C603B24C8BCCE0A12230B320045498EDC29282FF0603BC7B7DAE8FC1B05B52B2F301A9DC783B7", 16);
        private static BigInteger e = new BigInteger(3);
        private static BigInteger d = new BigInteger("59AE13E243392E89DED305764BDD9E92E4EAFA67BB6DAC7E1415E8C645B0950BCCD26246FD0D4AF37145AF5FA026C0EC3A94853013EAAE5FF1888360F4F9449EE023762EC195DFF3F30CA0B08B8C947E3859877B5D7DCED5C8715C58B53740B84E11FBC71349A27C31745FCEFEEEA57CFF291099205E230E0C7C27E8E1C0512B", 16);
        
        internal static void Initialize()
        {
            Console.Clear();
            DateTime Start = DateTime.Now;
            SystemMute = false;

            IrcEnabled = false;
            ServerStarted = DateTime.Now;
            Console.Title = " Cargando Atom Emulator";
            DefaultEncoding = Encoding.Default;

            //Mensaje de inicio
            Console.ForegroundColor = ConsoleColor.Red;
            Logging.WriteLine("");
            Console.WriteLine("         ###    ########  #######  ##     ##");
            Console.WriteLine("        ## ##      ##    ##     ## ###   ###");
            Console.WriteLine("       ##   ##     ##    ##     ## #### ####");
            Console.WriteLine("      ##     ##    ##    ##     ## ## ### ##");
            Console.WriteLine("      #########    ##    ##     ## ##     ##");
            Console.WriteLine("      ##     ##    ##    ##     ## ##     ##");
            Console.WriteLine("      ##     ##    ##     #######  ##     ##" + Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("    © 2012 Atom. Desarrollado para uso privado");
            Console.WriteLine("    Todos los derechos reservados");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.ResetColor();
            Logging.WriteLine("");

            cultureInfo = CultureInfo.CreateSpecificCulture("en-GB");
            LanguageLocale.Init();

            try
            {
                publicToken = new BigInteger(DiffieHellman.GenerateRandomHexString(15), 16).ToString();
                globalCrypto = new HabboCrypto(n, e, d);
                ChatCommandRegister.Init();
                PetCommandHandeler.Init();
                PetLocale.Init();
                Configuration = new ConfigurationData(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Settings/configuration.ini"));
                License = new ConfigurationData(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"Settings/license.ini"));

                if (AtomEnvironment.GetConfig().data["db.password"].Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(" [" + DateTime.Now.ToString("T") + "] [ERROR] → La contraseña MySQL no puede quedar en blanco");
                    Console.ResetColor();
                    Console.WriteLine(" [" + DateTime.Now.ToString("T") + "] [INFO] → Presiona una tecla para cerrar el servidor");
                    Console.ReadKey();
                    Environment.Exit(1);

                }
                else if (AtomEnvironment.GetConfig().data["db.password"] == "cambiame")
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(" [" + DateTime.Now.ToString("T") + "] [ERROR] → La contraseña MySQL no puede ser 'cambiame'");
                    Console.ResetColor();
                    Console.WriteLine(" [" + DateTime.Now.ToString("T") + "] [INFO] → Presiona una tecla para cerrar el servidor");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
                else
                {

                    DateTime Starts = DateTime.Now;

                    dbType = GetConfig().data.ContainsKey("db.mssql") && GetConfig().data["db.mssql"] == "true" ? DatabaseType.MSSQL : DatabaseType.MySQL;

                    manager = new DatabaseManager(uint.Parse(AtomEnvironment.GetConfig().data["db.pool.maxsize"]), int.Parse(AtomEnvironment.GetConfig().data["db.pool.minsize"]), dbType);
                    manager.setServerDetails(
                        AtomEnvironment.GetConfig().data["db.hostname"],
                        uint.Parse(AtomEnvironment.GetConfig().data["db.port"]),
                        AtomEnvironment.GetConfig().data["db.username"],
                        AtomEnvironment.GetConfig().data["db.password"],
                        AtomEnvironment.GetConfig().data["db.name"]);
                    manager.init();

                    TimeSpan TimeUsed2 = DateTime.Now - Starts;
                    Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] MySQL → Listo (" + TimeUsed2.Seconds + " s, " + TimeUsed2.Milliseconds + " ms)");

                    LanguageLocale.InitSwearWord();

                    friendRequestLimit = (uint)(int.Parse(AtomEnvironment.GetConfig().data["client.maxrequests"]));

                    Game = new Game(int.Parse(AtomEnvironment.GetConfig().data["game.tcp.conlimit"]));
                    Game.ContinueLoading();

                    try
                    {
                        ConnectionManager = new ConnectionHandeling(int.Parse(AtomEnvironment.GetConfig().data["game.tcp.port"]),
                            int.Parse(AtomEnvironment.GetConfig().data["game.tcp.conlimit"]),
                            int.Parse(AtomEnvironment.GetConfig().data["game.tcp.conperip"]),
                            AtomEnvironment.GetConfig().data["game.tcp.enablenagles"].ToLower() == "true");
                    }
                    catch (Exception a)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(" [" + DateTime.Now.ToString("T") + "] [ERROR] → Ha ocurrido un error" + a);
                    }
                    ConnectionManager.init();
                    ConnectionManager.Start();

                    StaticClientMessageHandler.Initialize();
                    ClientMessageFactory.Init();

                    string[] arrayshit = AtomEnvironment.GetConfig().data["mus.tcp.allowedaddr"].Split(Convert.ToChar(","));
                    MusSystem = new MusSocket(AtomEnvironment.GetConfig().data["mus.tcp.bindip"], int.Parse(AtomEnvironment.GetConfig().data["mus.tcp.port"]), arrayshit, 0);

                    //InitIRC(); 

                    groupsEnabled = false;
                    if (Configuration.data.ContainsKey("groups.enabled"))
                    {
                        if (Configuration.data["groups.enabled"] == "true")
                        {
                            groupsEnabled = true;
                        }
                    }

                    useSSO = true;
                    if (Configuration.data.ContainsKey("auth.ssodisabled"))
                    {
                        if (Configuration.data["auth.ssodisabled"] == "false")
                        {
                            useSSO = false;
                        }
                    }

                    if (Configuration.data.ContainsKey("spambans.enabled"))
                    {
                        if (Configuration.data["spambans.enabled"] == "true")
                        {
                            spamBans = true;
                            spamBans_limit = Convert.ToInt32(Configuration.data["spambans.limit"]);
                            Console.WriteLine("Spam Bans enabled");
                        }
                    }
                    if (Configuration.data.ContainsKey("SeparatedTasksInMainLoops.enabled"))
                    {
                        if (Configuration.data["SeparatedTasksInMainLoops.enabled"] == "true")
                        {
                            SeparatedTasksInMainLoops = true;
                            TimeSpan TimeUsed3 = DateTime.Now - Starts;
                            Console.WriteLine(" [" + DateTime.Now.ToString("T") + "] Multitarea en MainLoop → Listo! (" + TimeUsed3.Seconds + " s, " + TimeUsed3.Milliseconds + " ms)");
                        }
                    }

                    if (Configuration.data.ContainsKey("SeparatedTasksInGameClientManager.enabled"))
                    {
                        if (Configuration.data["SeparatedTasksInGameClientManager.enabled"] == "true")
                        {
                            SeparatedTasksInGameClientManager = true;
                            Console.WriteLine("MultiTasking in ClientManager");
                        }
                    }

                    TimeSpan TimeUsed = DateTime.Now - Start;
                    Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Núcleo → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");

                    using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.runFastQuery("UPDATE server_status SET bannerdata='" + AtomEnvironment.globalCrypto.Prime + ":" + AtomEnvironment.globalCrypto.Generator + "';");
                    }
                    Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Clave de encriptación → Listo! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");


                    isLive = true;
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(" [" + DateTime.Now.ToString("T") + "] Debug → Activo");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] Debug → Inactivo");
                        Logging.DisablePrimaryWriting(false);
                    }

                    //Console.Beep();

                    //Logging.LogCriticalException("FATAIL FAILURE");

                }
            }
            catch (KeyNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] [ERROR] → Por favor, revisa tu archivo de configuración");
                Console.ResetColor();
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] [INFO] → Presiona una tecla para cerrar el servidor");
                Logging.WriteLine(e.ToString());
                Console.ReadKey(true);
                AtomEnvironment.Destroy();

                return;
            }
            catch (InvalidOperationException e)
            {
                Logging.WriteLine("Failed to initialize Atom Emulator: " + e.Message);
                Logging.WriteLine("Press any key to shut down ...");

                Console.ReadKey(true);
                AtomEnvironment.Destroy();

                return;
            }

            catch (Exception e)
            {
                Console.WriteLine("Fatal error during startup: " + e.ToString());
                Console.WriteLine("Press a key to exit");

                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        internal static void InitIRC()
        {
            //if (ButterflyEnvironment.GetConfig().data["irc.enabled"] == "true")
            //{
            //    UserFactory.Init();

            //    messagingBot = new IRCBot(
            //        ButterflyEnvironment.GetConfig().data["irc.server"],
            //        int.Parse(ButterflyEnvironment.GetConfig().data["irc.port"]),
            //        ButterflyEnvironment.GetConfig().data["irc.user"],
            //        ButterflyEnvironment.GetConfig().data["irc.nick"],
            //        ButterflyEnvironment.GetConfig().data["irc.channel"],
            //        ButterflyEnvironment.GetConfig().data["irc.password"]);

            //    messagingBot.Start();
            //    IrcEnabled = true;
            //}
        }

        //private static string encodeVL64(int i)
        //{
        //    byte[] wf = new byte[6];
        //    int pos = 0;
        //    int startPos = pos;
        //    int bytes = 1;
        //    int negativeMask = i >= 0 ? 0 : 4;
        //    i = Math.Abs(i);
        //    wf[pos++] = (byte)(64 + (i & 3));
        //    for (i >>= 2; i != 0; i >>= 6)
        //    {
        //        bytes++;
        //        wf[pos++] = (byte)(64 + (i & 0x3f));
        //    }

        //    wf[startPos] = (byte)(wf[startPos] | bytes << 3 | negativeMask);

        //    System.Text.ASCIIEncoding encoder = new ASCIIEncoding();
        //    string tmp = encoder.GetString(wf);
        //    return tmp.Replace("\0", "");
        //}

        internal static bool EnumToBool(string Enum)
        {
            return (Enum == "1");
        }



        internal static string BoolToEnum(bool Bool)
        {
            if (Bool)
            {
                return "1";
            }

            return "0";
        }

        internal static int GetRandomNumber(int Min, int Max)
        {
            RandomBase Quick = new Quick();
            return Quick.Next(Min, Max);
        }

        internal static int GetUnixTimestamp()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double unixTime = ts.TotalSeconds;
            return (int)unixTime;
        }

        internal static string FilterInjectionChars(string Input)
        {
            return FilterInjectionChars(Input, false);
        }

        private static readonly List<char> allowedchars = new List<char>(new char[]{ 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 
                                                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 
                                                'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.' });

        internal static string FilterFigure(string figure)
        {
            foreach (char character in figure)
            {
                if (!isValid(character))
                    return "lg-3023-1335.hr-828-45.sh-295-1332.hd-180-4.ea-3168-89.ca-1813-62.ch-235-1332";
            }

            return figure;
        }

        private static bool isValid(char character)
        {
            return allowedchars.Contains(character);
        }

        internal static string FilterInjectionChars(string Input, bool AllowLinebreaks)
        {
            /*Input = Input.Replace(Convert.ToChar(1), ' ');
            Input = Input.Replace(Convert.ToChar(2), ' ');
            //Input = Input.Replace(Convert.ToChar(3), ' ');
            Input = Input.Replace(Convert.ToChar(9), ' ');

            if (!AllowLinebreaks)
            {
                Input = Input.Replace(Convert.ToChar(13), ' ');
            }*/

            return Input;
        }

        internal static bool IsValidAlphaNumeric(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }

            for (int i = 0; i < inputStr.Length; i++)
            {
                if (!(char.IsLetter(inputStr[i])) && (!(char.IsNumber(inputStr[i]))))
                {
                    return false;
                }
            }

            return true;
        }

        private static Dictionary<uint, Habbo> usersCached = new Dictionary<uint, Habbo>();
        internal static Habbo getHabboForId(uint UserId)
        {
            try
            {
                HabboHotel.GameClients.GameClient game = GetGame().GetClientManager().GetClientByUserID(UserId);
                if (game != null)
                {
                    Habbo noUser = game.GetHabbo();
                    if (noUser != null && noUser.Id > 0)
                    {
                        if (usersCached.ContainsKey(UserId))
                            usersCached.Remove(UserId);
                        return noUser;
                    }
                }
                else
                {
                    if (usersCached.ContainsKey(UserId))
                        return usersCached[UserId];
                    else
                    {
                        UserData data = UserDataFactory.GetUserData((int)UserId);
                        Habbo Generated = data.user;
                        if (Generated != null)
                        {
                            Generated.InitInformation(data);
                            usersCached.Add(UserId, Generated);
                            return Generated;
                        }

                    }
                }
                return null;
            }
            catch { return null; }
        }

        internal static Habbo getHabboForName(String UserName)
        {
            try
            {
                using (IQueryAdapter dbClient = GetDatabaseManager().getQueryreactor())
                {
                    dbClient.setQuery("SELECT id FROM users WHERE username = '" + UserName + "'");
                    int id = dbClient.getInteger();
                    if (id > 0)
                    {
                        return getHabboForId((uint)id);
                    }
                }
                return null;
            }
            catch { return null; }
        }

        internal static ConfigurationData GetConfig()
        {
            return Configuration;
        }

        internal static ConfigurationData GetLicense()
        {
            return License;
        }

        //internal static DatabaseManagerOld GetDatabase()
        //{
        //    return DatabaseManager;
        //}

        internal static Encoding GetDefaultEncoding()
        {
            return DefaultEncoding;
        }

        internal static ConnectionHandeling GetConnectionManager()
        {
            return ConnectionManager;
        }

        internal static Game GetGame()
        {
            return Game;
        }

        //internal static Game GameInstance
        //{
        //    get
        //    {
        //        return Game;
        //    }
        //    set
        //    {
        //        Game = value;
        //    }
        //}

        internal static void Destroy()
        {
            isLive = false;
            Console.WriteLine(" [" + DateTime.Now.ToString("T") + "] [INFO] → Destruyendo núcleo");

            if (GetGame() != null)
            {
                GetGame().Destroy();
                Game = null;
            }

            if (GetConnectionManager() != null)
            {
                Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] [INFO] → Destruyendo conexiones");
                GetConnectionManager().Destroy();
                //ConnectionManager = null;
            }

            if (manager != null)
            {
                try
                {
                    Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] [INFO] → Destruyendo conexiones MySQL");
                    //GetDatabase().StopClientMonitor();
                    manager.destroy();
                    //GetDatabase().DestroyDatabaseManager();
                    //DatabaseManager = null;
                }
                catch { }
            }

            Logging.WriteLine(" [" + DateTime.Now.ToString("T") + "] [INFO] → Servidor cerrado con éxito");

            //Environment.Exit(0); Cba :P
        }

        private static bool ShutdownInitiated = false;

        internal static bool ShutdownStarted
        {
            get
            {
                return ShutdownInitiated;
            }
        }

        internal static void SendMassMessage(string Message)
        {
            try
            {
                ServerMessage HotelAlert = new ServerMessage(Outgoing.BroadcastMessage);
                HotelAlert.AppendStringWithBreak(Message);
                AtomEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(HotelAlert);
            }
            catch (Exception e) { Logging.HandleException(e, "ButterflyEnvironment.SendMassMessage"); }
        }

        internal static DatabaseManager GetDatabaseManager()
        {
            return manager;
        }

        internal static void PreformShutDown()
        {
            PreformShutDown(true);
        }

        internal static void PreformShutDown(bool ExitWhenDone)
        {
            if (ShutdownInitiated || !isLive)
                return;

            StringBuilder builder = new StringBuilder();

            DateTime ShutdownStart = DateTime.Now;

            DateTime MessaMessage = DateTime.Now;
            ShutdownInitiated = true;

            SendMassMessage(LanguageLocale.GetValue("shutdown.alert"));
            AppendTimeStampWithComment(ref builder, MessaMessage, "Hotel pre-warning");

            Game.StopGameLoop();
            Console.Write("Game loop stopped");

            DateTime ConnectionClose = DateTime.Now;
            Console.WriteLine("Server shutting down...");
            Console.Title = "<<- SERVER SHUTDOWN ->>";

            GetConnectionManager().Destroy();
            AppendTimeStampWithComment(ref builder, ConnectionClose, "Socket close");

            DateTime sConnectionClose = DateTime.Now;
            GetGame().GetClientManager().CloseAll();
            AppendTimeStampWithComment(ref builder, sConnectionClose, "Furni pre-save and connection close");

            DateTime RoomRemove = DateTime.Now;
            Console.WriteLine("<<- SERVER SHUTDOWN ->> ROOM SAVE");
            Game.GetRoomManager().RemoveAllRooms();
            AppendTimeStampWithComment(ref builder, RoomRemove, "Room destructor");

            DateTime DbSave = DateTime.Now;

            using (IQueryAdapter dbClient = manager.getQueryreactor())
            {
                dbClient.runFastQuery("TRUNCATE TABLE user_tickets");
                dbClient.runFastQuery("TRUNCATE TABLE user_online");
                dbClient.runFastQuery("TRUNCATE TABLE room_active");
                dbClient.runFastQuery("UPDATE users SET online = 0");
                dbClient.runFastQuery("UPDATE rooms SET users_now = 0");
            }
            AppendTimeStampWithComment(ref builder, DbSave, "Database pre-save");

            DateTime connectionShutdown = DateTime.Now;
            ConnectionManager.Destroy();
            AppendTimeStampWithComment(ref builder, connectionShutdown, "Connection shutdown");

            DateTime gameDestroy = DateTime.Now;
            Game.Destroy();
            AppendTimeStampWithComment(ref builder, gameDestroy, "Game destroy");

            DateTime databaseDeconstructor = DateTime.Now;
            try
            {
                Console.WriteLine("Destroying database manager.");

                manager.destroy();
            }
            catch { }
            AppendTimeStampWithComment(ref builder, databaseDeconstructor, "Database shutdown");

            TimeSpan timeUsedOnShutdown = DateTime.Now - ShutdownStart;
            builder.AppendLine("Total time on shutdown " + TimeSpanToString(timeUsedOnShutdown));
            builder.AppendLine("You have reached ==> [END OF SESSION]");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine();

            Logging.LogShutdown(builder);

            Console.WriteLine("System disposed, goodbye!");
            if (ExitWhenDone)
                Environment.Exit(Environment.ExitCode);
        }

        internal static string TimeSpanToString(TimeSpan span)
        {
            return span.Seconds + " s, " + span.Milliseconds + " ms";
        }

        internal static void AppendTimeStampWithComment(ref StringBuilder builder, DateTime time, string text)
        {
            builder.AppendLine(text + " =>[" + TimeSpanToString(DateTime.Now - time) + "]");
        }


    }
}