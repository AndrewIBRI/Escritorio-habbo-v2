namespace Butterfly.Core
{
    using Butterfly;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Windows.Forms;
    using Database_Manager.Database;

    internal class LanguageLocale
    {
        private static List<string> swearwords;
        private static Dictionary<string, string> values;
        internal static string welcomeAlert;
        internal static bool welcomeAlertEnabled;

        internal static string FilterSwearwords(string original)
        {
            foreach (string str in swearwords)
            {
                original = ReplaceEx(original, str, "*******");
            }
            return original;
        }

        internal static string GetValue(string value)
        {
            if (!values.ContainsKey(value))
            {
                throw new MissingLocaleException("Missing language locale for [" + value + "]");
            }
            return values[value];
        }

        internal static void Init()
        {
            values = IniReader.ReadFile(Path.Combine(Application.StartupPath, "System/locale.ini"));
            InitWelcomeMessage();
        }

        internal static void InitSwearWord()
        {
            DataTable table;
            swearwords = new List<string>();
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT word FROM room_swearword_filter");
                table = adapter.getTable();
            }
            foreach (DataRow row in table.Rows)
            {
                string item = (string) row[0];
                swearwords.Add(item);
            }
        }

        private static void InitWelcomeMessage()
        {
            welcomeAlertEnabled = IniReader.ReadFile(Path.Combine(Application.StartupPath, "System/welcome_config.ini"))["welcome.alert.enabled"] == "true";
            if (welcomeAlertEnabled)
            {
                welcomeAlert = File.ReadAllText(Path.Combine(Application.StartupPath, "System/welcome_message.ini"));
            }
        }

        private static string ReplaceEx(string original, string pattern, string replacement)
        {
            int num2;
            int num3;
            int num5;
            int index = num2 = num3 = 0;
            string str = original.ToUpper();
            string str2 = pattern.ToUpper();
            int num4 = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            char[] chArray = new char[original.Length + Math.Max(0, num4)];
            while ((num3 = str.IndexOf(str2, num2)) != -1)
            {
                num5 = num2;
                while (num5 < num3)
                {
                    index++;
                    chArray[index] = original[num5];
                    num5++;
                }
                num5 = 0;
                while (num5 < replacement.Length)
                {
                    index++;
                    chArray[index] = replacement[num5];
                    num5++;
                }
                num2 = num3 + pattern.Length;
            }
            if (num2 == 0)
            {
                return original;
            }
            for (num5 = num2; num5 < original.Length; num5++)
            {
                index++;
                chArray[index] = original[num5];
            }
            return new string(chArray, 0, index);
        }
    }
}

