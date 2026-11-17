namespace Butterfly.Util
{
    using System;
    using System.Drawing;

    internal class CoordinationUtil
    {
        internal static double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow((double) (a.X - b.X), 2.0) + Math.Pow((double) (a.Y - b.Y), 2.0));
        }

        internal static Point GetPointBehind(Point point, int rot)
        {
            return GetPointInFront(point, RotationIverse(rot));
        }

        internal static Point GetPointInFront(Point point, int rot)
        {
            Point point2 = new Point(point.X, point.Y);
            if (rot == 0)
            {
                point2.Y--;
                return point2;
            }
            if (rot == 1)
            {
                point2.X--;
                point2.Y--;
                return point2;
            }
            if (rot == 2)
            {
                point2.X++;
                return point2;
            }
            if (rot == 3)
            {
                point2.X--;
                point2.Y++;
                return point2;
            }
            if (rot == 4)
            {
                point2.Y++;
                return point2;
            }
            if (rot == 5)
            {
                point2.X++;
                point2.Y++;
                return point2;
            }
            if (rot == 6)
            {
                point2.X--;
                return point2;
            }
            if (rot == 7)
            {
                point2.X++;
                point2.Y--;
            }
            return point2;
        }

        internal static int RotationIverse(int rot)
        {
            if (rot > 3)
            {
                rot -= 4;
                return rot;
            }
            rot += 4;
            return rot;
        }
    }
}

