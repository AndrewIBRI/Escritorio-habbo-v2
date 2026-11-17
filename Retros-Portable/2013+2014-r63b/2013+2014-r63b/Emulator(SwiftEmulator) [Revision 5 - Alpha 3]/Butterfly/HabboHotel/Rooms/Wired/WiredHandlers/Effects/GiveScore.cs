namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Data;
    using System.Runtime.InteropServices;

    internal class GiveScore : IWiredEffect, IWiredTrigger
    {
        private int currentGameCount;
        private RoomEventDelegate delegateFunction;
        private GameManager gameManager;
        private uint itemID;
        private int maxCountPerGame;
        private int scoreToGive;

        public GiveScore(int maxCountPerGame, int scoreToGive, GameManager gameManager, uint itemID)
        {
            this.maxCountPerGame = maxCountPerGame;
            this.currentGameCount = 0;
            this.scoreToGive = scoreToGive;
            this.delegateFunction = new RoomEventDelegate(this.gameManager_OnGameStart);
            this.gameManager = gameManager;
            this.itemID = itemID;
            gameManager.OnGameStart += this.delegateFunction;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = ' " + this.itemID + "'");
        }

        public void Dispose()
        {
            this.gameManager.OnGameStart -= this.delegateFunction;
            this.gameManager = null;
            this.delegateFunction = null;
        }

        private void gameManager_OnGameStart(object sender, EventArgs e)
        {
            this.currentGameCount = 0;
        }

        public bool Handle(RoomUser user, Team team, RoomItem item)
        {
            if ((team != Team.none) && (this.maxCountPerGame > this.currentGameCount))
            {
                this.currentGameCount++;
                this.gameManager.AddPointToTeam(team, this.scoreToGive, user);
                this.gameManager.GetRoom().GetWiredHandler().OnEvent(this.itemID);
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
            dbClient.setQuery("SELECT trigger_data, trigger_data_2 FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int) this.itemID);
            DataRow row = dbClient.getRow();
            if (row != null)
            {
                this.maxCountPerGame = Convert.ToInt32(row[0].ToString());
                this.scoreToGive = Convert.ToInt32(row[1].ToString());
            }
            else
            {
                this.maxCountPerGame = 0;
                this.scoreToGive = 0;
            }
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int) this.itemID, "integer", this.scoreToGive.ToString(), this.maxCountPerGame.ToString(), false);
        }
    }
}

