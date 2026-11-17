using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.HabboHotel.GameClients;
namespace Atom.PacketQueue
{
    public class paqueteEnCola
    {
        public GameClient GameClient { get; set; }
        public byte[] datos { get; set; }
    }
}
