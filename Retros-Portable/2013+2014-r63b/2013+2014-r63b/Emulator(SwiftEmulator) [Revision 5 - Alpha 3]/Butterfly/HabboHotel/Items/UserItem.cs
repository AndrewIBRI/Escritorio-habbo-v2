namespace Butterfly.HabboHotel.Items
{
    using Butterfly;
    using Butterfly.HabboHotel.Habbo.Guilds;
    using Butterfly.Collections;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Catalogs;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.RoomIvokedItems;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.RoomBots;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using Butterfly.Messages;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    internal class UserItem
    {
        internal uint BaseItem;
        internal string ExtraData;
        internal string Group_data;
        internal List<string> Guilds_Data = new List<string>();
        internal uint Id;
        internal bool isWallItem;
        private Item mBaseItem;
        internal uint RareId;
        internal uint placedBy;
        
        internal UserItem(uint Id, uint BaseItem, string ExtraData, uint rareid, string groupstring, uint placedBy)
        {
            this.Id = Id;
            this.placedBy = placedBy;
            this.BaseItem = BaseItem;
            this.ExtraData = ExtraData;
            this.mBaseItem = this.GetBaseItem();
            if (this.mBaseItem == null)
            {
            }
            this.isWallItem = this.mBaseItem.Type == 'i';
            this.RareId = rareid;
            this.Group_data = groupstring;
            if (this.Group_data.Length > 0)
            {
                foreach (string str in this.Group_data.Split(new char[] { ';' }))
                {
                    this.Guilds_Data.Add(str);
                }
            }
        }

        internal Item GetBaseItem()
        {
            return ButterflyEnvironment.GetGame().GetItemManager().GetItem(this.BaseItem);
        }

        internal CatalogItem GetRareCatalogItem(uint ItemId)
        {
            return ButterflyEnvironment.GetGame().GetCatalog().RaresItems[ItemId];
        }

        internal void SerializeFloor(ServerMessage Message, bool Inventory)
        {
            if ((this.GetBaseItem().InteractionType == InteractionType.gift)&&(Inventory))
            {
                int ribbon, sprite, color;
                try
                {
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        DataRow row;
                        adapter.setQuery("SELECT * FROM user_gifts WHERE inventory_id = " + this.Id + " LIMIT 1");
                        row = adapter.getRow();
                        ribbon = Convert.ToInt32(row["ribbon"]);
                        sprite = Convert.ToInt32(row["gift_sprite"]);
                        color = Convert.ToInt32(row["color"]);
                    }
                    Message.AppendInt32((-1) * (int)this.Id);
                    Message.AppendString(this.mBaseItem.Type.ToString().ToUpper());
                    Message.AppendInt32(this.Id);
                    Message.AppendInt32(sprite);
                    Message.AppendInt32(9);
                    Message.AppendInt32(0);
                    Message.AppendString(this.ExtraData);
                    Message.AppendBoolean(this.GetBaseItem().AllowRecycle);
                    Message.AppendBoolean(this.GetBaseItem().AllowTrade);
                    Message.AppendBoolean(this.GetBaseItem().AllowInventoryStack);
                    Message.AppendBoolean(Marketplace.CanSellItem(this));
                    Message.AppendInt32(-1);
                    Message.AppendBoolean(false);
                    Message.AppendInt32(-1);
                    Message.AppendString("");
                    Message.AppendInt32((ribbon * 1000) + color);
                }
                catch
                {
                    this.SerializeFloor(Message, false);
                }
            }
            else if (this.GetBaseItem().Name == "boutique_mannequin1")
            {
                Message.AppendInt32(this.Id);
                Message.AppendString(this.mBaseItem.Type.ToString().ToUpper());
                Message.AppendInt32(this.Id);
                Message.AppendInt32(this.GetBaseItem().SpriteId);
                Message.AppendInt32(1);
                Message.AppendInt32(1);
                Message.AppendInt32(3);


                if (ExtraData != "")
                {
                    Message.AppendString("GENDER");
                    Message.AppendString(ExtraData.Split(Convert.ToChar(9))[1]);
                    Message.AppendString("FIGURE");
                    Message.AppendString(ExtraData.Split(Convert.ToChar(9))[2]);
                    Message.AppendString("OUTFIT_NAME");
                    Message.AppendString(ExtraData.Split(Convert.ToChar(9))[0]);
                }
                else
                {
                    Message.AppendString("GENDER");
                    Message.AppendString("m");
                    Message.AppendString("FIGURE");
                    Message.AppendString("ch-210-62.lg-270-62");
                    Message.AppendString("OUTFIT_NAME");
                    Message.AppendString("");
                }
                Message.AppendBoolean(GetBaseItem().AllowRecycle);
                Message.AppendBoolean(GetBaseItem().AllowTrade);
                Message.AppendBoolean(GetBaseItem().AllowInventoryStack);
                Message.AppendBoolean(Marketplace.CanSellItem(this));
                Message.AppendInt32(-1);
                Message.AppendBoolean(false);
                Message.AppendInt32(-1);
                Message.AppendString("");
                Message.AppendInt32(0);
            }

            else
            {

                Message.AppendInt32(this.Id);
                Message.AppendString(this.mBaseItem.Type.ToString().ToUpper());
                Message.AppendInt32(this.Id);
                Message.AppendInt32(this.GetBaseItem().SpriteId);
                if (this.mBaseItem.AllowGroupItem)
                {
                    Message.AppendInt32(0x11);
                }
                else if (this.mBaseItem.InteractionType == InteractionType.musicdisc)
                {
                    Message.AppendInt32(8);
                }
                else
                {
                    Message.AppendInt32(1);
                }
                if (this.RareId > 0)
                {
                    Message.AppendString("");
                    Message.AppendBoolean(true);
                    Message.AppendBoolean(false);
                }
                else if (this.mBaseItem.AllowGroupItem)
                {
                    try
                    {
                        MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(int.Parse(this.Guilds_Data[0]));
                        if (guild == null)
                        {
                            Message.AppendInt32(0);
                        }
                        else
                        {
                            Message.AppendInt32(2);
                            Message.AppendInt32(5);
                            Message.AppendString("0");
                            Message.AppendString(guild.Id.ToString());
                            Message.AppendString(guild.Image);
                            Message.AppendString(this.Guilds_Data[1]);
                            Message.AppendString(this.Guilds_Data[2]);
                        }
                    }
                    catch
                    {
                        Message.AppendInt32(0);
                    }

                }
                else
                {
                    Message.AppendInt32(0);
                }
                if (this.mBaseItem.InteractionType == InteractionType.musicdisc)
                {
                    Message.AppendString("");
                }
                else if (!this.mBaseItem.AllowGroupItem)
                {
                    Message.AppendString(this.ExtraData);
                }
                if (this.RareId > 0)
                {
                    Message.AppendInt32(this.RareId);
                    Message.AppendInt32(this.GetRareCatalogItem(this.BaseItem).LimitedStack);
                }
                Message.AppendBoolean(this.GetBaseItem().AllowRecycle);
                Message.AppendBoolean(this.GetBaseItem().AllowTrade);
                if (this.RareId > 0)
                {
                    Message.AppendBoolean(false);
                }
                else
                {
                    Message.AppendBoolean(this.GetBaseItem().AllowInventoryStack);
                }
                Message.AppendBoolean(Marketplace.CanSellItem(this));
                Message.AppendInt32(-1);
                Message.AppendBoolean(true);
                Message.AppendInt32(-1);
                if (this.mBaseItem.Type.ToString().ToUpper() == "S")
                {
                    Message.AppendString("");
                    Message.AppendInt32(-1);
                }
            }
        }


        internal void SerializeWall(ServerMessage Message, bool Inventory)
        {

            Message.AppendInt32(this.Id);
            Message.AppendString(this.mBaseItem.Type.ToString().ToUpper());
            Message.AppendInt32(this.Id);
            Message.AppendInt32(this.GetBaseItem().SpriteId);
            if (this.GetBaseItem().Name.Contains("a2"))
            {
                Message.AppendInt32(3);
            }
            else if (this.GetBaseItem().Name.Contains("wallpaper"))
            {
                Message.AppendInt32(2);
            }
            else if (this.GetBaseItem().Name.Contains("landscape"))
            {
                Message.AppendInt32(4);
            }
            else
            {
                Message.AppendInt32(1);
            }
            int i = 0;
            Message.AppendInt32(i);
            Message.AppendString(this.ExtraData);
            Message.AppendBoolean(this.GetBaseItem().AllowRecycle);
            Message.AppendBoolean(this.GetBaseItem().AllowTrade);
            Message.AppendBoolean(this.GetBaseItem().AllowInventoryStack);
            Message.AppendBoolean(Marketplace.CanSellItem(this));
            Message.AppendInt32(-1);
            Message.AppendBoolean(true);
            Message.AppendInt32(-1);
        }
    }
}

