namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using System;

    internal class InteractorTeleport : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
            RoomUser roomUserByHabbo;
            Item.ExtraData = "0";
            if (Item.InteractingUser != 0)
            {
                roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Item.InteractingUser);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.ClearMovement(true);
                    roomUserByHabbo.AllowOverride = false;
                    roomUserByHabbo.CanWalk = true;
                }
                Item.InteractingUser = 0;
            }
            if (Item.InteractingUser2 != 0)
            {
                roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Item.InteractingUser2);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.ClearMovement(true);
                    roomUserByHabbo.AllowOverride = false;
                    roomUserByHabbo.CanWalk = true;
                }
                Item.InteractingUser2 = 0;
            }
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
            RoomUser roomUserByHabbo;
            Item.ExtraData = "0";
            if (Item.InteractingUser != 0)
            {
                roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Item.InteractingUser);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.UnlockWalking();
                }
                Item.InteractingUser = 0;
            }
            if (Item.InteractingUser2 != 0)
            {
                roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Item.InteractingUser2);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.UnlockWalking();
                }
                Item.InteractingUser2 = 0;
            }
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if ((((Item != null) && (Item.GetRoom() != null)) && (Session != null)) && (Session.GetHabbo() != null))
            {
                RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    if ((roomUserByHabbo.Coordinate == Item.Coordinate) || (roomUserByHabbo.Coordinate == Item.SquareInFront))
                    {
                        if (Item.InteractingUser == 0)
                        {
                            Item.InteractingUser = roomUserByHabbo.GetClient().GetHabbo().Id;
                        }
                    }
                    else if (roomUserByHabbo.CanWalk)
                    {
                        roomUserByHabbo.MoveTo(Item.SquareInFront);
                    }
                }
            }
        }
    }
}

