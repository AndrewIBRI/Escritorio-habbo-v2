namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Data;

    internal class EntersRoom : IWiredTrigger
    {
        private RoomEventDelegate delegateFunction;
        private WiredHandler handler;
        private bool isOneUser;
        private RoomItem item;
        private string userName;

        public EntersRoom(RoomItem item, WiredHandler handler, RoomUserManager roomUserManager, bool isOneUser, string userName)
        {
            this.item = item;
            this.handler = handler;
            this.isOneUser = isOneUser;
            this.userName = userName;
            this.delegateFunction = new RoomEventDelegate(this.roomUserManager_OnUserEnter);
            roomUserManager.OnUserEnter += this.delegateFunction;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            this.handler = null;
            this.userName = null;
            if ((this.item != null) && (this.item.GetRoom() != null))
            {
                this.item.GetRoom().GetRoomUserManager().OnUserEnter -= this.delegateFunction;
            }
            this.item = null;
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.item.Id);
            DataRow row = dbClient.getRow();
            if (row != null)
            {
                this.userName = row[0].ToString();
            }
            else
            {
                this.userName = string.Empty;
            }
            this.isOneUser = !string.IsNullOrEmpty(this.userName);
        }

        private void roomUserManager_OnUserEnter(object sender, EventArgs e)
        {
            RoomUser user = (RoomUser) sender;
            if (!(((user.IsBot || !this.isOneUser) || (string.IsNullOrEmpty(this.userName) || (user.GetUsername() != this.userName))) && this.isOneUser))
            {
                this.handler.OnEvent(this.item.Id);
                this.handler.RequestStackHandle(this.item.Coordinate, null, user, Team.none);
            }
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.item.Id, "integer", string.Empty, this.userName, false);
        }
    }
}

