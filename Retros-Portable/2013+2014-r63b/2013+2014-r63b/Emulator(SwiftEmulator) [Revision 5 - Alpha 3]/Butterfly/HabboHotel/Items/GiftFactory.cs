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
using Butterfly.HabboHotel.Catalogs;
using System.Runtime.InteropServices;
using Butterfly.HabboHotel.RoomBots;

namespace Butterfly.HabboHotel.Items
{
    class GiftFactory
    {
        public static void DeliverGift()
        {

        }
        public static void UnwrapGift() { }
        public static void PlaceGift() { }
        public static void SerializePurchase(CatalogItem item, uint itemId, GameClient Session, uint receiverId, int giftSprite, int giftId) 
        {
            Item giftCase = ButterflyEnvironment.GetGame().GetItemManager().GetItemSprite(giftSprite);

            Console.WriteLine(item.GetBaseItem(itemId).ItemId);
            Console.WriteLine(item.GetBaseItem(itemId).Name);
            Console.WriteLine(item.GetBaseItem(itemId).Type.ToString().ToLower());
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
            //Session.GetMessageHandler().GetResponse().AppendInt32(1);
            Session.GetMessageHandler().SendResponse();

            GameClient clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(receiverId);
            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_GiftGiver", 1);
            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(clientByUserID, "ACH_GiftReceiver", 1);

            UserItem item3;
            try
            {
                item3 = ButterflyEnvironment.getHabboForId(receiverId).GetInventoryComponent().AddNewItem(0, giftCase.ItemId, "", true, false, 0, "", clientByUserID.GetHabbo().Id, 0);
            }
            catch (Exception ex)
            {
                Butterfly.Core.Logging.LogCriticalException("Failed to send gift for gift_id = " + giftId +"\n"+ex.ToString());
                return;
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

    }
}
