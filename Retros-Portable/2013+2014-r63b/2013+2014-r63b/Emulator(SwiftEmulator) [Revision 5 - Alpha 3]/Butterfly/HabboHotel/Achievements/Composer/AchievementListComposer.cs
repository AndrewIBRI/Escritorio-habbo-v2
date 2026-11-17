namespace Butterfly.HabboHotel.Achievements.Composer
{
    using Butterfly.HabboHotel.Achievements;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.Messages;
    using HabboEvents;
    using System;
    using System.Collections.Generic;

    internal class AchievementListComposer
    {
        internal static ServerMessage Compose(GameClient Session, List<Achievement> Achievements)
        {
            ServerMessage message = new ServerMessage(Outgoing.AchievementList);
            message.AppendInt32(Achievements.Count);
            foreach (Achievement achievement in Achievements)
            {
                UserAchievement achievementData = Session.GetHabbo().GetAchievementData(achievement.GroupName);
                int i = (achievementData != null) ? (achievementData.Level + 1) : 1;
                int count = achievement.Levels.Count;
                if (i > count)
                {
                    i = count;
                }
                AchievementLevel level = achievement.Levels[i];
                message.AppendInt32(achievement.Id);
                message.AppendInt32(i);
                message.AppendString(achievement.GroupName + i);
                message.AppendInt32(level.RewardPixels);
                message.AppendInt32(level.RewardPoints);
                message.AppendInt32((achievementData != null) ? achievementData.Progress : 0);
                message.AppendBoolean((achievementData != null) ? (achievementData.Level >= count) : false);
                message.AppendString(achievement.Category);
                message.AppendString(string.Empty);
                message.AppendInt32(count);
                message.AppendInt32(0);
            }
            message.AppendString("");
            return message;
        }
    }
}

