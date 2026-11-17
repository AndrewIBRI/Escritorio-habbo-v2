namespace Butterfly.HabboHotel.Achievements.Composer
{
    using Butterfly.Messages;
    using HabboEvents;
    using System;

    internal class AchievementScoreUpdateComposer
    {
        internal static ServerMessage Compose(int Score)
        {
            ServerMessage message = new ServerMessage(Outgoing.AchievementPoints);
            message.AppendInt32(Score);
            return message;
        }
    }
}

