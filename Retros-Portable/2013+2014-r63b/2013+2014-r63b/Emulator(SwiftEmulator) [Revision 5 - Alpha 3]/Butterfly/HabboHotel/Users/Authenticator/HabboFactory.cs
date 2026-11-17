namespace Butterfly.HabboHotel.Users.Authenticator
{
    using Butterfly;
    using Butterfly.HabboHotel.Habbo.Users;
    using System;
    using System.Data;

    internal static class HabboFactory
    {
        internal static Habbo GenerateHabbo(DataRow dRow, DataRow group)
        {
            uint id = Convert.ToUInt32(dRow["id"]);
            string username = (string) dRow["username"];
            string realName = (string) dRow["real_name"];
            uint rank = Convert.ToUInt32(dRow["rank"]);
            string motto = (string) dRow["motto"];
            string look = (string) dRow["look"];
            string gender = (string) dRow["gender"];
            string lastOnline = (string) dRow["last_online"];
            int credits = (int) dRow["credits"];
            int activityPoints = (int) dRow["activity_points"];
            double lastActivityPointsUpdate = Convert.ToDouble(dRow["activity_points_lastupdate"]);
            bool muted = ButterflyEnvironment.EnumToBool(dRow["is_muted"].ToString());
            uint homeRoom = Convert.ToUInt32(dRow["home_room"]);
            int respect = (int) dRow["respect"];
            int dailyRespectPoints = (int) dRow["daily_respect_points"];
            int dailyPetRespectPoints = (int) dRow["daily_pet_respect_points"];
            bool mutantPenalty = dRow["mutant_penalty"].ToString() != "0";
            bool hasFriendRequestsDisabled = ButterflyEnvironment.EnumToBool(dRow["block_newfriends"].ToString());
            int achievementPoints = (int) dRow["achievement_points"];
            string quests = (string) dRow["quests"];
            string queststates = (string) dRow["queststates"];
            bool canchangename = ButterflyEnvironment.EnumToBool(dRow["canchangename"].ToString());
            int favoriteGroup = (int) dRow["FavoriteGroup"];
            bool passedQuiz = ButterflyEnvironment.EnumToBool(dRow["passed_quiz"].ToString());
            return new Habbo(id, username, realName, rank, motto, look, gender, credits, activityPoints, (int)dRow["crystals"], lastActivityPointsUpdate, muted, homeRoom, respect, dailyRespectPoints, dailyPetRespectPoints, mutantPenalty, hasFriendRequestsDisabled, group, achievementPoints, lastOnline, quests, queststates, canchangename, favoriteGroup, passedQuiz, (int)dRow["points_online"]);
        }
    }
}

