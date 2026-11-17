using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Plus.Core;
using log4net;
using log4net.Config;
using System.Globalization;
using System.Threading;

namespace Plus
{

    public class Program
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        private static readonly ILog log = LogManager.GetLogger("Plus.Program");

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]

        public static void Main(string[] Args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            // Only run Windows Win32 console adjustments when on Windows NT
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                try
                {
                    DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
                }
                catch { }
            }

            XmlConfigurator.Configure();

            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = false;
            }
            catch { }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;

            PlusEnvironment.Initialize();

            while (true)
            {
                try
                {
                    if (Environment.UserInteractive && !Console.IsInputRedirected)
                    {
                        string input = Console.ReadLine();
                        if (!string.IsNullOrEmpty(input))
                        {
                            string s = input.Split(' ')[0];
                            ConsoleCommands.InvokeCommand(s);
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception) args.ExceptionObject;
            //Logger.LogCriticalException("SYSTEM CRITICAL EXCEPTION: " + e);
            PlusEnvironment.PerformShutDown();
        }

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private delegate bool EventHandler(CtrlType sig);
    }
}