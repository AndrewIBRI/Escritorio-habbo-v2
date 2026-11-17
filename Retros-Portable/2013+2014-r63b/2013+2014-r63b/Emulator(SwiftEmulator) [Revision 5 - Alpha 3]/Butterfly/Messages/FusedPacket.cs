namespace Butterfly.Messages
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct FusedPacket
    {
        internal readonly ServerMessage content;
        internal readonly string requirements;
        public FusedPacket(ServerMessage content, string requirements)
        {
            this.content = content;
            this.requirements = requirements;
        }
    }
}

