namespace Butterfly.HabboHotel.RoomBots
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Rooms;
    using System.Data;
    using Butterfly.Messages;
    using System.Collections.Generic;
    using HabboEvents;
    using System;
    using System.Drawing;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;

    internal class GenericBot : BotAI
    {
        private int ActionTimer;
        private int SpeechTimer;
        private int speechDelta;
        private bool canSpeak;
        private int nextAction;
        private int virtualId;

        internal GenericBot(int VirtualId,int botId)
        {
            int speechInterval = 7;

            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT speaking_interval from user_bots WHERE id = " + botId);
                speechInterval = adapter.getInteger();
                adapter.setQuery("SELECT automatic_chat from user_bots WHERE id = " + botId);
                canSpeak = Convert.ToBoolean(adapter.getString());
            }

            this.SpeechTimer = speechInterval*2;
            this.speechDelta = speechInterval*2;
            this.ActionTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(15, 30);
            this.nextAction = 0;
            this.virtualId = VirtualId;
        }

        internal override void OnSelfEnterRoom()
        {
       
                
 
        }

        internal override void OnSelfLeaveRoom(bool Kicked)
        {
        }

        internal override void OnTimerTick()
        {
            if (base.GetBotData() != null)
            {
                if (this.SpeechTimer <= 0)
                {
                    if (base.GetBotData().RandomSpeech.Count > 0)
                    {
                        if (canSpeak)
                        {
                            RandomSpeech randomSpeech = base.GetBotData().GetRandomSpeech();
                            base.GetRoomUser().Chat(null, randomSpeech.Message, randomSpeech.Shout, 0);
                        }
                    }
                    this.SpeechTimer = speechDelta;
                }
                else
                {
                    this.SpeechTimer--;
                }
                if (this.nextAction > 0)
                {
          
                    this.nextAction--;
                }
                else
                {
                    List<Point> list = base.GetRoom().GetGameMap().getWalkableList();
                    if (list.Count == 0)
                        return;
                    int randomNumber = new Random(DateTime.Now.Millisecond+this.virtualId^2).Next(0, list.Count-1);
                    switch (base.GetBotData().WalkingMode.ToLower())
                    {
                        case "freeroam":
                        {
                            base.GetRoomUser().MoveTo(list[randomNumber].X,list[randomNumber].Y);
                            break;
                        }
                        case "specified_range":
                        {
                            base.GetRoomUser().MoveTo(list[randomNumber].X, list[randomNumber].Y);
                            break;
                        }
                    }
                    this.nextAction = new Random(DateTime.Now.Millisecond+this.virtualId^2).Next(15, 30);
                }
            }
        }

        internal override void OnUserEnterRoom(RoomUser User)
        {
        }

        internal override void OnUserLeaveRoom(GameClient Client)
        {
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

                            if (str == "whisper")
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
    }
}

