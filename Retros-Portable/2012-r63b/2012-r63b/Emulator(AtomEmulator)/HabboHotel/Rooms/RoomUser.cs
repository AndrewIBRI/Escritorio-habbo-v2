using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.HabboHotel.ChatMessageStorage;
using Atom.HabboHotel.GameClients;
using Atom.HabboHotel.Misc;
using Atom.HabboHotel.Pathfinding;
using Atom.HabboHotel.Pets;
using Atom.HabboHotel.RoomBots;
using Atom.HabboHotel.Rooms.Games;
using Atom.IRC;
using Atom.Messages;
using Uber.HabboHotel.Rooms;
using System.Drawing;
using Atom.Core;
using Database_Manager.Database.Session_Details.Interfaces;
using HabboEvents;

namespace Atom.HabboHotel.Rooms
{
    public class RoomUser : IEquatable<RoomUser>
    {
        internal UInt32 HabboId;
        internal Int32 VirtualId;
        internal UInt32 RoomId;

        internal int IdleTime;//byte
        //internal int Steps;

        internal int X;//byte
        internal int Y;//byte
        internal double Z;
        internal byte SqState;

        internal int CarryItemID;//byte
        internal int CarryTimer;//byte

        internal int RotHead;//byte
        internal int RotBody;//byte

        internal bool CanWalk;
        internal bool AllowOverride;
        internal bool TeleportEnabled;

        internal int GoalX;//byte
        internal int GoalY;//byte

        internal Boolean SetStep;
        internal int SetX;//byte
        internal int SetY;//byte
        internal double SetZ;
        internal uint userID;

        internal RoomBot BotData;
        internal BotAI BotAI;

        internal ItemEffectType CurrentItemEffect;
        internal bool Freezed;
        internal int FreezeCounter;
        internal Team team;
        internal FreezePowerUp banzaiPowerUp;
        internal int FreezeLives;

        internal bool shieldActive;
        internal int shieldCounter;

        internal bool throwBallAtGoal;
        internal bool moonwalkEnabled = false;
        internal bool isFlying = false;
        internal int flyk = 0;
        internal bool montandoBol = false;
        internal bool sentadoBol = false;
        internal bool acostadoBol = false;
        internal uint montandoID = 0;

        internal Point Coordinate
        {
            get
            {
                return new Point(X, Y);
            }
        }

        public bool Equals(RoomUser comparedUser)
        {
            return (comparedUser.HabboId == this.HabboId);
        }

        internal bool IsPet
        {
            get
            {
                return (IsBot && BotData.IsPet);
            }
        }

        internal string GetUsername()
        {
            if (IsBot)
                return string.Empty;

            if (this.GetClient() != null)
            {
                return GetClient().GetHabbo().Username;
            }

            return string.Empty;
        }

        internal int CurrentEffect
        {
            get
            {
                return GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().CurrentEffect;
            }
        }

        internal Pet PetData;

        internal Boolean IsWalking;
        internal Boolean UpdateNeeded;
        internal Boolean IsAsleep;

        internal Dictionary<string, string> Statusses;

        internal int DanceId;

        //internal List<Coord> Path;
        //internal int PathStep;

        //internal bool PathRecalcNeeded;
        //internal int PathRecalcX;
        //internal int PathRecalcY;
        //private int mMessageAmount;

        //internal int TeleDelay;//byte
        private int FloodCount;//byte

        internal Boolean IsDancing
        {
            get
            {
                if (DanceId >= 1)
                {
                    return true;
                }

                return false;
            }
        }

        internal Boolean NeedsAutokick
        {
            get
            {
                if (IsBot)
                    return false;
                if (GetClient() == null || GetClient().GetHabbo() == null)
                    return true;
                if (GetClient().GetHabbo().Rank > 1)
                    return false;
                if (IdleTime >= 1800)
                    return true;

                return false;
            }
        }

        internal bool IsTrading
        {
            get
            {
                if (IsBot)
                {
                    return false;
                }

                if (Statusses.ContainsKey("trd"))
                {
                    return true;
                }

                return false;
            }
        }

        internal bool IsOwner()
        {
            if (IsBot)
                return false;
            return (GetUsername() == GetRoom().Owner);
        }

        internal bool IsBot
        {
            get
            {
                if (this.BotData != null)
                {
                    return true;
                }

                return false;
            }
        }

        internal bool IsSpectator;

        internal int InternalRoomID;

        private Queue events;

        internal RoomUser(uint HabboId, uint RoomId, int VirtualId, Room room, bool isSpectator)
        {
            this.Freezed = false;
            this.HabboId = HabboId;
            this.RoomId = RoomId;
            this.VirtualId = VirtualId;
            this.IdleTime = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.RotHead = 0;
            this.RotBody = 0;
            this.UpdateNeeded = true;
            this.Statusses = new Dictionary<string, string>();
            //this.Path = new List<Coord>();
            //this.PathStep = 0;
            //this.TeleDelay = -1;
            this.mRoom = room;

            this.AllowOverride = false;
            this.CanWalk = true;

            this.IsSpectator = isSpectator;
            this.SqState = 3;
            //this.Steps = 0;

            this.InternalRoomID = 0;
            this.CurrentItemEffect = ItemEffectType.None;
            this.events = new Queue();
            this.FreezeLives = 0;
            //this.mMessageAmount = 0;
        }

        internal RoomUser(uint HabboId, uint RoomId, int VirtualId, GameClient pClient, Room room)
        {
            this.mClient = pClient;
            this.Freezed = false;
            this.HabboId = HabboId;
            this.RoomId = RoomId;
            this.VirtualId = VirtualId;
            this.IdleTime = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.RotHead = 0;
            this.RotBody = 0;
            this.UpdateNeeded = true;
            this.Statusses = new Dictionary<string, string>();
            //this.Path = new List<Coord>();
            //this.PathStep = 0;
            //this.TeleDelay = -1;

            this.AllowOverride = false;
            this.CanWalk = true;

            this.IsSpectator = false;
            this.SqState = 3;
            //this.Steps = 0;

            this.InternalRoomID = 0;
            this.CurrentItemEffect = ItemEffectType.None;
            this.mRoom = room;
            this.events = new Queue();
        }

        internal void Unidle()
        {
            this.IdleTime = 0;

            if (this.IsAsleep)
            {
                this.IsAsleep = false;

                ServerMessage Message = new ServerMessage(Outgoing.IdleStatus);
                Message.AppendInt32(VirtualId);
                Message.AppendBoolean(false);

                GetRoom().SendMessage(Message);
            }
        }

        internal void OnFly()
        {
            if (flyk == 0)
            {
                flyk++;
                return;
            }

            double lastK = 0.5 * Math.Sin(0.7 * flyk);
            flyk++;
            double nextK = 0.5 * Math.Sin(0.7 * flyk);
            double differance = nextK - lastK;

            GetRoom().SendMessage(GetRoom().GetRoomItemHandler().UpdateUserOnRoller(this, this.Coordinate, 0, this.Z + differance));
        }

        internal void Dispose()
        {
            Statusses.Clear();
            mRoom = null;
            mClient = null;
        }

        internal void Chat(GameClient Session, string Message, bool Shout)
        {
            
            if (Session != null)
            {
                if (Session.GetHabbo().Rank < 5)
                {
                    if (GetRoom().RoomMuted)
                        return;
                }
            }
            
            Unidle();

            if (IsPet||IsBot) {
                goto NoCheckings;
            }

            Users.Habbo clientUser = GetClient().GetHabbo();
            if (clientUser.Muted)
            {
                GetClient().SendNotif("You are muted.");
                return;
            }

            if (Message.StartsWith(":") && Session != null)
            {
                string[] parsedCommand = Message.Split(' ');
                if (ChatCommandRegister.IsChatCommand(parsedCommand[0].ToLower().Substring(1)))
                {
                    ChatCommandHandler handler = new ChatCommandHandler(Message.Split(' '), Session);

                    if (handler.WasExecuted())
                    {
                        Logging.LogMessage(string.Format(" [" + DateTime.Now.ToString("T") + "] [INFO] → {0} ha usado el comando {1}", GetUsername(), Message));
                        if (Session.GetHabbo().Rank > 5)
                        {
                            AtomEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, string.Empty, "Chat command", string.Format("Issued chat command {0}", Message));
                        }
                        return;
                    }
                }
            }


            uint rank = 1;
            Message = LanguageLocale.FilterSwearwords(Message);
            if (Session != null && Session.GetHabbo() != null)
                rank = Session.GetHabbo().Rank;
            TimeSpan SinceLastMessage = DateTime.Now - clientUser.spamFloodTime;
            if (SinceLastMessage.TotalSeconds > clientUser.spamProtectionTime && clientUser.spamProtectionBol == true)
            {
                FloodCount = 0;
                clientUser.spamProtectionBol = false;
                clientUser.spamProtectionAbuse = 0;
            }
            else
            {
                if (SinceLastMessage.TotalSeconds > 4)
                    FloodCount = 0;
            }

            if (SinceLastMessage.TotalSeconds < clientUser.spamProtectionTime && clientUser.spamProtectionBol == true)
            {
                ServerMessage Packet = new ServerMessage(Outgoing.FloodFilter);
                int timeToWait=clientUser.spamProtectionTime - SinceLastMessage.Seconds;
                Packet.AppendInt32(timeToWait); //Blocked for X sec
                GetClient().SendMessage(Packet);

                if (AtomEnvironment.spamBans == true)
                {
                    clientUser.spamProtectionAbuse++;
                    GameClient toBan;
                    toBan = AtomEnvironment.GetGame().GetClientManager().GetClientByUsername(Session.GetHabbo().Username);
                    if (clientUser.spamProtectionAbuse >= AtomEnvironment.spamBans_limit)
                    {
                        AtomEnvironment.GetGame().GetBanManager().BanUser(toBan, "SPAM*ABUSE", 800, LanguageLocale.GetValue("flood.banmessage"), false);
                    }
                    else
                    {
                        toBan.SendNotif(LanguageLocale.GetValue("flood.pleasewait").Replace("%secs%", Convert.ToString(timeToWait)));
                    }
                }
                return;
            }

            if (SinceLastMessage.TotalSeconds < 4 && FloodCount > 5 && rank < 5)
            {
                ServerMessage Packet = new ServerMessage(Outgoing.FloodFilter);
                clientUser.spamProtectionCount += 1;
                if (clientUser.spamProtectionCount % 2 == 0)
                {
                    clientUser.spamProtectionTime = (10 * clientUser.spamProtectionCount);
                }
                else
                {
                    clientUser.spamProtectionTime = 10 * (clientUser.spamProtectionCount - 1);
                }
                clientUser.spamProtectionBol = true;
                Packet.AppendInt32(clientUser.spamProtectionTime - SinceLastMessage.Seconds); //Blocked for X sec
                GetClient().SendMessage(Packet);
                return;
            }

            clientUser.spamFloodTime = DateTime.Now;
            FloodCount++;

            if (!IsBot)
            {
                AtomEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, HabboHotel.Quests.QuestType.SOCIAL_CHAT);
                IQueryAdapter DbClient = AtomEnvironment.GetDatabaseManager().getQueryreactor();

                if (AtomEnvironment.GetConfig().data["game.chatlog"] == "1")
                {
                    DbClient.setQuery("INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('" + Session.GetHabbo().Id + "','" + GetRoom().RoomId + "','" + DateTime.Now.Hour + "','" + DateTime.Now.Minute + "','" + AtomEnvironment.GetUnixTimestamp() + "',@message,@username,'" + DateTime.Now.ToLongDateString() + "')");
                    DbClient.addParameter("message", Message);
                    DbClient.addParameter("username", Session.GetHabbo().Username);
                    DbClient.runQuery();
                }
            }

        NoCheckings:
            InvokedChatMessage message = new InvokedChatMessage(this, Message, Shout);
            GetRoom().QueueChatMessage(message);
        }

        internal void OnChat(InvokedChatMessage message)
        {
            string Message = message.message;

            if (GetRoom() != null && !GetRoom().AllowsShous(this, Message))
                return;

            int ChatHeader = Outgoing.Talk;

            if (message.shout)
            {
                ChatHeader = Outgoing.Shout;
            }

            string Site = "";

            ServerMessage ChatMessage = new ServerMessage(ChatHeader);
            ChatMessage.AppendInt32(VirtualId);

            //if (Message.Contains("http://") || Message.Contains("www."))
            //{
            //    string[] Split = Message.Split(' ');

            //    foreach (string Msg in Split)
            //    {
            //        if (Msg.StartsWith("http://") || Msg.StartsWith("www."))
            //        {
            //            Site = Msg;
            //        }
            //    }

            //    Message = Message.Replace(Site, "{0}");
            //}

            ChatMessage.AppendStringWithBreak(Message);

            if (!string.IsNullOrEmpty(Site))
            {
                ChatMessage.AppendBoolean(false);
                ChatMessage.AppendBoolean(true);
                ChatMessage.AppendStringWithBreak(Site.Replace("http://", string.Empty));
                ChatMessage.AppendStringWithBreak(Site);
            }

            ChatMessage.AppendInt32(GetSpeechEmotion(Message));
            ChatMessage.AppendInt32(0);
            ChatMessage.AppendInt32(-1);

            GetRoom().GetRoomUserManager().TurnHeads(X, Y, HabboId);
            GetRoom().SendMessage(ChatMessage);

            if (!IsBot)
            {
                GetRoom().OnUserSay(this, Message, message.shout);
                LogMessage(Message);
            }

            message.Dispose();
        }

        private static void LogMessage(string message)
        {
        //    ChatMessage chatMessage = ChatMessageFactory.CreateMessage(message, GetClient(), GetRoom());

        //    foreach (RoomUser user in GetRoom().UserList.Values)
        //    {
        //        if (!user.IsBot && user.GetClient() != null && user.GetClient().GetHabbo() != null)
        //            user.GetClient().GetHabbo().GetChatMessageManager().AddMessage(chatMessage);
        //    }

        //    GetRoom().GetChatMessageManager().AddMessage(chatMessage);
        }

        internal static int GetSpeechEmotion(string Message)
        {
            Message = Message.ToLower();

            if (Message.Contains(":)") || Message.Contains(":d") || Message.Contains("=]") || 
                Message.Contains("=d") || Message.Contains(":>"))
            {
                return 1;
            }

            if (Message.Contains(">:(") || Message.Contains(":@"))
            {
                return 2;
            }

            if (Message.Contains(":o"))
            {
                return 3;
            }

            if (Message.Contains(":(") || Message.Contains("=[") || Message.Contains(":'(") || Message.Contains("='["))
            {
                return 4;
            }

            return 0;
        }

        internal void ClearMovement(bool Update)
        {
            IsWalking = false;
            Statusses.Remove("mv");
            GoalX = 0;
            GoalY = 0;
            SetStep = false;
            SetX = 0;
            SetY = 0;
            SetZ = 0;

            if (Update)
            {
                UpdateNeeded = true;
            }
        }

        internal void MoveTo(Point c)
        {
            MoveTo(c.X, c.Y);
        }

        internal void MoveTo(int pX, int pY, bool pOverride)
        {
            if (GetRoom().GetGameMap().SquareHasUsers(pX, pY) && !pOverride)
                return;

            Unidle();

            if (TeleportEnabled)
            {
                GetRoom().SendMessage(GetRoom().GetRoomItemHandler().UpdateUserOnRoller(this, new Point(pX, pY), 0, GetRoom().GetGameMap().SqAbsoluteHeight(GoalX, GoalY)));
                GetRoom().GetRoomUserManager().UpdateUserStatus(this, false);
                return;
            }

            IsWalking = true;
            GoalX = pX;
            GoalY = pY;
            throwBallAtGoal = false;
        }

        internal void MoveTo(int pX, int pY)
        {
            MoveTo(pX, pY, false);
        }

        internal void UnlockWalking()
        {
            this.AllowOverride = false;
            this.CanWalk = true;
        }

        internal void SetPos(int pX, int pY, double pZ)
        {
            this.X = pX;
            this.Y = pY;
            this.Z = pZ;
            if (isFlying)
                Z += 4 + 0.5 * Math.Sin(0.7 * flyk);
        }

        internal void CarryItem(int Item)
        {
            this.CarryItemID = Item;

            if (Item > 0)
            {
                this.CarryTimer = 240;
            }
            else
            {
                this.CarryTimer = 0;
            }

            ServerMessage Message = new ServerMessage(Outgoing.ApplyCarryItem);
            Message.AppendInt32(VirtualId);
            Message.AppendInt32(Item);
            GetRoom().SendMessage(Message);
        }


        internal void SetRot(int Rotation)
        {
            SetRot(Rotation, false); //**************
        }

        internal void SetRot(int Rotation, bool HeadOnly)
        {
            if (Statusses.ContainsKey("lay") || IsWalking)
            {
                return;
            }

            int diff = this.RotBody - Rotation;

            this.RotHead = this.RotBody;

            if (Statusses.ContainsKey("sit") || HeadOnly)
            {
                if (RotBody == 2 || RotBody == 4)
                {
                    if (diff > 0)
                    {
                        RotHead = RotBody - 1;
                    }
                    else if (diff < 0)
                    {
                        RotHead = RotBody + 1;
                    }
                }
                else if (RotBody == 0 || RotBody == 6)
                {
                    if (diff > 0)
                    {
                        RotHead = RotBody - 1;
                    }
                    else if (diff < 0)
                    {
                        RotHead = RotBody + 1;
                    }
                }
            }
            else if (diff <= -2 || diff >= 2)
            {
                this.RotHead = Rotation;
                this.RotBody = Rotation;
            }
            else
            {
                this.RotHead = Rotation;
            }

            this.UpdateNeeded = true;
        }

        internal void AddStatus(string Key, string Value)
        {
            Statusses[Key] = Value;
        }

        internal void RemoveStatus(string Key)
        {
            if (Statusses.ContainsKey(Key))
            {
                Statusses.Remove(Key);
            }
        }

        internal void ApplyEffect(int effectID)
        {
            if (IsBot || GetClient() == null || GetClient().GetHabbo() == null || GetClient().GetHabbo().GetAvatarEffectsInventoryComponent() == null)
                return;

            GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(effectID);
        }

        //internal void ResetStatus()
        //{
        //    Statusses = new Dictionary<string, string>();
        //}

        internal void Serialize(ServerMessage Message, bool gotPublicRoom)
        {
            if (Message == null)
                return;

            if (IsSpectator)
                return;

            if (IsBot)
            {
                Message.AppendInt32(BotAI.BaseId);
                Message.AppendStringWithBreak(BotData.Name);
                Message.AppendStringWithBreak(BotData.Motto);
                // no saddle:2 2 -1 1 3 -1 1
                // with saddle (cool): 3 2 -1 1 4 10 0 3 -1 1
                // with saddle (normal):  3 2 -1 1 3 -1 1 4 9 0
                Message.AppendStringWithBreak(BotData.Look.ToLower() + ((PetData.HaveSaddle) ? " 3 4 10 0 2 " + PetData.PetHair + " " + PetData.HairDye + " 3 " + PetData.PetHair + " " + PetData.HairDye + "" : " 2 2 " + PetData.PetHair + " " + PetData.HairDye + " 3 " + PetData.PetHair + " " + PetData.HairDye + ""));
                Message.AppendInt32(VirtualId);
                Message.AppendInt32(X);
                Message.AppendInt32(Y);
                Message.AppendStringWithBreak(TextHandling.GetString(Z));
                Message.AppendInt32(0);
                Message.AppendInt32((BotData.AiType == AIType.Pet) ? 2 : 3);
                if (BotData.AiType == AIType.Pet)
                {
                    Message.AppendUInt(PetData.Type);
                    Message.AppendUInt(PetData.OwnerId); // userid
                    Message.AppendString(PetData.OwnerName); // username
                    Message.AppendInt32(1);
                    Message.AppendBoolean(PetData.HaveSaddle);
                    Message.AppendBoolean(montandoBol);
                    Message.AppendInt32(0);
                    Message.AppendInt32(0);
                    Message.AppendString("");
                }
            }
            else if (!IsBot && GetClient() != null && GetClient().GetHabbo() != null)
            {
                Users.Habbo User = GetClient().GetHabbo();
                Message.AppendUInt(User.Id);
                Message.AppendStringWithBreak(User.Username);
                Message.AppendStringWithBreak(User.Motto);
                Message.AppendStringWithBreak(User.Look);
                Message.AppendInt32(VirtualId);
                Message.AppendInt32(X);
                Message.AppendInt32(Y);
                Message.AppendStringWithBreak(TextHandling.GetString(Z));
                Message.AppendInt32(0);
                Message.AppendInt32(1);
                Message.AppendStringWithBreak(User.Gender.ToLower());
                if (User.userGroup != null)
                {
                    Message.AppendInt32(User.userGroup.groupID);
                    Message.AppendInt32(1);
                    Message.AppendString(User.userGroup.groupDescription);
                }
                else
                {
                    Message.AppendInt32(0);
                    Message.AppendInt32(0);
                    Message.AppendString("");
                }
                Message.AppendBoolean(false);
                Message.AppendInt32(0);
                Message.AppendBoolean(true);
            }
        }

        internal void SerializeStatus(ServerMessage Message)
        {
            if (IsSpectator)
            {
                return;
            }

            Message.AppendInt32(VirtualId);
            Message.AppendInt32(X);
            Message.AppendInt32(Y);
            Message.AppendStringWithBreak(TextHandling.GetString(Z));
            Message.AppendInt32(RotHead);
            Message.AppendInt32(RotBody);
            StringBuilder StatusComposer = new StringBuilder();
            StatusComposer.Append("/");

            foreach (KeyValuePair<string, string> Status in Statusses)
            {
                StatusComposer.Append(Status.Key);

                if (Status.Value != string.Empty)
                {
                    StatusComposer.Append(" ");
                    StatusComposer.Append(Status.Value);
                }

                StatusComposer.Append("/");
            }

            StatusComposer.Append("/");
            Message.AppendString(StatusComposer.ToString());
        }

        internal void SerializeStatus(ServerMessage Message, String Status)
        {
            if (IsSpectator)
            {
                return;
            }

            Message.AppendInt32(VirtualId);
            Message.AppendInt32(X);
            Message.AppendInt32(Y);
            Message.AppendStringWithBreak(TextHandling.GetString(Z));
            Message.AppendInt32(RotHead);
            Message.AppendInt32(RotBody);
            StringBuilder StatusComposer = new StringBuilder();
            Message.AppendString(Status);
        }

        private GameClient mClient;
        internal GameClient GetClient()
        {
            if (IsBot)
            {
                return null;
            }
            if (mClient == null)
                mClient = AtomEnvironment.GetGame().GetClientManager().GetClientByUserID(HabboId);
            return mClient;
        }

        private Room mRoom;
        private Room GetRoom()
        {
            if (mRoom == null)
                mRoom = AtomEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
            return mRoom;
        }
    }

    internal enum ItemEffectType
    {
        None,
        Swim,
        SwimLow,
        SwimHalloween,
        Iceskates,
        Normalskates,
        PublicPool
        //Skateboard?
    }

    internal static class ByteToItemEffectEnum
    {
        internal static ItemEffectType Parse(byte pByte)
        {
            switch (pByte)
            {
                case 0:
                    return ItemEffectType.None;
                case 1:
                    return ItemEffectType.Swim;
                case 2:
                    return ItemEffectType.Normalskates;
                case 3:
                    return ItemEffectType.Iceskates;
                case 4:
                    return ItemEffectType.SwimLow;
                case 5:
                    return ItemEffectType.SwimHalloween;
                case 6:
                    return ItemEffectType.PublicPool;
                default:
                    return ItemEffectType.None;
            }
        }
    }
    //0 = none
    //1 = pool
    //2 = normal skates
    //3 = ice skates
}
