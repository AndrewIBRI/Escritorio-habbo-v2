namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;

    internal class GameStarts : IWiredTrigger
    {
        private RoomEventDelegate gameStartsDeletgate;
        private WiredHandler handler;
        private RoomItem item;

        public GameStarts(RoomItem item, WiredHandler handler, GameManager gameManager)
        {
            this.item = item;
            this.handler = handler;
            this.gameStartsDeletgate = new RoomEventDelegate(this.gameManager_OnGameStart);
            gameManager.OnGameStart += this.gameStartsDeletgate;
        }

        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
        }

        public void Dispose()
        {
            this.handler.GetRoom().GetGameManager().OnGameStart -= this.gameStartsDeletgate;
            this.item = null;
            this.handler = null;
        }

        private void gameManager_OnGameStart(object sender, EventArgs e)
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

