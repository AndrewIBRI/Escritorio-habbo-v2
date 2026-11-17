using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atom.MainServerConnectionHandeling
{
    public class PingRepsonse : AbstractNoDataPacket
    {
        public PingRepsonse() : base(SharedPacketLib.DataPackets.ServerOpCode.Ping_Response) { }
    }
}
