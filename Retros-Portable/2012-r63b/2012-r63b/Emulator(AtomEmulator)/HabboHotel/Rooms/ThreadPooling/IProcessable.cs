using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atom.HabboHotel.Rooms.ThreadPooling
{
    interface IProcessable
    {
        void ProcessLogic();
        bool isLongRunTask();
    }
}
