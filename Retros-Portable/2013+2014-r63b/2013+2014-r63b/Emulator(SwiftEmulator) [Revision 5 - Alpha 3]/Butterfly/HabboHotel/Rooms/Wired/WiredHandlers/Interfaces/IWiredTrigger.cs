namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces
{
    using Butterfly.HabboHotel.Rooms;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;

    internal interface IWiredTrigger
    {
        void DeleteFromDatabase(IQueryAdapter dbClient);
        void Dispose();
        void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom);
        void SaveToDatabase(IQueryAdapter dbClient);
    }
}

