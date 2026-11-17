namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers;
    using System;
    using System.Runtime.InteropServices;

    internal interface IWiredEffect
    {
        bool Handle(RoomUser user, Team team, RoomItem item);
        bool IsSpecial(out SpecialEffects action);
    }
}

