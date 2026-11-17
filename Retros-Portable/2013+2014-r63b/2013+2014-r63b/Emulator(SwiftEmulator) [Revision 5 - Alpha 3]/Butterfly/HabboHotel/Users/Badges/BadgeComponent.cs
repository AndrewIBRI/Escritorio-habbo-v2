namespace Butterfly.HabboHotel.Users.Badges
{
    using Butterfly;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class BadgeComponent
    {
        private Hashtable Badges = new Hashtable();
        private uint UserId;

        internal BadgeComponent(uint userId, UserData data)
        {
            foreach (Badge badge in data.badges)
            {
                if (!this.Badges.ContainsKey(badge.Code))
                {
                    this.Badges.Add(badge.Code, badge);
                }
            }
            this.UserId = userId;
        }

        internal Badge GetBadge(string Badge)
        {
            if (this.Badges.ContainsKey(Badge))
            {
                return (Badge) this.Badges[Badge];
            }
            return null;
        }

        internal void GiveBadge(string Badge, bool InDatabase)
        {
            this.GiveBadge(Badge, 0, InDatabase);
        }

        internal void GiveBadge(string Badge, int Slot, bool InDatabase)
        {
            if (!this.HasBadge(Badge))
            {
                if (InDatabase)
                {
                    using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        adapter.setQuery(string.Concat(new object[] { "INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES (", this.UserId, ",@badge,", Slot, ")" }));
                        adapter.addParameter("badge", Badge);
                        adapter.runQuery();
                    }
                }
                this.Badges.Add(Badge, new Badge(Badge, Slot));
            }
        }

        internal bool HasBadge(string Badge)
        {
            return this.Badges.ContainsKey(Badge);
        }

        internal bool HasBadgeList(List<string> Badge)
        {
            foreach (string str in Badge)
            {
                if (!this.Badges.ContainsKey(str))
                {
                    return false;
                }
            }
            return true;
        }

        internal void RemoveBadge(string Badge)
        {
            if (this.HasBadge(Badge))
            {
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("DELETE FROM user_badges WHERE badge_id = @badge AND user_id = " + this.UserId + " LIMIT 1");
                    adapter.addParameter("badge", Badge);
                    adapter.runQuery();
                }
                this.Badges.Remove(this.GetBadge(Badge));
            }
        }

        internal void ResetSlots()
        {
            foreach (Badge badge in this.Badges.Values)
            {
                badge.Slot = 0;
            }
        }

        internal ServerMessage Serialize()
        {
            List<Badge> list = new List<Badge>();
            ServerMessage message = new ServerMessage(Outgoing.BadgesInventory);
            message.AppendInt32(this.Count);
            foreach (Badge badge in this.Badges.Values)
            {
                message.AppendInt32(0);
                message.AppendString(badge.Code);
                if (badge.Slot > 0)
                {
                    list.Add(badge);
                }
            }
            message.AppendInt32(list.Count);
            foreach (Badge badge in list)
            {
                message.AppendInt32(badge.Slot);
                message.AppendString(badge.Code);
            }
            return message;
        }

        internal Hashtable BadgeList
        {
            get
            {
                return this.Badges;
            }
        }

        internal int Count
        {
            get
            {
                return this.Badges.Count;
            }
        }

        internal int EquippedCount
        {
            get
            {
                int num = 0;
                foreach (Badge badge in this.Badges.Values)
                {
                    if (badge.Slot > 0)
                    {
                        num++;
                    }
                }
                return num;
            }
        }
    }
}

