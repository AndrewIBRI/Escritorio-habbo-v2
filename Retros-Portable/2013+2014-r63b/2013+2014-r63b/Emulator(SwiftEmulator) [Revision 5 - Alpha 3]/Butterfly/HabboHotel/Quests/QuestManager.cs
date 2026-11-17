namespace Butterfly.HabboHotel.Quests
{
    using Butterfly;
    using Butterfly.HabboHotel.Habbo.Users;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class QuestManager
    {
        public int Award;
        public string Category;
        public int CountFurnis;
        public string ExtraValue;
        public int Id;
        public List<QuestManager> Quests;
        public string SecondType;
        public string Type;
        public string UsersActivate;
        public List<uint> UsersId;

        public int GetIntValue(string Type)
        {
            switch (Type)
            {
                case "social":
                    return 3;

                case "identity":
                    return 4;

                case "explore":
                    return 5;

                case "battleball":
                    return 7;

                case "freeze":
                    return 8;
            }
            return 0;
        }

        public QuestManager GetMyActualQuest(string Type, Habbo User)
        {
            foreach (QuestManager manager in this.QuestsInCategory(Type))
            {
                if (!User.QuestsCompleted.Contains(manager.Id))
                {
                    return manager;
                }
            }
            using (List<QuestManager>.Enumerator enumerator = this.QuestsInCategory(Type).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    return enumerator.Current;
                }
            }
            return null;
        }

        public QuestManager GetQuestById(int Id)
        {
            foreach (QuestManager manager in this.Quests)
            {
                if (manager.Id == Id)
                {
                    return manager;
                }
            }
            return null;
        }

        public Butterfly.HabboHotel.Quests.Quests GetQuests()
        {
            return new Butterfly.HabboHotel.Quests.Quests();
        }

        public List<QuestManager> GetQuestsForType(string Type)
        {
            List<QuestManager> list = new List<QuestManager>();
            foreach (QuestManager manager in ButterflyEnvironment.GetGame().GetQuestManager().Quests)
            {
                if (manager.Category == Type)
                {
                    list.Add(manager);
                }
            }
            return list;
        }

        public List<string> GetStringTypes()
        {
            List<string> list = new List<string>();
            foreach (QuestManager manager in this.Quests)
            {
                if (!list.Contains(manager.Category))
                {
                    list.Add(manager.Category);
                }
            }
            return list;
        }

        public void Init(IQueryAdapter dbClient)
        {
            this.Quests = new List<QuestManager>();
            dbClient.setQuery("SELECT * FROM quests");
            DataTable table = dbClient.getTable();
            foreach (DataRow row in table.Rows)
            {
                QuestManager item = new QuestManager {
                    Id = (int) row["id"],
                    Type = (string) row["quest_type"],
                    SecondType = (string) row["secondtype"],
                    ExtraValue = (string) row["extravalue"],
                    Category = (string) row["category"],
                    UsersActivate = (string) row["usersactivate"],
                    UsersId = new List<uint>()
                };
                if (item.UsersActivate.Length > 0)
                {
                    foreach (string str in item.UsersActivate.Split(new char[] { ';' }))
                    {
                        try
                        {
                            item.UsersId.Add(uint.Parse(str));
                        }
                        catch
                        {
                        }
                    }
                }
                item.Award = (int) row["award"];
                item.CountFurnis = (int) row["countfurnis"];
                this.Quests.Add(item);
            }
        }

        public bool IsActivated(int QuestId, Habbo User)
        {
            foreach (QuestManager manager in this.Quests)
            {
                if (manager.Id == QuestId)
                {
                    return manager.UsersId.Contains(User.Id);
                }
            }
            return false;
        }

        public List<QuestManager> MyQuestsCompleted(string Type, Habbo User)
        {
            List<QuestManager> list = new List<QuestManager>();
            foreach (QuestManager manager in this.Quests)
            {
                if ((manager.Category == Type) && User.QuestsCompleted.Contains(manager.Id))
                {
                    list.Add(manager);
                }
            }
            return list;
        }

        public List<QuestManager> QuestsInCategory(string Type)
        {
            List<QuestManager> list = new List<QuestManager>();
            foreach (QuestManager manager in this.Quests)
            {
                if (manager.Category == Type)
                {
                    list.Add(manager);
                }
            }
            return list;
        }
    }
}

