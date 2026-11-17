namespace Butterfly.HabboHotel.Users.Subscriptions
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;

    internal class SubscriptionManager
    {
        private Dictionary<string, Subscription> Subscriptions;
        private uint UserId;

        internal SubscriptionManager(uint userID, UserData userData)
        {
            this.UserId = userID;
            this.Subscriptions = userData.subscriptions;
        }

        internal void AddOrExtendSubscription(string SubscriptionId, int DurationSeconds, GameClient Session)
        {
            IQueryAdapter adapter;
            SubscriptionId = SubscriptionId.ToLower();
            ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_VipClub", 1);
            if (this.Subscriptions.ContainsKey(SubscriptionId))
            {
                Subscription subscription = this.Subscriptions[SubscriptionId];
                if (subscription.IsValid())
                {
                    subscription.ExtendSubscription(DurationSeconds);
                }
                else
                {
                    subscription.SetEndTime(ButterflyEnvironment.GetUnixTimestamp() + DurationSeconds);
                }
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery(string.Concat(new object[] { "UPDATE user_subscriptions SET timestamp_expire = ", subscription.ExpireTime, " WHERE user_id = ", this.UserId, " AND subscription_id = 'club_habbo'" }));
                    adapter.runQuery();
                }
            }
            else
            {
                int unixTimestamp = ButterflyEnvironment.GetUnixTimestamp();
                int timeExpire = ButterflyEnvironment.GetUnixTimestamp() + DurationSeconds;
                Subscription subscription2 = new Subscription(SubscriptionId, timeExpire);
                using (adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery(string.Concat(new object[] { "INSERT INTO user_subscriptions (user_id,subscription_id,timestamp_activated,timestamp_expire) VALUES (", this.UserId, ",'club_habbo',", unixTimestamp, ",", timeExpire, ")" }));
                    adapter.runQuery();
                }
                this.Subscriptions.Add(subscription2.SubscriptionId.ToLower(), subscription2);
            }
        }

        internal void Clear()
        {
            this.Subscriptions.Clear();
        }

        internal Subscription GetSubscription(string SubscriptionId)
        {
            if (this.Subscriptions.ContainsKey(SubscriptionId))
            {
                return this.Subscriptions[SubscriptionId];
            }
            return null;
        }

        internal bool HasSubscription(string SubscriptionId)
        {
            if (!this.Subscriptions.ContainsKey(SubscriptionId))
            {
                return false;
            }
            Subscription subscription = this.Subscriptions[SubscriptionId];
            return subscription.IsValid();
        }

        internal List<string> SubList
        {
            get
            {
                List<string> list = new List<string>();
                foreach (Subscription subscription in this.Subscriptions.Values)
                {
                    list.Add(subscription.SubscriptionId);
                }
                return list;
            }
        }
    }
}

