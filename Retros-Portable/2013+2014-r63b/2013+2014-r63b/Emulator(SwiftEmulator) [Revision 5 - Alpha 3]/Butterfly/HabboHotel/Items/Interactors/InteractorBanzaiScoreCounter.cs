namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms.Games;
    using System;

    internal class InteractorBanzaiScoreCounter : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
            if (Item.team != Team.none)
            {
                Item.ExtraData = Item.GetRoom().GetGameManager().Points[(int) Item.team].ToString();
                Item.UpdateState(false, true);
            }
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (UserHasRights)
            {
                Item.GetRoom().GetGameManager().Points[(int) Item.team] = 0;
                Item.ExtraData = "0";
                Item.UpdateState();
            }
        }

        private static void UpdateTeamPoints(int points, Team team, RoomItem Item)
        {
            if (team != Team.none)
            {
                Item.GetRoom().GetGameManager().Points[(int) Item.team] = points;
                Item.UpdateState();
            }
        }
    }
}

