namespace Butterfly.HabboHotel.Users.Messenger
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.Messages;
    using System;

    internal class MessengerBuddy
    {
        private GameClient client;
        private Room currentRoom;
        private readonly string mLastOnline;
        private readonly string mLook;
        private readonly string mMotto;
        private readonly string mUsername;
        private readonly uint UserId;
        private readonly uint mRelation;

        internal MessengerBuddy(uint UserId, string pUsername, string pLook, string pMotto, string pLastOnline, uint relation)
        {
            this.UserId = UserId;
            this.mUsername = pUsername;
            this.mLook = pLook;
            this.mMotto = pMotto;
            this.mLastOnline = pLastOnline;
            this.mRelation = relation;

        }

        internal void Serialize(ServerMessage reply)
        {
            reply.AppendInt32(this.UserId);
            reply.AppendString(this.mUsername);
            reply.AppendInt32(1);
            bool isOnline = this.IsOnline;
            reply.AppendBoolean(isOnline);
            if (isOnline)
            {
                reply.AppendBoolean(this.InRoom);
            }
            else
            {
                reply.AppendBoolean(false);
            }
            reply.AppendString(this.mLook);
            reply.AppendInt32(0);
            reply.AppendString(this.mMotto);
            reply.AppendString(string.Empty);//fb name
            reply.AppendString(string.Empty);
            reply.AppendBoolean(true);
            reply.AppendInt32(mRelation);
            
        }

        internal void UpdateUser()
        {
            this.client = ButterflyEnvironment.GetGame().GetClientManager().GetClient(this.UserId);
            this.UpdateUser(this.client);
        }

        internal void UpdateUser(GameClient client)
        {
            this.client = client;
            if ((client != null) && (client.GetHabbo() != null))
            {
                this.currentRoom = client.GetHabbo().CurrentRoom;
            }
        }

        private GameClient Client
        {
            get
            {
                return this.client;
            }
            set
            {
                this.client = value;
            }
        }

        internal Room CurrentRoom
        {
            get
            {
                return this.currentRoom;
            }
            set
            {
                this.currentRoom = value;
            }
        }

        internal uint Id
        {
            get
            {
                return this.UserId;
            }
        }

        internal bool InRoom
        {
            get
            {
                return (this.currentRoom != null);
            }
        }

        internal bool IsOnline
        {
            get
            {
                return ((((this.client != null) && (this.client.GetHabbo() != null)) && (this.client.GetHabbo().GetMessenger() != null)) && !this.client.GetHabbo().GetMessenger().AppearOffline);
            }
        }
    }
}

