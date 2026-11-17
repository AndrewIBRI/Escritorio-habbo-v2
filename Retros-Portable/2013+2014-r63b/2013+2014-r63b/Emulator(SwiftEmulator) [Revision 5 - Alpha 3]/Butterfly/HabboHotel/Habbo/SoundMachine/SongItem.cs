namespace Butterfly.HabboHotel.Habbo.SoundMachine
{
    using Butterfly;
    using Butterfly.HabboHotel.Items;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;

    internal class SongItem
    {
        public Item baseItem;
        public int itemID;
        public int songID;

        public SongItem(UserItem item)
        {
            this.itemID = (int) item.Id;
            this.songID = TextHandling.Parse2(item.ExtraData);
            this.baseItem = item.GetBaseItem();
        }

        public SongItem(int itemID, int songID, int baseItem)
        {
            this.itemID = itemID;
            this.songID = songID;
            this.baseItem = ButterflyEnvironment.GetGame().GetItemManager().GetItem((uint) baseItem);
        }

        public void RemoveFromDatabase()
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("DELETE FROM items_rooms_songs WHERE itemid = " + this.itemID);
                adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO items (item_id, base_id) VALUES ('", this.itemID, "','", this.baseItem.ItemId, "')" }));
            }
        }

        public void SaveToDatabase(int roomID)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO items_rooms_songs VALUES (", this.itemID, ",", roomID, ",", this.songID, ",", this.baseItem.ItemId, ")" }));
                adapter.runFastQuery("DELETE FROM items WHERE item_id = '" + this.itemID + "'");
            }
        }
    }
}

