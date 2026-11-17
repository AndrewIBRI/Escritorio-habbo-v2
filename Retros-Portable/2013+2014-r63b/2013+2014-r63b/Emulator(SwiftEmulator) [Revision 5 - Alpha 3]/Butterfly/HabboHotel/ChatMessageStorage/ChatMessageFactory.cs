namespace Butterfly.HabboHotel.ChatMessageStorage
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Rooms;
    using System;

    internal class ChatMessageFactory
    {
        internal static ChatMessage CreateMessage(string message, GameClient user, Room room)
        {
            uint id = user.GetHabbo().Id;
            string username = user.GetHabbo().Username;
            uint roomId = room.RoomId;
            string name = room.Name;
            bool isPublic = room.IsPublic;
            return new ChatMessage(id, username, roomId, name, isPublic, message, DateTime.Now);
        }
    }
}

