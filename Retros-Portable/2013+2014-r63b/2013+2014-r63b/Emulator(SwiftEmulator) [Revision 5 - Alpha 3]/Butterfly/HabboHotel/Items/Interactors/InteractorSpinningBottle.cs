namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using System;

    internal class InteractorSpinningBottle : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
            Item.UpdateState(true, false);
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (Item.ExtraData != "-1")
            {
                Item.ExtraData = "-1";
                Item.UpdateState(false, true);
                Item.ReqUpdate(3, true);
            }
        }
    }
}

