using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Messages;
using Atom.HabboHotel.GameClients;
using Atom.HabboHotel.Quests;

namespace Atom.HabboHotel.Quests.Composer
{
    class QuestCompletedComposer
    {
        internal static ServerMessage Compose(GameClient Session, Quest Quest)
        {
            ServerMessage Message = new ServerMessage(801);
            QuestListComposer.SerializeQuest(Message, Session, Quest, Quest.Category);
            return Message;
        }
    }
}
