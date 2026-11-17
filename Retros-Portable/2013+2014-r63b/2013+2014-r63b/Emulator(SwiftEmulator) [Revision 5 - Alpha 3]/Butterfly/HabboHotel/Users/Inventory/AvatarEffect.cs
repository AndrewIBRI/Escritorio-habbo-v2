namespace Butterfly.HabboHotel.Users.Inventory
{
    using Butterfly;
    using System;

    internal class AvatarEffect
    {
        internal bool Activated;
        internal int EffectId;
        internal double StampActivated;
        internal int TotalDuration;

        internal AvatarEffect(int EffectId, int TotalDuration, bool Activated, double ActivateTimestamp)
        {
            this.EffectId = EffectId;
            this.TotalDuration = TotalDuration;
            this.Activated = Activated;
            this.StampActivated = ActivateTimestamp;
        }

        internal void Activate()
        {
            this.Activated = true;
            this.StampActivated = ButterflyEnvironment.GetUnixTimestamp();
        }

        internal bool HasExpired
        {
            get
            {
                if (this.TimeLeft == -1)
                {
                    return false;
                }
                return (this.TimeLeft <= 0);
            }
        }

        internal int TimeLeft
        {
            get
            {
                if (!this.Activated)
                {
                    return -1;
                }
                double num = ButterflyEnvironment.GetUnixTimestamp() - this.StampActivated;
                if (num >= this.TotalDuration)
                {
                    return 0;
                }
                return (this.TotalDuration - ((int) num));
            }
        }
    }
}

