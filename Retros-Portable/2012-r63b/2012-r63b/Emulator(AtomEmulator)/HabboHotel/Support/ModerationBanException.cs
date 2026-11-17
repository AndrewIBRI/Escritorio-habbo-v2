using System;

namespace Atom.HabboHotel.Support
{
    [Serializable()]
    public class ModerationBanException : Exception
    {
        internal ModerationBanException(string Reason) : base(Reason) { }
    }
}
