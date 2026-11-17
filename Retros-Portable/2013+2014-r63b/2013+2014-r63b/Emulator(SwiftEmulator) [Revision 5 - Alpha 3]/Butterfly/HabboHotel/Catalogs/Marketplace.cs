namespace Butterfly.HabboHotel.Catalogs
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    internal class Marketplace
    {
        internal static int CalculateComissionPrice(float SellingPrice)
        {
            return (int) Math.Ceiling((double) (SellingPrice / 100f));
        }

        internal static bool CanSellItem(UserItem Item)
        {
            if (!(Item.GetBaseItem().AllowTrade && Item.GetBaseItem().AllowMarketplaceSell))
            {
                return false;
            }
            return true;
        }

        internal static double FormatTimestamp()
        {
            return (double) (ButterflyEnvironment.GetUnixTimestamp() - 0x2a300);
        }

        internal static string FormatTimestampString()
        {
            return TextHandling.GetFirstSiffer(FormatTimestamp()).ToString();
        }

        internal static int[] GetAvarage(int FurniId)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery(string.Concat(new object[] { "SELECT * FROM catalog_marketplace_offers WHERE sprite_id = '", FurniId, "' AND state = '1' AND timestamp >= '", FormatTimestampString(), "'" }));
                DataTable table = adapter.getTable();
                int count = table.Rows.Count;
                int num2 = 0;
                foreach (DataRow row in table.Rows)
                {
                    num2 += (int) row["total_price"];
                }
                return new int[] { (num2 / count), count };
            }
        }

        internal static void SellItem(GameClient Session, int ItemId, int SellingPrice)
        {
            UserItem item = Session.GetHabbo().GetInventoryComponent().GetItem((uint) ItemId);
            if (!((item != null) && CanSellItem(item)))
            {
                ServerMessage message = new ServerMessage(Outgoing.AddMarket);
                message.AppendInt32(0);
                Session.SendMessage(message);
            }
            else
            {
                if (SellingPrice > 0x2710)
                {
                    SellingPrice = 0x2710;
                }
                int num = CalculateComissionPrice((float) SellingPrice);
                int num2 = SellingPrice + num;
                int num3 = 1;
                if (item.GetBaseItem().Type == 'i')
                {
                    num3++;
                }
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { 
                        "INSERT INTO catalog_marketplace_offers (item_id,user_id,asking_price,total_price,public_name,sprite_id,item_type,timestamp,extra_data) VALUES ('", item.BaseItem, "','", Session.GetHabbo().Id, "','", SellingPrice, "','", num2, "','", item.GetBaseItem().Name, "','", item.GetBaseItem().SpriteId, "','", num3, "','", ButterflyEnvironment.GetUnixTimestamp(), 
                        "','", item.ExtraData, "')"
                     }));
                }
                Session.GetHabbo().GetInventoryComponent().RemoveItem((uint) ItemId, false);
                Session.GetHabbo().GetInventoryComponent().RunDBUpdate();
                ServerMessage message2 = new ServerMessage(Outgoing.AddMarket);
                message2.AppendInt32(1);
                Session.SendMessage(message2);
            }
        }

        internal static ServerMessage SerializeOffers(int MinCost, int MaxCost, string SearchQuery, int FilterMode)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("WHERE state = '1' AND timestamp >= " + FormatTimestampString());
            if (MinCost >= 0)
            {
                builder.Append(" AND total_price > " + MinCost);
            }
            if (MaxCost >= 0)
            {
                builder.Append(" AND total_price < " + MaxCost);
            }
            if (SearchQuery.Length >= 1)
            {
                builder.Append(" AND public_name LIKE '%" + SearchQuery + "%'");
            }
            switch (FilterMode)
            {
                case 1:
                    builder.Append(" ORDER BY asking_price DESC");
                    break;

                case 2:
                    builder.Append(" ORDER BY asking_price ASC");
                    break;
            }
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM catalog_marketplace_offers " + builder.ToString());
                DataTable table = adapter.getTable();
                adapter.setQuery("SELECT COUNT(DISTINCT sprite_id) FROM catalog_marketplace_offers " + builder.ToString() + " LIMIT 250");
                int i = adapter.getInteger();
                int num2 = 0;
                List<int> list = new List<int>();
                ServerMessage message = new ServerMessage(Outgoing.GetOffers);
                message.AppendInt32(i);
                foreach (DataRow row in table.Rows)
                {
                    if (!list.Contains((int) row["sprite_id"]))
                    {
                        list.Add((int) row["sprite_id"]);
                        int[] avarage = GetAvarage((int) row["sprite_id"]);
                        message.AppendInt32(int.Parse(row["offer_id"].ToString()));
                        message.AppendInt32(1);
                        message.AppendInt32(1);
                        message.AppendInt32((int) row["sprite_id"]);
                        message.AppendInt32(0);
                        message.AppendString("");
                        message.AppendInt32((int) row["total_price"]);
                        message.AppendInt32(0);
                        message.AppendInt32(avarage[0]);
                        message.AppendInt32(avarage[1]);
                        num2 += avarage[1];
                    }
                }
                message.AppendInt32(num2);
                return message;
            }
        }

        internal static ServerMessage SerializeOwnOffers(int HabboId)
        {
            int i = 0;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT timestamp, state, offer_id, item_type, sprite_id, total_price FROM catalog_marketplace_offers WHERE user_id = " + HabboId);
                DataTable table = adapter.getTable();
                adapter.setQuery("SELECT SUM(asking_price) FROM catalog_marketplace_offers WHERE state = '2' AND user_id = " + HabboId);
                string s = adapter.getString();
                if (s.Length > 0)
                {
                    i = int.Parse(s);
                }
                ServerMessage message = new ServerMessage(Outgoing.MySells);
                message.AppendInt32(i);
                if (table != null)
                {
                    message.AppendInt32(table.Rows.Count);
                    foreach (DataRow row in table.Rows)
                    {
                        int num2 = (int) Math.Floor((double) (((((double) row["timestamp"]) + 172800.0) - ButterflyEnvironment.GetUnixTimestamp()) / 60.0));
                        int num3 = int.Parse(row["state"].ToString());
                        if (num2 <= 0)
                        {
                            num3 = 3;
                            num2 = 0;
                        }
                        message.AppendInt32(Convert.ToUInt32(row["offer_id"]));
                        message.AppendInt32(num3);
                        message.AppendInt32(int.Parse(row["item_type"].ToString()));
                        message.AppendInt32((int) row["sprite_id"]);
                        message.AppendInt32(0);
                        message.AppendString("");
                        message.AppendInt32((int) row["total_price"]);
                        message.AppendInt32(num2);
                        message.AppendInt32((int) row["sprite_id"]);
                    }
                }
                else
                {
                    message.AppendInt32(0);
                }
                return message;
            }
        }
    }
}

