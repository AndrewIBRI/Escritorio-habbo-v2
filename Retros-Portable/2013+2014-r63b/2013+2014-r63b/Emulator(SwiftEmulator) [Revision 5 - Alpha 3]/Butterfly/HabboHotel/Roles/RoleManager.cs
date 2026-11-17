namespace Butterfly.HabboHotel.Roles
{
    using Butterfly.HabboHotel.Habbo.Users;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class RoleManager
    {
        private Dictionary<string, uint> Rights = new Dictionary<string, uint>();
        private Dictionary<string, string> SubRights = new Dictionary<string, string>();

        internal RoleManager()
        {
        }

        internal void ClearRights()
        {
            this.Rights.Clear();
        }

        internal bool ContainsRight(string Right)
        {
            return this.Rights.ContainsKey(Right);
        }

        internal List<string> GetRightsForHabbo(Habbo Habbo)
        {
            List<string> list = new List<string>();
            if (Habbo != null)
            {
                list.AddRange(this.GetRightsForRank(Habbo.Rank));
                foreach (string str in Habbo.GetSubscriptionManager().SubList)
                {
                    list.AddRange(this.GetRightsForSub(str));
                }
            }
            return list;
        }

        internal List<string> GetRightsForRank(uint RankId)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, uint> pair in this.Rights)
            {
                if (!((RankId < pair.Value) || list.Contains(pair.Key)))
                {
                    list.Add(pair.Key);
                }
            }
            return list;
        }

        internal List<string> GetRightsForSub(string SubId)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> pair in this.SubRights)
            {
                if (pair.Value == SubId)
                {
                    list.Add(pair.Key);
                }
            }
            return list;
        }

        internal void LoadRights(IQueryAdapter dbClient)
        {
            this.ClearRights();
            dbClient.setQuery("SELECT fuse, rank FROM fuserights;");
            DataTable table = dbClient.getTable();
            dbClient.setQuery("SELECT fuse, sub FROM fuserights_subs;");
            DataTable table2 = dbClient.getTable();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    this.Rights.Add((string) row["fuse"], Convert.ToUInt32(row["rank"]));
                }
            }
            if (table2 != null)
            {
                foreach (DataRow row in table2.Rows)
                {
                    this.SubRights.Add((string) row["fuse"], (string) row["sub"]);
                }
            }
        }

        internal bool RankHasRight(uint RankId, string Fuse)
        {
            if (!this.ContainsRight(Fuse))
            {
                return false;
            }
            uint num = this.Rights[Fuse];
            return (RankId >= num);
        }

        internal bool SubHasRight(string Sub, string Fuse)
        {
            return (this.SubRights.ContainsKey(Fuse) && (this.SubRights[Fuse] == Sub));
        }
    }
}

