namespace Butterfly.HabboHotel.RoomBots
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Pathfinding;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.Rooms;
    using System;
    using System.Drawing;

    internal class PetBot : BotAI
    {
        private int ActionTimer;
        private int EnergyTimer;
        private int SpeechTimer;

        internal PetBot(int VirtualId)
        {
            this.SpeechTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 60);
            this.ActionTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 30 + VirtualId);
            this.EnergyTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 60);
        }

        internal override void OnSelfEnterRoom()
        {
            Point point = base.GetRoom().GetGameMap().getRandomWalkableSquare();
            if (base.GetRoomUser() != null)
            {
                base.GetRoomUser().MoveTo(point.X, point.Y);
            }
        }

        internal override void OnSelfLeaveRoom(bool Kicked)
        {
        }

        internal override void OnTimerTick()
        {
            if (this.SpeechTimer <= 0)
            {
                RoomUser roomUser = base.GetRoomUser();
                if (roomUser.PetData.DBState != DatabaseUpdateState.NeedsInsert)
                {
                    roomUser.PetData.DBState = DatabaseUpdateState.NeedsUpdate;
                }
                if (roomUser != null)
                {
                    Random random = new Random();
                    this.RemovePetStatus();
                    string[] strArray = PetLocale.GetValue("speech.pet" + roomUser.PetData.Type);
                    string message = strArray[random.Next(0, strArray.Length - 1)];
                    if (message.Length != 3)
                    {
                        roomUser.Chat(null, message, false, 0);
                    }
                    else
                    {
                        roomUser.Statusses.Add(message, TextHandling.GetString(roomUser.Z));
                    }
                }
                this.SpeechTimer = ButterflyEnvironment.GetRandomNumber(20, 120);
            }
            else
            {
                this.SpeechTimer--;
            }
            if (this.ActionTimer <= 0)
            {
                try
                {
                    this.RemovePetStatus();
                    this.ActionTimer = ButterflyEnvironment.GetRandomNumber(15, 40 + base.GetRoomUser().PetData.VirtualId);
                    if (!base.GetRoomUser().montandoBol)
                    {
                        this.RemovePetStatus();
                        Point point = base.GetRoom().GetGameMap().getRandomWalkableSquare();
                        base.GetRoomUser().MoveTo(point.X, point.Y);
                    }
                }
                catch (Exception exception)
                {
                    Logging.HandleException(exception, "PetBot.OnTimerTick");
                }
            }
            else
            {
                this.ActionTimer--;
            }
            if (this.EnergyTimer <= 0)
            {
                this.RemovePetStatus();
                base.GetRoomUser().PetData.PetEnergy(true);
                this.EnergyTimer = ButterflyEnvironment.GetRandomNumber(30, 120);
            }
            else
            {
                this.EnergyTimer--;
            }
        }

        internal override void OnUserEnterRoom(RoomUser User)
        {
            if (((User.GetClient() != null) && (User.GetClient().GetHabbo() != null)))
            {
                base.GetRoomUser().Chat(null, "I have missed you!!!", true, 0);
            }
        }

        internal override void OnUserLeaveRoom(GameClient Client)
        {
        }

        internal override void OnUserSay(RoomUser User, string Message)
        {
            int x;
            int y;
            string[] strArray;
            Random random;
            RoomUser roomUser = base.GetRoomUser();
            if (roomUser.PetData.DBState != DatabaseUpdateState.NeedsInsert)
            {
                roomUser.PetData.DBState = DatabaseUpdateState.NeedsUpdate;
            }
            if (Message.ToLower().Equals(roomUser.PetData.Name.ToLower()))
            {
                roomUser.SetRot(Rotation.Calculate(roomUser.X, roomUser.Y, User.X, User.Y), false);
                return;
            }
            if ((!Message.ToLower().StartsWith(roomUser.PetData.Name.ToLower() + " ") || (User.GetClient().GetHabbo().Username.ToLower() != base.GetRoomUser().PetData.OwnerName.ToLower())) && (!Message.ToLower().StartsWith(roomUser.PetData.Name.ToLower() + " ") || (PetCommandHandeler.TryInvoke(Message.Substring(roomUser.PetData.Name.ToLower().Length + 1)) != 8)))
            {
                return;
            }
            string input = Message.Substring(roomUser.PetData.Name.ToLower().Length + 1);
            int randomNumber = ButterflyEnvironment.GetRandomNumber(1, 8);
            if ((((roomUser.PetData.Energy <= 10) || (randomNumber >= 6)) && (roomUser.PetData.Level <= 15)) && (PetCommandHandeler.TryInvoke(input) != 8))
            {
                this.RemovePetStatus();
                if (roomUser.PetData.Energy < 10)
                {
                    strArray = PetLocale.GetValue("pet.tired");
                    random = new Random();
                    roomUser.Chat(null, strArray[random.Next(0, strArray.Length - 1)], false, 0);
                    roomUser.Statusses.Add("lay", TextHandling.GetString(roomUser.Z));
                    this.SpeechTimer = 50;
                    this.ActionTimer = 0x2d;
                    this.EnergyTimer = 5;
                }
                else
                {
                    strArray = PetLocale.GetValue("pet.lazy");
                    random = new Random();
                    roomUser.Chat(null, strArray[random.Next(0, strArray.Length - 1)], false, 0);
                    roomUser.PetData.PetEnergy(false);
                }
                return;
            }
            this.RemovePetStatus();
            switch (PetCommandHandeler.TryInvoke(input))
            {
                case 1:
                {
                    this.RemovePetStatus();
                    Point point = base.GetRoom().GetGameMap().getRandomWalkableSquare();
                    roomUser.MoveTo(point.X, point.Y);
                    roomUser.PetData.AddExpirience(10);
                    goto Label_071A;
                }
                case 4:
                    this.RemovePetStatus();
                    roomUser.Statusses.Add("wag", TextHandling.GetString(roomUser.Z));
                    roomUser.PetData.AddExpirience(10);
                    this.ActionTimer = 30;
                    this.EnergyTimer = 5;
                    goto Label_071A;

                case 5:
                    this.RemovePetStatus();
                    roomUser.Statusses.Add("lay", TextHandling.GetString(roomUser.Z));
                    roomUser.PetData.AddExpirience(10);
                    this.ActionTimer = 30;
                    this.EnergyTimer = 5;
                    goto Label_071A;

                case 6:
                    this.RemovePetStatus();
                    x = User.X;
                    y = User.Y;
                    this.ActionTimer = 30;
                    if (User.RotBody != 4)
                    {
                        if (User.RotBody == 0)
                        {
                            y = User.Y - 1;
                        }
                        else if (User.RotBody == 6)
                        {
                            x = User.X - 1;
                        }
                        else if (User.RotBody == 2)
                        {
                            x = User.X + 1;
                        }
                        else if (User.RotBody == 3)
                        {
                            x = User.X + 1;
                            y = User.Y + 1;
                        }
                        else if (User.RotBody == 1)
                        {
                            x = User.X + 1;
                            y = User.Y - 1;
                        }
                        else if (User.RotBody == 7)
                        {
                            x = User.X - 1;
                            y = User.Y - 1;
                        }
                        else if (User.RotBody == 5)
                        {
                            x = User.X - 1;
                            y = User.Y + 1;
                        }
                        break;
                    }
                    y = User.Y + 1;
                    break;

                case 7:
                    this.RemovePetStatus();
                    this.ActionTimer = 30;
                    roomUser.PetData.AddExpirience(10);
                    roomUser.IsWalking = false;
                    goto Label_071A;

                case 8:
                    this.RemovePetStatus();
                    x = User.X;
                    y = User.Y;
                    this.ActionTimer = 30;
                    if (User.RotBody != 4)
                    {
                        if (User.RotBody == 0)
                        {
                            y = User.Y - 1;
                        }
                        else if (User.RotBody == 6)
                        {
                            x = User.X - 1;
                        }
                        else if (User.RotBody == 2)
                        {
                            x = User.X + 1;
                        }
                        else if (User.RotBody == 3)
                        {
                            x = User.X + 1;
                            y = User.Y + 1;
                        }
                        else if (User.RotBody == 1)
                        {
                            x = User.X + 1;
                            y = User.Y - 1;
                        }
                        else if (User.RotBody == 7)
                        {
                            x = User.X - 1;
                            y = User.Y - 1;
                        }
                        else if (User.RotBody == 5)
                        {
                            x = User.X - 1;
                            y = User.Y + 1;
                        }
                    }
                    else
                    {
                        y = User.Y + 1;
                    }
                    roomUser.PetData.AddExpirience(10);
                    roomUser.MoveTo(x, y);
                    goto Label_071A;

                case 9:
                    this.RemovePetStatus();
                    roomUser.Chat(null, "Hola " + roomUser.PetData.OwnerName + ", \x00bfQu\x00e9 tal?", false, 0);
                    roomUser.PetData.AddExpirience(10);
                    this.ActionTimer = 30;
                    this.EnergyTimer = 5;
                    goto Label_071A;

                case 10:
                    this.RemovePetStatus();
                    roomUser.PetData.AddExpirience(10);
                    this.ActionTimer = 30;
                    this.EnergyTimer = 5;
                    goto Label_071A;

                case 12:
                    this.RemovePetStatus();
                    roomUser.PetData.AddExpirience(10);
                    roomUser.MoveTo(roomUser.X - 3, roomUser.Y);
                    this.ActionTimer = 30;
                    this.EnergyTimer = 5;
                    goto Label_071A;

                case 13:
                    this.RemovePetStatus();
                    roomUser.PetData.AddExpirience(10);
                    roomUser.MoveTo(roomUser.X + 3, roomUser.Y);
                    this.ActionTimer = 30;
                    this.EnergyTimer = 5;
                    goto Label_071A;

                case 14:
                    this.RemovePetStatus();
                    roomUser.PetData.AddExpirience(10);
                    roomUser.MoveTo(roomUser.X, roomUser.Y - 3);
                    this.ActionTimer = 30;
                    this.EnergyTimer = 5;
                    goto Label_071A;

                default:
                    strArray = PetLocale.GetValue("pet.unknowncommand");
                    random = new Random();
                    roomUser.Chat(null, strArray[random.Next(0, strArray.Length - 1)], false, 0);
                    goto Label_071A;
            }
            roomUser.PetData.AddExpirience(10);
            roomUser.MoveTo(x, y);
        Label_071A:
            roomUser.PetData.PetEnergy(false);
            roomUser.PetData.PetEnergy(false);
        }

        internal override void OnUserShout(RoomUser User, string Message)
        {
        }

        private void RemovePetStatus()
        {
            RoomUser roomUser = base.GetRoomUser();
            roomUser.Statusses.Remove("sit");
            roomUser.Statusses.Remove("lay");
            roomUser.Statusses.Remove("snf");
            roomUser.Statusses.Remove("eat");
            roomUser.Statusses.Remove("ded");
            roomUser.Statusses.Remove("jmp");
        }

        internal void test()
        {
        }
    }
}

