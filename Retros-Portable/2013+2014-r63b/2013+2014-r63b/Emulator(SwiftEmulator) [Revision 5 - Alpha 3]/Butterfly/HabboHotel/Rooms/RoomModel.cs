namespace Butterfly.HabboHotel.Rooms
{
    using System;
    using System.Globalization;

    internal class RoomModel
    {
        internal bool ClubOnly;
        internal int DoorOrientation;
        internal int DoorX;
        internal int DoorY;
        internal double DoorZ;
        internal bool gotPublicPool;
        internal string Heightmap;
        internal int MapSizeX;
        internal int MapSizeY;
        internal byte[,] mRoomModelfx;
        internal short[,] SqFloorHeight;
        internal byte[,] SqSeatRot;
        internal SquareState[,] SqState;
        internal string StaticFurniMap;

        internal RoomModel(int DoorX, int DoorY, double DoorZ, int DoorOrientation, string Heightmap, string StaticFurniMap, bool ClubOnly, string Poolmap)
        {
            try
            {
                int num;
                string str;
                int num2;
                this.DoorX = DoorX;
                this.DoorY = DoorY;
                this.DoorZ = DoorZ;
                this.DoorOrientation = DoorOrientation;
                this.Heightmap = Heightmap.ToLower();
                this.StaticFurniMap = StaticFurniMap;
                this.gotPublicPool = !string.IsNullOrEmpty(Poolmap);
                string[] strArray = Heightmap.Split(new char[] { Convert.ToChar(13) });
                string[] strArray2 = Poolmap.Split(new char[] { Convert.ToChar(13) });
                this.MapSizeX = strArray[0].Length;
                this.MapSizeY = strArray.Length;
                this.ClubOnly = ClubOnly;
                this.SqState = new SquareState[this.MapSizeX, this.MapSizeY];
                this.SqFloorHeight = new short[this.MapSizeX, this.MapSizeY];
                this.SqSeatRot = new byte[this.MapSizeX, this.MapSizeY];
                if (this.gotPublicPool)
                {
                    this.mRoomModelfx = new byte[this.MapSizeX, this.MapSizeY];
                }
                for (num = 0; num < this.MapSizeY; num++)
                {
                    str = strArray[num];
                    str = str.Replace("\r", "").Replace("\n", "");
                    num2 = 0;
                    foreach (char ch in str)
                    {
                        if (ch == 'x')
                        {
                            this.SqState[num2, num] = SquareState.BLOCKED;
                        }
                        else
                        {
                            this.SqState[num2, num] = SquareState.OPEN;
                            this.SqFloorHeight[num2, num] = parse(ch);
                        }
                        num2++;
                    }
                }
                if (this.gotPublicPool)
                {
                    for (num = 0; num < this.MapSizeY; num++)
                    {
                        str = strArray2[num];
                        str = str.Replace("\r", "").Replace("\n", "");
                        num2 = 0;
                        foreach (char ch in str)
                        {
                            if (ch != '0')
                            {
                                this.mRoomModelfx[num2, num] = parseByte(ch);
                            }
                            num2++;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error during room modeldata loading for model " + Heightmap);
                throw exception;
            }
        }

        internal static bool isNumeric(string val, NumberStyles NumberStyle)
        {
            double num;
            return double.TryParse(val, NumberStyle, CultureInfo.CurrentCulture, out num);
        }

        internal static short parse(char input)
        {
            switch (input)
            {
                case '0':
                    return 0;

                case '1':
                    return 1;

                case '2':
                    return 2;

                case '3':
                    return 3;

                case '4':
                    return 4;

                case '5':
                    return 5;

                case '6':
                    return 6;

                case '7':
                    return 7;

                case '8':
                    return 8;

                case '9':
                    return 9;
            }
            throw new FormatException("The input was not in a correct format: input must be a number between 0 and 9");
        }

        internal static byte parseByte(char input)
        {
            switch (input)
            {
                case '0':
                    return 0;

                case '1':
                    return 1;

                case '2':
                    return 2;

                case '3':
                    return 3;

                case '4':
                    return 4;

                case '5':
                    return 5;

                case '6':
                    return 6;

                case '7':
                    return 7;

                case '8':
                    return 8;

                case '9':
                    return 9;
            }
            throw new FormatException("The input was not in a correct format: input must be a number between 0 and 9");
        }
    }
}

