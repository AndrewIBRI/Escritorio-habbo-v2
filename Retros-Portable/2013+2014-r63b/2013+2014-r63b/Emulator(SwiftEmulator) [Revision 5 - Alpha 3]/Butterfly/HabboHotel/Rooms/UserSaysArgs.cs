namespace Butterfly.HabboHotel.Rooms
{
    using System;

    public class UserSaysArgs : EventArgs
    {
        internal readonly string message;
        internal readonly RoomUser user;

        public UserSaysArgs(RoomUser user, string message)
        {
            this.user = user;
            this.message = message;
        }
    }
}

