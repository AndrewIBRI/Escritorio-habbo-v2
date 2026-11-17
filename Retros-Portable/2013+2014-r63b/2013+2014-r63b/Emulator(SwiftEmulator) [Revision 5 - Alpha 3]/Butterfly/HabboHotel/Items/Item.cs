namespace Butterfly.HabboHotel.Items
{
    using System;
    using System.Collections.Generic;

    internal class Item
    {
        internal bool AllowGift;
        internal bool AllowGroupItem;
        internal bool AllowInventoryStack;
        internal bool AllowMarketplaceSell;
        internal bool AllowRecycle;
        internal bool AllowTrade;
        internal double Height;
        private uint Id;
        internal Butterfly.HabboHotel.Items.InteractionType InteractionType;
        internal bool IsSeat;
        internal int Length;
        internal int Modes;
        internal string Name;
        internal string PublicName;
        internal int SpriteId;
        internal bool Stackable;
        internal char Type;
        internal List<int> VendingIds;
        internal bool Walkable;
        internal int Width;

        internal Item(uint Id, int Sprite, string PublicName, string Name, string Type, int Width, int Length, double Height, bool Stackable, bool Walkable, bool IsSeat, bool AllowRecycle, bool AllowTrade, bool AllowMarketplaceSell, bool AllowGift, bool AllowInventoryStack, bool AllowGroupItem, Butterfly.HabboHotel.Items.InteractionType InteractionType, int Modes, string VendingIds)
        {
            this.Id = Id;
            this.SpriteId = Sprite;
            this.PublicName = PublicName;
            this.Name = Name;
            this.Type = char.Parse(Type);
            this.Width = Width;
            this.Length = Length;
            this.Height = Height;
            this.Stackable = Stackable;
            this.Walkable = Walkable;
            this.IsSeat = IsSeat;
            this.AllowRecycle = AllowRecycle;
            this.AllowTrade = AllowTrade;
            this.AllowMarketplaceSell = AllowMarketplaceSell;
            this.AllowGift = AllowGift;
            this.AllowInventoryStack = AllowInventoryStack;
            this.InteractionType = InteractionType;
            this.Modes = Modes;
            this.AllowGroupItem = AllowGroupItem;
            this.VendingIds = new List<int>();
            if (VendingIds.Contains(","))
            {
                foreach (string str in VendingIds.Split(new char[] { ',' }))
                {
                    this.VendingIds.Add(int.Parse(str));
                }
            }
            else if (!(VendingIds.Equals("") || (int.Parse(VendingIds) <= 0)))
            {
                this.VendingIds.Add(int.Parse(VendingIds));
            }
        }

        internal uint ItemId
        {
            get
            {
                return this.Id;
            }
        }
    }
}

