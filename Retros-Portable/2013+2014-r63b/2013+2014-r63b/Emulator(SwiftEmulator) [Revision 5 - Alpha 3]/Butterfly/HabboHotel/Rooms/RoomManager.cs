namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Events;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Guilds;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal class RoomManager
    {
        private Dictionary<RoomData, int> activeRooms = new Dictionary<RoomData, int>();
        private Queue activeRoomsAddQueue = new Queue();
        private Queue activeRoomsRemoveQueue = new Queue();
        private Queue activeRoomsUpdateQueue = new Queue();
        private DateTime cycleLastExecution;
        private EventManager eventManager = new EventManager();
        private Dictionary<RoomData, int> guildRooms = new Dictionary<RoomData, int>();
        private Queue guildRoomsAddQueue = new Queue();
        private Queue guildRoomsRemoveQueue = new Queue();
        private Hashtable loadedRoomData = new Hashtable();
        private Dictionary<uint, Room> loadedRooms = new Dictionary<uint, Room>();
        internal const int MAX_PETS_PER_ROOM = 15;
        private List<Room> mToRemove = new List<Room>();
        private IEnumerable<KeyValuePair<RoomData, int>> orderedActiveRooms;
        private IEnumerable<KeyValuePair<RoomData, int>> orderedGuildRooms;
        private IEnumerable<KeyValuePair<RoomData, int>> orderedVotedRooms;
        private static DateTime removeLastExecution;
        internal static bool roomCyclingEnabled = true;
        private Queue roomDataToAddQueue = new Queue();
        private Dictionary<uint, List<RoomLinkInformation>> roomLinks;
        private Hashtable roomModels = new Hashtable();
        private Queue roomsToAddQueue = new Queue();
        private Queue roomsToRemoveQueue = new Queue();
        private Dictionary<RoomData, int> votedRooms = new Dictionary<RoomData, int>();
        private Queue votedRoomsAddQueue = new Queue();
        private Queue votedRoomsRemoveQueue = new Queue();

        internal RoomManager()
        {
        }

        internal RoomData CreateRoom(GameClient Session, string Name, string Model)
        {
            Name = ButterflyEnvironment.FilterInjectionChars(Name);
            if (!this.roomModels.ContainsKey(Model))
            {
                Session.SendNotif(LanguageLocale.GetValue("room.modelmissing"));
                return null;
            }
            if (!(!((RoomModel) this.roomModels[Model]).ClubOnly || Session.GetHabbo().GetSubscriptionManager().HasSubscription("club_habbo")))
            {
                Session.SendNotif(LanguageLocale.GetValue("room.missingclub"));
                return null;
            }
            if (Name.Length < 3)
            {
                Session.SendNotif(LanguageLocale.GetValue("room.namelengthshort"));
                return null;
            }
            uint roomId = 0;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MSSQL)
                {
                    adapter.setQuery("INSERT INTO rooms (roomtype,caption,owner,model_name,description,public_ccts,tags,password) OUTPUT INSERTED.* VALUES ('private',@caption,@username,@model,'','','','')");
                }
                else
                {
                    adapter.setQuery("INSERT INTO rooms (roomtype,caption,owner,model_name) VALUES ('private',@caption,@username,@model)");
                }
                adapter.addParameter("caption", Name);
                adapter.addParameter("model", Model);
                adapter.addParameter("username", Session.GetHabbo().Username);
                roomId = (uint) adapter.insertQuery();
            }
            RoomData item = this.GenerateRoomData(roomId);
            Session.GetHabbo().UsersRooms.Add(item);
            return item;
        }

        internal RoomData FetchRoomData(uint RoomId, DataRow dRow)
        {
            if (this.loadedRoomData.ContainsKey(RoomId))
            {
                return (RoomData) this.loadedRoomData[RoomId];
            }
            RoomData data = new RoomData();
            if (this.IsRoomLoaded(RoomId))
            {
                data.Fill(this.GetRoom(RoomId));
            }
            else
            {
                data.Fill(dRow);
            }
            this.loadedRoomData.Add(RoomId, data);
            return data;
        }

        internal RoomData GenerateNullableRoomData(uint RoomId)
        {
            if (this.GenerateRoomData(RoomId) != null)
            {
                return this.GenerateRoomData(RoomId);
            }
            RoomData data = new RoomData();
            data.FillNull(RoomId);
            return data;
        }

        internal RoomData GenerateRoomData(uint RoomId)
        {
            if (this.loadedRoomData.ContainsKey(RoomId))
            {
                return (RoomData) this.loadedRoomData[RoomId];
            }
            RoomData data = new RoomData();
            if (this.IsRoomLoaded(RoomId))
            {
                return this.GetRoom(RoomId).RoomData;
            }
            DataRow row = null;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE id = " + RoomId);
                row = adapter.getRow();
            }
            if (row == null)
            {
                return null;
            }
            data.Fill(row);
            this.loadedRoomData.Add(RoomId, data);
            return data;
        }

        internal KeyValuePair<RoomData, int>[] GetActiveRooms()
        {
            return this.orderedActiveRooms.ToArray<KeyValuePair<RoomData, int>>();
        }

        private static RoomModel GetCustomData(uint roomID)
        {
            DataRow row;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT doorx,doory,height,modeldata,poolmap FROM room_models_customs WHERE roomid = " + roomID);
                row = adapter.getRow();
            }
            if (row == null)
            {
                throw new Exception("The custom room model for room " + roomID + " was not found");
            }
            return new RoomModel((int) row["doorx"], (int) row["doory"], (double) row["height"], 2, (string) row["modeldata"], "", false, string.Empty);
        }

        internal EventManager GetEventManager()
        {
            return this.eventManager;
        }

        internal KeyValuePair<RoomData, int>[] GetEventRoomsForCategory(int categoryID)
        {
            return this.eventManager.GetRoomsForCategory(categoryID);
        }

        internal KeyValuePair<RoomData, int>[] GetGuildRooms()
        {
            if (this.guildRooms.Count > 0)
            {
                return this.orderedGuildRooms.ToArray<KeyValuePair<RoomData, int>>();
            }
            return null;
        }

        internal List<RoomLinkInformation> getLinkedRoomData(uint roomID)
        {
            if (this.roomLinks.ContainsKey(roomID))
            {
                return this.roomLinks[roomID];
            }
            return new List<RoomLinkInformation>();
        }

        internal RoomModel GetModel(string Model, uint RoomID)
        {
            if (Model == "custom")
            {
                return GetCustomData(RoomID);
            }
            if (this.roomModels.ContainsKey(Model))
            {
                return (RoomModel) this.roomModels[Model];
            }
            return null;
        }

        internal Room GetRoom(uint roomID)
        {
            Room room;
            if (this.loadedRooms.TryGetValue(roomID, out room))
            {
                return room;
            }
            return null;
        }

        internal KeyValuePair<RoomData, int>[] GetVotedRooms()
        {
            return this.orderedVotedRooms.ToArray<KeyValuePair<RoomData, int>>();
        }

        internal void InitRoomLinks(IQueryAdapter dbClient)
        {
            this.roomLinks = new Dictionary<uint, List<RoomLinkInformation>>();
            dbClient.setQuery("SELECT * FROM room_links");
            DataTable table = dbClient.getTable();
            foreach (DataRow row in table.Rows)
            {
                RoomLinkInformation item = new RoomLinkInformation(row);
                if (this.roomLinks.ContainsKey(item.roomID))
                {
                    this.roomLinks[item.roomID].Add(item);
                }
                else
                {
                    List<RoomLinkInformation> list = new List<RoomLinkInformation> {
                        item
                    };
                    this.roomLinks.Add(item.roomID, list);
                }
            }
        }

        internal void InitVotedRooms(IQueryAdapter dbClient)
        {
            if (dbClient.dbType == DatabaseType.MySQL)
            {
                dbClient.setQuery("SELECT rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE score > 0 AND roomtype = 'private' ORDER BY score DESC LIMIT 40");
            }
            else
            {
                dbClient.setQuery("SELECT TOP 40 rooms.*, room_active.active_users FROM rooms LEFT JOIN room_active ON (room_active.roomid = rooms.id) WHERE score > 0 AND roomtype = 'private' ORDER BY score DESC");
            }
            DataTable table = dbClient.getTable();
            foreach (DataRow row in table.Rows)
            {
                RoomData data = this.FetchRoomData(Convert.ToUInt32(row["id"]), row);
                this.QueueVoteAdd(data);
            }
        }

        internal bool IsRoomLoaded(uint RoomId)
        {
            return this.loadedRooms.ContainsKey(RoomId);
        }

        internal void LoadModels(IQueryAdapter dbClient)
        {
            this.roomModels.Clear();
            dbClient.setQuery("SELECT id,door_x,door_y,door_z,door_dir,heightmap,public_items,club_only,poolmap FROM room_models");
            DataTable table = dbClient.getTable();
            dbClient.setQuery("SELECT model, x, y, rot, content FROM room_model_static");
            DataTable table2 = dbClient.getTable();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    string key = (string) row["id"];
                    string staticFurniMap = (string) row["public_items"];
                    this.roomModels.Add(key, new RoomModel((int) row["door_x"], (int) row["door_y"], (double) row["door_z"], (int) row["door_dir"], (string) row["heightmap"], staticFurniMap, ButterflyEnvironment.EnumToBool(row["club_only"].ToString()), (string) row["poolmap"]));
                }
            }
        }

        internal Room LoadRoom(uint Id)
        {
            object obj2;
            if (this.IsRoomLoaded(Id))
            {
                return this.GetRoom(Id);
            }
            RoomData data = this.GenerateRoomData(Id);
            if (data == null)
            {
                return null;
            }
            Room room = new Room(data);
            lock ((obj2 = this.guildRoomsAddQueue.SyncRoot))
            {
                this.guildRoomsAddQueue.Enqueue(data);
            }
            lock ((obj2 = this.roomsToAddQueue.SyncRoot))
            {
                this.roomsToAddQueue.Enqueue(room);
            }
            room.InitPets();
            room.LoadBots();
            room.InitUserBots();
            Logging.WriteLine("[RoomMgr] Stanza caricata: " + Id);
            return room;
        }

        internal void updateFurniState()
        { 
        
        }

        internal void OnCycle()
        {
            try
            {
                DateTime now = DateTime.Now;
                this.WorkRoomDataQueue();
                this.WorkRoomsToAddQueue();
                this.WorkRoomsToRemoveQueue();
                this.RoomCycleTask();
                this.RemoveTask();
                bool flag = this.WorkActiveRoomsAddQueue();
                bool flag2 = this.WorkActiveRoomsRemoveQueue();
                bool flag3 = this.WorkActiveRoomsUpdateQueue();
                if ((flag || flag2) || flag3)
                {
                    this.SortActiveRooms();
                }
                bool flag4 = this.WorkVotedRoomsAddQueue();
                bool flag5 = this.WorkVotedRoomsRemoveQueue();
                if (flag4 || flag5)
                {
                    this.SortVotedRooms();
                }
                bool flag6 = this.WorkGuildRoomsAddQueue();
                bool flag7 = this.WorkGuildRoomsRemoveQueue();
                if (flag6 || flag7)
                {
                    this.SortGuildRooms();
                }
                this.eventManager.onCycle();
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                if (span.TotalSeconds > 3.0)
                {
                    Console.WriteLine("RoomManager.OnCycle spent: " + span.TotalSeconds + " seconds in working.");
                }
                ButterflyEnvironment.GetGame().RoomManagerCycle_ended = true;
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "RoomManager.OnCycle Exception --> Not inclusive");
            }
        }

        internal void OnCycle_RoomDedicatedThread()
        {
            while (ButterflyEnvironment.GetGame().gameLoopActive_EXT)
            {
                try
                {
                    if (ButterflyEnvironment.GetGame().gameLoopEnabled_EXT)
                    {
                        ButterflyEnvironment.GetGame().RoomManagerCycle_ended = false;
                        DateTime now = DateTime.Now;
                        this.WorkRoomDataQueue();
                        this.WorkRoomsToAddQueue();
                        this.WorkRoomsToRemoveQueue();
                        this.RoomCycleTask();
                        this.RemoveTask();
                        bool flag = this.WorkActiveRoomsAddQueue();
                        bool flag2 = this.WorkActiveRoomsRemoveQueue();
                        bool flag3 = this.WorkActiveRoomsUpdateQueue();
                        if ((flag || flag2) || flag3)
                        {
                            this.SortActiveRooms();
                        }
                        bool flag4 = this.WorkVotedRoomsAddQueue();
                        bool flag5 = this.WorkVotedRoomsRemoveQueue();
                        if (flag4 || flag5)
                        {
                            this.SortVotedRooms();
                        }
                        this.eventManager.onCycle();
                        TimeSpan span = (TimeSpan) (DateTime.Now - now);
                        if (span.TotalSeconds > 3.0)
                        {
                            Console.WriteLine("RoomManager.OnCycle spent: " + span.TotalSeconds + " seconds in working.");
                        }
                        ButterflyEnvironment.GetGame().RoomManagerCycle_ended = true;
                    }
                    Thread.Sleep(ButterflyEnvironment.GetGame().gameLoopSleepTime_EXT);
                }
                catch (Exception exception)
                {
                    Logging.LogThreadException(exception.ToString(), "RoomManager.OnCycle Exception.");
                }
            }
        }

        internal void QueueActiveRoomAdd(RoomData data)
        {
            object obj2;
            lock ((obj2 = this.activeRoomsAddQueue.SyncRoot))
            {
                this.activeRoomsAddQueue.Enqueue(data);
            }
            if (data.GroupId > 0)
            {
                lock ((obj2 = this.guildRoomsAddQueue.SyncRoot))
                {
                    this.guildRoomsAddQueue.Enqueue(data);
                }
            }
        }

        internal void QueueActiveRoomRemove(RoomData data)
        {
            lock (this.activeRoomsRemoveQueue.SyncRoot)
            {
                this.activeRoomsRemoveQueue.Enqueue(data);
            }
        }

        internal void QueueActiveRoomUpdate(RoomData data)
        {
            lock (this.activeRoomsUpdateQueue.SyncRoot)
            {
                this.activeRoomsUpdateQueue.Enqueue(data);
            }
        }

        internal void QueueVoteAdd(RoomData data)
        {
            lock (this.votedRoomsAddQueue.SyncRoot)
            {
                this.votedRoomsAddQueue.Enqueue(data);
            }
        }

        internal void QueueVoteRemove(RoomData data)
        {
            lock (this.votedRoomsRemoveQueue.SyncRoot)
            {
                this.votedRoomsRemoveQueue.Enqueue(data);
            }
        }

        internal void RemoveAllRooms()
        {
            int count = this.loadedRooms.Count;
            int num2 = 0;
            foreach (Room room in this.loadedRooms.Values)
            {
                ButterflyEnvironment.GetGame().GetRoomManager().UnloadRoom(room);
                Console.Clear();
                Console.WriteLine("<<- SERVER SHUTDOWN ->> ROOM ITEM SAVE: " + string.Format("{0:0.##}", (((double) num2) / ((double) count)) * 100.0) + "%");
                num2++;
            }
            Console.WriteLine("Done disposing rooms!");
        }

        internal void RemoveTask()
        {
            TimeSpan span = (TimeSpan) (DateTime.Now - removeLastExecution);
            if (span.TotalMilliseconds >= 5000.0)
            {
                removeLastExecution = DateTime.Now;
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    foreach (Room room in this.mToRemove)
                    {
                        Logging.WriteLine("[RoomMgr] Requesting unload of idle room - ID#: " + room.RoomId);
                        room.GetRoomItemHandler().SaveFurniture(adapter);
                        this.UnloadRoom(room);
                    }
                }
                this.mToRemove.Clear();
            }
        }

        internal void RoomCycleTask()
        {
            TimeSpan span = (TimeSpan) (DateTime.Now - this.cycleLastExecution);
            if ((span.TotalMilliseconds >= 500.0) && roomCyclingEnabled)
            {
                this.cycleLastExecution = DateTime.Now;
                foreach (Room room in this.loadedRooms.Values)
                {
                    if (room.mIsIdle)
                    {
                        this.mToRemove.Add(room);
                    }
                    else if ((room.procesoTask == null) || room.procesoTask.IsCompleted)
                    {
                        room.procesoTask = new Task(new Action(room.ProcessRoom));
                        room.procesoTask.Start();
                        room.procesoIntentos = 0;
                    }
                    else
                    {
                        room.procesoIntentos++;
                        if (room.procesoIntentos > 30)
                        {
                            this.mToRemove.Add(room);
                            Console.WriteLine("Conflictive Room: " + room.RoomId);
                        }
                    }
                }
            }
        }

        private void SortActiveRooms()
        {
            this.orderedActiveRooms = (from t in this.activeRooms
                orderby t.Value descending
                select t).Take<KeyValuePair<RoomData, int>>(40);
        }

        private void SortGuildRooms()
        {
            this.orderedGuildRooms = from t in this.guildRooms.Take<KeyValuePair<RoomData, int>>(40)
                orderby t.Value descending
                select t;
        }

        private void SortVotedRooms()
        {
            this.orderedVotedRooms = from t in this.votedRooms.Take<KeyValuePair<RoomData, int>>(40)
                orderby t.Value descending
                select t;
        }

        internal void UnloadRoom(Room Room)
        {
            if (Room != null)
            {
                object obj2;
                lock ((obj2 = this.roomsToRemoveQueue.SyncRoot))
                {
                    this.roomsToRemoveQueue.Enqueue(Room.RoomId);
                }
                lock ((obj2 = this.guildRoomsRemoveQueue.SyncRoot))
                {
                    this.guildRoomsRemoveQueue.Enqueue(Room.RoomData);
                }
                Room.Destroy();
                Logging.WriteLine(string.Concat(new object[] { "[RoomMgr] Unloaded room: \"", Room.Name, "\" (ID: ", Room.RoomId, ")" }));
            }
        }

        private bool WorkActiveRoomsAddQueue()
        {
            if (this.activeRoomsAddQueue.Count > 0)
            {
                lock (this.activeRoomsAddQueue.SyncRoot)
                {
                    while (this.activeRoomsAddQueue.Count > 0)
                    {
                        RoomData key = (RoomData) this.activeRoomsAddQueue.Dequeue();
                        if (!this.activeRooms.ContainsKey(key))
                        {
                            this.activeRooms.Add(key, key.UsersNow);
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private bool WorkActiveRoomsRemoveQueue()
        {
            if (this.activeRoomsRemoveQueue.Count > 0)
            {
                lock (this.activeRoomsRemoveQueue.SyncRoot)
                {
                    while (this.activeRoomsRemoveQueue.Count > 0)
                    {
                        RoomData key = (RoomData) this.activeRoomsRemoveQueue.Dequeue();
                        this.activeRooms.Remove(key);
                    }
                }
                return true;
            }
            return false;
        }

        private bool WorkActiveRoomsUpdateQueue()
        {
            if (this.activeRoomsUpdateQueue.Count > 0)
            {
                lock (this.activeRoomsUpdateQueue.SyncRoot)
                {
                    while (this.activeRoomsUpdateQueue.Count > 0)
                    {
                        RoomData key = (RoomData) this.activeRoomsUpdateQueue.Dequeue();
                        if (!this.activeRooms.ContainsKey(key))
                        {
                            this.activeRooms.Add(key, key.UsersNow);
                        }
                        else
                        {
                            this.activeRooms[key] = key.UsersNow;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private bool WorkGuildRoomsAddQueue()
        {
            if (this.guildRoomsAddQueue.Count > 0)
            {
                lock (this.guildRoomsAddQueue.SyncRoot)
                {
                    while (this.guildRoomsAddQueue.Count > 0)
                    {
                        RoomData key = (RoomData) this.guildRoomsAddQueue.Dequeue();
                        if (key.GroupId <= 0)
                        {
                            return false;
                        }
                        MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(key.GroupId);
                        if (!this.guildRooms.ContainsKey(key))
                        {
                            this.guildRooms.Add(key, guild.Members.Count);
                        }
                        else
                        {
                            this.guildRooms[key] = guild.Members.Count;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private bool WorkGuildRoomsRemoveQueue()
        {
            if (this.guildRoomsRemoveQueue.Count > 0)
            {
                lock (this.guildRoomsRemoveQueue.SyncRoot)
                {
                    while (this.guildRoomsRemoveQueue.Count > 0)
                    {
                        RoomData key = (RoomData) this.guildRoomsRemoveQueue.Dequeue();
                        this.guildRooms.Remove(key);
                    }
                }
                return true;
            }
            return false;
        }

        private void WorkRoomDataQueue()
        {
            if (this.roomDataToAddQueue.Count > 0)
            {
                lock (this.roomDataToAddQueue.SyncRoot)
                {
                    while (this.roomDataToAddQueue.Count > 0)
                    {
                        RoomData data = (RoomData) this.roomDataToAddQueue.Dequeue();
                        if (!this.loadedRooms.ContainsKey(data.Id))
                        {
                            this.loadedRoomData.Add(data.Id, data);
                        }
                    }
                }
            }
        }

        private void WorkRoomsToAddQueue()
        {
            if (this.roomsToAddQueue.Count > 0)
            {
                lock (this.roomsToAddQueue.SyncRoot)
                {
                    while (this.roomsToAddQueue.Count > 0)
                    {
                        Room room = (Room) this.roomsToAddQueue.Dequeue();
                        if (!this.loadedRooms.ContainsKey(room.RoomId))
                        {
                            this.loadedRooms.Add(room.RoomId, room);
                        }
                    }
                }
            }
        }

        private void WorkRoomsToRemoveQueue()
        {
            if (this.roomsToRemoveQueue.Count > 0)
            {
                lock (this.roomsToRemoveQueue.SyncRoot)
                {
                    while (this.roomsToRemoveQueue.Count > 0)
                    {
                        uint key = (uint) this.roomsToRemoveQueue.Dequeue();
                        this.loadedRooms.Remove(key);
                    }
                }
            }
        }

        private bool WorkVotedRoomsAddQueue()
        {
            if (this.votedRoomsAddQueue.Count > 0)
            {
                lock (this.votedRoomsAddQueue.SyncRoot)
                {
                    while (this.votedRoomsAddQueue.Count > 0)
                    {
                        RoomData key = (RoomData) this.votedRoomsAddQueue.Dequeue();
                        if (!this.votedRooms.ContainsKey(key))
                        {
                            this.votedRooms.Add(key, key.Score);
                        }
                        else
                        {
                            this.votedRooms[key] = key.Score;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private bool WorkVotedRoomsRemoveQueue()
        {
            if (this.votedRoomsRemoveQueue.Count > 0)
            {
                lock (this.votedRoomsRemoveQueue.SyncRoot)
                {
                    while (this.votedRoomsRemoveQueue.Count > 0)
                    {
                        RoomData key = (RoomData) this.votedRoomsRemoveQueue.Dequeue();
                        this.votedRooms.Remove(key);
                    }
                }
                return true;
            }
            return false;
        }

        internal int LoadedRoomsCount
        {
            get
            {
                return this.loadedRooms.Count;
            }
        }
    }
}

