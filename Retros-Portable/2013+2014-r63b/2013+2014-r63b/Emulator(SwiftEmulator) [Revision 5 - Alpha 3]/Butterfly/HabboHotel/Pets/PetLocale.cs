namespace Butterfly.HabboHotel.Pets
{
    using Butterfly.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    internal class PetLocale
    {
        private static Dictionary<string, string[]> values;

        internal static string[] GetValue(string key)
        {
            string[] strArray;
            if (values.TryGetValue(key, out strArray))
            {
                return strArray;
            }
            return new string[] { ("NO KEY FOUND FOR " + key) };
        }

        internal static void Init()
        {
            Dictionary<string, string> dictionary = IniReader.ReadFile(Path.Combine(Application.StartupPath, "System/locale.pets.ini"));
            values = new Dictionary<string, string[]>();
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                values.Add(pair.Key, pair.Value.Split(new char[] { ',' }));
            }
        }
    }
}

