using System;
using System.Threading;
using Butterfly.Core;
using Butterfly.Net;
using System.Text;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.Misc
{
    internal class LowPriorityWorker
    {
        private static int UserPeak;

        private static string FormatToBytes(int Num)
        {
            if (Num < 1024)
                return Num + "B/s";
            else if (Num < 1048576) //KB/s
            {
                double Traffic = Num / 1024;
                return String.Format("{0:0.##}", Traffic) + "KB/s";
            }
            else //MB/s
            {
                double Traffic = Num / 1048576;
                return String.Format("{0:0.##}", Traffic) + "MB/s";
            }
        }

        private static DateTime consoleLastExecution;

        internal static void Init(IQueryAdapter dbClient)
        {
            dbClient.setQuery("SELECT userpeak FROM server_status");
            UserPeak = dbClient.getInteger();
        }

        private static DateTime processLastExecution;
        internal static void Process()
        {
            try
            {
                DateTime startTaskTime;
                TimeSpan spentTime;
                startTaskTime = DateTime.Now;

                TimeSpan sinceLastTime = DateTime.Now - processLastExecution;

                if (sinceLastTime.TotalMilliseconds >= 30000)
                {
                    processLastExecution = DateTime.Now;
                    try
                    {

                        int Status = 1;
                        int UsersOnline = ButterflyEnvironment.GetGame().GetClientManager().ClientCount;
                        int RoomsLoaded = ButterflyEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;

                        TimeSpan Uptime = DateTime.Now - ButterflyEnvironment.ServerStarted;
                        string addOn = string.Empty;
                        if (System.Diagnostics.Debugger.IsAttached)
                            addOn = "[DEBUG] ";
                        Console.Title = addOn + "ButterStorm | Uptime: " + Uptime.Minutes + " minutes, " + Uptime.Hours + " hours and " + Uptime.Days + " day | " +
                            "Online users: " + UsersOnline + " | Loaded rooms: " + RoomsLoaded;

                        #region Garbage Collection
                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();
                        #endregion

                        #region Statistics


                        if (UsersOnline > UserPeak)
                            UserPeak = UsersOnline;

                        using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            dbClient.runFastQuery("UPDATE server_status SET stamp = '" + ButterflyEnvironment.GetUnixTimestamp() + "', status = " + Status + ", users_online = " + UsersOnline + ", rooms_loaded = " + RoomsLoaded + ", server_ver = '" + ButterflyEnvironment.PrettyVersion + "', userpeak = " + UserPeak + "");
                        }
                        #endregion
                    }
                    catch (Exception e) { Logging.LogThreadException(e.ToString(), "Server status update task"); }
                }
                spentTime = DateTime.Now - startTaskTime;

                if (spentTime.TotalSeconds > 3)
                {
                    Console.WriteLine("LowPriorityWorker.Process spent: " + spentTime.TotalSeconds + " seconds in working.");
                }
                ButterflyEnvironment.GetGame().LowPriorityWorker_ended = true;
            }
            catch (Exception e) { Logging.LogThreadException(e.ToString(), "LowPriorityWorker.Process Exception --> Not inclusive"); }
        }
    }
}