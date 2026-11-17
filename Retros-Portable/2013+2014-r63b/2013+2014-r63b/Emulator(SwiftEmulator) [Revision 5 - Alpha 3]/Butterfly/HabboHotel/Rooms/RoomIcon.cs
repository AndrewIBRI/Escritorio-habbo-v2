namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly.Messages;
    using System;
    using System.Collections.Generic;

    internal class RoomIcon
    {
        internal int BackgroundImage;
        internal int ForegroundImage;
        internal Dictionary<int, int> Items;

        internal RoomIcon(int BackgroundImage, int ForegroundImage, Dictionary<int, int> Items)
        {
            this.BackgroundImage = BackgroundImage;
            this.ForegroundImage = ForegroundImage;
            this.Items = Items;
        }

        internal void Destroy()
        {
            if (this.Items != null)
            {
                this.Items.Clear();
            }
            this.Items = null;
        }

        internal void Serialize(ServerMessage Message)
        {
            Message.AppendInt32(this.BackgroundImage);
            Message.AppendInt32(this.ForegroundImage);
            Message.AppendInt32(this.Items.Count);
            foreach (KeyValuePair<int, int> pair in this.Items)
            {
                Message.AppendInt32(pair.Key);
                Message.AppendInt32(pair.Value);
            }
        }
    }
}

