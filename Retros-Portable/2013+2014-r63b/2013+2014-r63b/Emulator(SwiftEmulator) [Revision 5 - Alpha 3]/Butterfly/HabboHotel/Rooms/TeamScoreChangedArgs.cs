namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly.HabboHotel.Rooms.Games;
    using System;

    public class TeamScoreChangedArgs : EventArgs
    {
        internal readonly int Points;
        internal readonly Butterfly.HabboHotel.Rooms.Games.Team Team;
        internal readonly RoomUser user;

        public TeamScoreChangedArgs(int points, Butterfly.HabboHotel.Rooms.Games.Team team, RoomUser user)
        {
            this.Points = points;
            this.Team = team;
            this.user = user;
        }
    }
}

