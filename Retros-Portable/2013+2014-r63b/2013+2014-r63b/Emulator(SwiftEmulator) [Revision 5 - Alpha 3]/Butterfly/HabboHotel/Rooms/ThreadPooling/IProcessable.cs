namespace Butterfly.HabboHotel.Rooms.ThreadPooling
{
    using System;

    internal interface IProcessable
    {
        bool isLongRunTask();
        void ProcessLogic();
    }
}

