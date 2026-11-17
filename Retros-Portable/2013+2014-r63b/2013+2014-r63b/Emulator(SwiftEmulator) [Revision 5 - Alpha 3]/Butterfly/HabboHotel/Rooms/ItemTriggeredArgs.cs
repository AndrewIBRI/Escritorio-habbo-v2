namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly.HabboHotel.Items;
    using System;

    public class ItemTriggeredArgs : EventArgs
    {
        internal readonly RoomItem TriggeringItem;
        internal readonly RoomUser TriggeringUser;

        public ItemTriggeredArgs(RoomUser user, RoomItem item)
        {
            this.TriggeringUser = user;
            this.TriggeringItem = item;
        }
    }
}

