namespace Butterfly.HabboHotel.Users.Relationship
{
    using Butterfly;
    using Butterfly.Collections;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using Butterfly.Messages;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

	class RelationshipComposer
	{
        private Dictionary<uint, uint> composedRelations = new Dictionary<uint,uint>();
        internal uint UserID;
        private uint[] relationshipStatusIndex = { 0, 0, 0};  //love,friend,enemy
        GameClient client;


        internal RelationshipComposer(uint userID, UserData data)
        {
            this.UserID = userID;
            foreach(Relationship relation in data.relationships)
            {
                if(!this.composedRelations.ContainsKey(relation.targetID))
                {
                    composedRelations.Add(relation.targetID,relation.relationshipStatus);

                    switch (relation.relationshipStatus)
                    {
                        case 1:
                            relationshipStatusIndex[0]++;
                            break;
                        case 2:
                            relationshipStatusIndex[1]++;
                            break;
                        case 3:
                            relationshipStatusIndex[2]++;
                            break;
                    }
                }
            }  
        }

        internal void RelationshipStatusUpdate(uint targetID, uint newRelationshipStatus)
        {
            if ((composedRelations[targetID] == newRelationshipStatus) || (!composedRelations.ContainsKey(targetID) && (newRelationshipStatus == 0)))
                return;
            else
            {
                if (newRelationshipStatus != 0)
                {
                    if (this.HasRelationshipWith(targetID))
                    {
                        using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.setQuery("UPDATE user_relationships SET relationshipstatus=@newstatus WHERE requester_id = @sender AND target_id = @target LIMIT 1");
                            adapter.addParameter("sender", this.UserID);
                            adapter.addParameter("target", targetID);
                            adapter.addParameter("newstatus", newRelationshipStatus);
                            adapter.runQuery();
                        }

                        relationshipStatusIndex[composedRelations[targetID] - 1]--;
                        composedRelations[targetID] = newRelationshipStatus;
                        relationshipStatusIndex[newRelationshipStatus - 1]++;
                    }
                    else
                    {
                        using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.setQuery("INSERT INTO user_relationships (requester_id,target_id,relationshipstatus) VALUES (@sender,@target,@newstatus)");
                            adapter.addParameter("sender", this.UserID);
                            adapter.addParameter("target", targetID);
                            adapter.addParameter("newstatus", newRelationshipStatus);
                            adapter.runQuery();
                        }
                        relationshipStatusIndex[newRelationshipStatus - 1]++;
                        this.composedRelations.Add(targetID, newRelationshipStatus);
                    }

                }
                else
                {
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery("DELETE FROM user_relationships WHERE requester_id = @sender AND target_id = @target LIMIT 1");
                        adapter.addParameter("sender", this.UserID);
                        adapter.addParameter("target", targetID);
                        adapter.runQuery();
                    }
                    relationshipStatusIndex[composedRelations[targetID] - 1]--;
                    this.composedRelations.Remove(targetID);
                }
            }
        }

        internal ServerMessage SerializeRelationshipsProfile()
        {
            Random random = new Random();
            int indexesCounter = 0;
            ServerMessage Packet = new ServerMessage(Outgoing.ProfileRelationships);
            Packet.AppendInt32(this.UserID);
            for (int i = 0; i < 3; i++)
            {
                if (relationshipStatusIndex[i] > 0)
                    indexesCounter++;
            }
            Packet.AppendInt32(indexesCounter);
            if (indexesCounter > 0)
            {
                for (uint i = 1; i <= 3; i++)
                {
                    if (composedRelations.ContainsValue(i))
                    {
                        List<uint> relationshipsList = new List<uint>();
                        Packet.AppendInt32(i);
                        Packet.AppendInt32(relationshipStatusIndex[i - 1]);
                        foreach (KeyValuePair<uint, uint> iterator in this.composedRelations)
                        {
                            if (iterator.Value == i)
                            {
                                relationshipsList.Add(iterator.Key);
                            }
                        }
                        uint target_id =relationshipsList[random.Next(0, relationshipsList.Count)];
                        Packet.AppendInt32(target_id);
                        using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            DataRow row;
                            adapter.setQuery("SELECT * FROM users where id = " + target_id);
                            row = adapter.getRow();
                            Packet.AppendString((string)row["username"]);
                            //Packet.AppendString((string)row["look"]); newer versions only
                        }
                    }
                }
            }
            return Packet;
        }

        internal bool HasRelationshipWith(uint targetID)
        {
            return this.composedRelations.ContainsKey(targetID);
        }

        internal uint RelationshipStatusWith(uint targetID)
        {
            if (!HasRelationshipWith(targetID))
            {
                return 0;
            }
            else
                return composedRelations[targetID];
        }

        internal Dictionary<uint, uint> GetRelationshipList
        {
            get
            {
                return this.composedRelations;
            }
        }

        internal uint GetRelationshipsLovesCount
        {
            get
            {
                return this.relationshipStatusIndex[0];
            }
        }
        internal uint GetRelationshipsFriendsCount
        {
            get
            {
                return this.relationshipStatusIndex[1];
            }
        }
        internal uint GetRelationshipsEnemysCount
        {
            get
            {
                return this.relationshipStatusIndex[2];
            }
        }

        internal bool IsOnline
        {
            get
            {
                return ((((this.client != null) && (this.client.GetHabbo() != null)) && (this.client.GetHabbo().GetMessenger() != null)) && !this.client.GetHabbo().GetMessenger().AppearOffline);
            }
        }
        internal ServerMessage SerializeRelationshipUpdate(uint targetId, uint newRelationshipState)
        {
            this.RelationshipStatusUpdate(targetId, newRelationshipState);
            Habbo habbo = ButterflyEnvironment.getHabboForId(targetId);
            this.client = habbo.GetClient();
            ServerMessage reply = new ServerMessage(Outgoing.FriendUpdate);
            reply.AppendInt32(0);
            reply.AppendInt32(1);
            reply.AppendInt32(0);
            reply.AppendInt32(habbo.Id);
            reply.AppendString(habbo.Username);
            reply.AppendInt32(1);
            bool isOnline = this.IsOnline;
            bool inRoom;
            if (habbo.CurrentRoom != null)
                inRoom = true;
            else
                inRoom = false;
            reply.AppendBoolean(isOnline);
            if (isOnline)
            {
                reply.AppendBoolean(inRoom);
            }
            else
            {
                reply.AppendBoolean(false);
            }
            reply.AppendString(habbo.Look);
            reply.AppendInt32(0);
            reply.AppendString(habbo.Motto);
            reply.AppendString(string.Empty);
            reply.AppendString(string.Empty);
            reply.AppendBoolean(true);
            reply.AppendInt32(newRelationshipState);
            return reply;
        }
	}
}
