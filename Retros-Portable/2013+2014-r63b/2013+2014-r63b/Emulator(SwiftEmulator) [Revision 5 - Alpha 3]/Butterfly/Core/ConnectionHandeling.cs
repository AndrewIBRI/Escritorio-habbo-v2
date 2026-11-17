namespace Butterfly.Core
{
    using Butterfly;
    using Butterfly.Net;
    using ConnectionManager;
    using System;
    using System.Collections;

    public class ConnectionHandeling
    {
        private Hashtable liveConnections = new Hashtable();
        private SocketManager manager = new SocketManager();

        public ConnectionHandeling(int port, int maxConnections, int connectionsPerIP, bool enabeNagles)
        {
            this.manager.init(port, maxConnections, connectionsPerIP, new InitialPacketParser(), !enabeNagles);
        }

        internal void CloseConnection(int p)
        {
            try
            {
                object obj2 = this.liveConnections[p];
                if (obj2 != null)
                {
                    ((ConnectionInformation) obj2).Dispose();
                    ButterflyEnvironment.GetGame().GetClientManager().DisposeConnection((uint) p);
                }
            }
            catch (Exception exception)
            {
                Logging.LogException(exception.ToString());
            }
        }

        private void connectionChanged(ConnectionInformation information, ConnectionState state)
        {
            if (state == ConnectionState.closed)
            {
                this.CloseConnection(information.getConnectionID());
                this.liveConnections.Remove(information.getConnectionID());
            }
        }

        internal void Destroy()
        {
            this.manager.destroy();
        }

        internal void init()
        {
            this.manager.connectionEvent += new SocketManager.ConnectionEvent(this.manager_connectionEvent);
        }

        private void manager_connectionEvent(ConnectionInformation connection)
        {
            this.liveConnections.Add(connection.getConnectionID(), connection);
            connection.connectionChanged += new ConnectionInformation.ConnectionChange(this.connectionChanged);
            ButterflyEnvironment.GetGame().GetClientManager().CreateAndStartClient((uint) connection.getConnectionID(), connection);
        }

        internal void Start()
        {
            this.manager.initializeConnectionRequests();
        }
    }
}

