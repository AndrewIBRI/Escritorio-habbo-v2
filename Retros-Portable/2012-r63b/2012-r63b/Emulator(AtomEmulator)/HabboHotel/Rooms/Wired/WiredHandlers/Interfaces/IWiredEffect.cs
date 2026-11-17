using Atom.HabboHotel.Items;
using Atom.HabboHotel.Rooms.Games;

namespace Atom.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces
{
    interface IWiredEffect
    {
        bool Handle(RoomUser user, Team team, RoomItem item);
        bool IsSpecial(out SpecialEffects action);
    }
}
