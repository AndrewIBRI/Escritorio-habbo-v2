using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atom.Messages;
using Atom.HabboHotel.Quests;
using Atom.HabboHotel.GameClients;

namespace Atom.HabboHotel.Quests.Composer
{
    class QuestStartedComposer
    {
        internal static ServerMessage Compose(GameClient Session, Quest Quest)
        {
            ServerMessage Message = new ServerMessage(802);
            QuestListComposer.SerializeQuest(Message, Session, Quest, Quest.Category);
            return Message;
        }
    }
}
