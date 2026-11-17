namespace Butterfly.HabboHotel.Misc
{
    using Butterfly.Core;
    using System;

    internal static class AntiMutant
    {
        internal static bool ValidateLook(string Look, string Gender)
        {
            bool flag = false;
            if (Look.Length < 1)
            {
                return false;
            }
            try
            {
                string[] strArray = Look.Split(new char[] { '.' });
                if (strArray.Length < 4)
                {
                    return false;
                }
                foreach (string str in strArray)
                {
                    string[] strArray2 = str.Split(new char[] { '-' });
                    if (strArray2.Length < 3)
                    {
                        return false;
                    }
                    string str2 = strArray2[0];
                    int num = int.Parse(strArray2[1]);
                    int num2 = int.Parse(strArray2[1]);
                    if ((num <= 0) || (num2 < 0))
                    {
                        return false;
                    }
                    if (str2.Length != 2)
                    {
                        return false;
                    }
                    if (str2 == "hd")
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.HandleException(exception, "AntiMutant.ValidateLook");
                return false;
            }
            if (!(flag && (!(Gender != "M") || !(Gender != "F"))))
            {
                return false;
            }
            return true;
        }
    }
}

