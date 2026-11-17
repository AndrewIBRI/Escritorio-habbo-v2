namespace Butterfly.HabboHotel.Rooms.HeightMap
{
    using Butterfly.HabboHotel.Rooms;
    using System;

    internal class HeightMapUpdateParser
    {
        public string parseUpdate(RoomModel model, int x, int y, string oldStatus, string newStatus)
        {
            string str = "";
            int num = x + (y * model.MapSizeX);
            int num2 = num / 0x5f;
            int num3 = num % 0x5f;
            while (num2 > 0)
            {
                str = str + "!" + '\x007f';
            }
            return ((str + "!" + ((char) (num3 + 0x20))) + newStatus + oldStatus);
        }
    }
}

