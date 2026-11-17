namespace Butterfly.HabboHotel.Navigators
{
    using System;

    internal class FlatCat
    {
        internal string Caption;
        internal int Id;
        internal int MinRank;

        internal FlatCat(int Id, string Caption, int MinRank)
        {
            this.Id = Id;
            this.Caption = Caption;
            this.MinRank = MinRank;
        }
    }
}

