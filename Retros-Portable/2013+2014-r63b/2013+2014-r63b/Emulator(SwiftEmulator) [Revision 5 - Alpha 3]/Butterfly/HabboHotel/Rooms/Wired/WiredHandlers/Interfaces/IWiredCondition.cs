namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces
{
    using Butterfly.HabboHotel.Rooms;
    using System;

    internal interface IWiredCondition : IWiredTrigger
    {
        bool AllowsExecution(RoomUser user);
    }
}

