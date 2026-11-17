namespace Butterfly.HabboHotel.Items
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Catalogs;
    using Butterfly.HabboHotel.Habbo.Guilds;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Items.Interactors;
    using Butterfly.HabboHotel.Pathfinding;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    

    public class RoomItem : IEquatable<RoomItem>
    {
        internal uint BaseItem;
        internal string ExtraData;
        internal string Figure;
        internal FreezePowerUp freezePowerUp;
        internal string Gender;
        internal List<string> Guilds_Data;
        internal uint placedBy;
        internal string Guilds_Data_String;
        internal uint Id;
        internal uint interactingBallUser;
        internal uint InteractingUser;
        internal uint InteractingUser2;
        public byte interactionCount;
        internal byte interactionCountHelper;
        internal bool IsTrans;
        private Dictionary<int, ThreeDCoord> mAffectedPoints;
        internal bool MagicRemove;
        private Item mBaseItem;
        private bool mIsFloorItem;
        private bool mIsRoller;
        private bool mIsWallItem;
        private Room mRoom;
        private int mX;
        private int mY;
        private double mZ;
        internal string originalExtraData;
        internal bool pendingReset;
        private Point placedPosition;
        internal uint RareId;
        internal uint RoomId;
        internal int Rot;
        internal Team team;
        internal int UpdateCounter;
        private bool updateNeeded;
        internal int value;
        internal WallCoordinate wallCoord;
        internal IWiredCondition wiredCondition;
        internal IWiredTrigger wiredHandler;

        internal event OnItemTrigger itemTriggerEventHandler;

        internal event UserWalksFurniDelegate OnUserWalksOffFurni;

        internal event UserWalksFurniDelegate OnUserWalksOnFurni;

        internal RoomItem(uint Id, uint RoomId, uint BaseItem, string ExtraData, WallCoordinate wallCoord, Room pRoom, string Guilds_data_string, uint rareid, uint placedBy)
        {
            this.placedBy = placedBy;
            this.Guilds_Data = new List<string>();
            this.pendingReset = false;
            this.MagicRemove = false;
            this.Id = Id;
            this.RoomId = RoomId;
            this.BaseItem = BaseItem;
            this.ExtraData = ExtraData;
            this.originalExtraData = ExtraData;
            this.mX = 0;
            this.mY = 0;
            this.mZ = 0.0;
            this.UpdateNeeded = false;
            this.UpdateCounter = 0;
            this.InteractingUser = 0;
            this.InteractingUser2 = 0;
            this.IsTrans = false;
            this.RareId = rareid;
            this.interactingBallUser = 0;
            this.interactionCount = 0;
            this.value = 0;
            this.placedPosition = new Point(0, 0);
            this.wallCoord = wallCoord;
            this.Guilds_Data_String = Guilds_data_string;
            if (this.Guilds_Data_String.Length > 0)
            {
                try
                {
                    foreach (string str in this.Guilds_Data_String.Split(new char[] { ';' }))
                    {
                        this.Guilds_Data.Add(str);
                    }
                }
                catch
                {
                }
            }
            this.mBaseItem = ButterflyEnvironment.GetGame().GetItemManager().GetItem(BaseItem);
            this.mRoom = pRoom;
            if (this.GetBaseItem() == null)
            {
                Logging.LogException("Unknown baseID: " + BaseItem);
            }
            this.mIsWallItem = true;
            this.mIsFloorItem = false;
            this.mAffectedPoints = new Dictionary<int, ThreeDCoord>();
            
        }

        internal RoomItem(uint Id, uint RoomId, uint BaseItem, string ExtraData, int X, int Y, double Z, int Rot, Room pRoom, string Guilds_data_string, uint rareid, uint placedBy)
        {
            this.placedBy = placedBy;
            this.Guilds_Data = new List<string>();
            this.pendingReset = false;
            this.MagicRemove = false;
            this.Id = Id;
            this.RoomId = RoomId;
            this.BaseItem = BaseItem;
            this.ExtraData = ExtraData;
            this.originalExtraData = ExtraData;
            this.mX = X;
            if ((((this.GetX + 3) > pRoom.GetGameMap().StaticModel.MapSizeX) && ((Rot == 2) || (Rot == 6))) && (this.GetBaseItem().InteractionType == InteractionType.obstacle_h))
            {
                this.mX--;
            }
            this.mY = Y;
            if ((((this.GetY + 2) > pRoom.GetGameMap().StaticModel.MapSizeY) && ((Rot == 0) || (Rot == 4))) && (this.GetBaseItem().InteractionType == InteractionType.obstacle_h))
            {
                this.mY--;
            }
            if (!double.IsInfinity(Z))
            {
                this.mZ = Z;
            }
            this.Rot = Rot;
            this.UpdateNeeded = false;
            this.UpdateCounter = 0;
            this.InteractingUser = 0;
            this.InteractingUser2 = 0;
            this.RareId = rareid;
            this.IsTrans = false;
            this.interactingBallUser = 0;
            this.interactionCount = 0;
            this.value = 0;
            this.placedPosition = new Point(X, Y);
            this.Guilds_Data_String = Guilds_data_string;
            if (this.Guilds_Data_String.Length > 0)
            {
                try
                {
                    foreach (string str in this.Guilds_Data_String.Split(new char[] { ';' }))
                    {
                        this.Guilds_Data.Add(str);
                    }
                }
                catch
                {
                }
            }
            this.mBaseItem = ButterflyEnvironment.GetGame().GetItemManager().GetItem(BaseItem);
            this.mRoom = pRoom;
            if (this.GetBaseItem() == null)
            {
                Logging.LogException("Unknown baseID: " + BaseItem);
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery("DELETE FROM items WHERE item_id = '" + this.Id + "'");
                    this.Id = 0;
                }
            }
            InteractionType interactionType = this.GetBaseItem().InteractionType;
            if (interactionType != InteractionType.teleport)
            {
                switch (interactionType)
                {
                    case InteractionType.footballcountergreen:
                    case InteractionType.banzaigategreen:
                    case InteractionType.banzaiscoregreen:
                    case InteractionType.freezegreencounter:
                    case InteractionType.freezegreengate:
                        this.team = Team.green;
                        break;

                    case InteractionType.footballcounteryellow:
                    case InteractionType.banzaigateyellow:
                    case InteractionType.banzaiscoreyellow:
                    case InteractionType.freezeyellowcounter:
                    case InteractionType.freezeyellowgate:
                        this.team = Team.yellow;
                        break;

                    case InteractionType.footballcounterblue:
                    case InteractionType.banzaigateblue:
                    case InteractionType.banzaiscoreblue:
                    case InteractionType.freezebluecounter:
                    case InteractionType.freezebluegate:
                        this.team = Team.blue;
                        break;

                    case InteractionType.footballcounterred:
                    case InteractionType.banzaigatered:
                    case InteractionType.banzaiscorered:
                    case InteractionType.freezeredcounter:
                    case InteractionType.freezeredgate:
                        this.team = Team.red;
                        break;

                    case InteractionType.banzaitele:
                        this.ExtraData = "";
                        break;

                    case InteractionType.roller:
                        this.mIsRoller = true;
                        pRoom.GetRoomItemHandler().GotRollers = true;
                        break;

                    case InteractionType.gld_wall:
                        
                        break;
                }
            }
            else
            {
                this.IsTrans = true;
                this.ReqUpdate(0, true);
            }
            this.mIsWallItem = this.GetBaseItem().Type.ToString().ToLower() == "i";
            this.mIsFloorItem = this.GetBaseItem().Type.ToString().ToLower() == "s";
            this.mAffectedPoints = Gamemap.GetAffectedTiles(this.GetBaseItem().Length, this.GetBaseItem().Width, this.mX, this.mY, Rot);
            
        }

        internal void Destroy()
        {
            this.mRoom = null;
            this.mAffectedPoints.Clear();
            if (this.wiredHandler != null)
            {
                this.wiredHandler.Dispose();
            }
            this.wiredHandler = null;
            this.itemTriggerEventHandler = null;
            this.OnUserWalksOffFurni = null;
            this.OnUserWalksOnFurni = null;
        }

        public bool Equals(RoomItem comparedItem)
        {
            return (comparedItem.Id == this.Id);
        }

        internal Item GetBaseItem()
        {
            if (this.mBaseItem == null)
            {
                this.mBaseItem = ButterflyEnvironment.GetGame().GetItemManager().GetItem(this.BaseItem);
            }
            return this.mBaseItem;
        }

        internal Point GetPlacementPosition()
        {
            return this.placedPosition;
        }

        internal CatalogItem GetRareCatalogItem(int ItemId)
        {
            return ButterflyEnvironment.GetGame().GetCatalog().RaresItems[(uint) ItemId];
        }

        internal Room GetRoom()
        {
            if (this.mRoom == null)
            {
                this.mRoom = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.RoomId);
            }
            return this.mRoom;
        }

        internal void OnTrigger(RoomUser user)
        {
            if (this.itemTriggerEventHandler != null)
            {
                this.itemTriggerEventHandler(null, new ItemTriggeredArgs(user, this));
            }
        }

        internal void ProcessUpdates()
        {
            this.UpdateCounter--;
            if ((this.UpdateCounter <= 0) || this.IsTrans)
            {
                int num4;
                this.UpdateNeeded = false;
                this.UpdateCounter = 0;
                RoomUser roomUserByHabbo = null;
                RoomUser user2 = null;
                switch (this.GetBaseItem().InteractionType)
                {
                    case InteractionType.banzaicounter:
                        if (!string.IsNullOrEmpty(this.ExtraData))
                        {
                            num4 = 0;
                            try
                            {
                                num4 = int.Parse(this.ExtraData);
                            }
                            catch
                            {
                                Console.WriteLine("Error: Couldn't parse Banzai Timer extradata");
                            }
                            if (num4 > 0)
                            {
                                if (this.interactionCountHelper == 1)
                                {
                                    num4--;
                                    this.interactionCountHelper = 0;
                                    if (!this.GetRoom().GetBanzai().isBanzaiActive)
                                    {
                                        break;
                                    }
                                    this.ExtraData = num4.ToString();
                                    this.UpdateState();
                                }
                                else
                                {
                                    this.interactionCountHelper = (byte)(this.interactionCountHelper + 1);
                                }
                                this.UpdateCounter = 1;
                            }
                            else
                            {
                                this.UpdateCounter = 0;
                                this.GetRoom().GetBanzai().BanzaiEnd();
                            }
                        }
                        else
                        {
                            ServerMessage message = new ServerMessage(Outgoing.UpdateFloorItemExtraData);
                            message.AppendString(this.Id.ToString());
                            message.AppendInt32(0);
                            message.AppendString(this.ExtraData);
                        }
                        break;

                    case InteractionType.banzaitele:
                        this.ExtraData = string.Empty;
                        this.UpdateState();
                        break;

                    case InteractionType.banzaipuck:
                        if (this.interactionCount <= 4)
                        {
                            this.interactionCount = 0;
                            this.UpdateCounter = 0;
                            break;
                        }
                        this.interactionCount = (byte) (this.interactionCount + 1);
                        this.UpdateCounter = 1;
                        break;

                    case InteractionType.gld_wall:
                                              
                        break;
                    
                    case InteractionType.freezetimer:
                        if (!string.IsNullOrEmpty(this.ExtraData))
                        {
                            num4 = 0;
                            try
                            {
                                num4 = int.Parse(this.ExtraData);
                            }
                            catch
                            {
                            }
                            if (num4 > 0)
                            {
                                if (this.interactionCountHelper == 1)
                                {
                                    num4--;
                                    this.interactionCountHelper = 0;
                                    if (!this.GetRoom().GetFreeze().GameIsStarted)
                                    {
                                        break;
                                    }
                                    this.ExtraData = num4.ToString();
                                    this.UpdateState();
                                }
                                else
                                {
                                    this.interactionCountHelper = (byte) (this.interactionCountHelper + 1);
                                }
                                this.UpdateCounter = 1;
                            }
                            else
                            {
                                this.UpdateNeeded = false;
                                this.GetRoom().GetFreeze().StopGame();
                            }
                            break;
                        }
                        break;

                    case InteractionType.freezetile:
                        if (this.InteractingUser > 0)
                        {
                            this.ExtraData = "11000";
                            this.UpdateState(false, true);
                            this.GetRoom().GetFreeze().onFreezeTiles(this, this.freezePowerUp, this.InteractingUser);
                            this.InteractingUser = 0;
                            this.interactionCountHelper = 0;
                        }
                        break;

                    case InteractionType.scoreboard:
                        if (!string.IsNullOrEmpty(this.ExtraData))
                        {
                            num4 = 0;
                            try
                            {
                                num4 = int.Parse(this.ExtraData);
                            }
                            catch
                            {
                            }
                            if (num4 > 0)
                            {
                                if (this.interactionCountHelper == 1)
                                {
                                    num4--;
                                    this.interactionCountHelper = 0;
                                    this.ExtraData = num4.ToString();
                                    this.UpdateState();
                                }
                                else
                                {
                                    this.interactionCountHelper = (byte) (this.interactionCountHelper + 1);
                                }
                                this.UpdateCounter = 1;
                            }
                            else
                            {
                                this.UpdateCounter = 0;
                            }
                            break;
                        }
                        break;

                    case InteractionType.vendingmachine:
                        if (this.ExtraData == "1")
                        {
                            roomUserByHabbo = this.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(this.InteractingUser);
                            if (roomUserByHabbo != null)
                            {
                                roomUserByHabbo.UnlockWalking();
                                int num3 = this.GetBaseItem().VendingIds[ButterflyEnvironment.GetRandomNumber(0, this.GetBaseItem().VendingIds.Count - 1)];
                                roomUserByHabbo.CarryItem(num3);
                            }
                            this.InteractingUser = 0;
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        break;

                    case InteractionType.alert:
                        if (this.ExtraData == "1")
                        {
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        break;

                    case InteractionType.onewaygate:
                    case InteractionType.gld_gate:
                        roomUserByHabbo = null;
                        if (this.InteractingUser > 0)
                        {
                            roomUserByHabbo = this.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(this.InteractingUser);
                        }
                        if (((roomUserByHabbo != null) && (roomUserByHabbo.X == this.mX)) && (roomUserByHabbo.Y == this.mY))
                        {
                            this.ExtraData = "1";
                            roomUserByHabbo.MoveTo(this.SquareBehind);
                            this.ReqUpdate(1, false);
                            this.UpdateState(false, true);
                        }
                        else if ((roomUserByHabbo != null) && (roomUserByHabbo.Coordinate == this.SquareBehind))
                        {
                            roomUserByHabbo.UnlockWalking();
                            this.ExtraData = "0";
                            this.InteractingUser = 0;
                            this.UpdateState(false, true);
                        }
                        else if (this.ExtraData == "1")
                        {
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        if (roomUserByHabbo == null)
                        {
                            this.InteractingUser = 0;
                        }
                        break;

                    case InteractionType.loveshuffler:
                        if (!(this.ExtraData == "0"))
                        {
                            if (this.ExtraData != "-1")
                            {
                                this.ExtraData = "-1";
                            }
                        }
                        else
                        {
                            this.ExtraData = ButterflyEnvironment.GetRandomNumber(1, 4).ToString();
                            this.ReqUpdate(20, false);
                        }
                        this.UpdateState(false, true);
                        return;

                    case InteractionType.habbowheel:
                        this.ExtraData = ButterflyEnvironment.GetRandomNumber(1, 10).ToString();
                        this.UpdateState();
                        break;

                    case InteractionType.dice:
                        this.ExtraData = ButterflyEnvironment.GetRandomNumber(1, 6).ToString();
                        this.UpdateState();
                        break;

                    case InteractionType.bottle:
                        this.ExtraData = ButterflyEnvironment.GetRandomNumber(0, 7).ToString();
                        this.UpdateState();
                        break;

                    case InteractionType.teleport:
                    {
                        roomUserByHabbo = null;
                        user2 = null;
                        bool flag = false;
                        bool flag2 = false;
                        if (this.InteractingUser > 0)
                        {
                            roomUserByHabbo = this.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(this.InteractingUser);
                            if (roomUserByHabbo == null)
                            {
                                this.InteractingUser = 0;
                            }
                            else if (!(roomUserByHabbo.Coordinate == this.Coordinate))
                            {
                                if (roomUserByHabbo.Coordinate == this.SquareInFront)
                                {
                                    roomUserByHabbo.AllowOverride = true;
                                    flag = true;
                                    if (roomUserByHabbo.IsWalking && ((roomUserByHabbo.GoalX != this.mX) || (roomUserByHabbo.GoalY != this.mY)))
                                    {
                                        roomUserByHabbo.ClearMovement(true);
                                    }
                                    roomUserByHabbo.CanWalk = false;
                                    roomUserByHabbo.AllowOverride = true;
                                    roomUserByHabbo.MoveTo(this.Coordinate.X, this.Coordinate.Y, true);
                                }
                                else
                                {
                                    this.InteractingUser = 0;
                                }
                            }
                            else
                            {
                                roomUserByHabbo.AllowOverride = false;
                                if (!TeleHandler.IsTeleLinked(this.Id, this.mRoom))
                                {
                                    roomUserByHabbo.UnlockWalking();
                                    this.InteractingUser = 0;
                                }
                                else
                                {
                                    flag2 = true;
                                    uint linkedTele = TeleHandler.GetLinkedTele(this.Id, this.mRoom);
                                    uint teleRoomId = TeleHandler.GetTeleRoomId(linkedTele, this.mRoom);
                                    if (teleRoomId != this.RoomId)
                                    {
                                        if (((!roomUserByHabbo.IsBot && (roomUserByHabbo != null)) && ((roomUserByHabbo.GetClient() != null) && (roomUserByHabbo.GetClient().GetHabbo() != null))) && (roomUserByHabbo.GetClient().GetMessageHandler() != null))
                                        {
                                            roomUserByHabbo.GetClient().GetHabbo().IsTeleporting = true;
                                            roomUserByHabbo.GetClient().GetHabbo().TeleportingRoomID = teleRoomId;
                                            roomUserByHabbo.GetClient().GetHabbo().TeleporterId = linkedTele;
                                            roomUserByHabbo.GetClient().GetMessageHandler().PrepareRoomForUser(teleRoomId, "");
                                        }
                                    }
                                    else
                                    {
                                        RoomItem item = this.GetRoom().GetRoomItemHandler().GetItem(linkedTele);
                                        if (item != null)
                                        {
                                            roomUserByHabbo.SetPos(item.GetX, item.GetY, item.GetZ);
                                            roomUserByHabbo.SetRot(item.Rot, false);
                                            item.ExtraData = "2";
                                            item.UpdateState(false, true);
                                            item.InteractingUser2 = this.InteractingUser;
                                        }
                                        else
                                        {
                                            roomUserByHabbo.UnlockWalking();
                                        }
                                    }
                                    this.InteractingUser = 0;
                                }
                            }
                        }
                        if (this.InteractingUser2 > 0)
                        {
                            user2 = this.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(this.InteractingUser2);
                            if (user2 != null)
                            {
                                flag = true;
                                user2.UnlockWalking();
                                user2.MoveTo(this.SquareInFront);
                            }
                            this.InteractingUser2 = 0;
                        }
                        if (flag)
                        {
                            if (this.ExtraData != "1")
                            {
                                this.ExtraData = "1";
                                this.UpdateState(false, true);
                            }
                        }
                        else if (flag2)
                        {
                            if (this.ExtraData != "2")
                            {
                                this.ExtraData = "2";
                                this.UpdateState(false, true);
                            }
                        }
                        else if (this.ExtraData != "0")
                        {
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        this.ReqUpdate(1, false);
                        return;
                    }
                    case InteractionType.banzaifloor:
                        if (this.value != 3)
                        {
                            break;
                        }
                        if (this.interactionCountHelper == 1)
                        {
                            this.interactionCountHelper = 0;
                            switch (this.team)
                            {
                                case Team.red:
                                    this.ExtraData = "5";
                                    break;

                                case Team.green:
                                    this.ExtraData = "8";
                                    break;

                                case Team.blue:
                                    this.ExtraData = "11";
                                    break;

                                case Team.yellow:
                                    this.ExtraData = "14";
                                    break;
                            }
                        }
                        else
                        {
                            this.ExtraData = "";
                            this.interactionCountHelper = (byte) (this.interactionCountHelper + 1);
                        }
                        this.UpdateState();
                        this.interactionCount = (byte) (this.interactionCount + 1);
                        if (this.interactionCount < 0x10)
                        {
                            this.UpdateCounter = 1;
                        }
                        else
                        {
                            this.UpdateCounter = 0;
                        }
                        return;
                }
            }
        }

        internal void refreshItem()
        {
            this.mBaseItem = null;
        }

        internal void ReqUpdate(int Cycles, bool setUpdate)
        {
            this.UpdateCounter = Cycles;
            if (setUpdate)
            {
                this.UpdateNeeded = true;
            }
        }

        internal void Serialize(ServerMessage Message, int UserId)
        {
            if (this.GetBaseItem().Name == "boutique_mannequin1")
            {
                Message.AppendInt32(this.Id);
                Message.AppendInt32(this.GetBaseItem().SpriteId);
                Message.AppendInt32(this.mX);
                Message.AppendInt32(this.mY);
                Message.AppendInt32(this.Rot);
                Message.AppendString(string.Format("{0:0.00}", TextHandling.GetString(this.mZ)));
                Message.AppendInt32(0);
                Message.AppendInt32(1);
                Message.AppendInt32(3);


                if (ExtraData != "")
                {
                    Message.AppendString("GENDER");
                    Message.AppendString(ExtraData.Split(Convert.ToChar(9))[1]);
                    Message.AppendString("FIGURE");
                    Message.AppendString(ExtraData.Split(Convert.ToChar(9))[2]);
                    Message.AppendString("OUTFIT_NAME");
                    Message.AppendString(ExtraData.Split(Convert.ToChar(9))[0]);
                }
                else
                {
                    Message.AppendString("GENDER");
                    Message.AppendString("m");
                    Message.AppendString("FIGURE");
                    Message.AppendString("ch-210-62.lg-270-62");
                    Message.AppendString("OUTFIT_NAME");
                    Message.AppendString("");
                }
                Message.AppendInt32(-1);
                Message.AppendInt32(2);
                Message.AppendInt32(UserId);
            }
            else
            {
                if (this.IsFloorItem)
                {
                    Message.AppendInt32(this.Id);
                    Message.AppendInt32(this.GetBaseItem().SpriteId);
                    Message.AppendInt32(this.mX);
                    Message.AppendInt32(this.mY);
                    Message.AppendInt32(this.Rot);
                    Message.AppendString(string.Format("{0:0.00}", TextHandling.GetString(this.mZ)));
                    //Message.AppendString("0.0");

                    if (this.GetBaseItem().Name == "ads_mpu_720" || this.GetBaseItem().Name == "ads_background" || this.GetBaseItem().Name == "ads_mpu_300" || this.GetBaseItem().Name == "ads_mpu_160")
                    {

                        Message.AppendInt32(0);
                        Message.AppendInt32(1);
                        if (ExtraData != "")
                        {
                            Message.AppendInt32(ExtraData.Split(Convert.ToChar(9)).Length / 2);

                            for (int i = 0; i <= ExtraData.Split(Convert.ToChar(9)).Length - 1; i++)
                            {
                                Message.AppendString(ExtraData.Split(Convert.ToChar(9))[i]);
                            }
                        }
                        else
                        {
                            Message.AppendInt32(0);
                        }

                    }

                    else if (this.GetBaseItem().Name.Substring(0, GetBaseItem().Name.Length - 1).Contains("present_wrap"))
                    {
                        int i = 0;
                        if (this.ExtraData.Contains(Convert.ToChar(5).ToString()))
                        {
                            int num2 = int.Parse(this.ExtraData.Split(new char[] { '\x0005' })[1]);
                            int num3 = int.Parse(this.ExtraData.Split(new char[] { '\x0005' })[2]);
                            i = (num2 * 0x3e8) + num3;
                        }
                        Message.AppendInt32(i);
                        if (this.ExtraData.Contains(Convert.ToChar(5).ToString()))
                        {
                            Habbo habbo = ButterflyEnvironment.getHabboForId((uint)int.Parse(this.ExtraData.Split(new char[] { ';' })[0]));
                            if (habbo != null)
                            {
                                Message.AppendInt32(1);
                                Message.AppendInt32(6);
                                Message.AppendString("EXTRA_PARAM");
                                Message.AppendString("");
                                Message.AppendString("MESSAGE");
                                Message.AppendString(this.ExtraData.Split(new char[] { ';' })[1].Split(new char[] { '\x0005' })[0]);
                                Message.AppendString("PURCHASER_NAME");
                                Message.AppendString(habbo.Username);
                                Message.AppendString("PURCHASER_FIGURE");
                                Message.AppendString(habbo.Look);
                                Message.AppendString("PRODUCT_CODE");
                                Message.AppendString("");
                                Message.AppendString("state");
                                Message.AppendString(this.MagicRemove ? "1" : "0");
                            }
                            else
                            {
                                Message.AppendInt32(0);
                            }
                        }
                        else
                        {
                            Message.AppendInt32(0);
                        }
                    }
                    else
                    {
                        Message.AppendInt32(0);
                        if (this.RareId > 0)
                        {
                            Message.AppendString("");
                            Message.AppendBoolean(true);
                            Message.AppendBoolean(false);
                        }
                        else if (this.mBaseItem.AllowGroupItem)
                        {
                            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(int.Parse(this.Guilds_Data_String.Split(new char[] { ';' })[0]));
                            if (guild == null)
                            {
                                Message.AppendInt32(0);
                            }
                            else
                            {
                                Message.AppendInt32(2);
                                Message.AppendInt32(5);
                                Message.AppendString(this.ExtraData);
                                Message.AppendString(guild.Id.ToString());
                                Message.AppendString(guild.Image);
                                Message.AppendString(this.Guilds_Data_String.Split(new char[] { ';' })[1]);
                                Message.AppendString(this.Guilds_Data_String.Split(new char[] { ';' })[2]);
                            }
                        }
                        else
                        {
                            Message.AppendInt32(0);
                        }
                        if (this.mBaseItem.InteractionType == InteractionType.trophy)
                        {
                            Message.AppendString(this.ExtraData);
                        }
                        else if (!this.mBaseItem.AllowGroupItem)
                        {
                            Message.AppendString(this.ExtraData);
                        }
                        if (this.RareId > 0)
                        {
                            Message.AppendInt32(this.RareId);
                            Message.AppendInt32(this.GetRareCatalogItem((int)this.BaseItem).LimitedStack);
                        }
                    }
                    Message.AppendInt32(-1);
                    Message.AppendInt32(1);//era 1
                    Message.AppendInt32(UserId);

                    if (!this.mRoom.MovedThinksObH && (this.GetBaseItem().InteractionType == InteractionType.obstacle_h))
                    {
                        if ((this.Rot == 2) || (this.Rot == 6))
                        {
                            this.mX = this.GetX + 1;
                        }
                        else if ((this.Rot == 0) || (this.Rot == 4))
                        {
                            this.mY = this.GetY + 1;
                        }
                    }
                }
                else if (this.IsWallItem)
                {
                    Message.AppendString(this.Id + string.Empty);
                    Message.AppendInt32(this.GetBaseItem().SpriteId);
                    Message.AppendString(this.wallCoord.ToString());
                    if (this.GetBaseItem().InteractionType == InteractionType.postit)
                    {
                        Message.AppendString(this.ExtraData.Split(new char[] { ' ' })[0]);
                    }
                    else
                    {
                        Message.AppendString(this.ExtraData);
                    }
                    Message.AppendInt32(-1);
                    Message.AppendInt32(-1);
                    Message.AppendInt32(UserId);
                }
            }
        }

        internal void SetState(int pX, int pY, double pZ, Dictionary<int, ThreeDCoord> Tiles)
        {
            this.mX = pX;
            this.mY = pY;
            if (!double.IsInfinity(pZ))
            {
                this.mZ = pZ;
            }
            this.mAffectedPoints = Tiles;
        }

        internal void UpdateState()
        {
            this.UpdateState(true, true);
        }

        internal void UpdateState(bool inDb, bool inRoom)
        {

            
            if (this.GetRoom() != null)
            {
                if (inDb)
                {
                    this.GetRoom().GetRoomItemHandler().UpdateItem(this);
                }
                if (inRoom)
                {
                    ServerMessage message = new ServerMessage();
                    if (this.RareId > 0) //PROVISORY FIX
                    {
                        message.Init(Outgoing.UpdateItemOnRoom);
                        this.Serialize(message, (int)this.placedBy);
                    }
                    else if (this.IsFloorItem)
                    {
                        message.Init(Outgoing.UpdateFloorItemExtraData);
                        message.AppendString(this.Id.ToString());
                        message.AppendInt32(0);
                        message.AppendString(this.ExtraData);
                    }
                    else
                    {
                        message.Init(Outgoing.UpdateWallItemOnRoom);
                        this.Serialize(message, this.GetRoom().OwnerId);
                    }
                    this.GetRoom().SendMessage(message);
                }
            }
        }

        internal void UserWalksOffFurni(RoomUser user)
        {
            if (this.OnUserWalksOffFurni != null)
            {
                this.OnUserWalksOffFurni(this, new UserWalksOnArgs(user));
            }
        }

        internal void UserWalksOnFurni(RoomUser user)
        {
            if (this.OnUserWalksOnFurni != null)
            {
                this.OnUserWalksOnFurni(this, new UserWalksOnArgs(user));
            }
        }

        internal Point Coordinate
        {
            get
            {
                return new Point(this.mX, this.mY);
            }
        }

        internal Dictionary<int, ThreeDCoord> GetAffectedTiles
        {
            get
            {
                return this.mAffectedPoints;
            }
        }

        internal List<Point> GetCoords
        {
            get
            {
                List<Point> list = new List<Point> {
                    this.Coordinate
                };
                foreach (ThreeDCoord coord in this.mAffectedPoints.Values)
                {
                    list.Add(new Point(coord.X, coord.Y));
                }
                return list;
            }
        }

        internal int GetX
        {
            get
            {
                return this.mX;
            }
        }

        internal int GetY
        {
            get
            {
                return this.mY;
            }
        }

        internal double GetZ
        {
            get
            {
                return this.mZ;
            }
        }

        internal FurniInteractor Interactor
        {
            get
            {
                switch (this.GetBaseItem().InteractionType)
                {
                    case InteractionType.gate:
                        return new InteractorGate(this.GetBaseItem().Modes);
                    
                    case InteractionType.mannequin:
                        return new InteractorMannequin();
                    
                    case InteractionType.scoreboard:
                        return new InteractorScoreboard();

                    case InteractionType.vendingmachine:
                        return new InteractorVendor();

                    case InteractionType.alert:
                        return new InteractorAlert();

                    case InteractionType.onewaygate:
                        return new InteractorOneWayGate();

                    case InteractionType.gld_gate:
                        return new InteractorOneWayGate();

                    case InteractionType.loveshuffler:
                        return new InteractorLoveShuffler();

                    case InteractionType.habbowheel:
                        return new InteractorHabboWheel();

                    case InteractionType.dice:
                        return new InteractorDice();

                    case InteractionType.bottle:
                        return new InteractorSpinningBottle();

                    case InteractionType.teleport:
                        return new InteractorTeleport();

                    case InteractionType.football:
                        return new InteractorFootball();

                    case InteractionType.footballcountergreen:
                    case InteractionType.footballcounteryellow:
                    case InteractionType.footballcounterblue:
                    case InteractionType.footballcounterred:
                        return new InteractorScoreCounter();

                    case InteractionType.banzaifloor:
                        return new InteractorNone();

                    case InteractionType.banzaiscoreblue:
                    case InteractionType.banzaiscorered:
                    case InteractionType.banzaiscoreyellow:
                    case InteractionType.banzaiscoregreen:
                        return new InteractorBanzaiScoreCounter();

                    case InteractionType.banzaicounter:
                        return new InteractorBanzaiTimer();

                    case InteractionType.banzaipuck:
                        return new InteractorBanzaiPuck();

                    case InteractionType.freezetimer:
                        return new InteractorFreezeTimer();

                    case InteractionType.freezetileblock:
                    case InteractionType.freezetile:
                        return new InteractorFreezeTile();

                    case InteractionType.jukebox:
                        return new InteractorJukebox();

                    case InteractionType.triggertimer:
                    case InteractionType.triggerroomenter:
                    case InteractionType.triggergameend:
                    case InteractionType.triggergamestart:
                    case InteractionType.triggerrepeater:
                    case InteractionType.triggeronusersay:
                    case InteractionType.triggerscoreachieved:
                    case InteractionType.triggerstatechanged:
                    case InteractionType.triggerwalkonfurni:
                    case InteractionType.triggerwalkofffurni:
                    case InteractionType.actiongivescore:
                    case InteractionType.actionposreset:
                    case InteractionType.actionmoverotate:
                    case InteractionType.actionresettimer:
                    case InteractionType.actionshowmessage:
                    case InteractionType.actionteleportto:
                    case InteractionType.actiontogglestate:
                    case InteractionType.conditionfurnishaveusers:
                    case InteractionType.conditionstatepos:
                    case InteractionType.conditiontimelessthan:
                    case InteractionType.conditiontimemorethan:
                    case InteractionType.conditiontriggeronfurni:
                        return new WiredInteractor();

                    case InteractionType.wire:
                    case InteractionType.wireCenter:
                    case InteractionType.wireCorner:
                    case InteractionType.wireSplitter:
                    case InteractionType.wireStandard:
                        return new InteractorIgnore();
                }
                return new InteractorGenericSwitch(this.GetBaseItem().Modes);
            }
        }

        internal bool IsFloorItem
        {
            get
            {
                return this.mIsFloorItem;
            }
        }

        internal bool IsRoller
        {
            get
            {
                return this.mIsRoller;
            }
        }

        internal bool IsWallItem
        {
            get
            {
                return this.mIsWallItem;
            }
        }

        internal Point SquareBehind
        {
            get
            {
                Point point = new Point(this.mX, this.mY);
                if (this.Rot == 0)
                {
                    point.Y++;
                    return point;
                }
                if (this.Rot == 2)
                {
                    point.X--;
                    return point;
                }
                if (this.Rot == 4)
                {
                    point.Y--;
                    return point;
                }
                if (this.Rot == 6)
                {
                    point.X++;
                }
                return point;
            }
        }

        internal Point SquareInFront
        {
            get
            {
                Point point = new Point(this.mX, this.mY);
                if (this.Rot == 0)
                {
                    point.Y--;
                    return point;
                }
                if (this.Rot == 2)
                {
                    point.X++;
                    return point;
                }
                if (this.Rot == 4)
                {
                    point.Y++;
                    return point;
                }
                if (this.Rot == 6)
                {
                    point.X--;
                }
                return point;
            }
        }

        internal double TotalHeight
        {
            get
            {
                return (this.mZ + this.GetBaseItem().Height);
            }
        }

        internal bool UpdateNeeded
        {
            get
            {
                return this.updateNeeded;
            }
            set
            {
                if (value)
                {
                    this.GetRoom().GetRoomItemHandler().QueueRoomItemUpdate(this);
                }
                this.updateNeeded = value;
            }
        }
    }
}

