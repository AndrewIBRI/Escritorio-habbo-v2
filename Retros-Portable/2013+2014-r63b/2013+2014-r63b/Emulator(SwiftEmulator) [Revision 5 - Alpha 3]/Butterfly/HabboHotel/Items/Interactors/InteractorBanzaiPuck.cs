namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using System;
    using System.Drawing;

    internal class InteractorBanzaiPuck : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (Session != null)
            {
                int num3;
                int num4;
                RoomUser roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                Point coordinate = roomUserByHabbo.Coordinate;
                Point c = Item.Coordinate;
                int num = coordinate.X - c.X;
                int num2 = coordinate.Y - c.Y;
                if ((((num <= 1) && (num >= -1)) && (num2 <= 1)) && (num2 >= -1))
                {
                    num *= 2;
                    num2 *= 2;
                    num3 = Item.GetX + num;
                    num4 = Item.GetY + num2;
                    Item.GetRoom().GetSoccer().MoveBall(Item, Session, num3, num4);
                    roomUserByHabbo.MoveTo(c);
                }
                else
                {
                    Item.interactingBallUser = Session.GetHabbo().Id;
                    num *= -1;
                    num2 *= -1;
                    if (num > 1)
                    {
                        num = 1;
                    }
                    else if (num < -1)
                    {
                        num = -1;
                    }
                    if (num2 > 1)
                    {
                        num2 = 1;
                    }
                    else if (num2 < -1)
                    {
                        num2 = -1;
                    }
                    num3 = Item.GetX + num;
                    num4 = Item.GetY + num2;
                    roomUserByHabbo.MoveTo(new Point(num3, num4));
                }
            }
        }
    }
}

