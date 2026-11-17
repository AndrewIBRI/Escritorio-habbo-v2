namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;

    internal class GameEnds : IWiredTrigger
    {
        private RoomEventDelegate gameEndsDeletgate;
        private WiredHandler handler;
        private RoomItem item;

        public GameEnds(RoomItem item, WiredHandler handler, GameManager gameManager)
        {
            this.item = item;
            this.handler = handler;
            this.gameEndsDeletgate = new RoomEventDelegate(this.gameManager_OnGameEnd);
            gameManager.OnGameEnd += this.gameEndsDeletgate;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
        }

        public void Dispose()
        {
            this.handler.GetRoom().GetGameManager().OnGameEnd -= this.gameEndsDeletgate;
            this.item = null;
            this.handler = null;
        }

        private void gameManager_OnGameEnd(object sender, EventArgs e)
        {
            this.handler.RequestStackHandle(this.item.Coordinate, null, null, Team.none);
            this.handler.OnEvent(this.item.Id);
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
        }

        public void SaveToDatabase(IQueryAdapter dbClient)
        {
        }
    }
}

