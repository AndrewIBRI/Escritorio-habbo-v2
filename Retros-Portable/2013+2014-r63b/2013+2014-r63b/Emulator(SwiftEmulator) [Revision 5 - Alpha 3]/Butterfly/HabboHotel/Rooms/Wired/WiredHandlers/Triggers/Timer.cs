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

    internal class Timer : IWiredTrigger, IWiredCycleable, IWiredTimer
    {
        private int currentCycle;
        private RoomEventDelegate delegateFunction;
        private bool disposed;
        private WiredHandler handler;
        private RoomItem item;
        private int requiredCycles;

        public Timer(RoomItem item, WiredHandler handler, int cycleCount, GameManager gameManager)
        {
            this.item = item;
            this.handler = handler;
            this.requiredCycles = cycleCount;
            this.currentCycle = 0;
            this.delegateFunction = new RoomEventDelegate(this.gameManager_OnGameEnd);
            gameManager.OnGameEnd += this.delegateFunction;
            this.disposed = false;
        }

        private void continueTimer()
        {
            this.handler.RequestCycle(this);
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            this.disposed = true;
            this.handler.GetRoom().GetGameManager().OnGameEnd -= this.delegateFunction;
            this.item = null;
            this.handler = null;
        }

        public bool Disposed()
        {
            return this.disposed;
        }

        private void gameManager_OnGameEnd(object sender, EventArgs e)
        {
            this.continueTimer();
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.item.Id);
            DataRow row = dbClient.getRow();
            if (row != null)
            {
                this.requiredCycles = Convert.ToInt32(row[0].ToString());
            }
            else
            {
                this.requiredCycles = 20;
            }
        }

        public bool OnCycle()
        {
            if (this.requiredCycles > this.currentCycle)
            {
                this.handler.RequestStackHandle(this.item.Coordinate, null, null, Team.none);
                this.handler.OnEvent(this.item.Id);
                this.resetTimer();
                return false;
            }
            this.currentCycle++;
            return true;
        }

        private void resetTimer()
        {
            this.currentCycle = 0;
        }

        public void ResetTimer()
        {
            this.currentCycle = this.requiredCycles;
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.item.Id, "integer", string.Empty, this.requiredCycles.ToString(), false);
        }
    }
}

