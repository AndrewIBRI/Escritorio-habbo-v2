namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using System;

    internal class InteractorOneWayGate : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
            if (Item.InteractingUser != 0)
            {
                RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Item.InteractingUser);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.ClearMovement(true);
                    roomUserByHabbo.UnlockWalking();
                }
                Item.InteractingUser = 0;
            }
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
            if (Item.InteractingUser != 0)
            {
                RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Item.InteractingUser);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.ClearMovement(true);
                    roomUserByHabbo.UnlockWalking();
                }
                Item.InteractingUser = 0;
            }
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (Session != null)
            {
                if (Item.GetBaseItem().InteractionType == InteractionType.onewaygate)
                {
                    RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (roomUserByHabbo != null)
                    {
                        if ((roomUserByHabbo.Coordinate != Item.SquareInFront) && roomUserByHabbo.CanWalk)
                        {
                            roomUserByHabbo.MoveTo(Item.SquareInFront);
                        }
                        else if (Item.GetRoom().GetGameMap().CanWalk(Item.SquareBehind.X, Item.SquareBehind.Y, roomUserByHabbo.AllowOverride) && (Item.InteractingUser == 0))
                        {
                            Item.InteractingUser = roomUserByHabbo.HabboId;
                            roomUserByHabbo.CanWalk = false;
                            if (roomUserByHabbo.IsWalking && ((roomUserByHabbo.GoalX != Item.SquareInFront.X) || (roomUserByHabbo.GoalY != Item.SquareInFront.Y)))
                            {
                                roomUserByHabbo.ClearMovement(true);
                            }
                            roomUserByHabbo.AllowOverride = true;
                            roomUserByHabbo.MoveTo(Item.Coordinate);
                            Item.ReqUpdate(4, true);

                        }
                    }
                }

                if (Item.GetBaseItem().InteractionType == InteractionType.gld_gate)
                {
                    if (Session.GetHabbo().ImGuilds.Contains(Convert.ToInt32(Item.Guilds_Data[0])))
                    {
                        RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                        if (roomUserByHabbo != null)
                        {
                            if (Item.GetRoom().GetGameMap().CanWalk(Item.SquareBehind.X, Item.SquareBehind.Y, roomUserByHabbo.AllowOverride) && roomUserByHabbo.Coordinate == Item.SquareInFront && (Item.InteractingUser == 0))
                            {
                                Item.ExtraData = "1";
                                Item.UpdateState(false, true);
                                System.Threading.Thread.Sleep(1000);
                                Item.InteractingUser = roomUserByHabbo.HabboId;
                                roomUserByHabbo.CanWalk = false;
                                roomUserByHabbo.AllowOverride = true;
                                roomUserByHabbo.MoveTo(Item.Coordinate);
                                Item.ExtraData = "0";
                                Item.UpdateState(false, true);
                                roomUserByHabbo.MoveTo(Item.SquareBehind);
                                roomUserByHabbo.CanWalk = true;
                                System.Threading.Thread.Sleep(1000);
                                roomUserByHabbo.AllowOverride = false;
                                Item.InteractingUser = 0;
                            }
                            else if (Item.GetRoom().GetGameMap().CanWalk(Item.SquareInFront.X, Item.SquareInFront.Y, roomUserByHabbo.AllowOverride) && roomUserByHabbo.Coordinate == Item.SquareBehind && (Item.InteractingUser == 0))
                            {
                                Item.ExtraData = "1";
                                Item.UpdateState(false, true);
                                System.Threading.Thread.Sleep(1000);
                                Item.InteractingUser = roomUserByHabbo.HabboId;
                                roomUserByHabbo.CanWalk = false;
                                roomUserByHabbo.AllowOverride = true;
                                roomUserByHabbo.MoveTo(Item.Coordinate);
                                Item.ExtraData = "0";
                                Item.UpdateState(false, true);
                                roomUserByHabbo.MoveTo(Item.SquareInFront);
                                roomUserByHabbo.CanWalk = true;
                                System.Threading.Thread.Sleep(1000);
                                roomUserByHabbo.AllowOverride = false;
                                Item.InteractingUser = 0;
                            }
                        }
                    }
                    else
                    {
                        Session.SendNotif("Only groupmembers can enter this gate.");
                    }
                }
            }
        }
    }
}

