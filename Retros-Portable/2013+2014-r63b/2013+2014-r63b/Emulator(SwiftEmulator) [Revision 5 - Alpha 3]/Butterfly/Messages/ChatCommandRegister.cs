namespace Butterfly.Messages
{
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Misc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    internal class ChatCommandRegister
    {
        private static Dictionary<int, string> commandRegister;
        private static Dictionary<string, ChatCommand> commandRegisterInvokeable;

        internal static string GenerateCommandList(GameClient client)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ChatCommand command in commandRegisterInvokeable.Values)
            {
                if ((command.commandID != 400) && command.UserGotAuthorization(client))
                {
                    builder.Append(command.input + " " + command.prefix + " - " + command.description + "\r");
                }
            }
            return builder.ToString();
        }

        internal static ChatCommand GetCommand(string command)
        {
            return commandRegisterInvokeable[command];
        }

        internal static void Init()
        {
            commandRegister = new Dictionary<int, string>();
            commandRegisterInvokeable = new Dictionary<string, ChatCommand>();
            InitCommandRegister();
            InitInvokeableRegister();
        }

        private static void InitCommandRegister()
        {
            commandRegister = IniReader.ReadFileWithInt(Path.Combine(Application.StartupPath, "System/commands_register.ini"));
        }

        private static void InitInvokeableRegister()
        {
            Dictionary<string, string> dictionary = IniReader.ReadFile(Path.Combine(Application.StartupPath, "System/commands.ini"));
            foreach (KeyValuePair<int, string> pair in commandRegister)
            {
                int key = pair.Key;
                string str = pair.Value;
                int pRank = int.Parse(dictionary[str + ".minrank"]);
                string pDescription = dictionary[str + ".description"];
                string[] strArray = dictionary[str + ".input"].ToLower().Split(new char[] { ',' });
                string pPrefix = dictionary[str + ".prefix"];
                string[] clubsAllowed = dictionary[str + ".clubs"].ToLower().Split(new char[] { ',' });
                foreach (string str4 in strArray)
                {
                    commandRegisterInvokeable.Add(str4, new ChatCommand(key, str4, pRank, pDescription, pPrefix, clubsAllowed));
                }
            }
        }

        internal static void InvokeCommand(ChatCommandHandler commandHandler, int commandID)
        {
            switch (commandID)
            {
                case 1:
                    commandHandler.pickall();
                    break;

                case 2:
                    commandHandler.setspeed();
                    break;

                case 3:
                    commandHandler.unload();
                    break;

                case 4:
                    commandHandler.disablediagonal();
                    break;

                case 5:
                    commandHandler.setmax();
                    break;

                case 6:
                    commandHandler.overridee();
                    break;

                case 7:
                    commandHandler.teleport();
                    break;

                case 8:
                    ChatCommandHandler.catarefresh();
                    break;

                case 10:
                    commandHandler.roomalert();
                    break;

                case 11:
                    commandHandler.coords();
                    break;

                case 12:
                    commandHandler.coins();
                    break;

                case 13:
                    commandHandler.pixels();
                    break;

                case 14:
                    commandHandler.handitem();
                    break;

                case 15:
                    commandHandler.hotelalert();
                    break;

                case 0x10:
                    commandHandler.freeze();
                    break;

                case 0x11:
                    commandHandler.buyx();
                    break;

                case 0x12:
                    commandHandler.enable();
                    break;

                case 0x13:
                    commandHandler.roommute();
                    break;

                case 20:
                    commandHandler.masscredits();
                    break;

                case 0x15:
                    commandHandler.globalcredits();
                    break;

                case 0x16:
                    commandHandler.openroom();
                    break;

                case 0x17:
                    commandHandler.roombadge();
                    break;

                case 0x18:
                    commandHandler.massbadge();
                    break;

                case 0x19:
                    commandHandler.language();
                    break;

                case 0x1a:
                    commandHandler.userinfo();
                    break;

                case 0x1b:
                    commandHandler.linkAlert();
                    break;

                case 0x1c:
                    commandHandler.shutdown();
                    break;

                case 0x1d:
                    commandHandler.dumpmaps();
                    break;

                case 30:
                    commandHandler.giveBadge();
                    break;

                case 0x1f:
                    commandHandler.invisible();
                    break;

                case 0x20:
                    commandHandler.ban();
                    break;

                case 0x21:
                    commandHandler.disconnect();
                    break;

                case 0x22:
                    commandHandler.superban();
                    break;

                case 0x23:
                    commandHandler.langban();
                    break;

                case 0x24:
                    commandHandler.roomkick();
                    break;

                case 0x25:
                    commandHandler.mute();
                    break;

                case 0x26:
                    commandHandler.unmute();
                    break;

                case 0x27:
                    commandHandler.alert();
                    break;

                case 40:
                    commandHandler.kick();
                    break;

                case 0x29:
                    commandHandler.commands();
                    break;

                case 0x2a:
                    commandHandler.faq();
                    break;

                case 0x2b:
                    commandHandler.info();
                    break;

                case 0x2c:
                    commandHandler.enablestatus();
                    break;

                case 0x2d:
                    commandHandler.disablefriends();
                    break;

                case 0x2e:
                    commandHandler.enablefriends();
                    break;

                case 0x2f:
                    commandHandler.disabletrade();
                    break;

                case 0x30:
                    commandHandler.enabletrade();
                    break;

                case 0x31:
                    commandHandler.mordi();
                    break;

                case 50:
                    commandHandler.wheresmypet();
                    break;

                case 0x33:
                    commandHandler.powerlevels();
                    break;

                case 0x34:
                    commandHandler.forcerot();
                    break;

                case 0x35:
                    commandHandler.seteffect();
                    break;

                case 0x36:
                    commandHandler.empty();
                    break;

                case 0x37:
                    commandHandler.whosonline();
                    break;

                case 0x38:
                    commandHandler.stalk();
                    break;

                case 0x39:
                    commandHandler.registerIRC();
                    break;

                case 0x3a:
                    commandHandler.unbanUser();
                    break;

                case 0x3b:
                    commandHandler.giveCrystals();
                    break;

                case 60:
                    commandHandler.warp();
                    break;

                case 0x3d:
                    commandHandler.deleteMission();
                    break;

                case 0x3f:
                    commandHandler.come();
                    break;

                case 0x40:
                    commandHandler.moonwalk();
                    break;

                case 0x41:
                    commandHandler.push();
                    break;

                case 0x42:
                    commandHandler.pull();
                    break;

                case 0x43:
                    commandHandler.copylook();
                    break;

                case 0x44:
                    commandHandler.Fly();
                    break;

                case 0x45:
                    commandHandler.sit();
                    break;

                case 70:
                    commandHandler.lay();
                    break;

                case 0x47:
                    commandHandler.nuevaVotacion();
                    break;

                case 0x48:
                    commandHandler.votar();
                    break;

                case 0x49:
                    commandHandler.FinVotacion();
                    break;

                case 0x4a:
                    commandHandler.refreshitems();
                    break;

                case 0x4b:
                    commandHandler.staffmessages();
                    break;

                case 0x4c:
                    commandHandler.givescore();
                    break;

                case 0x4d:
                    commandHandler.startquestion();
                    break;

                case 0x4e:
                    commandHandler.everybodysignnow();
                    break;

                case 0x4f:
                    commandHandler.everybodyactionnow();
                    break;

                case 80:
                    commandHandler.everybodysaysnow();
                    break;

                case 0x51:
                    commandHandler.everybodyshoutsnow();
                    break;
                case 199:
                    commandHandler.ejectall();
                    break;
		case 0x52:
                    commandHandler.refreshnavigator();
                    break;
            }
        }

        internal static bool IsChatCommand(string command)
        {
            return commandRegisterInvokeable.ContainsKey(command);
        }
    }
}

