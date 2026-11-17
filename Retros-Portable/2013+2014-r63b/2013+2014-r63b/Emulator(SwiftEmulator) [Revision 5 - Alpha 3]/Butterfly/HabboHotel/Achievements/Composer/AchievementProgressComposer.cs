namespace Butterfly.HabboHotel.Achievements.Composer
{
    using Butterfly.HabboHotel.Achievements;
    using Butterfly.Messages;
    using HabboEvents;
    using System;

    internal class AchievementProgressComposer
    {
        internal static ServerMessage Compose(Achievement Achievement, int TargetLevel, AchievementLevel TargetLevelData, int TotalLevels, UserAchievement UserData)
        {
            ServerMessage message = new ServerMessage(Outgoing.AchievementProgress);
            message.AppendInt32(Achievement.Id);
            message.AppendInt32(TargetLevel);
            message.AppendString(Achievement.GroupName + TargetLevel);
            message.AppendInt32(TargetLevelData.Requirement);
            message.AppendInt32(TargetLevelData.RewardPixels);
            message.AppendInt32(TargetLevelData.RewardPoints);
            message.AppendInt32(0);
            message.AppendInt32((UserData != null) ? UserData.Progress : 0);
            message.AppendBoolean((UserData != null) ? (UserData.Level >= TotalLevels) : false);
            message.AppendString(Achievement.Category);
            message.AppendInt32(TotalLevels);
            message.AppendInt32(0);
            return message;
        }
    }
}

