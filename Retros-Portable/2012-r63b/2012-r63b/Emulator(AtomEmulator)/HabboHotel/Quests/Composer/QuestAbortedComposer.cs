using Atom.Messages;

namespace Atom.HabboHotel.Quests.Composer
{
    class QuestAbortedComposer
    {
        internal static ServerMessage Compose()
        {
            return new ServerMessage(803);
        }
    }
}
