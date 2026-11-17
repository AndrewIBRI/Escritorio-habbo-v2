namespace Butterfly.HabboHotel.Catalogs
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Data;

    internal class VoucherHandler
    {
        internal static int GetVoucherValue(string Code)
        {
            DataRow row;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT value FROM credit_vouchers WHERE code = @code");
                adapter.addParameter("code", Code);
                row = adapter.getRow();
            }
            if (row != null)
            {
                return (int) row[0];
            }
            return 0;
        }

        private static bool IsValidCode(string Code)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT null FROM credit_vouchers WHERE code = @code");
                adapter.addParameter("code", Code);
                if (adapter.getRow() != null)
                {
                    return true;
                }
            }
            return false;
        }

        private static void TryDeleteVoucher(string Code)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("DELETE FROM credit_vouchers WHERE code = @code");
                adapter.addParameter("code", Code);
                adapter.runQuery();
            }
        }

        internal static void TryRedeemVoucher(GameClient Session, string Code)
        {
            if (!IsValidCode(Code))
            {
                ServerMessage message = new ServerMessage(0xd5);
                message.AppendString("1");
                Session.SendMessage(message);
            }
            else
            {
                int voucherValue = GetVoucherValue(Code);
                TryDeleteVoucher(Code);
                Habbo habbo = Session.GetHabbo();
                habbo.Credits += voucherValue;
                Session.GetHabbo().UpdateCreditsBalance();
                Session.SendMessage(new ServerMessage(0xd4));
            }
        }
    }
}

