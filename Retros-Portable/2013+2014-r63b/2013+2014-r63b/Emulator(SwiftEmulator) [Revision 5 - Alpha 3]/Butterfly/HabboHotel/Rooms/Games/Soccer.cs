namespace Butterfly.HabboHotel.Rooms.Games
{
    using Butterfly.Collections;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Pathfinding;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.Messages;
    using HabboEvents;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    internal class Soccer
    {
        private QueuedDictionary<uint, RoomItem> balls;
        private RoomItem[] gates;
        private Room room;

        public Soccer(Room room)
        {
            this.room = room;
            this.gates = new RoomItem[4];
            this.balls = new QueuedDictionary<uint, RoomItem>();
        }

        internal void AddBall(RoomItem item)
        {
            this.balls.Add(item.Id, item);
        }

        internal void Destroy()
        {
            Array.Clear(this.gates, 0, this.gates.Length);
            this.gates = null;
            this.room = null;
            this.balls.Destroy();
            this.balls = null;
        }

        private bool GameItemOverlaps(RoomItem gameItem)
        {
            Point coordinate = gameItem.Coordinate;
            foreach (RoomItem item in this.GetFootballItemsForAllTeams())
            {
                foreach (ThreeDCoord coord in item.GetAffectedTiles.Values)
                {
                    if ((coord.X == coordinate.X) && (coord.Y == coordinate.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<RoomItem> GetFootballItemsForAllTeams()
        {
            List<RoomItem> list = new List<RoomItem>();
            foreach (RoomItem item in this.room.GetGameManager().GetItems(Team.red).Values)
            {
                list.Add(item);
            }
            foreach (RoomItem item in this.room.GetGameManager().GetItems(Team.green).Values)
            {
                list.Add(item);
            }
            foreach (RoomItem item in this.room.GetGameManager().GetItems(Team.blue).Values)
            {
                list.Add(item);
            }
            foreach (RoomItem item in this.room.GetGameManager().GetItems(Team.yellow).Values)
            {
                list.Add(item);
            }
            return list;
        }

        private void HandleFootballGameItems(Point ballItemCoord, RoomUser user)
        {
            if (user.team != Team.none)
            {
                foreach (RoomItem item in this.room.GetGameManager().GetItems(Team.red).Values)
                {
                    foreach (ThreeDCoord coord in item.GetAffectedTiles.Values)
                    {
                        if ((coord.X == ballItemCoord.X) && (coord.Y == ballItemCoord.Y))
                        {
                            this.room.GetGameManager().AddPointToTeam(user.team, user);
                            return;
                        }
                    }
                }
                foreach (RoomItem item in this.room.GetGameManager().GetItems(Team.green).Values)
                {
                    foreach (ThreeDCoord coord in item.GetAffectedTiles.Values)
                    {
                        if ((coord.X == ballItemCoord.X) && (coord.Y == ballItemCoord.Y))
                        {
                            this.room.GetGameManager().AddPointToTeam(user.team, user);
                            return;
                        }
                    }
                }
                foreach (RoomItem item in this.room.GetGameManager().GetItems(Team.blue).Values)
                {
                    foreach (ThreeDCoord coord in item.GetAffectedTiles.Values)
                    {
                        if ((coord.X == ballItemCoord.X) && (coord.Y == ballItemCoord.Y))
                        {
                            this.room.GetGameManager().AddPointToTeam(user.team, user);
                            return;
                        }
                    }
                }
                foreach (RoomItem item in this.room.GetGameManager().GetItems(Team.yellow).Values)
                {
                    foreach (ThreeDCoord coord in item.GetAffectedTiles.Values)
                    {
                        if ((coord.X == ballItemCoord.X) && (coord.Y == ballItemCoord.Y))
                        {
                            this.room.GetGameManager().AddPointToTeam(user.team, user);
                            break;
                        }
                    }
                }
            }
        }

        internal void MoveBall(RoomItem item, GameClient mover, int newX, int newY)
        {
            if (((item != null) && (mover != null)) && this.room.GetGameMap().itemCanBePlacedHere(newX, newY))
            {
                Point coordinate = item.Coordinate;
                bool flag = this.GameItemOverlaps(item);
                double num = this.room.GetGameMap().Model.SqFloorHeight[newX, newY];
                ServerMessage message = new ServerMessage();
                message.Init(Outgoing.ObjectOnRoller);
                message.AppendInt32(item.Coordinate.X);
                message.AppendInt32(item.Coordinate.Y);
                message.AppendInt32(newX);
                message.AppendInt32(newY);
                message.AppendInt32(1);
                message.AppendInt32(item.Id);
                message.AppendString(item.GetZ.ToString().Replace(',', '.'));
                message.AppendString(num.ToString().Replace(',', '.'));
                message.AppendInt32(0);
                this.room.SendMessage(message);
                if ((coordinate.X != newX) || (coordinate.Y != newY))
                {
                    this.room.GetRoomItemHandler().SetFloorItem(mover, item, newX, newY, item.Rot, false, false, false, false);
                    if (!flag)
                    {
                        this.HandleFootballGameItems(new Point(newX, newY), this.room.GetRoomUserManager().GetRoomUserByHabbo(mover.GetHabbo().Id));
                    }
                }
            }
        }

        internal void MoveBall(RoomItem item, GameClient client, Point user, Point ball, int length)
        {
            int num = user.X - ball.X;
            int num2 = user.Y - ball.Y;
            if ((((num <= 1) && (num >= -1)) && (num2 <= 1)) && (num2 >= -1))
            {
                int x = ball.X;
                int y = ball.Y;
                for (int i = 1; i < length; i++)
                {
                    x = num * (0 - i);
                    y = num2 * (0 - i);
                    x += item.GetX;
                    y += item.GetY;
                    if (!this.room.GetGameMap().itemCanBePlacedHere(x, y))
                    {
                        if (i != 1)
                        {
                            i--;
                            x = num * (0 - i);
                            y = num2 * (0 - i);
                            x += item.GetX;
                            y += item.GetY;
                        }
                        break;
                    }
                }
                if ((x != ball.X) || (y != ball.Y))
                {
                    this.MoveBall(item, client, x, y);
                }
            }
        }

        internal void OnCycle()
        {
            this.balls.OnCycle();
        }

        internal void onGateRemove(RoomItem item)
        {
            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.footballgoalgreen:
                case InteractionType.footballcountergreen:
                    this.room.GetGameManager().RemoveFurnitureFromTeam(item, Team.green);
                    break;

                case InteractionType.footballgoalyellow:
                case InteractionType.footballcounteryellow:
                    this.room.GetGameManager().RemoveFurnitureFromTeam(item, Team.yellow);
                    break;

                case InteractionType.footballgoalblue:
                case InteractionType.footballcounterblue:
                    this.room.GetGameManager().RemoveFurnitureFromTeam(item, Team.blue);
                    break;

                case InteractionType.footballgoalred:
                case InteractionType.footballcounterred:
                    this.room.GetGameManager().RemoveFurnitureFromTeam(item, Team.red);
                    break;
            }
        }

        internal void OnUserWalk(RoomUser User)
        {
            if (User != null)
            {
                foreach (RoomItem item in this.balls.Values)
                {
                    int num = User.X - item.GetX;
                    int num2 = User.Y - item.GetY;
                    if ((((num <= 1) && (num >= -1)) && (num2 <= 1)) && (num2 >= -1))
                    {
                        int x = num * -1;
                        int y = num2 * -1;
                        x += item.GetX;
                        y += item.GetY;
                        if ((item.interactingBallUser == User.userID) && item.GetRoom().GetGameMap().ValidTile(x, y))
                        {
                            item.interactingBallUser = 0;
                            item.interactionCountHelper = 0;
                            this.MoveBall(item, User.GetClient(), User.Coordinate, item.Coordinate, 6);
                        }
                        else if (item.GetRoom().GetGameMap().ValidTile(x, y))
                        {
                            if (item.interactionCountHelper == 0)
                            {
                                this.MoveBall(item, User.GetClient(), User.Coordinate, item.Coordinate, new Random().Next(0, 6));
                                item.interactionCountHelper = (byte) (item.interactionCountHelper + 1);
                            }
                            else
                            {
                                this.MoveBall(item, User.GetClient(), x, y);
                            }
                        }
                    }
                }
            }
        }

        internal void RegisterGate(RoomItem item)
        {
            if (this.gates[0] == null)
            {
                item.team = Team.blue;
                this.gates[0] = item;
            }
            else if (this.gates[1] == null)
            {
                item.team = Team.red;
                this.gates[1] = item;
            }
            else if (this.gates[2] == null)
            {
                item.team = Team.green;
                this.gates[2] = item;
            }
            else if (this.gates[3] == null)
            {
                item.team = Team.yellow;
                this.gates[3] = item;
            }
        }

        internal void RemoveBall(uint itemID)
        {
            this.balls.Remove(itemID);
        }

        internal void UnRegisterGate(RoomItem item)
        {
            switch (item.team)
            {
                case Team.red:
                    this.gates[1] = null;
                    break;

                case Team.green:
                    this.gates[2] = null;
                    break;

                case Team.blue:
                    this.gates[0] = null;
                    break;

                case Team.yellow:
                    this.gates[3] = null;
                    break;
            }
        }
    }
}

