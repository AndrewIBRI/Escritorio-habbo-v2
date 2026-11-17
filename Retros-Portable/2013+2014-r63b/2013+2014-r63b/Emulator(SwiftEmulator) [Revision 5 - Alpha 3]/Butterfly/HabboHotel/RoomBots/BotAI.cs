namespace Butterfly.HabboHotel.RoomBots
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Rooms;
    using System;

    internal abstract class BotAI
    {
        internal int BaseId;
        private Room room;
        private uint RoomId;
        private RoomUser roomUser;
        private int RoomUserId;

        internal BotAI()
        {
        }

        internal RoomBot GetBotData()
        {
            if (this.GetRoomUser() == null)
            {
                return null;
            }
            return this.GetRoomUser().BotData;
        }

        internal Room GetRoom()
        {
            return this.room;
        }

        internal RoomUser GetRoomUser()
        {
            return this.roomUser;
        }

        internal void Init(int pBaseId, int pRoomUserId, uint pRoomId, RoomUser user, Room room)
        {
            this.BaseId = pBaseId;
            this.RoomUserId = pRoomUserId;
            this.RoomId = pRoomId;
            this.roomUser = user;
            this.room = room;
        }

        internal abstract void OnSelfEnterRoom();
        internal abstract void OnSelfLeaveRoom(bool Kicked);
        internal abstract void OnTimerTick();
        internal abstract void OnUserEnterRoom(RoomUser User);
        internal abstract void OnUserLeaveRoom(GameClient Client);
        internal abstract void OnUserSay(RoomUser User, string Message);
        internal abstract void OnUserShout(RoomUser User, string Message);
    }
}

