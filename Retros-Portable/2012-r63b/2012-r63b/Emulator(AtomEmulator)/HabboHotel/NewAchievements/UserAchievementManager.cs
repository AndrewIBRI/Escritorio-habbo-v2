using System.Collections.Generic;
using Atom.HabboHotel.GameClients;

namespace Atom.HabboHotel.NewAchievements
{
    class UserAchievementManager
    {
        private Dictionary<uint, Achievement> achivements;
        private GameClient client;

        public UserAchievementManager(GameClient client, Dictionary<uint, Achievement> achievements)
        {
            this.client = client;
            this.achivements = achievements;
        }
    }
}
