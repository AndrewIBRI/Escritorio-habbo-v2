namespace Butterfly.HabboHotel.GameClients
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Misc;
    using Butterfly.HabboHotel.Users.Messenger;
    using Butterfly.Messages;
    using ConnectionManager;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class GameClientManager
    {
        private Queue authorizedPacketSending = new Queue();
        private Queue badgeQueue = new Queue();
        private Queue broadcastQueue = new Queue();
        private Dictionary<uint, GameClient> clients = new Dictionary<uint, GameClient>();
        private Queue clientsAddQueue = new Queue();
        private Queue clientsToRemove = new Queue();
        private Queue creditQueuee = new Queue();
        internal int creditsOnLogin;
        private int cycleCreditsAmount;
        private bool cycleCreditsEnabled;
        private DateTime cycleCreditsLastUpdate;
        private int cycleCreditsTime;
        private int cyclePixelsAmount;
        private bool cyclePixelsEnabled;
        private DateTime cyclePixelsLastUpdate;
        private int cyclePixelsTime;
        internal bool FastRun = false;
        private Task FirstThread_Task;
        private Hashtable idUsernameRegister = new Hashtable();
        private int pingInterval;
        private static DateTime pingLastExecution;
        internal int pixelsOnLogin;
        private Task SecondThread_Task;
        private Task ThirdThread_Task;
        private Queue timedOutConnections = new Queue();
        private Hashtable userIDRegister = new Hashtable();
        private Hashtable usernameIdRegister = new Hashtable();
        private Hashtable usernameRegister = new Hashtable();

        internal GameClientManager()
        {
            new Thread(new ThreadStart(this.HandleTimeouts)).Start();
            this.pingInterval = int.Parse(ButterflyEnvironment.GetConfig().data["client.ping.interval"]);
            if (ButterflyEnvironment.GetConfig().data.ContainsKey("game.pixel.enabled"))
            {
                this.cyclePixelsEnabled = ButterflyEnvironment.GetConfig().data["game.pixel.enabled"] == "true";
                this.cyclePixelsAmount = int.Parse(ButterflyEnvironment.GetConfig().data["game.pixel.amount"]);
                this.cyclePixelsTime = int.Parse(ButterflyEnvironment.GetConfig().data["game.pixel.time"]) * 0x3e8;
                this.pixelsOnLogin = int.Parse(ButterflyEnvironment.GetConfig().data["game.login.pixel.receiveamount"]);
            }
            else
            {
                this.cyclePixelsEnabled = false;
                this.cyclePixelsAmount = 0;
                this.cyclePixelsTime = 0;
                this.pixelsOnLogin = 0;
            }
            if (ButterflyEnvironment.GetConfig().data.ContainsKey("game.credits.enabled"))
            {
                this.cycleCreditsEnabled = ButterflyEnvironment.GetConfig().data["game.credits.enabled"] == "true";
                this.cycleCreditsAmount = int.Parse(ButterflyEnvironment.GetConfig().data["game.credits.amount"]);
                this.cycleCreditsTime = int.Parse(ButterflyEnvironment.GetConfig().data["game.credits.time"]) * 0x3e8;
                this.creditsOnLogin = int.Parse(ButterflyEnvironment.GetConfig().data["game.login.credits.receiveamount"]);
            }
            else
            {
                this.cycleCreditsEnabled = false;
                this.cycleCreditsAmount = 0;
                this.cycleCreditsTime = 0;
                this.creditsOnLogin = 0;
            }
        }

        private void AddClients()
        {
            DateTime now = DateTime.Now;
            if (this.clientsAddQueue.Count > 0)
            {
                lock (this.clientsAddQueue.SyncRoot)
                {
                    while (this.clientsAddQueue.Count > 0)
                    {
                        GameClient client = (GameClient) this.clientsAddQueue.Dequeue();
                        this.clients.Add(client.ConnectionID, client);
                        client.StartConnection();
                    }
                }
            }
            TimeSpan span = (TimeSpan) (DateTime.Now - now);
            if (span.TotalSeconds > 3.0)
            {
                Console.WriteLine("GameClientManager.AddClients spent: " + span.TotalSeconds + " seconds in working.");
            }
        }

        private void BroadcastMessage(ServerMessage message)
        {
            lock (this.broadcastQueue.SyncRoot)
            {
                this.broadcastQueue.Enqueue(message);
            }
        }

        private void BroadcastPackets()
        {
            try
            {
                DateTime now = DateTime.Now;
                if (this.broadcastQueue.Count > 0)
                {
                    lock (this.broadcastQueue.SyncRoot)
                    {
                        while (this.broadcastQueue.Count > 0)
                        {
                            byte[] bytes = ((ServerMessage) this.broadcastQueue.Dequeue()).GetBytes();
                            foreach (GameClient client in this.clients.Values)
                            {
                                try
                                {
                                    client.GetConnection().SendData(bytes);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("GameClientManager.BroadcastPackets spent: " + span.TotalSeconds + " seconds in working.");
                }
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "GameClientManager.BroadcastPackets Exception --> Not inclusive");
            }
        }

        private void BroadcastPacketsWithRankRequirement()
        {
            try
            {
                DateTime now = DateTime.Now;
                if (this.authorizedPacketSending.Count > 0)
                {
                    lock (this.authorizedPacketSending.SyncRoot)
                    {
                        while (this.authorizedPacketSending.Count > 0)
                        {
                            FusedPacket packet = (FusedPacket) this.authorizedPacketSending.Dequeue();
                            foreach (GameClient client in this.clients.Values)
                            {
                                try
                                {
                                    if ((client.GetHabbo() != null) && client.GetHabbo().HasFuse(packet.requirements))
                                    {
                                        client.SendMessage(packet.content);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("GameClientManager.BroadcastPacketsWithRankRequirement spent: " + span.TotalSeconds + " seconds in working.");
                }
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "GameClientManager.BroadcastPacketsWithRankRequirement Exception --> Not inclusive");
            }
        }

        private void CheckCycleUpdates()
        {
            try
            {
                TimeSpan span2;
                DateTime now = DateTime.Now;
                if (this.cyclePixelsEnabled)
                {
                    span2 = (TimeSpan) (DateTime.Now - this.cyclePixelsLastUpdate);
                    if (span2.TotalMilliseconds >= this.cyclePixelsTime)
                    {
                        this.cyclePixelsLastUpdate = DateTime.Now;
                        try
                        {
                            foreach (GameClient client in this.clients.Values)
                            {
                                if (client.GetHabbo() != null)
                                {
                                    PixelManager.GivePixels(client, this.cyclePixelsAmount);
                                }
                            }
                        }
                        catch (Exception exception1)
                        {
                            Exception exception = exception1;
                            Logging.LogThreadException(exception.ToString(), "GCMExt.cyclePixelsEnabled task");
                        }
                    }
                }
                if (this.cycleCreditsEnabled)
                {
                    span2 = (TimeSpan) (DateTime.Now - this.cycleCreditsLastUpdate);
                    if (span2.TotalMilliseconds >= this.cycleCreditsTime)
                    {
                        this.cycleCreditsLastUpdate = DateTime.Now;
                        try
                        {
                            foreach (GameClient client in this.clients.Values)
                            {
                                if (client.GetHabbo() != null)
                                {
                                    Habbo habbo = client.GetHabbo();
                                    habbo.Credits += this.cycleCreditsAmount;
                                    client.GetHabbo().UpdateCreditsBalance();
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            Logging.LogThreadException(exception2.ToString(), "GCMExt.cycleCreditsEnabled task");
                        }
                    }
                }
                this.CheckEffects();
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("GameClientManager.CheckCycleUpdates spent: " + span.TotalSeconds + " seconds in working.");
                }
            }
            catch (Exception exception3)
            {
                Logging.LogThreadException(exception3.ToString(), "GameClientManager.CheckCycleUpdates Exception --> Not inclusive");
            }
        }

        private void CheckEffects()
        {
            foreach (GameClient client in this.clients.Values)
            {
                if ((client.GetHabbo() != null) && (client.GetHabbo().GetAvatarEffectsInventoryComponent() != null))
                {
                    client.GetHabbo().GetAvatarEffectsInventoryComponent().CheckExpired();
                }
            }
        }

        internal void CloseAll()
        {
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            int num = 0;
            foreach (GameClient client in this.clients.Values)
            {
                if (client.GetHabbo() != null)
                {
                    num++;
                }
            }
            if (num < 1)
            {
                num = 1;
            }
            int num2 = 0;
            int count = this.clients.Count;
            foreach (GameClient client in this.clients.Values)
            {
                num2++;
                if (client.GetHabbo() != null)
                {
                    try
                    {
                        client.GetHabbo().GetInventoryComponent().RunDBUpdate();
                        builder.Append(client.GetHabbo().GetQueryString);
                        flag = true;
                        Console.Clear();
                        Console.WriteLine("<<- SERVER SHUTDOWN ->> INVENTORY SAVE: " + string.Format("{0:0.##}", (((double) num2) / ((double) num)) * 100.0) + "%");
                    }
                    catch
                    {
                    }
                }
            }
            if (flag)
            {
                try
                {
                    if (builder.Length > 0)
                    {
                        using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.runFastQuery(builder.ToString());
                        }
                    }
                }
                catch (Exception exception1)
                {
                    Exception pException = exception1;
                    Logging.HandleException(pException, "GameClientManager.CloseAll()");
                }
            }
            Console.WriteLine("Done saving users inventory!");
            Console.WriteLine("Closing server connections...");
            try
            {
                int num4 = 0;
                foreach (GameClient client in this.clients.Values)
                {
                    num4++;
                    if (client.GetConnection() != null)
                    {
                        try
                        {
                            client.GetConnection().Dispose();
                        }
                        catch
                        {
                        }
                        Console.Clear();
                        Console.WriteLine("<<- SERVER SHUTDOWN ->> CONNECTION CLOSE: " + string.Format("{0:0.##}", (((double) num4) / ((double) count)) * 100.0) + "%");
                    }
                }
            }
            catch (Exception exception2)
            {
                Logging.LogCriticalException(exception2.ToString());
            }
            this.clients.Clear();
            Console.WriteLine("Connections closed!");
        }

        internal void CreateAndStartClient(uint clientID, ConnectionInformation connection)
        {
            GameClient client = new GameClient(clientID, connection);
            if (this.clients.ContainsKey(clientID))
            {
                this.clients.Remove(clientID);
            }
            lock (this.clientsAddQueue.SyncRoot)
            {
                this.clientsAddQueue.Enqueue(client);
            }
        }

        internal void DisposeConnection(uint clientID)
        {
            GameClient client = this.GetClient(clientID);
            if (client != null)
            {
                client.Stop();
            }
            lock (this.clientsToRemove.SyncRoot)
            {
                this.clientsToRemove.Enqueue(clientID);
            }
        }

        private void FirstThread_Execution()
        {
            try
            {
                this.GiveCredits();
                this.GiveBadges();
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "GameClientManager.FirstThread_Execution Exception");
            }
        }

        internal GameClient GetClient(uint clientID)
        {
            if (this.clients.ContainsKey(clientID))
            {
                return this.clients[clientID];
            }
            return null;
        }

        internal GameClient GetClientByUserID(uint userID)
        {
            if (this.userIDRegister.ContainsKey(userID))
            {
                return (GameClient) this.userIDRegister[userID];
            }
            return null;
        }

        internal GameClient GetClientByUsername(string username)
        {
            if (this.usernameRegister.ContainsKey(username.ToLower()))
            {
                return (GameClient) this.usernameRegister[username.ToLower()];
            }
            return null;
        }

        internal IEnumerable<GameClient> GetClientsById(Dictionary<uint, MessengerBuddy>.KeyCollection users)
        {
            foreach (uint iteratorVariable0 in users)
            {
                GameClient clientByUserID = this.GetClientByUserID(iteratorVariable0);
                if (clientByUserID != null)
                {
                    yield return clientByUserID;
                }
            }
        }

        internal string GetNameById(uint Id)
        {
            GameClient clientByUserID = this.GetClientByUserID(Id);
            if (clientByUserID != null)
            {
                return clientByUserID.GetHabbo().Username;
            }
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT username FROM users WHERE id = " + Id);
                return adapter.getString();
            }
        }

        internal string GetLookById(uint Id)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT look FROM users WHERE id = " + Id);
                return adapter.getString();
            }
        }

        private void GiveAllOnlineCredits(int amount)
        {
            lock (this.creditQueuee.SyncRoot)
            {
                this.creditQueuee.Enqueue(amount);
            }
        }

        private void GiveBadges()
        {
            try
            {
                DateTime now = DateTime.Now;
                if (this.badgeQueue.Count > 0)
                {
                    lock (this.badgeQueue.SyncRoot)
                    {
                        while (this.badgeQueue.Count > 0)
                        {
                            string badge = (string) this.badgeQueue.Dequeue();
                            foreach (GameClient client in this.clients.Values)
                            {
                                if (client.GetHabbo() != null)
                                {
                                    try
                                    {
                                        client.GetHabbo().GetBadgeComponent().GiveBadge(badge, true);
                                        client.SendNotif(LanguageLocale.GetValue("user.badgereceived"));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("GameClientManager.GiveBadges spent: " + span.TotalSeconds + " seconds in working.");
                }
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "GameClientManager.GiveBadges Exception --> Not inclusive");
            }
        }

        private void GiveCredits()
        {
            try
            {
                DateTime now = DateTime.Now;
                if (this.creditQueuee.Count > 0)
                {
                    lock (this.creditQueuee.SyncRoot)
                    {
                        while (this.creditQueuee.Count > 0)
                        {
                            int num = (int) this.creditQueuee.Dequeue();
                            foreach (GameClient client in this.clients.Values)
                            {
                                if (client.GetHabbo() != null)
                                {
                                    try
                                    {
                                        Habbo habbo = client.GetHabbo();
                                        habbo.Credits += num;
                                        client.GetHabbo().UpdateCreditsBalance();
                                        client.SendNotif(LanguageLocale.GetValue("user.creditsreceived") + ": " + num);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("GameClientManager.GiveCredits spent: " + span.TotalSeconds + " seconds in working.");
                }
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "GameClientManager.GiveCredits Exception --> Not inclusive");
            }
        }

        private void HandleTimeouts()
        {
            while (true)
            {
                try
                {
                    while (this.timedOutConnections.Count > 0)
                    {
                        GameClient client = null;
                        lock (this.timedOutConnections.SyncRoot)
                        {
                            if (this.timedOutConnections.Count > 0)
                            {
                                client = (GameClient) this.timedOutConnections.Dequeue();
                            }
                        }
                        if (client != null)
                        {
                            client.Disconnect();
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.LogThreadException(exception.ToString(), "HandleTimeouts");
                }
                Thread.Sleep(0x1388);
            }
        }

        internal void LogClonesOut(uint UserID)
        {
            GameClient clientByUserID = this.GetClientByUserID(UserID);
            if (clientByUserID != null)
            {
                clientByUserID.Disconnect();
            }
        }

        internal void OnCycle()
        {
            try
            {
                DateTime now = DateTime.Now;
                if (!ButterflyEnvironment.SeparatedTasksInGameClientManager)
                {
                    this.CheckCycleUpdates();
                    this.TestClientConnections();
                    this.RemoveClients();
                    this.AddClients();
                    this.GiveCredits();
                    this.GiveBadges();
                    this.BroadcastPacketsWithRankRequirement();
                    this.BroadcastPackets();
                    ButterflyEnvironment.GetGame().ClientManagerCycle_ended = true;
                }
                else
                {
                    if ((this.FirstThread_Task == null) || this.FirstThread_Task.IsCompleted)
                    {
                        this.FirstThread_Task = new Task(new Action(this.FirstThread_Execution));
                        this.FirstThread_Task.Start();
                    }
                    if ((this.SecondThread_Task == null) || this.SecondThread_Task.IsCompleted)
                    {
                        this.SecondThread_Task = new Task(new Action(this.SecondThread_Execution));
                        this.SecondThread_Task.Start();
                    }
                    if ((this.ThirdThread_Task == null) || this.ThirdThread_Task.IsCompleted)
                    {
                        this.ThirdThread_Task = new Task(new Action(this.ThirdThread_Execution));
                        this.ThirdThread_Task.Start();
                    }
                    while ((!this.FirstThread_Task.IsCompleted || !this.SecondThread_Task.IsCompleted) || !this.ThirdThread_Task.IsCompleted)
                    {
                    }
                    this.RemoveClients();
                    this.AddClients();
                    ButterflyEnvironment.GetGame().ClientManagerCycle_ended = true;
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("GameClientManager.OnCycle spent: " + span.TotalSeconds + " seconds in working.");
                }
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "GameClientManager.OnCycle Exception --> Not inclusive");
            }
        }

        internal void QueueBadgeUpdate(string badge)
        {
            lock (this.badgeQueue.SyncRoot)
            {
                this.badgeQueue.Enqueue(badge);
            }
        }

        internal void QueueBroadcaseMessage(ServerMessage message)
        {
            lock (this.broadcastQueue.SyncRoot)
            {
                this.broadcastQueue.Enqueue(message);
            }
        }

        internal void QueueBroadcaseMessage(ServerMessage message, string requirements)
        {
            FusedPacket packet = new FusedPacket(message, requirements);
            lock (this.authorizedPacketSending.SyncRoot)
            {
                this.authorizedPacketSending.Enqueue(packet);
            }
        }

        internal void QueueCreditsUpdate(int amount)
        {
            lock (this.creditQueuee.SyncRoot)
            {
                this.creditQueuee.Enqueue(amount);
            }
        }

        internal void RegisterClient(GameClient client, uint userID, string username)
        {
            if (this.usernameRegister.ContainsKey(username.ToLower()))
            {
                this.usernameRegister[username.ToLower()] = client;
            }
            else
            {
                this.usernameRegister.Add(username.ToLower(), client);
            }
            if (this.userIDRegister.ContainsKey(userID))
            {
                this.userIDRegister[userID] = client;
            }
            else
            {
                this.userIDRegister.Add(userID, client);
            }
            if (!this.usernameIdRegister.ContainsKey(username))
            {
                this.usernameIdRegister.Add(username, userID);
            }
            if (!this.idUsernameRegister.ContainsKey(userID))
            {
                this.idUsernameRegister.Add(userID, username);
            }
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MySQL)
                {
                    adapter.setQuery("REPLACE INTO user_online VALUES (" + userID + ")");
                }
                else
                {
                    adapter.setQuery(string.Concat(new object[] { "IF NOT EXISTS (SELECT userid FROM user_online WHERE userid = ", userID, ")    INSERT INTO user_online VALUES (", userID, ")" }));
                }
                adapter.runFastQuery("UPDATE users SET online = '1' WHERE id = '" + userID + "'");
            }
        }

        private void RemoveClients()
        {
            try
            {
                DateTime now = DateTime.Now;
                if (this.clientsToRemove.Count > 0)
                {
                    lock (this.clientsToRemove.SyncRoot)
                    {
                        while (this.clientsToRemove.Count > 0)
                        {
                            uint key = (uint) this.clientsToRemove.Dequeue();
                            this.clients.Remove(key);
                        }
                    }
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("GameClientManager.RemoveClients spent: " + span.TotalSeconds + " seconds in working.");
                }
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "GameClientManager.RemoveClients Exception --> Not inclusive");
            }
        }

        private void SecondThread_Execution()
        {
            try
            {
                this.CheckCycleUpdates();
                this.TestClientConnections();
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "GameClientManager.SecondThread_Execution Exception");
            }
        }

        private void TestClientConnections()
        {
            try
            {
                DateTime now = DateTime.Now;
                TimeSpan span2 = (TimeSpan) (DateTime.Now - pingLastExecution);
                if (span2.TotalMilliseconds >= this.pingInterval)
                {
                    try
                    {
                        object obj2;
                        ServerMessage message = new ServerMessage(Outgoing.Ping);
                        List<GameClient> list = new List<GameClient>();
                        foreach (GameClient client in this.clients.Values)
                        {
                            TimeSpan span3 = (TimeSpan) (DateTime.Now - pingLastExecution.AddMilliseconds((double) this.pingInterval));
                            TimeSpan span4 = (TimeSpan) (DateTime.Now - client.TimePingedReceived);
                            if ((span4.TotalMilliseconds - span3.TotalMilliseconds) < (this.pingInterval + 0x2710))
                            {
                                //Console.WriteLine("No dai...");
                                list.Add(client);
                            }
                            else
                            {
                                //Console.WriteLine("Faccio crashare");
                                lock ((obj2 = this.timedOutConnections.SyncRoot))
                                {
                                    this.timedOutConnections.Enqueue(client);
                                }
                            }
                        }
                        DateTime time2 = DateTime.Now;
                        byte[] bytes = message.GetBytes();
                        foreach (GameClient client2 in list)
                        {
                            try
                            {
                                client2.GetConnection().SendUnsafeData(bytes);
                            }
                            catch
                            {
                                lock ((obj2 = this.timedOutConnections.SyncRoot))
                                {
                                    this.timedOutConnections.Enqueue(client2);
                                }
                            }
                        }
                        TimeSpan span5 = (TimeSpan) (DateTime.Now - time2);
                        if (span5.TotalSeconds > 3.0)
                        {
                            Console.WriteLine("Spent seconds on testing: " + ((int) span5.TotalSeconds));
                        }
                        if (span5.TotalSeconds > 3.0)
                        {
                            Console.WriteLine("Spent seconds on disconnecting: " + ((int) span5.TotalSeconds));
                        }
                    }
                    catch (Exception exception1)
                    {
                        Logging.LogThreadException(exception1.ToString(), "Connection checker task");
                    }
                    pingLastExecution = DateTime.Now;
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("GameClientManager.TestClientConnections spent: " + span.TotalSeconds + " seconds in working.");
                }
            }
            catch (Exception exception2)
            {
                Logging.LogThreadException(exception2.ToString(), "GameClientManager.TestClientConnections Exception --> Not inclusive");
            }
        }

        private void ThirdThread_Execution()
        {
            try
            {
                this.BroadcastPacketsWithRankRequirement();
                this.BroadcastPackets();
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "GameClientManager.ThirdThread_Execution Exception");
            }
        }

        internal void UnregisterClient(uint userid, string username)
        {
            this.userIDRegister.Remove(userid);
            this.usernameRegister.Remove(username.ToLower());
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("DELETE FROM user_online WHERE id = " + userid);
            }
        }

        internal int ClientCount
        {
            get
            {
                return this.clients.Count;
            }
        }

        internal int connectionCount
        {
            get
            {
                return this.clients.Count;
            }
        }

    }
}

