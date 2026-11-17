namespace Butterfly.HabboHotel.Users.Messenger
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.Messages;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class HabboMessenger
    {
        internal bool AppearOffline;
        private Dictionary<uint, MessengerBuddy> friends = new Dictionary<uint, MessengerBuddy>();
        public Dictionary<uint, MessengerRequest> requests = new Dictionary<uint, MessengerRequest>();
        private uint UserId;

        internal HabboMessenger(uint UserId)
        {
            this.UserId = UserId;
        }

        internal void ClearRequests()
        {
            this.requests.Clear();
        }

        internal void CreateFriendship(uint friendID)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MSSQL)
                {
                    adapter.runFastQuery(string.Concat(new object[] { "DELETE FROM messenger_friendships WHERE sender = ", this.UserId, " AND receiver = ", friendID }));
                    adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO messenger_friendships (sender,receiver) VALUES (", this.UserId, ",", friendID, ")" }));
                }
                else
                {
                    adapter.runFastQuery(string.Concat(new object[] { "REPLACE INTO messenger_friendships (sender,receiver) VALUES (", this.UserId, ",", friendID, ")" }));
                }
            }
            this.OnNewFriendship(friendID);
            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(friendID);
            if ((clientByUserID != null) && (clientByUserID.GetHabbo().GetMessenger() != null))
            {
                clientByUserID.GetHabbo().GetMessenger().OnNewFriendship(this.UserId);
            }
        }

        internal void DeliverInstantMessage(string message, uint convoID)
        {
            ServerMessage message2 = new ServerMessage(Outgoing.InstantChat);
            message2.AppendInt32(convoID);
            message2.AppendString(message);
            message2.AppendInt32(0);
            this.GetClient().SendMessage(message2);
        }

        internal void DeliverInstantMessageError(int ErrorId, uint ConversationId)
        {
            ServerMessage message = new ServerMessage(Outgoing.InstantChatError);
            message.AppendInt32(ErrorId);
            message.AppendInt32(ConversationId);
            this.GetClient().SendMessage(message);
        }

        internal void Destroy()
        {
            IEnumerable<GameClient> clientsById = ButterflyEnvironment.GetGame().GetClientManager().GetClientsById(this.friends.Keys);
            foreach (GameClient client in clientsById)
            {
                if ((client.GetHabbo() != null) && (client.GetHabbo().GetMessenger() != null))
                {
                    client.GetHabbo().GetMessenger().UpdateFriend(this.UserId, null, true);
                }
            }
        }

        internal void DestroyFriendship(uint friendID)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "DELETE FROM messenger_friendships WHERE (sender = ", this.UserId, " AND receiver = ", friendID, ") OR (receiver = ", this.UserId, " AND sender = ", friendID, ")" }));
            }
            this.OnDestroyFriendship(friendID);
            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(friendID);
            if ((clientByUserID != null) && (clientByUserID.GetHabbo().GetMessenger() != null))
            {
                clientByUserID.GetHabbo().GetMessenger().OnDestroyFriendship(this.UserId);
            }
        }

        internal bool FriendshipExists(uint friendID)
        {
            return this.friends.ContainsKey(friendID);
        }

        internal IEnumerable<RoomData> GetActiveFriendsRooms()
        {
            foreach (MessengerBuddy iteratorVariable0 in from p in this.friends.Values
                where p.InRoom
                select p)
            {
                yield return iteratorVariable0.CurrentRoom.RoomData;
            }
        }

        private GameClient GetClient()
        {
            return ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(this.UserId);
        }

        internal MessengerRequest GetRequest(uint senderID)
        {
            if (this.requests.ContainsKey(senderID))
            {
                return this.requests[senderID];
            }
            return null;
        }

        internal void HandleAllRequests()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "DELETE FROM messenger_requests WHERE sender = ", this.UserId, " OR receiver = ", this.UserId }));
            }
            this.ClearRequests();
        }

        internal void HandleRequest(uint sender)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "DELETE FROM messenger_requests WHERE (sender = ", this.UserId, " AND receiver = ", sender, ") OR (receiver = ", this.UserId, " AND sender = ", sender, ")" }));
            }
            this.requests.Remove(sender);
        }

        internal void Init(Dictionary<uint, MessengerBuddy> friends, Dictionary<uint, MessengerRequest> requests)
        {
            this.requests = new Dictionary<uint, MessengerRequest>(requests);
            this.friends = new Dictionary<uint, MessengerBuddy>(friends);
        }

        internal void OnDestroyFriendship(uint Friend)
        {
            this.friends.Remove(Friend);
            this.GetClient().GetMessageHandler().GetResponse().Init(Outgoing.FriendUpdate);
            this.GetClient().GetMessageHandler().GetResponse().AppendInt32(0);
            this.GetClient().GetMessageHandler().GetResponse().AppendInt32(1);
            this.GetClient().GetMessageHandler().GetResponse().AppendInt32(-1);
            this.GetClient().GetMessageHandler().GetResponse().AppendInt32(Friend);
            this.GetClient().GetMessageHandler().SendResponse();
        }

        internal void OnNewFriendship(uint friendID)
        {
            MessengerBuddy buddy;
            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(friendID);
            if ((clientByUserID == null) || (clientByUserID.GetHabbo() == null))
            {
                DataRow row;
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("SELECT username,motto,look,last_online FROM users WHERE id = " + friendID);
                    row = adapter.getRow();
                }
                buddy = new MessengerBuddy(friendID, (string)row["username"], (string)row["look"], (string)row["motto"], (string)row["last_online"], ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(this.UserId).GetHabbo().GetRelationshipComposer().RelationshipStatusWith(friendID));
            }
            else
            {
                Habbo habbo = clientByUserID.GetHabbo();
                buddy = new MessengerBuddy(friendID, habbo.Username, habbo.Look, habbo.Motto, string.Empty,habbo.GetRelationshipComposer().RelationshipStatusWith(friendID));
                buddy.UpdateUser(clientByUserID);
            }
            if (!this.friends.ContainsKey(friendID))
            {
                this.friends.Add(friendID, buddy);
            }
            this.GetClient().SendMessage(SerializeUpdate(buddy));
        }


        internal void OnNewRequest(uint friendID)
        {
            if (!this.requests.ContainsKey(friendID))
            {
                this.requests.Add(friendID, new MessengerRequest(this.UserId, friendID, ButterflyEnvironment.GetGame().GetClientManager().GetNameById(friendID)));
            }
        }

        internal void OnStatusChanged(bool notification)
        {
            IEnumerable<GameClient> clientsById = ButterflyEnvironment.GetGame().GetClientManager().GetClientsById(this.friends.Keys);
            foreach (GameClient client in clientsById)
            {
                if (((client != null) && (client.GetHabbo() != null)) && (client.GetHabbo().GetMessenger() != null))
                {
                    client.GetHabbo().GetMessenger().UpdateFriend(this.UserId, client, true);
                    this.UpdateFriend(client.GetHabbo().Id, client, notification);
                }
            }
        }

        internal ServerMessage PerformSearch(string query)
        {
            List<SearchResult> searchResult = SearchResultFactory.GetSearchResult(query);
            List<SearchResult> list2 = new List<SearchResult>();
            List<SearchResult> list3 = new List<SearchResult>();
            foreach (SearchResult result in searchResult)
            {
                if (result.userID != this.GetClient().GetHabbo().Id)
                {
                    if (this.FriendshipExists(result.userID))
                    {
                        list2.Add(result);
                    }
                    else
                    {
                        list3.Add(result);
                    }
                }
            }
            ServerMessage reply = new ServerMessage(Outgoing.SearchFriend);
            reply.AppendInt32(list2.Count);
            foreach (SearchResult result in list2)
            {
                result.Searialize(reply);
            }
            reply.AppendInt32(list3.Count);
            foreach (SearchResult result in list3)
            {
                result.Searialize(reply);
            }
            return reply;
        }

        internal bool RequestBuddy(string UserQuery)
        {
            uint id;
            bool hasFriendRequestsDisabled;
            IQueryAdapter adapter;
            GameClient clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(UserQuery);
            if (clientByUsername == null)
            {
                DataRow row = null;
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("SELECT id,block_newfriends FROM users WHERE username = @query");
                    adapter.addParameter("query", UserQuery.ToLower());
                    row = adapter.getRow();
                }
                if (row == null)
                {
                    return false;
                }
                id = Convert.ToUInt32(row["id"]);
                hasFriendRequestsDisabled = ButterflyEnvironment.EnumToBool(row["block_newfriends"].ToString());
            }
            else
            {
                id = clientByUsername.GetHabbo().Id;
                hasFriendRequestsDisabled = clientByUsername.GetHabbo().HasFriendRequestsDisabled;
            }
            if (hasFriendRequestsDisabled)
            {
                this.GetClient().GetMessageHandler().GetResponse().Init(260);
                this.GetClient().GetMessageHandler().GetResponse().AppendInt32(0x27);
                this.GetClient().GetMessageHandler().GetResponse().AppendInt32(3);
                this.GetClient().GetMessageHandler().SendResponse();
                return true;
            }
            uint requestID = id;
            if (!this.RequestExists(requestID))
            {
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    if (adapter.dbType == DatabaseType.MSSQL)
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "DELETE FROM messenger_requests WHERE sender = ", id, " AND receiver = ", requestID }));
                        adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO messenger_requests (sender,receiver) VALUES (", this.UserId, ",", requestID, ")" }));
                    }
                    else
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "REPLACE INTO messenger_requests (sender,receiver) VALUES (", this.UserId, ",", requestID, ")" }));
                    }
                }
                GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(requestID);
                if ((clientByUserID == null) || (clientByUserID.GetHabbo() == null))
                {
                    return true;
                }
                MessengerRequest request = new MessengerRequest(requestID, this.UserId, ButterflyEnvironment.GetGame().GetClientManager().GetNameById(this.UserId));
                clientByUserID.GetHabbo().GetMessenger().OnNewRequest(this.UserId);
                ServerMessage message = new ServerMessage(Outgoing.SendFriendRequest);
                request.Serialize(message);
                clientByUserID.SendMessage(message);
                this.requests.Add(requestID, request);
            }
            return true;
        }

        internal bool RequestExists(uint requestID)
        {
            if (this.requests.ContainsKey(requestID))
            {
                return true;
            }
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT sender FROM messenger_friendships WHERE sender = @myID AND receiver = @friendID");
                adapter.addParameter("myID", (int) this.UserId);
                adapter.addParameter("friendID", (int) requestID);
                return adapter.findsResult();
            }
        }

        internal void SendInstantMessage(uint ToId, string Message)
        {
            if (!this.FriendshipExists(ToId))
            {
                this.DeliverInstantMessageError(6, ToId);
            }
            else
            {
                GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(ToId);
                if ((clientByUserID == null) || (clientByUserID.GetHabbo().GetMessenger() == null))
                {
                    this.DeliverInstantMessageError(5, ToId);
                }
                else if (this.GetClient().GetHabbo().Muted)
                {
                    this.DeliverInstantMessageError(4, ToId);
                }
                else
                {
                    if (clientByUserID.GetHabbo().Muted)
                    {
                        this.DeliverInstantMessageError(3, ToId);
                    }
                    if (!(Message == ""))
                    {
                        clientByUserID.GetHabbo().GetMessenger().DeliverInstantMessage(Message, this.UserId);
                    }
                }
            }
        }

        internal ServerMessage SerializeFriends()
        {
            ServerMessage reply = new ServerMessage(Outgoing.InitFriends);
            reply.AppendInt32(this.GetClient().GetHabbo().GetSubscriptionManager().HasSubscription("club_habbo") ? 0x44c : 300);
            reply.AppendInt32(300);
            reply.AppendInt32(800);
            reply.AppendInt32(0x44c);
            reply.AppendInt32(0);
            reply.AppendInt32(this.friends.Count);
            foreach (MessengerBuddy buddy in this.friends.Values)
            {
                buddy.Serialize(reply);
            }
            return reply;
        }

        internal ServerMessage SerializeRequests()
        {
            ServerMessage message = new ServerMessage(Outgoing.InitRequests);
            message.AppendInt32((this.requests.Count > ButterflyEnvironment.friendRequestLimit) ? ((int) ButterflyEnvironment.friendRequestLimit) : this.requests.Count);
            message.AppendInt32((this.requests.Count > ButterflyEnvironment.friendRequestLimit) ? ((int) ButterflyEnvironment.friendRequestLimit) : this.requests.Count);
            int num = 0;
            foreach (MessengerRequest request in this.requests.Values)
            {
                num++;
                if (num > ButterflyEnvironment.friendRequestLimit)
                {
                    return message;
                }
                request.Serialize(message);
            }
            return message;
        }

        internal static ServerMessage SerializeUpdate(MessengerBuddy friend)
        {
            ServerMessage reply = new ServerMessage(Outgoing.FriendUpdate);
            reply.AppendInt32(0);
            reply.AppendInt32(1);
            reply.AppendInt32(0);
            friend.Serialize(reply);
            return reply;
        }

        internal void UpdateFriend(uint userid, GameClient client, bool notification)
        {
            if (this.friends.ContainsKey(userid))
            {
                this.friends[userid].UpdateUser(client);
                if (notification)
                {
                    GameClient client2 = this.GetClient();
                    if (client2 != null)
                    {
                        client2.SendMessage(SerializeUpdate(this.friends[userid]));
                    }
                }
            }
        }

        internal int myFriends
        {
            get
            {
                return this.friends.Count;
            }
        }

    }
}

