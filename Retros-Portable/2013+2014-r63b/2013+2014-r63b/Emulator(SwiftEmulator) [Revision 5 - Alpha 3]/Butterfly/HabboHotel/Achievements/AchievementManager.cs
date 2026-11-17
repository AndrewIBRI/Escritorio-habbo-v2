namespace Butterfly.HabboHotel.Achievements
{
    using Butterfly;
    using Butterfly.HabboHotel.Achievements.Composer;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AchievementManager
    {
        internal Dictionary<string, Achievement> Achievements = new Dictionary<string, Achievement>();

        internal AchievementManager(IQueryAdapter dbClient)
        {
            this.LoadAchievements(dbClient);
        }

        internal bool ContainsAchievement(string AchievementGroup, int Level, GameClient Session)
        {
            if (this.Achievements.ContainsKey(AchievementGroup))
            {
                UserAchievement achievementData = Session.GetHabbo().GetAchievementData(AchievementGroup);
                return ((achievementData != null) && (achievementData.Level >= Level));
            }
            return false;
        }

        internal bool ContainsGroupAch(Dictionary<string, int> AchievementGroup, GameClient Session)
        {
            if (AchievementGroup.Count <= 0)
            {
                return false;
            }
            foreach (KeyValuePair<string, int> pair in AchievementGroup)
            {
                if (!this.Achievements.ContainsKey(pair.Key) || (pair.Value != 1))
                {
                    if (!this.Achievements.ContainsKey(pair.Key))
                    {
                        return false;
                    }
                    UserAchievement achievementData = Session.GetHabbo().GetAchievementData(pair.Key);
                    if (achievementData == null)
                    {
                        return false;
                    }
                    if (achievementData.Level < pair.Value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal Achievement GetAchievement(string AchievementGroup)
        {
            if (this.Achievements.ContainsKey(AchievementGroup))
            {
                return this.Achievements[AchievementGroup];
            }
            return null;
        }

        internal void GetList(GameClient Session, ClientMessage Message)
        {
            Session.SendMessage(AchievementListComposer.Compose(Session, this.Achievements.Values.ToList<Achievement>()));
        }

        internal void LoadAchievements(IQueryAdapter dbClient)
        {
            AchievementLevelFactory.GetAchievementLevels(out this.Achievements, dbClient);
        }

        internal bool ProgressUserAchievement(GameClient Session, string AchievementGroup, int ProgressAmount)
        {
            try
            {
                if (this.Achievements.ContainsKey(AchievementGroup))
                {
                    Achievement achievement = null;
                    achievement = this.Achievements[AchievementGroup];
                    UserAchievement achievementData = Session.GetHabbo().GetAchievementData(AchievementGroup);
                    if (achievementData == null)
                    {
                        achievementData = new UserAchievement(AchievementGroup, 0, 0);
                        Session.GetHabbo().Achievements.Add(AchievementGroup, achievementData);
                    }
                    int count = achievement.Levels.Count;
                    if ((achievementData != null) && (achievementData.Level == count))
                    {
                        return false;
                    }
                    int num2 = (achievementData != null) ? (achievementData.Level + 1) : 1;
                    if (num2 > count)
                    {
                        num2 = count;
                    }
                    AchievementLevel targetLevelData = achievement.Levels[num2];
                    int num3 = (achievementData != null) ? (achievementData.Progress + ProgressAmount) : ProgressAmount;
                    int num4 = (achievementData != null) ? achievementData.Level : 0;
                    int targetLevel = num4 + 1;
                    if (targetLevel > count)
                    {
                        targetLevel = count;
                    }
                    if (num3 >= targetLevelData.Requirement)
                    {
                        num4++;
                        targetLevel++;
                        int num6 = num3 - targetLevelData.Requirement;
                        num3 = 0;
                        Session.GetHabbo().GetBadgeComponent().GiveBadge(AchievementGroup + num2, true);
                        if (targetLevel > count)
                        {
                            targetLevel = count;
                        }
                        Habbo habbo = Session.GetHabbo();
                        habbo.ActivityPoints += targetLevelData.RewardPixels;
                        Session.GetHabbo().UpdateActivityPointsBalance(false);
                        Habbo habbo2 = Session.GetHabbo();
                        habbo2.AchievementPoints += 5;
                        Session.SendMessage(AchievementUnlockedComposer.Compose(achievement, num2, targetLevelData.RewardPoints, targetLevelData.RewardPixels));
                        try
                        {
                            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                if (num2 < 2)
                                {
                                    adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_badges VALUES (NULL, '", Session.GetHabbo().Id, "', '", AchievementGroup, num4, "', '0')" }));
                                    adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_achievement VALUES ('", Session.GetHabbo().Id, "','", AchievementGroup, "','", num4, "','", num3, "')" }));
                                }
                                else
                                {
                                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE user_badges SET badge_id = '", AchievementGroup, num4, "' WHERE user_id = '", Session.GetHabbo().Id, "' AND badge_id = '", AchievementGroup, num4 - 1, "'" }));
                                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE user_achievement SET level = '", num4, "' WHERE groupname = '", AchievementGroup, "' AND userid = '", Session.GetHabbo().Id, "'" }));
                                }
                                adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET achievement_points = '", Session.GetHabbo().AchievementPoints, "' WHERE id = '", Session.GetHabbo().Id, "'" }));
                            }
                        }
                        catch
                        {
                        }
                        achievementData.Level = num4;
                        achievementData.Progress = num3;
                        Habbo habbo3 = Session.GetHabbo();
                        habbo3.AchievementPoints += targetLevelData.RewardPoints;
                        Habbo habbo4 = Session.GetHabbo();
                        habbo4.ActivityPoints += targetLevelData.RewardPixels;
                        Session.GetHabbo().UpdateActivityPointsBalance(false);
                        Session.SendMessage(AchievementScoreUpdateComposer.Compose(Session.GetHabbo().AchievementPoints));
                        AchievementLevel level2 = achievement.Levels[targetLevel];
                        Session.SendMessage(AchievementProgressComposer.Compose(achievement, targetLevel, level2, count, Session.GetHabbo().GetAchievementData(AchievementGroup)));
                        return true;
                    }
                    achievementData.Level = num4;
                    achievementData.Progress = num3;
                    Session.SendMessage(AchievementProgressComposer.Compose(achievement, num2, targetLevelData, count, Session.GetHabbo().GetAchievementData(AchievementGroup)));
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}

