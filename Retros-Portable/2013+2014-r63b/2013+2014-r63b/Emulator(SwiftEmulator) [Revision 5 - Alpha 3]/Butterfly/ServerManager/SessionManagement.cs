namespace Butterfly.ServerManager
{
    using Butterfly.Core;
    using System;
    using System.Collections.Generic;

    internal static class SessionManagement
    {
        private static List<Session> mSessions;

        internal static void BroadcastExceptionNotification(ExceptionType type, int token)
        {
            switch (type)
            {
                case ExceptionType.UserException:
                    BroadcastMessage("exception:id=5," + token);
                    break;

                case ExceptionType.SQLException:
                    BroadcastMessage("exception:id=3," + token);
                    break;

                case ExceptionType.FatalException:
                    BroadcastMessage("exception:id=2," + token);
                    break;

                case ExceptionType.ThreadedException:
                    BroadcastMessage("exception:id=4," + token);
                    break;

                case ExceptionType.StandardException:
                    BroadcastMessage("exception:id=1," + token);
                    break;

                case ExceptionType.DDOSException:
                    BroadcastMessage("exception:id=6," + token);
                    break;
            }
        }

        private static void BroadcastMessage(string message)
        {
            try
            {
                foreach (Session session in mSessions)
                {
                    session.SendMessage(message);
                }
            }
            catch
            {
            }
        }

        internal static void IncreaseDisconnection()
        {
            foreach (Session session in mSessions)
            {
                session.Disconnection++;
            }
        }

        internal static void IncreaseError()
        {
            foreach (Session session in mSessions)
            {
                session.DisconnectionError++;
            }
        }

        internal static void Init()
        {
            mSessions = new List<Session>();
        }

        internal static void RegisterSession(Session pSession)
        {
            if (!mSessions.Contains(pSession))
            {
                mSessions.Add(pSession);
            }
        }

        internal static void RemoveSession(Session pSession)
        {
            mSessions.Remove(pSession);
        }
    }
}

