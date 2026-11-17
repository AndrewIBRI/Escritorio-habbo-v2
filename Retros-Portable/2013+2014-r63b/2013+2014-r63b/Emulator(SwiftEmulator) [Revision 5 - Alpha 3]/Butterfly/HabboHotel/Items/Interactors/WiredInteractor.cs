namespace Butterfly.HabboHotel.Items.Interactors
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

    internal class WiredInteractor : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (((Session != null) && (Item != null)) && UserHasRights)
            {
                ServerMessage message;
                string s = "";
                List<RoomItem> list = new List<RoomItem>();
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    try
                    {
                        adapter.setQuery("SELECT trigger_data FROM trigger_item WHERE trigger_id = @id ");
                        adapter.addParameter("id", (int) Item.Id);
                        s = adapter.getString();
                    }
                    catch
                    {
                    }
                    try
                    {
                        adapter.setQuery("SELECT triggers_item FROM trigger_in_place WHERE original_trigger = @id");
                        adapter.addParameter("id", (int) Item.Id);
                        DataTable table = adapter.getTable();
                        foreach (DataRow row in table.Rows)
                        {
                            RoomItem item = Item.GetRoom().GetRoomItemHandler().GetItem(Convert.ToUInt32(row[0]));
                            if ((item != null) && !list.Contains(item))
                            {
                                list.Add(item);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                switch (Item.GetBaseItem().InteractionType)
                {
                    case InteractionType.triggertimer:
                        message = new ServerMessage(Outgoing.WiredTrigger);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(1);
                        message.AppendInt32(1);
                        message.AppendInt32(1);
                        message.AppendInt32(3);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.triggerroomenter:
                        message = new ServerMessage(Outgoing.WiredTrigger);
                        message.AppendBoolean(false);
                        message.AppendInt32(0);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(7);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.triggergameend:
                        message = new ServerMessage(Outgoing.WiredTrigger);
                        message.AppendBoolean(false);
                        message.AppendInt32(0);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(8);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.triggergamestart:
                        message = new ServerMessage(Outgoing.WiredTrigger);
                        message.AppendBoolean(false);
                        message.AppendInt32(0);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(8);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.triggerrepeater:
                        message = new ServerMessage(Outgoing.WiredTrigger);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(1);
                        message.AppendInt32(10);
                        message.AppendInt32(0);
                        message.AppendInt32(6);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.triggeronusersay:
                        message = new ServerMessage(Outgoing.WiredTrigger);
                        message.AppendBoolean(false);
                        message.AppendInt32(0);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.triggerscoreachieved:
                        message = new ServerMessage(Outgoing.WiredTrigger);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(1);
                        message.AppendInt32(100);
                        message.AppendInt32(0);
                        message.AppendInt32(10);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.triggerstatechanged:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(8);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.triggerwalkonfurni:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(8);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.triggerwalkofffurni:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(8);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.actiongivescore:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(2);
                        message.AppendInt32(5);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(6);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.actionposreset:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.actionmoverotate:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(2);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(4);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.actionresettimer:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.actionshowmessage:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(0);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(7);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.actionteleportto:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(8);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.actiontogglestate:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(8);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.conditionfurnishaveusers:
                        message = new ServerMessage(Outgoing.WiredCondition);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(1);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.conditionstatepos:
                        message = new ServerMessage(Outgoing.WiredCondition);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(1);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.conditiontimelessthan:
                    case InteractionType.conditiontimemorethan:
                        message = new ServerMessage(Outgoing.WiredCondition);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.conditiontriggeronfurni:
                        message = new ServerMessage(Outgoing.WiredCondition);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.specialrandom:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(8);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;

                    case InteractionType.specialunseen:
                        message = new ServerMessage(Outgoing.WiredEffect);
                        message.AppendBoolean(false);
                        message.AppendInt32(5);
                        message.AppendInt32(list.Count);
                        foreach (RoomItem item2 in list)
                        {
                            message.AppendInt32(item2.Id);
                        }
                        message.AppendInt32(Item.GetBaseItem().SpriteId);
                        message.AppendInt32(Item.Id);
                        message.AppendString(s);
                        message.AppendInt32(0);
                        message.AppendInt32(8);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        message.AppendInt32(0);
                        Session.SendMessage(message);
                        break;
                }
            }
        }
    }
}

