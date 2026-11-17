namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct UserWalksFurniValue
    {
        internal readonly RoomUser user;
        internal readonly RoomItem item;
        public UserWalksFurniValue(RoomUser user, RoomItem item)
        {
            this.user = user;
            this.item = item;
        }
    }
}

