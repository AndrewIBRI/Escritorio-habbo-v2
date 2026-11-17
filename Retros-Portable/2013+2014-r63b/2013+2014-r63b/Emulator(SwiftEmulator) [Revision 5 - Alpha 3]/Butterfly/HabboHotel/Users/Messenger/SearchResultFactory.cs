namespace Butterfly.HabboHotel.Users.Messenger
{
    using Butterfly;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class SearchResultFactory
    {
        internal static List<SearchResult> GetSearchResult(string query)
        {
            DataTable table;
            List<SearchResult> list = new List<SearchResult>();
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MySQL)
                {
                    adapter.setQuery("SELECT id,username,motto,look,last_online FROM users WHERE username LIKE @query LIMIT 50");
                }
                else
                {
                    adapter.setQuery("SELECT TOP 50 id,username,motto,look,last_online FROM users WHERE username LIKE @query");
                }
                adapter.addParameter("query", query + "%");
                table = adapter.getTable();
            }
            foreach (DataRow row in table.Rows)
            {
                uint userID = Convert.ToUInt32(row[0]);
                string username = (string) row[1];
                string motto = (string) row[2];
                string look = (string) row[3];
                string str4 = (string) row[4];
                SearchResult item = new SearchResult(userID, username, motto, look, str4);
                list.Add(item);
            }
            return list;
        }
    }
}

