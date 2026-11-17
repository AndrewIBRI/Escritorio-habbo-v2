namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.Messages;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class RoomEvent
    {
        internal int Category;
        internal string Description;
        internal string Name;
        internal uint RoomId;
        internal string StartTime;
        internal ArrayList Tags;

        internal RoomEvent(uint RoomId, string Name, string Description, int Category, List<string> tags)
        {
            this.RoomId = RoomId;
            this.Name = Name;
            this.Description = Description;
            this.Category = Category;
            this.StartTime = DateTime.Now.ToShortTimeString();
            this.Tags = new ArrayList();
            if (tags != null)
            {
                foreach (string str in tags)
                {
                    this.Tags.Add(str);
                }
            }
        }

        internal ServerMessage Serialize(GameClient Session)
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomEvent);
            message.AppendInt32(Session.GetHabbo().Id);
            message.AppendString(Session.GetHabbo().Username);
            message.AppendInt32(this.RoomId);
            message.AppendInt32(this.Category);
            message.AppendString(this.Name);
            message.AppendString(this.Description);
            message.AppendString(this.StartTime);
            message.AppendInt32(this.Tags.Count);
            foreach (string str in this.Tags.ToArray())
            {
                message.AppendString(str);
            }
            return message;
        }

        internal void SerializeTo(RoomData data, ServerMessage Message)
        {
            Message.AppendInt32(data.OwnerId);
            Message.AppendString(data.Owner);
            Message.AppendInt32(this.RoomId);
            Message.AppendInt32(this.Category);
            Message.AppendString(this.Name);
            Message.AppendString(this.Description);
            Message.AppendString(this.StartTime);
            Message.AppendInt32(this.Tags.Count);
            foreach (string str in this.Tags.ToArray())
            {
                Message.AppendString(str);
            }
        }
    }
}

