namespace Butterfly.HabboHotel.Support
{
    using System;

    internal class HelpCategory
    {
        internal string Caption;
        private uint Id;

        internal HelpCategory(uint Id, string Caption)
        {
            this.Id = Id;
            this.Caption = Caption;
        }

        internal uint CategoryId
        {
            get
            {
                return this.Id;
            }
        }
    }
}

