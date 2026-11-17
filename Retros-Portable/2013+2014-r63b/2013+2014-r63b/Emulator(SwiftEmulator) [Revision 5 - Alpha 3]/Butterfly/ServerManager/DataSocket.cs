namespace Butterfly.ServerManager
{
    using Butterfly.Core;
    using System;
    using System.Net;
    using System.Net.Sockets;

    internal class DataSocket
    {
        private static AsyncCallback mConnectionReqCallback;
        private static Socket mListener;

        private static void ConnectionRequest(IAsyncResult iAr)
        {
            try
            {
                new Session(((Socket) iAr.AsyncState).EndAccept(iAr));
            }
            catch
            {
            }
            WaitForNextConnection();
        }

        internal static void SetupListener(int Port)
        {
            SessionManagement.Init();
            mListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, Port);
            mListener.Bind(localEP);
            mListener.Listen(0x3e8);
            mConnectionReqCallback = new AsyncCallback(DataSocket.ConnectionRequest);
        }

        internal static void Start()
        {
            WaitForNextConnection();
        }

        private static void WaitForNextConnection()
        {
            try
            {
                mListener.BeginAccept(mConnectionReqCallback, mListener);
            }
            catch (Exception exception)
            {
                Butterfly.Core.Logging.HandleException(exception, "DataSocket.WaitForNextConnection");
            }
        }
    }
}

