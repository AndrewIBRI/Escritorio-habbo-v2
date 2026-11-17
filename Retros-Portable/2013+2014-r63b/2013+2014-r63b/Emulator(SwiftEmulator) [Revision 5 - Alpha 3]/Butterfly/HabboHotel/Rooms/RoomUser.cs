namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.ChatMessageStorage;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Guilds;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Misc;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.RoomBots;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.Messages;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;
    using Uber.HabboHotel.Rooms;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    public class RoomUser : IEquatable<RoomUser>
    {
        internal bool acostadoBol;
        internal bool AllowOverride;
        internal bool alreadyVoted;
        internal FreezePowerUp banzaiPowerUp;
        internal Butterfly.HabboHotel.RoomBots.BotAI BotAI;
        internal RoomBot BotData;
        internal bool CanWalk;
        internal int CarryItemID;
        internal int CarryTimer;
        internal ItemEffectType CurrentItemEffect;
        internal int DanceId;
        private Queue events;
        private int FloodCount;
        internal int flyk;
        internal int FreezeCounter;
        internal bool Freezed;
        internal int FreezeLives;
        internal int GoalX;
        internal int GoalY;
        internal uint HabboId;
        internal int IdleTime;
        internal int InternalRoomID;
        internal bool IsAsleep;
        internal bool isFlying;
        internal bool IsSpectator;
        internal bool IsWalking;
        private GameClient mClient;
        internal bool montandoBol;
        internal uint montandoID;
        internal bool moonwalkEnabled;
        private Room mRoom;
        internal Pet PetData;
        internal uint RoomId;
        internal int RotBody;
        internal int RotHead;
        internal bool sentadoBol;
        internal bool SetStep;
        internal int SetX;
        internal int SetY;
        internal double SetZ;
        internal bool shieldActive;
        internal int shieldCounter;
        internal byte SqState;
        internal bool SquareGuildGate;
        internal Dictionary<string, string> Statusses;
        internal Team team;
        internal bool TeleportEnabled;
        internal bool throwBallAtGoal;
        internal bool UpdateNeeded;
        internal uint userID;
        internal int VirtualId;
        internal int votes;
        internal int X;
        internal int Y;
        internal double Z;

        internal RoomUser(uint HabboId, uint RoomId, int VirtualId, GameClient pClient, Room room)
        {
            this.SquareGuildGate = false;
            this.moonwalkEnabled = false;
            this.isFlying = false;
            this.flyk = 0;
            this.montandoBol = false;
            this.sentadoBol = false;
            this.acostadoBol = false;
            this.montandoID = 0;
            this.alreadyVoted = false;
            this.votes = 0;
            this.mClient = pClient;
            this.Freezed = false;
            this.HabboId = HabboId;
            this.RoomId = RoomId;
            this.VirtualId = VirtualId;
            this.IdleTime = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0.0;
            this.RotHead = 0;
            this.RotBody = 0;
            this.UpdateNeeded = true;
            this.Statusses = new Dictionary<string, string>();
            this.AllowOverride = false;
            this.CanWalk = true;
            this.IsSpectator = false;
            this.SqState = 3;
            this.InternalRoomID = 0;
            this.CurrentItemEffect = ItemEffectType.None;
            this.mRoom = room;
            this.events = new Queue();
        }

        internal RoomUser(uint HabboId, uint RoomId, int VirtualId, Room room, bool isSpectator)
        {
            this.SquareGuildGate = false;
            this.moonwalkEnabled = false;
            this.isFlying = false;
            this.flyk = 0;
            this.montandoBol = false;
            this.sentadoBol = false;
            this.acostadoBol = false;
            this.montandoID = 0;
            this.alreadyVoted = false;
            this.votes = 0;
            this.Freezed = false;
            this.HabboId = HabboId;
            this.RoomId = RoomId;
            this.VirtualId = VirtualId;
            this.IdleTime = 0;
            this.X = 0;
            this.Y = 0;
            this.Z = 0.0;
            this.RotHead = 0;
            this.RotBody = 0;
            this.UpdateNeeded = true;
            this.Statusses = new Dictionary<string, string>();
            this.mRoom = room;
            this.AllowOverride = false;
            this.CanWalk = true;
            this.IsSpectator = isSpectator;
            this.SqState = 3;
            this.InternalRoomID = 0;
            this.CurrentItemEffect = ItemEffectType.None;
            this.events = new Queue();
            this.FreezeLives = 0;
        }

        internal void AddStatus(string Key, string Value)
        {
            this.Statusses[Key] = Value;
        }

        internal void ApplyEffect(int effectID)
        {
            if (((!this.IsBot && (this.GetClient() != null)) && (this.GetClient().GetHabbo() != null)) && (this.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent() != null))
            {
                this.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().ApplyCustomEffect(effectID);
            }
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
            ServerMessage message = new ServerMessage(Outgoing.ApplyCarryItem);
            message.AppendInt32(this.VirtualId);
            message.AppendInt32(Item);
            this.GetRoom().SendMessage(message);
        }

        internal void Chat(GameClient Session, string Message, bool Shout, int TextColor = 0)
        {
            Message = LanguageLocale.FilterSwearwords(Message);
            if (((Session == null) || (Session.GetHabbo().Rank >= 5)) || !this.GetRoom().RoomMuted)
            {
                this.Unidle();
                if (!this.IsPet && !this.IsBot)
                {
                    ServerMessage message;
                    Habbo habbo = this.GetClient().GetHabbo();
                    if (habbo.Muted)
                    {
                        this.GetClient().SendNotif("You are muted.");
                        return;
                    }
                    if ((Message.StartsWith(":") && (Session != null)) && ChatCommandRegister.IsChatCommand(Message.Split(new char[] { ' ' })[0].ToLower().Substring(1)))
                    {
                        ChatCommandHandler handler = new ChatCommandHandler(Message.Split(new char[] { ' ' }), Session);
                        if (handler.WasExecuted())
                        {
                            Logging.LogMessage(string.Format("User {0} issued command {1}", this.GetUsername(), Message));
                            if (Session.GetHabbo().Rank > 5)
                            {
                                ButterflyEnvironment.GetGame().GetModerationTool().LogStaffEntry(Session.GetHabbo().Username, string.Empty, "Chat command", string.Format("Issued chat command {0}", Message));
                            }
                            return;
                        }
                    }
                    uint rank = 1;
                    //Message = LanguageLocale.FilterSwearwords(Message);
                    if ((Session != null) && (Session.GetHabbo() != null))
                    {
                        rank = Session.GetHabbo().Rank;
                    }
                    TimeSpan span = (TimeSpan) (DateTime.Now - habbo.spamFloodTime);
                    if ((span.TotalSeconds > habbo.spamProtectionTime) && habbo.spamProtectionBol)
                    {
                        this.FloodCount = 0;
                        habbo.spamProtectionBol = false;
                        habbo.spamProtectionAbuse = 0;
                    }
                    else if (span.TotalSeconds > 4.0)
                    {
                        this.FloodCount = 0;
                    }
                    if ((span.TotalSeconds < habbo.spamProtectionTime) && habbo.spamProtectionBol)
                    {
                        message = new ServerMessage(Outgoing.FloodFilter);
                        int i = habbo.spamProtectionTime - span.Seconds;
                        message.AppendInt32(i);
                        this.GetClient().SendMessage(message);
                        if (ButterflyEnvironment.spamBans)
                        {
                            habbo.spamProtectionAbuse++;
                            GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(Session.GetHabbo().Username);
                            if (habbo.spamProtectionAbuse >= ButterflyEnvironment.spamBans_limit)
                            {
                                ButterflyEnvironment.GetGame().GetBanManager().BanUser(clientByUsername, "SPAM*ABUSE", 800.0, LanguageLocale.GetValue("flood.banmessage"), false);
                            }
                            else
                            {
                                clientByUsername.SendNotif(LanguageLocale.GetValue("flood.pleasewait").Replace("%secs%", Convert.ToString(i)));
                            }
                        }
                        return;
                    }
                    if (((span.TotalSeconds < 4.0) && (this.FloodCount > 5)) && (rank < 5))
                    {
                        message = new ServerMessage(Outgoing.FloodFilter);
                        habbo.spamProtectionCount++;
                        if ((habbo.spamProtectionCount % 2) == 0)
                        {
                            habbo.spamProtectionTime = 10 * habbo.spamProtectionCount;
                        }
                        else
                        {
                            habbo.spamProtectionTime = 10 * (habbo.spamProtectionCount - 1);
                        }
                        habbo.spamProtectionBol = true;
                        message.AppendInt32((int) (habbo.spamProtectionTime - span.Seconds));
                        this.GetClient().SendMessage(message);
                        return;
                    }
                    habbo.spamFloodTime = DateTime.Now;
                    this.FloodCount++;
                    if (!this.IsBot)
                    {
                        ChatMessage message2 = new ChatMessage(Session.GetHabbo().Id, Session.GetHabbo().Username, this.mRoom.RoomId, this.mRoom.Name, this.mRoom.IsPublic, Message, DateTime.Now);
                        Session.GetHabbo().GetChatMessageManager().AddMessage(message2);
                        this.mRoom.GetChatMessageManager().AddMessage(message2);
                    }
                
                //if (TextColor == 0 && Session.GetHabbo().Rank >= 6)
                //{
                //    TextColor = 1;
                //}
                
                }

                

                InvokedChatMessage message3 = new InvokedChatMessage(this, Message, Shout, TextColor);
                this.GetRoom().QueueChatMessage(message3);
            }
        }

        internal void ClearMovement(bool Update)
        {
            this.IsWalking = false;
            this.Statusses.Remove("mv");
            this.GoalX = 0;
            this.GoalY = 0;
            this.SetStep = false;
            this.SetX = 0;
            this.SetY = 0;
            this.SetZ = 0.0;
            if (Update)
            {
                this.UpdateNeeded = true;
            }
        }

        internal void Dispose()
        {
            this.Statusses.Clear();
            this.mRoom = null;
            this.mClient = null;
        }

        public bool Equals(RoomUser comparedUser)
        {
            return (comparedUser.HabboId == this.HabboId);
        }

        internal GameClient GetClient()
        {
            if (this.IsBot)
            {
                return null;
            }
            if (this.mClient == null)
            {
                this.mClient = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(this.HabboId);
            }
            return this.mClient;
        }

        private Room GetRoom()
        {
            if (this.mRoom == null)
            {
                this.mRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.RoomId);
            }
            return this.mRoom;
        }

        internal static int GetSpeechEmotion(string Message)
        {
            Message = Message.ToLower();
            if (((Message.Contains(":)") || Message.Contains(":d")) || (Message.Contains("=]") || Message.Contains("=d"))) || Message.Contains(":>"))
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
            if (((Message.Contains(":(") || Message.Contains("=[")) || Message.Contains(":'(")) || Message.Contains("='["))
            {
                return 4;
            }
            return 0;
        }

        internal string GetUsername()
        {
            if (!this.IsBot && (this.GetClient() != null))
            {
                return this.GetClient().GetHabbo().Username;
            }
            return string.Empty;
        }

        internal bool IsOwner()
        {
            if (this.IsBot)
            {
                return false;
            }
            return (this.GetUsername() == this.GetRoom().Owner);
        }

        internal void MoveTo(Point c)
        {
            this.MoveTo(c.X, c.Y);
        }

        internal void MoveTo(int pX, int pY)
        {
            this.MoveTo(pX, pY, false);
        }

        internal void MoveTo(int pX, int pY, bool pOverride)
        {
            if (!this.GetRoom().GetGameMap().SquareHasUsers(pX, pY) || pOverride)
            {
                this.Unidle();
                if (this.TeleportEnabled)
                {
                    this.GetRoom().SendMessage(this.GetRoom().GetRoomItemHandler().UpdateUserOnRoller(this, new Point(pX, pY), 0, this.GetRoom().GetGameMap().SqAbsoluteHeight(this.GoalX, this.GoalY)));
                    this.GetRoom().GetRoomUserManager().UpdateUserStatus(this, false);
                }
                else
                {
                    this.IsWalking = true;
                    this.GoalX = pX;
                    this.GoalY = pY;
                    this.throwBallAtGoal = false;
                }
            }
        }

        internal void OnChat(InvokedChatMessage message)
        {
            string str = message.message;
            if ((this.GetRoom() == null) || this.GetRoom().AllowsShous(this, str))
            {
                int talk = Outgoing.Talk;
                if (message.shout)
                {
                    talk = Outgoing.Shout;
                }
                string str2 = "";
                ServerMessage message2 = new ServerMessage(talk);
                message2.AppendInt32(this.VirtualId);
                message2.AppendString(str);
                if (!string.IsNullOrEmpty(str2))
                {
                    message2.AppendBoolean(false);
                    message2.AppendBoolean(true);
                    message2.AppendString(str2.Replace("http://", string.Empty));
                    message2.AppendString(str2);
                }
                message2.AppendInt32(GetSpeechEmotion(str));
                message2.AppendInt32(message.TextColor);
                message2.AppendInt32(0);
                message2.AppendInt32(-1);
                this.GetRoom().GetRoomUserManager().TurnHeads(this.X, this.Y, this.HabboId);
                this.GetRoom().SendMessage(message2);
                if (!this.IsBot)
                {
                    this.GetRoom().OnUserSay(this, str, message.shout);
                }
                if (this.GetRoom().UserCount > 1)
                {
                    ButterflyEnvironment.GetGame().GetQuestManager().GetQuests().UpdateState("CHAT_WITH_SOMEONE", this.GetClient());
                }
                message.Dispose();
            }
        }

        internal void OnFly()
        {
            if (this.flyk == 0)
            {
                this.flyk++;
            }
            else
            {
                double num = 0.5 * Math.Sin(0.7 * this.flyk);
                this.flyk++;
                double num2 = 0.5 * Math.Sin(0.7 * this.flyk);
                double num3 = num2 - num;
                this.GetRoom().SendMessage(this.GetRoom().GetRoomItemHandler().UpdateUserOnRoller(this, this.Coordinate, 0, this.Z + num3));
            }
        }

        internal void RemoveStatus(string Key)
        {
            if (this.Statusses.ContainsKey(Key))
            {
                this.Statusses.Remove(Key);
            }
        }

        internal void Serialize(ServerMessage Message, bool gotPublicRoom)
        {
            if ((Message != null) && !this.IsSpectator)
            {
                if (this.IsBot)
                {
                    Message.AppendInt32(this.BotAI.BaseId);
                    Message.AppendString(this.BotData.Name);
                    Message.AppendString(this.BotData.Motto);
                    if (this.IsPet)
                    {
                        if (this.PetData.HaveSaddle == 2)
                        {
                            Message.AppendString(string.Concat(new object[] { this.BotData.Look.ToLower(), " 3 4 10 0 2 ", this.PetData.PetHair, " ", this.PetData.HairDye, " 3 ", this.PetData.PetHair, " ", this.PetData.HairDye }));
                        }
                        else if (this.PetData.HaveSaddle == 1)
                        {
                            Message.AppendString(string.Concat(new object[] { this.BotData.Look.ToLower(), " 3 2 ", this.PetData.PetHair, " ", this.PetData.HairDye, " 3 ", this.PetData.PetHair, " ", this.PetData.HairDye, " 4 9 0" }));
                        }

                        else
                        {
                            Message.AppendString(string.Concat(new object[] { this.BotData.Look.ToLower(), " 2 2 ", this.PetData.PetHair, " ", this.PetData.HairDye, " 3 ", this.PetData.PetHair, " ", this.PetData.HairDye }));
                        }
                    }
                    else 
                    {
                        Message.AppendString(this.BotData.Look);
                    }
                    Message.AppendInt32(this.VirtualId);
                    Message.AppendInt32(this.X);
                    Message.AppendInt32(this.Y);
                    Message.AppendString(TextHandling.GetString(this.Z));
                    Message.AppendInt32(0);
                    if (this.BotData.OwnerId == 0)
                    {
                    Message.AppendInt32((this.BotData.AiType == AIType.Pet) ? 2 : 3);
                    }
                     else
                    {
                        Message.AppendInt32(4);
                    }
                    if (this.BotData.AiType == AIType.Pet)
                    {
                        Message.AppendInt32(this.PetData.Type);
                        Message.AppendInt32(this.PetData.OwnerId);
                        Message.AppendString(this.PetData.OwnerName);
                        Message.AppendInt32(1);
                        Message.AppendBoolean(this.PetData.HaveSaddle != 0);
                        Message.AppendBoolean(this.montandoBol);
                        Message.AppendInt32(0);
                        Message.AppendInt32(0);
                        Message.AppendString("");
                    }
                    else if (this.BotData.OwnerId != 0)
                    {
                        Message.AppendString("m");
                        Message.AppendInt32(BotData.OwnerId);
                        string Owner;
                        using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            dbClient.setQuery("SELECT username FROM users WHERE id = '" + BotData.OwnerId + "'");
                            Owner = dbClient.getString();
                        }
                        Message.AppendString(Owner);
                        Message.AppendInt32(4);
                        Message.AppendShort(1);
                        Message.AppendShort(2);
                        Message.AppendShort(5);
                        Message.AppendShort(4);
                    }
                }
                else if ((!this.IsBot && (this.GetClient() != null)) && (this.GetClient().GetHabbo() != null))
                {
                    Habbo habbo = this.GetClient().GetHabbo();
                    Message.AppendInt32(habbo.Id);
                    Message.AppendString(habbo.Username);
                    Message.AppendString(habbo.Motto);
                    Message.AppendString(habbo.Look);
                    Message.AppendInt32(this.VirtualId);
                    Message.AppendInt32(this.X);
                    Message.AppendInt32(this.Y);
                    Message.AppendString(TextHandling.GetString(this.Z));
                    Message.AppendInt32(this.RotBody);
                    Message.AppendInt32(1);
                    Message.AppendString(habbo.Gender.ToLower());
                    if (habbo.FavoriteGuild == 0)
                    {
                        Message.AppendInt32(-1);
                        Message.AppendInt32(-1);
                        Message.AppendInt32(0);
                    }
                    else
                    {
                        MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(habbo.FavoriteGuild);
                        Message.AppendInt32(habbo.FavoriteGuild);
                        Message.AppendInt32(2);
                        Message.AppendString(guild.Name);
                        Message.AppendString("");
                    }
                    Message.AppendInt32(habbo.AchievementPoints);
                }
            }
        }

        internal void SerializeStatus(ServerMessage Message)
        {
            if (!this.IsSpectator)
            {
                Message.AppendInt32(this.VirtualId);
                Message.AppendInt32(this.X);
                Message.AppendInt32(this.Y);
                Message.AppendString(TextHandling.GetString(this.Z));
                Message.AppendInt32(this.RotHead);
                Message.AppendInt32(this.RotBody);
                StringBuilder builder = new StringBuilder();
                builder.Append("/");
                foreach (KeyValuePair<string, string> pair in this.Statusses)
                {
                    builder.Append(pair.Key);
                    if (pair.Value != string.Empty)
                    {
                        builder.Append(" ");
                        builder.Append(pair.Value);
                    }
                    builder.Append("/");
                }
                builder.Append("/");
                Message.AppendString(builder.ToString());
            }
        }

        internal void SerializeStatus(ServerMessage Message, string Status)
        {
            if (!this.IsSpectator)
            {
                Message.AppendInt32(this.VirtualId);
                Message.AppendInt32(this.X);
                Message.AppendInt32(this.Y);
                Message.AppendString(TextHandling.GetString(this.Z));
                Message.AppendInt32(this.RotHead);
                Message.AppendInt32(this.RotBody);
                StringBuilder builder = new StringBuilder();
                Message.AppendString(Status);
            }
        }

        internal void SetPos(int pX, int pY, double pZ)
        {
            this.X = pX;
            this.Y = pY;
            this.Z = pZ;
            if (this.isFlying)
            {
                this.Z += 4.0 + (0.5 * Math.Sin(0.7 * this.flyk));
            }
        }

        internal void SetRot(int Rotation)
        {
            this.SetRot(Rotation, false);
        }

        internal void SetRot(int Rotation, bool HeadOnly)
        {
            if (!this.Statusses.ContainsKey("lay") && !this.IsWalking)
            {
                int num = this.RotBody - Rotation;
                this.RotHead = this.RotBody;
                if (this.Statusses.ContainsKey("sit") || HeadOnly)
                {
                    if ((this.RotBody == 2) || (this.RotBody == 4))
                    {
                        if (num > 0)
                        {
                            this.RotHead = this.RotBody - 1;
                        }
                        else if (num < 0)
                        {
                            this.RotHead = this.RotBody + 1;
                        }
                    }
                    else if ((this.RotBody == 0) || (this.RotBody == 6))
                    {
                        if (num > 0)
                        {
                            this.RotHead = this.RotBody - 1;
                        }
                        else if (num < 0)
                        {
                            this.RotHead = this.RotBody + 1;
                        }
                    }
                }
                else if ((num <= -2) || (num >= 2))
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
        }

        internal void Unidle()
        {
            this.IdleTime = 0;
            if (this.IsAsleep)
            {
                this.IsAsleep = false;
                ServerMessage message = new ServerMessage(Outgoing.IdleStatus);
                message.AppendInt32(this.VirtualId);
                message.AppendBoolean(false);
                this.GetRoom().SendMessage(message);
            }
        }

        internal void UnlockWalking()
        {
            this.AllowOverride = false;
            this.CanWalk = true;
        }

        internal Point Coordinate
        {
            get
            {
                return new Point(this.X, this.Y);
            }
        }

        internal int CurrentEffect
        {
            get
            {
                return this.GetClient().GetHabbo().GetAvatarEffectsInventoryComponent().CurrentEffect;
            }
        }

        internal bool IsBot
        {
            get
            {
                return (this.BotData != null);
            }
        }

        internal bool IsDancing
        {
            get
            {
                return (this.DanceId >= 1);
            }
        }

        internal bool IsPet
        {
            get
            {
                return (this.IsBot && this.BotData.IsPet);
            }
        }

        internal bool IsTrading
        {
            get
            {
                if (this.IsBot)
                {
                    return false;
                }
                return this.Statusses.ContainsKey("trd");
            }
        }

        internal bool NeedsAutokick
        {
            get
            {
                if (this.IsBot)
                {
                    return false;
                }
                if ((this.GetClient() == null) || (this.GetClient().GetHabbo() == null))
                {
                    return true;
                }
                if (this.GetClient().GetHabbo().Rank > 1)
                {
                    return false;
                }
                return (this.IdleTime >= 0x708);
            }
        }
    }
}

