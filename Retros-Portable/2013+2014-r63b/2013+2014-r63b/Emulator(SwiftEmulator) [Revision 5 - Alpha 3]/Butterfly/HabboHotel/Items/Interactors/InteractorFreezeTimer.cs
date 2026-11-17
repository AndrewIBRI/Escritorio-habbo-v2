namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using System;

    internal class InteractorFreezeTimer : FurniInteractor
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
                if (Request == 2)
                {
                    if (Item.pendingReset && (num > 0))
                    {
                        num = 0;
                        Item.pendingReset = false;
                    }
                    else
                    {
                        if (((((num == 0) || (num == 30)) || ((num == 60) || (num == 120))) || ((num == 180) || (num == 300))) || (num == 600))
                        {
                            if (num == 0)
                            {
                                num = 30;
                            }
                            else if (num == 30)
                            {
                                num = 60;
                            }
                            else if (num == 60)
                            {
                                num = 120;
                            }
                            else if (num == 120)
                            {
                                num = 180;
                            }
                            else if (num == 180)
                            {
                                num = 300;
                            }
                            else if (num == 300)
                            {
                                num = 600;
                            }
                            else if (num == 600)
                            {
                                num = 0;
                            }
                        }
                        else
                        {
                            num = 0;
                        }
                        Item.UpdateNeeded = false;
                    }
                }
                else if ((Request == 1) && !Item.GetRoom().GetFreeze().GameIsStarted)
                {
                    Item.UpdateNeeded = !Item.UpdateNeeded;
                    if (Item.UpdateNeeded)
                    {
                        Item.GetRoom().GetFreeze().StartGame();
                    }
                    Item.pendingReset = true;
                }
                Item.ExtraData = num.ToString();
                Item.UpdateState();
            }
        }
    }
}

