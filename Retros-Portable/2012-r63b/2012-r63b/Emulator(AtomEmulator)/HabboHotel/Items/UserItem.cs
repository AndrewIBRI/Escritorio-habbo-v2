using System;
using Atom.Core;
using Atom.HabboHotel.Catalogs;
using Atom.Messages;

namespace Atom.HabboHotel.Items
{
    class UserItem
    {
        internal UInt32 Id;
        internal UInt32 BaseItem;
        internal string ExtraData;
        private Item mBaseItem;
        internal bool isWallItem;

        internal UserItem(UInt32 Id, UInt32 BaseItem, string ExtraData)
        {
            this.Id = Id;
            this.BaseItem = BaseItem;
            this.ExtraData = ExtraData;
            this.mBaseItem = GetBaseItem();
            if (mBaseItem == null)
            {
                Console.WriteLine("Unknown baseItem ID: " + BaseItem);
                Logging.LogException("Unknown baseItem ID: " + BaseItem);
            }
            this.isWallItem = (mBaseItem.Type == 'i');
        }

        //internal void Serialize(ServerMessage Message)
        //{

        //    Message.AppendUInt(Id);
        //    Message.AppendInt32(0);
        //    if (mBaseItem == null)
        //        Logging.LogException("Unknown base: " + BaseItem);
        //    Message.AppendStringWithBreak(mBaseItem.Type.ToString().ToUpper());
        //    Message.AppendUInt(Id);
        //    Message.AppendInt32(mBaseItem.SpriteId);

        //    if (mBaseItem.Name.Contains("a2"))
        //        Message.AppendInt32(3);
        //    else if (mBaseItem.Name.Contains("wallpaper"))
        //        Message.AppendInt32(2);
        //    else if (mBaseItem.Name.Contains("landscape"))
        //        Message.AppendInt32(4);
        //    else
        //        Message.AppendInt32(0);

        //    Message.AppendStringWithBreak(ExtraData);
        //    Message.AppendBoolean(mBaseItem.AllowRecycle);
        //    Message.AppendBoolean(mBaseItem.AllowTrade);
        //    Message.AppendBoolean(mBaseItem.AllowInventoryStack);
        //    Message.AppendBoolean(Marketplace.CanSellItem(this));
        //    Message.AppendInt32(-1);

        //    if (mBaseItem.Type == 's')
        //    {
        //        Message.AppendStringWithBreak("");
        //        Message.AppendInt32(-1);
        //    }
        //}

        internal void SerializeWall(ServerMessage Message, Boolean Inventory)
        {
            Message.AppendUInt(Id);
            Message.AppendStringWithBreak(mBaseItem.Type.ToString().ToUpper());
            Message.AppendUInt(Id);
            Message.AppendInt32(GetBaseItem().SpriteId);

            if (GetBaseItem().Name.Contains("a2"))
            {
                Message.AppendInt32(3);
            }
            else if (GetBaseItem().Name.Contains("wallpaper"))
            {
                Message.AppendInt32(2);
            }
            else if (GetBaseItem().Name.Contains("landscape"))
            {
                Message.AppendInt32(4);
            }
            else
            {
                Message.AppendInt32(1);
            }
            int result = 0;
            if (this.GetBaseItem().InteractionType == InteractionType.gift && ExtraData.Contains(Convert.ToChar(5).ToString()))
            {
                int color = int.Parse(ExtraData.Split((char)5)[1]);
                int lazo = int.Parse(ExtraData.Split((char)5)[2]);
                result = color * 1000 + lazo;
            }
            Message.AppendInt32(result);
            Message.AppendStringWithBreak(ExtraData);
            Message.AppendBoolean(GetBaseItem().AllowRecycle);
            Message.AppendBoolean(GetBaseItem().AllowTrade);
            Message.AppendBoolean(GetBaseItem().AllowInventoryStack);
            Message.AppendBoolean(Marketplace.CanSellItem(this));
            Message.AppendInt32(-1);
        }

        internal void SerializeFloor(ServerMessage Message, Boolean Inventory)
        {
            Message.AppendUInt(Id);
            Message.AppendStringWithBreak(mBaseItem.Type.ToString().ToUpper());
            Message.AppendUInt(Id);
            Message.AppendInt32(GetBaseItem().SpriteId);
            Message.AppendInt32(1);
            Message.AppendInt32(0); // ?
            Message.AppendStringWithBreak(ExtraData);
            Message.AppendBoolean(GetBaseItem().AllowRecycle);
            Message.AppendBoolean(GetBaseItem().AllowTrade);
            Message.AppendBoolean(GetBaseItem().AllowInventoryStack);
            Message.AppendBoolean(Marketplace.CanSellItem(this));
            Message.AppendInt32(-1);
            Message.AppendStringWithBreak("");
            Message.AppendInt32(0);
        }

        internal Item GetBaseItem()
        {
            return AtomEnvironment.GetGame().GetItemManager().GetItem(BaseItem);
        }
    }
}
