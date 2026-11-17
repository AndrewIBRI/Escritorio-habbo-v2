namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using System;

    internal class InteractorScoreboard : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (UserHasRights)
            {
                int num = 0;
                if (!string.IsNullOrEmpty(Item.ExtraData))
                {
                    try
                    {
                        num = int.Parse(Item.ExtraData);
                    }
                    catch
                    {
                    }
                }
                if (Request == 1)
                {
                    if (Item.pendingReset && (num > 0))
                    {
                        num = 0;
                        Item.pendingReset = false;
                    }
                    else
                    {
                        num += 60;
                        Item.UpdateNeeded = false;
                    }
                }
                else if (Request == 2)
                {
                    Item.UpdateNeeded = !Item.UpdateNeeded;
                    Item.pendingReset = true;
                }
                Item.ExtraData = num.ToString();
                Item.UpdateState();
            }
        }
    }
}

