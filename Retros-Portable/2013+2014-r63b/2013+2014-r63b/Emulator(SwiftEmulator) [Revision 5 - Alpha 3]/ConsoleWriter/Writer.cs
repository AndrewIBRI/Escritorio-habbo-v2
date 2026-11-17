namespace ConsoleWriter
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;

    internal class Writer
    {
        private static Queue logQueue;
        private static bool mDisabled = false;

        public static void DisablePrimaryWriting(bool ClearConsole)
        {
            mDisabled = true;
            if (ClearConsole)
            {
                Console.Clear();
            }
        }

        public static void HandleException(Exception pException, string pLocation)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Exception logged " + DateTime.Now.ToString() + " in " + pLocation + ":");
            builder.AppendLine(pException.ToString());
            if (pException.InnerException != null)
            {
                builder.AppendLine("Inner exception:");
                builder.AppendLine(pException.InnerException.ToString());
            }
            if (pException.HelpLink != null)
            {
                builder.AppendLine("Help link:");
                builder.AppendLine(pException.HelpLink);
            }
            if (pException.Source != null)
            {
                builder.AppendLine("Source:");
                builder.AppendLine(pException.Source);
            }
            if (pException.Data != null)
            {
                builder.AppendLine("Data:");
                foreach (DictionaryEntry entry in pException.Data)
                {
                    builder.AppendLine(string.Concat(new object[] { "  Key: ", entry.Key, "Value: ", entry.Value }));
                }
            }
            if (pException.Message != null)
            {
                builder.AppendLine("Message:");
                builder.AppendLine(pException.Message);
            }
            if (pException.StackTrace != null)
            {
                builder.AppendLine("Stack trace:");
                builder.AppendLine(pException.StackTrace);
            }
            builder.AppendLine();
            builder.AppendLine();
            LogException(builder.ToString());
        }

        public static void Init()
        {
            logQueue = new Queue();
        }

        public static void LogCacheError(string logText)
        {
            WriteToFile(@"Logs\cacheerror.txt", logText + "\r\n\r\n");
            WriteLine("Critical error saved");
        }

        public static void LogCriticalException(string logText)
        {
            WriteToFile(@"Logs\criticalexceptions.txt", logText + "\r\n\r\n");
            WriteLine("CRITICAL ERROR LOGGED");
        }

        public static void LogDDOS(string logText)
        {
            WriteToFile(@"Logs\ddos.txt", logText + "\r\n\r\n");
            WriteLine(logText);
        }

        public static void LogException(string logText)
        {
            WriteToFile(@"Logs\exceptions.txt", logText + "\r\n\r\n");
            WriteLine("Exception has been saved");
        }

        public static void LogMessage(string logText)
        {
            WriteToFile(@"Logs\logg.txt", logText + "\r\n\r\n");
            WriteLine(logText);
        }

        public static void LogPacketException(string Packet, string Exception)
        {
            WriteToFile(@"Logs\packeterror.txt", "Error in packet " + Packet + ": \r\n" + Exception + "\r\n\r\n");
            WriteLine("User disconnection logged");
        }

        public static void LogQueryError(Exception Exception, string query)
        {
            WriteToFile(@"Logs\MySQLerrors.txt", string.Concat(new object[] { "Error in query: \r\n", query, "\r\n", Exception, "\r\n\r\n" }));
            WriteLine("Error in query caught");
        }

        public static void LogShutdown(StringBuilder builder)
        {
            WriteToFile(@"Logs\shutdownlog.txt", builder.ToString());
        }

        public static void LogShutdownError(string p)
        {
        }

        public static void LogThreadException(string Exception, string Threadname)
        {
            WriteToFile(@"Logs\threaderror.txt", "Error in thread " + Threadname + ": \r\n" + Exception + "\r\n\r\n");
            WriteLine("Error in " + Threadname + " caught");
        }

        private static void WriteCallback(IAsyncResult callback)
        {
            FileStream asyncState = (FileStream) callback.AsyncState;
            asyncState.EndWrite(callback);
            asyncState.Dispose();
        }

        private static void WriteFileContent(string message, string location)
        {
            FileStream state = new FileStream(location, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 0x1000, true);
            byte[] bytes = Encoding.ASCII.GetBytes(Environment.NewLine + message);
            IAsyncResult result = state.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback(Writer.WriteCallback), state);
        }

        public static void WriteLine(string Line)
        {
            if (!mDisabled)
            {
                Console.WriteLine(Line);
            }
        }

        private static void WriteToFile(string path, string content)
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(Environment.NewLine + content);
                stream.Write(bytes, 0, bytes.Length);
                stream.Dispose();
            }
            catch (Exception exception)
            {
                WriteLine("Could not write to file: " + exception.ToString() + ":" + content);
            }
        }

        private static void WriteToLogFile()
        {
            while (true)
            {
                try
                {
                    Dictionary<string, StringBuilder> dictionary = new Dictionary<string, StringBuilder>();
                    while (logQueue.Count > 0)
                    {
                        ConsoleWriter.LogMessage message = null;
                        lock (logQueue.SyncRoot)
                        {
                            if (logQueue.Count > 0)
                            {
                                message = (ConsoleWriter.LogMessage) logQueue.Dequeue();
                            }
                        }
                        if (message != null)
                        {
                            if (dictionary.ContainsKey(message.location))
                            {
                                dictionary[message.location].Append(message.message);
                            }
                            else
                            {
                                dictionary.Add(message.location, new StringBuilder(message.message));
                            }
                        }
                    }
                    foreach (KeyValuePair<string, StringBuilder> pair in dictionary)
                    {
                        string introduced8 = pair.Value.ToString();
                        WriteFileContent(introduced8, pair.Key);
                    }
                    dictionary.Clear();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error in console writer: " + exception.ToString());
                }
                Thread.Sleep(0xbb8);
            }
        }

        public static bool DisabledState
        {
            get
            {
                return mDisabled;
            }
            set
            {
                mDisabled = value;
            }
        }
    }
}

