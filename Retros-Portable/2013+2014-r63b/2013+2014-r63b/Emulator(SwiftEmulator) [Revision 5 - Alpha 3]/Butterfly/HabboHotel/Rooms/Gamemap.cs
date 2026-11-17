namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Pathfinding;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class Gamemap
    {
        internal bool DiagonalEnabled;
        internal bool gotPublicPool;
        private Hashtable mCoordinatedItems;
        private DynamicRoomModel mDynamicModel;
        private byte[,] mGameMap;
        private double[,] mItemHeightMap;
        private RoomModel mStaticModel;
        private byte[,] mUserItemEffect;
        private Room room;
        private List<RoomLinkInformation> roomLinkInformation;
        private Hashtable userMap;

        public Gamemap(Room room)
        {
            this.room = room;
            this.DiagonalEnabled = true;
            this.mStaticModel = ButterflyEnvironment.GetGame().GetRoomManager().GetModel(room.ModelName, room.RoomId);
            if (this.mStaticModel == null)
            {
                throw new Exception("No modeldata found for roomID " + room.RoomId);
            }
            this.mDynamicModel = new DynamicRoomModel(this.mStaticModel);
            this.mCoordinatedItems = new Hashtable();
            this.gotPublicPool = room.RoomData.Model.gotPublicPool;
            this.mGameMap = new byte[this.Model.MapSizeX, this.Model.MapSizeY];
            this.mItemHeightMap = new double[this.Model.MapSizeX, this.Model.MapSizeY];
            this.userMap = new Hashtable();
            if (room.IsPublic)
            {
                this.roomLinkInformation = ButterflyEnvironment.GetGame().GetRoomManager().getLinkedRoomData(room.RoomId);
            }
        }

        internal void AddCoordinatedItem(RoomItem item, Point coord)
        {
            List<RoomItem> list = new List<RoomItem>();
            if (!this.mCoordinatedItems.ContainsKey(coord))
            {
                list = new List<RoomItem> {
                    item
                };
                this.mCoordinatedItems.Add(coord, list);
            }
            else
            {
                list = (List<RoomItem>) this.mCoordinatedItems[coord];
                if (!list.Contains(item))
                {
                    list.Add(item);
                    this.mCoordinatedItems[coord] = list;
                }
            }
        }

        internal bool AddItemToMap(RoomItem Item)
        {
            return this.AddItemToMap(Item, true);
        }

        internal bool AddItemToMap(RoomItem Item, bool handleGameItem)
        {
            if (handleGameItem)
            {
                if (this.room.GotWired() && WiredUtillity.TypeIsWired(Item.GetBaseItem().InteractionType))
                {
                    this.room.GetWiredHandler().AddFurniture(Item);
                }
                this.AddSpecialItems(Item);
                switch (Item.GetBaseItem().InteractionType)
                {
                    case InteractionType.footballgoalgreen:
                    case InteractionType.footballcountergreen:
                    case InteractionType.banzaiscoregreen:
                    case InteractionType.freezegreencounter:
                    case InteractionType.freezegreengate:
                        this.room.GetGameManager().AddFurnitureToTeam(Item, Team.green);
                        break;

                    case InteractionType.footballgoalyellow:
                    case InteractionType.footballcounteryellow:
                    case InteractionType.banzaiscoreyellow:
                    case InteractionType.freezeyellowcounter:
                    case InteractionType.freezeyellowgate:
                        this.room.GetGameManager().AddFurnitureToTeam(Item, Team.yellow);
                        break;

                    case InteractionType.footballgoalblue:
                    case InteractionType.footballcounterblue:
                    case InteractionType.banzaiscoreblue:
                    case InteractionType.freezebluecounter:
                    case InteractionType.freezebluegate:
                        this.room.GetGameManager().AddFurnitureToTeam(Item, Team.blue);
                        break;

                    case InteractionType.footballgoalred:
                    case InteractionType.footballcounterred:
                    case InteractionType.banzaiscorered:
                    case InteractionType.freezeredcounter:
                    case InteractionType.freezeredgate:
                        this.room.GetGameManager().AddFurnitureToTeam(Item, Team.red);
                        break;

                    case InteractionType.freezeexit:
                        this.room.GetFreeze().ExitTeleport = Item;
                        break;

                    case InteractionType.roller:
                        if (!this.room.GetRoomItemHandler().mRollers.ContainsKey(Item.Id))
                        {
                            this.room.GetRoomItemHandler().mRollers.Add(Item.Id, Item);
                        }
                        break;
                }
            }
            if (Item.GetBaseItem().Type == 's')
            {
                foreach (Point point in Item.GetCoords)
                {
                    Point coord = new Point(point.X, point.Y);
                    this.AddCoordinatedItem(Item, coord);
                }
                if (Item.GetX > (this.Model.MapSizeX - 1))
                {
                    this.Model.AddX();
                    this.GenerateMaps(true);
                    return false;
                }
                if (Item.GetY > (this.Model.MapSizeY - 1))
                {
                    this.Model.AddY();
                    this.GenerateMaps(true);
                    return false;
                }
                foreach (Point point in Item.GetCoords)
                {
                    if (!this.ConstructMapForItem(Item, point))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void AddSpecialItems(RoomItem item)
        {
            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.fbgate:
                {
                    this.room.GetSoccer().RegisterGate(item);
                    string[] strArray = item.ExtraData.Split(new char[] { ':' });
                    if (string.IsNullOrEmpty(item.ExtraData) || (strArray.Length <= 1))
                    {
                        item.Gender = "M";
                        switch (item.team)
                        {
                            case Team.red:
                                item.Figure = "lg-275-96.hr-115-61.hd-180-3.ch-265-96.sh-305-62";
                                return;

                            case Team.green:
                                item.Figure = "lg-275-102.hr-115-61.hd-180-3.ch-265-102.sh-305-62";
                                return;

                            case Team.blue:
                                item.Figure = "lg-275-108.hr-115-61.hd-180-3.ch-265-108.sh-305-62";
                                return;

                            case Team.yellow:
                                item.Figure = "lg-275-93.hr-115-61.hd-207-14.ch-265-93.sh-305-62";
                                return;
                        }
                    }
                    else
                    {
                        item.Gender = strArray[0];
                        item.Figure = strArray[1];
                    }
                    break;
                }
                case InteractionType.football:
                    this.room.GetSoccer().AddBall(item);
                    break;

                case InteractionType.banzaitele:
                    this.room.GetGameItemHandler().AddTeleport(item, item.Id);
                    item.ExtraData = "";
                    break;

                case InteractionType.banzaipuck:
                    this.room.GetBanzai().AddPuck(item);
                    break;

                case InteractionType.banzaipyramid:
                    this.room.GetGameItemHandler().AddPyramid(item, item.Id);
                    break;

                case InteractionType.freezeexit:
                {
                    RoomItem exitTeleport = this.room.GetFreeze().ExitTeleport;
                    if ((exitTeleport != null) && (item.Id == exitTeleport.Id))
                    {
                        this.room.GetFreeze().ExitTeleport = null;
                    }
                    break;
                }
                case InteractionType.banzaifloor:
                    this.room.GetBanzai().AddTile(item, item.Id);
                    break;

                case InteractionType.freezetileblock:
                    this.room.GetFreeze().AddFreezeBlock(item);
                    break;

                case InteractionType.freezetile:
                    this.room.GetFreeze().AddFreezeTile(item);
                    break;
            }
        }

        internal void AddToMap(RoomItem item)
        {
            this.AddItemToMap(item);
        }

        internal void AddUserToMap(RoomUser user, Point coord)
        {
            if (this.userMap.ContainsKey(coord))
            {
                ((List<RoomUser>) this.userMap[coord]).Add(user);
            }
            else
            {
                List<RoomUser> list = new List<RoomUser> {
                    user
                };
                this.userMap.Add(coord, list);
            }
        }

        internal bool CanRollItemHere(int x, int y)
        {
            if (!this.ValidTile(x, y))
            {
                return false;
            }
            if (this.Model.SqState[x, y] == SquareState.BLOCKED)
            {
                return false;
            }
            return true;
        }

        internal static bool CanWalk(byte pState, bool pOverride)
        {
            if (!pOverride)
            {
                return ((pState == 3) || (pState == 1));
            }
            return true;
        }

        internal bool CanWalk(int X, int Y, bool Override)
        {
            if (!this.room.AllowWalkthrough)
            {
                if (Override)
                {
                    return true;
                }
                if (this.room.GetRoomUserManager().GetUserForSquare(X, Y) != null)
                {
                    return false;
                }
            }
            return true;
        }

        private bool ConstructMapForItem(RoomItem Item, Point Coord)
        {
            try
            {
                if (Coord.X > (this.Model.MapSizeX - 1))
                {
                    this.Model.AddX();
                    this.GenerateMaps(true);
                    return false;
                }
                if (Coord.Y > (this.Model.MapSizeY - 1))
                {
                    this.Model.AddY();
                    this.GenerateMaps(true);
                    return false;
                }
                if (this.Model.SqState[Coord.X, Coord.Y] == SquareState.BLOCKED)
                {
                    this.Model.OpenSquare(Coord.X, Coord.Y, Item.GetZ);
                    this.Model.SetUpdateState();
                }
                if (this.mItemHeightMap[Coord.X, Coord.Y] <= Item.TotalHeight)
                {
                    this.mItemHeightMap[Coord.X, Coord.Y] = Item.TotalHeight - this.mDynamicModel.SqFloorHeight[Item.GetX, Item.GetY];
                    this.mUserItemEffect[Coord.X, Coord.Y] = 0;
                    switch (Item.GetBaseItem().InteractionType)
                    {
                        case InteractionType.iceskates:
                            this.mUserItemEffect[Coord.X, Coord.Y] = 3;
                            break;

                        case InteractionType.normslaskates:
                            this.mUserItemEffect[Coord.X, Coord.Y] = 2;
                            break;

                        case InteractionType.lowpool:
                            this.mUserItemEffect[Coord.X, Coord.Y] = 4;
                            break;

                        case InteractionType.haloweenpool:
                            this.mUserItemEffect[Coord.X, Coord.Y] = 5;
                            break;

                        case InteractionType.pool:
                            this.mUserItemEffect[Coord.X, Coord.Y] = 1;
                            break;
                    }
                    if (Item.GetBaseItem().Walkable)
                    {
                        if (this.mGameMap[Coord.X, Coord.Y] != 3)
                        {
                            this.mGameMap[Coord.X, Coord.Y] = 1;
                        }
                    }
                    else if (((Item.GetZ <= (this.Model.SqFloorHeight[Item.GetX, Item.GetY] + 0.1)) && (Item.GetBaseItem().InteractionType == InteractionType.gate)) && (Item.ExtraData == "1"))
                    {
                        if (this.mGameMap[Coord.X, Coord.Y] != 3)
                        {
                            this.mGameMap[Coord.X, Coord.Y] = 1;
                        }
                    }
                    
                    else if (Item.GetBaseItem().IsSeat || (Item.GetBaseItem().InteractionType == InteractionType.bed))
                    {
                        this.mGameMap[Coord.X, Coord.Y] = 3;
                    }
                    else if (this.mGameMap[Coord.X, Coord.Y] != 3)
                    {
                        this.mGameMap[Coord.X, Coord.Y] = 0;
                    }
                }
                if (Item.GetBaseItem().InteractionType == InteractionType.bed)
                {
                    this.mGameMap[Coord.X, Coord.Y] = 3;
                }
            }
            catch (Exception exception)
            {
                Logging.LogException(string.Concat(new object[] { "Error during map generation for room ", this.room.RoomId, ". Exception: ", exception.ToString() }));
            }
            return true;
        }

        internal void Destroy()
        {
            this.userMap.Clear();
            this.mDynamicModel.Destroy();
            this.mCoordinatedItems.Clear();
            if (this.roomLinkInformation != null)
            {
                this.roomLinkInformation.Clear();
            }
            Array.Clear(this.mGameMap, 0, this.mGameMap.Length);
            Array.Clear(this.mUserItemEffect, 0, this.mUserItemEffect.Length);
            Array.Clear(this.mItemHeightMap, 0, this.mItemHeightMap.Length);
            this.userMap = null;
            this.mGameMap = null;
            this.mUserItemEffect = null;
            this.mItemHeightMap = null;
            this.mCoordinatedItems = null;
            this.roomLinkInformation = null;
            this.mDynamicModel = null;
            this.room = null;
            this.mStaticModel = null;
        }

        internal string GenerateMapDump()
        {
            int num;
            StringBuilder builder2;
            int num2;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Game map:");
            for (num = 0; num < this.Model.MapSizeY; num++)
            {
                builder2 = new StringBuilder();
                num2 = 0;
                while (num2 < this.Model.MapSizeX)
                {
                    builder2.Append(this.mGameMap[num2, num].ToString());
                    num2++;
                }
                builder.AppendLine(builder2.ToString());
            }
            builder.AppendLine();
            builder.AppendLine("Item height map:");
            for (num = 0; num < this.Model.MapSizeY; num++)
            {
                builder2 = new StringBuilder();
                num2 = 0;
                while (num2 < this.Model.MapSizeX)
                {
                    builder2.Append("[" + this.mItemHeightMap[num2, num].ToString() + "]");
                    num2++;
                }
                builder.AppendLine(builder2.ToString());
            }
            builder.AppendLine();
            builder.AppendLine("Static data:");
            for (num = 0; num < this.Model.MapSizeY; num++)
            {
                builder2 = new StringBuilder();
                num2 = 0;
                while (num2 < this.Model.MapSizeX)
                {
                    builder2.Append("[" + this.Model.SqState[num2, num].ToString() + "]");
                    num2++;
                }
                builder.AppendLine(builder2.ToString());
            }
            builder.AppendLine();
            builder.AppendLine("Static data height:");
            for (num = 0; num < this.Model.MapSizeY; num++)
            {
                builder2 = new StringBuilder();
                num2 = 0;
                while (num2 < this.Model.MapSizeX)
                {
                    builder2.Append("[" + this.Model.SqFloorHeight[num2, num].ToString() + "]");
                    num2++;
                }
                builder.AppendLine(builder2.ToString());
            }
            builder.AppendLine();
            builder.AppendLine("Pool map:");
            for (num = 0; num < this.Model.MapSizeY; num++)
            {
                builder2 = new StringBuilder();
                for (num2 = 0; num2 < this.Model.MapSizeX; num2++)
                {
                    builder2.Append("[" + this.mUserItemEffect[num2, num].ToString() + "]");
                }
                builder.AppendLine(builder2.ToString());
            }
            builder.AppendLine();
            return builder.ToString();
        }

        internal void GenerateMaps(bool checkLines = true)
        {
            int getX = 0;
            int getY = 0;
            this.mCoordinatedItems = new Hashtable();
            if (checkLines)
            {
                RoomItem[] array = this.room.GetRoomItemHandler().mFloorItems.Values.ToArray<RoomItem>();
                foreach (RoomItem item in array)
                {
                    if ((item.GetX > this.Model.MapSizeX) && (item.GetX > getX))
                    {
                        getX = item.GetX;
                    }
                    if ((item.GetY > this.Model.MapSizeY) && (item.GetY > getY))
                    {
                        getY = item.GetY;
                    }
                }
                Array.Clear(array, 0, array.Length);
                array = null;
            }
            if ((getY > (this.Model.MapSizeY - 1)) || (getX > (this.Model.MapSizeX - 1)))
            {
                if (getX < this.Model.MapSizeX)
                {
                    getX = this.Model.MapSizeX;
                }
                if (getY < this.Model.MapSizeY)
                {
                    getY = this.Model.MapSizeY;
                }
                this.Model.SetMapsize(getX + 7, getY + 7);
                this.GenerateMaps(false);
            }
            else
            {
                int num3;
                int num4;
                int num5;
                int num6;
                if ((getX != this.StaticModel.MapSizeX) || (getY != this.StaticModel.MapSizeY))
                {
                    this.mUserItemEffect = new byte[this.Model.MapSizeX, this.Model.MapSizeY];
                    this.mGameMap = new byte[this.Model.MapSizeX, this.Model.MapSizeY];
                    this.mItemHeightMap = new double[this.Model.MapSizeX, this.Model.MapSizeY];
                    for (num3 = 0; num3 < this.Model.MapSizeY; num3++)
                    {
                        num4 = 0;
                        while (num4 < this.Model.MapSizeX)
                        {
                            this.mGameMap[num4, num3] = 0;
                            this.mUserItemEffect[num4, num3] = 0;
                            if ((num4 == this.Model.DoorX) && (num3 == this.Model.DoorY))
                            {
                                this.mGameMap[num4, num3] = 3;
                            }
                            else if (this.Model.SqState[num4, num3] == SquareState.OPEN)
                            {
                                this.mGameMap[num4, num3] = 1;
                            }
                            else if (this.Model.SqState[num4, num3] == SquareState.SEAT)
                            {
                                this.mGameMap[num4, num3] = 2;
                            }
                            else if (this.Model.SqState[num4, num3] == SquareState.POOL)
                            {
                                this.mUserItemEffect[num4, num3] = 6;
                            }
                            num4++;
                        }
                    }
                    if (this.gotPublicPool)
                    {
                        for (num5 = 0; num5 < this.StaticModel.MapSizeY; num5++)
                        {
                            num6 = 0;
                            while (num6 < this.StaticModel.MapSizeX)
                            {
                                if (this.StaticModel.mRoomModelfx[num6, num5] != 0)
                                {
                                    this.mUserItemEffect[num6, num5] = this.StaticModel.mRoomModelfx[num6, num5];
                                }
                                num6++;
                            }
                        }
                    }
                }
                else
                {
                    this.mUserItemEffect = new byte[this.Model.MapSizeX, this.Model.MapSizeY];
                    this.mGameMap = new byte[this.Model.MapSizeX, this.Model.MapSizeY];
                    this.mItemHeightMap = new double[this.Model.MapSizeX, this.Model.MapSizeY];
                    for (num3 = 0; num3 < this.Model.MapSizeY; num3++)
                    {
                        for (num4 = 0; num4 < this.Model.MapSizeX; num4++)
                        {
                            this.mGameMap[num4, num3] = 0;
                            this.mUserItemEffect[num4, num3] = 0;
                            if ((num4 == this.Model.DoorX) && (num3 == this.Model.DoorY))
                            {
                                this.mGameMap[num4, num3] = 3;
                            }
                            else if (this.Model.SqState[num4, num3] == SquareState.OPEN)
                            {
                                this.mGameMap[num4, num3] = 1;
                            }
                            else if (this.Model.SqState[num4, num3] == SquareState.SEAT)
                            {
                                this.mGameMap[num4, num3] = 2;
                            }
                            else if (this.Model.SqState[num4, num3] == SquareState.POOL)
                            {
                                this.mUserItemEffect[num4, num3] = 6;
                            }
                        }
                    }
                    if (this.gotPublicPool)
                    {
                        for (num5 = 0; num5 < this.StaticModel.MapSizeY; num5++)
                        {
                            for (num6 = 0; num6 < this.StaticModel.MapSizeX; num6++)
                            {
                                if (this.StaticModel.mRoomModelfx[num6, num5] != 0)
                                {
                                    this.mUserItemEffect[num6, num5] = this.StaticModel.mRoomModelfx[num6, num5];
                                }
                            }
                        }
                    }
                }
                RoomItem[] itemArray2 = this.room.GetRoomItemHandler().mFloorItems.Values.ToArray<RoomItem>();
                foreach (RoomItem item2 in itemArray2)
                {
                    if (!this.AddItemToMap(item2))
                    {
                        break;
                    }
                }
                Array.Clear(itemArray2, 0, itemArray2.Length);
                itemArray2 = null;
                if (!this.room.AllowWalkthrough)
                {
                    foreach (RoomUser user in this.room.GetRoomUserManager().UserList.Values)
                    {
                        user.SqState = this.mGameMap[user.X, user.Y];
                        this.mGameMap[user.X, user.Y] = 0;
                    }
                }
                this.mGameMap[this.Model.DoorX, this.Model.DoorY] = 3;
            }
        }

        internal static Dictionary<int, ThreeDCoord> GetAffectedTiles(int Length, int Width, int PosX, int PosY, int Rotation)
        {
            int num2;
            int num3;
            int key = 0;
            Dictionary<int, ThreeDCoord> dictionary = new Dictionary<int, ThreeDCoord>();
            if (Length > 1)
            {
                if ((Rotation == 0) || (Rotation == 4))
                {
                    for (num2 = 1; num2 < Length; num2++)
                    {
                        key++;
                        dictionary.Add(key, new ThreeDCoord(PosX, PosY + num2, num2));
                        num3 = 1;
                        while (num3 < Width)
                        {
                            key++;
                            dictionary.Add(key, new ThreeDCoord(PosX + num3, PosY + num2, (num2 < num3) ? num3 : num2));
                            num3++;
                        }
                    }
                }
                else if ((Rotation == 2) || (Rotation == 6))
                {
                    for (num2 = 1; num2 < Length; num2++)
                    {
                        key++;
                        dictionary.Add(key, new ThreeDCoord(PosX + num2, PosY, num2));
                        num3 = 1;
                        while (num3 < Width)
                        {
                            key++;
                            dictionary.Add(key, new ThreeDCoord(PosX + num2, PosY + num3, (num2 < num3) ? num3 : num2));
                            num3++;
                        }
                    }
                }
            }
            if (Width > 1)
            {
                if ((Rotation == 0) || (Rotation == 4))
                {
                    for (num2 = 1; num2 < Width; num2++)
                    {
                        key++;
                        dictionary.Add(key, new ThreeDCoord(PosX + num2, PosY, num2));
                        num3 = 1;
                        while (num3 < Length)
                        {
                            key++;
                            dictionary.Add(key, new ThreeDCoord(PosX + num2, PosY + num3, (num2 < num3) ? num3 : num2));
                            num3++;
                        }
                    }
                    return dictionary;
                }
                if ((Rotation != 2) && (Rotation != 6))
                {
                    return dictionary;
                }
                for (num2 = 1; num2 < Width; num2++)
                {
                    key++;
                    dictionary.Add(key, new ThreeDCoord(PosX, PosY + num2, num2));
                    for (num3 = 1; num3 < Length; num3++)
                    {
                        key++;
                        dictionary.Add(key, new ThreeDCoord(PosX + num3, PosY + num2, (num2 < num3) ? num3 : num2));
                    }
                }
            }
            return dictionary;
        }

        internal List<RoomItem> GetAllRoomItemForSquare(int pX, int pY)
        {
            Point key = new Point(pX, pY);
            List<RoomItem> list = new List<RoomItem>();
            if (this.mCoordinatedItems.ContainsKey(key))
            {
                List<RoomItem> list2 = (List<RoomItem>) this.mCoordinatedItems[key];
                foreach (RoomItem item in list2)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        internal List<RoomItem> GetCoordinatedItems(Point coord)
        {
            Point key = new Point(coord.X, coord.Y);
            if (this.mCoordinatedItems.ContainsKey(key))
            {
                return (List<RoomItem>) this.mCoordinatedItems[key];
            }
            return new List<RoomItem>();
        }

        internal byte GetFloorStatus(Point coord)
        {
            if ((coord.X > this.mGameMap.GetUpperBound(0)) || (coord.Y > this.mGameMap.GetUpperBound(1)))
            {
                return 1;
            }
            return this.mGameMap[coord.X, coord.Y];
        }

        internal double GetHeightForSquareFromData(Point coord)
        {
            if ((coord.X > this.mDynamicModel.SqFloorHeight.GetUpperBound(0)) || (coord.Y > this.mDynamicModel.SqFloorHeight.GetUpperBound(1)))
            {
                return 1.0;
            }
            return this.mDynamicModel.SqFloorHeight[coord.X, coord.Y];
        }

        internal Point getRandomWalkableSquare()
        {
            List<Point> list = new List<Point>();
            for (int i = 0; i < (this.mGameMap.GetUpperBound(1) - 1); i++)
            {
                for (int j = 0; j < (this.mGameMap.GetUpperBound(0) - 1); j++)
                {
                    if (((this.mStaticModel.DoorX != j) && (this.mStaticModel.DoorY != i)) && (this.mGameMap[j, i] == 1))
                    {
                        list.Add(new Point(j, i));
                    }
                }
            }
            int randomNumber = new Random(DateTime.Now.Millisecond).Next(0, list.Count);
            try
            {
                return list[randomNumber - 1];
            }
            catch
            {
                return list[randomNumber];
            }
        }
        internal List<Point> getWalkableList()
        {
            List<Point> list = new List<Point>();
            for (int i = 0; i < (this.mGameMap.GetUpperBound(1) - 1); i++)
            {
                for (int j = 0; j < (this.mGameMap.GetUpperBound(0) - 1); j++)
                {
                    if (((this.mStaticModel.DoorX != j) && (this.mStaticModel.DoorY != i)) && (this.mGameMap[j, i] == 1))
                    {
                        list.Add(new Point(j, i));
                    }
                }
            }
            return list;
        }

        internal List<RoomItem> GetRoomItemForSquare(int pX, int pY)
        {
            Point key = new Point(pX, pY);
            List<RoomItem> list = new List<RoomItem>();
            if (this.mCoordinatedItems.ContainsKey(key))
            {
                List<RoomItem> list2 = (List<RoomItem>) this.mCoordinatedItems[key];
                foreach (RoomItem item in list2)
                {
                    if ((item.Coordinate.X == key.X) && (item.Coordinate.Y == key.Y))
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        internal List<RoomItem> GetRoomItemForSquare(int pX, int pY, double minZ)
        {
            List<RoomItem> list = new List<RoomItem>();
            Point key = new Point(pX, pY);
            if (this.mCoordinatedItems.ContainsKey(key))
            {
                List<RoomItem> list2 = (List<RoomItem>) this.mCoordinatedItems[key];
                foreach (RoomItem item in list2)
                {
                    if ((item.GetZ > minZ) && ((item.GetX == pX) && (item.GetY == pY)))
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        internal List<RoomUser> GetRoomUsers(Point coord)
        {
            if (this.userMap.ContainsKey(coord))
            {
                return (List<RoomUser>) this.userMap[coord];
            }
            return new List<RoomUser>();
        }

        internal void HandleRoomLinks(RoomUser user)
        {
            foreach (RoomLinkInformation information in from p in this.roomLinkInformation
                where (p.fromX == user.SetX) && (p.fromY == user.SetY)
                select p)
            {
                RoomUser userForSquare = this.room.GetRoomUserManager().GetUserForSquare(information.fromX, information.fromY);
                if ((userForSquare != null) && ((userForSquare.GetClient() != null) && (userForSquare.GetClient().GetMessageHandler() != null)))
                {
                    userForSquare.GetClient().SetDoorPos = true;
                    userForSquare.GetClient().newDoorPos = new Point(information.toX, information.toY);
                    userForSquare.GetClient().GetMessageHandler().PrepareRoomForUser(information.toRoomID, "");
                }
            }
        }

        internal bool itemCanBePlacedHere(int x, int y)
        {
            if ((((this.mDynamicModel.MapSizeX - 1) < x) || ((this.mDynamicModel.MapSizeY - 1) < y)) || ((x == this.mDynamicModel.DoorX) && (y == this.mDynamicModel.DoorY)))
            {
                return false;
            }
            return (this.mGameMap[x, y] == 1);
        }

        internal bool MapGotUser(Point coord)
        {
            return (this.GetRoomUsers(coord).Count > 0);
        }

        internal bool RemoveCoordinatedItem(RoomItem item, Point coord)
        {
            Point key = new Point(coord.X, coord.Y);
            if (this.mCoordinatedItems.ContainsKey(key))
            {
                ((List<RoomItem>) this.mCoordinatedItems[key]).Remove(item);
                return true;
            }
            return false;
        }

        internal bool RemoveFromMap(RoomItem item)
        {
            if (this.room.GotWired() && WiredUtillity.TypeIsWired(item.GetBaseItem().InteractionType))
            {
                this.room.GetWiredHandler().RemoveFurniture(item);
            }
            return this.RemoveFromMap(item, true);
        }

        internal bool RemoveFromMap(RoomItem item, bool handleGameItem)
        {
            if (handleGameItem)
            {
                this.RemoveSpecialItem(item);
            }
            if (this.room.GotSoccer())
            {
                this.room.GetSoccer().onGateRemove(item);
            }
            bool flag = false;
            foreach (Point point in item.GetCoords)
            {
                if (this.RemoveCoordinatedItem(item, point))
                {
                    flag = true;
                }
            }
            Hashtable hashtable = new Hashtable();
            foreach (Point point2 in item.GetCoords)
            {
                Point key = new Point(point2.X, point2.Y);
                if (this.mCoordinatedItems.ContainsKey(key))
                {
                    List<RoomItem> list = (List<RoomItem>) this.mCoordinatedItems[key];
                    if (!hashtable.ContainsKey(point2))
                    {
                        hashtable.Add(point2, list);
                    }
                }
                this.SetDefaultValue(point2.X, point2.Y);
            }
            foreach (Point point in hashtable.Keys)
            {
                if (hashtable.ContainsKey(point))
                {
                    List<RoomItem> list2 = (List<RoomItem>) hashtable[point];
                    foreach (RoomItem item2 in list2)
                    {
                        this.ConstructMapForItem(item2, point);
                    }
                }
            }
            hashtable.Clear();
            hashtable = null;
            return flag;
        }

        private void RemoveSpecialItem(RoomItem item)
        {
            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.fbgate:
                    this.room.GetSoccer().UnRegisterGate(item);
                    break;

                case InteractionType.football:
                    this.room.GetSoccer().RemoveBall(item.Id);
                    break;

                case InteractionType.banzaitele:
                    this.room.GetGameItemHandler().RemoveTeleport(item.Id);
                    break;

                case InteractionType.banzaipuck:
                    this.room.GetBanzai().RemovePuck(item.Id);
                    break;

                case InteractionType.banzaipyramid:
                    this.room.GetGameItemHandler().RemovePyramid(item.Id);
                    break;

                case InteractionType.banzaifloor:
                    this.room.GetBanzai().RemoveTile(item.Id);
                    break;

                case InteractionType.freezetileblock:
                    this.room.GetFreeze().RemoveFreezeBlock(item.Id);
                    break;

                case InteractionType.freezetile:
                    this.room.GetFreeze().RemoveFreezeTile(item.Id);
                    break;
            }
        }

        internal void RemoveUserFromMap(RoomUser user, Point coord)
        {
            if (this.userMap.ContainsKey(coord))
            {
                ((List<RoomUser>) this.userMap[coord]).Remove(user);
            }
        }

        private void SetDefaultValue(int x, int y)
        {
            this.mGameMap[x, y] = 0;
            this.mUserItemEffect[x, y] = 0;
            this.mItemHeightMap[x, y] = 0.0;
            if ((x == this.Model.DoorX) && (y == this.Model.DoorY))
            {
                this.mGameMap[x, y] = 3;
            }
            else if (this.Model.SqState[x, y] == SquareState.OPEN)
            {
                this.mGameMap[x, y] = 1;
            }
            else if (this.Model.SqState[x, y] == SquareState.SEAT)
            {
                this.mGameMap[x, y] = 2;
            }
        }

        internal double SqAbsoluteHeight(int X, int Y)
        {
            Point key = new Point(X, Y);
            if (this.mCoordinatedItems.ContainsKey(key))
            {
                List<RoomItem> itemsOnSquare = (List<RoomItem>) this.mCoordinatedItems[key];
                return this.SqAbsoluteHeight(X, Y, itemsOnSquare);
            }
            return this.mDynamicModel.SqFloorHeight[X, Y];
        }

        internal double SqAbsoluteHeight(int X, int Y, List<RoomItem> ItemsOnSquare)
        {
            try
            {
                double totalHeight = 0.0;
                bool flag = false;
                double height = 0.0;
                foreach (RoomItem item in ItemsOnSquare)
                {
                    if (item.TotalHeight > totalHeight)
                    {
                        if (item.GetBaseItem().IsSeat || (item.GetBaseItem().InteractionType == InteractionType.bed))
                        {
                            flag = true;
                            height = item.GetBaseItem().Height;
                        }
                        else
                        {
                            flag = false;
                        }
                        totalHeight = item.TotalHeight;
                    }
                }
                double num3 = this.Model.SqFloorHeight[X, Y];
                double num4 = totalHeight - this.Model.SqFloorHeight[X, Y];
                if (flag)
                {
                    num4 -= height;
                }
                if (num4 < 0.0)
                {
                    num4 = 0.0;
                }
                return (num3 + num4);
            }
            catch (Exception exception)
            {
                Logging.HandleException(exception, "Room.SqAbsoluteHeight");
                return 0.0;
            }
        }

        internal bool SquareHasUsers(int X, int Y)
        {
            return this.MapGotUser(new Point(X, Y));
        }

        internal bool SquareIsOpen(int x, int y, bool pOverride)
        {
            if (((this.mDynamicModel.MapSizeX - 1) < x) || ((this.mDynamicModel.MapSizeY - 1) < y))
            {
                return false;
            }
            return CanWalk(this.mGameMap[x, y], pOverride);
        }

        internal void TeleportToItem(RoomUser user, RoomItem item)
        {
            this.GameMap[user.X, user.Y] = user.SqState;
            this.UpdateUserMovement(new Point(user.Coordinate.X, user.Coordinate.Y), new Point(item.Coordinate.X, item.Coordinate.Y), user);
            user.X = item.GetX;
            user.Y = item.GetY;
            user.Z = item.GetZ;
            if (user.isFlying)
            {
                user.Z += 4.0 + (0.5 * Math.Sin(0.7 * user.flyk));
            }
            user.SqState = this.GameMap[item.GetX, item.GetY];
            this.GameMap[user.X, user.Y] = 1;
            user.RotBody = item.Rot;
            user.RotHead = item.Rot;
            user.GoalX = user.X;
            user.GoalY = user.Y;
            user.SetStep = false;
            user.IsWalking = false;
            user.UpdateNeeded = true;
        }

        internal static int TileDistance(int X1, int Y1, int X2, int Y2)
        {
            return (Math.Abs((int) (X1 - X2)) + Math.Abs((int) (Y1 - Y2)));
        }

        internal static bool TilesTouching(int X1, int Y1, int X2, int Y2)
        {
            return (((Math.Abs((int) (X1 - X2)) <= 1) && (Math.Abs((int) (Y1 - Y2)) <= 1)) || ((X1 == X2) && (Y1 == Y2)));
        }

        internal void updateMapForItem(RoomItem item)
        {
            this.RemoveFromMap(item);
            this.AddToMap(item);
        }

        internal void UpdateUserMovement(Point oldCoord, Point newCoord, RoomUser user)
        {
            this.RemoveUserFromMap(user, oldCoord);
            this.AddUserToMap(user, newCoord);
        }

        internal bool validTile(int x, int y)
        {
            return (this.mDynamicModel.SqState[x, y] == SquareState.OPEN);
        }

        internal bool ValidTile(int X, int Y)
        {
            if ((((X < 0) || (Y < 0)) || (X >= this.Model.MapSizeX)) || (Y >= this.Model.MapSizeY))
            {
                return false;
            }
            return true;
        }

        internal Hashtable CoordinatedItems
        {
            get
            {
                return this.mCoordinatedItems;
            }
        }

        internal byte[,] EffectMap
        {
            get
            {
                return this.mUserItemEffect;
            }
        }

        internal byte[,] GameMap
        {
            get
            {
                return this.mGameMap;
            }
        }

        internal double[,] ItemHeightMap
        {
            get
            {
                return this.mItemHeightMap;
            }
        }

        internal DynamicRoomModel Model
        {
            get
            {
                return this.mDynamicModel;
            }
        }

        internal RoomModel StaticModel
        {
            get
            {
                return this.mStaticModel;
            }
        }
    }
}

