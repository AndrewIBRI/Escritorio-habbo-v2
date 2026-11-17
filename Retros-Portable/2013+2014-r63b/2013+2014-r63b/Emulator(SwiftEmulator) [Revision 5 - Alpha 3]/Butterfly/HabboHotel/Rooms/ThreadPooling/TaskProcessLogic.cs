namespace Butterfly.HabboHotel.Rooms.ThreadPooling
{
    using System;

    internal class TaskProcessLogic
    {
        internal static void processMessage(IProcessable information)
        {
            information.ProcessLogic();
        }
    }
}

