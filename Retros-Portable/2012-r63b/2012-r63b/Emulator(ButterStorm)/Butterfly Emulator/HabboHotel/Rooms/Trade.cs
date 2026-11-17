using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Butterfly.Core;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Items;
using Butterfly.Messages;
using HabboEvents;

namespace Butterfly.HabboHotel.Rooms
{
    class Trade
    {
        private TradeUser[] Users;
        private int TradeStage;
        private UInt32 RoomId;

        private UInt32 oneId;
        private UInt32 twoId;

        internal Trade(UInt32 UserOneId, UInt32 UserTwoId, UInt32 RoomId)
        {
            this.oneId = UserOneId;
            this.twoId = UserTwoId;

            this.Users = new TradeUser[2];
            this.Users[0] = new TradeUser(UserOneId, RoomId);
            this.Users[1] = new TradeUser(UserTwoId, RoomId);
            this.TradeStage = 1;
            this.RoomId = RoomId;

            foreach (TradeUser User in Users)
            {
                if (!User.GetRoomUser().Statusses.ContainsKey("trd"))
                {
                    User.GetRoomUser().AddStatus("trd", "");
                    User.GetRoomUser().UpdateNeeded = true;
                }
            }

            ServerMessage Message = new ServerMessage(Outgoing.TradeStart);
            Message.AppendUInt(UserOneId);
            Message.AppendInt32(1); // ready
            Message.AppendUInt(UserTwoId);
            Message.AppendInt32(1); // ready
            SendMessageToUsers(Message);
        }

        internal bool AllUsersAccepted
        {
            get
            {
                for (int i = 0; i < Users.Length; i++)
                {
                    if (Users[i] == null)
                        continue;
                    if (!Users[i].HasAccepted)
                        return false;
                }

                return true;
            }
        }

        internal bool ContainsUser(UInt32 Id)
        {
            for (int i = 0; i < Users.Length; i++)
            {
                if (Users[i] == null)
                    continue;
                if (Users[i].UserId == Id)
                    return true;
            }

            return false;
        }

        internal TradeUser GetTradeUser(UInt32 Id)
        {
            for (int i = 0; i < Users.Length; i++)
            {
                if (Users[i] == null)
                    continue;
                if (Users[i].UserId == Id)
                    return Users[i];
            }

            return null;
        }

        internal void OfferItem(UInt32 UserId, UserItem Item)
        {
            TradeUser User = GetTradeUser(UserId);

            if (User == null || Item == null || !Item.GetBaseItem().AllowTrade || User.HasAccepted || TradeStage != 1)
            {
                return;
            }

            ClearAccepted();
            if(!User.OfferedItems.Contains(Item))
                User.OfferedItems.Add(Item);
            UpdateTradeWindow();
        }

        internal void TakeBackItem(UInt32 UserId, UserItem Item)
        {
            TradeUser User = GetTradeUser(UserId);

            if (User == null || Item == null || User.HasAccepted || TradeStage != 1)
            {
                return;
            }

            ClearAccepted();

            User.OfferedItems.Remove(Item);
            UpdateTradeWindow();
        }

        internal void Accept(UInt32 UserId)
        {
            TradeUser User = GetTradeUser(UserId);

            if (User == null || TradeStage != 1)
            {
                return;
            }

            User.HasAccepted = true;

            ServerMessage Message = new ServerMessage(Outgoing.TradeAcceptUpdate);
            Message.AppendUInt(UserId);
            Message.AppendInt32(1);
            SendMessageToUsers(Message);

            if (AllUsersAccepted)
            {
                SendMessageToUsers(new ServerMessage(Outgoing.TradeComplete));
                TradeStage++;
                ClearAccepted();
            }
        }

        internal void Unaccept(UInt32 UserId)
        {
            TradeUser User = GetTradeUser(UserId);

            if (User == null || TradeStage != 1 || AllUsersAccepted)
            {
                return;
            }

            User.HasAccepted = false;

            ServerMessage Message = new ServerMessage(Outgoing.TradeAcceptUpdate);
            Message.AppendUInt(UserId);
            Message.AppendInt32(0);
            SendMessageToUsers(Message);
        }

        internal void CompleteTrade(UInt32 UserId)
        {
            TradeUser User = GetTradeUser(UserId);

            if (User == null || TradeStage != 2)
            {
                return;
            }

            User.HasAccepted = true;

            ServerMessage Message = new ServerMessage(Outgoing.TradeAcceptUpdate);
            Message.AppendUInt(UserId);
            Message.AppendInt32(1);
            SendMessageToUsers(Message);

            if (AllUsersAccepted)
            {
                TradeStage = 999;
                Finnito();
                //Task pTask = new Task(Finnito);
                //pTask.Start();
            }
        }

        private void Finnito()
        {
            try
            {
                DeliverItems();
                CloseTradeClean();
            }
            catch (Exception e)
            {
                Logging.LogThreadException(e.ToString(), "Trade task");
            }
        }

        internal void ClearAccepted()
        {
            foreach (TradeUser User in Users)
            {
                User.HasAccepted = false;
            }
        }

        internal void UpdateTradeWindow()
        {
            ServerMessage Message = new ServerMessage(Outgoing.TradeUpdate);

            for (int i = 0; i < Users.Length; i++)
            {
                TradeUser User = Users[i];
                if (User == null)
                    continue;

                Message.AppendUInt(User.UserId);
                Message.AppendInt32(User.OfferedItems.Count);

                foreach (UserItem Item in User.OfferedItems)
                {
                    Message.AppendUInt(Item.Id);
                    Message.AppendStringWithBreak(Item.GetBaseItem().Type.ToString().ToLower());
                    Message.AppendUInt(Item.Id);
                    Message.AppendInt32(Item.GetBaseItem().SpriteId);
                    Message.AppendInt32(0); // undef
                    Message.AppendBoolean(true);
                    Message.AppendInt32(0);
                    Message.AppendStringWithBreak("");
                    Message.AppendInt32(0); // xmas 09 furni had a special furni tag here, with wired day (wat?)
                    Message.AppendInt32(0); // xmas 09 furni had a special furni tag here, wired month (wat?)
                    Message.AppendInt32(0); // xmas 09 furni had a special furni tag here, wired year (wat?)
                    
                    if (Item.GetBaseItem().Type == 's')
                    {
                        Message.AppendInt32(0);
                    }
                }
            }


            SendMessageToUsers(Message);
        }

        internal void DeliverItems()
        {
            // List items
            List<UserItem> ItemsOne = GetTradeUser(oneId).OfferedItems;
            List<UserItem> ItemsTwo = GetTradeUser(twoId).OfferedItems;

            // Verify they are still in user inventory
            foreach (UserItem I in ItemsOne)
            {
                if (GetTradeUser(oneId).GetClient().GetHabbo().GetInventoryComponent().GetItem(I.Id) == null)
                {
                    GetTradeUser(oneId).GetClient().SendNotif(LanguageLocale.GetValue("trade.failed"));
                    GetTradeUser(twoId).GetClient().SendNotif(LanguageLocale.GetValue("trade.failed"));

                    return;
                }
            }

            foreach (UserItem I in ItemsTwo)
            {
                if (GetTradeUser(twoId).GetClient().GetHabbo().GetInventoryComponent().GetItem(I.Id) == null)
                {
                    GetTradeUser(oneId).GetClient().SendNotif(LanguageLocale.GetValue("trade.failed"));
                    GetTradeUser(twoId).GetClient().SendNotif(LanguageLocale.GetValue("trade.failed"));

                    return;
                }
            }

            GetTradeUser(twoId).GetClient().GetHabbo().GetInventoryComponent().RunDBUpdate();
            GetTradeUser(oneId).GetClient().GetHabbo().GetInventoryComponent().RunDBUpdate();
            // Deliver them
            foreach (UserItem I in ItemsOne)
            {
                GetTradeUser(oneId).GetClient().GetHabbo().GetInventoryComponent().RemoveItem(I.Id, false);
                GetTradeUser(twoId).GetClient().GetHabbo().GetInventoryComponent().AddNewItem(I.Id, I.BaseItem, I.ExtraData, false, false, 0);

                GetTradeUser(oneId).GetClient().GetHabbo().GetInventoryComponent().RunDBUpdate();
                GetTradeUser(twoId).GetClient().GetHabbo().GetInventoryComponent().RunDBUpdate();
            }

            foreach (UserItem I in ItemsTwo)
            {
                GetTradeUser(twoId).GetClient().GetHabbo().GetInventoryComponent().RemoveItem(I.Id, false);
                GetTradeUser(oneId).GetClient().GetHabbo().GetInventoryComponent().AddNewItem(I.Id, I.BaseItem, I.ExtraData, false, false, 0);

                GetTradeUser(twoId).GetClient().GetHabbo().GetInventoryComponent().RunDBUpdate();
                GetTradeUser(oneId).GetClient().GetHabbo().GetInventoryComponent().RunDBUpdate();
            }

            // Send warn
            ServerMessage MessageForOne = new ServerMessage(Outgoing.SendPurchaseAlert);
            MessageForOne.AppendInt32(1); // items
            int type = 1;
            foreach (UserItem item in ItemsOne)
            {
                if (item.GetBaseItem().Type.ToString().ToLower() != "s")
                    type = 2;
            }
            MessageForOne.AppendInt32(type);
            MessageForOne.AppendInt32(ItemsOne.Count);
            foreach (UserItem item in ItemsOne)
            {
                MessageForOne.AppendUInt(item.Id);
            }
            GetTradeUser(twoId).GetClient().SendMessage(MessageForOne);

            ServerMessage MessageForTwo = new ServerMessage(Outgoing.SendPurchaseAlert);
            MessageForTwo.AppendInt32(1); // items
            type = 1;
            foreach (UserItem item in ItemsTwo)
            {
                if (item.GetBaseItem().Type.ToString().ToLower() != "s")
                    type = 2;
            }
            MessageForTwo.AppendInt32(type);
            MessageForTwo.AppendInt32(ItemsTwo.Count);
            foreach (UserItem item in ItemsTwo)
            {
                MessageForTwo.AppendUInt(item.Id);
            }
            GetTradeUser(oneId).GetClient().SendMessage(MessageForTwo);

            // Update inventories
            GetTradeUser(oneId).GetClient().GetHabbo().GetInventoryComponent().UpdateItems(false);
            GetTradeUser(twoId).GetClient().GetHabbo().GetInventoryComponent().UpdateItems(false);
        }

        internal void CloseTradeClean()
        {
            for (int i = 0; i < Users.Length; i++)
            {
                TradeUser User = Users[i];
                if (User == null)
                    continue;
                if (User.GetRoomUser() == null)
                    continue;

                User.GetRoomUser().RemoveStatus("trd");
                User.GetRoomUser().UpdateNeeded = true;
            }

            SendMessageToUsers(new ServerMessage(Outgoing.TradeCloseClean));
            GetRoom().ActiveTrades.Remove(this);
        }

        internal void CloseTrade(UInt32 UserId)
        {
            for (int i = 0; i < Users.Length; i++)
            {
                TradeUser User = Users[i];
                if (User == null)
                    continue;
                if (User.GetRoomUser() == null)
                    continue;

                User.GetRoomUser().RemoveStatus("trd");
                User.GetRoomUser().UpdateNeeded = true;
            }

            ServerMessage Message = new ServerMessage(Outgoing.TradeClose);
            Message.AppendUInt(UserId);
            SendMessageToUsers(Message);
        }

        internal void SendMessageToUsers(ServerMessage Message)
        {
            if (Users == null)
                return;
            for (int i = 0; i < Users.Length; i++)
            {
                TradeUser User = Users[i];
                if (User == null)
                    continue;
                if (User != null)
                    if (User.GetClient() != null)
                        User.GetClient().SendMessage(Message);
            }
        }

        private Room GetRoom()
        {
            return ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);
        }
    }

    class TradeUser
    {
        internal UInt32 UserId;
        private UInt32 RoomId;
        private bool Accepted;

        internal List<UserItem> OfferedItems;

        internal bool HasAccepted
        {
            get
            {
                return Accepted;
            }

            set
            {
                Accepted = value;
            }
        }

        internal TradeUser(UInt32 UserId, UInt32 RoomId)
        {
            this.UserId = UserId;
            this.RoomId = RoomId;
            this.Accepted = false;
            this.OfferedItems = new List<UserItem>();
        }

        internal RoomUser GetRoomUser()
        {
            Room Room = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(RoomId);

            if (Room == null)
            {
                return null;
            }
            
            return Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
        }

        internal GameClient GetClient()
        {
            return ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
        }
    }
}
