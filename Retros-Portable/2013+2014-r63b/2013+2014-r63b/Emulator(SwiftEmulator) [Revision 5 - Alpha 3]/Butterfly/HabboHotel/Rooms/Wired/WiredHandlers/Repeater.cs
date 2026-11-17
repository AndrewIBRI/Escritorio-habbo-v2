namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;

    public class Repeater : IWiredTrigger, IWiredCycleable, IWiredTimer
    {
        private int cycleCount;
        private int cyclesRequired;
        private bool disposed;
        private WiredHandler handler;
        private RoomItem item;

        public Repeater(WiredHandler handler, RoomItem item, int cyclesRequired)
        {
            this.handler = handler;
            this.cyclesRequired = cyclesRequired;
            this.item = item;
            handler.RequestCycle(this);
            this.disposed = false;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            this.disposed = true;
            this.handler = null;
            this.item = null;
        }

        public bool Disposed()
        {
            return this.disposed;
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.item.Id);
            this.cyclesRequired = dbClient.getInteger();
        }

        public bool OnCycle()
        {
            this.cycleCount++;
            if (this.cycleCount > this.cyclesRequired)
            {
                this.handler.RequestStackHandle(this.item.Coordinate, null, null, Team.none);
                this.handler.OnEvent(this.item.Id);
                this.cycleCount = 0;
            }
            return true;
        }

        public void ResetTimer()
        {
            this.cycleCount = 0;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.item.Id, "integer", string.Empty, this.cyclesRequired.ToString(), false);
        }
    }
}

