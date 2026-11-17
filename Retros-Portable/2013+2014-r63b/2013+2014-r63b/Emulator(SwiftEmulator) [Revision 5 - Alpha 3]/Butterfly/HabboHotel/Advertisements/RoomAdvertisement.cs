namespace Butterfly.HabboHotel.Advertisements
{
    using Butterfly;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;

    internal class RoomAdvertisement
    {
        internal string AdImage;
        internal string AdLink;
        internal uint Id;
        internal int Views;
        internal int ViewsLimit;

        internal RoomAdvertisement(uint Id, string AdImage, string AdLink, int Views, int ViewsLimit)
        {
            this.Id = Id;
            this.AdImage = AdImage;
            this.AdLink = AdLink;
            this.Views = Views;
            this.ViewsLimit = ViewsLimit;
        }

        internal void OnView()
        {
            this.Views++;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE room_ads SET views = views + 1 WHERE id = " + this.Id);
            }
        }

        internal bool ExceededLimit
        {
            get
            {
                if (this.ViewsLimit <= 0)
                {
                    return false;
                }
                return (this.Views >= this.ViewsLimit);
            }
        }
    }
}

