namespace Butterfly.HabboHotel.Users.Competitions
{
    using Butterfly;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public class Ranking
    {
        public static Dictionary<uint, Ranking> getRanking;
        public string Information;
        public int RoomId;
        public int Score;
        public RankingType Type;
        public uint UserId;

        public static void AddScoreToUserId(int tScore, uint UserId, RankingType t)
        {
            if (!getRanking.ContainsKey(UserId))
            {
                AddUserToRanking(UserId, t, "hlatCompetitions", 0);
            }
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "UPDATE user_rankings SET score = ", getRanking[UserId].Score + tScore, " WHERE userid = ", UserId, " AND type = '", parseEnum(t), "';" }));
            }
        }

        public static void AddUserToRanking(uint UserId, RankingType t, string Information = "hlatCompetitions", int RoomId = 0)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_rankings (type, information, userid, roomid, score) VALUES ('", parseEnum(t), "', '", Information, "', ", UserId, ", ", RoomId, ", 0);" }));
            }
            Ranking ranking = new Ranking {
                UserId = UserId,
                Type = t,
                Information = Information,
                RoomId = RoomId,
                Score = 0
            };
            getRanking.Add(UserId, ranking);
        }

        public static List<Ranking> getCompetitionForInfo(string Information)
        {
            List<Ranking> list = new List<Ranking>();
            foreach (Ranking ranking in getRanking.Values)
            {
                if ((ranking.Type == RankingType.COMPETITIONS) && (ranking.Information == Information))
                {
                    list.Add(ranking);
                }
            }
            return list;
        }

        public static List<Ranking> getRankingForType(RankingType t)
        {
            List<Ranking> list = new List<Ranking>();
            foreach (Ranking ranking in getRanking.Values)
            {
                if (ranking.Type == t)
                {
                    list.Add(ranking);
                }
            }
            return list;
        }

        public static void initRankings(IQueryAdapter dbClient)
        {
            dbClient.setQuery("SELECT * FROM user_rankings ORDER BY score ASC");
            DataTable table = dbClient.getTable();
            getRanking = new Dictionary<uint, Ranking>();
            foreach (DataRow row in table.Rows)
            {
                Ranking ranking = new Ranking {
                    UserId = (uint) row["userid"]
                };
                string str = (string) row["type"];
                if (str == "competitions")
                {
                    ranking.Type = RankingType.COMPETITIONS;
                }
                else if (str == "snowwar")
                {
                    ranking.Type = RankingType.SNOWWAR;
                }
                else if (str == "fastfood")
                {
                    ranking.Type = RankingType.FASTFOOD;
                }
                else if (str == "slotcar")
                {
                    ranking.Type = RankingType.SLOTCAR;
                }
                else
                {
                    ranking.Type = RankingType.NONE;
                }
                ranking.Information = (string) row["information"];
                ranking.RoomId = (int) row["roomid"];
                ranking.Score = (int) row["score"];
                getRanking.Add(ranking.UserId, ranking);
            }
        }

        private static string parseEnum(RankingType t)
        {
            if (t == RankingType.COMPETITIONS)
            {
                return "competitions";
            }
            if (t == RankingType.SNOWWAR)
            {
                return "snowwar";
            }
            if (t == RankingType.FASTFOOD)
            {
                return "fastfood";
            }
            if (t == RankingType.SLOTCAR)
            {
                return "slotcar";
            }
            if (t == RankingType.NONE)
            {
                return "";
            }
            return "";
        }
    }
}

