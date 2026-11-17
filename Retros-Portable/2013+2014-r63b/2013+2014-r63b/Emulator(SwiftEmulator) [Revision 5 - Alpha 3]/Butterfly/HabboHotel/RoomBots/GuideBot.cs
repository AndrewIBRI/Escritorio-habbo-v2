namespace Butterfly.HabboHotel.RoomBots
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.Messages;
    using HabboEvents;
    using System;
    using System.Drawing;

    internal class GuideBot : BotAI
    {
        private int ActionTimer = 0;
        private int SpeechTimer = 0;

        internal GuideBot()
        {
        }

        internal override void OnSelfEnterRoom()
        {
            base.GetRoomUser().Chat(null, LanguageLocale.GetValue("guide.message"), true, 0);
            base.GetRoomUser().Chat(null, LanguageLocale.GetValue("guide.question"), false, 0);
        }

        internal override void OnSelfLeaveRoom(bool Kicked)
        {
        }

        internal override void OnTimerTick()
        {
            if (this.SpeechTimer <= 0)
            {
                if ((base.GetBotData() != null) && (base.GetBotData().RandomSpeech.Count > 0))
                {
                    RandomSpeech randomSpeech = base.GetBotData().GetRandomSpeech();
                    base.GetRoomUser().Chat(null, randomSpeech.Message, randomSpeech.Shout, 0);
                }
                this.SpeechTimer = ButterflyEnvironment.GetRandomNumber(0, 150);
            }
            else
            {
                this.SpeechTimer--;
            }
            if (this.ActionTimer <= 0)
            {
                Point point = base.GetRoom().GetGameMap().getRandomWalkableSquare();
                base.GetRoomUser().MoveTo(point.X, point.Y);
                this.ActionTimer = ButterflyEnvironment.GetRandomNumber(0, 30);
            }
            else
            {
                this.ActionTimer--;
            }
        }

        internal override void OnUserEnterRoom(RoomUser User)
        {
        }

        internal override void OnUserLeaveRoom(GameClient Client)
        {
            if ((((base.GetRoom() != null) && (Client != null)) && (Client.GetHabbo() != null)) && (base.GetRoom().Owner.ToLower() == Client.GetHabbo().Username.ToLower()))
            {
                base.GetRoom().GetRoomUserManager().RemoveBot(base.GetRoomUser().VirtualId, false);
            }
        }

        internal override void OnUserSay(RoomUser User, string Message)
        {
            if (Gamemap.TileDistance(base.GetRoomUser().X, base.GetRoomUser().Y, User.X, User.Y) <= 8)
            {
                BotResponse response = base.GetBotData().GetResponse(Message);
                if (response != null)
                {
                    string str = response.ResponseType.ToLower();
                    if (str != null)
                    {
                        if (!(str == "say"))
                        {
                            if (str == "shout")
                            {
                                base.GetRoomUser().Chat(null, response.ResponseText, true, 0);
                            }
                            else if (str == "whisper")
                            {
                                ServerMessage message = new ServerMessage(Outgoing.Whisp);
                                message.AppendInt32(base.GetRoomUser().VirtualId);
                                message.AppendString(response.ResponseText);
                                message.AppendInt32(0);
                                message.AppendInt32(0);
                                message.AppendInt32(0);
                                message.AppendInt32(-1);
                                User.GetClient().SendMessage(message);
                            }
                        }
                        else
                        {
                            base.GetRoomUser().Chat(null, response.ResponseText, false, 0);
                        }
                    }
                    if (response.ServeId >= 1)
                    {
                        User.CarryItem(response.ServeId);
                    }
                }
            }
        }

        internal override void OnUserShout(RoomUser User, string Message)
        {
        }
    }
}

