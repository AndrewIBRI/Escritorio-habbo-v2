namespace Butterfly.HabboHotel.Advertisements
{
    using Butterfly;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class AdvertisementManager
    {
        internal List<RoomAdvertisement> RoomAdvertisements = new List<RoomAdvertisement>();

        internal AdvertisementManager()
        {
        }

        internal RoomAdvertisement GetRandomRoomAdvertisement()
        {
            if (this.RoomAdvertisements.Count <= 0)
            {
                return null;
            }
            while (true)
            {
                int randomNumber = ButterflyEnvironment.GetRandomNumber(0, this.RoomAdvertisements.Count - 1);
                if (!((this.RoomAdvertisements[randomNumber] == null) || this.RoomAdvertisements[randomNumber].ExceededLimit))
                {
                    return this.RoomAdvertisements[randomNumber];
                }
            }
        }

        internal void LoadRoomAdvertisements(IQueryAdapter dbClient)
        {
            this.RoomAdvertisements.Clear();
            dbClient.setQuery("SELECT * FROM room_ads WHERE enabled = 1");
            DataTable table = dbClient.getTable();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    this.RoomAdvertisements.Add(new RoomAdvertisement(Convert.ToUInt32(row["id"]), (string) row["ad_image"], (string) row["ad_link"], (int) row["views"], (int) row["views_limit"]));
                }
            }
        }
    }
}

