namespace Butterfly.HabboHotel.Rooms.Wired
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Conditions;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;

    internal class WiredLoader
    {
        private static void HandleConditionLoad(IWiredCondition handler, RoomItem item)
        {
            if (item.wiredCondition != null)
            {
                item.wiredCondition.Dispose();
            }
            item.wiredCondition = handler;
        }

        private static void HandleItemLoad(IWiredTrigger handler, WiredHandler wiredHandler, RoomItem item)
        {
            if (item.wiredHandler != null)
            {
                item.wiredHandler.Dispose();
            }
            item.wiredHandler = handler;
        }

        internal static void LoadWiredItem(RoomItem item, Room room, IQueryAdapter dbClient)
        {
            IWiredTrigger trigger;
            IWiredTrigger trigger2;
            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.triggertimer:
                    trigger2 = new Timer(item, room.GetWiredHandler(), 0, room.GetGameManager());
                    trigger2.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger2, room.GetWiredHandler(), item);
                    break;

                case InteractionType.triggerroomenter:
                    trigger2 = new EntersRoom(item, room.GetWiredHandler(), room.GetRoomUserManager(), false, string.Empty);
                    trigger2.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger2, room.GetWiredHandler(), item);
                    break;

                case InteractionType.triggergameend:
                    trigger2 = new GameEnds(item, room.GetWiredHandler(), room.GetGameManager());
                    HandleItemLoad(trigger2, room.GetWiredHandler(), item);
                    break;

                case InteractionType.triggergamestart:
                    trigger2 = new GameStarts(item, room.GetWiredHandler(), room.GetGameManager());
                    HandleItemLoad(trigger2, room.GetWiredHandler(), item);
                    break;

                case InteractionType.triggerrepeater:
                    trigger2 = new Repeater(room.GetWiredHandler(), item, 0);
                    trigger2.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger2, room.GetWiredHandler(), item);
                    break;

                case InteractionType.triggeronusersay:
                    trigger2 = new UserSays(item, room.GetWiredHandler(), false, string.Empty, room);
                    trigger2.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger2, room.GetWiredHandler(), item);
                    break;

                case InteractionType.triggerscoreachieved:
                    trigger2 = new ScoreAchieved(item, room.GetWiredHandler(), 0, room.GetGameManager());
                    trigger2.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger2, room.GetWiredHandler(), item);
                    break;

                case InteractionType.triggerstatechanged:
                    trigger2 = new SateChanged(room.GetWiredHandler(), item, new List<RoomItem>(), 0);
                    trigger2.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger2, room.GetWiredHandler(), item);
                    break;

                case InteractionType.triggerwalkonfurni:
                    trigger2 = new WalksOnFurni(item, room.GetWiredHandler(), new List<RoomItem>(), 0);
                    trigger2.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger2, room.GetWiredHandler(), item);
                    break;

                case InteractionType.triggerwalkofffurni:
                    trigger2 = new WalksOffFurni(item, room.GetWiredHandler(), new List<RoomItem>(), 0);
                    trigger2.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger2, room.GetWiredHandler(), item);
                    break;

                case InteractionType.actiongivescore:
                    trigger = new GiveScore(0, 0, room.GetGameManager(), item.Id);
                    trigger.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger, room.GetWiredHandler(), item);
                    break;

                case InteractionType.actionposreset:
                    trigger = new PositionReset(new List<RoomItem>(), 0, room.GetRoomItemHandler(), room.GetWiredHandler(), item.Id);
                    trigger.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger, room.GetWiredHandler(), item);
                    break;

                case InteractionType.actionmoverotate:
                    trigger = new MoveRotate(MovementState.none, RotationState.none, new List<RoomItem>(), 0, room, room.GetWiredHandler(), item.Id);
                    trigger.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger, room.GetWiredHandler(), item);
                    break;

                case InteractionType.actionresettimer:
                    trigger = new TimerReset(room, room.GetWiredHandler(), new List<RoomItem>(), 0, item.Id);
                    trigger.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger, room.GetWiredHandler(), item);
                    break;

                case InteractionType.actionshowmessage:
                    trigger = new ShowMessage(string.Empty, room.GetWiredHandler(), item.Id);
                    trigger.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger, room.GetWiredHandler(), item);
                    break;

                case InteractionType.actionteleportto:
                    trigger = new TeleportToItem(room.GetGameMap(), room.GetWiredHandler(), new List<RoomItem>(), 0, item.Id);
                    trigger.LoadFromDatabase(dbClient, room);
                    HandleItemLoad(trigger, room.GetWiredHandler(), item);
                    break;

                case InteractionType.actiontogglestate:
                    trigger = new ToggleItemState(room.GetGameMap(), room.GetWiredHandler(), new List<RoomItem>(), 0, item);
                    trigger.LoadFromDatabase(dbClient, room);
                    item.wiredHandler = trigger;
                    HandleItemLoad(trigger, room.GetWiredHandler(), item);
                    break;

                case InteractionType.conditionfurnishaveusers:
                {
                    IWiredCondition handler = new FurniHasUser(item, new List<RoomItem>());
                    handler.LoadFromDatabase(dbClient, room);
                    HandleConditionLoad(handler, item);
                    break;
                }
                case InteractionType.conditionstatepos:
                {
                    IWiredCondition condition2 = new FurniStatePosMatch(item, new List<RoomItem>());
                    condition2.LoadFromDatabase(dbClient, room);
                    HandleConditionLoad(condition2, item);
                    break;
                }
                case InteractionType.conditiontimelessthan:
                {
                    IWiredCondition condition3 = new LessThanTimer(0, room, item);
                    condition3.LoadFromDatabase(dbClient, room);
                    HandleConditionLoad(condition3, item);
                    break;
                }
                case InteractionType.conditiontimemorethan:
                {
                    IWiredCondition condition4 = new MoreThanTimer(0, room, item);
                    condition4.LoadFromDatabase(dbClient, room);
                    HandleConditionLoad(condition4, item);
                    break;
                }
                case InteractionType.conditiontriggeronfurni:
                {
                    IWiredCondition condition5 = new TriggerUserIsOnFurni(item, new List<RoomItem>());
                    condition5.LoadFromDatabase(dbClient, room);
                    HandleConditionLoad(condition5, item);
                    break;
                }
            }
        }
    }
}

