namespace Butterfly.HabboHotel.Pathfinding
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct HeightInfo
    {
        private double[,] mMap;
        private int mMaxX;
        private int mMaxY;
        internal HeightInfo(int MaxX, int MaxY, double[,] Map)
        {
            this.mMap = Map;
            this.mMaxX = MaxX;
            this.mMaxY = MaxY;
        }

        internal double GetState(int x, int y)
        {
            if ((x >= this.mMaxX) || (x < 0))
            {
                return 0.0;
            }
            if ((y >= this.mMaxY) || (y < 0))
            {
                return 0.0;
            }
            return this.mMap[x, y];
        }
    }
}

