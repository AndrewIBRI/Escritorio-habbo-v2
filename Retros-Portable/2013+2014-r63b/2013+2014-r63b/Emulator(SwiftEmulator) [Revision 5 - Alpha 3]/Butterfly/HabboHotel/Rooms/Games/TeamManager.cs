namespace Butterfly.HabboHotel.Rooms.Games
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using System;
    using System.Collections.Generic;

    public class TeamManager
    {
        public List<RoomUser> BlueTeam;
        public string Game;
        public List<RoomUser> GreenTeam;
        public List<RoomUser> RedTeam;
        public List<RoomUser> YellowTeam;

        public void AddUser(RoomUser user)
        {
            if (user.team.Equals(Team.blue))
            {
                this.BlueTeam.Add(user);
            }
            else if (user.team.Equals(Team.red))
            {
                this.RedTeam.Add(user);
            }
            else if (user.team.Equals(Team.yellow))
            {
                this.YellowTeam.Add(user);
            }
            else if (user.team.Equals(Team.green))
            {
                this.GreenTeam.Add(user);
            }
            string str = this.Game.ToLower();
            if (str != null)
            {
                Room currentRoom;
                if (!(str == "banzai"))
                {
                    if (str == "freeze")
                    {
                        currentRoom = user.GetClient().GetHabbo().CurrentRoom;
                        foreach (RoomItem item in currentRoom.GetRoomItemHandler().mFloorItems.Values)
                        {
                            if (item.GetBaseItem().InteractionType.Equals(InteractionType.freezebluegate))
                            {
                                item.ExtraData = this.BlueTeam.Count.ToString();
                                item.UpdateState();
                            }
                            else if (item.GetBaseItem().InteractionType.Equals(InteractionType.freezeredgate))
                            {
                                item.ExtraData = this.RedTeam.Count.ToString();
                                item.UpdateState();
                            }
                            else if (item.GetBaseItem().InteractionType.Equals(InteractionType.freezegreengate))
                            {
                                item.ExtraData = this.GreenTeam.Count.ToString();
                                item.UpdateState();
                            }
                            else if (item.GetBaseItem().InteractionType.Equals(InteractionType.freezeyellowgate))
                            {
                                item.ExtraData = this.YellowTeam.Count.ToString();
                                item.UpdateState();
                            }
                        }
                    }
                }
                else
                {
                    currentRoom = user.GetClient().GetHabbo().CurrentRoom;
                    foreach (RoomItem item in currentRoom.GetRoomItemHandler().mFloorItems.Values)
                    {
                        if (item.GetBaseItem().InteractionType.Equals(InteractionType.banzaigateblue))
                        {
                            item.ExtraData = this.BlueTeam.Count.ToString();
                            item.UpdateState();
                        }
                        else if (item.GetBaseItem().InteractionType.Equals(InteractionType.banzaigatered))
                        {
                            item.ExtraData = this.RedTeam.Count.ToString();
                            item.UpdateState();
                        }
                        else if (item.GetBaseItem().InteractionType.Equals(InteractionType.banzaigategreen))
                        {
                            item.ExtraData = this.GreenTeam.Count.ToString();
                            item.UpdateState();
                        }
                        else if (item.GetBaseItem().InteractionType.Equals(InteractionType.banzaigateyellow))
                        {
                            item.ExtraData = this.YellowTeam.Count.ToString();
                            item.UpdateState();
                        }
                    }
                }
            }
        }

        public bool CanEnterOnTeam(Team t)
        {
            if (t.Equals(Team.blue))
            {
                return (this.BlueTeam.Count < 5);
            }
            if (t.Equals(Team.red))
            {
                return (this.RedTeam.Count < 5);
            }
            if (t.Equals(Team.yellow))
            {
                return (this.YellowTeam.Count < 5);
            }
            return (t.Equals(Team.green) && (this.GreenTeam.Count < 5));
        }

        public static TeamManager createTeamforGame(string Game)
        {
            return new TeamManager { Game = Game, BlueTeam = new List<RoomUser>(), RedTeam = new List<RoomUser>(), GreenTeam = new List<RoomUser>(), YellowTeam = new List<RoomUser>() };
        }

        public void OnUserLeave(RoomUser user)
        {
            if (user.team.Equals(Team.blue))
            {
                this.BlueTeam.Remove(user);
            }
            else if (user.team.Equals(Team.red))
            {
                this.RedTeam.Remove(user);
            }
            else if (user.team.Equals(Team.yellow))
            {
                this.YellowTeam.Remove(user);
            }
            else if (user.team.Equals(Team.green))
            {
                this.GreenTeam.Remove(user);
            }
            string str = this.Game.ToLower();
            if (str != null)
            {
                Room currentRoom;
                if (!(str == "banzai"))
                {
                    if (str == "freeze")
                    {
                        currentRoom = user.GetClient().GetHabbo().CurrentRoom;
                        foreach (RoomItem item in currentRoom.GetRoomItemHandler().mFloorItems.Values)
                        {
                            if (item.GetBaseItem().InteractionType.Equals(InteractionType.freezebluegate))
                            {
                                item.ExtraData = this.BlueTeam.Count.ToString();
                                item.UpdateState();
                            }
                            else if (item.GetBaseItem().InteractionType.Equals(InteractionType.freezeredgate))
                            {
                                item.ExtraData = this.RedTeam.Count.ToString();
                                item.UpdateState();
                            }
                            else if (item.GetBaseItem().InteractionType.Equals(InteractionType.freezegreengate))
                            {
                                item.ExtraData = this.GreenTeam.Count.ToString();
                                item.UpdateState();
                            }
                            else if (item.GetBaseItem().InteractionType.Equals(InteractionType.freezeyellowgate))
                            {
                                item.ExtraData = this.YellowTeam.Count.ToString();
                                item.UpdateState();
                            }
                        }
                    }
                }
                else
                {
                    currentRoom = user.GetClient().GetHabbo().CurrentRoom;
                    foreach (RoomItem item in currentRoom.GetRoomItemHandler().mFloorItems.Values)
                    {
                        if (item.GetBaseItem().InteractionType.Equals(InteractionType.banzaigateblue))
                        {
                            item.ExtraData = this.BlueTeam.Count.ToString();
                            item.UpdateState();
                        }
                        else if (item.GetBaseItem().InteractionType.Equals(InteractionType.banzaigatered))
                        {
                            item.ExtraData = this.RedTeam.Count.ToString();
                            item.UpdateState();
                        }
                        else if (item.GetBaseItem().InteractionType.Equals(InteractionType.banzaigategreen))
                        {
                            item.ExtraData = this.GreenTeam.Count.ToString();
                            item.UpdateState();
                        }
                        else if (item.GetBaseItem().InteractionType.Equals(InteractionType.banzaigateyellow))
                        {
                            item.ExtraData = this.YellowTeam.Count.ToString();
                            item.UpdateState();
                        }
                    }
                }
            }
        }
    }
}

