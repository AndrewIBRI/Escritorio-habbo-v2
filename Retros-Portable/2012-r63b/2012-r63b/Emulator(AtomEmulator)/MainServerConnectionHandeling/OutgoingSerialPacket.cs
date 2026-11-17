using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atom.MainServerConnectionHandeling
{
    public class OutgoingSerialPacket : AbstractNoDataPacket
    {
        public OutgoingSerialPacket(string user, string serial) : base (SharedPacketLib.DataPackets.ServerOpCode.License_Response)
        {
            base.packet.WriteString(user);
            base.packet.WriteString(serial);
        }
    }
}
