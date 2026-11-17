namespace Butterfly.HabboHotel.Rooms.Wired
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Conditions;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class WiredSaver
    {
        private static List<RoomItem> GetItems(ClientMessage message, Room room, out int itemCount)
        {
            List<RoomItem> list = new List<RoomItem>();
            itemCount = message.PopWiredInt32();
            for (int i = 0; i < itemCount; i++)
            {
                uint pId = message.PopWiredUInt();
                RoomItem item = room.GetRoomItemHandler().GetItem(pId);
                if (!((item == null) || WiredUtillity.TypeIsWired(item.GetBaseItem().InteractionType)))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        internal static void HandleConditionSave(uint itemID, Room room, ClientMessage clientMessage, GameClient Session)
        {
            IWiredCondition condition;
            RoomItem item = room.GetRoomItemHandler().GetItem(itemID);
            if (item != null)
            {
                if (item.wiredHandler != null)
                {
                    item.wiredHandler.Dispose();
                    item.wiredHandler = null;
                }
                InteractionType interactionType = item.GetBaseItem().InteractionType;
                switch (interactionType)
                {
                    case InteractionType.conditionfurnishaveusers:
                    case InteractionType.conditionstatepos:
                    case InteractionType.conditiontimelessthan:
                    case InteractionType.conditiontimemorethan:
                    case InteractionType.conditiontriggeronfurni:
                    {
                        int num4;
                        clientMessage.PopWiredInt32();
                        int num = clientMessage.PopWiredInt32();
                        int num2 = clientMessage.PopWiredInt32();
                        int num3 = clientMessage.PopWiredInt32();
                        clientMessage.PopFixedString();
                        List<RoomItem> items = GetItems(clientMessage, room, out num4);
                        condition = null;
                        switch (interactionType)
                        {
                            case InteractionType.conditionfurnishaveusers:
                                condition = new FurniHasUser(item, items);
                                goto Label_0118;

                            case InteractionType.conditionstatepos:
                                condition = new FurniStatePosMatch(item, items);
                                goto Label_0118;

                            case InteractionType.conditiontimelessthan:
                                condition = new LessThanTimer(500, room, item);
                                goto Label_0118;

                            case InteractionType.conditiontimemorethan:
                                condition = new MoreThanTimer(500, room, item);
                                goto Label_0118;

                            case InteractionType.conditiontriggeronfurni:
                                condition = new TriggerUserIsOnFurni(item, items);
                                goto Label_0118;
                        }
                        break;
                    }
                }
            }
            return;
        Label_0118:
            item.wiredCondition = condition;
            room.GetWiredHandler().conditionHandler.AddOrIgnoreRefferance(item);
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                condition.SaveToDatabase(adapter);
            }
            Session.SendMessage(new ServerMessage(Outgoing.SaveWired));
        }

        internal static void HandleDefaultSave(uint itemID, Room room)
        {
            RoomItem item = room.GetRoomItemHandler().GetItem(itemID);
            if (item != null)
            {
                IWiredTrigger trigger;
                List<RoomItem> list;
                int num3;
                IWiredTrigger trigger2;
                switch (item.GetBaseItem().InteractionType)
                {
                    case InteractionType.triggertimer:
                    {
                        int cycleCount = 0;
                        trigger2 = new Timer(item, room.GetWiredHandler(), cycleCount, room.GetGameManager());
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.triggerroomenter:
                    {
                        string userName = string.Empty;
                        trigger2 = new EntersRoom(item, room.GetWiredHandler(), room.GetRoomUserManager(), !string.IsNullOrEmpty(userName), userName);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.triggergameend:
                        trigger2 = new GameEnds(item, room.GetWiredHandler(), room.GetGameManager());
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.triggergamestart:
                        trigger2 = new GameStarts(item, room.GetWiredHandler(), room.GetGameManager());
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.triggerrepeater:
                    {
                        int cyclesRequired = 0;
                        trigger2 = new Repeater(room.GetWiredHandler(), item, cyclesRequired);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.triggeronusersay:
                    {
                        bool flag = false;
                        string triggerMessage = string.Empty;
                        trigger2 = new UserSays(item, room.GetWiredHandler(), !flag, triggerMessage, room);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.triggerscoreachieved:
                    {
                        int scoreLevel = 0;
                        trigger2 = new ScoreAchieved(item, room.GetWiredHandler(), scoreLevel, room.GetGameManager());
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.triggerstatechanged:
                        list = new List<RoomItem>();
                        num3 = 0;
                        trigger2 = new SateChanged(room.GetWiredHandler(), item, list, num3);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.triggerwalkonfurni:
                        list = new List<RoomItem>();
                        num3 = 0;
                        trigger2 = new WalksOnFurni(item, room.GetWiredHandler(), list, num3);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.triggerwalkofffurni:
                        list = new List<RoomItem>();
                        num3 = 0;
                        trigger2 = new WalksOffFurni(item, room.GetWiredHandler(), list, num3);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.actiongivescore:
                    {
                        int scoreToGive = 0;
                        int maxCountPerGame = 0;
                        trigger = new GiveScore(maxCountPerGame, scoreToGive, room.GetGameManager(), itemID);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.actionposreset:
                        list = new List<RoomItem>();
                        num3 = 0;
                        trigger = new PositionReset(list, num3, room.GetRoomItemHandler(), room.GetWiredHandler(), itemID);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.actionmoverotate:
                    {
                        MovementState none = MovementState.none;
                        RotationState rotation = RotationState.none;
                        list = new List<RoomItem>();
                        num3 = 0;
                        trigger = new MoveRotate(none, rotation, list, num3, room, room.GetWiredHandler(), itemID);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.actionresettimer:
                        list = new List<RoomItem>();
                        num3 = 0;
                        trigger = new TimerReset(room, room.GetWiredHandler(), list, num3, itemID);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.actionshowmessage:
                        trigger = new ShowMessage(string.Empty, room.GetWiredHandler(), itemID);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.actionteleportto:
                        list = new List<RoomItem>();
                        num3 = 0;
                        trigger = new TeleportToItem(room.GetGameMap(), room.GetWiredHandler(), list, num3, itemID);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.actiontogglestate:
                        list = new List<RoomItem>();
                        num3 = 0;
                        trigger = new ToggleItemState(room.GetGameMap(), room.GetWiredHandler(), list, num3, item);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;
                }
            }
        }

        internal static void HandleSave(GameClient Session, uint itemID, Room room, ClientMessage clientMessage)
        {
            RoomItem item = room.GetRoomItemHandler().GetItem(itemID);
            if (item != null)
            {
                int num;
                string str;
                IWiredTrigger trigger;
                int num2;
                List<RoomItem> list;
                int num3;
                IWiredTrigger trigger2;
                bool flag2;
                bool flag3;
                if (item.wiredHandler != null)
                {
                    item.wiredHandler.Dispose();
                    item.wiredHandler = null;
                }
                switch (item.GetBaseItem().InteractionType)
                {
                    case InteractionType.triggertimer:
                    {
                        num = clientMessage.PopWiredInt32();
                        int cycleCount = clientMessage.PopWiredInt32();
                        trigger = new Timer(item, room.GetWiredHandler(), cycleCount, room.GetGameManager());
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.triggerroomenter:
                    {
                        num = clientMessage.PopWiredInt32();
                        string userName = clientMessage.PopFixedString();
                        trigger = new EntersRoom(item, room.GetWiredHandler(), room.GetRoomUserManager(), !string.IsNullOrEmpty(userName), userName);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.triggergameend:
                        trigger = new GameEnds(item, room.GetWiredHandler(), room.GetGameManager());
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.triggergamestart:
                        trigger = new GameStarts(item, room.GetWiredHandler(), room.GetGameManager());
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.triggerrepeater:
                    {
                        num = clientMessage.PopWiredInt32();
                        int cyclesRequired = clientMessage.PopWiredInt32();
                        trigger = new Repeater(room.GetWiredHandler(), item, cyclesRequired);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.triggeronusersay:
                    {
                        num = clientMessage.PopWiredInt32();
                        bool isOwnerOnly = clientMessage.PopWiredInt32() == 1;
                        str = clientMessage.PopFixedString();
                        trigger = new UserSays(item, room.GetWiredHandler(), isOwnerOnly, str, room);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.triggerscoreachieved:
                    {
                        num = clientMessage.PopWiredInt32();
                        int scoreLevel = clientMessage.PopWiredInt32();
                        trigger = new ScoreAchieved(item, room.GetWiredHandler(), scoreLevel, room.GetGameManager());
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.triggerstatechanged:
                        int num9;
                        num = clientMessage.PopWiredInt32();
                        flag2 = clientMessage.PopWiredBoolean();
                        flag3 = clientMessage.PopWiredBoolean();
                        list = GetItems(clientMessage, room, out num9);
                        num3 = clientMessage.PopWiredInt32();
                        trigger = new SateChanged(room.GetWiredHandler(), item, list, num3);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.triggerwalkonfurni:
                        num = clientMessage.PopWiredInt32();
                        str = clientMessage.PopFixedString();
                        list = GetItems(clientMessage, room, out num2);
                        num3 = clientMessage.PopWiredInt32();
                        trigger = new WalksOnFurni(item, room.GetWiredHandler(), list, num3);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.triggerwalkofffurni:
                        num = clientMessage.PopWiredInt32();
                        str = clientMessage.PopFixedString();
                        list = GetItems(clientMessage, room, out num2);
                        num3 = clientMessage.PopWiredInt32();
                        trigger = new WalksOnFurni(item, room.GetWiredHandler(), list, num3);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.actiongivescore:
                    {
                        num = clientMessage.PopWiredInt32();
                        int scoreToGive = clientMessage.PopWiredInt32();
                        trigger2 = new GiveScore(clientMessage.PopWiredInt32(), scoreToGive, room.GetGameManager(), itemID);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.actionposreset:
                        num = clientMessage.PopWiredInt32();
                        flag2 = clientMessage.PopWiredBoolean();
                        flag3 = clientMessage.PopWiredBoolean();
                        list = GetItems(clientMessage, room, out num2);
                        num3 = clientMessage.PopWiredInt32();
                        trigger2 = new PositionReset(list, num3, room.GetRoomItemHandler(), room.GetWiredHandler(), itemID);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.actionmoverotate:
                    {
                        num = clientMessage.PopWiredInt32();
                        MovementState movement = (MovementState) clientMessage.PopWiredInt32();
                        RotationState rotation = (RotationState) clientMessage.PopWiredInt32();
                        flag2 = clientMessage.PopWiredBoolean();
                        flag3 = clientMessage.PopWiredBoolean();
                        list = GetItems(clientMessage, room, out num2);
                        num3 = clientMessage.PopWiredInt32();
                        trigger = new MoveRotate(movement, rotation, list, num3, room, room.GetWiredHandler(), itemID);
                        HandleTriggerSave(trigger, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.actionresettimer:
                        num = clientMessage.PopWiredInt32();
                        flag2 = clientMessage.PopWiredBoolean();
                        flag3 = clientMessage.PopWiredBoolean();
                        list = GetItems(clientMessage, room, out num2);
                        num3 = clientMessage.PopWiredInt32();
                        trigger2 = new TimerReset(room, room.GetWiredHandler(), list, num3, itemID);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.actionshowmessage:
                        num = clientMessage.PopWiredInt32();
                        trigger2 = new ShowMessage(clientMessage.PopFixedString(), room.GetWiredHandler(), itemID);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;

                    case InteractionType.actionteleportto:
                    {
                        num = clientMessage.PopWiredInt32();
                        string str2 = clientMessage.PopFixedString();
                        list = GetItems(clientMessage, room, out num2);
                        num3 = clientMessage.PopWiredInt32();
                        trigger2 = new TeleportToItem(room.GetGameMap(), room.GetWiredHandler(), list, num3, itemID);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;
                    }
                    case InteractionType.actiontogglestate:
                        num = clientMessage.PopWiredInt32();
                        str = clientMessage.PopFixedString();
                        list = GetItems(clientMessage, room, out num2);
                        num3 = clientMessage.PopWiredInt32();
                        trigger2 = new ToggleItemState(room.GetGameMap(), room.GetWiredHandler(), list, num3, item);
                        HandleTriggerSave(trigger2, room.GetWiredHandler(), room, itemID);
                        break;
                }
                Session.SendMessage(new ServerMessage(Outgoing.SaveWired));
            }
        }

        private static void HandleTriggerSave(IWiredTrigger handler, WiredHandler manager, Room room, uint itemID)
        {
            RoomItem item = room.GetRoomItemHandler().GetItem(itemID);
            if (item != null)
            {
                item.wiredHandler = handler;
                manager.RemoveFurniture(item);
                manager.AddFurniture(item);
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    handler.SaveToDatabase(adapter);
                }
            }
        }
    }
}

