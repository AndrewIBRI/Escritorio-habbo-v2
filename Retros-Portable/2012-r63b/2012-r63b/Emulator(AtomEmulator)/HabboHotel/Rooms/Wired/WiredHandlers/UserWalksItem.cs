using Atom.HabboHotel.Items;

namespace Atom.HabboHotel.Rooms.Wired.WiredHandlers
{
    struct UserWalksFurniValue
    {
        internal readonly RoomUser user;
        internal readonly RoomItem item;

        public UserWalksFurniValue(RoomUser user, RoomItem item)
        {
            this.user = user;
            this.item = item;
        }
    }
}
