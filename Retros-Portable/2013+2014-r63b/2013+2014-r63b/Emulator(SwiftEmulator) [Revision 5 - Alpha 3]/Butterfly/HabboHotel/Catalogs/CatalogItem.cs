namespace Butterfly.HabboHotel.Catalogs
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Items;
    using Butterfly.Messages;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class CatalogItem
    {
        internal readonly int Amount;
        internal readonly int CreditsCost;
        internal readonly int CrystalCost;
        internal readonly bool HaveOffer;
        internal string ExtraData;
        internal readonly uint Id;
        internal readonly bool IsLimited;
        internal readonly string ItemIdString;
        internal List<uint> Items;
        internal int LimitedSelled;
        internal readonly int LimitedStack;
        internal readonly string Badge;
        internal readonly string Name;
        internal readonly int OudeCredits;
        internal readonly int PageID;
        internal readonly int PixelsCost;
        internal readonly uint songID;

        internal CatalogItem(DataRow Row)
        {
            this.Id = Convert.ToUInt32(Row["id"]);
            this.Name = (string) Row["catalog_name"];
            this.ItemIdString = (string) Row["item_ids"];
            this.Items = new List<uint>();
            if (this.ItemIdString.Contains(";"))
            {
                string[] strArray = this.ItemIdString.Split(new char[] { ';' });
                foreach (string str in strArray)
                {
                    if (str != "")
                    {
                        this.Items.Add((uint) int.Parse(str));
                    }
                }
            }
            else
            {
                this.Items.Add((uint) int.Parse(this.ItemIdString));
            }
            this.PageID = (int) Row["page_id"];
            this.CreditsCost = (int) Row["cost_credits"];
            this.PixelsCost = (int) Row["cost_pixels"];
            this.Amount = (int) Row["amount"];
            this.CrystalCost = (int) Row["cost_crystal"];
            this.OudeCredits = (int) Row["cost_oude_belcredits"];
            this.songID = Convert.ToUInt32(Row["song_id"]);
            this.LimitedSelled = (int) Row["limited_sells"];
            this.LimitedStack = (int) Row["limited_stack"];
            this.IsLimited = this.LimitedStack > 0;
            this.HaveOffer = ((string) Row["offer_active"]) == "1";
            this.Badge = (string)Row["badge"];
            this.ExtraData = (string)Row["extradata"];
        }

        internal Item GetBaseItem(uint ItemIds)
        {
            Item item = ButterflyEnvironment.GetGame().GetItemManager().GetItem(ItemIds);
            if (item == null)
            {
            }
            return item;
        }

        internal void Serialize(ServerMessage Message)
        {
            try
            {
                if (this.PageID != 5)
                {
                    Message.AppendInt32(this.Id);
                    Message.AppendString(this.Name);
                    Message.AppendInt32(this.CreditsCost);
                    if (this.CrystalCost > 0)
                    {
                        Message.AppendInt32(CrystalCost);
                        Message.AppendInt32(105);
                    }
                    else
                    {
                        Message.AppendInt32(PixelsCost);
                        Message.AppendInt32(0); //
                    }
                    Message.AppendBoolean(true);//false sul pac   true
                    if (Badge == "")
                    {
                        Message.AppendInt32(Items.Count);
                    }
                    else
                    {
                        Message.AppendInt32(Items.Count + 1);
                        Message.AppendString("b");
                        Message.AppendString(Badge);
                    }
                    foreach (uint num in this.Items)
                    {
                        Message.AppendString(this.GetBaseItem(num).Type.ToString());
                        Message.AppendInt32(this.GetBaseItem(num).SpriteId);
                        if ((this.Name.Contains("wallpaper_single") || this.Name.Contains("floor_single")) || this.Name.Contains("landscape_single"))
                        {
                            string[] strArray = this.Name.Split(new char[] { '_' });
                            Message.AppendString(strArray[2]);
                        }
                        else if (Name.Contains("rentable_bot"))
                        {
                            Message.AppendString("hr-3020-34.hd-3091-2.ch-225-92.lg-3058-100.sh-3089-1338.ca-3084-78-108.wa-2005");
                        }
                        else if ((this.songID > 0) && (this.GetBaseItem(num).InteractionType == InteractionType.musicdisc))
                        {
                            Message.AppendString(this.songID.ToString());
                        }
                        else
                        {
                            Message.AppendString(string.Empty);
                        }
                        Message.AppendInt32(this.Amount);
                        if (!this.IsLimited)
                            Message.AppendInt32(0);//0 sul pac   -1 (1 se limitato)
                    }
                    Message.AppendBoolean(this.IsLimited);
                    if (this.IsLimited)
                    {
                        Message.AppendInt32(this.LimitedStack);
                        Message.AppendInt32((int)(this.LimitedStack - this.LimitedSelled));
                        Message.AppendInt32(0);
                    }
                    //Message.AppendBoolean(true);
                    //Message.AppendInt32(0);
                    Message.AppendBoolean(!this.IsLimited && this.HaveOffer);
                }
            }
            catch
            {
                Console.WriteLine(string.Concat(new object[] { "Unable to load furniture item ", this.Id, ": ", this.Name }));
            }
        }

        internal void SerializeClub(ServerMessage Message, GameClient Session)
        {
            try
            {
                Message.AppendInt32(this.Id);
                Message.AppendString(this.Name);
                Message.AppendInt32(this.CreditsCost);
                Message.AppendBoolean(false);
                int i = 0;
                int num2 = 0;
                if (this.Name.Contains("HABBO_CLUB_VIP_"))
                {
                    if (this.Name.Contains("_DAY"))
                    {
                        i = int.Parse(this.Name.Split(new char[] { '_' })[3]);
                    }
                    else if (this.Name.Contains("_MONTH"))
                    {
                        num2 = int.Parse(this.Name.Split(new char[] { '_' })[3]);
                        i = 0x1f * num2;
                    }
                }
                else if (this.Name.Equals("deal_vip_1_year_and_badge"))
                {
                    num2 = 12;
                    i = 0x1f * num2;
                }
                else if (this.Name.Equals("HABBO_CLUB_VIP_5_YEAR"))
                {
                    num2 = 60;
                    i = 0x1f * num2;
                }
                DateTime now = DateTime.Now;
                if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("club_habbo"))
                {
                    double expireTime = Session.GetHabbo().GetSubscriptionManager().GetSubscription("club_habbo").ExpireTime;
                    double num4 = expireTime - ButterflyEnvironment.GetUnixTimestamp();
                    int num5 = (int) Math.Ceiling((double) (num4 / 86400.0));
                    now = DateTime.Now.AddDays((double) num5);
                }
                now = now.AddDays((double) i);
                Message.AppendInt32(2);//0
                Message.AppendInt32(num2);
                Message.AppendInt32(num2);
                Message.AppendInt32(i);
                Message.AppendInt32(i);
                Message.AppendInt32(now.Year);
                Message.AppendInt32(now.Month);
                Message.AppendInt32(now.Day);
            }
            catch
            {
            }
        }
    }
}

