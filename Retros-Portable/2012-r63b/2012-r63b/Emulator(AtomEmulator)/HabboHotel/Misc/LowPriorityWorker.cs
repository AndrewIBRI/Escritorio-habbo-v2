using System;
using System.Threading;
using Atom.Core;
using Atom.Net;
using System.Text;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Atom.HabboHotel.Misc
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
                        int UsersOnline = AtomEnvironment.GetGame().GetClientManager().ClientCount;
                        int RoomsLoaded = AtomEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;

                        TimeSpan Uptime = DateTime.Now - AtomEnvironment.ServerStarted;
                        string addOn = string.Empty;
                        if (System.Diagnostics.Debugger.IsAttached)
                            addOn = "[DEBUG] ";
                        Console.Title = addOn + "Atom | Tiempo online: " + Uptime.Minutes + " minutos, " + Uptime.Hours + " horas y " + Uptime.Days + " días | " +
                        "Online users: " + AtomEnvironment.GetGame().GetClientManager().ClientCount + " | Salas cargadas: " + AtomEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;

                        #region Garbage Collection
                        //GC.Collect();
                        //GC.WaitForPendingFinalizers();
                        #endregion

                        #region Statistics


                        if (UsersOnline > UserPeak)
                            UserPeak = UsersOnline;

                        using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            dbClient.runFastQuery("UPDATE server_status SET stamp = '" + AtomEnvironment.GetUnixTimestamp() + "', status = " + Status + ", users_online = " + UsersOnline + ", rooms_loaded = " + RoomsLoaded + ", server_ver = '" + AtomEnvironment.PrettyVersion + "', userpeak = " + UserPeak + "");
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
                AtomEnvironment.GetGame().LowPriorityWorker_ended = true;
            }
            catch (Exception e) {  }
        }
    }
}