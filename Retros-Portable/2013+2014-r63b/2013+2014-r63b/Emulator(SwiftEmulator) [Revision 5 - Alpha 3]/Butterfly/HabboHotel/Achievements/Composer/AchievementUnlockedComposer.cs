namespace Butterfly.HabboHotel.Achievements.Composer
{
    using Butterfly.HabboHotel.Achievements;
    using Butterfly.Messages;
    using HabboEvents;
    using System;

    internal class AchievementUnlockedComposer
    {
        internal static ServerMessage Compose(Achievement Achievement, int Level, int PointReward, int PixelReward)
        {
            ServerMessage message = new ServerMessage(Outgoing.UnlockAchievement);
            message.AppendInt32(Achievement.Id);
            message.AppendInt32(Level);
            message.AppendInt32(0x38f);
            message.AppendString(Achievement.GroupName + Level);
            message.AppendInt32(PointReward);
            message.AppendInt32(PixelReward);
            message.AppendInt32(0);
            message.AppendInt32(10);
            message.AppendString((Level > 1) ? (Achievement.GroupName + (Level - 1)) : string.Empty);
            message.AppendString(Achievement.Category);
            message.AppendBoolean(true);

            message.AppendInt32(0);
            message.AppendInt32(0);
            return message;
        }
    }
}

