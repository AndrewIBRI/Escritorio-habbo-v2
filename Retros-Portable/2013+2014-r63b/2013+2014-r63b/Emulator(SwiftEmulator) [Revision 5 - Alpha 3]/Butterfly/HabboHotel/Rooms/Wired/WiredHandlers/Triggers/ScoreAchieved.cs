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

    internal class ScoreAchieved : IWiredTrigger
    {
        private RoomEventDelegate gameEndDeletgate;
        private WiredHandler handler;
        private RoomItem item;
        private TeamScoreChangedDelegate scoreChangedDelegate;
        private int scoreLevel;
        private bool used;

        public ScoreAchieved(RoomItem item, WiredHandler handler, int scoreLevel, GameManager gameManager)
        {
            this.item = item;
            this.handler = handler;
            this.scoreLevel = scoreLevel;
            this.used = false;
            this.scoreChangedDelegate = new TeamScoreChangedDelegate(this.gameManager_OnScoreChanged);
            this.gameEndDeletgate = new RoomEventDelegate(this.gameManager_OnGameEnd);
            gameManager.OnScoreChanged += this.scoreChangedDelegate;
            gameManager.OnGameEnd += this.gameEndDeletgate;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
        }

        public void Dispose()
        {
            this.handler.GetRoom().GetGameManager().OnScoreChanged -= this.scoreChangedDelegate;
            this.handler.GetRoom().GetGameManager().OnGameEnd -= this.gameEndDeletgate;
            this.item = null;
            this.handler = null;
        }

        private void gameManager_OnGameEnd(object sender, EventArgs e)
        {
            this.used = false;
        }

        private void gameManager_OnScoreChanged(object sender, TeamScoreChangedArgs e)
        {
            if (!((e.Points <= this.scoreLevel) || this.used))
            {
                this.used = true;
                this.handler.RequestStackHandle(this.item.Coordinate, null, e.user, e.Team);
                this.handler.OnEvent(this.item.Id);
            }
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.item.Id);
            DataRow row = dbClient.getRow();
            if (row != null)
            {
                this.scoreLevel = Convert.ToInt32(row[0].ToString());
            }
            else
            {
                this.scoreLevel = 200;
            }
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.item.Id, "integer", string.Empty, this.scoreLevel.ToString(), false);
        }
    }
}

