namespace Butterfly.HabboHotel.Rooms
{
    using System;

    internal static class ByteToItemEffectEnum
    {
        internal static ItemEffectType Parse(byte pByte)
        {
            switch (pByte)
            {
                case 0:
                    return ItemEffectType.None;

                case 1:
                    return ItemEffectType.Swim;

                case 2:
                    return ItemEffectType.Normalskates;

                case 3:
                    return ItemEffectType.Iceskates;

                case 4:
                    return ItemEffectType.SwimLow;

                case 5:
                    return ItemEffectType.SwimHalloween;

                case 6:
                    return ItemEffectType.PublicPool;
            }
            return ItemEffectType.None;
        }
    }
}

