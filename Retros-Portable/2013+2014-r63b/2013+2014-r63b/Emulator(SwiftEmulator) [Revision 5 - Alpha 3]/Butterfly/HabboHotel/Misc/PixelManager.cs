namespace Butterfly.HabboHotel.Misc
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Users;
    using System;

    internal class PixelManager
    {
        private const int RCV_AMOUNT = 10;
        private const int RCV_EVERY_MINS = 30;

        internal static void GivePixels(GameClient Client)
        {
            double unixTimestamp = ButterflyEnvironment.GetUnixTimestamp();
            Client.GetHabbo().LastActivityPointsUpdate = unixTimestamp;
            Habbo habbo = Client.GetHabbo();
            habbo.ActivityPoints += 10;
            Client.GetHabbo().UpdateActivityPointsBalance(0);
        }

        internal static void GivePixels(GameClient Client, int amount)
        {
            double unixTimestamp = ButterflyEnvironment.GetUnixTimestamp();
            Client.GetHabbo().LastActivityPointsUpdate = unixTimestamp;
            Habbo habbo = Client.GetHabbo();
            habbo.ActivityPoints += amount;
            Client.GetHabbo().UpdateActivityPointsBalance(0);
        }

        internal static bool NeedsUpdate(GameClient Client)
        {
            try
            {
                if (Client.GetHabbo() == null)
                {
                    return false;
                }
                double num = (ButterflyEnvironment.GetUnixTimestamp() - Client.GetHabbo().LastActivityPointsUpdate) / 60.0;
                if (num >= 30.0)
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.HandleException(exception, "PixelManager.NeedsUpdate");
            }
            return false;
        }
    }
}

