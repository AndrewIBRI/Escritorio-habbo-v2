namespace Butterfly.HabboHotel.Support
{
    using System;

    [Serializable]
    public class ModerationBanException : Exception
    {
        internal ModerationBanException(string Reason) : base(Reason)
        {
        }
    }
}

