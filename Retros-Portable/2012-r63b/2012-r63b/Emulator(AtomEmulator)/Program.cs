using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Atom.Core;
using System.Net;
using System.Collections.Specialized;
using System.Net.Cache;
using Atom.MainServerConnectionHandeling;
using System.Threading;

namespace Atom
{
    internal class Program
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        public static MainServerConnectionHolder LicHandeler { get; private set; }
        private delegate bool EventHandler(CtrlType sig);

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        [STAThreadAttribute]
        internal static void Main()
        {
            ConsoleWriter.Writer.Init();

            Program.InitEnvironment();
            //LicHandeler = new MainServerConnectionHolder("127.0.0.1", 9001, name, license.data["license.license"]);

            while (true)
            {
                Console.CursorVisible = true;
                if (Logging.DisabledState)
                    Console.WriteLine(" [" + DateTime.Now.ToString("T") + "] Atom Emulator → ");
                ConsoleCommandHandeling.InvokeCommand(Console.ReadLine());
            }
        }

        [MTAThread]
        internal static void InitEnvironment()
        {
            if (!AtomEnvironment.isLive)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = false;
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

                AtomEnvironment.Initialize();
            }
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Logging.DisablePrimaryWriting(true);
            Exception e = (Exception)args.ExceptionObject;
            Logging.LogCriticalException("SYSTEM CRITICAL EXCEPTION: " + e.ToString());
            AtomEnvironment.SendMassMessage("A fatal error crashed the server, server shutting down.");
            AtomEnvironment.PreformShutDown(true);
        }
    }
}
