namespace Butterfly.HabboHotel.Catalogs
{
    using Butterfly;
    using Butterfly.HabboHotel.Items;
    using System;

    internal class EcotronReward
    {
        internal uint BaseId;
        internal uint DisplayId;
        internal uint RewardLevel;

        internal EcotronReward(uint DisplayId, uint BaseId, uint RewardLevel)
        {
            this.DisplayId = DisplayId;
            this.BaseId = BaseId;
            this.RewardLevel = RewardLevel;
        }

        internal Item GetBaseItem()
        {
            return ButterflyEnvironment.GetGame().GetItemManager().GetItem(this.BaseId);
        }
    }
}

