namespace Butterfly.HabboHotel.Support
{
    using Butterfly;
    using Butterfly.Messages;
    using Database_Manager.Database;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class HelpTool
    {
        internal Dictionary<uint, HelpCategory> Categories = new Dictionary<uint, HelpCategory>();
        internal List<HelpTopic> ImportantTopics = new List<HelpTopic>();
        internal List<HelpTopic> KnownIssues = new List<HelpTopic>();
        internal Dictionary<uint, HelpTopic> Topics = new Dictionary<uint, HelpTopic>();

        internal HelpTool()
        {
        }

        internal int ArticlesInCategory(uint CategoryId)
        {
            int num = 0;
            foreach (HelpTopic topic in this.Topics.Values)
            {
                if (topic.CategoryId == CategoryId)
                {
                    num++;
                }
            }
            return num;
        }

        internal HelpCategory GetCategory(uint CategoryId)
        {
            if (this.Categories.ContainsKey(CategoryId))
            {
                return this.Categories[CategoryId];
            }
            return null;
        }

        internal HelpTopic GetTopic(uint TopicId)
        {
            if (this.Topics.ContainsKey(TopicId))
            {
                return this.Topics[TopicId];
            }
            return null;
        }

        internal void LoadCategories(IQueryAdapter dbClient)
        {
            this.Categories.Clear();
            DataTable table = null;
            dbClient.setQuery("SELECT id, caption FROM help_subjects");
            table = dbClient.getTable();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    this.Categories.Add(Convert.ToUInt32(row["id"]), new HelpCategory(Convert.ToUInt32(row["id"]), (string) row["caption"]));
                }
            }
        }

        internal void LoadTopics(IQueryAdapter dbClient)
        {
            this.Topics.Clear();
            DataTable table = null;
            dbClient.setQuery("SELECT id, title, body, subject, known_issue FROM help_topics");
            table = dbClient.getTable();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    HelpTopic topic = new HelpTopic(Convert.ToUInt32(row["id"]), (string) row["title"], (string) row["body"], Convert.ToUInt32(row["subject"]));
                    this.Topics.Add(Convert.ToUInt32(row["id"]), topic);
                    switch (int.Parse(row["known_issue"].ToString()))
                    {
                        case 1:
                            this.KnownIssues.Add(topic);
                            break;

                        case 2:
                            this.ImportantTopics.Add(topic);
                            break;
                    }
                }
            }
        }

        internal ServerMessage SerializeCategory(HelpCategory Category)
        {
            ServerMessage message = new ServerMessage(0x20a);
            message.AppendInt32(Category.CategoryId);
            message.AppendString("");
            message.AppendInt32(this.ArticlesInCategory(Category.CategoryId));
            foreach (HelpTopic topic in this.Topics.Values)
            {
                if (topic.CategoryId == Category.CategoryId)
                {
                    message.AppendInt32(topic.TopicId);
                    message.AppendString(topic.Caption);
                }
            }
            return message;
        }

        internal ServerMessage SerializeFrontpage()
        {
            ServerMessage message = new ServerMessage(Outgoing.OpenHelpTool);
            message.AppendInt32(0);
            return message;
        }

        internal ServerMessage SerializeIndex()
        {
            ServerMessage message = new ServerMessage(0x207);
            message.AppendInt32(this.Categories.Count);
            foreach (HelpCategory category in this.Categories.Values)
            {
                message.AppendInt32(category.CategoryId);
                message.AppendString(category.Caption);
                message.AppendInt32(this.ArticlesInCategory(category.CategoryId));
            }
            return message;
        }

        internal static ServerMessage SerializeSearchResults(string Query)
        {
            DataTable table = null;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (adapter.dbType == DatabaseType.MySQL)
                {
                    adapter.setQuery("SELECT id,title FROM help_topics WHERE title LIKE @query OR body LIKE @query LIMIT 25");
                }
                else
                {
                    adapter.setQuery("SELECT TOP 25 id,title FROM help_topics WHERE title LIKE @query OR body LIKE @query");
                }
                adapter.addParameter("query", Query + "%");
                table = adapter.getTable();
            }
            ServerMessage message = new ServerMessage(0x209);
            if (table == null)
            {
                message.AppendBoolean(false);
                return message;
            }
            message.AppendInt32(table.Rows.Count);
            foreach (DataRow row in table.Rows)
            {
                message.AppendInt32(Convert.ToUInt32(row["id"]));
                message.AppendString((string) row["title"]);
            }
            return message;
        }

        internal static ServerMessage SerializeTopic(HelpTopic Topic)
        {
            ServerMessage message = new ServerMessage(520);
            message.AppendInt32(Topic.TopicId);
            message.AppendString(Topic.Body);
            return message;
        }
    }
}

