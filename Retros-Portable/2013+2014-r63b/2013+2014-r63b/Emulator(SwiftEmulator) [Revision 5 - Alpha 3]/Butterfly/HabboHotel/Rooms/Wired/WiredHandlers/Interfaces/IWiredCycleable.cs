namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces
{
    using System;

    internal interface IWiredCycleable
    {
        bool Disposed();
        bool OnCycle();
    }
}

