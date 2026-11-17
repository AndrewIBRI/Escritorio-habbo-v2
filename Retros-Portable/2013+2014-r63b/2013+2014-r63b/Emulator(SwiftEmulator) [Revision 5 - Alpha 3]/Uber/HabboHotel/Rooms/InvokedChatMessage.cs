namespace Uber.HabboHotel.Rooms
{
    using Butterfly.HabboHotel.Rooms;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct InvokedChatMessage
    {
        internal RoomUser user;
        internal string message;
        internal bool shout;
        internal int TextColor;
        public InvokedChatMessage(RoomUser user, string message, bool shout, int textcolor)
        {
            this.user = user;
            this.message = message;
            this.shout = shout;
            this.TextColor = textcolor;
        }

        internal void Dispose()
        {
            this.user = null;
            this.message = null;
        }
    }
}

