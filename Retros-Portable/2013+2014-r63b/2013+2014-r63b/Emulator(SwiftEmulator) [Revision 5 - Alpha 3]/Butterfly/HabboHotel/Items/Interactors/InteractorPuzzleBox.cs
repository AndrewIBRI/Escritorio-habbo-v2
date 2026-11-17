namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.Messages;
    using HabboEvents;
    using System;
    using System.Drawing;

    internal class InteractorPuzzleBox : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (Session == null)
            {
                RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                Point point = new Point(Item.Coordinate.X + 1, Item.Coordinate.Y);
                Point point2 = new Point(Item.Coordinate.X - 1, Item.Coordinate.Y);
                Point point3 = new Point(Item.Coordinate.X, Item.Coordinate.Y + 1);
                Point point4 = new Point(Item.Coordinate.X, Item.Coordinate.Y - 1);
                if (roomUserByHabbo != null)
                {
                    if ((((roomUserByHabbo.Coordinate != point) && (roomUserByHabbo.Coordinate != point2)) && (roomUserByHabbo.Coordinate != point3)) && (roomUserByHabbo.Coordinate != point4))
                    {
                        if (roomUserByHabbo.CanWalk)
                        {
                            roomUserByHabbo.MoveTo(Item.SquareInFront);
                        }
                    }
                    else
                    {
                        int x = Item.Coordinate.X;
                        int y = Item.Coordinate.Y;
                        if (roomUserByHabbo.Coordinate == point)
                        {
                            x = Item.Coordinate.X - 1;
                            y = Item.Coordinate.Y;
                        }
                        else if (roomUserByHabbo.Coordinate == point2)
                        {
                            x = Item.Coordinate.X + 1;
                            y = Item.Coordinate.Y;
                        }
                        else if (roomUserByHabbo.Coordinate == point3)
                        {
                            x = Item.Coordinate.X;
                            y = Item.Coordinate.Y - 1;
                        }
                        else if (roomUserByHabbo.Coordinate == point4)
                        {
                            x = Item.Coordinate.X;
                            y = Item.Coordinate.Y + 1;
                        }
                        if (Item.GetRoom().GetGameMap().ValidTile(x, y))
                        {
                            double k = Item.GetRoom().GetGameMap().SqAbsoluteHeight(x, y);
                            ServerMessage message = new ServerMessage(Outgoing.ObjectOnRoller);
                            message.AppendInt32(Item.Coordinate.X);
                            message.AppendInt32(Item.Coordinate.Y);
                            message.AppendInt32(x);
                            message.AppendInt32(y);
                            message.AppendInt32(1);
                            message.AppendInt32(Item.Id);
                            message.AppendString(TextHandling.GetString(k));
                            message.AppendString("M");
                            Item.GetRoom().SendMessage(message);
                            Item.GetRoom().GetRoomItemHandler().SetFloorItem(roomUserByHabbo.GetClient(), Item, x, y, Item.Rot, false, false, true);
                        }
                    }
                }
            }
        }
    }
}

