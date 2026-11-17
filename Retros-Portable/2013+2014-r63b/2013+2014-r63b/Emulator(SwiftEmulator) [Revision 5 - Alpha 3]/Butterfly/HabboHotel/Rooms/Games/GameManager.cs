namespace Butterfly.HabboHotel.Rooms.Games
{
    using Butterfly.Collections;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using System;
    using System.Drawing;
    using System.Threading;

    internal class GameManager
    {
        private QueuedDictionary<uint, RoomItem> blueTeamItems = new QueuedDictionary<uint, RoomItem>();
        private QueuedDictionary<uint, RoomItem> greenTeamItems = new QueuedDictionary<uint, RoomItem>();
        private QueuedDictionary<uint, RoomItem> redTeamItems = new QueuedDictionary<uint, RoomItem>();
        private Room room;
        internal int[] TeamPoints = new int[5];
        private QueuedDictionary<uint, RoomItem> yellowTeamItems = new QueuedDictionary<uint, RoomItem>();

        internal event RoomEventDelegate OnGameEnd;

        internal event RoomEventDelegate OnGameStart;

        internal event TeamScoreChangedDelegate OnScoreChanged;

        public GameManager(Room room)
        {
            this.room = room;
        }

        internal void AddFurnitureToTeam(RoomItem item, Team team)
        {
            switch (team)
            {
                case Team.red:
                    this.redTeamItems.Add(item.Id, item);
                    break;

                case Team.green:
                    this.greenTeamItems.Add(item.Id, item);
                    break;

                case Team.blue:
                    this.blueTeamItems.Add(item.Id, item);
                    break;

                case Team.yellow:
                    this.yellowTeamItems.Add(item.Id, item);
                    break;
            }
        }

        internal void AddPointToTeam(Team team, RoomUser user)
        {
            this.AddPointToTeam(team, 1, user);
        }

        internal void AddPointToTeam(Team team, int points, RoomUser user)
        {
            int num = this.TeamPoints[(int) team] += points;
            if (num < 0)
            {
                num = 0;
            }
            this.TeamPoints[(int) team] = num;
            if (this.OnScoreChanged != null)
            {
                this.OnScoreChanged(null, new TeamScoreChangedArgs(num, team, user));
            }
            foreach (RoomItem item in this.GetFurniItems(team).Values)
            {
                if (!isSoccerGoal(item.GetBaseItem().InteractionType))
                {
                    item.ExtraData = this.TeamPoints[(int) team].ToString();
                    item.UpdateState();
                }
            }
        }

        internal void Destroy()
        {
            Array.Clear(this.TeamPoints, 0, this.TeamPoints.Length);
            this.redTeamItems.Destroy();
            this.blueTeamItems.Destroy();
            this.greenTeamItems.Destroy();
            this.yellowTeamItems.Destroy();
            this.TeamPoints = null;
            this.OnScoreChanged = null;
            this.OnGameStart = null;
            this.OnGameEnd = null;
            this.redTeamItems = null;
            this.blueTeamItems = null;
            this.greenTeamItems = null;
            this.yellowTeamItems = null;
            this.room = null;
        }

        internal RoomItem GetFirstScoreBoard(Team team)
        {
            switch (team)
            {
                case Team.red:
                    foreach (RoomItem item in this.redTeamItems.Values)
                    {
                        if (item.GetBaseItem().InteractionType == InteractionType.freezeredcounter)
                        {
                            return item;
                        }
                    }
                    break;

                case Team.green:
                    foreach (RoomItem item in this.greenTeamItems.Values)
                    {
                        if (item.GetBaseItem().InteractionType == InteractionType.freezegreencounter)
                        {
                            return item;
                        }
                    }
                    break;

                case Team.blue:
                    foreach (RoomItem item in this.blueTeamItems.Values)
                    {
                        if (item.GetBaseItem().InteractionType == InteractionType.freezebluecounter)
                        {
                            return item;
                        }
                    }
                    break;

                case Team.yellow:
                    foreach (RoomItem item in this.yellowTeamItems.Values)
                    {
                        if (item.GetBaseItem().InteractionType == InteractionType.freezeyellowcounter)
                        {
                            return item;
                        }
                    }
                    break;
            }
            return null;
        }

        private QueuedDictionary<uint, RoomItem> GetFurniItems(Team team)
        {
            switch (team)
            {
                case Team.red:
                    return this.redTeamItems;

                case Team.green:
                    return this.greenTeamItems;

                case Team.blue:
                    return this.blueTeamItems;

                case Team.yellow:
                    return this.yellowTeamItems;
            }
            return new QueuedDictionary<uint, RoomItem>();
        }

        internal QueuedDictionary<uint, RoomItem> GetItems(Team team)
        {
            switch (team)
            {
                case Team.red:
                    return this.redTeamItems;

                case Team.green:
                    return this.greenTeamItems;

                case Team.blue:
                    return this.blueTeamItems;

                case Team.yellow:
                    return this.yellowTeamItems;
            }
            return new QueuedDictionary<uint, RoomItem>();
        }

        internal Room GetRoom()
        {
            return this.room;
        }

        private int GetScoreForTeam(Team team)
        {
            return this.TeamPoints[(int) team];
        }

        internal Team getWinningTeam()
        {
            int num = 1;
            int num2 = 0;
            for (int i = 1; i < 5; i++)
            {
                if (this.TeamPoints[i] > num2)
                {
                    num2 = this.TeamPoints[i];
                    num = i;
                }
            }
            return (Team) num;
        }

        private static bool isSoccerGoal(InteractionType type)
        {
            return ((((type == InteractionType.footballgoalblue) || (type == InteractionType.footballgoalgreen)) || (type == InteractionType.footballgoalred)) || (type == InteractionType.footballgoalyellow));
        }

        private void LockGate(RoomItem item)
        {
            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.freezebluegate:
                case InteractionType.freezegreengate:
                case InteractionType.freezeredgate:
                case InteractionType.freezeyellowgate:
                    foreach (RoomUser user in this.room.GetGameMap().GetRoomUsers(new Point(item.GetX, item.GetY)))
                    {
                        user.SqState = 0;
                    }
                    this.room.GetGameMap().GameMap[item.GetX, item.GetY] = 0;
                    break;
            }
        }

        internal void LockGates()
        {
            foreach (RoomItem item in this.redTeamItems.Values)
            {
                this.LockGate(item);
            }
            foreach (RoomItem item in this.greenTeamItems.Values)
            {
                this.LockGate(item);
            }
            foreach (RoomItem item in this.blueTeamItems.Values)
            {
                this.LockGate(item);
            }
            foreach (RoomItem item in this.yellowTeamItems.Values)
            {
                this.LockGate(item);
            }
        }

        internal void OnCycle()
        {
            this.redTeamItems.OnCycle();
            this.blueTeamItems.OnCycle();
            this.greenTeamItems.OnCycle();
            this.yellowTeamItems.OnCycle();
        }

        internal void RemoveFurnitureFromTeam(RoomItem item, Team team)
        {
            switch (team)
            {
                case Team.red:
                    this.redTeamItems.Remove(item.Id);
                    break;

                case Team.green:
                    this.greenTeamItems.Remove(item.Id);
                    break;

                case Team.blue:
                    this.blueTeamItems.Remove(item.Id);
                    break;

                case Team.yellow:
                    this.yellowTeamItems.Remove(item.Id);
                    break;
            }
        }

        internal void Reset()
        {
            this.AddPointToTeam(Team.blue, this.GetScoreForTeam(Team.blue) * -1, null);
            this.AddPointToTeam(Team.green, this.GetScoreForTeam(Team.green) * -1, null);
            this.AddPointToTeam(Team.red, this.GetScoreForTeam(Team.red) * -1, null);
            this.AddPointToTeam(Team.yellow, this.GetScoreForTeam(Team.yellow) * -1, null);
        }

        internal void StartGame()
        {
            if (this.OnGameStart != null)
            {
                this.OnGameStart(null, null);
            }
        }

        internal void StopGame()
        {
            if (this.OnGameEnd != null)
            {
                this.OnGameEnd(null, null);
            }
            this.room.lastTimerReset = DateTime.Now;
        }

        private void UnlockGate(RoomItem item)
        {
            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.freezebluegate:
                case InteractionType.freezegreengate:
                case InteractionType.freezeredgate:
                case InteractionType.freezeyellowgate:
                    foreach (RoomUser user in this.room.GetGameMap().GetRoomUsers(new Point(item.GetX, item.GetY)))
                    {
                        user.SqState = 1;
                    }
                    this.room.GetGameMap().GameMap[item.GetX, item.GetY] = 1;
                    break;
            }
        }

        internal void UnlockGates()
        {
            foreach (RoomItem item in this.redTeamItems.Values)
            {
                this.UnlockGate(item);
            }
            foreach (RoomItem item in this.greenTeamItems.Values)
            {
                this.UnlockGate(item);
            }
            foreach (RoomItem item in this.blueTeamItems.Values)
            {
                this.UnlockGate(item);
            }
            foreach (RoomItem item in this.yellowTeamItems.Values)
            {
                this.UnlockGate(item);
            }
        }

        internal int[] Points
        {
            get
            {
                return this.TeamPoints;
            }
            set
            {
                this.TeamPoints = value;
            }
        }
    }
}

