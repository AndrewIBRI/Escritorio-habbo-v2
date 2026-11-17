namespace Butterfly.HabboHotel.RoomBots
{
    using System;

    internal class RandomSpeech
    {
        internal uint BotID;
        internal string Message;
        internal bool Shout;

        internal RandomSpeech(string Message, bool Shout, uint BotID)
        {
            this.BotID = BotID;
            this.Message = Message;
            this.Shout = Shout;
        }
    }
}

