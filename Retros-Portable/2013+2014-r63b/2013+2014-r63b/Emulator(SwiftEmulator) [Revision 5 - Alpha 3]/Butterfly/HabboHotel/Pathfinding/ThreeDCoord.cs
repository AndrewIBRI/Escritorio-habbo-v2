namespace Butterfly.HabboHotel.Pathfinding
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct ThreeDCoord : IEquatable<ThreeDCoord>
    {
        internal int X;
        internal int Y;
        internal int Z;
        internal ThreeDCoord(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool Equals(ThreeDCoord comparedCoord)
        {
            return (((this.X == comparedCoord.X) && (this.Y == comparedCoord.Y)) && (this.Z == comparedCoord.Z));
        }

        public bool Equals(Point comparedCoord)
        {
            return ((this.X == comparedCoord.X) && (this.Y == comparedCoord.Y));
        }

        public static bool operator ==(ThreeDCoord a, ThreeDCoord b)
        {
            return (((a.X == b.X) && (a.Y == b.Y)) && (a.Z == b.Z));
        }

        public static bool operator !=(ThreeDCoord a, ThreeDCoord b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return ((this.X ^ this.Y) ^ this.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return base.GetHashCode().Equals(obj.GetHashCode());
        }
    }
}

