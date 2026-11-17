namespace Butterfly.HabboHotel.Games.SnowStorm
{
    using Butterfly.HabboHotel.Rooms;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    internal class SnowModel
    {
        internal string HeightMap;
        internal int Id;
        internal int MapSizeX;
        internal int MapSizeY;
        internal List<Butterfly.HabboHotel.Games.SnowStorm.SnowItems> SnowItems;
        internal SquareState[,] SqState;

        internal SnowModel(DataRow dRow)
        {
            this.Id = (int) dRow["id"];
            this.HeightMap = (string) dRow["heightmap"];
            string[] strArray = this.HeightMap.Split(new char[] { Convert.ToChar(13) });
            this.MapSizeX = strArray[0].Length;
            this.MapSizeY = strArray.Length;
            this.SqState = new SquareState[this.MapSizeX, this.MapSizeY];
            for (int i = 0; i < this.MapSizeY; i++)
            {
                string str = strArray[i];
                str = str.Replace("\r", "").Replace("\n", "");
                int num2 = 0;
                foreach (char ch in str)
                {
                    if (ch == 'x')
                    {
                        this.SqState[num2, i] = SquareState.BLOCKED;
                    }
                    else
                    {
                        this.SqState[num2, i] = SquareState.OPEN;
                    }
                    num2++;
                }
            }
            this.SnowItems = new List<Butterfly.HabboHotel.Games.SnowStorm.SnowItems>();
        }

        internal string SerializeHeightMap()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.MapSizeY; i++)
            {
                for (int j = 0; j < this.MapSizeX; j++)
                {
                    if (this.SqState[j, i] == SquareState.BLOCKED)
                    {
                        builder.Append("x");
                    }
                    else
                    {
                        builder.Append("0");
                    }
                }
                builder.Append(Convert.ToChar(13));
            }
            return builder.ToString();
        }
    }
}

