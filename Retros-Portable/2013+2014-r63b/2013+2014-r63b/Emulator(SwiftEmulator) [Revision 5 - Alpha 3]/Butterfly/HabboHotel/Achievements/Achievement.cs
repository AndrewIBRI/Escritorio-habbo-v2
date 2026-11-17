namespace Butterfly.HabboHotel.Achievements
{
    using System;
    using System.Collections.Generic;

    internal class Achievement
    {
        internal readonly string Category;
        internal readonly string GroupName;
        internal readonly uint Id;
        internal readonly Dictionary<int, AchievementLevel> Levels;

        public Achievement(uint Id, string GroupName, string Category)
        {
            this.Id = Id;
            this.GroupName = GroupName;
            this.Category = Category;
            this.Levels = new Dictionary<int, AchievementLevel>();
        }

        public void AddLevel(AchievementLevel Level)
        {
            this.Levels.Add(Level.Level, Level);
        }
    }
}

