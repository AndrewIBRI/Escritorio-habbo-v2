namespace Butterfly.HabboHotel.Users.Authenticator
{
    using System;

    [Serializable]
    public class IncorrectLoginException : Exception
    {
        internal IncorrectLoginException(string Reason) : base(Reason)
        {
        }
    }
}

