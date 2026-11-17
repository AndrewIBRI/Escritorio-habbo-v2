namespace Butterfly.HabboHotel.Items
{
    using Butterfly;
    using System;

    internal class WallCoordinate
    {
        private readonly int lengthX;
        private readonly int lengthY;
        private readonly char side;
        private readonly int widthX;
        private readonly int widthY;

        public WallCoordinate(string wallPosition)
        {
            string[] strArray = wallPosition.Split(new char[] { ' ' });
            if (strArray[2] == "l")
            {
                this.side = 'l';
            }
            else
            {
                this.side = 'r';
            }
            string[] strArray2 = strArray[0].Substring(3).Split(new char[] { ',' });
            this.widthX = TextHandling.Parse2(strArray2[0]);
            this.widthY = TextHandling.Parse2(strArray2[1]);
            string[] strArray3 = strArray[1].Substring(2).Split(new char[] { ',' });
            this.lengthX = TextHandling.Parse2(strArray3[0]);
            this.lengthY = TextHandling.Parse2(strArray3[1]);
        }

        public WallCoordinate(double x, double y, sbyte n)
        {
            TextHandling.Split(x, out this.widthX, out this.widthY);
            TextHandling.Split(y, out this.lengthX, out this.lengthY);
            if (n == 7)
            {
                this.side = 'r';
            }
            else
            {
                this.side = 'l';
            }
        }

        internal string GenerateDBShit()
        {
            return string.Concat(new object[] { "x: ", TextHandling.Combine(this.widthX, this.widthY), " y: ", TextHandling.Combine(this.lengthX, this.lengthY) });
        }

        internal double GetXValue()
        {
            return TextHandling.Combine(this.widthX, this.widthY);
        }

        internal double GetYValue()
        {
            return TextHandling.Combine(this.lengthX, this.lengthY);
        }

        internal int n()
        {
            if (this.side == 'l')
            {
                return 8;
            }
            return 7;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { ":w=", this.widthX, ",", this.widthY, " l=", this.lengthX, ",", this.lengthY, " ", this.side });
        }
    }
}

