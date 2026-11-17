using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atom.MainServerConnectionHandeling
{
    public class ServerFullMessage : AbstractNoDataPacket
    {
        public ServerFullMessage() : base(SharedPacketLib.DataPackets.ServerOpCode.Server_Full) { }
    }
}
