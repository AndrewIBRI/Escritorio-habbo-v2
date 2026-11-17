namespace Butterfly.HabboHotel.Misc
{
    using Butterfly;
    using Butterfly.Core;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Diagnostics;

    internal class LowPriorityWorker
    {
        private static DateTime processLastExecution;
        private static int UserPeak;

        private static string FormatToBytes(int Num)
        {
            double num;
            if (Num < 0x400)
            {
                return (Num + "B/s");
            }
            if (Num < 0x100000)
            {
                num = Num / 0x400;
                return (string.Format("{0:0.##}", num) + "KB/s");
            }
            num = Num / 0x100000;
            return (string.Format("{0:0.##}", num) + "MB/s");
        }

        internal static void Init(IQueryAdapter dbClient)
        {
            dbClient.setQuery("SELECT userpeak FROM server_status");
            UserPeak = dbClient.getInteger();
        }

        internal static void Process()
        {
            try
            {
                DateTime now = DateTime.Now;
                TimeSpan span2 = (TimeSpan) (DateTime.Now - processLastExecution);
                if (span2.TotalMilliseconds >= 30000.0)
                {
                    processLastExecution = DateTime.Now;
                    try
                    {
                        int num = 1;
                        int clientCount = ButterflyEnvironment.GetGame().GetClientManager().ClientCount;
                        int loadedRoomsCount = ButterflyEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;

                        //Butterfly.ButterflyEnvironment.GetGame().GetGuilds().OnCycle_Remove();
                        TimeSpan span3 = (TimeSpan) (DateTime.Now - ButterflyEnvironment.ServerStarted);
                        string str = string.Empty;
                        Console.Title = string.Concat(new object[] { str, "swift | Uptime: ", span3.Minutes, " minutes, ", span3.Hours, " hours and ", span3.Days, " day | Online users: ", clientCount, " | Loaded rooms: ", loadedRoomsCount });
                        if (clientCount > UserPeak)
                        {
                            UserPeak = clientCount;
                        }
                        using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.runFastQuery(string.Concat(new object[] { "UPDATE server_status SET stamp = '", ButterflyEnvironment.GetUnixTimestamp(), "', status = ", num, ", users_online = ", clientCount, ", rooms_loaded = ", loadedRoomsCount, ", server_ver = 'Carlos D Edition(s)', userpeak = ", UserPeak }));
                        }
                    }
                    catch (Exception exception1)
                    {
                        Logging.LogThreadException(exception1.ToString(), "Server status update task");
                    }
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("LowPriorityWorker.Process spent: " + span.TotalSeconds + " seconds in working.");
                }
                ButterflyEnvironment.GetGame().LowPriorityWorker_ended = true;
            }
            catch (Exception exception2)
            {
                Logging.LogThreadException(exception2.ToString(), "LowPriorityWorker.Process Exception --> Not inclusive");
            }
        }
    }
}

