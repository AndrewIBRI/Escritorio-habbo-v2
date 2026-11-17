namespace Butterfly.HabboHotel.ChatMessageStorage
{
    using Butterfly.Messages;
    using System;

    internal class ChatMessage
    {
        private readonly string message;
        internal readonly uint roomID;
        internal readonly bool roomIsPublic;
        internal readonly string roomName;
        private readonly DateTime timeSpoken;
        private readonly uint userID;
        private readonly string username;

        public ChatMessage(uint userID, string username, uint roomID, string roomName, bool roomIsPublic, string message, DateTime timeSpoken)
        {
            this.userID = userID;
            this.username = username;
            this.roomID = roomID;
            this.message = message;
            this.timeSpoken = timeSpoken;
            this.roomName = roomName;
            this.roomIsPublic = roomIsPublic;
        }

        internal void Serialize(ref ServerMessage packet)
        {
            packet.AppendInt32(this.timeSpoken.Hour);
            packet.AppendInt32(this.timeSpoken.Minute);
            packet.AppendString(this.username);
            packet.AppendString(this.message);
        }
    }
}

