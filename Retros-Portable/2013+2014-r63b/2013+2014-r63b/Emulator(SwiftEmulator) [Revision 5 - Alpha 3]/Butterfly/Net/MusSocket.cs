namespace Butterfly.Net
{
    using Butterfly.Core;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    internal class MusSocket
    {
        internal HashSet<string> allowedIps;
        internal Socket msSocket;
        internal string musIp;
        internal int musPort;

        internal MusSocket(string _musIp, int _musPort, string[] _allowedIps, int backlog)
        {
            this.musIp = _musIp;
            this.musPort = _musPort;
            this.allowedIps = new HashSet<string>();
            foreach (string str in _allowedIps)
            {
                this.allowedIps.Add(str);
            }
            try
            {
                this.msSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.msSocket.Bind(new IPEndPoint(IPAddress.Any, this.musPort));
                this.msSocket.Listen(backlog);
                this.msSocket.BeginAccept(new AsyncCallback(this.OnEvent_NewConnection), this.msSocket);
                Butterfly.Core.Logging.WriteLine("MUS socket -> READY!");
            }
            catch (Exception exception)
            {
                throw new ArgumentException("Could not set up MUS socket:\n" + exception.ToString());
            }
        }

        internal void OnEvent_NewConnection(IAsyncResult iAr)
        {
            try
            {
                Socket socket = ((Socket) iAr.AsyncState).EndAccept(iAr);
                string item = socket.RemoteEndPoint.ToString().Split(new char[] { ':' })[0];
                if (this.allowedIps.Contains(item) || (item == "127.0.0.1"))
                {
                    MusConnection connection = new MusConnection(socket);
                }
                else
                {
                    socket.Close();
                }
            }
            catch (Exception)
            {
            }
            this.msSocket.BeginAccept(new AsyncCallback(this.OnEvent_NewConnection), this.msSocket);
        }
    }
}

