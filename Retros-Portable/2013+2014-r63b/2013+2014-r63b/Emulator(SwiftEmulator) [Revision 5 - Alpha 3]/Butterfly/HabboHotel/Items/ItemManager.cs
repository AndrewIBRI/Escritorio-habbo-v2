namespace Butterfly.HabboHotel.Items
{
    using Butterfly.Core;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class ItemManager
    {
        private Dictionary<uint, Item> Items = new Dictionary<uint, Item>();

        internal ItemManager()
        {
        }

        internal bool ContainsItem(uint Id)
        {
            return this.Items.ContainsKey(Id);
        }

        internal Item GetItem(uint Id)
        {
            if (this.ContainsItem(Id))
            {
                return this.Items[Id];
            }
            return null;
        }

        internal Item GetItemSprite(int Id)
        {
            foreach (KeyValuePair<uint, Item> pair in this.Items)
            {
                if (pair.Value.SpriteId == Id)
                {
                    return pair.Value;
                }
            }
            return null;
        }

        internal void LoadItems(IQueryAdapter dbClient)
        {
            this.Items = new Dictionary<uint, Item>();
            dbClient.setQuery("SELECT * FROM items_base");
            DataTable table = dbClient.getTable();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        uint id = Convert.ToUInt16(row[0]);
                        int sprite = (int) row[1];
                        string publicName = (string) row[2];
                        string name = (string) row[3];
                        string str3 = (string) row[4];
                        int width = (int) row[5];
                        int length = (int) row[6];
                        double height = Convert.ToDouble(row[7]);
                        bool stackable = Convert.ToInt32(row[8]) == 1;
                        bool walkable = Convert.ToInt32(row[9]) == 1;
                        bool isSeat = Convert.ToInt32(row[10]) == 1;
                        bool allowRecycle = Convert.ToInt32(row[11]) == 1;
                        bool allowTrade = Convert.ToInt32(row[12]) == 1;
                        bool allowMarketplaceSell = Convert.ToInt32(row[13]) == 1;
                        bool allowGift = Convert.ToInt32(row[14]) == 1;
                        bool allowInventoryStack = Convert.ToInt32(row[15]) == 1;
                        bool allowGroupItem = Convert.ToInt32(row[0x10]) == 1;
                        InteractionType typeFromString = InterractionTypes.GetTypeFromString((string) row[0x11]);
                        int modes = (int) row[0x12];
                        string vendingIds = (string) row[0x13];
                        Item item = new Item(id, sprite, publicName, name, str3, width, length, height, stackable, walkable, isSeat, allowRecycle, allowTrade, allowMarketplaceSell, allowGift, allowInventoryStack, allowGroupItem, typeFromString, modes, vendingIds);
                        this.Items.Add(id, item);
                    }
                    catch
                    {
                        Console.ReadKey();
                        Logging.WriteLine("Could not load item #" + Convert.ToUInt32(row[0]) + ", please verify the data is okay.");
                    }
                }
            }
        }
    }
}

