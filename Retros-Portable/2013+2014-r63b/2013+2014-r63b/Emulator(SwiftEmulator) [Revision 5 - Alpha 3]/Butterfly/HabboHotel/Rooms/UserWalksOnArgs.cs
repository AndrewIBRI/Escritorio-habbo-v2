namespace Butterfly.HabboHotel.Rooms
{
    using System;

    public class UserWalksOnArgs : EventArgs
    {
        internal readonly RoomUser user;

        public UserWalksOnArgs(RoomUser user)
        {
            this.user = user;
        }
    }
}

