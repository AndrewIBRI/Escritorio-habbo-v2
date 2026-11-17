namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using System;

    internal class InteractorHabboWheel : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "-1";
            Item.ReqUpdate(10, true);
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "-1";
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (UserHasRights && (Item.ExtraData != "-1"))
            {
                Item.ExtraData = "-1";
                Item.UpdateState();
                Item.ReqUpdate(10, true);
            }
        }
    }
}

