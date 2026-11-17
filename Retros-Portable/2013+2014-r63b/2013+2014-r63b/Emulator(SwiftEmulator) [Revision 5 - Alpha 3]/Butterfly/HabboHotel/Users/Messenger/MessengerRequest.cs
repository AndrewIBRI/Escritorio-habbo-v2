namespace Butterfly.HabboHotel.Users.Messenger
{
    using Butterfly;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.Messages;
    using System;

    internal class MessengerRequest
    {
        private uint FromUser;
        private string mUsername;
        private uint ToUser;

        internal MessengerRequest(uint ToUser, uint FromUser, string pUsername)
        {
            this.ToUser = ToUser;
            this.FromUser = FromUser;
            this.mUsername = pUsername;
        }

        internal void Serialize(ServerMessage Request)
        {
            Request.AppendInt32(this.FromUser);
            Request.AppendString(this.mUsername);
            Habbo habbo = ButterflyEnvironment.getHabboForName(this.mUsername);
            Request.AppendString((habbo != null) ? habbo.Look : "");
        }

        internal uint From
        {
            get
            {
                return this.FromUser;
            }
        }

        internal uint To
        {
            get
            {
                return this.ToUser;
            }
        }
    }
}

