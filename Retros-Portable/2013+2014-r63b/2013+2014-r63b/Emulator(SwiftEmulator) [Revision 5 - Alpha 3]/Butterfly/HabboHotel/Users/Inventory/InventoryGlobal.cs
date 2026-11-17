namespace Butterfly.HabboHotel.Users.Inventory
{
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using System;

    internal class InventoryGlobal
    {
        internal static InventoryComponent GetInventory(uint UserId, GameClient Client, UserData data)
        {
            return new InventoryComponent(UserId, Client, data);
        }

        internal void saveAll()
        {
        }

        private static void WorkItem()
        {
        }
    }
}

