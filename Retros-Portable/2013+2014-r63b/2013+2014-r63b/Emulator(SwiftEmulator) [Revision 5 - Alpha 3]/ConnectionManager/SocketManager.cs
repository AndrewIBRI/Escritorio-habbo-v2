namespace ConnectionManager
{
    using ConnectionManager.Socket_Exceptions;
    using SharedPacketLib;
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SocketManager
    {
        private bool acceptConnections;
        private int acceptedConnections;
        private Hashtable activeConnections;
        private Socket connectionListener;
        private bool disableNagleAlgorithm;
        private Hashtable ipConnectionCount;
        private int maximumConnections;
        private int maxIpConnectionCount;
        private IDataParser parser;
        private int portInformation;

        public event ConnectionEvent connectionEvent;

        private void alterIpConnectionCount(string ip, int amount)
        {
        }

        private void c_connectionChanged(ConnectionInformation information, ConnectionState state)
        {
            if (state == ConnectionState.closed)
            {
                this.reportDisconnect(information);
            }
        }

        public void destroy()
        {
            this.acceptConnections = false;
            try
            {
                this.connectionListener.Close();
            }
            catch
            {
            }
            this.connectionListener = null;
        }

        public int getAcceptedConnections()
        {
            return this.acceptedConnections;
        }

        private int getAmountOfConnectionFromIp(string ip)
        {
            return 0;
        }

        public void init(int portID, int maxConnections, int connectionsPerIP, IDataParser parser, bool disableNaglesAlgorithm)
        {
            this.parser = parser;
            this.disableNagleAlgorithm = disableNaglesAlgorithm;
            this.initializeFields();
            this.maximumConnections = maxConnections;
            this.portInformation = portID;
            this.maxIpConnectionCount = connectionsPerIP;
            this.acceptedConnections = 0;
            this.prepareConnectionDetails();
        }

        public void initializeConnectionRequests()
        {
            this.connectionListener.Listen(100);
            this.acceptConnections = true;
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            try
            {
                this.connectionListener.BeginAccept(new AsyncCallback(this.newConnectionRequest), this.connectionListener);
            }
            catch
            {
                this.destroy();
            }
        }

        private void initializeFields()
        {
            this.activeConnections = new Hashtable();
            this.ipConnectionCount = new Hashtable();
        }

        internal bool isConnected()
        {
            return (this.connectionListener != null);
        }

        private void newConnectionRequest(IAsyncResult iAr)
        {
            if ((this.connectionListener != null) && this.acceptConnections)
            {
                try
                {
                    //Console.WriteLine("Connection recieved, lets do this.");
                    Socket dataStream = ((Socket) iAr.AsyncState).EndAccept(iAr);

                    dataStream.NoDelay = this.disableNagleAlgorithm;
                    string ip = dataStream.RemoteEndPoint.ToString().Split(new char[] { ':' })[0];
                    this.acceptedConnections++;
                    ConnectionInformation connection = new ConnectionInformation(dataStream, this.acceptedConnections, this, this.parser.Clone() as IDataParser, ip);
                    this.reportUserLogin(ip);
                    connection.connectionChanged += new ConnectionInformation.ConnectionChange(this.c_connectionChanged);
                    if (this.connectionEvent != null)
                    {
                        this.connectionEvent(connection);
                    }
                }
                catch
                {
                }
                finally
                {
                    this.connectionListener.BeginAccept(new AsyncCallback(this.newConnectionRequest), this.connectionListener);
                }
            }
        }

        private void prepareConnectionDetails()
        {
            this.connectionListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.connectionListener.NoDelay = this.disableNagleAlgorithm;
            try
            {
                this.connectionListener.Bind(new IPEndPoint(IPAddress.Any, this.portInformation));
            }
            catch (SocketException exception)
            {
                throw new SocketInitializationException(exception.Message);
            }
        }

        public void reportDisconnect(ConnectionInformation gameConnection)
        {
            gameConnection.connectionChanged -= new ConnectionInformation.ConnectionChange(this.c_connectionChanged);
            this.reportUserLogout(gameConnection.getIp());
        }

        private void reportUserLogin(string ip)
        {
            this.alterIpConnectionCount(ip, this.getAmountOfConnectionFromIp(ip) + 1);
        }

        private void reportUserLogout(string ip)
        {
            this.alterIpConnectionCount(ip, this.getAmountOfConnectionFromIp(ip) - 1);
        }

        public delegate void ConnectionEvent(ConnectionInformation connection);
    }
}

