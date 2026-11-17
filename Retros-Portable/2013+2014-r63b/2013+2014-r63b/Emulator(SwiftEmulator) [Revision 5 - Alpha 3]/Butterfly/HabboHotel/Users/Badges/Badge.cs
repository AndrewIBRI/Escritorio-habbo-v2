namespace Butterfly.HabboHotel.Users.Badges
{
    using System;

    internal class Badge
    {
        internal string Code;
        internal int Slot;

        internal Badge(string Code, int Slot)
        {
            this.Code = Code;
            this.Slot = Slot;
        }
    }
}

