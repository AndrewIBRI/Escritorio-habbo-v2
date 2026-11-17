namespace Butterfly.HabboHotel.Games.SnowStorm
{
    using Butterfly;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.Messages;
    using HabboEvents;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class SnowStorm
    {
        internal int Kvalue = 0;
        internal int MaxUsers;
        internal SnowModel Model;
        internal int WarId;
        internal int WarLevel;
        internal Habbo WarOwner;
        internal int WarStarted;
        internal List<Habbo> WarUsers;

        internal SnowStorm(Habbo warowner)
        {
            WarsData stormWars = ButterflyEnvironment.GetGame().GetStormWars();
            stormWars.LastWarId++;
            this.WarId = ButterflyEnvironment.GetGame().GetStormWars().LastWarId;
            this.WarLevel = 9;
            this.WarOwner = warowner;
            this.WarUsers = new List<Habbo>();
            this.WarStarted = 0;
            this.MaxUsers = 10;
            this.Model = ButterflyEnvironment.GetGame().GetStormWars().RoomModel[this.WarLevel];
            this.Kvalue = 0;
            ButterflyEnvironment.GetGame().GetStormWars().Wars.Add(this.WarId, this);
        }

        internal void AssignTeams()
        {
            for (int i = 0; i < this.WarUsers.Count; i++)
            {
                Habbo habbo = this.WarUsers[i];
                habbo.SnowStep = 2;
                switch (i)
                {
                    case 0:
                    case 2:
                    case 4:
                    case 6:
                    case 8:
                        if ((this.WarLevel == 9) && (i == 0))
                        {
                            habbo.SnowUserId = 20;
                            habbo.SnowX = 0x3e80;
                            habbo.SnowY = 0x11f80;
                        }
                        else if ((this.WarLevel == 9) && (i == 2))
                        {
                            habbo.SnowUserId = 0x15;
                            habbo.SnowX = 0x4b00;
                            habbo.SnowY = 0x15e0a;
                        }
                        habbo.SnowTeam = 1;
                        break;

                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 9:
                        habbo.SnowTeam = 2;
                        break;
                }
            }
        }

        internal void ProcessWar()
        {
            new ServerMessage(Outgoing.Game2GameStatusMessageEvent).AppendInt32(this.Kvalue);
            this.Kvalue++;
        }

        internal void SendToStorm(ServerMessage Packet, bool ToMe, uint UserId = 0)
        {
            foreach (Habbo habbo in this.WarUsers)
            {
                //if ((!ToMe) || (habbo.Id != UserId))
                //{
                    //Console.WriteLine(habbo.Username + " - " + Packet.ToString());
                    habbo.GetClient().SendMessage(Packet);
                //}
            }
        }

        internal void SnowStormStart()
        {
            if (this.WarStarted <= 1)
            {
                this.WarStarted = 2;
            }
        }
    }
}

