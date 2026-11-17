namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using System;
    using System.Collections.Generic;

    internal class TradeUser
    {
        private bool Accepted;
        internal List<UserItem> OfferedItems;
        private uint RoomId;
        internal uint UserId;

        internal TradeUser(uint UserId, uint RoomId)
        {
            this.UserId = UserId;
            this.RoomId = RoomId;
            this.Accepted = false;
            this.OfferedItems = new List<UserItem>();
        }

        internal GameClient GetClient()
        {
            return ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(this.UserId);
        }

        internal RoomUser GetRoomUser()
        {
            Room room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.RoomId);
            if (room == null)
            {
                return null;
            }
            return room.GetRoomUserManager().GetRoomUserByHabbo(this.UserId);
        }

        internal bool HasAccepted
        {
            get
            {
                return this.Accepted;
            }
            set
            {
                this.Accepted = value;
            }
        }
    }
}

