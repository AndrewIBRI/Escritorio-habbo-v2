namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using Messages;
    using HabboEvents;
    using System;

    class InteractorMannequin : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {

        }


        internal override void OnRemove(GameClient Session, RoomItem Item)
        {


        }

        internal string GetHair(string _Figure)
        {

            string FigurePartHair = _Figure;
            string GetHairPart;

            GetHairPart = System.Text.RegularExpressions.Regex.Split(_Figure, "hr")[1];
            FigurePartHair = GetHairPart.Split('.')[0];
            string FigurePartBody = _Figure;
            string GetBodyPart;

            GetBodyPart = System.Text.RegularExpressions.Regex.Split(_Figure, "hd")[1];
            FigurePartBody = GetBodyPart.Split('.')[0];

            string _Uni = Convert.ToString("hr" + FigurePartHair + "." + "hd" + FigurePartBody + ".");

            return _Uni;
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (Item.ExtraData != "")
            {
                Session.GetHabbo().Gender = Item.ExtraData.Split(Convert.ToChar(9))[1];
                Session.GetHabbo().Look = Item.ExtraData.Split(Convert.ToChar(9))[2];
                Session.GetMessageHandler().GetResponse().Init(Outgoing.UpdateUserInformation);
                Session.GetMessageHandler().GetResponse().AppendInt32(-1);
                Session.GetMessageHandler().GetResponse().AppendString(Session.GetHabbo().Look);
                Session.GetMessageHandler().GetResponse().AppendString(Session.GetHabbo().Gender.ToLower());
                Session.GetMessageHandler().GetResponse().AppendString(Session.GetHabbo().Motto);
                Session.GetMessageHandler().GetResponse().AppendInt32(Session.GetHabbo().AchievementPoints);
                Session.GetMessageHandler().SendResponse();


                if (Session.GetHabbo().InRoom)
                {
                    Room Room = Session.GetHabbo().CurrentRoom;


                    if (Room == null)
                    {
                        return;
                    }


                    RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);


                    if (User == null)
                    {
                        return;
                    }


                    ServerMessage RoomUpdate = new ServerMessage(Outgoing.UpdateUserInformation);
                    RoomUpdate.AppendInt32(User.VirtualId);
                    RoomUpdate.AppendString(Session.GetHabbo().Look);
                    RoomUpdate.AppendString(Session.GetHabbo().Gender.ToLower());
                    RoomUpdate.AppendString(Session.GetHabbo().Motto);
                    RoomUpdate.AppendInt32(Session.GetHabbo().AchievementPoints);
                    Room.SendMessage(RoomUpdate);
                }
            }
        }
    }
    internal class InteractorDice : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            RoomUser roomUserByHabbo = null;
            if (Session != null)
            {
                roomUserByHabbo = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            }
            if (roomUserByHabbo != null)
            {
                if (Gamemap.TilesTouching(Item.GetX, Item.GetY, roomUserByHabbo.X, roomUserByHabbo.Y))
                {
                    if (Item.ExtraData != "-1")
                    {
                        if (Request == -1)
                        {
                            Item.ExtraData = "0";
                            Item.UpdateState();
                        }
                        else
                        {
                            Item.ExtraData = "-1";
                            Item.UpdateState(false, true);
                            Item.ReqUpdate(4, true);
                        }
                    }
                }
                else
                {
                    roomUserByHabbo.MoveTo(Item.SquareInFront);
                }
            }
        }
    }
}


