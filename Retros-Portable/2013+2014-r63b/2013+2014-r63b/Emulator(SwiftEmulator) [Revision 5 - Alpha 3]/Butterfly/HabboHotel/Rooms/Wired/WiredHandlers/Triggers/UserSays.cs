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
    using System.Runtime.InteropServices;

    internal class UserSays : IWiredTrigger
    {
        private RoomUserSaysDelegate delegateFunction;
        private WiredHandler handler;
        private bool isOwnerOnly;
        private RoomItem item;
        private string triggerMessage;

        public UserSays(RoomItem item, WiredHandler handler, bool isOwnerOnly, string triggerMessage, Room room)
        {
            this.item = item;
            this.handler = handler;
            this.isOwnerOnly = isOwnerOnly;
            this.triggerMessage = triggerMessage;
            this.delegateFunction = new RoomUserSaysDelegate(this.roomUserManager_OnUserSays);
            room.OnUserSays += this.delegateFunction;
        }

        private bool canBeTriggered(string message)
        {
            return (message.ToLower() == this.triggerMessage.ToLower());
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            this.handler.GetRoom().OnUserSays -= this.delegateFunction;
            this.item = null;
            this.handler = null;
            this.triggerMessage = null;
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data,  all_user_triggerable FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.item.Id);
            DataRow row = dbClient.getRow();
            if (row != null)
            {
                this.triggerMessage = row[0].ToString();
                this.isOwnerOnly = row[1].ToString() == "1";
            }
            else
            {
                this.triggerMessage = string.Empty;
                this.isOwnerOnly = false;
            }
        }

        private void roomUserManager_OnUserSays(object sender, UserSaysArgs e, out bool messageHandled)
        {
            RoomUser user = e.user;
            string message = e.message;
            if ((!this.isOwnerOnly && this.canBeTriggered(message)) || ((this.isOwnerOnly && user.IsOwner()) && this.canBeTriggered(message)))
            {
                this.handler.RequestStackHandle(this.item.Coordinate, null, user, Team.none);
                this.handler.OnEvent(this.item.Id);
                messageHandled = true;
            }
            else
            {
                messageHandled = false;
            }
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.item.Id, "string", string.Empty, this.triggerMessage, this.isOwnerOnly);
        }
    }
}

