namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Pathfinding;
    using Butterfly.HabboHotel.Rooms;
    using System;

    internal class InteractorVendor : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
            if (Item.InteractingUser > 0)
            {
                RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Item.InteractingUser);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.CanWalk = true;
                }
            }
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
            if (Item.InteractingUser > 0)
            {
                RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Item.InteractingUser);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.CanWalk = true;
                }
            }
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if ((((Item.ExtraData != "1") && (Item.GetBaseItem().VendingIds.Count >= 1)) && (Item.InteractingUser == 0)) && (Session != null))
            {
                RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    if (!Gamemap.TilesTouching(roomUserByHabbo.X, roomUserByHabbo.Y, Item.GetX, Item.GetY))
                    {
                        roomUserByHabbo.MoveTo(Item.SquareInFront);
                    }
                    else
                    {
                        Item.InteractingUser = Session.GetHabbo().Id;
                        roomUserByHabbo.CanWalk = false;
                        roomUserByHabbo.ClearMovement(true);
                        roomUserByHabbo.SetRot(Rotation.Calculate(roomUserByHabbo.X, roomUserByHabbo.Y, Item.GetX, Item.GetY), false);
                        Item.ReqUpdate(2, true);
                        Item.ExtraData = "1";
                        Item.UpdateState(false, true);
                    }
                }
            }
        }
    }
}

