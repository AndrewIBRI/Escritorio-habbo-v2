namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly;
    using Butterfly.Collections;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Pathfinding;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.RoomBots;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Users.Inventory;
    using Butterfly.Messages;
    using Butterfly.Util;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;

    internal class RoomUserManager
    {
        private int petCount;
        private Hashtable pets;
        private Hashtable bots;
        private int primaryPrivateUserID;
        private Room room;
        private int secondaryPrivateUserID;
        private List<RoomUser> ToRemove;
        private int userCount;
        private QueuedDictionary<int, RoomUser> userlist;
        internal Hashtable usersByUserID;
        internal Hashtable usersByUsername;

        internal event RoomEventDelegate OnUserEnter;

        public RoomUserManager(Room room)
        {
            this.room = room;
            this.userlist = new QueuedDictionary<int, RoomUser>(new System.EventHandler(this.OnUserAdd), null, new System.EventHandler(this.onRemove), null);
            this.pets = new Hashtable();
            this.bots = new Hashtable();
            this.usersByUsername = new Hashtable();
            this.usersByUserID = new Hashtable();
            this.primaryPrivateUserID = 0;
            this.secondaryPrivateUserID = 0;
            this.ToRemove = new List<RoomUser>(room.UsersMax);
            this.petCount = 0;
            this.userCount = 0;
        }

        internal void AddUserToRoom(GameClient Session, bool Spectator)
        {
            int num4;
            this.primaryPrivateUserID = (num4 = this.primaryPrivateUserID) + 1;
            RoomUser user = new RoomUser(Session.GetHabbo().Id, this.room.RoomId, num4, this.room, Spectator);
            Habbo habbo = user.GetClient().GetHabbo();
            if (habbo.spamProtectionBol)
            {
                TimeSpan span = (TimeSpan) (DateTime.Now - habbo.spamFloodTime);
                if (span.TotalSeconds < habbo.spamProtectionTime)
                {
                    ServerMessage message = new ServerMessage(Outgoing.FloodFilter);
                    int i = habbo.spamProtectionTime - span.Seconds;
                    message.AppendInt32(i);
                    user.GetClient().SendMessage(message);
                }
            }
            user.userID = Session.GetHabbo().Id;
            string username = Session.GetHabbo().Username;
            uint userID = user.userID;
            if (this.usersByUsername.ContainsKey(username.ToLower()))
            {
                this.usersByUsername.Remove(username.ToLower());
            }
            if (this.usersByUserID.ContainsKey(userID))
            {
                this.usersByUserID.Remove(userID);
            }
            this.usersByUsername.Add(Session.GetHabbo().Username.ToLower(), user);
            this.usersByUserID.Add(Session.GetHabbo().Id, user);
            this.secondaryPrivateUserID = (num4 = this.secondaryPrivateUserID) + 1;
            int key = num4;
            user.InternalRoomID = key;
            Session.CurrentRoomUserID = key;
            Session.GetHabbo().visitedRooms.Add(this.room.RoomData);
            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_RoomEntry", 1);
            Session.GetHabbo().CurrentRoomId = this.room.RoomId;
            this.UserList.Add(key, user);

        }

        internal void AppendPetsUpdateString(IQueryAdapter dbClient)
        {
            QueryChunk chunk = new QueryChunk("INSERT INTO user_pets (id,user_id,room_id,name,type,race,color,expirience,energy,createstamp,nutrition,respect,z,y,z) VALUES ");
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                List<uint> list = new List<uint>();

                foreach (Pet pet in this.GetPets())
                {
                    if (!list.Contains(pet.PetId))
                    {
                        list.Add(pet.PetId);

                        if (pet.DBState == DatabaseUpdateState.NeedsInsert)
                        {
                            chunk.AddParameter("name", pet.Name);
                            chunk.AddParameter("race", pet.Race);
                            chunk.AddParameter("color", pet.Color);
                            chunk.AddQuery(string.Concat(new object[] { 
                            "(", pet.PetId, ",", pet.OwnerId, ",", pet.RoomId, ",@name,", pet.Type, ",@race,@color,0,100,'", pet.CreationStamp, 
                            "',0,0,0,0,0)"
                         }));
                            chunk.Execute(dbClient);
                            chunk.Dispose();

                        }

                        else if (pet.DBState == DatabaseUpdateState.NeedsUpdate)
                        {
                            adapter.setQuery(string.Concat(new object[] { 
                                    "UPDATE user_pets SET room_id = ", pet.RoomId, ", name = @name, race = @race, color = @color, type = ", pet.Type, ", expirience = ", pet.Expirience, ", energy = ", pet.Energy, ", nutrition = ", pet.Nutrition, 
                                    ", respect = ", pet.Respect, ", createstamp = '", pet.CreationStamp, "', x = ", pet.X, ", Y = ", pet.Y, ", Z = ", pet.Z, " WHERE id = ", pet.PetId
                                 }));
                            adapter.addParameter("name", pet.Name);

                        }

                        pet.DBState = DatabaseUpdateState.Updated;
                    }
                }
            }
            chunk.Dispose();
        }

        internal void backupCounters(ref int primaryCounter, ref int secondaryCounter)
        {
            primaryCounter = this.primaryPrivateUserID;
            secondaryCounter = this.secondaryPrivateUserID;
        }
        internal RoomUser DeployBot(RoomBot Bot, Pet PetData)
        {
            int num2;
            this.primaryPrivateUserID = (num2 = this.primaryPrivateUserID) + 1;
            RoomUser user = new RoomUser(0, this.room.RoomId, num2, this.room, false);
            this.secondaryPrivateUserID = (num2 = this.secondaryPrivateUserID) + 1;
            int key = num2;
            user.InternalRoomID = key;
            this.userlist.Add(key, user);
            DynamicRoomModel model = this.room.GetGameMap().Model;
            if ((((Bot.X > 0) && (Bot.Y > 0)) && (Bot.X < model.MapSizeX)) && (Bot.Y < model.MapSizeY))
            {
                user.SetPos(Bot.X, Bot.Y, (double)Bot.Z);
                user.SetRot(Bot.Rot, false);
            }
            else
            {
                Bot.X = model.DoorX;
                Bot.Y = model.DoorY;
                user.SetPos(model.DoorX, model.DoorY, model.DoorZ);
                user.SetRot(model.DoorOrientation, false);
            }
            user.BotData = Bot;
            user.BotAI = Bot.GenerateBotAI(user.VirtualId,(int)Bot.BotId);
            if (user.IsPet)
            {
                user.BotAI.Init((int)Bot.BotId, user.VirtualId, this.room.RoomId, user, this.room);
                user.PetData = PetData;
                user.PetData.VirtualId = user.VirtualId;
            }
            else
            {
                user.BotAI.Init((int)Bot.BotId, user.VirtualId, this.room.RoomId, user, this.room);
            }
            this.UpdateUserStatus(user, false);
            user.UpdateNeeded = true;
            ServerMessage message = new ServerMessage(Outgoing.PlaceBot);
            message.AppendInt32(1);
            user.Serialize(message, this.room.GetGameMap().gotPublicPool);
            this.room.SendMessage(message);
            user.BotAI.OnSelfEnterRoom();
            if (user.BotData.AiType == AIType.Guide)
            {
                this.room.guideBotIsCalled = true;
            }
            if (user.IsPet)
            {
                if (this.pets.ContainsKey(user.PetData.PetId))
                {
                    this.pets[user.PetData.PetId] = user;
                }
                else
                {
                    this.pets.Add(user.PetData.PetId, user);
                }
                this.petCount++;
            }
            if (user.BotData.AiType == AIType.Generic)
            {
                if (bots.ContainsKey(user.BotData.BotId)) //Pet allready placed
                    bots[user.BotData.BotId] = user;
                else
                    bots.Add(user.BotData.BotId, user);

                petCount++;
            }
            return user;
        }

        /*internal RoomUser DeployBot(RoomBot Bot, Pet PetData)
        {
            int num2;
            this.primaryPrivateUserID = (num2 = this.primaryPrivateUserID) + 1;
            RoomUser user = new RoomUser(0, this.room.RoomId, num2, this.room, false);
            this.secondaryPrivateUserID = (num2 = this.secondaryPrivateUserID) + 1;
            int key = num2;
            user.InternalRoomID = key;
            this.userlist.Add(key, user);
            DynamicRoomModel model = this.room.GetGameMap().Model;
            if ((((Bot.X > 0) && (Bot.Y > 0)) && (Bot.X < model.MapSizeX)) && (Bot.Y < model.MapSizeY))
            {
                user.SetPos(Bot.X, Bot.Y, (double) Bot.Z);
                user.SetRot(Bot.Rot, false);
            }
            else
            {
                Bot.X = model.DoorX;
                Bot.Y = model.DoorY;
                user.SetPos(model.DoorX, model.DoorY, model.DoorZ);
                user.SetRot(model.DoorOrientation, false);
            }
            user.BotData = Bot;
            user.BotAI = Bot.GenerateBotAI(user.VirtualId);
            if (user.IsPet)
            {
                user.BotAI.Init((int) Bot.BotId, user.VirtualId, this.room.RoomId, user, this.room);
                user.PetData = PetData;
                user.PetData.VirtualId = user.VirtualId;
            }
            else
            {
                user.BotAI.Init(-1, user.VirtualId, this.room.RoomId, user, this.room);
            }
            this.UpdateUserStatus(user, false);
            user.UpdateNeeded = true;
            ServerMessage message = new ServerMessage(Outgoing.PlaceBot);
            message.AppendInt32(1);
            user.Serialize(message, this.room.GetGameMap().gotPublicPool);
            this.room.SendMessage(message);
            user.BotAI.OnSelfEnterRoom();
            if (user.BotData.AiType == AIType.Guide)
            {
                this.room.guideBotIsCalled = true;
            }
            if (user.IsPet)
            {
                if (this.pets.ContainsKey(user.PetData.PetId))
                {
                    this.pets[user.PetData.PetId] = user;
                }
                else
                {
                    this.pets.Add(user.PetData.PetId, user);
                }
                this.petCount++;
            }
            return user;
        }
        */
        internal void Destroy()
        {
            this.room = null;
            this.usersByUsername.Clear();
            this.usersByUsername = null;
            this.usersByUserID.Clear();
            this.usersByUserID = null;
            this.OnUserEnter = null;
            this.pets.Clear();
            this.pets = null;
            this.userlist.Destroy();
            this.userlist = null;
        }
        internal RoomUser GetBot(uint PetId)
        {
            if (bots.ContainsKey(PetId))
                return (RoomUser)bots[PetId];

            return null;
        }
        internal RoomUser GetPet(uint PetId)
        {
            if (this.pets.ContainsKey(PetId))
            {
                return (RoomUser) this.pets[PetId];
            }
            return null;
        }

        internal List<Pet> GetPets()
        {
            List<KeyValuePair<int, RoomUser>> list = this.UserList.ToList();
            List<Pet> list2 = new List<Pet>();
            foreach (KeyValuePair<int, RoomUser> pair in list)
            {
                RoomUser user = pair.Value;
                if (user.IsPet)
                {
                    list2.Add(user.PetData);
                }
            }
            return list2;
        }

        internal RoomUser GetRoomUserByHabbo(string pName)
        {
            if (this.usersByUsername.ContainsKey(pName.ToLower()))
            {
                return (RoomUser) this.usersByUsername[pName.ToLower()];
            }
            return null;
        }

        internal RoomUser GetRoomUserByHabbo(uint pId)
        {
            if (this.usersByUserID.ContainsKey(pId))
            {
                return (RoomUser) this.usersByUserID[pId];
            }
            return null;
        }

        internal List<RoomUser> GetRoomUserByRank(int minRank)
        {
            List<RoomUser> list = new List<RoomUser>();
            foreach (RoomUser user in this.UserList.Values)
            {
                if (((!user.IsBot && (user.GetClient() != null)) && (user.GetClient().GetHabbo() != null)) && (user.GetClient().GetHabbo().Rank > minRank))
                {
                    list.Add(user);
                }
            }
            return list;
        }

        internal RoomUser GetRoomUserByVirtualId(int VirtualId)
        {
            return this.UserList.GetValue(VirtualId);
        }

        internal int GetRoomUserCount()
        {
            return this.userlist.Inner.Count;
        }

        public List<RoomUser> GetRoomUsers()
        {
            List<KeyValuePair<int, RoomUser>> list = this.UserList.ToList();
            List<RoomUser> list2 = new List<RoomUser>();
            foreach (KeyValuePair<int, RoomUser> pair in list)
            {
                if (!pair.Value.IsBot)
                {
                    list2.Add(pair.Value);
                }
            }
            return list2;
        }

        internal RoomUser GetUserForSquare(int x, int y)
        {
            return this.room.GetGameMap().GetRoomUsers(new Point(x, y)).FirstOrDefault<RoomUser>();
        }

        private bool isValid(RoomUser user)
        {
            if (!user.IsBot)
            {
                if (user.GetClient() == null)
                {
                    return false;
                }
                if (user.GetClient().GetHabbo() == null)
                {
                    return false;
                }
                if (user.GetClient().GetHabbo().CurrentRoomId != this.room.RoomId)
                {
                    return false;
                }
            }
            return true;
        }

        internal void OnCycle(ref int idleCount)
        {
            this.ToRemove.Clear();
            int count = 0;
            foreach (RoomUser user in this.UserList.Values)
            {
                if (!this.isValid(user))
                {
                    if (user.GetClient() != null)
                    {
                        this.RemoveUserFromRoom(user.GetClient(), false, false);
                    }
                    else
                    {
                        this.RemoveRoomUser(user);
                    }
                }
                bool flag = false;
                user.IdleTime++;
                if (!(user.IsAsleep || (user.IdleTime < 600)))
                {
                    user.IsAsleep = true;
                    ServerMessage message = new ServerMessage(Outgoing.IdleStatus);
                    message.AppendInt32(user.VirtualId);
                    message.AppendBoolean(true);
                    this.room.SendMessage(message);
                }
                if (!(!user.NeedsAutokick || this.ToRemove.Contains(user)))
                {
                    this.ToRemove.Add(user);
                }
                else
                {
                    if (user.CarryItemID > 0)
                    {
                        user.CarryTimer--;
                        if (user.CarryTimer <= 0)
                        {
                            user.CarryItem(0);
                        }
                    }
                    if (this.room.GotFreeze())
                    {
                        this.room.GetFreeze().CycleUser(user);
                    }
                    if (user.isFlying)
                    {
                        user.OnFly();
                    }
                    if (user.SetStep)
                    {
                        if (this.room.GetGameMap().CanWalk(user.SetX, user.SetY, user.AllowOverride) || user.montandoBol)
                        {
                            this.room.GetGameMap().UpdateUserMovement(new Point(user.Coordinate.X, user.Coordinate.Y), new Point(user.SetX, user.SetY), user);
                            List<RoomItem> coordinatedItems = this.room.GetGameMap().GetCoordinatedItems(new Point(user.X, user.Y));
                            user.X = user.SetX;
                            user.Y = user.SetY;
                            user.Z = user.SetZ;
                            if (user.isFlying)
                            {
                                user.Z += 4.0 + (0.5 * Math.Sin(0.7 * user.flyk));
                            }
                            foreach (RoomItem item in coordinatedItems)
                            {
                                item.UserWalksOffFurni(user);
                            }
                            if (!((((user.X != this.room.GetGameMap().Model.DoorX) || (user.Y != this.room.GetGameMap().Model.DoorY)) || this.ToRemove.Contains(user)) || user.IsBot))
                            {
                                this.ToRemove.Add(user);
                                goto Label_0FF0;
                            }
                            if (this.room.IsPublic)
                            {
                                this.room.GetGameMap().HandleRoomLinks(user);
                            }
                            this.UpdateUserStatus(user, true);
                        }
                        user.SetStep = false;
                    }
                    if (user.IsWalking && !user.Freezed)
                    {
                        RoomUser roomUserByVirtualId;
                        ServerMessage message2;
                        Gamemap gameMap = this.room.GetGameMap();
                        SquarePoint point = DreamPathfinder.GetNextStep(user.X, user.Y, user.GoalX, user.GoalY, gameMap.GameMap, gameMap.ItemHeightMap, gameMap.Model.MapSizeX, gameMap.Model.MapSizeY, user.AllowOverride, gameMap.DiagonalEnabled);
                        if ((point.X == user.X) && (point.Y == user.Y))
                        {
                            user.IsWalking = false;
                            user.RemoveStatus("mv");
                            this.UpdateUserStatus(user, false);
                            if (!(!user.montandoBol || user.IsPet))
                            {
                                roomUserByVirtualId = this.GetRoomUserByVirtualId(Convert.ToInt32(user.montandoID));
                                roomUserByVirtualId.IsWalking = false;
                                roomUserByVirtualId.RemoveStatus("mv");
                                message2 = new ServerMessage(Outgoing.UpdateState);
                                message2.AppendInt32(1);
                                roomUserByVirtualId.SerializeStatus(message2, "");
                                user.GetClient().GetHabbo().CurrentRoom.SendMessage(message2);
                            }
                        }
                        else
                        {
                            int x = point.X;
                            int y = point.Y;
                            user.RemoveStatus("mv");
                            double k = this.room.GetGameMap().SqAbsoluteHeight(x, y);
                            user.Statusses.Remove("lay");
                            user.Statusses.Remove("sit");
                            string status = "";
                            string str2 = "";
                            if (!user.isFlying)
                            {
                                if (user.montandoBol && !user.IsPet)
                                {
                                    roomUserByVirtualId = this.GetRoomUserByVirtualId(Convert.ToInt32(user.montandoID));
                                    RoomItem furniObject = this.room.GetRoomItemHandler().GetFurniObject(x, y);
                                    if (furniObject != null)
                                    {
                                        if (furniObject.GetBaseItem().InteractionType == InteractionType.obstacle_h)
                                        {
                                            str2 = string.Concat(new object[] { "mv ", x, ",", y, ",", TextHandling.GetString(k), "///jmp" });
                                            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID((uint) this.room.OwnerId), "ACH_RoomHorseJumpCount", 1);
                                            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(user.GetClient(), "ACH_HorseJumping", 1);
                                            if ((user.GoalX == x) && (user.GoalY == y))
                                            {
                                                if (user.X == (x - 1))
                                                {
                                                    user.MoveTo(x + 1, y);
                                                }
                                                else if (user.X == (x + 1))
                                                {
                                                    user.MoveTo(x - 1, y);
                                                }
                                                else if (user.Y == (y - 1))
                                                {
                                                    user.MoveTo(x, y + 1);
                                                }
                                                else if (user.Y == (y + 1))
                                                {
                                                    user.MoveTo(x, y - 1);
                                                }
                                                else if ((furniObject.Rot == 0) || (furniObject.Rot == 4))
                                                {
                                                    user.MoveTo(x, y + 1);
                                                }
                                                else
                                                {
                                                    user.MoveTo(x + 1, y);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            str2 = string.Concat(new object[] { "mv ", x, ",", y, ",", TextHandling.GetString(k) });
                                        }
                                    }
                                    else
                                    {
                                        furniObject = this.room.GetRoomItemHandler().GetFurniObject(x, y - 1);
                                        if (furniObject == null)
                                        {
                                            furniObject = this.room.GetRoomItemHandler().GetFurniObject(x - 1, y);
                                            if (furniObject == null)
                                            {
                                                str2 = string.Concat(new object[] { "mv ", x, ",", y, ",", TextHandling.GetString(k) });
                                            }
                                            else if ((furniObject.Rot == 0) || (furniObject.Rot == 4))
                                            {
                                                if (furniObject.GetBaseItem().InteractionType == InteractionType.obstacle_h)
                                                {
                                                    str2 = string.Concat(new object[] { "mv ", x, ",", y, ",", TextHandling.GetString(k), "///jmp" });
                                                    ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID((uint) this.room.OwnerId), "ACH_RoomHorseJumpCount", 1);
                                                    ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(user.GetClient(), "ACH_HorseJumping", 1);
                                                    if ((user.GoalX == x) && (user.GoalY == y))
                                                    {
                                                        if (user.Y == (y - 1))
                                                        {
                                                            user.MoveTo(x, y + 1);
                                                        }
                                                        else if (user.Y == (y + 1))
                                                        {
                                                            user.MoveTo(x, y - 1);
                                                        }
                                                        else if ((furniObject.Rot == 0) || (furniObject.Rot == 4))
                                                        {
                                                            user.MoveTo(x, y + 1);
                                                        }
                                                        else
                                                        {
                                                            user.MoveTo(x + 1, y);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    str2 = string.Concat(new object[] { "mv ", x, ",", y, ",", TextHandling.GetString(k) });
                                                }
                                            }
                                        }
                                        else if ((furniObject.Rot == 2) || (furniObject.Rot == 6))
                                        {
                                            if (furniObject.GetBaseItem().InteractionType == InteractionType.obstacle_h)
                                            {
                                                str2 = string.Concat(new object[] { "mv ", x, ",", y, ",", TextHandling.GetString(k), "///jmp" });
                                                ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID((uint) this.room.OwnerId), "ACH_RoomHorseJumpCount", 1);
                                                ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(user.GetClient(), "ACH_HorseJumping", 1);
                                                if ((user.GoalX == x) && (user.GoalY == y))
                                                {
                                                    if (user.X == (x - 1))
                                                    {
                                                        user.MoveTo(x + 1, y);
                                                    }
                                                    else if (user.X == (x + 1))
                                                    {
                                                        user.MoveTo(x - 1, y);
                                                    }
                                                    else if ((furniObject.Rot == 0) || (furniObject.Rot == 4))
                                                    {
                                                        user.MoveTo(x, y + 1);
                                                    }
                                                    else
                                                    {
                                                        user.MoveTo(x + 1, y);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                str2 = string.Concat(new object[] { "mv ", x, ",", y, ",", TextHandling.GetString(k) });
                                            }
                                        }
                                    }
                                    status = string.Concat(new object[] { "mv ", x, ",", y, ",", TextHandling.GetString(k + 1.0) });
                                    user.AddStatus("mv", string.Concat(new object[] { x, ",", y, ",", TextHandling.GetString(k + 1.0) }));
                                }
                                else
                                {
                                    user.AddStatus("mv", string.Concat(new object[] { x, ",", y, ",", TextHandling.GetString(k) }));
                                }
                            }
                            else
                            {
                                user.AddStatus("mv", string.Concat(new object[] { x, ",", y, ",", TextHandling.GetString((k + 4.0) + (0.5 * Math.Sin(0.7 * user.flyk))) }));
                            }
                            int num5 = Rotation.Calculate(user.X, user.Y, x, y, user.moonwalkEnabled);
                            user.RotBody = num5;
                            user.RotHead = num5;
                            user.SetStep = true;
                            user.SetX = x;
                            user.SetY = y;
                            user.SetZ = k;
                            this.UpdateUserEffect(user, user.SetX, user.SetY);
                            flag = true;
                            this.room.GetGameMap().GameMap[user.X, user.Y] = user.SqState;
                            user.SqState = this.room.GetGameMap().GameMap[user.SetX, user.SetY];
                            if (user.sentadoBol)
                            {
                                user.sentadoBol = false;
                            }
                            if (user.acostadoBol)
                            {
                                user.acostadoBol = false;
                            }
                            if (!(!user.montandoBol || user.IsPet))
                            {
                                roomUserByVirtualId = this.GetRoomUserByVirtualId(Convert.ToInt32(user.montandoID));
                                roomUserByVirtualId.RotBody = num5;
                                roomUserByVirtualId.RotHead = num5;
                                roomUserByVirtualId.SetStep = true;
                                roomUserByVirtualId.SetX = x;
                                roomUserByVirtualId.SetY = y;
                                roomUserByVirtualId.SetZ = k;
                                this.UpdateUserEffect(roomUserByVirtualId, roomUserByVirtualId.SetX, roomUserByVirtualId.SetY);
                                flag = true;
                                message2 = new ServerMessage(Outgoing.UpdateState);
                                message2.AppendInt32(2);
                                user.SerializeStatus(message2, status);
                                roomUserByVirtualId.SerializeStatus(message2, str2);
                                user.GetClient().GetHabbo().CurrentRoom.SendMessage(message2);
                                this.UpdateUserEffect(user, user.SetX, user.SetY);
                            }
                            if (!this.room.AllowWalkthrough)
                            {
                                this.room.GetGameMap().GameMap[x, y] = 0;
                            }
                        }
                        if (!user.montandoBol)
                        {
                            user.UpdateNeeded = true;
                        }
                    }
                    else if (user.Statusses.ContainsKey("mv"))
                    {
                        user.RemoveStatus("mv");
                        user.UpdateNeeded = true;
                    }
                    if (user.IsBot)
                    {
                        user.BotAI.OnTimerTick();
                    }
                    else
                    {
                        count++;
                    }
                    if (!flag)
                    {
                        this.UpdateUserEffect(user, user.X, user.Y);
                    }
                Label_0FF0:;
                }
            }
            if (count == 0)
            {
                idleCount++;
            }
            foreach (RoomUser user3 in this.ToRemove)
            {
                GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(user3.HabboId);
                if (clientByUserID != null)
                {
                    this.RemoveUserFromRoom(clientByUserID, true, false);
                    clientByUserID.CurrentRoomUserID = -1;
                }
                else
                {
                    this.RemoveRoomUser(user3);
                }
            }
            if (this.userCount != count)
            {
                this.UpdateUserCount(count);
            }
        }

        private void onRemove(object sender, EventArgs args)
        {
            try
            {
                KeyValuePair<int, RoomUser> pair = (KeyValuePair<int, RoomUser>) sender;
                RoomUser user = pair.Value;
                GameClient client = user.GetClient();
                int key = pair.Key;
                List<RoomUser> list = new List<RoomUser>();
                foreach (RoomUser user2 in this.UserList.Values)
                {
                    if (user2.IsBot)
                    {
                        list.Add(user2);
                    }
                }
                List<RoomUser> list2 = new List<RoomUser>();
                foreach (RoomUser user3 in list)
                {
                    user3.BotAI.OnUserLeaveRoom(client);
                    if (!((!user3.IsPet || (user3.PetData.OwnerId != user.userID)) || this.room.CheckRights(client, true)))
                    {
                        list2.Add(user3);
                    }
                }
                foreach (RoomUser user4 in list2)
                {
                    if (((user.GetClient() != null) && (user.GetClient().GetHabbo() != null)) && (user.GetClient().GetHabbo().GetInventoryComponent() != null))
                    {
                        user.GetClient().GetHabbo().GetInventoryComponent().AddPet(user4.PetData);
                        this.RemoveBot(user4.VirtualId, false);
                    }
                }
                this.room.GetGameMap().RemoveUserFromMap(user, new Point(user.X, user.Y));
            }
            catch (Exception exception)
            {
                Logging.LogCriticalException(exception.ToString());
            }
        }

        private void OnUserAdd(object sender, EventArgs args)
        {
            try
            {
                KeyValuePair<int, RoomUser> pair = (KeyValuePair<int, RoomUser>) sender;
                RoomUser user = pair.Value;
                if (((user != null) && (user.GetClient() != null)) && (user.GetClient().GetHabbo() != null))
                {
                    GameClient session = user.GetClient();
                    if ((session != null) && (session.GetHabbo() != null))
                    {
                        if (!user.IsSpectator)
                        {
                            DynamicRoomModel model = this.room.GetGameMap().Model;
                            if (this.room.IsPublic && session.SetDoorPos)
                            {
                                user.SetPos(session.newDoorPos.X, session.newDoorPos.Y, (double) this.room.GetGameMap().StaticModel.SqFloorHeight[session.newDoorPos.X, session.newDoorPos.Y]);
                                user.SetRot(model.DoorOrientation, false);
                                session.SetDoorPos = false;
                            }
                            else
                            {
                                user.SetPos(model.DoorX, model.DoorY, model.DoorZ);
                                user.SetRot(model.DoorOrientation, false);
                            }
                            if (this.room.CheckRights(session, true))
                            {
                                user.AddStatus("flatctrl 4", "useradmin");
                            }
                            else if (this.room.CheckRights(session))
                            {
                                user.AddStatus("flatctrl 1", "");
                            }
                            user.CurrentItemEffect = ItemEffectType.None;
                            if (!user.IsBot && user.GetClient().GetHabbo().IsTeleporting)
                            {
                                RoomItem item = this.room.GetRoomItemHandler().GetItem(user.GetClient().GetHabbo().TeleporterId);
                                if (item != null)
                                {
                                    user.SetPos(item.GetX, item.GetY, item.GetZ);
                                    user.SetRot(item.Rot, false);
                                    item.InteractingUser2 = session.GetHabbo().Id;
                                    item.ExtraData = "2";
                                    item.UpdateState(false, true);
                                }
                            }
                            user.GetClient().GetHabbo().IsTeleporting = false;
                            user.GetClient().GetHabbo().TeleporterId = 0;
                            ServerMessage message = new ServerMessage(Outgoing.SetRoomUser);
                            message.AppendInt32(1);
                            user.Serialize(message, this.room.GetGameMap().gotPublicPool);
                            this.room.SendMessage(message);
                            if (!(!(this.room.Owner != session.GetHabbo().Username) || this.room.IsPublic))
                            {
                                ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("ENTER_OTHER_USERS_ROOM", session);
                            }
                        }
                        if (session.GetHabbo().GetMessenger() != null)
                        {
                            session.GetHabbo().GetMessenger().OnStatusChanged(true);
                        }
                        if (!user.IsSpectator)
                        {
                            foreach (RoomUser user2 in this.UserList.Values)
                            {
                                if (user.IsBot)
                                {
                                    user2.BotAI.OnUserEnterRoom(user);
                                }
                            }
                        }
                        user.GetClient().GetMessageHandler().OnRoomUserAdd();
                        if (this.OnUserEnter != null)
                        {
                            this.OnUserEnter(user, null);
                        }
                        if (this.room.GotMusicController())
                        {
                            this.room.GetRoomMusicController().OnNewUserEnter(user);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.LogCriticalException(exception.ToString());
            }
        }

        private void onUserUpdateStatus()
        {
            foreach (RoomUser user in this.UserList.Values)
            {
                this.UpdateUserStatus(user, false);
            }
        }

        internal void RemoveBot(int VirtualId, bool Kicked)
        {
            RoomUser roomUserByVirtualId = this.GetRoomUserByVirtualId(VirtualId);
            if ((roomUserByVirtualId != null) && roomUserByVirtualId.IsBot)
            {
                if (roomUserByVirtualId.IsPet)
                {
                    this.pets.Remove(roomUserByVirtualId.PetData.PetId);
                    this.petCount--;
                }
                roomUserByVirtualId.BotAI.OnSelfLeaveRoom(Kicked);
                ServerMessage message = new ServerMessage(Outgoing.UserLeftRoom);
                message.AppendString(roomUserByVirtualId.VirtualId.ToString());
                this.room.SendMessage(message);
                this.userlist.Remove(roomUserByVirtualId.InternalRoomID);
            }
        }

        private void RemoveRoomUser(RoomUser user)
        {
            this.UserList.Remove(user.InternalRoomID);
            user.InternalRoomID = -1;
            this.room.GetGameMap().GameMap[user.X, user.Y] = user.SqState;
            this.room.GetGameMap().RemoveUserFromMap(user, new Point(user.X, user.Y));
            ServerMessage message = new ServerMessage(Outgoing.UserLeftRoom);
            message.AppendString(user.VirtualId.ToString());
            this.room.SendMessage(message);
        }

        internal void RemoveUserFromRoom(GameClient Session, bool NotifyClient, bool NotifyKick)
        {
            try
            {
                if ((Session != null) && (Session.GetHabbo() != null))
                {
                    Session.GetHabbo().GetAvatarEffectsInventoryComponent().CurrentEffect = 0;
                    if (NotifyClient)
                    {
                        if (NotifyKick)
                        {
                            Session.GetMessageHandler().GetResponse().Init(Outgoing.RoomError);
                            Session.GetMessageHandler().GetResponse().AppendInt32(0xfa8);
                            Session.GetMessageHandler().SendResponse();
                        }
                        Session.GetMessageHandler().GetResponse().Init(Outgoing.OutOfRoom);
                        Session.GetMessageHandler().SendResponse();
                    }
                    RoomUser roomUserByHabbo = this.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (roomUserByHabbo != null)
                    {
                        if (roomUserByHabbo.team != Team.none)
                        {
                            this.room.GetTeamManagerForBanzai().OnUserLeave(roomUserByHabbo);
                            this.room.GetTeamManagerForFreeze().OnUserLeave(roomUserByHabbo);
                        }
                        if (roomUserByHabbo.montandoBol)
                        {
                            roomUserByHabbo.montandoBol = false;
                            RoomUser roomUserByVirtualId = this.GetRoomUserByVirtualId((int) roomUserByHabbo.montandoID);
                            if (roomUserByVirtualId != null)
                            {
                                roomUserByVirtualId.montandoBol = false;
                                roomUserByVirtualId.montandoID = 0;
                            }
                        }
                        if (roomUserByHabbo.sentadoBol || roomUserByHabbo.acostadoBol)
                        {
                            roomUserByHabbo.sentadoBol = false;
                            roomUserByHabbo.acostadoBol = false;
                        }
                        this.RemoveRoomUser(roomUserByHabbo);
                        if ((Session.GetHabbo() != null) && !roomUserByHabbo.IsSpectator)
                        {
                            if (roomUserByHabbo.CurrentItemEffect != ItemEffectType.None)
                            {
                                roomUserByHabbo.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().CurrentEffect = -1;
                            }
                            if (Session.GetHabbo() != null)
                            {
                                if (this.room.HasActiveTrade(Session.GetHabbo().Id))
                                {
                                    this.room.TryStopTrade(Session.GetHabbo().Id);
                                }
                                Session.GetHabbo().CurrentRoomId = 0;
                                if (Session.GetHabbo().GetMessenger() != null)
                                {
                                    Session.GetHabbo().GetMessenger().OnStatusChanged(true);
                                }
                            }
                        }
                        this.usersByUserID.Remove(roomUserByHabbo.userID);
                        if (Session.GetHabbo() != null)
                        {
                            this.usersByUsername.Remove(Session.GetHabbo().Username.ToLower());
                        }
                        roomUserByHabbo.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.LogCriticalException("Error during removing user from room:" + exception.ToString());
            }
        }

        internal void RequestRoomReload()
        {
            this.userlist.QueueDelegate(new onCycleDoneDelegate(this.room.onReload));
        }

        internal void SavePets(IQueryAdapter dbClient)
        {
            try
            {
                if (this.GetPets().Count > 0)
                {
                    this.AppendPetsUpdateString(dbClient);
                }
            }
            catch (Exception exception)
            {
                Logging.LogCriticalException(string.Concat(new object[] { "Error during saving furniture for room ", this.room.RoomId, ". Stack: ", exception.ToString() }));
            }
        }

        internal ServerMessage SerializeStatusUpdates(bool All)
        {
            List<RoomUser> list = new List<RoomUser>();
            foreach (RoomUser user in this.UserList.Values)
            {
                if (!All)
                {
                    if (!user.UpdateNeeded)
                    {
                        continue;
                    }
                    user.UpdateNeeded = false;
                }
                list.Add(user);
            }
            if (list.Count == 0)
            {
                return null;
            }
            ServerMessage message = new ServerMessage(Outgoing.UpdateState);
            message.AppendInt32(list.Count);
            foreach (RoomUser user in list)
            {
                user.SerializeStatus(message);
            }
            return message;
        }

        internal List<RoomItem> SquareItems(RoomUser User, bool On)
        {
            List<RoomItem> furniObjects = new List<RoomItem>();
            if (On)
            {
                furniObjects = this.room.GetRoomItemHandler().GetFurniObjects(User.X, User.Y);
                furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X, User.Y + 1));
                furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X, User.Y - 1));
                furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X + 1, User.Y));
                furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X + 1, User.Y + 1));
                furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X + 1, User.Y - 1));
                furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X - 1, User.Y));
                furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X - 1, User.Y + 1));
                furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X - 1, User.Y - 1));
                return furniObjects;
            }
            furniObjects = this.room.GetRoomItemHandler().GetFurniObjects(User.X, User.Y + 2);
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X, User.Y - 2));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X + 1, User.Y + 2));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X + 1, User.Y - 2));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X + 2, User.Y + 2));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X + 2, User.Y - 2));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X - 1, User.Y + 2));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X - 1, User.Y - 2));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X - 2, User.Y + 2));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X - 2, User.Y - 2));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X - 2, User.Y));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X + 2, User.Y));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X - 2, User.Y - 1));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X + 2, User.Y - 1));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X + 2, User.Y + 1));
            furniObjects.AddRange(this.room.GetRoomItemHandler().GetFurniObjects(User.X - 2, User.Y + 1));
            return furniObjects;
        }

        internal void TurnHeads(int X, int Y, uint SenderId)
        {
            foreach (RoomUser user in this.UserList.Values)
            {
                if (user.HabboId != SenderId)
                {
                    user.SetRot(Rotation.Calculate(user.X, user.Y, X, Y), true);
                }
            }
        }

        internal void UpdateUserCount(int count)
        {
            this.userCount = count;
            this.room.RoomData.UsersNow = count;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "UPDATE room_active SET active_users = ", count, " WHERE roomid = ", this.room.RoomId }));
            }
            if (this.room.HasOngoingEvent)
            {
                ButterflyEnvironment.GetGame().GetRoomManager().GetEventManager().QueueUpdateEvent(this.room.RoomData, this.room.Event.Category);
            }
            ButterflyEnvironment.GetGame().GetRoomManager().QueueActiveRoomUpdate(this.room.RoomData);
        }

        private void UpdateUserEffect(RoomUser User, int x, int y)
        {
            if (!User.IsBot)
            {
                byte pByte = this.room.GetGameMap().EffectMap[x, y];
                if (pByte <= 0)
                {
                    if ((User.CurrentItemEffect != ItemEffectType.None) && (pByte == 0))
                    {
                        User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyEffect(-1);
                        User.CurrentItemEffect = ItemEffectType.None;
                        User.RemoveStatus("swim");
                    }
                }
                else
                {
                    ItemEffectType type = ByteToItemEffectEnum.Parse(pByte);
                    if (type != User.CurrentItemEffect)
                    {
                        switch (type)
                        {
                            case ItemEffectType.None:
                                User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(-1);
                                User.CurrentItemEffect = type;
                                break;

                            case ItemEffectType.Swim:
                                ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("SWIM", User.GetClient());
                                User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(0x1d);
                                User.CurrentItemEffect = type;
                                break;

                            case ItemEffectType.SwimLow:
                                ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("SWIM", User.GetClient());
                                User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(30);
                                User.CurrentItemEffect = type;
                                break;

                            case ItemEffectType.SwimHalloween:
                                ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("SWIM", User.GetClient());
                                User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(0x25);
                                User.CurrentItemEffect = type;
                                break;

                            case ItemEffectType.Iceskates:
                                if (!(User.GetClient().GetHabbo().Gender.ToLower() == "m"))
                                {
                                    User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(0x27);
                                }
                                else
                                {
                                    User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(0x26);
                                }
                                User.CurrentItemEffect = ItemEffectType.Iceskates;
                                ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("ICE_SKATE", User.GetClient());
                                return;

                            case ItemEffectType.Normalskates:
                                if (!(User.GetClient().GetHabbo().Gender.ToLower() == "m"))
                                {
                                    User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(0x38);
                                }
                                else
                                {
                                    User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(0x37);
                                }
                                User.CurrentItemEffect = type;
                                ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("ROLLER_SKATE", User.GetClient());
                                return;

                            case ItemEffectType.PublicPool:
                                User.AddStatus("swim", string.Empty);
                                User.CurrentItemEffect = type;
                                break;
                        }
                    }
                }
            }
        }

        internal void UpdateUserStats(List<RoomUser> users, Hashtable userID, Hashtable userName, int primaryID, int secondaryID)
        {
            foreach (RoomUser user in users)
            {
                this.userlist.Inner.Add(user.InternalRoomID, user);
            }
            foreach (RoomUser user in userName.Values)
            {
                this.usersByUsername.Add(user.GetClient().GetHabbo().Username.ToLower(), user);
            }
            foreach (RoomUser user in userID.Values)
            {
                this.usersByUserID.Add(user.userID, user);
            }
            this.primaryPrivateUserID = primaryID;
            this.secondaryPrivateUserID = secondaryID;
            this.room.InitPets();
            this.room.InitUserBots();
            
        }

        internal void UpdateUserStatus(RoomUser User, bool cyclegameitems)
        {
            try
            {
                if (User != null)
                {
                    double num;
                    if (User.IsBot)
                    {
                        cyclegameitems = false;
                    }
                    if (User.Statusses.ContainsKey("lay") || User.Statusses.ContainsKey("sit"))
                    {
                        User.Statusses.Remove("lay");
                        User.Statusses.Remove("sit");
                        User.UpdateNeeded = true;
                    }
                    if (User.Statusses.ContainsKey("sign"))
                    {
                        User.Statusses.Remove("sign");
                        User.UpdateNeeded = true;
                    }
                    List<RoomItem> allRoomItemForSquare = new CoordItemSearch(this.room.GetGameMap().CoordinatedItems).GetAllRoomItemForSquare(User.X, User.Y);
                    if (!(!User.montandoBol || User.IsPet))
                    {
                        num = this.room.GetGameMap().SqAbsoluteHeight(User.X, User.Y, allRoomItemForSquare) + 1.0;
                    }
                    else
                    {
                        num = this.room.GetGameMap().SqAbsoluteHeight(User.X, User.Y, allRoomItemForSquare);
                    }
                    if (num != User.Z)
                    {
                        User.Z = num;
                        if (User.isFlying)
                        {
                            User.Z += 4.0 + (0.5 * Math.Sin(0.7 * User.flyk));
                        }
                        User.UpdateNeeded = true;
                    }
                    DynamicRoomModel model = this.room.GetGameMap().Model;
                    if (((model.SqState[User.X, User.Y] == SquareState.SEAT) || User.sentadoBol) || User.acostadoBol)
                    {
                        if (User.sentadoBol)
                        {
                            if (!User.Statusses.ContainsKey("sit"))
                            {
                                User.Statusses.Add("sit", Convert.ToString((double) (model.SqFloorHeight[User.X, User.Y] + 0.55)).Replace(",", "."));
                            }
                            User.Z = model.SqFloorHeight[User.X, User.Y];
                            User.UpdateNeeded = true;
                        }
                        else if (User.acostadoBol)
                        {
                            if (!User.Statusses.ContainsKey("lay"))
                            {
                                User.Statusses.Add("lay", Convert.ToString((double) (model.SqFloorHeight[User.X, User.Y] + 0.55)).Replace(",", "."));
                            }
                            User.Z = model.SqFloorHeight[User.X, User.Y];
                            User.UpdateNeeded = true;
                        }
                        else
                        {
                            if (!User.Statusses.ContainsKey("sit"))
                            {
                                User.Statusses.Add("sit", "1.0");
                            }
                            User.Z = model.SqFloorHeight[User.X, User.Y];
                            if (User.isFlying)
                            {
                                User.Z += 4.0 + (0.5 * Math.Sin(0.7 * User.flyk));
                            }
                            User.RotHead = model.SqSeatRot[User.X, User.Y];
                            User.RotBody = model.SqSeatRot[User.X, User.Y];
                            User.UpdateNeeded = true;
                        }
                    }
                    foreach (RoomItem item in allRoomItemForSquare)
                    {
                        int num2;
                        TeamManager teamManagerForBanzai;
                        AvatarEffectsInventoryComponent avatarEffectsInventoryComponent;
                        ServerMessage message2;
                        if (cyclegameitems)
                        {
                            item.UserWalksOnFurni(User);
                        }
                        if (item.GetBaseItem().IsSeat)
                        {
                            if (!User.Statusses.ContainsKey("sit"))
                            {
                                User.Statusses.Add("sit", TextHandling.GetString(item.GetBaseItem().Height));
                            }
                            User.Z = item.GetZ;
                            if (User.isFlying)
                            {
                                User.Z += 4.0 + (0.5 * Math.Sin(0.7 * User.flyk));
                            }
                            User.RotHead = item.Rot;
                            User.RotBody = item.Rot;
                            User.UpdateNeeded = true;
                        }
                        switch (item.GetBaseItem().InteractionType)
                        {
                            case InteractionType.bed:
                            {
                                if (!User.Statusses.ContainsKey("lay"))
                                {
                                    User.Statusses.Add("lay", TextHandling.GetString(item.GetBaseItem().Height) + " null");
                                }
                                User.Z = item.GetZ;
                                if (User.isFlying)
                                {
                                    User.Z += 4.0 + (0.1 * Math.Sin(0.7 * User.flyk));
                                }
                                User.RotHead = item.Rot;
                                User.RotBody = item.Rot;
                                User.UpdateNeeded = true;
                                continue;
                            }
                            case InteractionType.fbgate:
                            {
                                if (cyclegameitems)
                                {
                                    if (User.team != item.team)
                                    {
                                        User.team = item.team;
                                    }
                                    else if (User.team == item.team)
                                    {
                                        User.team = Team.none;
                                    }
                                    if ((!string.IsNullOrEmpty(item.Figure) && ((User != null) && !User.IsBot)) && (User.Coordinate == item.Coordinate))
                                    {
                                        if ((User.GetClient().GetHabbo().Gender != item.Gender) && (User.GetClient().GetHabbo().Look != item.Figure))
                                        {
                                            User.GetClient().GetHabbo().tempGender = User.GetClient().GetHabbo().Gender;
                                            User.GetClient().GetHabbo().tempLook = User.GetClient().GetHabbo().Look;
                                            User.GetClient().GetHabbo().Gender = item.Gender;
                                            User.GetClient().GetHabbo().Look = item.Figure;
                                        }
                                        else
                                        {
                                            User.GetClient().GetHabbo().Gender = User.GetClient().GetHabbo().tempGender;
                                            User.GetClient().GetHabbo().Look = User.GetClient().GetHabbo().tempLook;
                                        }
                                        ServerMessage message = new ServerMessage(Outgoing.UpdateUserInformation);
                                        message.AppendInt32(User.VirtualId);
                                        message.AppendString(User.GetClient().GetHabbo().Look);
                                        message.AppendString(User.GetClient().GetHabbo().Gender.ToLower());
                                        message.AppendString(User.GetClient().GetHabbo().Motto);
                                        message.AppendInt32(User.GetClient().GetHabbo().AchievementPoints);
                                        this.room.SendMessage(message);
                                    }
                                }
                                continue;
                            }
                            case InteractionType.banzaigateblue:
                            case InteractionType.banzaigatered:
                            case InteractionType.banzaigateyellow:
                            case InteractionType.banzaigategreen:
                            {
                                if (cyclegameitems)
                                {
                                    num2 = ((int) item.team) + 0x20;
                                    teamManagerForBanzai = User.GetClient().GetHabbo().CurrentRoom.GetTeamManagerForBanzai();
                                    avatarEffectsInventoryComponent = User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent();
                                    if (User.GetClient().GetHabbo().QuestActivate == 0x1c)
                                    {
                                        ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("GAME_JOIN_GAME", User.GetClient());
                                    }
                                    if (User.team == item.team)
                                    {
                                        break;
                                    }
                                    if (teamManagerForBanzai.CanEnterOnTeam(item.team))
                                    {
                                        if (User.team != Team.none)
                                        {
                                            teamManagerForBanzai.OnUserLeave(User);
                                        }
                                        User.team = item.team;
                                        teamManagerForBanzai.AddUser(User);
                                        if (avatarEffectsInventoryComponent.CurrentEffect != num2)
                                        {
                                            avatarEffectsInventoryComponent.ApplyCustomEffect(num2);
                                        }
                                    }
                                }
                                continue;
                            }
                            case InteractionType.banzaitele:
                            {
                                this.room.GetGameItemHandler().onTeleportRoomUserEnter(User, item);
                                continue;
                            }
                            case InteractionType.freezeyellowgate:
                            case InteractionType.freezeredgate:
                            case InteractionType.freezegreengate:
                            case InteractionType.freezebluegate:
                                if (!cyclegameitems)
                                {
                                    continue;
                                }
                                num2 = ((int) item.team) + 0x27;
                                teamManagerForBanzai = User.GetClient().GetHabbo().CurrentRoom.GetTeamManagerForFreeze();
                                avatarEffectsInventoryComponent = User.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent();
                                if (User.GetClient().GetHabbo().QuestActivate == 0x30)
                                {
                                    ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("GAME_JOIN_GAME", User.GetClient());
                                }
                                if (User.team == item.team)
                                {
                                    goto Label_0A84;
                                }
                                if (teamManagerForBanzai.CanEnterOnTeam(item.team))
                                {
                                    if (User.team != Team.none)
                                    {
                                        teamManagerForBanzai.OnUserLeave(User);
                                    }
                                    User.team = item.team;
                                    teamManagerForBanzai.AddUser(User);
                                    if (avatarEffectsInventoryComponent.CurrentEffect != num2)
                                    {
                                        avatarEffectsInventoryComponent.ApplyCustomEffect(num2);
                                    }
                                }
                                goto Label_0AB3;

                            default:
                            {
                                continue;
                            }
                        }
                        teamManagerForBanzai.OnUserLeave(User);
                        if (avatarEffectsInventoryComponent.CurrentEffect == num2)
                        {
                            avatarEffectsInventoryComponent.ApplyCustomEffect(0);
                        }
                        User.team = Team.none;
                        continue;
                    Label_0A84:
                        teamManagerForBanzai.OnUserLeave(User);
                        if (avatarEffectsInventoryComponent.CurrentEffect == num2)
                        {
                            avatarEffectsInventoryComponent.ApplyCustomEffect(0);
                        }
                        User.team = Team.none;
                    Label_0AB3:
                        message2 = new ServerMessage(Outgoing.InRoomGameStatus);
                        message2.AppendBoolean(User.team != Team.none);
                        User.GetClient().SendMessage(message2);
                    }
                    if (cyclegameitems)
                    {
                        if (this.room.GotSoccer())
                        {
                            this.room.GetSoccer().OnUserWalk(User);
                        }
                        if (this.room.GotBanzai())
                        {
                            this.room.GetBanzai().OnUserWalk(User);
                        }
                        this.room.GetFreeze().OnUserWalk(User);
                    }
                }
            }
            catch
            {
            }
        }

        internal void UpdateUserStatusses()
        {
            onCycleDoneDelegate function = new onCycleDoneDelegate(this.onUserUpdateStatus);
            this.UserList.QueueDelegate(function);
        }

        internal int PetCount
        {
            get
            {
                return this.petCount;
            }
        }

        internal QueuedDictionary<int, RoomUser> UserList
        {
            get
            {
                return this.userlist;
            }
        }
    }
}

