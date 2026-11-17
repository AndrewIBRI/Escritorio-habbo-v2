namespace Butterfly.HabboHotel.Pets
{
    using Butterfly;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.Messages;
    using HabboEvents;
    using System;

    internal class Pet
    {
        internal bool CanMountAllPeople;
        internal string Color;
        internal double CreationStamp;
        internal DatabaseUpdateState DBState;
        internal int Energy;
        internal int[] experienceLevels = new int[] { 
            100, 200, 400, 600, 0x3e8, 0x514, 0x708, 0x960, 0xc80, 0x10cc, 0x1c20, 0x2134, 0x2774, 0x33f4, 0x445c, 0x59d8, 
            0xcabc
         };
        internal int Expirience;
        internal int HairDye;
        internal int HaveSaddle;
        internal string Name;
        internal int Nutrition;
        internal uint OwnerId;
        internal int PetHair;
        internal uint PetId;
        internal bool PlacedInRoom;
        internal string Race;
        internal int Respect;
        internal uint RoomId;
        internal uint Type;
        internal int VirtualId;
        internal int X;
        internal int Y;
        internal double Z;

        internal Pet(uint PetId, uint OwnerId, uint RoomId, string Name, uint Type, string Race, string Color, int Expirience, int Energy, int Nutrition, int Respect, double CreationStamp, int X, int Y, double Z, int havesaddle, int hairdye, int PetHair, bool CanMountAllPeople)
        {
            this.PetId = PetId;
            this.OwnerId = OwnerId;
            this.RoomId = RoomId;
            this.Name = Name;
            this.Type = Type;
            this.Race = Race;
            this.Color = Color;
            this.Expirience = Expirience;
            this.Energy = Energy;
            this.Nutrition = Nutrition;
            this.Respect = Respect;
            this.CreationStamp = CreationStamp;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.PlacedInRoom = false;
            this.DBState = DatabaseUpdateState.Updated;
            this.HaveSaddle = havesaddle;
            this.HairDye = hairdye;
            this.PetHair = PetHair;
            this.CanMountAllPeople = CanMountAllPeople;
        }

        internal void AddExpirience(int Amount)
        {
            this.Expirience += Amount;
            if (this.Expirience < 0xcabc)
            {
                if (this.DBState != DatabaseUpdateState.NeedsInsert)
                {
                    this.DBState = DatabaseUpdateState.NeedsUpdate;
                }
                if (this.Room != null)
                {
                    ServerMessage message = new ServerMessage(Outgoing.AddExperience);
                    message.AppendInt32(this.PetId);
                    message.AppendInt32(this.VirtualId);
                    message.AppendInt32(Amount);
                    this.Room.SendMessage(message);
                    if (this.Expirience > this.ExpirienceGoal)
                    {
                        ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(this.OwnerId), "ACH_PetLevelUp", 1);
                        ServerMessage message2 = new ServerMessage(Outgoing.Talk);
                        message2.AppendInt32(this.VirtualId);
                        message2.AppendString("*leveled up to level " + this.Level + " *");
                        message2.AppendInt32(0);
                        message2.AppendInt32(0);
                        message2.AppendInt32(0);
                        message2.AppendInt32(-1);
                        this.Room.SendMessage(message2);
                    }
                }
            }
        }

        internal void OnRespect()
        {
            this.Respect++;
            ServerMessage message = new ServerMessage(Outgoing.RespectPet);
            message.AppendInt32(this.VirtualId);
            message.AppendInt32(0);
            this.Room.SendMessage(message);
            if (this.DBState != DatabaseUpdateState.NeedsInsert)
            {
                this.DBState = DatabaseUpdateState.NeedsUpdate;
            }
            if (this.Expirience <= 0xcabc)
            {
                this.AddExpirience(10);
            }
        }

        internal void PetEnergy(bool Add)
        {
            int num;
            if (Add)
            {
                if (this.Energy == 100)
                {
                    return;
                }
                if (this.Energy > 0x55)
                {
                    num = MaxEnergy - this.Energy;
                }
                else
                {
                    num = 10;
                }
            }
            else
            {
                num = 15;
            }
            int randomNumber = ButterflyEnvironment.GetRandomNumber(4, num);
            if (!Add)
            {
                this.Energy -= randomNumber;
                if (this.Energy < 0)
                {
                    this.Energy = 1;
                    randomNumber = 1;
                }
            }
            else
            {
                this.Energy += randomNumber;
            }
            if (this.DBState != DatabaseUpdateState.NeedsInsert)
            {
                this.DBState = DatabaseUpdateState.NeedsUpdate;
            }
        }

        internal ServerMessage SerializeInfo()
        {
            ServerMessage message = new ServerMessage(Outgoing.PetInformation);
            message.AppendInt32(this.PetId);
            message.AppendString(this.Name);
            message.AppendInt32(this.Level);
            message.AppendInt32(MaxLevel);
            message.AppendInt32(this.Expirience);
            message.AppendInt32(this.ExpirienceGoal);
            message.AppendInt32(this.Energy);
            message.AppendInt32(MaxEnergy);
            message.AppendInt32(this.Nutrition);
            message.AppendInt32(MaxNutrition);
            message.AppendInt32(this.Respect);
            message.AppendInt32(this.OwnerId);
            message.AppendInt32(this.Age);
            message.AppendString(this.OwnerName);
            message.AppendInt32(1);
            message.AppendBoolean(this.HaveSaddle != 0);
            message.AppendBoolean(ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.RoomId).GetRoomUserManager().GetRoomUserByVirtualId(this.VirtualId).montandoBol);
            message.AppendInt32(0);
            if(this.CanMountAllPeople)
                message.AppendInt32(1);
            else
                message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendString("");
            message.AppendBoolean(false);
            message.AppendBoolean(this.CanMountAllPeople);
            message.AppendInt32(0);
            message.AppendString("");
            message.AppendBoolean(false);
            message.AppendInt32(-1);
            message.AppendInt32(-1);
            message.AppendInt32(-1);
            message.AppendBoolean(false);
            return message;
        }

        internal void SerializeInventory(ServerMessage Message)
        {
            Message.AppendInt32(this.PetId);
            Message.AppendString(this.Name);
            Message.AppendInt32(this.Type);
            Message.AppendInt32(int.Parse(this.Race));
            Message.AppendString(this.Color);
            Message.AppendInt32(0);
            Message.AppendInt32(0);
            Message.AppendInt32(0);
        }

        internal void TalkAbout(string Message)
        {
            ServerMessage message = new ServerMessage(Outgoing.Talk);
            message.AppendInt32(this.VirtualId);
            message.AppendString(Message);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendInt32(-1);
            this.Room.SendMessage(message);
        }

        internal int Age
        {
            get
            {
                return (int) Math.Floor((double) ((ButterflyEnvironment.GetUnixTimestamp() - this.CreationStamp) / 86400.0));
            }
        }

        internal int ExpirienceGoal
        {
            get
            {
                return this.experienceLevels[this.Level - 1];
            }
        }

        internal bool IsInRoom
        {
            get
            {
                return (this.RoomId > 0);
            }
        }

        internal int Level
        {
            get
            {
                for (int i = 0; i < this.experienceLevels.Length; i++)
                {
                    if (this.Expirience < this.experienceLevels[i])
                    {
                        return (i + 1);
                    }
                }
                return (this.experienceLevels.Length + 1);
            }
        }

        internal string Look
        {
            get
            {
                return string.Concat(new object[] { this.Type, " ", this.Race, " ", this.Color });
            }
        }

        internal static int MaxEnergy
        {
            get
            {
                return 100;
            }
        }

        internal static int MaxLevel
        {
            get
            {
                return 20;
            }
        }

        internal static int MaxNutrition
        {
            get
            {
                return 150;
            }
        }

        internal string OwnerName
        {
            get
            {
                return ButterflyEnvironment.GetGame().GetClientManager().GetNameById(this.OwnerId);
            }
        }

        internal Butterfly.HabboHotel.Rooms.Room Room
        {
            get
            {
                if (!this.IsInRoom)
                {
                    return null;
                }
                return ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.RoomId);
            }
        }
    }
}

