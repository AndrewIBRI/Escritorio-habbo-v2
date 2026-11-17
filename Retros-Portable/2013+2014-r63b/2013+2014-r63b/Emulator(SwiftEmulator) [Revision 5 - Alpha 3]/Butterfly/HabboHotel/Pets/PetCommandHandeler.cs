namespace Butterfly.HabboHotel.Pets
{
    using Butterfly.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    internal class PetCommandHandeler
    {
        private static Dictionary<int, string> commandRegister;
        private static Dictionary<string, PetCommand> petCommands;

        internal static void Init()
        {
            commandRegister = IniReader.ReadFileWithInt(Path.Combine(Application.StartupPath, "System/commands_register.pets.ini"));
            petCommands = new Dictionary<string, PetCommand>();
            InitCommands();
        }

        private static void InitCommands()
        {
            Dictionary<string, string> dictionary = IniReader.ReadFile(Path.Combine(Application.StartupPath, "System/commands.pets.ini"));
            foreach (KeyValuePair<int, string> pair in commandRegister)
            {
                int key = pair.Key;
                string str = pair.Value;
                string str2 = dictionary[str + ".input"];
                petCommands.Add(str2, new PetCommand(key, str2));
            }
        }

        internal static int TryInvoke(string input)
        {
            PetCommand command;
            if (petCommands.TryGetValue(input, out command))
            {
                return command.commandID;
            }
            return 0;
        }
    }
}

