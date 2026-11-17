namespace Butterfly.HabboHotel.Items
{
    using Butterfly;
    using Butterfly.HabboHotel.Rooms;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Data;

    internal static class TeleHandler
    {
        internal static uint GetLinkedTele(uint TeleId, Room pRoom)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT tele_two_id FROM items_tele_links WHERE tele_one_id = " + TeleId);
                DataRow row = adapter.getRow();
                if (row == null)
                {
                    return 0;
                }
                return Convert.ToUInt32(row[0]);
            }
        }

        internal static uint GetTeleRoomId(uint TeleId, Room pRoom)
        {
            if (pRoom.GetRoomItemHandler().GetItem(TeleId) != null)
            {
                return pRoom.RoomId;
            }
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT room_id FROM items_rooms WHERE item_id = " + TeleId);
                DataRow row = adapter.getRow();
                if (row == null)
                {
                    return 0;
                }
                return Convert.ToUInt32(row[0]);
            }
        }

        internal static bool IsTeleLinked(uint TeleId, Room pRoom)
        {
            uint linkedTele = GetLinkedTele(TeleId, pRoom);
            if (linkedTele == 0)
            {
                return false;
            }
            RoomItem item = pRoom.GetRoomItemHandler().GetItem(linkedTele);
            if (((item == null) || (item.GetBaseItem().InteractionType != InteractionType.teleport)) && (GetTeleRoomId(linkedTele, pRoom) == 0))
            {
                return false;
            }
            return true;
        }
    }
}

