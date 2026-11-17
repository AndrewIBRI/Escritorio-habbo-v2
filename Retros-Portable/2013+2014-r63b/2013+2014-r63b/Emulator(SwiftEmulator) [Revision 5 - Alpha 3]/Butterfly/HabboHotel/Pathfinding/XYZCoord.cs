namespace Butterfly.HabboHotel.Pathfinding
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct XYZCoord
    {
        internal int X;
        internal int Y;
        internal double Z;
        public XYZCoord(int x, int y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}

