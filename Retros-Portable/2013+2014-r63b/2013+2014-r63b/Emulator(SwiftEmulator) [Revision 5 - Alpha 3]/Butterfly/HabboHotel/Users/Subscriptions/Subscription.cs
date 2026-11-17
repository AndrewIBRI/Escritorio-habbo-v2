namespace Butterfly.HabboHotel.Users.Subscriptions
{
    using Butterfly;
    using Butterfly.Core;
    using System;

    internal class Subscription
    {
        private string Caption;
        private int TimeExpire;

        internal Subscription(string Caption, int TimeExpire)
        {
            this.Caption = Caption;
            this.TimeExpire = TimeExpire;
        }

        internal void ExtendSubscription(int Time)
        {
            try
            {
                this.TimeExpire += Time;
            }
            catch (Exception exception)
            {
                Logging.LogException(string.Concat(new object[] { "T: ", this.TimeExpire, ".", Time }));
                throw exception;
            }
        }

        internal bool IsValid()
        {
            if (this.TimeExpire <= ButterflyEnvironment.GetUnixTimestamp())
            {
                return false;
            }
            return true;
        }

        internal void SetEndTime(int time)
        {
            this.TimeExpire = time;
        }

        internal int ExpireTime
        {
            get
            {
                return this.TimeExpire;
            }
        }

        internal string SubscriptionId
        {
            get
            {
                return this.Caption;
            }
        }
    }
}

