namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using System;

    internal class InteractorFreezeTile : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (((Session != null) && (Session.GetHabbo() != null)) && (Item.InteractingUser <= 0))
            {
                string username = Session.GetHabbo().Username;
                RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(username);
                roomUserByHabbo.GoalX = Item.GetX;
                roomUserByHabbo.GoalY = Item.GetY;
                if (roomUserByHabbo.team != Team.none)
                {
                    roomUserByHabbo.throwBallAtGoal = true;
                }
            }
        }
    }
}

