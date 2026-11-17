namespace Butterfly.HabboHotel.Quests
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections.Generic;

    internal class Quests
    {
        public void ActiveEndQuest(GameClient Session)
        {
            int id = Session.GetHabbo().LastQuestId + 1;
            QuestManager questById = ButterflyEnvironment.GetGame().GetQuestManager().GetQuestById(Session.GetHabbo().LastQuestId);
            QuestManager manager2 = ButterflyEnvironment.GetGame().GetQuestManager().GetQuestById(id);
            if ((manager2 != null) && (questById.Category == manager2.Category))
            {
                IQueryAdapter adapter;
                Session.GetHabbo().ActiveQuestNow = true;
                Session.GetHabbo().QuestActivate = manager2.Id;
                if (!Session.GetHabbo().QuestsStates.ContainsKey(manager2.Id))
                {
                    Session.GetHabbo().QuestsStates.Add(manager2.Id, 0);
                    Habbo habbo = Session.GetHabbo();
                    object questsS = habbo.QuestsS;
                    habbo.QuestsS = string.Concat(new object[] { questsS, manager2.Id, "-", 0, ";" });
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET queststates = '", Session.GetHabbo().QuestsS, "' WHERE id = '", Session.GetHabbo().Id, "'" }));
                    }
                }
                manager2.UsersId.Add(Session.GetHabbo().Id);
                manager2.UsersActivate = manager2.UsersActivate + Session.GetHabbo().Id + ";";
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE quests SET usersactivate = '", manager2.UsersActivate, "' WHERE id = '", manager2.Id, "'" }));
                }
                this.LoadQuests(Session);
                ServerMessage message = new ServerMessage(Outgoing.ActivateQuest);
                message.AppendString(manager2.Category);
                message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().MyQuestsCompleted(manager2.Category, Session.GetHabbo()).Count);
                message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().QuestsInCategory(manager2.Category).Count);
                message.AppendInt32(-1);
                message.AppendInt32(manager2.Id);
                message.AppendBoolean(ButterflyEnvironment.GetGame().GetQuestManager().IsActivated(manager2.Id, Session.GetHabbo()));
                message.AppendString(manager2.Type);
                message.AppendString(manager2.ExtraValue);
                message.AppendInt32(manager2.Award);
                message.AppendString(manager2.SecondType);
                if (Session.GetHabbo().QuestsStates.ContainsKey(manager2.Id))
                {
                    message.AppendInt32(Session.GetHabbo().QuestsStates[manager2.Id]);
                }
                else
                {
                    message.AppendInt32(0);
                }
                message.AppendInt32(manager2.CountFurnis);
                message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().GetIntValue(manager2.Category));
                message.AppendString("set_kuurna");
                message.AppendString("MAIN_CHAIN");
                message.AppendBoolean(true);
                Session.SendMessage(message);
            }
        }

        public void ActiveQuest(GameClient Session, ClientMessage Request)
        {
            this.CancelQuest(Session);
            int id = Request.PopWiredInt32();
            Session.GetHabbo().ActiveQuestNow = true;
            QuestManager questById = ButterflyEnvironment.GetGame().GetQuestManager().GetQuestById(id);
            if (questById != null)
            {
                IQueryAdapter adapter;
                Session.GetHabbo().QuestActivate = questById.Id;
                if (!Session.GetHabbo().QuestsStates.ContainsKey(questById.Id))
                {
                    Session.GetHabbo().QuestsStates.Add(questById.Id, 0);
                    Habbo habbo = Session.GetHabbo();
                    object questsS = habbo.QuestsS;
                    habbo.QuestsS = string.Concat(new object[] { questsS, questById.Id, "-", 0, ";" });
                    using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET queststates = '", Session.GetHabbo().QuestsS, "' WHERE id = '", Session.GetHabbo().Id, "'" }));
                    }
                }
                questById.UsersId.Add(Session.GetHabbo().Id);
                questById.UsersActivate = questById.UsersActivate + Session.GetHabbo().Id + ";";
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE quests SET usersactivate = '", questById.UsersActivate, "' WHERE id = '", questById.Id, "'" }));
                }
                this.LoadQuests(Session);
                ServerMessage message = new ServerMessage(Outgoing.ActivateQuest);
                message.AppendString(questById.Category);
                message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().MyQuestsCompleted(questById.Category, Session.GetHabbo()).Count);
                message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().QuestsInCategory(questById.Category).Count);
                message.AppendInt32(-1);
                message.AppendInt32(questById.Id);
                message.AppendBoolean(ButterflyEnvironment.GetGame().GetQuestManager().IsActivated(questById.Id, Session.GetHabbo()));
                message.AppendString(questById.Type);
                message.AppendString(questById.ExtraValue);
                message.AppendInt32(questById.Award);
                message.AppendString(questById.SecondType);
                if (Session.GetHabbo().QuestsStates.ContainsKey(questById.Id))
                {
                    message.AppendInt32(Session.GetHabbo().QuestsStates[questById.Id]);
                }
                else
                {
                    message.AppendInt32(0);
                }
                message.AppendInt32(questById.CountFurnis);
                message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().GetIntValue(questById.Category));
                message.AppendString("set_kuurna");
                message.AppendString("MAIN_CHAIN");
                message.AppendBoolean(true);
                Session.SendMessage(message);
            }
        }

        public void CancelQuest(GameClient Session)
        {
            QuestManager questById = ButterflyEnvironment.GetGame().GetQuestManager().GetQuestById(Session.GetHabbo().QuestActivate);
            if (questById != null)
            {
                questById.UsersId.Remove(Session.GetHabbo().Id);
                Session.GetHabbo().QuestActivate = 0;
                questById.UsersActivate = questById.UsersActivate.Replace(Session.GetHabbo().Id + ";", "");
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE quests SET usersactivate = '", questById.UsersActivate, "' WHERE id = '", questById.Id, "'" }));
                }
                this.LoadQuests(Session);
                ServerMessage message = new ServerMessage(Outgoing.QuitAlertQ);
                message.AppendBoolean(false);
                Session.SendMessage(message);
            }
        }

        public void CompleteQuest(GameClient Session)
        {
            QuestManager questById = ButterflyEnvironment.GetGame().GetQuestManager().GetQuestById(Session.GetHabbo().QuestActivate);
            if (questById != null)
            {
                IQueryAdapter adapter;
                Session.GetHabbo().QuestsS = Session.GetHabbo().QuestsS.Replace(string.Concat(new object[] { questById.Id, "-", Session.GetHabbo().QuestsStates[questById.Id], ";" }), "");
                Habbo habbo = Session.GetHabbo();
                habbo.MyQuests = habbo.MyQuests + questById.Id + ";";
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET queststates = '", Session.GetHabbo().QuestsS, "', quests = '", Session.GetHabbo().MyQuests, "' WHERE id = '", Session.GetHabbo().Id, "'" }));
                }
                Session.GetHabbo().QuestsStates.Remove(questById.Id);
                Session.GetHabbo().QuestsCompleted.Add(questById.Id);
                Session.GetHabbo().QuestActivate = 0;
                Session.GetHabbo().LastQuestId = questById.Id;
                Session.GetHabbo().ActiveQuestNow = true;
                questById.UsersActivate = questById.UsersActivate.Replace(Session.GetHabbo().Id + ";", "");
                questById.UsersId.Remove(Session.GetHabbo().Id);
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE quests SET usersactivate = '", questById.UsersActivate, "' WHERE id = '", questById.Id, "'" }));
                }
                this.LoadQuests(Session);
            }
        }

        public void LoadQuests(GameClient Session)
        {
            ServerMessage message = new ServerMessage(Outgoing.LoadQuests);
            message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().GetStringTypes().Count);
            foreach (string str in ButterflyEnvironment.GetGame().GetQuestManager().GetStringTypes())
            {
                message.AppendString(str);
                List<QuestManager> questsForType = ButterflyEnvironment.GetGame().GetQuestManager().GetQuestsForType(str);
                message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().MyQuestsCompleted(str, Session.GetHabbo()).Count);
                message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().QuestsInCategory(str).Count);
                if (ButterflyEnvironment.GetGame().GetQuestManager().MyQuestsCompleted(str, Session.GetHabbo()).Count != ButterflyEnvironment.GetGame().GetQuestManager().QuestsInCategory(str).Count)
                {
                    message.AppendInt32(-1);
                    QuestManager myActualQuest = ButterflyEnvironment.GetGame().GetQuestManager().GetMyActualQuest(str, Session.GetHabbo());
                    message.AppendInt32(myActualQuest.Id);
                    message.AppendBoolean(myActualQuest.Id == Session.GetHabbo().QuestActivate);
                    message.AppendString(myActualQuest.Type);
                    message.AppendString(myActualQuest.ExtraValue);
                    message.AppendInt32(myActualQuest.Award);
                    message.AppendString(myActualQuest.SecondType);
                    if (Session.GetHabbo().QuestsStates.ContainsKey(myActualQuest.Id))
                    {
                        message.AppendInt32(Session.GetHabbo().QuestsStates[myActualQuest.Id]);
                    }
                    else
                    {
                        message.AppendInt32(0);
                    }
                    message.AppendInt32(myActualQuest.CountFurnis);
                    message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().GetIntValue(str));
                    message.AppendString("set_kuurna");
                    message.AppendString("MAIN_CHAIN");
                    message.AppendBoolean(true);
                }
                else
                {
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendBoolean(false);
                    message.AppendString("");
                    message.AppendString("");
                    message.AppendInt32(0);
                    message.AppendString("");
                    message.AppendInt32(0);
                    message.AppendInt32(0);
                    message.AppendInt32(3);
                    message.AppendString("");
                    message.AppendString("MAIN_CHAIN");
                    message.AppendBoolean(true);
                }
            }
            if (!Session.GetHabbo().ActiveQuestNow)
            {
                message.AppendBoolean(true);
            }
            else
            {
                message.AppendBoolean(false);
            }
            Session.GetHabbo().ActiveQuestNow = false;
            Session.SendMessage(message);
        }

        public void UpdateState(string QuestType, GameClient Session)
        {
            try
            {
                QuestManager questById = ButterflyEnvironment.GetGame().GetQuestManager().GetQuestById(Session.GetHabbo().QuestActivate);
                if ((questById != null) && (questById.Type == QuestType))
                {
                    Dictionary<int, int> dictionary;
                    int num;
                    (dictionary = Session.GetHabbo().QuestsStates)[num = questById.Id] = dictionary[num] + 1;
                    Session.GetHabbo().QuestsS = Session.GetHabbo().QuestsS.Replace(string.Concat(new object[] { ";", questById.Id, "-", Session.GetHabbo().QuestsStates[questById.Id] - 1, ";" }), string.Concat(new object[] { ";", questById.Id, "-", Session.GetHabbo().QuestsStates[questById.Id], ";" }));
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.runFastQuery(string.Concat(new object[] { "UPDATE users SET queststates = '", Session.GetHabbo().QuestsS, "' WHERE id = '", Session.GetHabbo().Id, "'" }));
                    }
                    ServerMessage message = new ServerMessage(Outgoing.CompleteQuests);
                    message.AppendString(questById.Category);
                    message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().MyQuestsCompleted(questById.Category, Session.GetHabbo()).Count);
                    message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().QuestsInCategory(questById.Category).Count);
                    message.AppendInt32(-1);
                    message.AppendInt32(questById.Id);
                    message.AppendBoolean(ButterflyEnvironment.GetGame().GetQuestManager().IsActivated(questById.Id, Session.GetHabbo()));
                    message.AppendString(questById.Type);
                    message.AppendString(questById.ExtraValue);
                    message.AppendInt32(questById.Award);
                    message.AppendString(questById.SecondType);
                    if (Session.GetHabbo().QuestsStates.ContainsKey(questById.Id))
                    {
                        message.AppendInt32(Session.GetHabbo().QuestsStates[questById.Id]);
                    }
                    else
                    {
                        message.AppendInt32(0);
                    }
                    message.AppendInt32(questById.CountFurnis);
                    message.AppendInt32(ButterflyEnvironment.GetGame().GetQuestManager().GetIntValue(questById.Category));
                    message.AppendString("set_kuurna");
                    message.AppendString("MAIN_CHAIN");
                    message.AppendBoolean(true);
                    message.AppendBoolean(true);
                    Session.SendMessage(message);
                    if (Session.GetHabbo().QuestsStates[questById.Id] == questById.CountFurnis)
                    {
                        this.CompleteQuest(Session);
                    }
                }
            }
            catch
            {
            }
        }

        public void WhenLoadClient(GameClient Session)
        {
            foreach (QuestManager manager in ButterflyEnvironment.GetGame().GetQuestManager().Quests)
            {
                if (manager.UsersId.Contains(Session.GetHabbo().Id))
                {
                    Session.GetHabbo().QuestActivate = manager.Id;
                    break;
                }
            }
        }
    }
}

