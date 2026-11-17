namespace Butterfly.HabboHotel.RoomBots
{
    using System;
    using System.Collections.Generic;

    internal class BotResponse
    {
        internal uint BotId;
        internal List<string> Keywords;
        internal string ResponseText;
        internal string ResponseType;
        internal int ServeId;

        internal BotResponse(uint BotId, string Keywords, string ResponseText, string ResponseType, int ServeId)
        {
            this.BotId = BotId;
            this.Keywords = new List<string>();
            this.ResponseText = ResponseText;
            this.ResponseType = ResponseType;
            this.ServeId = ServeId;
            foreach (string str in Keywords.Split(new char[] { ';' }))
            {
                this.Keywords.Add(str.ToLower());
            }
        }

        internal bool KeywordMatched(string Message)
        {
            foreach (string str in this.Keywords)
            {
                if (Message.ToLower().Contains(str.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

