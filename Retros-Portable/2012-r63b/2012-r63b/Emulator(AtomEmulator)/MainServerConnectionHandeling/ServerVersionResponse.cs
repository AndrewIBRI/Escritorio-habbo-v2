using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atom.MainServerConnectionHandeling
{
    public class ServerVersionResponse : AbstractNoDataPacket
    {
        public ServerVersionResponse(int versionNumber) : base(SharedPacketLib.DataPackets.ServerOpCode.Core_client_sends_version)
        {
            base.packet.WriteInt(versionNumber);
        }

    }
}
