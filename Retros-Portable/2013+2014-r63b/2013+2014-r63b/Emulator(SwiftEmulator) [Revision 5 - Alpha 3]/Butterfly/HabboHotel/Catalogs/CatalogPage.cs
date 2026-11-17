namespace Butterfly.HabboHotel.Catalogs
{
    using Butterfly;
    using Butterfly.Messages;
    using System;
    using System.Collections;

    internal class CatalogPage
    {
        internal string Caption;
        internal bool ClubOnly;
        internal bool ComingSoon;
        internal bool Enabled;
        internal int IconColor;
        internal int IconImage;
        private int Id;
        internal Hashtable Items = new Hashtable();
        internal string Layout;
        internal string LayoutHeadline;
        internal string LayoutSpecial;
        internal string LayoutTeaser;
        internal uint MinRank;
        private ServerMessage mMessage;
        internal int ParentId;
        internal string Text1;
        internal string Text2;
        internal string TextDetails;
        internal string TextTeaser;
        internal bool Visible;

        internal CatalogPage(int Id, int ParentId, string Caption, bool Visible, bool Enabled, bool ComingSoon, uint MinRank, bool ClubOnly, int IconColor, int IconImage, string Layout, string LayoutHeadline, string LayoutTeaser, string LayoutSpecial, string Text1, string Text2, string TextDetails, string TextTeaser, ref Hashtable CataItems)
        {
            this.Id = Id;
            this.ParentId = ParentId;
            this.Caption = Caption;
            this.Visible = Visible;
            this.Enabled = Enabled;
            this.ComingSoon = ComingSoon;
            this.MinRank = MinRank;
            this.ClubOnly = ClubOnly;
            this.IconColor = IconColor;
            this.IconImage = IconImage;
            this.Layout = Layout;
            this.LayoutHeadline = LayoutHeadline;
            this.LayoutTeaser = LayoutTeaser;
            this.LayoutSpecial = LayoutSpecial;
            this.Text1 = Text1;
            this.Text2 = Text2;
            this.TextDetails = TextDetails;
            this.TextTeaser = TextTeaser;
            foreach (CatalogItem item in CataItems.Values)
            {
                if (item.PageID == Id)
                {
                    this.Items.Add(item.Id, item);
                }
            }
        }

        internal CatalogItem GetItem(uint pId)
        {
            if (this.Items.ContainsKey(pId))
            {
                return (CatalogItem) this.Items[pId];
            }
            return null;
        }

        internal void InitMsg()
        {
            this.mMessage = Catalog.SerializePage(this);
        }

        internal void Serialize(int Rank, ServerMessage Message)
        {
            Message.AppendBoolean(this.Visible);
            Message.AppendInt32(this.IconColor);
            Message.AppendInt32(this.IconImage);
            Message.AppendInt32(this.Id);
            Message.AppendString(this.Layout);
            Message.AppendString(this.Caption);
            Message.AppendInt32(ButterflyEnvironment.GetGame().GetCatalog().GetTreeSize(Rank, this.Id));
        }

        internal ServerMessage GetMessage
        {
            get
            {
                return this.mMessage;
            }
        }

        internal int PageId
        {
            get
            {
                return this.Id;
            }
        }
    }
}

