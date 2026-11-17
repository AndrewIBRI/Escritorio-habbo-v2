namespace Butterfly.HabboHotel.RoomBots
{
    using Butterfly;
    using System;
    using System.Collections.Generic;
    using Butterfly.Messages;

    internal class RoomBot
    {
        internal AIType AiType;
        internal uint BotId;
        internal string Look;
        internal int maxX;
        internal int maxY;
        internal int OwnerId;
        internal int minX;
        internal int minY;
        internal string Motto;
        internal string Name;
        internal List<Butterfly.HabboHotel.RoomBots.RandomSpeech> RandomSpeech;
        internal List<BotResponse> Responses;
        internal uint RoomId;
        internal int Rot;
        internal string WalkingMode;
        internal int X;
        internal int Y;
        internal int Z;

        internal RoomBot(uint BotId, uint RoomId, AIType AiType, string WalkingMode, string Name, string Motto, string Look, int X, int Y, int Z, int Rot, int minX, int minY, int maxX, int maxY, ref List<Butterfly.HabboHotel.RoomBots.RandomSpeech> Speeches, ref List<BotResponse> Responses, int OwnerId)
        {
            this.BotId = BotId;
            this.RoomId = RoomId;
            this.AiType = AiType;
            this.WalkingMode = WalkingMode;
            this.Name = Name;
            this.Motto = Motto;
            this.Look = Look;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Rot = Rot;
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
            this.LoadRandomSpeech(Speeches);
            this.LoadResponses(Responses);
            this.OwnerId = OwnerId;
        }
        internal void SerializeInventory(ServerMessage Message)
        {
            
            Message.AppendInt32(BotId); // id
            Message.AppendString(Name); // name
            Message.AppendString("");
            Message.AppendString("m");
            Message.AppendString(Look); // color
         
            //Message.AppendBoolean(true);

        }
        internal BotAI GenerateBotAI(int VirtualId,int botId)
        {
            switch (this.AiType)
            {
                case AIType.Pet:
                    return new PetBot(VirtualId);

                case AIType.Guide:
                    return new GuideBot();
            }
            return new GenericBot(VirtualId,botId);
        }

        internal Butterfly.HabboHotel.RoomBots.RandomSpeech GetRandomSpeech()
        {
            return this.RandomSpeech[ButterflyEnvironment.GetRandomNumber(0, this.RandomSpeech.Count - 1)];
        }

        internal BotResponse GetResponse(string Message)
        {
            foreach (BotResponse response in this.Responses)
            {
                if (response.KeywordMatched(Message))
                {
                    return response;
                }
            }
            return null;
        }

        internal void LoadRandomSpeech(List<Butterfly.HabboHotel.RoomBots.RandomSpeech> Speeches)
        {
            this.RandomSpeech = new List<Butterfly.HabboHotel.RoomBots.RandomSpeech>();
            foreach (Butterfly.HabboHotel.RoomBots.RandomSpeech speech in Speeches)
            {
                if (speech.BotID == this.BotId)
                {
                    this.RandomSpeech.Add(speech);
                }
            }
        }

        internal void LoadResponses(List<BotResponse> Respons)
        {
            this.Responses = new List<BotResponse>();
            foreach (BotResponse response in Respons)
            {
                if (response.BotId == this.BotId)
                {
                    this.Responses.Add(response);
                }
            }
        }

        internal bool IsPet
        {
            get
            {
                return (this.AiType == AIType.Pet);
            }
        }
    }
}

