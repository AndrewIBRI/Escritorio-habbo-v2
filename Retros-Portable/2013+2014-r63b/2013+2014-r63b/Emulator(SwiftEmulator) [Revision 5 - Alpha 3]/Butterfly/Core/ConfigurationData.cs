namespace Butterfly.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    internal class ConfigurationData
    {
        internal Dictionary<string, string> data = new Dictionary<string, string>();
        internal bool fileHasBeenRead = false;

        internal ConfigurationData(string filePath, bool maynotexist = false)
        {
            if (!File.Exists(filePath))
            {
                if (!maynotexist)
                {
                    throw new ArgumentException("Unable to locate configuration file at '" + filePath + "'.");
                }
            }
            else
            {
                this.fileHasBeenRead = true;
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string str = "";
                        while ((str = reader.ReadLine()) != null)
                        {
                            if ((str.Length >= 1) && !str.StartsWith("#"))
                            {
                                int index = str.IndexOf('=');
                                if (index != -1)
                                {
                                    string key = str.Substring(0, index);
                                    string str3 = str.Substring(index + 1);
                                    this.data.Add(key, str3);
                                }
                            }
                        }
                        reader.Close();
                    }
                }
                catch (Exception exception)
                {
                    throw new ArgumentException("Could not process configuration file: " + exception.Message);
                }
            }
        }
    }
}

