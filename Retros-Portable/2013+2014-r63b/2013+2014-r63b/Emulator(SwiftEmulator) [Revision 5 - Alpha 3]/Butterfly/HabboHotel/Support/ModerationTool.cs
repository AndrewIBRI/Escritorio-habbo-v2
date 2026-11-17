namespace Butterfly.HabboHotel.Support
{
    using Butterfly;
    using Butterfly.Collections;
    using Butterfly.Core;
    using Butterfly.HabboHotel.ChatMessageStorage;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.RoomIvokedItems;
    using Butterfly.Messages;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public class ModerationTool
    {
        internal List<string> RoomMessagePresets = new List<string>();
        internal List<SupportTicket> Tickets = new List<SupportTicket>();
        internal List<string> UserMessagePresets = new List<string>();

        internal ModerationTool()
        {
        }

        internal static void AlertUser(GameClient ModSession, uint UserId, string Message, bool Caution)
        {
            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if ((clientByUserID != null) && (clientByUserID.GetHabbo().Id != ModSession.GetHabbo().Id))
            {
                if (Caution && (clientByUserID.GetHabbo().Rank >= ModSession.GetHabbo().Rank))
                {
                    ModSession.SendNotif(LanguageLocale.GetValue("moderation.caution.missingrank"));
                    Caution = false;
                }
                clientByUserID.SendNotif(Message, Caution);
                if (Caution)
                {
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery("UPDATE user_info SET cautions = cautions + 1 WHERE user_id = " + UserId);
                    }
                }
            }
        }

        internal static void BanUser(GameClient ModSession, uint UserId, int Length, string Message)
        {
            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if ((clientByUserID != null) && (clientByUserID.GetHabbo().Id != ModSession.GetHabbo().Id))
            {
                if (clientByUserID.GetHabbo().Rank >= ModSession.GetHabbo().Rank)
                {
                    ModSession.SendNotif(LanguageLocale.GetValue("moderation.ban.missingrank"));
                }
                else
                {
                    double lengthSeconds = Length;
                    ButterflyEnvironment.GetGame().GetBanManager().BanUser(clientByUserID, ModSession.GetHabbo().Username, lengthSeconds, Message, false);
                }
            }
        }

        internal void CloseTicket(GameClient Session, uint TicketId, int Result)
        {
            SupportTicket ticket = this.GetTicket(TicketId);
            if (((ticket != null) && (ticket.Status == TicketStatus.PICKED)) && (ticket.ModeratorId == Session.GetHabbo().Id))
            {
                TicketStatus iNVALID;
                GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(ticket.SenderId);
                switch (Result)
                {
                    case 1:
                        iNVALID = TicketStatus.INVALID;
                        break;

                    case 2:
                        iNVALID = TicketStatus.ABUSIVE;
                        using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.runFastQuery("UPDATE user_info SET cfhs_abusive = cfhs_abusive + 1 WHERE user_id = " + ticket.SenderId);
                        }
                        break;

                    default:
                        iNVALID = TicketStatus.RESOLVED;
                        break;
                }
                if (clientByUserID == null)
                {
                }
                if (clientByUserID != null)
                {
                    ServerMessage ClosedResult = new ServerMessage(Outgoing.ModResponse);
                    ClosedResult.AppendInt32(Result);
                    clientByUserID.SendMessage(ClosedResult);
                }
                ticket.Close(iNVALID, true);
                SendTicketToModerators(ticket);
            }
        }

        internal void DeletePendingTicketForUser(uint Id)
        {
            foreach (SupportTicket ticket in this.Tickets)
            {
                if (ticket.SenderId == Id)
                {
                    ticket.Delete(true);
                    SendTicketToModerators(ticket);
                    break;
                }
            }
        }

        internal SupportTicket GetTicket(uint TicketId)
        {
            foreach (SupportTicket ticket in this.Tickets)
            {
                if (ticket.TicketId == TicketId)
                {
                    return ticket;
                }
            }
            return null;
        }

        internal static void KickUser(GameClient ModSession, uint UserId, string Message, bool Soft)
        {
            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (((clientByUserID != null) && (clientByUserID.GetHabbo().CurrentRoomId >= 1)) && (clientByUserID.GetHabbo().Id != ModSession.GetHabbo().Id))
            {
                if (clientByUserID.GetHabbo().Rank >= ModSession.GetHabbo().Rank)
                {
                    ModSession.SendNotif(LanguageLocale.GetValue("moderation.kick.missingrank"));
                }
                else
                {
                    Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(clientByUserID.GetHabbo().CurrentRoomId);
                    if (room != null)
                    {
                        room.GetRoomUserManager().RemoveUserFromRoom(clientByUserID, true, false);
                        clientByUserID.CurrentRoomUserID = -1;
                        if (!Soft)
                        {
                            clientByUserID.SendNotif(Message);
                            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.runFastQuery("UPDATE user_info SET cautions = cautions + 1 WHERE user_id = " + UserId);
                            }
                        }
                    }
                }
            }
        }

        internal void LoadMessagePresets(IQueryAdapter dbClient)
        {
            this.UserMessagePresets.Clear();
            this.RoomMessagePresets.Clear();
            dbClient.setQuery("SELECT type,message FROM moderation_presets WHERE enabled = 2");
            DataTable table = dbClient.getTable();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    string item = (string) row["message"];
                    string str2 = row["type"].ToString().ToLower();
                    if (str2 != null)
                    {
                        if (!(str2 == "message"))
                        {
                            if (str2 == "roommessage")
                            {
                                goto Label_00B6;
                            }
                        }
                        else
                        {
                            this.UserMessagePresets.Add(item);
                        }
                    }
                    continue;
                Label_00B6:
                    this.RoomMessagePresets.Add(item);
                }
            }
        }

        internal void LoadPendingTickets(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("TRUNCATE TABLE moderation_tickets");
        }

        internal void LogStaffEntry(string modName, string target, string type, string description)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("INSERT INTO staff_logs (staffuser,target,action_type,description) VALUES (@username,@target,@type,@desc)");
                adapter.addParameter("username", modName);
                adapter.addParameter("target", target);
                adapter.addParameter("type", type);
                adapter.addParameter("desc", description);
                adapter.runQuery();
            }
        }

        internal static void PerformRoomAction(GameClient ModSession, uint RoomId, bool KickUsers, bool LockRoom, bool InappropriateRoom)
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
            if (room != null)
            {
                IQueryAdapter adapter;
                if (LockRoom)
                {
                    room.State = 1;
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery("UPDATE rooms SET state = 'locked' WHERE id = " + room.RoomId);
                    }
                }
                if (InappropriateRoom)
                {
                    room.Name = LanguageLocale.GetValue("moderation.room.roomclosed");
                    room.Description = LanguageLocale.GetValue("moderation.room.roomclosed");
                    room.ClearTags();
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery("UPDATE rooms SET caption = 'Inappropriate to Hotel Management', description = 'Inappropriate to Hotel Management', tags = '' WHERE id = " + room.RoomId);
                    }
                }
                if (KickUsers)
                {
                    onCycleDoneDelegate function = new onCycleDoneDelegate(room.onRoomKick);
                    room.GetRoomUserManager().UserList.QueueDelegate(function);
                }
                room.SendMessage(room.UpdateRoomState());
            }
        }

        internal void PickTicket(GameClient Session, uint TicketId)
        {
            SupportTicket ticket = this.GetTicket(TicketId);
            if ((ticket != null) && (ticket.Status == TicketStatus.OPEN))
            {
                ticket.Pick(Session.GetHabbo().Id, true);
                SendTicketToModerators(ticket);
            }
        }

        internal void ReleaseTicket(GameClient Session, uint TicketId)
        {
            SupportTicket ticket = this.GetTicket(TicketId);
            if (((ticket != null) && (ticket.Status == TicketStatus.PICKED)) && (ticket.ModeratorId == Session.GetHabbo().Id))
            {
                ticket.Release(true);
                SendTicketToModerators(ticket);
            }
        }

        internal static void RoomAlert(uint RoomId, bool Caution, string Message)
        {
            if ((ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(RoomId) != null) && (Message.Length > 1))
            {
                Butterfly.HabboHotel.Rooms.RoomIvokedItems.RoomAlert alert = new Butterfly.HabboHotel.Rooms.RoomIvokedItems.RoomAlert(Message, 3);
            }
        }

        internal void SendNewTicket(GameClient Session, int Category, uint ReportedUser, string Message)
        {
            if (Session.GetHabbo().CurrentRoomId > 0)
            {
                RoomData data = ButterflyEnvironment.GetGame().GetRoomManager().GenerateNullableRoomData(Session.GetHabbo().CurrentRoomId);
                uint id = 0;
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    if (adapter.dbType == DatabaseType.MSSQL)
                    {
                        adapter.setQuery(string.Concat(new object[] { "INSERT INTO moderation_tickets (score,type,status,sender_id,reported_id,moderator_id,message,room_id,room_name,timestamp) OUTPUT INSERTED.* VALUES (1,'", Category, "','open','", Session.GetHabbo().Id, "','", ReportedUser, "','0',@message,'", data.Id, "',@name,'", ButterflyEnvironment.GetUnixTimestamp(), "')" }));
                    }
                    else
                    {
                        adapter.setQuery(string.Concat(new object[] { "INSERT INTO moderation_tickets (score,type,status,sender_id,reported_id,moderator_id,message,room_id,room_name,timestamp) VALUES (1,'", Category, "','open','", Session.GetHabbo().Id, "','", ReportedUser, "','0',@message,'", data.Id, "',@name,'", ButterflyEnvironment.GetUnixTimestamp(), "')" }));
                    }
                    adapter.addParameter("message", Message);
                    adapter.addParameter("name", data.Name);
                    id = Convert.ToUInt32(adapter.insertQuery());
                    adapter.runFastQuery("UPDATE user_info SET cfhs = cfhs + 1 WHERE user_id = " + Session.GetHabbo().Id);
                }
                SupportTicket item = new SupportTicket(id, 1, Category, Session.GetHabbo().Id, ReportedUser, Message, data.Id, data.Name, (double)ButterflyEnvironment.GetUnixTimestamp());
                this.Tickets.Add(item);
                SendTicketToModerators(item);
            }
            else
            {
                uint id = 0;
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    if (adapter.dbType == DatabaseType.MSSQL)
                    {
                        adapter.setQuery(string.Concat(new object[] { "INSERT INTO moderation_tickets (score,type,status,sender_id,reported_id,moderator_id,message,room_id,room_name,timestamp) OUTPUT INSERTED.* VALUES (1,'", Category, "','open','", Session.GetHabbo().Id, "','", ReportedUser, "','0',@message,'", 0, "',@name,'", ButterflyEnvironment.GetUnixTimestamp(), "')" }));
                    }
                    else
                    {
                        adapter.setQuery(string.Concat(new object[] { "INSERT INTO moderation_tickets (score,type,status,sender_id,reported_id,moderator_id,message,room_id,room_name,timestamp) VALUES (1,'", Category, "','open','", Session.GetHabbo().Id, "','", ReportedUser, "','0',@message,'", 0, "',@name,'", ButterflyEnvironment.GetUnixTimestamp(), "')" }));
                    }
                    adapter.addParameter("message", Message);
                    adapter.addParameter("name", "Empty");
                    id = Convert.ToUInt32(adapter.insertQuery());
                    adapter.runFastQuery("UPDATE user_info SET cfhs = cfhs + 1 WHERE user_id = " + Session.GetHabbo().Id);
                }
                SupportTicket item = new SupportTicket(id, 1, Category, Session.GetHabbo().Id, ReportedUser, Message, 0, "No Room", (double)ButterflyEnvironment.GetUnixTimestamp());
                this.Tickets.Add(item);
                SendTicketToModerators(item);
            }
        }

        internal static void SendTicketToModerators(SupportTicket Ticket)
        {
            ButterflyEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(Ticket.Serialize(), "fuse_mod");
        }

        internal static void SendTicketUpdateToModerators(SupportTicket Ticket)
        {
        }

        internal void SerializeOpenTickets(ref QueuedServerMessage serverMessages, uint userID)
        {
            foreach (SupportTicket ticket in this.Tickets)
            {
                if (((ticket.Status == TicketStatus.OPEN) || ((ticket.Status == TicketStatus.PICKED) && (ticket.ModeratorId == userID))) || (ticket.Status == TicketStatus.PICKED))
                {
                    serverMessages.appendResponse(ticket.Serialize());
                }
            }
        }

        internal static ServerMessage SerializeRoomChatlog(uint roomID)
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(roomID);
            ServerMessage message = new ServerMessage(Outgoing.RoomChatlog);
            message.AppendBoolean(room.IsPublic);
            message.AppendInt32(room.RoomId);
            message.AppendString(room.Name);
            if (room == null)
            {
                message.AppendInt32(0);
                return message;
            }
            ChatMessageManager chatMessageManager = room.GetChatMessageManager();
            message.AppendInt32(chatMessageManager.messageCount);
            chatMessageManager.Serialize(ref message);
            return message;
        }

        internal static ServerMessage SerializeRoomTool(RoomData Data)
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Data.Id);
            uint i = 0;
            IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor();
            try
            {
                adapter.setQuery("SELECT id FROM users WHERE username = @owner");
                adapter.addParameter("owner", Data.Owner);
                i = Convert.ToUInt32(adapter.getRow()[0]);
            }
            catch (Exception exception)
            {
                Logging.HandleException(exception, "ModerationTool.SerializeRoomTool");
            }
            finally
            {
                if (adapter != null)
                {
                    adapter.Dispose();
                }
            }
            ServerMessage message = new ServerMessage(Outgoing.RoomTool);
            message.AppendInt32(Data.Id);
            message.AppendInt32(Data.UsersNow);
            if (room != null)
            {
                message.AppendBoolean(room.GetRoomUserManager().GetRoomUserByHabbo(Data.Owner) != null);
            }
            else
            {
                message.AppendBoolean(false);
            }
            message.AppendInt32(i);
            message.AppendString(Data.Owner);
            message.AppendBoolean(room != null);
            message.AppendString(Data.Name);
            message.AppendString(Data.Description);
            message.AppendInt32(Data.TagCount);
            foreach (string str in Data.Tags)
            {
                message.AppendString(str);
            }
            message.AppendBoolean((room != null) ? room.HasOngoingEvent : false);
            if (room != null)
            {
                if (room.Event == null)
                {
                    return message;
                }
                message.AppendString(room.Event.Name);
                message.AppendString(room.Event.Description);
                message.AppendInt32(room.Event.Tags.Count);
                foreach (string str in room.Event.Tags.ToArray())
                {
                    message.AppendString(str);
                }
            }
            return message;
        }

        internal static ServerMessage SerializeRoomVisits(uint UserId)
        {
            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            ServerMessage message = new ServerMessage(Outgoing.RoomVisits);
            message.AppendInt32(UserId);
            message.AppendString(ButterflyEnvironment.GetGame().GetClientManager().GetNameById(UserId));
            if ((clientByUserID != null) && (clientByUserID.GetHabbo().visitedRooms.Count > 0))
            {
                message.AppendInt32(clientByUserID.GetHabbo().visitedRooms.Count);
                foreach (RoomData data in clientByUserID.GetHabbo().visitedRooms)
                {
                    message.AppendBoolean(data.IsPublicRoom);
                    message.AppendInt32(data.Id);
                    message.AppendString(data.Name);
                    message.AppendInt32(0);
                    message.AppendInt32(1);
                }
                return message;
            }
            message.AppendInt32(0);
            return message;
        }

        internal static ServerMessage SerializeTicketChatlog(SupportTicket Ticket, RoomData RoomData, double Timestamp)
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(RoomData.Id);
            ServerMessage message = new ServerMessage(Outgoing.IssueChatlog);
            message.AppendInt32(Ticket.TicketId);
            message.AppendInt32(Ticket.SenderId);
            message.AppendInt32(Ticket.ReportedId);
            message.AppendInt32(RoomData.Id);
            message.AppendBoolean(RoomData.IsPublicRoom);
            message.AppendInt32(RoomData.Id);
            message.AppendString(RoomData.Name);
            if (room == null)
            {
                message.AppendInt32(0);
                return message;
            }
            ChatMessageManager chatMessageManager = room.GetChatMessageManager();
            message.AppendInt32(chatMessageManager.messageCount);
            chatMessageManager.Serialize(ref message);
            return message;
        }

        /*internal ServerMessage SerializeTool()
        {
            ServerMessage message = new ServerMessage(Outgoing.OpenModTools);
            message.AppendInt32(this.UserMessagePresets.Count);
            foreach (string str in this.UserMessagePresets)
            {
                message.AppendString(str);
            }
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendBoolean(true);
            message.AppendBoolean(true);
            message.AppendBoolean(true);
            message.AppendBoolean(true);
            message.AppendBoolean(true);
            message.AppendBoolean(true);
            message.AppendBoolean(true);
            message.AppendInt32(this.RoomMessagePresets.Count);
            foreach (string str in this.RoomMessagePresets)
            {
                message.AppendString(str);
            }
            return message;
        }*/
		
		internal ServerMessage SerializeTool()
        {
            ServerMessage Response = new ServerMessage(Outgoing.OpenModTools);
            
            Response.AppendInt32(0);
            Response.AppendInt32(UserMessagePresets.Count);
            foreach (String Preset in UserMessagePresets)
            {
                Response.AppendString(Preset);
            }
            Response.AppendInt32(0); 
            
            Response.AppendBoolean(true);
            Response.AppendBoolean(true);
            Response.AppendBoolean(true);
            Response.AppendBoolean(true);
            Response.AppendBoolean(true);
            Response.AppendBoolean(true);
            Response.AppendBoolean(true);
            Response.AppendInt32(0);
            Response.AppendInt32(RoomMessagePresets.Count);
            foreach (String Preset in RoomMessagePresets)
            {
                Response.AppendString(Preset);
            }
            return Response;
        }

        internal static ServerMessage SerializeUserChatlog(uint UserId)
        {
            ServerMessage message;
            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if ((clientByUserID == null) || (clientByUserID.GetHabbo() == null))
            {
                message = new ServerMessage(Outgoing.UserChatlog);
                message.AppendInt32(UserId);
                message.AppendString("User not online");
                message.AppendInt32(0);
                return message;
            }
            Dictionary<int, List<ChatMessage>> sortedMessages = clientByUserID.GetHabbo().GetChatMessageManager().GetSortedMessages();
            message = new ServerMessage(Outgoing.UserChatlog);
            message.AppendInt32(UserId);
            message.AppendString(clientByUserID.GetHabbo().Username);
            message.AppendInt32(sortedMessages.Count);
            foreach (KeyValuePair<int, List<ChatMessage>> pair in sortedMessages)
            {
                List<ChatMessage> source = pair.Value;
                ChatMessage message2 = source.First<ChatMessage>();
                message.AppendBoolean(message2.roomIsPublic);
                message.AppendInt32(message2.roomID);
                message.AppendString(message2.roomName);
                message.AppendInt32(source.Count);
                foreach (ChatMessage message3 in source)
                {
                    message3.Serialize(ref message);
                }
            }
            return message;
        }

        internal static ServerMessage SerializeUserInfo(uint UserId)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT id, username, online, mail, look FROM users WHERE id = " + UserId);
                DataRow row = adapter.getRow();
                adapter.setQuery("SELECT reg_timestamp, login_timestamp, cfhs, cfhs_abusive, cautions, bans FROM user_info WHERE user_id = " + UserId);
                DataRow row2 = adapter.getRow();
                if (row == null)
                {
                    throw new NullReferenceException("No user found in database");
                }
                ServerMessage message = new ServerMessage(Outgoing.UserTool);
                message.AppendInt32(Convert.ToUInt32(row["id"]));
                message.AppendString((string) row["username"]);
                message.AppendString((string) row["look"]);
                if (row2 != null)
                {
                    message.AppendInt32((int) Math.Ceiling((double) ((ButterflyEnvironment.GetUnixTimestamp() - ((double) row2["reg_timestamp"])) / 60.0)));
                    message.AppendInt32((int) Math.Ceiling((double) ((ButterflyEnvironment.GetUnixTimestamp() - ((double) row2["login_timestamp"])) / 60.0)));
                }
                else
                {
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                }
                message.AppendBoolean(ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(Convert.ToUInt32(row["id"])) != null);
                if (row2 != null)
                {
                    message.AppendInt32((int) row2["cfhs"]);
                    message.AppendInt32((int) row2["cfhs_abusive"]);
                    message.AppendInt32((int) row2["cautions"]);
                    message.AppendInt32((int) row2["bans"]);
                }
                else
                {
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                }
                message.AppendString("hallo");
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendString((string) row["mail"]);
                return message;
            }
        }

        internal bool UsersHasPendingTicket(uint Id)
        {
            foreach (SupportTicket ticket in this.Tickets)
            {
                if ((ticket.SenderId == Id) && (ticket.Status == TicketStatus.OPEN))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

