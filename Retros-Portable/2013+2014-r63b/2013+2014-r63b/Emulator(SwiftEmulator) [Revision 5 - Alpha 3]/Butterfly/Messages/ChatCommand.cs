namespace Butterfly.Messages
{
    using Butterfly.HabboHotel.GameClients;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct ChatCommand
    {
        internal readonly int commandID;
        internal readonly string input;
        internal readonly int minrank;
        internal readonly string description;
        internal readonly string prefix;
        private string[] clubsAllowed;
        public ChatCommand(int pCommandID, string pInput, int pRank, string pDescription, string pPrefix, string[] clubsAllowed)
        {
            this.commandID = pCommandID;
            this.input = pInput;
            this.minrank = pRank;
            this.description = pDescription;
            this.prefix = pPrefix;
            this.clubsAllowed = clubsAllowed;
        }

        internal bool UserGotAuthorization(GameClient session)
        {
            foreach (string str in this.clubsAllowed)
            {
                if (session.GetHabbo().GetSubscriptionManager().HasSubscription(str))
                {
                    return true;
                }
            }
            if (this.minrank == 0)
            {
                return true;
            }
            if (this.minrank > 0)
            {
                if (this.minrank <= session.GetHabbo().Rank)
                {
                    return true;
                }
            }
            else if (this.minrank < 0)
            {
                if (this.minrank == -1)
                {
                    if (session.GetHabbo().CurrentRoom.CheckRights(session, false))
                    {
                        return true;
                    }
                }
                else if ((this.minrank == -2) && session.GetHabbo().CurrentRoom.CheckRights(session, true))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

