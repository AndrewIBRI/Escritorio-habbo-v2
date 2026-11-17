using System;
using System.Data;
using Atom.HabboHotel.Misc;
using Atom.HabboHotel.Rooms;
using Atom.HabboHotel.Users;
using Atom.HabboHotel.Users.Badges;
using Database_Manager.Database.Session_Details.Interfaces;
using HabboEvents;
using Database_Manager;
using Atom.Messages;
using Atom.HabboHotel.GameClients;

namespace Atom.Messages
{

    public static class TimeHelper
    {
        public static int ago(string timex)
        {
            DateTime time = DateTime.Parse(timex);
            TimeSpan difference = DateTime.Now - time;
            int ret = 0;
            if (difference.Days > 365)
            {
                int numYears = difference.Days / 365;
                return numYears;
            }
            else
            {
                if (difference.Minutes != 0)
                {
                    ret = difference.Minutes;
                }

            }
            return ret;
        }
    }
    partial class GameClientMessageHandler
    {

        internal void StreamLike()
        {
            int ID = Request.PopWiredInt32();

            using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("UPDATE friend_stream SET likes=likes+1 WHERE id='" + ID + "'");
            }
        }
        internal void SendToStream()
        {
            string MessageX = Request.PopFixedString();
            Console.WriteLine(" [" + DateTime.Now.ToString("T") + "] [STREAM] → " + Session.GetHabbo().Username + ": " + MessageX);
            Console.ResetColor();

            //int time = (TimeHelper.ago(DateTime.Now));
            DateTime timex = DateTime.Now;
            if (MessageX.StartsWith("{Oficial}"))
            {
                if (Session.GetHabbo().Rank >= 7)
                {
                    using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.runFastQuery("INSERT INTO friend_stream (user, text, type, likes, time) VALUES ('" + Session.GetHabbo().Username + "', '" + MessageX + "', '4', '0', '" + timex + "')");
                    }
                }
                else
                {
                    Session.SendNotif("No tienes acceso a publicar una notificación Oficial");
                }
            }
            else
            {
                using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("INSERT INTO friend_stream (user, text, type, likes, time) VALUES ('" + Session.GetHabbo().Username + "', '" + MessageX + "', '5', '0', '" + timex + "')");
                }
            }









            #region Reload Stream

            int StreamCount = 0;
            using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT * FROM friend_stream ORDER BY id DESC LIMIT 15");
                DataTable dTable = dbClient.getTable();
                foreach (DataRow dRow in dTable.Rows)
                {
                    StreamCount = StreamCount + 1;
                }
                GetResponse().Init(3394);
                GetResponse().AppendInt32(StreamCount);
                foreach (DataRow dRow in dTable.Rows)
                {
                    dbClient.setQuery("SELECT id,username,look,gender,rank FROM users WHERE username='" + (string)dRow[1] + "' ORDER BY id DESC LIMIT 1");
                    DataTable mTable = dbClient.getTable();
                    foreach (DataRow xRow in mTable.Rows)
                    {
                        int time = TimeHelper.ago((string)dRow[5]);
                        string username = (string)dRow[1];
                        string Message = (string)dRow[2];
                        GetResponse().AppendInt32((int)dRow[0]); // Message ID
                        if ((int)dRow[3] == 4)
                        {

                            GetResponse().AppendInt32(4); // User Sent
                            GetResponse().AppendString(Convert.ToString(dRow[0])); // String ID
                            GetResponse().AppendString((string)xRow[1]);
                            GetResponse().AppendString((string)xRow[3]);
                            GetResponse().AppendString("http://www.pixelizados.org/swfs/c_images/album1584/" + (string)xRow[2] + ".gif"); // User-Image/Badge
                            GetResponse().AppendInt32(time); // Time
                            GetResponse().AppendInt32(3);
                            GetResponse().AppendInt32((int)dRow[4]); // Likes
                            GetResponse().AppendBoolean(true);
                            GetResponse().AppendBoolean(true);
                            GetResponse().AppendBoolean(false);
                            Message = Message.Replace("{Oficial}", "");
                            GetResponse().AppendString(Message); // Message

                        }
                        else if ((int)dRow[3] == 5)
                        {
                            GetResponse().AppendInt32(5); // User Sent
                            GetResponse().AppendString(Convert.ToString(dRow[0])); // String ID
                            GetResponse().AppendString((string)xRow[1]);
                            GetResponse().AppendString((string)xRow[3]);
                            GetResponse().AppendString("http://www.pixelizados.org/swfs/c_images/album1584/" + (string)xRow[2] + ".gif"); // User-Image/Badge
                            GetResponse().AppendInt32(time); // Time
                            GetResponse().AppendInt32(0);
                            GetResponse().AppendInt32((int)dRow[4]); // Likes
                            GetResponse().AppendBoolean(true);
                            GetResponse().AppendBoolean(true);
                            GetResponse().AppendBoolean(false);
                            GetResponse().AppendString(Message); // Message

                        }
                    }
                }
                SendResponse();
            }
            #endregion
        }
        internal void InitStream()
        {
            int StreamCount = 0;
            using (IQueryAdapter dbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT * FROM friend_stream ORDER BY id DESC LIMIT 15");
                DataTable dTable = dbClient.getTable();
                foreach (DataRow dRow in dTable.Rows)
                {
                    StreamCount = StreamCount + 1;
                }
                GetResponse().Init(3394);
                GetResponse().AppendInt32(StreamCount);
                foreach (DataRow dRow in dTable.Rows)
                {
                    dbClient.setQuery("SELECT id,username,look,gender,rank FROM users WHERE username='" + (string)dRow[1] + "' ORDER BY id DESC LIMIT 1");
                    DataTable mTable = dbClient.getTable();
                    foreach (DataRow xRow in mTable.Rows)
                    {
                        int time = TimeHelper.ago((string)dRow[5]);
                        string username = (string)dRow[1];
                        string Message = (string)dRow[2];
                        GetResponse().AppendInt32((int)dRow[0]); // Message ID
                        if ((int)dRow[3] == 4)
                        {

                            GetResponse().AppendInt32(4); // User Sent
                            GetResponse().AppendString(Convert.ToString(dRow[0])); // String ID
                            GetResponse().AppendString((string)xRow[1]);
                            GetResponse().AppendString((string)xRow[3]);
                            GetResponse().AppendString("http://www.habbo.es/habbo-imaging/avatarimage?figure=" + (string)xRow[2] + ".gif"); // User-Image/Badge
                            GetResponse().AppendInt32(time); // Time
                            GetResponse().AppendInt32(3);
                            GetResponse().AppendInt32((int)dRow[4]); // Likes
                            GetResponse().AppendBoolean(true);
                            GetResponse().AppendBoolean(true);
                            GetResponse().AppendBoolean(false);
                            Message = Message.Replace("{Oficial}", "");
                            GetResponse().AppendString(Message); // Message

                        }
                        else if ((int)dRow[3] == 5)
                        {
                            GetResponse().AppendInt32(5); // User Sent
                            GetResponse().AppendString(Convert.ToString(dRow[0])); // String ID
                            GetResponse().AppendString((string)xRow[1]);
                            GetResponse().AppendString((string)xRow[3]);
                            GetResponse().AppendString("http://www.habbo.es/habbo-imaging/avatarimage?figure=" + (string)xRow[2] + ".gif"); // User-Image/Badge
                            GetResponse().AppendInt32(time); // Time
                            GetResponse().AppendInt32(0);
                            GetResponse().AppendInt32((int)dRow[4]); // Likes
                            GetResponse().AppendBoolean(true);
                            GetResponse().AppendBoolean(true);
                            GetResponse().AppendBoolean(false);
                            GetResponse().AppendString(Message); // Message

                        }
                    }
                }
                SendResponse();
            }
        }
    }
}