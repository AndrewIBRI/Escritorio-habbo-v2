namespace Butterfly.HabboHotel.Games.SnowStorm
{
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class SnowItems
    {
        internal string Extradata;
        internal int Id;
        internal int ItemId;
        internal int Level;
        internal string Name;
        internal int Rot;
        internal int SpriteId;
        internal int X;
        internal int Y;

        internal SnowItems()
        {
        }

        internal List<SnowItems> GetSnowItems(int Level, IQueryAdapter dbClient)
        {
            List<SnowItems> list = new List<SnowItems>();
            dbClient.setQuery("SELECT * FROM storm_items WHERE Snow_Level = '" + Level + "'");
            DataTable table = dbClient.getTable();
            foreach (DataRow row in table.Rows)
            {
                SnowItems item = new SnowItems {
                    Id = (int) row["Id"],
                    ItemId = (int) row["ItemId"],
                    Name = (string) row["Name"],
                    X = (int) row["X"],
                    Y = (int) row["Y"],
                    Rot = (int) row["Rot"],
                    SpriteId = (int) row["SpriteId"],
                    Extradata = (string) row["Extradata"],
                    Level = (int) row["Snow_Level"]
                };
                list.Add(item);
            }
            return list;
        }

        internal void SerializeItem(ServerMessage Message)
        {
            Message.AppendString(this.Name);
            Message.AppendInt32(this.ItemId);
            Message.AppendInt32(this.X);
            Message.AppendInt32(this.Y);
            Message.AppendInt32(this.Rot);
            Message.AppendInt32(1);
            Message.AppendInt32(this.SpriteId);
            Message.AppendInt32(this.Name.Contains("background") ? 1 : 0);
            Message.AppendInt32(0);
            Message.AppendBoolean(true);
            if (this.Name.Contains("background"))
            {
                Message.AppendInt32(1);
                Message.AppendInt32(5);
                Message.AppendString("state");
                Message.AppendString("0");
                Message.AppendString("offsetZ");
                Message.AppendString("9920");
                Message.AppendString("offsetY");
                Message.AppendString("1520");
                Message.AppendString("imageUrl");
                Message.AppendString(ButterflyEnvironment.HotelLink + "swfs/c_images/DEV_tests/snst_bg_2_big.png");
                Message.AppendString("offsetX");
                Message.AppendString("-1070");
            }
            else
            {
                Message.AppendInt32(0);
                Message.AppendString(this.Extradata);
            }
        }
    }
}

