namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using System;

    internal class InteractorLoveShuffler : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "-1";
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "-1";
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (UserHasRights && (Item.ExtraData != "0"))
            {
                Item.ExtraData = "0";
                Item.UpdateState(false, true);
                Item.ReqUpdate(10, true);
            }
        }
    }
}

