namespace Butterfly.Core
{
    using ConsoleWriter;
    using System;
    using System.Text;

    public static class Logging
    {
        private static int tokenID = 0;

        internal static void DisablePrimaryWriting(bool ClearConsole)
        {
            Writer.DisablePrimaryWriting(ClearConsole);
        }

        internal static void HandleException(Exception pException, string pLocation)
        {
            Writer.HandleException(pException, pLocation);
        }

        internal static void LogCacheError(string logText)
        {
            Writer.LogCacheError("TokenID: " + tokenID + logText);
        }

        internal static void LogCriticalException(string logText)
        {
            Writer.LogCriticalException("TokenID: " + tokenID + logText);
        }

        internal static void LogException(string logText)
        {
            Writer.LogException(string.Concat(new object[] { "TokenID: ", tokenID, Environment.NewLine, logText, Environment.NewLine }));
        }

        internal static void LogMessage(string logText)
        {
            Writer.LogMessage(logText);
        }

        internal static void LogPacketException(string Packet, string Exception)
        {
            Writer.LogPacketException(Packet, "TokenID: " + tokenID + Exception);
        }

        public static void LogQueryError(Exception Exception, string query)
        {
            Writer.LogQueryError(Exception, "TokenID: " + tokenID + query);
        }

        internal static void LogShutdown(StringBuilder builder)
        {
            Writer.LogShutdown(builder);
        }

        internal static void LogThreadException(string Exception, string Threadname)
        {
            Writer.LogThreadException("TokenID: " + tokenID + Exception, Threadname);
        }

        internal static void WriteLine(string Line)
        {
            Writer.WriteLine((Line.Length == 0) ? "" : ("» " + Line));
        }

        internal static bool DisabledState
        {
            get
            {
                return Writer.DisabledState;
            }
            set
            {
                Writer.DisabledState = value;
            }
        }
    }
}

