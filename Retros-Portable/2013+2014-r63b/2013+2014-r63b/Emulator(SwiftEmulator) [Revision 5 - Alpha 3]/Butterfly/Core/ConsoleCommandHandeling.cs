namespace Butterfly.Core
{
    using Butterfly;
    using Butterfly.HabboHotel;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.Messages;
    using ConnectionManager;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEncryption;
    using HabboEvents;
    using System;
    using System.IO;
    using System.Runtime;
    using System.Text;

    internal class ConsoleCommandHandeling
    {
        internal static bool isWaiting = false;

        internal static Game getGame()
        {
            return ButterflyEnvironment.GetGame();
        }

        internal static void InvokeCommand(string inputData)
        {
            if (string.IsNullOrEmpty(inputData) && Logging.DisabledState)
            {
                return;
            }
            Console.WriteLine();
            if (!Logging.DisabledState)
            {
                Logging.DisabledState = true;
                Console.WriteLine("Console writing disabled. Waiting for user input.");
                return;
            }
            try
            {
                uint num;
                IQueryAdapter adapter;
                string str;
                ServerMessage message;
                string[] strArray = inputData.Split(new char[] { ' ' });
                string str7 = strArray[0];
                switch (str7)
                {
                    case "roomload":
                        if (strArray.Length > 2)
                        {
                            break;
                        }
                        Console.WriteLine("Please sepcify the amount of rooms to load including the startID ");
                        goto Label_0FDC;

                    case "whatlicense":
                        Console.WriteLine(" License Serial: " + ButterflyEnvironment.InteralLicense);
                        goto Label_0FDC;

                    case "loadrooms":
                    {
                        num = uint.Parse(strArray[1]);
                        RoomLoader loader = new RoomLoader(num);
                        Console.WriteLine("Starting loading " + num + " rooms");
                        goto Label_0FDC;
                    }
                    case "systemmute":
                        ButterflyEnvironment.SystemMute = !ButterflyEnvironment.SystemMute;
                        if (!ButterflyEnvironment.SystemMute)
                        {
                            goto Label_02DD;
                        }
                        Console.WriteLine("Mute started");
                        goto Label_0FDC;

                    case "shutdown":
                        Logging.LogMessage("Server exiting at " + DateTime.Now);
                        Logging.DisablePrimaryWriting(true);
                        Console.WriteLine("The server is saving users furniture, rooms, etc. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!");
                        ButterflyEnvironment.PreformShutDown(true);
                        goto Label_0FDC;

                    case "flush":
                        if (strArray.Length >= 2)
                        {
                            goto Label_0348;
                        }
                        Console.WriteLine("You need to specify a parameter within your command. Type help for more information");
                        goto Label_0FDC;

                    case "view":
                        if (strArray.Length >= 2)
                        {
                            goto Label_0692;
                        }
                        Console.WriteLine("You need to specify a parameter within your command. Type help for more information");
                        goto Label_0FDC;

                    case "alert":
                        str = inputData.Substring(6);
                        message = new ServerMessage(Outgoing.SetCommandsView);
                        message.AppendInt32(1);
                        message.AppendString(LanguageLocale.GetValue("console.noticefromadmin") + "\n\n" + str);
                        getGame().GetClientManager().QueueBroadcaseMessage(message);
                        Console.WriteLine("[" + str + "] sent");
                        goto Label_0FDC;

                    case "broadcastalert":
                        str = inputData.Substring(15);
                        message = new ServerMessage(Outgoing.BroadcastMessage);
                        message.AppendString(LanguageLocale.GetValue("console.noticefromadmin") + "\n\n" + str);
                        message.AppendString("");
                        getGame().GetClientManager().QueueBroadcaseMessage(message);
                        Console.WriteLine("[" + str + "] sent");
                        goto Label_0FDC;

                    case "version":
                        Console.WriteLine("Carlos D Edition(s)");
                        goto Label_0FDC;

                    case "help":
                        Console.WriteLine("shutdown - shuts down the server");
                        Console.WriteLine("flush");
                        Console.WriteLine("     settings");
                        Console.WriteLine("          catalog - flushes catalog");
                        Console.WriteLine("          modeldata - flushes modeldata");
                        Console.WriteLine("          bans - flushes bans");
                        Console.WriteLine("     users - disconnects everyone that does not got a user");
                        Console.WriteLine("     connections - closes all server connectinons");
                        Console.WriteLine("     rooms - unloads all rooms");
                        Console.WriteLine("     ddosprotection - flushes ddos protection");
                        Console.WriteLine("     console - clears console");
                        Console.WriteLine("     toilet - flushes the toilet");
                        Console.WriteLine("     cache - flushes the cache");
                        Console.WriteLine("     commands - flushes the commands");
                        Console.WriteLine("view");
                        Console.WriteLine("     connections - views connections");
                        Console.WriteLine("     users - views users");
                        Console.WriteLine("     rooms - views rooms");
                        Console.WriteLine("     dbconnections - views active database connections");
                        Console.WriteLine("     console - views server output (Press enter to disable)");
                        Console.WriteLine("          Note: Parameter stat shows sumary instead of list");
                        Console.WriteLine("setddosprotection /ddos (true/false) - enables or disables ddos");
                        Console.WriteLine("alert (message) - sends alert to everyone online");
                        Console.WriteLine("broadcastalert (message) - sends broadcast alert to everyone online");
                        Console.WriteLine("help - shows commandlist");
                        Console.WriteLine("runquery - runs a query");
                        Console.WriteLine("diagdump - dumps data to file for diagnostic");
                        Console.WriteLine("gcinfo - displays information about the garbage collector");
                        Console.WriteLine("refreshitems - Refreshes items definition");
                        Console.WriteLine("setgc - sets the behaviour type of the garbage collector");
                        goto Label_0FDC;

                    case "refreshitems":
                        getGame().reloaditems();
                        Console.WriteLine("Item definition reloaded");
                        goto Label_0FDC;

                    case "runquery":
                    {
                        string query = inputData.Substring(9);
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.runFastQuery(query);
                        }
                        goto Label_0FDC;
                    }
                    case "queues":
                        Console.WriteLine("============Queues Status============");
                        Console.WriteLine(string.Concat(new object[] { "RC4 Queue", RC4.current.Count, " / ", RC4.concurrent }));
                        goto Label_0FDC;

                    case "diagdump":
                    {
                        DateTime now = DateTime.Now;
                        StringBuilder builder = new StringBuilder();
                        Console.WriteLine();
                        Console.WriteLine("============== SYSTEM DIAGNOSTICS DUMP ==============");
                        Console.WriteLine("Starting diagnostic dump at " + now.ToString());
                        Console.WriteLine();
                        builder.AppendLine("============== SYSTEM DIAGNOSTICS DUMP ==============");
                        builder.AppendLine("Starting diagnostic dump at " + now.ToString());
                        builder.AppendLine();
                        TimeSpan span = (TimeSpan) (DateTime.Now - ButterflyEnvironment.ServerStarted);
                        string str3 = string.Concat(new object[] { "Server uptime: ", span.Days, " day(s), ", span.Hours, " hour(s) and ", span.Minutes, " minute(s)" });
                        string str4 = "Active TCP connections: " + ButterflyEnvironment.GetGame().GetClientManager().ClientCount;
                        string str5 = "Active rooms: " + ButterflyEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;
                        Console.WriteLine(str3);
                        Console.WriteLine(str4);
                        Console.WriteLine(str5);
                        builder.AppendLine(str3);
                        builder.AppendLine(str4);
                        builder.AppendLine(str5);
                        Console.WriteLine();
                        builder.AppendLine();
                        Console.WriteLine("=== DATABASE STATUS ===");
                        builder.AppendLine("=== DATABASE STATUS ===");
                        builder.AppendLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("=== GAME LOOP STATUS ===");
                        builder.AppendLine();
                        builder.AppendLine("=== GAME LOOP STATUS ===");
                        string str6 = "Game loop status: " + ButterflyEnvironment.GetGame().GameLoopStatus;
                        Console.WriteLine(str6);
                        builder.AppendLine(str6);
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("Writing dumpfile...");
                        FileStream stream = new FileStream(@"Logs\dump" + now.ToString().Replace(':', '.').Replace(" ", string.Empty).Replace(@"\", ".") + ".txt", FileMode.Append, FileAccess.Write);
                        byte[] bytes = Encoding.ASCII.GetBytes(builder.ToString());
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Dispose();
                        Console.WriteLine("Done!");
                        goto Label_0FDC;
                    }
                    case "gcinfo":
                        Console.WriteLine("Mode: " + GCSettings.LatencyMode.ToString());
                        Console.WriteLine("Enabled: " + GCSettings.IsServerGC);
                        goto Label_0FDC;

                    case "setgc":
                        str7 = strArray[1].ToLower();
                        if ((str7 != null) && !(str7 == "interactive"))
                        {
                            if (str7 == "batch")
                            {
                                goto Label_0D7C;
                            }
                            if (str7 == "lowlatency")
                            {
                                goto Label_0D86;
                            }
                        }
                        GCSettings.LatencyMode = GCLatencyMode.Interactive;
                        goto Label_0D90;

                    case "packetdiag":
                        ButterflyEnvironment.diagPackets = !ButterflyEnvironment.diagPackets;
                        if (!ButterflyEnvironment.diagPackets)
                        {
                            goto Label_0DDA;
                        }
                        Console.WriteLine("Packet diagnostic enabled");
                        goto Label_0FDC;

                    case "settimeout":
                    {
                        int num4 = int.Parse(strArray[1]);
                        ButterflyEnvironment.timeout = num4;
                        Console.WriteLine("Packet timeout set to " + num4 + "ms");
                        goto Label_0FDC;
                    }
                    case "trigmodule":
                        switch (strArray[1].ToLower())
                        {
                            case "send":
                                goto Label_0E85;

                            case "receive":
                                goto Label_0EBE;

                            case "roomcycle":
                                goto Label_0EF7;

                            case "gamecycle":
                                goto Label_0F30;

                            case "db":
                                goto Label_0F66;
                        }
                        goto Label_0F9C;

                    default:
                        goto Label_0FAC;
                }
                num = uint.Parse(strArray[1]);
                uint num2 = uint.Parse(strArray[2]);
                for (uint i = num2; i < (num2 + num); i++)
                {
                    getGame().GetRoomManager().LoadRoom(i);
                }
                Console.WriteLine(string.Format("{0} rooms loaded", num));
                goto Label_0FDC;
            Label_02DD:
                Console.WriteLine("Mute ended");
                goto Label_0FDC;
            Label_0348:
                switch (strArray[1])
                {
                    case "database":
                        ButterflyEnvironment.GetDatabaseManager().destroy();
                        Console.WriteLine("Closed old connections");
                        goto Label_0FDC;

                    case "settings":
                        if (strArray.Length >= 3)
                        {
                            break;
                        }
                        Console.WriteLine("You need to specify a parameter within your command. Type help for more information");
                        goto Label_0FDC;

                    case "ddosprotection":
                    case "irc":
                        goto Label_0FDC;

                    case "console":
                        Console.Clear();
                        goto Label_0FDC;

                    case "toilet":
                        Console.WriteLine("Flushing toilet...");
                        Console.WriteLine("*SPLOUSH*");
                        Console.WriteLine("Toilet flushed");
                        goto Label_0FDC;

                    case "memory":
                        GC.Collect();
                        Console.WriteLine("Memory flushed");
                        goto Label_0FDC;

                    default:
                        unknownCommand(inputData);
                        goto Label_0FDC;
                }
                str7 = strArray[2];
                if (str7 != null)
                {
                    if (!(str7 == "catalog"))
                    {
                        if (str7 == "modeldata")
                        {
                            goto Label_0519;
                        }
                        if (str7 == "bans")
                        {
                            goto Label_057E;
                        }
                        if (str7 == "commands")
                        {
                            goto Label_05CE;
                        }
                        if (str7 == "language")
                        {
                            goto Label_05F9;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Flushing catalog settings");
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            getGame().GetCatalog().Initialize(adapter);
                        }
                        getGame().GetCatalog().InitCache();
                        getGame().GetClientManager().QueueBroadcaseMessage(new ServerMessage(0x1b9));
                        Console.WriteLine("Catalog flushed");
                    }
                }
                goto Label_0FDC;
            Label_0519:
                Console.WriteLine("Flushing modeldata");
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    getGame().GetRoomManager().LoadModels(adapter);
                    getGame().GetRoomManager().InitRoomLinks(adapter);
                }
                Console.WriteLine("Models flushed");
                goto Label_0FDC;
            Label_057E:
                Console.WriteLine("Flushing bans");
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    getGame().GetBanManager().LoadBans(adapter);
                }
                Console.WriteLine("Bans flushed");
                goto Label_0FDC;
            Label_05CE:
                Console.WriteLine("Flushing commands");
                ChatCommandRegister.Init();
                PetCommandHandeler.Init();
                PetLocale.Init();
                Console.WriteLine("Commands flushed");
                goto Label_0FDC;
            Label_05F9:
                Console.WriteLine("Flushing language files");
                LanguageLocale.Init();
                Console.WriteLine("Language files flushed");
                goto Label_0FDC;
            Label_0692:
                str7 = strArray[1];
                if (str7 == null)
                {
                    goto Label_0769;
                }
                if (!(str7 == "connections"))
                {
                    if (str7 == "users")
                    {
                        goto Label_0706;
                    }
                    if (str7 == "rooms")
                    {
                        goto Label_072D;
                    }
                    if (str7 == "console")
                    {
                        goto Label_0754;
                    }
                    goto Label_0769;
                }
                Console.WriteLine("Connection count: " + getGame().GetClientManager().connectionCount);
                goto Label_0FDC;
            Label_0706:
                Console.WriteLine("User count: " + getGame().GetClientManager().ClientCount);
                goto Label_0FDC;
            Label_072D:
                Console.WriteLine("Loaded room count: " + getGame().GetRoomManager().LoadedRoomsCount);
                goto Label_0FDC;
            Label_0754:
                Console.WriteLine("Press ENTER for disabling console writing");
                Logging.DisabledState = false;
                goto Label_0FDC;
            Label_0769:
                unknownCommand(inputData);
                goto Label_0FDC;
            Label_0D7C:
                GCSettings.LatencyMode = GCLatencyMode.Batch;
                goto Label_0D90;
            Label_0D86:
                GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            Label_0D90:
                Console.WriteLine("Latency mode set to: " + GCSettings.LatencyMode);
                goto Label_0FDC;
            Label_0DDA:
                Console.WriteLine("Packet diagnostic disabled");
                goto Label_0FDC;
            Label_0E85:
                if (ConnectionInformation.disableSend = !ConnectionInformation.disableSend)
                {
                    Console.WriteLine("Data sending disabled");
                }
                else
                {
                    Console.WriteLine("Data sending enabled");
                }
                goto Label_0FDC;
            Label_0EBE:
                if (ConnectionInformation.disableReceive = !ConnectionInformation.disableReceive)
                {
                    Console.WriteLine("Data receiving disabled");
                }
                else
                {
                    Console.WriteLine("Data receiving enabled");
                }
                goto Label_0FDC;
            Label_0EF7:
                if (RoomManager.roomCyclingEnabled = !RoomManager.roomCyclingEnabled)
                {
                    Console.WriteLine("Room cycling enabled");
                }
                else
                {
                    Console.WriteLine("Room cycling disabled");
                }
                goto Label_0FDC;
            Label_0F30:
                if (Game.gameLoopEnabled = !Game.gameLoopEnabled)
                {
                    Console.WriteLine("Game loop started");
                }
                else
                {
                    Console.WriteLine("Game loop stopped");
                }
                goto Label_0FDC;
            Label_0F66:
                if (DatabaseManager.dbEnabled = !DatabaseManager.dbEnabled)
                {
                    Console.WriteLine("Db enabled");
                }
                else
                {
                    Console.WriteLine("Db stopped");
                }
                goto Label_0FDC;
            Label_0F9C:
                Console.WriteLine("Unknown module");
                goto Label_0FDC;
            Label_0FAC:
                unknownCommand(inputData);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error in command [" + inputData + "]: " + exception.ToString());
            }
        Label_0FDC:
            Console.WriteLine();
        }

        private static void unknownCommand(string command)
        {
            Console.WriteLine(command + " is an unknown or unsupported command. Type help for more information");
        }
    }
}

