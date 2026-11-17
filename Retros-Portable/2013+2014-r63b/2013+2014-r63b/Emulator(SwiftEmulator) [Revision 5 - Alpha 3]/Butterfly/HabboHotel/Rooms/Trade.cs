namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Items;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections.Generic;

    internal class Trade
    {
        private uint oneId;
        private uint RoomId;
        private int TradeStage;
        private uint twoId;
        private TradeUser[] Users;

        internal Trade(uint UserOneId, uint UserTwoId, uint RoomId)
        {
            this.oneId = UserOneId;
            this.twoId = UserTwoId;
            this.Users = new TradeUser[] { new TradeUser(UserOneId, RoomId), new TradeUser(UserTwoId, RoomId) };
            this.TradeStage = 1;
            this.RoomId = RoomId;
            foreach (TradeUser user in this.Users)
            {
                if (!user.GetRoomUser().Statusses.ContainsKey("trd"))
                {
                    user.GetRoomUser().AddStatus("trd", "");
                    user.GetRoomUser().UpdateNeeded = true;
                }
            }
            ServerMessage message = new ServerMessage(Outgoing.TradeStart);
            message.AppendInt32(UserOneId);
            message.AppendInt32(1);
            message.AppendInt32(UserTwoId);
            message.AppendInt32(1);
            this.SendMessageToUsers(message);
        }

        internal void Accept(uint UserId)
        {
            TradeUser tradeUser = this.GetTradeUser(UserId);
            if ((tradeUser != null) && (this.TradeStage == 1))
            {
                tradeUser.HasAccepted = true;
                ServerMessage message = new ServerMessage(Outgoing.TradeAcceptUpdate);
                message.AppendInt32(UserId);
                message.AppendInt32(1);
                this.SendMessageToUsers(message);
                if (this.AllUsersAccepted)
                {
                    this.SendMessageToUsers(new ServerMessage(Outgoing.TradeComplete));
                    this.TradeStage++;
                    this.ClearAccepted();
                }
            }
        }

        internal void ClearAccepted()
        {
            foreach (TradeUser user in this.Users)
            {
                user.HasAccepted = false;
            }
        }

        internal void CloseTrade(uint UserId)
        {
            for (int i = 0; i < this.Users.Length; i++)
            {
                TradeUser user = this.Users[i];
                if ((user != null) && (user.GetRoomUser() != null))
                {
                    user.GetRoomUser().RemoveStatus("trd");
                    user.GetRoomUser().UpdateNeeded = true;
                }
            }
            ServerMessage message = new ServerMessage(Outgoing.TradeClose);
            message.AppendInt32(UserId);
            message.AppendInt32(2);
            this.SendMessageToUsers(message);
        }

        internal void CloseTradeClean()
        {
            for (int i = 0; i < this.Users.Length; i++)
            {
                TradeUser user = this.Users[i];
                if ((user != null) && (user.GetRoomUser() != null))
                {
                    user.GetRoomUser().RemoveStatus("trd");
                    user.GetRoomUser().UpdateNeeded = true;
                }
            }
            this.SendMessageToUsers(new ServerMessage(Outgoing.TradeCloseClean));
            this.GetRoom().ActiveTrades.Remove(this);
        }

        internal void CompleteTrade(uint UserId)
        {
            TradeUser tradeUser = this.GetTradeUser(UserId);
            if ((tradeUser != null) && (this.TradeStage == 2))
            {
                tradeUser.HasAccepted = true;
                ServerMessage message = new ServerMessage(Outgoing.TradeAcceptUpdate);
                message.AppendInt32(UserId);
                message.AppendInt32(1);
                this.SendMessageToUsers(message);
                if (this.AllUsersAccepted)
                {
                    this.TradeStage = 0x3e7;
                    this.Finnito();
                }
            }
        }

        internal bool ContainsUser(uint Id)
        {
            for (int i = 0; i < this.Users.Length; i++)
            {
                if ((this.Users[i] != null) && (this.Users[i].UserId == Id))
                {
                    return true;
                }
            }
            return false;
        }

        internal void DeliverItems()
        {
            IQueryAdapter adapter;
            List<UserItem> offeredItems = this.GetTradeUser(this.oneId).OfferedItems;
            List<UserItem> list2 = this.GetTradeUser(this.twoId).OfferedItems;
            foreach (UserItem item in offeredItems)
            {
                if (this.GetTradeUser(this.oneId).GetClient().GetHabbo().GetInventoryComponent().GetItem(item.Id) == null)
                {
                    this.GetTradeUser(this.oneId).GetClient().SendNotif(LanguageLocale.GetValue("trade.failed"));
                    this.GetTradeUser(this.twoId).GetClient().SendNotif(LanguageLocale.GetValue("trade.failed"));
                    return;
                }
            }
            foreach (UserItem item in list2)
            {
                if (this.GetTradeUser(this.twoId).GetClient().GetHabbo().GetInventoryComponent().GetItem(item.Id) == null)
                {
                    this.GetTradeUser(this.oneId).GetClient().SendNotif(LanguageLocale.GetValue("trade.failed"));
                    this.GetTradeUser(this.twoId).GetClient().SendNotif(LanguageLocale.GetValue("trade.failed"));
                    return;
                }
            }
            foreach (UserItem item in offeredItems)
            {
                this.GetTradeUser(this.oneId).GetClient().GetHabbo().GetInventoryComponent().RemoveItem(item.Id, false);
                this.GetTradeUser(this.twoId).GetClient().GetHabbo().GetInventoryComponent().AddNewItem(item.Id, item.BaseItem, item.ExtraData, false, false, item.RareId, item.Group_data, this.twoId,0);
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE items SET placedBy = ",this.twoId," WHERE item_id = ", item.Id }));
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE items_users SET user_id = ", this.twoId, " WHERE item_id = ", item.Id }));
                }
            }
            foreach (UserItem item in list2)
            {
                this.GetTradeUser(this.twoId).GetClient().GetHabbo().GetInventoryComponent().RemoveItem(item.Id, false);
                this.GetTradeUser(this.oneId).GetClient().GetHabbo().GetInventoryComponent().AddNewItem(item.Id, item.BaseItem, item.ExtraData, false, false, item.RareId, item.Group_data,this.oneId, 0);
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE items SET placedBy = ", this.oneId, " WHERE item_id = ", item.Id }));
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE items_users SET user_id = ", this.oneId, " WHERE item_id = ", item.Id }));
                }
            }
            ServerMessage message = new ServerMessage(Outgoing.SendPurchaseAlert);
            message.AppendInt32(1);
            int i = 1;
            foreach (UserItem item2 in offeredItems)
            {
                if (item2.GetBaseItem().Type.ToString().ToLower() != "s")
                {
                    i = 2;
                }
            }
            message.AppendInt32(i);
            message.AppendInt32(offeredItems.Count);
            foreach (UserItem item2 in offeredItems)
            {
                message.AppendInt32(item2.Id);
            }
            this.GetTradeUser(this.twoId).GetClient().SendMessage(message);
            ServerMessage message2 = new ServerMessage(Outgoing.SendPurchaseAlert);
            message2.AppendInt32(1);
            i = 1;
            foreach (UserItem item2 in list2)
            {
                if (item2.GetBaseItem().Type.ToString().ToLower() != "s")
                {
                    i = 2;
                }
            }
            message2.AppendInt32(i);
            message2.AppendInt32(list2.Count);
            foreach (UserItem item2 in list2)
            {
                message2.AppendInt32(item2.Id);
            }
            this.GetTradeUser(this.oneId).GetClient().SendMessage(message2);
            this.GetTradeUser(this.oneId).GetClient().GetHabbo().GetInventoryComponent().UpdateItems(false);
            this.GetTradeUser(this.twoId).GetClient().GetHabbo().GetInventoryComponent().UpdateItems(false);
        }

        private void Finnito()
        {
            try
            {
                this.DeliverItems();
                this.CloseTradeClean();
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "Trade task");
            }
        }

        private Room GetRoom()
        {
            return ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(this.RoomId);
        }

        internal TradeUser GetTradeUser(uint Id)
        {
            for (int i = 0; i < this.Users.Length; i++)
            {
                if ((this.Users[i] != null) && (this.Users[i].UserId == Id))
                {
                    return this.Users[i];
                }
            }
            return null;
        }

        internal void OfferItem(uint UserId, UserItem Item)
        {
            TradeUser tradeUser = this.GetTradeUser(UserId);
            if ((((tradeUser != null) && (Item != null)) && (Item.GetBaseItem().AllowTrade && !tradeUser.HasAccepted)) && (this.TradeStage == 1))
            {
                this.ClearAccepted();
                if (!tradeUser.OfferedItems.Contains(Item))
                {
                    tradeUser.OfferedItems.Add(Item);
                }
                this.UpdateTradeWindow();
            }
        }

        internal void SendMessageToUsers(ServerMessage Message)
        {
            if (this.Users != null)
            {
                for (int i = 0; i < this.Users.Length; i++)
                {
                    TradeUser user = this.Users[i];
                    if ((user != null) && ((user != null) && (user.GetClient() != null)))
                    {
                        user.GetClient().SendMessage(Message);
                    }
                }
            }
        }

        internal void TakeBackItem(uint UserId, UserItem Item)
        {
            TradeUser tradeUser = this.GetTradeUser(UserId);
            if ((((tradeUser != null) && (Item != null)) && !tradeUser.HasAccepted) && (this.TradeStage == 1))
            {
                this.ClearAccepted();
                tradeUser.OfferedItems.Remove(Item);
                this.UpdateTradeWindow();
            }
        }

        internal void Unaccept(uint UserId)
        {
            TradeUser tradeUser = this.GetTradeUser(UserId);
            if (((tradeUser != null) && (this.TradeStage == 1)) && !this.AllUsersAccepted)
            {
                tradeUser.HasAccepted = false;
                ServerMessage message = new ServerMessage(Outgoing.TradeAcceptUpdate);
                message.AppendInt32(UserId);
                message.AppendInt32(0);
                this.SendMessageToUsers(message);
            }
        }

        internal void UpdateTradeWindow()
        {
            ServerMessage message = new ServerMessage(Outgoing.TradeUpdate);
            for (int i = 0; i < this.Users.Length; i++)
            {
                TradeUser user = this.Users[i];
                if (user != null)
                {
                    message.AppendInt32(user.UserId);
                    message.AppendInt32(user.OfferedItems.Count);
                    foreach (UserItem item in user.OfferedItems)
                    {
                        message.AppendInt32(item.Id);
                        message.AppendString(item.GetBaseItem().Type.ToString().ToLower());
                        message.AppendInt32(item.Id);
                        message.AppendInt32(item.GetBaseItem().SpriteId);
                        message.AppendInt32(0);
                        message.AppendBoolean(true);
                        message.AppendInt32(0);
                        message.AppendString("");
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        if (item.GetBaseItem().Type == 's')
                        {
                            message.AppendInt32(0);
                        }
                    }
                }
            }
            this.SendMessageToUsers(message);
        }

        internal bool AllUsersAccepted
        {
            get
            {
                for (int i = 0; i < this.Users.Length; i++)
                {
                    if ((this.Users[i] != null) && !this.Users[i].HasAccepted)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}

