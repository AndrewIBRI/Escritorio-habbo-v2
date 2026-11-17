namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using System;

    internal class InteractorGenericSwitch : FurniInteractor
    {
        private int Modes;

        internal InteractorGenericSwitch(int Modes)
        {
            this.Modes = Modes - 1;
            if (this.Modes < 0)
            {
                this.Modes = 0;
            }
        }

        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (UserHasRights && (this.Modes != 0))
            {
                int num = 0;
                int num2 = 0;
                try
                {
                    num = int.Parse(Item.ExtraData);
                }
                catch
                {
                }
                if (num <= 0)
                {
                    num2 = 1;
                }
                else if (num >= this.Modes)
                {
                    num2 = 0;
                }
                else
                {
                    num2 = num + 1;
                }
                Item.ExtraData = num2.ToString();
                Item.UpdateState();
            }
        }
    }
}

