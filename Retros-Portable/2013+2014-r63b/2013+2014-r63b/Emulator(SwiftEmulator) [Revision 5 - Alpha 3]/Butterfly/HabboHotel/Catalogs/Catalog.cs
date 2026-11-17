namespace Butterfly.HabboHotel.Catalogs
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Guilds;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Pets;
    using Butterfly.HabboHotel.Users.Inventory;
    using Butterfly.Messages;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using Butterfly.HabboHotel.RoomBots;
    internal class Catalog
    {
        internal List<EcotronReward> EcotronRewards;
        private Hashtable gifts;
        private Butterfly.HabboHotel.Catalogs.Marketplace Marketplace = new Butterfly.HabboHotel.Catalogs.Marketplace();
        private ServerMessage[] mCataIndexCache;
        public ServerMessage mGuildPageBuy;
        internal Dictionary<int, CatalogPage> Pages;
        internal Dictionary<uint, CatalogItem> RaresItems;

        internal Catalog()
        {
        }

        internal static bool CheckPetName(string PetName)
        {
            if ((PetName.Length < 1) || (PetName.Length > 0x10))
            {
                return false;
            }
            if (!ButterflyEnvironment.IsValidAlphaNumeric(PetName))
            {
                return false;
            }
            return true;
        }
        internal static RoomBot GenerateBotFromRow(DataRow Row)
        {
            if (Row == null)
            {
                return null;
            }
            List<RandomSpeech> RandomSpeech = new List<RandomSpeech>();
            List<BotResponse> BotResponse = new List<BotResponse>();
            return new RoomBot(Convert.ToUInt32(Row["id"]), Convert.ToUInt32(Row["room_id"]), AIType.Generic, "freeroam", (string)Row["name"], (string)Row["motto"], (string)Row["look"], int.Parse(Row["x"].ToString()), int.Parse(Row["y"].ToString()), int.Parse(Row["z"].ToString()), 0, 0, 0, 0, 0, ref RandomSpeech, ref BotResponse, int.Parse(Row["user_id"].ToString()));
        }
        internal static Pet CreatePet(uint UserId, string Name, int Type, string Race, string Color)
        {
            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            Pet pet = new Pet(0x194, UserId, 0, Name, (uint) Type, Race, Color, 0, 100, 100, 0, (double) ButterflyEnvironment.GetUnixTimestamp(), 0, 0, 0.0, 0, 0, -1, false) {
                DBState = DatabaseUpdateState.NeedsUpdate
            };
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MSSQL)
                {
                    adapter.setQuery(string.Concat(new object[] { "INSERT INTO user_pets (user_id,name,type,race,color,expirience,energy,createstamp,nutrition,respect,x,y,z) OUTPUT INSERTED.* VALUES (", pet.OwnerId, ",@name,", pet.Type, ",@race,@color,0,100,'", pet.CreationStamp, "',0,0,0,0,0)" }));
                }
                else
                {
                    adapter.setQuery(string.Concat(new object[] { "INSERT INTO user_pets (user_id,name,type,race,color,expirience,energy,createstamp) VALUES (", pet.OwnerId, ",@name,", pet.Type, ",@race,@color,0,100,'", pet.CreationStamp, "')" }));
                }
                adapter.addParameter("name", pet.Name);
                adapter.addParameter("race", pet.Race);
                adapter.addParameter("color", pet.Color);
                pet.PetId = (uint) adapter.insertQuery();
            }
            clientByUserID.GetHabbo().GetInventoryComponent().AddPet(pet);
            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(clientByUserID, "ACH_PetLover", 1);
            clientByUserID.SendMessage(clientByUserID.GetHabbo().GetInventoryComponent().SerializePetInventory());
            
            return pet;
        }
        internal static RoomBot CreateBot(uint UserId, string Name, string Look, string Motto, int TemplateId)
        {
            List<RandomSpeech> RandomSpeech = new List<RandomSpeech>();
            List<BotResponse> BotResponse = new List<BotResponse>();
            RoomBot bot = new RoomBot((uint)new Random().Next(), 0, AIType.Generic, "freeroam", Name, Motto, Look, 0, 0, 0, 0, 0, 0, 0, 0, ref RandomSpeech, ref BotResponse, (int)UserId);


            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {

                dbClient.runFastQuery("INSERT INTO user_bots (user_id,name,look,motto,template_id) VALUES ('" + UserId + "', '" + Name + "', '" + Look + "', '" + Motto + "', '" + TemplateId + "')");
                bot.BotId = (uint)dbClient.insertQuery();
            }
            return bot;
        }
        internal List<UserItem> DeliverItems(GameClient Session, Item Item, int Amount, string ExtraData, uint Rareid, uint songID = 0, uint giftId =0)
        {
            int num;
            List<UserItem> list = new List<UserItem>();
            switch (Item.Type.ToString())
            {
                case "r":
                    {
                        RoomBot GeneratedBot = null;

                        GeneratedBot = CreateBot(Session.GetHabbo().Id, "Jon", "hr-3020-34.hd-3091-2.ch-225-92.lg-3058-100.sh-3089-1338.ca-3084-78-108.wa-2005", "", 0);
                        Session.GetHabbo().GetInventoryComponent().AddBot(GeneratedBot);
                        Session.SendMessage(Session.GetHabbo().GetInventoryComponent().SerializeBotInventory());
                        return list;
                    }
                case "i":
                case "s":
                    for (num = 0; num < Amount; num++)
                    {
                        IQueryAdapter adapter;
                        switch (Item.InteractionType)
                        {
                            case InteractionType.dimmer:
                            {
                                UserItem item3 = Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, ExtraData, true, false, 0, "",Session.GetHabbo().Id, 0);
                                uint id = item3.Id;
                                list.Add(item3);
                                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    adapter.runFastQuery("INSERT INTO items_moodlight (item_id,enabled,current_preset,preset_one,preset_two,preset_three) VALUES (" + id + ",0,1,'#000000,255,0','#000000,255,0','#000000,255,0')");
                                }
                                continue;
                            }
                            case InteractionType.teleport:
                            {
                                UserItem item = Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, "0", true, false, 0, "",Session.GetHabbo().Id, 0);
                                uint num2 = item.Id;
                                UserItem item2 = Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, "0", true, false, 0, "",Session.GetHabbo().Id, 0);
                                uint num3 = item2.Id;
                                list.Add(item);
                                list.Add(item2);
                                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO items_tele_links (tele_one_id,tele_two_id) VALUES (", num2, ",", num3, ")" }));
                                    adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO items_tele_links (tele_one_id,tele_two_id) VALUES (", num3, ",", num2, ")" }));
                                }
                                continue;
                            }
                            case InteractionType.pet0:
                            {
                                string[] strArray = ExtraData.Split(new char[] { '\n' });
                                Pet pet = CreatePet(Session.GetHabbo().Id, strArray[0], 0, strArray[1], strArray[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                
                                continue;
                            }
                            case InteractionType.pet1:
                            {
                                string[] strArray2 = ExtraData.Split(new char[] { '\n' });
                                Pet pet2 = CreatePet(Session.GetHabbo().Id, strArray2[0], 1, strArray2[1], strArray2[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet2);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet2:
                            {
                                string[] strArray3 = ExtraData.Split(new char[] { '\n' });
                                Pet pet3 = CreatePet(Session.GetHabbo().Id, strArray3[0], 2, strArray3[1], strArray3[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet3);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet3:
                            {
                                string[] strArray4 = ExtraData.Split(new char[] { '\n' });
                                Pet pet4 = CreatePet(Session.GetHabbo().Id, strArray4[0], 3, strArray4[1], strArray4[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet4);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet4:
                            {
                                string[] strArray5 = ExtraData.Split(new char[] { '\n' });
                                Pet pet5 = CreatePet(Session.GetHabbo().Id, strArray5[0], 4, strArray5[1], strArray5[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet5);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet5:
                            {
                                string[] strArray6 = ExtraData.Split(new char[] { '\n' });
                                Pet pet6 = CreatePet(Session.GetHabbo().Id, strArray6[0], 5, strArray6[1], strArray6[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet6);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet6:
                            {
                                string[] strArray7 = ExtraData.Split(new char[] { '\n' });
                                Pet pet7 = CreatePet(Session.GetHabbo().Id, strArray7[0], 6, strArray7[1], strArray7[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet7);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet7:
                            {
                                string[] strArray8 = ExtraData.Split(new char[] { '\n' });
                                Pet pet8 = CreatePet(Session.GetHabbo().Id, strArray8[0], 7, strArray8[1], strArray8[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet8);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet8:
                            {
                                string[] strArray9 = ExtraData.Split(new char[] { '\n' });
                                Pet pet9 = CreatePet(Session.GetHabbo().Id, strArray9[0], 8, strArray9[1], strArray9[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet9);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet9:
                            {
                                string[] strArray10 = ExtraData.Split(new char[] { '\n' });
                                Pet pet10 = CreatePet(Session.GetHabbo().Id, strArray10[0], 9, strArray10[1], strArray10[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet10);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet10:
                            {
                                string[] strArray11 = ExtraData.Split(new char[] { '\n' });
                                Pet pet11 = CreatePet(Session.GetHabbo().Id, strArray11[0], 10, strArray11[1], strArray11[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet11);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet11:
                            {
                                string[] strArray12 = ExtraData.Split(new char[] { '\n' });
                                Pet pet12 = CreatePet(Session.GetHabbo().Id, strArray12[0], 11, strArray12[1], strArray12[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet12);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet12:
                            {
                                string[] strArray13 = ExtraData.Split(new char[] { '\n' });
                                Pet pet13 = CreatePet(Session.GetHabbo().Id, strArray13[0], 12, strArray13[1], strArray13[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet13);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet13:
                            {
                                string[] strArray14 = ExtraData.Split(new char[] { '\n' });
                                Pet pet14 = CreatePet(Session.GetHabbo().Id, strArray14[0], 13, strArray14[1], strArray14[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet14);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet14:
                            {
                                string[] strArray15 = ExtraData.Split(new char[] { '\n' });
                                Pet pet15 = CreatePet(Session.GetHabbo().Id, strArray15[0], 14, strArray15[1], strArray15[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet15);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet15:
                            {
                                string[] strArray16 = ExtraData.Split(new char[] { '\n' });
                                Pet pet16 = CreatePet(Session.GetHabbo().Id, strArray16[0], 15, strArray16[1], strArray16[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet16);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet16:
                            {
                                string[] strArray17 = ExtraData.Split(new char[] { '\n' });
                                Pet pet17 = CreatePet(Session.GetHabbo().Id, strArray17[0], 0x10, strArray17[1], strArray17[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet17);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.pet17:
                            {
                                string[] strArray18 = ExtraData.Split(new char[] { '\n' });
                                Pet pet18 = CreatePet(Session.GetHabbo().Id, strArray18[0], 0x11, strArray18[1], strArray18[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(pet18);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0, "",Session.GetHabbo().Id, 0));
                                continue;
                            }
                            case InteractionType.musicdisc:
                            {
                                ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_MusicCollector", 1);
                                list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, songID.ToString(), true, false, Rareid, "",Session.GetHabbo().Id, songID));
                                continue;
                            }
                        }
                        if (Item.AllowGroupItem)
                        {
                            MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(int.Parse(ExtraData));
                            string groupString = string.Concat(new object[] { guild.Id, ";", guild.HtmlColor1, ";", guild.HtmlColor2 });
                            list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, ExtraData, true, false, Rareid, groupString,Session.GetHabbo().Id, songID));
                        }
                        else
                        {
                            list.Add(Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, ExtraData, true, false, Rareid, "",Session.GetHabbo().Id, songID));
                        }
                    }
                    return list;

                case "e":
                    for (num = 0; num < Amount; num++)
                    {
                        Session.GetHabbo().GetAvatarEffectsInventoryComponent().AddEffect(Item.SpriteId, 0xe10);
                    }
                    return list;
            }
            Session.SendNotif(LanguageLocale.GetValue("catalog.buyerror"));
            return list;
        }

        internal CatalogItem FindItem(uint ItemId)
        {
            foreach (CatalogPage page in this.Pages.Values)
            {
                if (page.Items.ContainsKey(ItemId))
                {
                    return (CatalogItem) page.Items[ItemId];
                }
            }
            return null;
        }

        internal static Pet GeneratePetFromRow(DataRow Row)
        {
            if (Row == null)
            {
                return null;
            }
            return new Pet(Convert.ToUInt32(Row["id"]), Convert.ToUInt32(Row["user_id"]), Convert.ToUInt32(Row["room_id"]), (string) Row["name"], Convert.ToUInt32(Row["type"]), (string) Row["race"], (string) Row["color"], (int) Row["expirience"], (int) Row["energy"], (int) Row["nutrition"], (int) Row["respect"], (double) Row["createstamp"], (int) Row["x"], (int) Row["y"], (double) Row["z"], (int) Row["have_saddle"], (int) Row["hairdye"], (int) Row["pethair"], ((int) Row["can_all"]) == 1);
        }

        internal Item GeneratePresent()
        {
            int count = this.gifts.Count;
            int randomNumber = ButterflyEnvironment.GetRandomNumber(0, count);
            int num3 = 0;
            if (count != 0)
            {
                foreach (Item item in this.gifts.Values)
                {
                    if (item != null)
                    {
                        if (num3 == randomNumber)
                        {
                            return item;
                        }
                        num3++;
                    }
                }
            }
            return null;
        }

        internal List<EcotronReward> GetEcotronRewardsForLevel(uint Level)
        {
            List<EcotronReward> list = new List<EcotronReward>();
            foreach (EcotronReward reward in this.EcotronRewards)
            {
                if (reward.RewardLevel == Level)
                {
                    list.Add(reward);
                }
            }
            return list;
        }

        public static ServerMessage GetGuildPage()
        {
            ServerMessage message = new ServerMessage(Outgoing.SendGuildElements);
            message.AppendInt32(GuildsPartsData.BaseBadges.Count);
            foreach (GuildsPartsData data in GuildsPartsData.BaseBadges)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.ExtraData1);
                message.AppendString(data.ExtraData2);
            }
            message.AppendInt32(GuildsPartsData.SymbolBadges.Count);
            foreach (GuildsPartsData data in GuildsPartsData.SymbolBadges)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.ExtraData1);
                message.AppendString(data.ExtraData2);
            }
            message.AppendInt32(GuildsPartsData.ColorBadges1.Count);
            foreach (GuildsPartsData data in GuildsPartsData.ColorBadges1)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.ExtraData1);
            }
            message.AppendInt32(GuildsPartsData.ColorBadges2.Count);
            foreach (GuildsPartsData data in GuildsPartsData.ColorBadges2)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.ExtraData1);
            }
            message.AppendInt32(GuildsPartsData.ColorBadges3.Count);
            foreach (GuildsPartsData data in GuildsPartsData.ColorBadges3)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.ExtraData1);
            }
            return message;
        }

        internal ServerMessage GetIndexMessageForRank(uint Rank)
        {
            if (Rank < 1)
            {
                Rank = 1;
            }
            if (Rank > 7)
            {
                Rank = 7;
            }
            return this.mCataIndexCache[Rank];
        }

        internal Butterfly.HabboHotel.Catalogs.Marketplace GetMarketplace()
        {
            return this.Marketplace;
        }

        internal CatalogPage GetPage(int Page)
        {
            if (!this.Pages.ContainsKey(Page))
            {
                return null;
            }
            return this.Pages[Page];
        }

        internal EcotronReward GetRandomEcotronReward()
        {
            uint level = 1;
            if (ButterflyEnvironment.GetRandomNumber(1, 0x7d0) == 0x7d0)
            {
                level = 5;
            }
            else if (ButterflyEnvironment.GetRandomNumber(1, 200) == 200)
            {
                level = 4;
            }
            else if (ButterflyEnvironment.GetRandomNumber(1, 40) == 40)
            {
                level = 3;
            }
            else if (ButterflyEnvironment.GetRandomNumber(1, 4) == 4)
            {
                level = 2;
            }
            List<EcotronReward> ecotronRewardsForLevel = this.GetEcotronRewardsForLevel(level);
            if ((ecotronRewardsForLevel != null) && (ecotronRewardsForLevel.Count >= 1))
            {
                return ecotronRewardsForLevel[ButterflyEnvironment.GetRandomNumber(0, ecotronRewardsForLevel.Count - 1)];
            }
            return new EcotronReward(0, 0x5c7, 0);
        }

        internal int GetTreeSize(int rank, int TreeId)
        {
            int num = 0;
            foreach (CatalogPage page in this.Pages.Values)
            {
                if ((page.MinRank <= rank) && (page.ParentId == TreeId))
                {
                    num++;
                }
            }
            return num;
        }

        internal void BuyGift(GameClient Session, int pageId, uint itemId, string extraData, string giftReceiverName, string giftMessage, int giftSprite, int giftRibbon, int giftColor, bool isAnonymous)
        {
            IQueryAdapter adapter;
            CatalogPage page;
            if ((this.Pages.TryGetValue(pageId, out page) && ((((page != null) && page.Enabled) && (page.Visible && (Session != null))) && (Session.GetHabbo() != null))) && ((!page.ClubOnly || Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club")) && (page.MinRank <= Session.GetHabbo().Rank)))
            {
                CatalogItem item = page.GetItem(itemId);
                if (item != null)
                {
                    int showSender = 1;
                    if (isAnonymous == false)
                        showSender = 0;

                    if (item.GetBaseItem(itemId).Type == 'e')
                    {
                        Session.SendNotif(LanguageLocale.GetValue("catalog.gift.send.error"));
                        return;
                    }
                    DataRow row;
                    uint userID = 0;
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery("SELECT id FROM users WHERE username = @gift_user");
                        adapter.addParameter("gift_user", giftReceiverName);
                        row = adapter.getRow();
                    }
                    if (row == null)
                    {
                        Session.GetMessageHandler().GetResponse().Init(Outgoing.GiftError);
                        Session.GetMessageHandler().GetResponse().AppendString(giftReceiverName);
                        Session.GetMessageHandler().SendResponse();
                        return;
                    }
                    userID = Convert.ToUInt32(row[0]);
                    if (userID == 0)
                    {
                        Session.GetMessageHandler().GetResponse().Init(Outgoing.GiftError);
                        Session.GetMessageHandler().GetResponse().AppendString(giftReceiverName);
                        Session.GetMessageHandler().SendResponse();
                        return;
                    }
                    if (!item.GetBaseItem(itemId).AllowGift)
                    {
                        return;
                    }

                    if (item != null)
                    {
                        if (item.Badge != "")
                        {
                            ButterflyEnvironment.getHabboForName(giftReceiverName).GetBadgeComponent().GiveBadge(item.Badge, true);
                        }
                    }
                    int limitedSelled = 0;
                    if (item.IsLimited)
                    {
                        if (item.LimitedStack <= item.LimitedSelled)
                        {
                            return;
                        }
                        item.LimitedSelled++;
                        limitedSelled = item.LimitedSelled;
                        using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        {
                            adapter.runFastQuery(string.Concat(new object[] { "UPDATE catalog_items_copy SET limited_sells = ", item.LimitedSelled, " WHERE id = ", item.Id }));
                        }
                        page.InitMsg();
                    }

                    bool flag = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    if (Session.GetHabbo().Credits < item.CreditsCost)
                    {
                        flag = true;
                    }
                    if (Session.GetHabbo().ActivityPoints < item.PixelsCost)
                    {
                        flag2 = true;
                    }
                    if (Session.GetHabbo().Crystals < item.CrystalCost)
                    {
                        flag3 = true;
                    }
                    if (!flag && !flag2 && !flag3)
                    {
                        Habbo habbo = Session.GetHabbo();
                        if (!(item.CreditsCost <= 0))
                        {
                            habbo.Credits -= item.CreditsCost;
                            Session.GetHabbo().UpdateCreditsBalance();
                        }
                        if (!(item.PixelsCost <= 0))
                        {
                            habbo.ActivityPoints -= item.PixelsCost;
                            Session.GetHabbo().UpdateActivityPointsBalance(true);
                        }
                        if (!(item.CrystalCost <= 0))
                        {
                            habbo.Crystals -= item.CrystalCost;
                            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET crystals = ", habbo.Crystals, " WHERE id = ", habbo.Id }));
                            }
                            Session.GetHabbo().UpdateActivityPointsBalance(true);
                        }
                    }

                    if (!item.GetBaseItem(itemId).AllowGroupItem)
                    {
                        Exception exception;
                        switch (item.GetBaseItem(itemId).InteractionType)
                        {
                            case InteractionType.none:
                                extraData = "";
                                goto Label_007;

                            case InteractionType.postit:
                                extraData = "FFFF33";
                                goto Label_007;

                            case InteractionType.roomeffect:
                                {
                                    double num9 = 0.0;
                                    try
                                    {
                                        if (string.IsNullOrEmpty(extraData))
                                        {
                                            num9 = 0.0;
                                        }
                                        else
                                        {
                                            num9 = double.Parse(extraData, ButterflyEnvironment.cultureInfo);
                                        }
                                    }
                                    catch (Exception exception2)
                                    {
                                        exception = exception2;
                                        Logging.HandleException(exception, "Catalog.HandlePurchase: " + extraData);
                                    }
                                    extraData = num9.ToString().Replace(',', '.');
                                    goto Label_007;
                                }
                            case InteractionType.dimmer:
                                extraData = "1,1,1,#000000,255";
                                goto Label_007;

                            case InteractionType.trophy:
                                extraData = string.Concat(new object[] { Session.GetHabbo().Username, Convert.ToChar(9), DateTime.Now.Day, "-", DateTime.Now.Month, "-", DateTime.Now.Year, Convert.ToChar(9), ButterflyEnvironment.FilterInjectionChars(extraData, true) });
                                goto Label_007;



                            case InteractionType.pet0:
                            case InteractionType.pet1:
                            case InteractionType.pet2:
                            case InteractionType.pet3:
                            case InteractionType.pet4:
                            case InteractionType.pet5:
                            case InteractionType.pet6:
                            case InteractionType.pet7:
                            case InteractionType.pet8:
                            case InteractionType.pet9:
                            case InteractionType.pet10:
                            case InteractionType.pet11:
                            case InteractionType.pet12:
                            case InteractionType.pet13:
                            case InteractionType.pet14:
                            case InteractionType.pet15:
                            case InteractionType.pet16:
                            case InteractionType.pet17:
                            case InteractionType.pet18:
                            case InteractionType.pet19:
                            case InteractionType.pet20:
                            case InteractionType.pet21:
                            case InteractionType.pet22:
                                try
                                {
                                    string[] strArray = extraData.Split(new char[] { '\n' });
                                    string petName = strArray[0];
                                    string str2 = strArray[1];
                                    string str3 = strArray[2];
                                    if (!CheckPetName(petName) || (str3.Length != 6))
                                    {
                                        return;
                                    }
                                }
                                catch (Exception exception1)
                                {
                                    exception = exception1;
                                    Logging.WriteLine(exception.ToString());
                                    Logging.HandleException(exception, "Catalog.HandlePurchase");
                                    return;
                                }
                                goto Label_007;

                            case InteractionType.musicdisc:
                                extraData = item.songID.ToString();
                                goto Label_007;

                            case InteractionType.rentals:
                                extraData = item.ExtraData;
                                goto Label_007;
                        }
                        extraData = "";

                        Console.WriteLine("Passed switch");
                    Label_007:
                        Console.WriteLine("Going to Serialize...");
                        Session.GetMessageHandler().GetResponse().Init(Outgoing.UpdateInventary);
                        Session.GetMessageHandler().SendResponse();
                        Session.GetMessageHandler().GetResponse().Init(Outgoing.SerializePurchaseInformation);
                        Session.GetMessageHandler().GetResponse().AppendInt32(item.GetBaseItem(itemId).ItemId);
                        Session.GetMessageHandler().GetResponse().AppendString(item.GetBaseItem(itemId).Name);
                        Session.GetMessageHandler().GetResponse().AppendBoolean(false);
                        Session.GetMessageHandler().GetResponse().AppendInt32(item.CreditsCost);
                        Session.GetMessageHandler().GetResponse().AppendInt32(item.PixelsCost);
                        Session.GetMessageHandler().GetResponse().AppendInt32(item.CrystalCost);
                        Session.GetMessageHandler().GetResponse().AppendBoolean(true);
                        Session.GetMessageHandler().GetResponse().AppendInt32(1);
                        Session.GetMessageHandler().GetResponse().AppendString(item.GetBaseItem(itemId).Type.ToString().ToLower());
                        Session.GetMessageHandler().GetResponse().AppendInt32(item.GetBaseItem(itemId).SpriteId);
                        Session.GetMessageHandler().GetResponse().AppendString("");
                        Session.GetMessageHandler().GetResponse().AppendInt32(1);
                        Session.GetMessageHandler().GetResponse().AppendInt32(0);
                        Session.GetMessageHandler().GetResponse().AppendBoolean(false);
                        Session.GetMessageHandler().GetResponse().AppendBoolean(true);
                        Session.GetMessageHandler().GetResponse().AppendInt32(1);
                        Session.GetMessageHandler().SendResponse();

                        GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(userID);
                        ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_GiftGiver", 1);
                        ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(clientByUserID, "ACH_GiftReceiver", 1);

                        
                        try
                        {
                            Item giftCase = ButterflyEnvironment.GetGame().GetItemManager().GetItemSprite(giftSprite);
                            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.setQuery("INSERT INTO user_gifts (page_id,item_id,extradata,target_name,message,ribbon,color,gift_sprite,show_sender,rare_id,inventory_id) VALUES " +
                                    "(" + pageId + "," + itemId + ",@extradata, @name, @message," + giftRibbon + "," + giftColor + "," + giftSprite + "," + showSender + "," + limitedSelled + ",@inventory_id)");
                                adapter.addParameter("name", giftReceiverName);
                                adapter.addParameter("message", giftMessage);
                                adapter.addParameter("extradata", extraData);
                                //adapter.addParameter("inventory_id", item3.Id);
                                adapter.runQuery();
                            }
                            if (clientByUserID != null)
                            {
                                clientByUserID.SendNotif(LanguageLocale.GetValue("catalog.gift.received") + Session.GetHabbo().Username);
                                clientByUserID.GetHabbo().GetInventoryComponent().SendFloorInventoryUpdate();
                                clientByUserID.GetMessageHandler().GetResponse().Init(Outgoing.SendPurchaseAlert);
                                clientByUserID.GetMessageHandler().GetResponse().AppendInt32(1);
                                clientByUserID.GetMessageHandler().GetResponse().AppendInt32(1);
                                clientByUserID.GetMessageHandler().GetResponse().AppendInt32(1);
                                //clientByUserID.GetMessageHandler().GetResponse().AppendInt32(item3.Id);
                                clientByUserID.GetMessageHandler().SendResponse();
                                InventoryComponent inventoryComponent = clientByUserID.GetHabbo().GetInventoryComponent();
                                if (inventoryComponent != null)
                                {
                                    inventoryComponent.RunDBUpdate();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        Session.SendNotif(LanguageLocale.GetValue("catalog.gift.sent"));
                    }
                }
            }
        }

        internal void HandlePurchase(GameClient Session, int PageId, uint ItemId, string ExtraData, int priceAmount, bool IsGift, string giftReceiverName, string giftMessage, int giftSprite, int giftRibbon, int giftColor, bool isAnonymous)
        {
            CatalogPage page;
            int num = priceAmount;
            if (priceAmount >= 6)
            {
                num--;
            }
            if (priceAmount >= 12)
            {
                num -= 2;
            }
            if (priceAmount >= 0x12)
            {
                num -= 2;
            }
            if (priceAmount >= 0x18)
            {
                num -= 2;
            }
            if (priceAmount >= 30)
            {
                num -= 2;
            }
            if (priceAmount >= 0x24)
            {
                num -= 2;
            }
            if (priceAmount >= 0x2a)
            {
                num -= 2;
            }
            if (priceAmount >= 0x30)
            {
                num -= 2;
            }
            if (priceAmount >= 0x36)
            {
                num -= 2;
            }
            if (priceAmount >= 60)
            {
                num -= 2;
            }
            if (priceAmount >= 0x42)
            {
                num -= 2;
            }
            if (priceAmount >= 0x48)
            {
                num -= 2;
            }
            if (priceAmount >= 0x4e)
            {
                num -= 2;
            }
            if (priceAmount >= 0x54)
            {
                num -= 2;
            }
            if (priceAmount >= 90)
            {
                num -= 2;
            }
            if (priceAmount >= 0x60)
            {
                num -= 2;
            }
            if (priceAmount >= 0x63)
            {
                num -= 2;
            }
            if ((this.Pages.TryGetValue(PageId, out page) && ((((page != null) && page.Enabled) && (page.Visible && (Session != null))) && (Session.GetHabbo() != null))) && ((!page.ClubOnly || Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club")) && (page.MinRank <= Session.GetHabbo().Rank)))
            {
                CatalogItem item = page.GetItem(ItemId);
                if (item != null)
                {
                    if (item.Badge != "")
                    {
                        Session.GetHabbo().GetBadgeComponent().GiveBadge(item.Badge, true);
                    }
                    if (item.Name.Contains("HABBO_CLUB_VIP") || item.Name.Equals("deal_vip_1_year_and_badge"))
                    {
                        if (item.CreditsCost <= Session.GetHabbo().Credits)
                        {
                            int num2 = 0;
                            int num3 = 0;
                            if (item.Name.Contains("HABBO_CLUB_VIP_"))
                            {
                                if (item.Name.Contains("_DAY"))
                                {
                                    num3 = int.Parse(item.Name.Split(new char[] { '_' })[3]);
                                }
                                else if (item.Name.Contains("_MONTH"))
                                {
                                    num2 = int.Parse(item.Name.Split(new char[] { '_' })[3]);
                                    num3 = 0x1f * num2;
                                }
                            }
                            else if (item.Name.Equals("deal_vip_1_year_and_badge"))
                            {
                                num2 = 12;
                                num3 = 0x1f * num2;
                            }
                            else if (item.Name.Equals("HABBO_CLUB_VIP_5_YEAR"))
                            {
                                num2 = 60;
                                num3 = 0x1f * num2;
                            }
                            if (item.CreditsCost > 0)
                            {
                                Habbo habbo = Session.GetHabbo();
                                habbo.Credits -= item.CreditsCost;
                                Session.GetHabbo().UpdateCreditsBalance();
                            }
                            Session.GetHabbo().GetSubscriptionManager().AddOrExtendSubscription("club_habbo", (num3 * 0x18) * 0xe10, Session);
                            Session.GetHabbo().SerializeClub();
                        }
                    }
                    else
                    {
                        IQueryAdapter adapter;
                        int limitedSelled = 0;
                        if (item.IsLimited)
                        {
                            num = 1;
                            priceAmount = 1;
                            if (item.LimitedStack <= item.LimitedSelled)
                            {
                                return;
                            }
                            item.LimitedSelled++;
                            limitedSelled = item.LimitedSelled;
                            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.runFastQuery(string.Concat(new object[] { "UPDATE catalog_items_copy SET limited_sells = ", item.LimitedSelled, " WHERE id = ", item.Id }));
                            }
                            page.InitMsg();
                        }
                        if (IsGift && (priceAmount > 1))
                        {
                            num = 1;
                            priceAmount = 1;
                            Session.SendNotif("You can buy only buy one gift at once.");
                        }
                        uint userID = 0;
                        bool flag = false;
                        bool flag2 = false,flag3 = false;
                        if (Session.GetHabbo().Credits < (item.CreditsCost * num))
                        {
                            flag = true;
                        }
                        if (Session.GetHabbo().ActivityPoints < (item.PixelsCost * num))
                        {
                            flag2 = true;
                        }
                        if (Session.GetHabbo().Crystals < item.CrystalCost)
                        {
                            flag3 = true;
                        }
                        if (!flag && !flag2 && !flag3)
                        {
                            Habbo habbo2 = Session.GetHabbo();
                            if ((item.OudeCredits * num) > 0)
                            {
                                int num7 = 0;
                                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    adapter.setQuery("SELECT belcredits FROM users WHERE id = " + Session.GetHabbo().Id);
                                    num7 = adapter.getInteger();
                                }
                                if (item.OudeCredits > num7)
                                {
                                    Session.SendNotif(LanguageLocale.GetValue("catalog.oudebelcreditserror") + item.OudeCredits);
                                    return;
                                }
                                num7 -= item.OudeCredits * num;
                                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET belcredits = ", num7, " WHERE id = ", Session.GetHabbo().Id }));
                                }
                                Session.SendNotif(LanguageLocale.GetValue("catalog.oudebelcreditsok") + num7);
                            }
                            if (!((item.CreditsCost <= 0) || IsGift))
                            {
                                habbo2.Credits -= item.CreditsCost * num;
                                Session.GetHabbo().UpdateCreditsBalance();
                            }
                            if (!((item.PixelsCost <= 0) || IsGift))
                            {
                                habbo2.ActivityPoints -= item.PixelsCost * num;
                                Session.GetHabbo().UpdateActivityPointsBalance(true);
                            }
                            if (!((item.CrystalCost <= 0) || IsGift))
                            {
                                habbo2.Crystals -= item.CrystalCost * num;
                                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET crystals = ", habbo2.Crystals, " WHERE id = ", Session.GetHabbo().Id }));
                                }
                                Session.GetHabbo().UpdateActivityPointsBalance(true);
                            }
                            foreach (uint num8 in item.Items)
                            {
                                if (IsGift)
                                {
                                    DataRow row;
                                    if (!item.GetBaseItem(num8).AllowGift)
                                    {
                                        break;
                                    }
                                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                    {
                                        adapter.setQuery("SELECT id FROM users WHERE username = @gift_user");
                                        adapter.addParameter("gift_user", giftReceiverName);
                                        row = adapter.getRow();
                                    }
                                    if (row == null)
                                    {
                                        Session.GetMessageHandler().GetResponse().Init(Outgoing.GiftError);
                                        Session.GetMessageHandler().GetResponse().AppendString(giftReceiverName);
                                        Session.GetMessageHandler().SendResponse();
                                        break;
                                    }
                                    userID = Convert.ToUInt32(row[0]);
                                    if (userID == 0)
                                    {
                                        Session.GetMessageHandler().GetResponse().Init(Outgoing.GiftError);
                                        Session.GetMessageHandler().GetResponse().AppendString(giftReceiverName);
                                        Session.GetMessageHandler().SendResponse();
                                        break;
                                    }
                                    if ((item.CreditsCost > 0) && IsGift)
                                    {
                                        Habbo habbo4 = Session.GetHabbo();
                                        habbo4.Credits -= item.CreditsCost * num;
                                        Session.GetHabbo().UpdateCreditsBalance();
                                    }
                                    if ((item.PixelsCost > 0) && IsGift)
                                    {
                                        Habbo habbo5 = Session.GetHabbo();
                                        habbo5.ActivityPoints -= item.PixelsCost * num;
                                        Session.GetHabbo().UpdateActivityPointsBalance(true);
                                    }
                                    if ((item.CrystalCost > 0) && IsGift)
                                    {
                                        Habbo habbo6 = Session.GetHabbo();
                                        habbo6.Crystals -= item.CrystalCost * num;
                                        Session.GetHabbo().UpdateActivityPointsBalance(true);
                                    }

                                }
                                if (IsGift && (item.GetBaseItem(num8).Type == 'e'))
                                {
                                    Session.SendNotif(LanguageLocale.GetValue("catalog.gift.send.error"));
                                    break;
                                }
                                if (!item.GetBaseItem(num8).AllowGroupItem)
                                {
                                    Exception exception;
                                    switch (item.GetBaseItem(num8).InteractionType)
                                    {
                                        case InteractionType.none:
                                            ExtraData = "";
                                            goto Label_0C75;

                                        case InteractionType.postit:
                                            ExtraData = "FFFF33";
                                            goto Label_0C75;

                                        case InteractionType.roomeffect:
                                            {
                                                double num9 = 0.0;
                                                try
                                                {
                                                    if (string.IsNullOrEmpty(ExtraData))
                                                    {
                                                        num9 = 0.0;
                                                    }
                                                    else
                                                    {
                                                        num9 = double.Parse(ExtraData, ButterflyEnvironment.cultureInfo);
                                                    }
                                                }
                                                catch (Exception exception2)
                                                {
                                                    exception = exception2;
                                                    Logging.HandleException(exception, "Catalog.HandlePurchase: " + ExtraData);
                                                }
                                                ExtraData = num9.ToString().Replace(',', '.');
                                                goto Label_0C75;
                                            }
                                        case InteractionType.dimmer:
                                            ExtraData = "1,1,1,#000000,255";
                                            goto Label_0C75;

                                        case InteractionType.trophy:
                                            ExtraData = string.Concat(new object[] { Session.GetHabbo().Username, Convert.ToChar(9), DateTime.Now.Day, "-", DateTime.Now.Month, "-", DateTime.Now.Year, Convert.ToChar(9), ButterflyEnvironment.FilterInjectionChars(ExtraData, true) });
                                            goto Label_0C75;



                                        case InteractionType.pet0:
                                        case InteractionType.pet1:
                                        case InteractionType.pet2:
                                        case InteractionType.pet3:
                                        case InteractionType.pet4:
                                        case InteractionType.pet5:
                                        case InteractionType.pet6:
                                        case InteractionType.pet7:
                                        case InteractionType.pet8:
                                        case InteractionType.pet9:
                                        case InteractionType.pet10:
                                        case InteractionType.pet11:
                                        case InteractionType.pet12:
                                        case InteractionType.pet13:
                                        case InteractionType.pet14:
                                        case InteractionType.pet15:
                                        case InteractionType.pet16:
                                        case InteractionType.pet17:
                                        case InteractionType.pet18:
                                        case InteractionType.pet19:
                                        case InteractionType.pet20:
                                        case InteractionType.pet21:
                                        case InteractionType.pet22:
                                            try
                                            {
                                                string[] strArray = ExtraData.Split(new char[] { '\n' });
                                                string petName = strArray[0];
                                                string str2 = strArray[1];
                                                string str3 = strArray[2];
                                                if (!CheckPetName(petName) || (str3.Length != 6))
                                                {
                                                    return;
                                                }
                                            }
                                            catch (Exception exception1)
                                            {
                                                exception = exception1;
                                                Logging.WriteLine(exception.ToString());
                                                Logging.HandleException(exception, "Catalog.HandlePurchase");
                                                return;
                                            }
                                            goto Label_0C75;

                                        case InteractionType.musicdisc:
                                            ExtraData = item.songID.ToString();
                                            goto Label_0C75;

                                        case InteractionType.rentals:
                                            ExtraData = item.ExtraData;
                                            goto Label_0C75;
                                    }
                                    ExtraData = "";
                                }
                            Label_0C75:
                                Session.GetMessageHandler().GetResponse().Init(Outgoing.UpdateInventary);
                                Session.GetMessageHandler().SendResponse();
                                Session.GetMessageHandler().GetResponse().Init(Outgoing.SerializePurchaseInformation);
                                Session.GetMessageHandler().GetResponse().AppendInt32(item.GetBaseItem(num8).ItemId);
                                Session.GetMessageHandler().GetResponse().AppendString(item.GetBaseItem(num8).Name);
                                Session.GetMessageHandler().GetResponse().AppendInt32(item.CreditsCost);
                                Session.GetMessageHandler().GetResponse().AppendInt32(item.PixelsCost);
                                Session.GetMessageHandler().GetResponse().AppendInt32(item.CrystalCost);
                                Session.GetMessageHandler().GetResponse().AppendBoolean(true);
                                Session.GetMessageHandler().GetResponse().AppendInt32(1);
                                Session.GetMessageHandler().GetResponse().AppendString(item.GetBaseItem(num8).Type.ToString().ToLower());
                                Session.GetMessageHandler().GetResponse().AppendInt32(item.GetBaseItem(num8).SpriteId);
                                Session.GetMessageHandler().GetResponse().AppendString("");
                                Session.GetMessageHandler().GetResponse().AppendInt32(1);
                                Session.GetMessageHandler().GetResponse().AppendInt32(0);
                                Session.GetMessageHandler().GetResponse().AppendString(ExtraData);
                                Session.GetMessageHandler().GetResponse().AppendInt32(1);
                                Session.GetMessageHandler().SendResponse();
                                if (IsGift)
                                {
                                    int showSender = 1;
                                    if (isAnonymous == false)
                                        showSender = 0;
                                    GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(userID);
                                    Item giftCase = ButterflyEnvironment.GetGame().GetItemManager().GetItemSprite(giftSprite);
                                    UserItem item3 = ButterflyEnvironment.getHabboForId(userID).GetInventoryComponent().AddNewItem(0, giftCase.ItemId, "", true, false, 0, "", clientByUserID.GetHabbo().Id, 0);
                                    ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_GiftGiver", 1);
                                    ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(clientByUserID, "ACH_GiftReceiver", 1);

                                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                                    {
                                        adapter.setQuery("INSERT INTO user_gifts (page_id,item_id,extradata,target_name,message,ribbon,color,gift_sprite,show_sender,rare_id,inventory_id) VALUES " +
                                            "(" + PageId + "," + num8 + ",@extradata, @name, @message," + giftRibbon + "," + giftColor + "," + giftSprite + "," + showSender + "," + limitedSelled + ",@inventory_id)");
                                        adapter.addParameter("name", giftReceiverName);
                                        adapter.addParameter("message", giftMessage);
                                        adapter.addParameter("extradata", ExtraData);
                                        adapter.addParameter("inventory_id", item3.Id);
                                        adapter.runQuery();
                                    }
                                    
                                    if (clientByUserID != null)
                                    {
                                        clientByUserID.SendNotif(LanguageLocale.GetValue("catalog.gift.received") + Session.GetHabbo().Username);
                                        clientByUserID.GetHabbo().GetInventoryComponent().SendFloorInventoryUpdate();
                                        clientByUserID.GetMessageHandler().GetResponse().Init(Outgoing.SendPurchaseAlert);
                                        clientByUserID.GetMessageHandler().GetResponse().AppendInt32(1);
                                        clientByUserID.GetMessageHandler().GetResponse().AppendInt32(1);
                                        clientByUserID.GetMessageHandler().GetResponse().AppendInt32(1);
                                        clientByUserID.GetMessageHandler().GetResponse().AppendInt32(item3.Id);
                                        clientByUserID.GetMessageHandler().SendResponse();
                                        InventoryComponent inventoryComponent = clientByUserID.GetHabbo().GetInventoryComponent();
                                        if (inventoryComponent != null)
                                        {
                                            inventoryComponent.RunDBUpdate();
                                        }
                                    }
                                    Session.SendNotif(LanguageLocale.GetValue("catalog.gift.sent"));
                                }
                                else
                                {
                                    Session.GetMessageHandler().GetResponse().Init(Outgoing.SendPurchaseAlert);
                                    Session.GetMessageHandler().GetResponse().AppendInt32(1);
                                    int i = 2;
                                    if (item.GetBaseItem(num8).Type.ToString().ToLower().Equals("s"))
                                    {
                                        if (item.GetBaseItem(num8).InteractionType == InteractionType.pet)
                                        {
                                            i = 3;
                                        }
                                        else
                                        {
                                            i = 1;
                                        }
                                    }
                                    Session.GetMessageHandler().GetResponse().AppendInt32(i);
                                    List<UserItem> list = this.DeliverItems(Session, item.GetBaseItem(num8), priceAmount * item.Amount, ExtraData, (uint)limitedSelled, item.songID);
                                    Session.GetMessageHandler().GetResponse().AppendInt32(list.Count);
                                    foreach (UserItem item3 in list)
                                    {
                                        Session.GetMessageHandler().GetResponse().AppendInt32(item3.Id);
                                    }
                                    Session.GetMessageHandler().SendResponse();
                                    Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                                }
                            }
                        }
                    }
                }
            }
        }

        internal void InitCache()
        {
            this.mCataIndexCache = new ServerMessage[8];
            this.RaresItems = new Dictionary<uint, CatalogItem>();
            for (int i = 1; i < 8; i++)
            {
                this.mCataIndexCache[i] = this.SerializeIndexForCache(i);
            }
            foreach (CatalogPage page in this.Pages.Values)
            {
                page.InitMsg();
            }
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM items_base_gifts");
                DataTable table = adapter.getTable();
                foreach (DataRow row in table.Rows)
                {
                    uint id = Convert.ToUInt32(row[0]);
                    Item item = ButterflyEnvironment.GetGame().GetItemManager().GetItem(id);
                    if (item != null)
                    {
                        this.gifts.Add(id, item);
                    }
                }
            }
            this.mGuildPageBuy = GetGuildPage();
        }

        internal void Initialize(IQueryAdapter dbClient)
        {
            this.Pages = new Dictionary<int, CatalogPage>();
            this.EcotronRewards = new List<EcotronReward>();
            this.gifts = new Hashtable();
            dbClient.runFastQuery("UPDATE catalog_items_copy,items_base SET catalog_items_copy.amount = 1 WHERE items_base.allow_gift = 1 AND catalog_items_copy.amount > 1");
            dbClient.setQuery("SELECT * FROM catalog_pages ORDER BY order_num");
            DataTable table = dbClient.getTable();
            dbClient.setQuery("SELECT * FROM ecotron_rewards ORDER BY item_id");
            DataTable table2 = dbClient.getTable();
            Hashtable cataItems = new Hashtable();
            dbClient.setQuery("SELECT * FROM catalog_items_copy");
            DataTable table3 = dbClient.getTable();
            if (table3 != null)
            {
                foreach (DataRow row in table3.Rows)
                {
                    if (!string.IsNullOrEmpty(row["item_ids"].ToString()) && (((int) row["amount"]) > 0))
                    {
                        cataItems.Add(Convert.ToUInt32(row["id"]), new CatalogItem(row));
                    }
                }
            }
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    bool visible = false;
                    bool enabled = false;
                    bool comingSoon = false;
                    if (row["visible"].ToString() == "1")
                    {
                        visible = true;
                    }
                    if (row["enabled"].ToString() == "1")
                    {
                        enabled = true;
                    }
                    if (row["coming_soon"].ToString() == "1")
                    {
                        comingSoon = true;
                    }
                    this.Pages.Add((int) row["id"], new CatalogPage((int) row["id"], (int) row["parent_id"], (string) row["caption"], visible, enabled, comingSoon, Convert.ToUInt32(row["min_rank"]), ButterflyEnvironment.EnumToBool(row["club_only"].ToString()), (int) row["icon_color"], (int) row["icon_image"], (string) row["page_layout"], (string) row["page_headline"], (string) row["page_teaser"], (string) row["page_special"], (string) row["page_text1"], (string) row["page_text2"], (string) row["page_text_details"], (string) row["page_text_teaser"], ref cataItems));
                }
            }
            if (table2 != null)
            {
                foreach (DataRow row in table2.Rows)
                {
                    this.EcotronRewards.Add(new EcotronReward(Convert.ToUInt32(row["display_id"]), Convert.ToUInt32(row["item_id"]), Convert.ToUInt32(row["reward_level"])));
                }
            }
            this.RestackByFrontpage();
        }

        internal void RestackByFrontpage()
        {
            CatalogPage page = this.Pages[1];
            Dictionary<int, CatalogPage> dictionary = new Dictionary<int, CatalogPage>(this.Pages);
            dictionary.Remove(1);
            this.Pages.Clear();
            this.Pages.Add(page.PageId, page);
            foreach (KeyValuePair<int, CatalogPage> pair in dictionary)
            {
                this.Pages.Add(pair.Key, pair.Value);
            }
        }

        internal ServerMessage SerializeIndexForCache(int rank)
        {
            ServerMessage message = new ServerMessage(Outgoing.OpenShop);
            message.AppendBoolean(true);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendInt32(-1);
            message.AppendString("root");
            message.AppendString("");
            message.AppendInt32(this.GetTreeSize(rank, -1));
            foreach (CatalogPage page in this.Pages.Values)
            {
                if ((page.ParentId == -1) && (page.MinRank <= rank))
                {
                    page.Serialize(rank, message);
                    foreach (CatalogPage page2 in this.Pages.Values)
                    {
                        if (page2.ParentId == page.PageId)
                        {
                            page2.Serialize(rank, message);
                        }
                    }
                }
            }
            message.AppendBoolean(false);
            return message;
        }

        internal static ServerMessage SerializePage(CatalogPage Page)
        {
            ServerMessage message = new ServerMessage(Outgoing.OpenShopPage);
            message.AppendInt32(Page.PageId);
            switch (Page.Layout)
            {
                case "frontpage":
                    message.AppendString("frontpage3");
                    message.AppendInt32(3);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendString(Page.LayoutTeaser);
                    message.AppendString("");
                    message.AppendInt32(11);
                    message.AppendString(Page.LayoutSpecial);
                    message.AppendString(Page.Text1);
                    message.AppendString("");
                    message.AppendString(Page.Text2);
                    message.AppendString(Page.TextTeaser);
                    message.AppendString(Page.TextDetails);
                    message.AppendString("");
                    message.AppendString("#FEFEFE");
                    message.AppendString("#FEFEFE");
                    message.AppendString(LanguageLocale.GetValue("catalog.waystogetcredits"));
                    message.AppendString("credits");
                    break;

                case "guilds":
                    message.AppendString("guild_frontpage");
                    message.AppendInt32(2);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendString(Page.LayoutTeaser);
                    message.AppendInt32(3);
                    message.AppendString(Page.Text1);
                    message.AppendString(Page.TextDetails.Replace("[13]", Convert.ToChar(13).ToString()).Replace("[10]", Convert.ToChar(13).ToString()));
                    message.AppendString(Page.TextTeaser);
                    break;

                case "guild_furni":
                    message.AppendString("guild_custom_furni");
                    message.AppendInt32(3);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendString(Page.LayoutTeaser);
                    message.AppendString(Page.LayoutSpecial);
                    message.AppendInt32(3);
                    message.AppendString(Page.Text1);
                    message.AppendString(Page.TextDetails);
                    message.AppendString(Page.TextTeaser);
                    break;

                case "recycler_info":
                    message.AppendString(Page.Layout);
                    message.AppendInt32(2);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendString(Page.LayoutTeaser);
                    message.AppendInt32(3);
                    message.AppendString(Page.Text1);
                    message.AppendString(Page.Text2);
                    message.AppendString(Page.TextDetails);
                    break;

                case "recycler_prizes":
                    message.AppendString("recycler_prizes");
                    message.AppendInt32(1);
                    message.AppendString("catalog_recycler_headline3");
                    message.AppendInt32(1);
                    message.AppendString(Page.Text1);
                    break;

                case "spaces_new":
                    message.AppendString(Page.Layout);
                    message.AppendInt32(1);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendInt32(1);
                    message.AppendString(Page.Text1);
                    break;

                case "recycler":
                    message.AppendString(Page.Layout);
                    message.AppendInt32(2);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendString(Page.LayoutTeaser);
                    message.AppendInt32(3);
                    message.AppendString(Page.Text1, 10);
                    message.AppendString(Page.Text2);
                    message.AppendString(Page.TextDetails);
                    break;

                case "trophies":
                    message.AppendString("trophies");
                    message.AppendInt32(1);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendInt32(2);
                    message.AppendString(Page.Text1);
                    message.AppendString(Page.TextDetails);
                    break;

                case "pets":
                    message.AppendString("pets");
                    message.AppendInt32(2);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendString(Page.LayoutTeaser);
                    message.AppendInt32(4);
                    message.AppendString(Page.Text1);
                    message.AppendString(LanguageLocale.GetValue("catalog.pickname"));
                    message.AppendString(LanguageLocale.GetValue("catalog.pickcolor"));
                    message.AppendString(LanguageLocale.GetValue("catalog.pickrace"));
                    break;

                case "soundmachine":
                    message.AppendString("soundmachine");
                    message.AppendInt32(2);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendString(Page.LayoutTeaser);
                    message.AppendInt32(2);
                    message.AppendString(Page.Text1);
                    message.AppendString(Page.TextDetails);
                    break;

                case "club_buy":
                    message.AppendString("vip_buy");
                    message.AppendInt32(2);
                    message.AppendString("ctlg_buy_vip_header");
                    message.AppendString("ctlg_gift_vip_teaser");
                    message.AppendInt32(0);
                    break;

                case "bots":

                    message.AppendString(Page.Layout);
                    message.AppendInt32(3);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendString(Page.LayoutTeaser);
                    message.AppendString(Page.LayoutSpecial);
                    message.AppendInt32(3);
                    message.AppendString(Page.Text1);
                    message.AppendString(Page.TextDetails);
                    message.AppendString(Page.TextTeaser);

                    break;

                case "roomads":

                    message.AppendString("roomads");
                    message.AppendInt32(2);
                    message.AppendString("events_header");
                    message.AppendString("");
                    message.AppendInt32(2);
                    message.AppendInt32(0);//0
                    message.AppendInt32(1);
                    message.AppendInt32(7384);
                    message.AppendString("room_ad_plus_badge");
                    message.AppendInt32(0);//0
                    message.AppendString("");
                    message.AppendInt32(3840);
                    message.AppendInt32(0);//0
                    message.AppendInt32(1);
                    message.AppendString("b");
                    message.AppendString("RADZZ");

                    break;

                default:
                    message.AppendString(Page.Layout);
                    message.AppendInt32(3);
                    message.AppendString(Page.LayoutHeadline);
                    message.AppendString(Page.LayoutTeaser);
                    message.AppendString(Page.LayoutSpecial);
                    message.AppendInt32(3);
                    message.AppendString(Page.Text1);
                    message.AppendString(Page.TextDetails);
                    message.AppendString(Page.TextTeaser);
                    break;
            }
            if ((!Page.Layout.Equals("frontpage") && !Page.Layout.Equals("club_buy")) && !Page.Layout.Equals("guilds") && !Page.Layout.Equals("roomads"))
            {
                message.AppendInt32(Page.Items.Count);
                foreach (CatalogItem item in Page.Items.Values)
                {
                    item.Serialize(message);
                    if (item.IsLimited && !ButterflyEnvironment.GetGame().GetCatalog().RaresItems.ContainsKey(item.Items[0]))
                    {
                        ButterflyEnvironment.GetGame().GetCatalog().RaresItems.Add(item.Items[0], item);
                    }
                }
            }
            else
            {
                message.AppendInt32(0);
            }
            message.AppendInt32(-1);
            message.AppendBoolean(false);
            return message;
        }

           internal Dictionary<uint, List<EcotronReward>> GetEcotronRewards()
        {
            Dictionary<uint, List<EcotronReward>> list = new Dictionary<uint, List<EcotronReward>>();
            foreach (EcotronReward reward in this.EcotronRewards)
            {
               
                 if (!list.ContainsKey(reward.RewardLevel))
                {
                    list.Add(reward.RewardLevel, new List<EcotronReward>());
                    list[reward.RewardLevel].Add(reward);
                 
                }
                else
                {
                    list[reward.RewardLevel].Add(reward);
                   
                }
               
            }
            return list;
        }
    }
}

