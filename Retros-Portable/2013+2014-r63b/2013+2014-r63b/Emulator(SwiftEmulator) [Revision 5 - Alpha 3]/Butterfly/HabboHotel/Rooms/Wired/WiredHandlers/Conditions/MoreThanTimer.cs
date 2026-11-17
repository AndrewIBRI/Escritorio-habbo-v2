namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Conditions
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Data;

    internal class MoreThanTimer : IWiredCondition, IWiredTrigger
    {
        private bool isDisposed = false;
        private RoomItem item;
        private Room room;
        private int timeout;

        public MoreThanTimer(int timeout, Room room, RoomItem item)
        {
            this.timeout = timeout;
            this.room = room;
            this.isDisposed = false;
            this.item = item;
        }

        public bool AllowsExecution(RoomUser user)
        {
            DateTime lastTimerReset = this.room.lastTimerReset;
            TimeSpan span = (TimeSpan) (DateTime.Now - this.room.lastTimerReset);
            return (span.TotalSeconds > this.timeout);
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            this.isDisposed = true;
            this.item = null;
            this.room = null;
        }

        public bool Disposed()
        {
            return this.isDisposed;
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.item.Id);
            DataRow row = dbClient.getRow();
            if (row != null)
            {
                this.timeout = Convert.ToInt32(row[0].ToString());
            }
            else
            {
                this.timeout = 20;
            }
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            if (dbClient.dbType == DatabaseType.MSSQL)
            {
                dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = " + this.item.Id);
                dbClient.setQuery("REPLACE INTO trigger_item SET trigger_id = @id, trigger_input = 'integer',  trigger_data = @trigger_data , all_user_triggerable = 0");
            }
            else
            {
                dbClient.setQuery("REPLACE INTO trigger_item SET trigger_id = @id, trigger_input = 'integer',  trigger_data = @trigger_data , all_user_triggerable = 0");
            }
            dbClient.addParameter("id", (int) this.item.Id);
            dbClient.addParameter("trigger_data", this.timeout);
            dbClient.runQuery();
        }
    }
}

