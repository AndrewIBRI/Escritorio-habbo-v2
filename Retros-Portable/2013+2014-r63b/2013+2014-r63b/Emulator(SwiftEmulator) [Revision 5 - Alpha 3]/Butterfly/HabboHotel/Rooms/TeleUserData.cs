namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.Messages;
    using System;

    internal class TeleUserData
    {
        private GameClientMessageHandler mHandler;
        private Habbo mUserRefference;
        private uint RoomId;
        private uint TeleId;

        internal TeleUserData(GameClientMessageHandler pHandler, Habbo pUserRefference, uint RoomId, uint TeleId)
        {
            this.mHandler = pHandler;
            this.mUserRefference = pUserRefference;
            this.RoomId = RoomId;
            this.TeleId = TeleId;
        }

        internal void Execute()
        {
            if ((this.mHandler != null) && (this.mUserRefference != null))
            {
                this.mUserRefference.IsTeleporting = true;
                this.mUserRefference.TeleporterId = this.TeleId;
                this.mHandler.PrepareRoomForUser(this.RoomId, "");
            }
        }
    }
}

