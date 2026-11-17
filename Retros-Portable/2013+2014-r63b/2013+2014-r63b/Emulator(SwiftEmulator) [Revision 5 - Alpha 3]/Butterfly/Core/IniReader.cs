namespace Butterfly.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class IniReader
    {
        internal static Dictionary<string, string> ReadFile(string path)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] strArray = File.ReadAllLines(path);
            foreach (string str in strArray)
            {
                if ((((str.Length != 0) && str.Contains("=")) && (str.Substring(0, 1) != "#")) && (str.Substring(0, 1) != "["))
                {
                    string[] strArray2 = str.Split(new char[] { '=' });
                    dictionary.Add(strArray2[0], strArray2[1]);
                }
            }
            return dictionary;
        }

        internal static Dictionary<int, string> ReadFileWithInt(string path)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            string[] strArray = File.ReadAllLines(path);
            foreach (string str in strArray)
            {
                if ((((str.Length != 0) && str.Contains("=")) && (str.Substring(0, 1) != "#")) && (str.Substring(0, 1) != "["))
                {
                    string[] strArray2 = str.Split(new char[] { '=' });
                    dictionary.Add(int.Parse(strArray2[1]), strArray2[0]);
                }
            }
            return dictionary;
        }
    }
}

