namespace Butterfly.HabboHotel.Support
{
    using System;

    internal class HelpTopic
    {
        internal string Body;
        internal string Caption;
        internal uint CategoryId;
        private uint Id;

        internal HelpTopic(uint Id, string Caption, string Body, uint CategoryId)
        {
            this.Id = Id;
            this.Caption = Caption;
            this.Body = Body;
            this.CategoryId = CategoryId;
        }

        internal uint TopicId
        {
            get
            {
                return this.Id;
            }
        }
    }
}

