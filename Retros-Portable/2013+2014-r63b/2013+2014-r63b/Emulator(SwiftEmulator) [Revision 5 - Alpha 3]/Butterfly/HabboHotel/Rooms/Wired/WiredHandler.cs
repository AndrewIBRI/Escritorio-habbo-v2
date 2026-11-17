namespace Butterfly.HabboHotel.Rooms.Wired
{
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Rooms.Games;
    using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
    using Butterfly.Messages;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using WiredSolver;

    public class WiredHandler
    {
        private Hashtable actionItems = new Hashtable();
        private Hashtable actionStacks = new Hashtable();
        internal ConditionHandler conditionHandler;
        private bool doCleanup = false;
        private Queue requestedTriggers = new Queue();
        private Queue requestingUpdates = new Queue();
        private Room room;
        private WiredSolverInstance wireSlower = new WiredSolverInstance();

        public WiredHandler(Room room)
        {
            this.room = room;
            this.conditionHandler = new ConditionHandler(room);
        }

        internal void AddFurniture(RoomItem item)
        {
            this.AddFurnitureToItems(item);
            this.AddFurnitureToItemStack(item);
        }

        private void AddFurnitureToItems(RoomItem item)
        {
            InteractionType interactionType = item.GetBaseItem().InteractionType;
            if (WiredUtillity.TypeIsWired(interactionType))
            {
                if (this.actionItems.ContainsKey(interactionType))
                {
                    ((List<RoomItem>) this.actionItems[interactionType]).Add(item);
                }
                else
                {
                    List<RoomItem> list = new List<RoomItem> {
                        item
                    };
                    this.actionItems.Add(interactionType, list);
                }
            }
        }

        private void AddFurnitureToItemStack(RoomItem item)
        {
            Point coordinate = item.Coordinate;
            if (this.actionStacks.ContainsKey(coordinate))
            {
                ((List<RoomItem>) this.actionStacks[coordinate]).Add(item);
            }
            else
            {
                List<RoomItem> list = new List<RoomItem> {
                    item
                };
                this.actionStacks.Add(coordinate, list);
            }
        }

        internal void AddWire(RoomItem item, Point location, int rotation, InteractionType wireType)
        {
            WireCurrentTransfer nONE = WireCurrentTransfer.NONE;
            switch (wireType)
            {
                case InteractionType.wireCenter:
                    nONE = WireCurrentTransfer.LEFT | WireCurrentTransfer.DOWN | WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
                    goto Label_00D7;

                case InteractionType.wireCorner:
                    switch (rotation)
                    {
                        case 2:
                            nONE = WireCurrentTransfer.LEFT | WireCurrentTransfer.DOWN;
                            goto Label_00D7;

                        case 4:
                            nONE = WireCurrentTransfer.LEFT | WireCurrentTransfer.UP;
                            goto Label_00D7;

                        case 6:
                            nONE = WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
                            goto Label_00D7;
                    }
                    break;

                case InteractionType.wireSplitter:
                    switch (rotation)
                    {
                        case 2:
                            nONE = WireCurrentTransfer.LEFT | WireCurrentTransfer.DOWN | WireCurrentTransfer.RIGHT;
                            goto Label_00D7;

                        case 4:
                            nONE = WireCurrentTransfer.LEFT | WireCurrentTransfer.DOWN | WireCurrentTransfer.UP;
                            goto Label_00D7;

                        case 6:
                            nONE = WireCurrentTransfer.LEFT | WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
                            goto Label_00D7;
                    }
                    nONE = WireCurrentTransfer.DOWN | WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
                    goto Label_00D7;

                case InteractionType.wireStandard:
                    switch (rotation)
                    {
                        case 2:
                        case 6:
                            nONE = WireCurrentTransfer.LEFT | WireCurrentTransfer.RIGHT;
                            goto Label_00D7;
                    }
                    nONE = WireCurrentTransfer.DOWN | WireCurrentTransfer.UP;
                    goto Label_00D7;

                default:
                    goto Label_00D7;
            }
            nONE = WireCurrentTransfer.DOWN | WireCurrentTransfer.RIGHT;
        Label_00D7:
            if (nONE != WireCurrentTransfer.NONE)
            {
                List<Point> coords = this.wireSlower.AddOrUpdateWire(location.X, location.Y, CurrentType.OFF, nONE);
                this.HandleItems(coords);
            }
        }

        private void CheckHandlingState(ref bool shouldBeHandled, Point coordinate)
        {
            foreach (RoomItem item in this.room.GetGameMap().GetCoordinatedItems(coordinate))
            {
                if (TypeIsWire(item.GetBaseItem().InteractionType))
                {
                    shouldBeHandled = true;
                }
            }
        }

        internal void Destroy()
        {
            if (this.actionItems != null)
            {
                this.actionItems.Clear();
            }
            this.actionItems = null;
            if (this.actionStacks != null)
            {
                this.actionStacks.Clear();
            }
            this.requestedTriggers.Clear();
            this.requestingUpdates.Clear();
            if (this.wireSlower != null)
            {
                this.wireSlower.Destroy();
            }
            this.wireSlower = null;
            this.room = null;
        }

        internal Room GetRoom()
        {
            return this.room;
        }

        private void HandleItems(List<Point> coords)
        {
            foreach (Point point in coords)
            {
                this.HandleItems(point);
            }
        }

        private void HandleItems(Point coord)
        {
            WireTransfer transfer = this.wireSlower.getWireTransfer(coord);
            List<RoomItem> list = new List<RoomItem>(this.room.GetGameMap().GetCoordinatedItems(coord));
            foreach (RoomItem item in list)
            {
                item.Interactor.OnTrigger(null, item, 0, true);
            }
            list.Clear();
            list = null;
        }

        private void HandleItems(List<Point> coords, Point startingPoint)
        {
            foreach (Point point in coords)
            {
                if (point != startingPoint)
                {
                    this.HandleItems(point);
                }
            }
        }

        internal void LoadWired()
        {
        }

        internal void OnCycle()
        {
            if (this.doCleanup)
            {
                foreach (List<RoomItem> list in this.actionStacks.Values)
                {
                    foreach (RoomItem item in list)
                    {
                        if (item.wiredCondition != null)
                        {
                            item.wiredCondition.Dispose();
                            item.wiredCondition = null;
                        }
                        if (item.wiredHandler != null)
                        {
                            item.wiredHandler.Dispose();
                            item.wiredHandler = null;
                        }
                    }
                }
                this.actionStacks.Clear();
                this.actionItems.Clear();
                this.requestedTriggers.Clear();
                this.requestingUpdates.Clear();
                this.doCleanup = false;
            }
            else
            {
                if (this.requestingUpdates.Count > 0)
                {
                    List<IWiredCycleable> list2 = new List<IWiredCycleable>();
                    lock (this.requestingUpdates.SyncRoot)
                    {
                        while (this.requestingUpdates.Count > 0)
                        {
                            IWiredCycleable cycleable = (IWiredCycleable) this.requestingUpdates.Dequeue();
                            if (!cycleable.Disposed() && cycleable.OnCycle())
                            {
                                list2.Add(cycleable);
                            }
                        }
                        foreach (IWiredCycleable cycleable2 in list2)
                        {
                            this.requestingUpdates.Enqueue(cycleable2);
                        }
                    }
                }
                this.conditionHandler.OnCycle();
            }
        }

        internal void OnEvent(uint itemID)
        {
            ServerMessage message = new ServerMessage(0x58);
            message.AppendString(itemID.ToString());
            message.AppendString("0");
            this.room.SendMessage(message);
        }

        internal void OnPickall()
        {
            this.doCleanup = true;
        }

        internal void RemoveFurniture(RoomItem item)
        {
            this.RemoveFurnitureFromItems(item);
            this.RemoveFurnitureFromStack(item);
        }

        private void RemoveFurnitureFromItems(RoomItem item)
        {
            InteractionType interactionType = item.GetBaseItem().InteractionType;
            if (this.actionItems.ContainsKey(interactionType))
            {
                ((List<RoomItem>) this.actionItems[interactionType]).Remove(item);
            }
        }

        private void RemoveFurnitureFromStack(RoomItem item)
        {
            Point coordinate = item.Coordinate;
            if (this.actionStacks.ContainsKey(coordinate))
            {
                ((List<RoomItem>) this.actionStacks[coordinate]).Remove(item);
            }
        }

        internal void RemoveWiredItem(Point coordinate)
        {
            this.wireSlower.RemoveWire(coordinate.X, coordinate.Y);
        }

        internal void RequestCycle(IWiredCycleable handler)
        {
            lock (this.requestingUpdates.SyncRoot)
            {
                this.requestingUpdates.Enqueue(handler);
            }
        }

        internal void RequestStackHandle(Point coordinate, RoomItem item, RoomUser user, Team team)
        {
            List<RoomItem> list = null;
            if (this.actionStacks.ContainsKey(coordinate) && this.conditionHandler.AllowsHandling(coordinate, user))
            {
                list = (List<RoomItem>) this.actionStacks[coordinate];
                foreach (RoomItem item2 in list)
                {
                    if (((user != null) || !(item2.GetBaseItem().Name == "wf_act_teleport_to")) && (item2.wiredHandler is IWiredEffect))
                    {
                        ((IWiredEffect) item2.wiredHandler).Handle(user, team, item);
                    }
                }
                bool shouldBeHandled = false;
                this.CheckHandlingState(ref shouldBeHandled, new Point(coordinate.X, coordinate.Y + 1));
                this.CheckHandlingState(ref shouldBeHandled, new Point(coordinate.X + 1, coordinate.Y));
                this.CheckHandlingState(ref shouldBeHandled, new Point(coordinate.X, coordinate.Y - 1));
                this.CheckHandlingState(ref shouldBeHandled, new Point(coordinate.X - 1, coordinate.Y));
                if (shouldBeHandled)
                {
                    this.room.GetWiredHandler().TriggerOnWire(coordinate);
                }
            }
        }

        internal void TriggerOnWire(Point location)
        {
            List<Point> list;
            WireTransfer transfer = this.wireSlower.getWireTransfer(location);
            WireCurrentTransfer transfer2 = WireCurrentTransfer.LEFT | WireCurrentTransfer.DOWN | WireCurrentTransfer.RIGHT | WireCurrentTransfer.UP;
            if (transfer.isPowered() && (transfer.getTransfer() == transfer2))
            {
                list = this.wireSlower.RemoveWire(location.X, location.Y);
            }
            else
            {
                list = this.wireSlower.AddOrUpdateWire(location.X, location.Y, CurrentType.SENDER, WireCurrentTransfer.NONE);
            }
            this.HandleItems(list, location);
        }

        internal static bool TypeIsWire(InteractionType type)
        {
            switch (type)
            {
                case InteractionType.wireCenter:
                case InteractionType.wireCorner:
                case InteractionType.wireSplitter:
                case InteractionType.wireStandard:
                    return true;
            }
            return false;
        }
    }
}

