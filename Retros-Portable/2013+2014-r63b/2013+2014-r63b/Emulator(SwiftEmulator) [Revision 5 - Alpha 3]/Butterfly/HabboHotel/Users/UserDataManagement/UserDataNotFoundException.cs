namespace Butterfly.HabboHotel.Users.UserDataManagement
{
    using System;

    internal class UserDataNotFoundException : Exception
    {
        public UserDataNotFoundException(string reason) : base(reason)
        {
        }
    }
}

