namespace Butterfly.HabboHotel.Achievements
{
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    internal class AchievementLevelFactory
    {
        internal static void GetAchievementLevels(out Dictionary<string, Achievement> achievements, IQueryAdapter dbClient)
        {
            achievements = new Dictionary<string, Achievement>();
            dbClient.setQuery("SELECT * FROM achievements");
            DataTable table = dbClient.getTable();
            foreach (DataRow row in table.Rows)
            {
                uint id = Convert.ToUInt32(row["id"]);
                string category = (string) row["category"];
                string key = (string) row["group_name"];
                int num2 = (int) row["level"];
                int rewardPixels = (int) row["reward_pixels"];
                int rewardPoints = (int) row["reward_points"];
                int requirement = (int) row["progress_needed"];
                AchievementLevel level = new AchievementLevel(num2, rewardPixels, rewardPoints, requirement);
                if (!achievements.ContainsKey(key))
                {
                    Achievement achievement = new Achievement(id, key, category);
                    achievement.AddLevel(level);
                    achievements.Add(key, achievement);
                }
                else
                {
                    achievements[key].AddLevel(level);
                }
            }
        }
    }
}

