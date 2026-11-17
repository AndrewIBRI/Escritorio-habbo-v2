namespace Butterfly.HabboHotel.Support
{
    using Butterfly;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;

    internal class SupportTicket
    {
        private uint Id;
        internal string Message;
        internal uint ModeratorId;
        private string ModName;
        internal uint ReportedId;
        private string ReportedName;
        internal uint RoomId;
        internal string RoomName;
        internal int Score;
        internal uint SenderId;
        private string SenderName;
        internal TicketStatus Status;
        internal double Timestamp;
        internal int Type;

        internal SupportTicket(uint Id, int Score, int Type, uint SenderId, uint ReportedId, string Message, uint RoomId, string RoomName, double Timestamp)
        {
            this.Id = Id;
            this.Score = Score;
            this.Type = Type;
            this.Status = TicketStatus.OPEN;
            this.SenderId = SenderId;
            this.ReportedId = ReportedId;
            this.ModeratorId = 0;
            this.Message = Message;
            this.RoomId = RoomId;
            this.RoomName = RoomName;
            this.Timestamp = Timestamp;
            this.SenderName = ButterflyEnvironment.GetGame().GetClientManager().GetNameById(SenderId);
            this.ReportedName = ButterflyEnvironment.GetGame().GetClientManager().GetNameById(ReportedId);
            this.ModName = ButterflyEnvironment.GetGame().GetClientManager().GetNameById(this.ModeratorId);
        }

        internal SupportTicket(uint Id, int Score, int Type, uint SenderId, uint ReportedId, string Message, uint RoomId, string RoomName, double Timestamp, object senderName, object reportedName, object modName)
        {
            this.Id = Id;
            this.Score = Score;
            this.Type = Type;
            this.Status = TicketStatus.OPEN;
            this.SenderId = SenderId;
            this.ReportedId = ReportedId;
            this.ModeratorId = 0;
            this.Message = Message;
            this.RoomId = RoomId;
            this.RoomName = RoomName;
            this.Timestamp = Timestamp;
            if (senderName == DBNull.Value)
            {
                this.SenderName = string.Empty;
            }
            else
            {
                this.SenderName = (string) senderName;
            }
            if (reportedName == DBNull.Value)
            {
                this.ReportedName = string.Empty;
            }
            else
            {
                this.ReportedName = (string) reportedName;
            }
            if (modName == DBNull.Value)
            {
                this.ModName = string.Empty;
            }
            else
            {
                this.ModName = (string) modName;
            }
        }

        internal void Close(TicketStatus NewStatus, bool UpdateInDb)
        {
            this.Status = NewStatus;
            if (UpdateInDb)
            {
                string str = "";
                switch (NewStatus)
                {
                    case TicketStatus.ABUSIVE:
                        str = "abusive";
                        break;

                    case TicketStatus.INVALID:
                        str = "invalid";
                        break;

                    default:
                        str = "resolved";
                        break;
                }
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE moderation_tickets SET status = '", str, "' WHERE id = ", this.Id }));
                }
            }
        }

        internal void Delete(bool UpdateInDb)
        {
            this.Status = TicketStatus.DELETED;
            if (UpdateInDb)
            {
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery("UPDATE moderation_tickets SET status = 'deleted' WHERE id = " + this.Id);
                }
            }
        }

        internal void Pick(uint pModeratorId, bool UpdateInDb)
        {
            this.Status = TicketStatus.PICKED;
            this.ModeratorId = pModeratorId;
            this.Timestamp = ButterflyEnvironment.GetUnixTimestamp();
            if (UpdateInDb)
            {
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE moderation_tickets SET status = 'picked', moderator_id = ", pModeratorId, ", timestamp = '", ButterflyEnvironment.GetUnixTimestamp(), "' WHERE id = ", this.Id }));
                }
            }
        }

        internal void Release(bool UpdateInDb)
        {
            this.Status = TicketStatus.OPEN;
            if (UpdateInDb)
            {
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery("UPDATE moderation_tickets SET status = 'open' WHERE id = " + this.Id);
                }
            }
        }

        internal ServerMessage Serialize()
        {
            ServerMessage message = new ServerMessage(Outgoing.SerializeIssue);
            message.AppendInt32(this.Id);
            message.AppendInt32(this.TabId);
            message.AppendInt32(1);
            message.AppendInt32(this.Type);
            message.AppendInt32(11);
            message.AppendInt32(this.Score);
            message.AppendInt32(this.SenderId);
            message.AppendString(this.SenderName);
            message.AppendInt32(this.ReportedId);
            message.AppendString(this.ReportedName);
            message.AppendInt32((this.Status == TicketStatus.PICKED) ? this.ModeratorId : 0);
            message.AppendString(this.ModName);
            message.AppendString(this.Message);
            message.AppendInt32(this.RoomId);
            message.AppendString(this.RoomName);
            RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(this.RoomId);
            if (data != null)
            {
                message.AppendInt32(data.IsPublicRoom ? 1 : 0);
                if (ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.RoomId) != null)
                {
                    Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.RoomId);
                    if (room.HasOngoingEvent)
                    {
                        room.Event.SerializeTo(data, message);
                    }
                    else
                    {
                        message.AppendString("-1");
                    }
                }
                message.AppendInt32(data.Category);
                message.AppendInt32(0);
                message.AppendString("");
                message.AppendInt32(0);
                message.AppendString("");
                return message;
            }
            message.AppendInt32(0);
            message.AppendString("-1");
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendString("");
            message.AppendInt32(0);
            message.AppendString("");
            return message;
        }

        internal int TabId
        {
            get
            {
                if (this.Status == TicketStatus.OPEN)
                {
                    return 1;
                }
                if (this.Status == TicketStatus.PICKED)
                {
                    return 2;
                }
                if (((this.Status == TicketStatus.ABUSIVE) || (this.Status == TicketStatus.INVALID)) || (this.Status == TicketStatus.RESOLVED))
                {
                    return 2;
                }
                if (this.Status == TicketStatus.DELETED)
                {
                    return 3;
                }
                return 0;
            }
        }

        internal uint TicketId
        {
            get
            {
                return this.Id;
            }
        }
    }
}

