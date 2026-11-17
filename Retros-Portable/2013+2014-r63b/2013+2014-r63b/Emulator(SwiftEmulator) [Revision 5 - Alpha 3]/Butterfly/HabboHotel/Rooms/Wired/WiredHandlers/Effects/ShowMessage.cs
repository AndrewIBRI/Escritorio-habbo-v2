namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Runtime.InteropServices;

    internal class ShowMessage : IWiredTrigger, IWiredEffect
    {
        private WiredHandler handler;
        private uint itemID;
        private string message;

        public ShowMessage(string message, WiredHandler handler, uint itemID)
        {
            this.itemID = itemID;
            this.handler = handler;
            this.message = message;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.itemID + "'");
        }

        public void Dispose()
        {
            this.message = null;
        }

        public bool Handle(RoomUser user, Team team, RoomItem item)
        {
            if (((user != null) && !user.IsBot) && (user.GetClient() != null))
            {
                ServerMessage message = new ServerMessage();
                message.Init(Outgoing.Whisp);
                message.AppendInt32(user.VirtualId);
                message.AppendString(this.message);
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendInt32(0);
                message.AppendInt32(-1);
                user.GetClient().SendMessage(message);
                this.handler.OnEvent(this.itemID);
                return true;
            }
            return false;
        }

        public bool IsSpecial(out SpecialEffects function)
        {
            function = SpecialEffects.None;
            return false;
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.itemID);
            this.message = dbClient.getString();
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.itemID, "integer", string.Empty, this.message, false);
        }
    }
}

