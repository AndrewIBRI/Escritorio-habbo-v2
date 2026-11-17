namespace ConnectionManager.Messages
{
    using ConnectionManager.LoggingSystem;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections;
    using System.Text;

    internal class MessageLoggerManager
    {
        private static bool enabled;
        private static Queue loggedMessages;
        private static DateTime timeSinceLastPacket;

        internal static void AddMessage(byte[] data, int connectionID, LogState state)
        {
            if (enabled)
            {
                string str;
                switch (state)
                {
                    case LogState.ConnectionOpen:
                        str = "CONCLOSE";
                        break;

                    case LogState.ConnectionClose:
                        str = "CONOPEN";
                        break;

                    default:
                        str = Encoding.Default.GetString(data);
                        break;
                }
                lock (loggedMessages.SyncRoot)
                {
                    Message message = new Message(connectionID, GenerateTimestamp(), str);
                    loggedMessages.Enqueue(message);
                }
            }
        }

        private static int GenerateTimestamp()
        {
            DateTime now = DateTime.Now;
            TimeSpan span = (TimeSpan) (now - timeSinceLastPacket);
            timeSinceLastPacket = now;
            return (int) span.TotalMilliseconds;
        }

        internal static void Save()
        {
            if (enabled)
            {
                lock (loggedMessages.SyncRoot)
                {
                    int count = loggedMessages.Count;
                    if (loggedMessages.Count > 0)
                    {
                        DatabaseManager manager = new DatabaseManager(1, 1, DatabaseType.MySQL);
                        using (IQueryAdapter adapter = manager.getQueryreactor())
                        {
                            while (loggedMessages.Count > 0)
                            {
                                Message message = (Message) loggedMessages.Dequeue();
                                adapter.setQuery("INSERT INTO system_packetlog (connectionid, timestamp, data) VALUES @connectionid @timestamp, @data");
                                adapter.addParameter("connectionid", message.ConnectionID);
                                adapter.addParameter("timestamp", message.GetTimestamp);
                                adapter.addParameter("data", message.GetData);
                                adapter.runQuery();
                            }
                        }
                    }
                }
            }
        }

        internal static bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                if (enabled)
                {
                    loggedMessages = new Queue();
                }
            }
        }
    }
}

