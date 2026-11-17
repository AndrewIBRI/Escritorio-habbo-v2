namespace Butterfly.HabboHotel.Support
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    internal class ModerationBanManager
    {
        private Hashtable bannedIPs = new Hashtable();
        private Hashtable bannedUsernames = new Hashtable();

        internal ModerationBanManager()
        {
        }

        internal void BanUser(GameClient Client, string Moderator, double LengthSeconds, string Reason, bool IpBan)
        {
            IQueryAdapter adapter;
            ModerationBanType uSERNAME = ModerationBanType.USERNAME;
            string username = Client.GetHabbo().Username;
            string query = "user";
            double expire = ButterflyEnvironment.GetUnixTimestamp() + LengthSeconds;
            if (IpBan)
            {
                uSERNAME = ModerationBanType.IP;
                username = Client.GetConnection().getIp();
                query = "ip";
            }
            ModerationBan ban = new ModerationBan(uSERNAME, username, Reason, expire);
            if (ban.Type == ModerationBanType.IP)
            {
                if (this.bannedIPs.ContainsKey(username))
                {
                    this.bannedIPs[username] = ban;
                }
                else
                {
                    this.bannedIPs.Add(username, ban);
                }
            }
            else if (this.bannedUsernames.ContainsKey(username))
            {
                this.bannedUsernames[username] = ban;
            }
            else
            {
                this.bannedUsernames.Add(username, ban);
            }
            using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery(string.Concat(new object[] { "INSERT INTO bans (bantype,value,reason,expire,added_by,added_date) VALUES (@rawvar,@var,@reason,'", expire, "',@mod,'", DateTime.Now.ToLongDateString(), "')" }));
                adapter.addParameter("rawvar", query);
                adapter.addParameter("var", username);
                adapter.addParameter("reason", Reason);
                adapter.addParameter("mod", Moderator);
                adapter.runQuery();
            }
            if (IpBan)
            {
                DataTable table = null;
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("SELECT id FROM users WHERE ip_last = @var");
                    adapter.addParameter("var", username);
                    table = adapter.getTable();
                }
                if (table != null)
                {
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            adapter.runFastQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = " + Convert.ToUInt32(row["id"]));
                        }
                    }
                }
                this.BanUser(Client, Moderator, LengthSeconds, Reason, false);
            }
            else
            {
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = " + Client.GetHabbo().Id);
                }
                Client.SendBanMessage(LanguageLocale.GetValue("moderation.banned") + " " + Reason);
                Client.Disconnect();
            }
        }

        internal string GetBanReason(string username, string ip)
        {
            ModerationBan ban;
            if (this.bannedUsernames.ContainsKey(username))
            {
                ban = (ModerationBan) this.bannedUsernames[username];
                if (!ban.Expired)
                {
                    return ban.ReasonMessage;
                }
            }
            else if (this.bannedIPs.ContainsKey(ip))
            {
                ban = (ModerationBan) this.bannedIPs[username];
                if (!ban.Expired)
                {
                    return ban.ReasonMessage;
                }
            }
            return string.Empty;
        }

        internal void LoadBans(IQueryAdapter dbClient)
        {
            this.bannedUsernames.Clear();
            this.bannedIPs.Clear();
            dbClient.setQuery("SELECT bantype,value,reason,expire FROM bans");
            DataTable table = dbClient.getTable();
            double unixTimestamp = ButterflyEnvironment.GetUnixTimestamp();
            foreach (DataRow row in table.Rows)
            {
                ModerationBanType uSERNAME;
                string variable = (string) row["value"];
                string reasonMessage = (string) row["reason"];
                double expire = (double) row["expire"];
                string str3 = (string) row["bantype"];
                if (str3 == "user")
                {
                    uSERNAME = ModerationBanType.USERNAME;
                }
                else
                {
                    uSERNAME = ModerationBanType.IP;
                }
                ModerationBan ban = new ModerationBan(uSERNAME, variable, reasonMessage, expire);
                if ((expire > unixTimestamp) && (ban.Type == ModerationBanType.USERNAME))
                {
                    if (!this.bannedUsernames.ContainsKey(variable))
                    {
                        this.bannedUsernames.Add(variable, ban);
                    }
                    else if (!this.bannedIPs.ContainsKey(variable))
                    {
                        this.bannedIPs.Add(variable, ban);
                    }
                }
            }
        }

        internal void UnbanUser(string usernameOrIP)
        {
            List<ModerationBan> list = new List<ModerationBan>();
            this.bannedUsernames.Remove(usernameOrIP);
            this.bannedIPs.Remove(usernameOrIP);
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("DELETE FROM bans WHERE value = @userorip");
                adapter.addParameter("userorip", usernameOrIP);
                adapter.runQuery();
            }
        }
    }
}

