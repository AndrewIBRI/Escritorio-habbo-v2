namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using System;

    internal abstract class FurniInteractor
    {
        protected FurniInteractor()
        {
        }

        internal abstract void OnPlace(GameClient Session, RoomItem Item);
        internal abstract void OnRemove(GameClient Session, RoomItem Item);
        internal abstract void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights);
    }
}

