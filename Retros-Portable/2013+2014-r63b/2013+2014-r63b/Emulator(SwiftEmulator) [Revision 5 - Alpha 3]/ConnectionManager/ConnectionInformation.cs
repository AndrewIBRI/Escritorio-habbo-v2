namespace ConnectionManager
{
    using Butterfly;
    using ConnectionManager.LoggingSystem;
    using ConnectionManager.Messages;
    using SharedPacketLib;
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class ConnectionInformation : IDisposable
    {
        private byte[] buffer;
        private int connectionID;
        private Socket dataSocket;
        public static bool disableReceive = false;
        public static bool disableSend = false;
        private string ip;
        private bool isConnected;
        private SocketManager manager;
        private AsyncCallback sendCallback;

        public event ConnectionChange connectionChanged;

        public ConnectionInformation(Socket dataStream, int connectionID, SocketManager manager, IDataParser parser, string ip)
        {
            this.parser = parser;
            this.buffer = new byte[GameSocketManagerStatics.BUFFER_SIZE];
            this.manager = manager;
            this.dataSocket = dataStream;
            this.dataSocket.SendBufferSize = GameSocketManagerStatics.BUFFER_SIZE;
            this.ip = ip;
            this.connectionID = connectionID;
            this.sendCallback = new AsyncCallback(this.sentData);
            if (this.connectionChanged != null)
            {
                this.connectionChanged(this, ConnectionState.open);
            }
            MessageLoggerManager.AddMessage(null, connectionID, LogState.ConnectionOpen);
        }

        internal void disconnect()
        {
            try
            {
                if (this.isConnected)
                {
                    this.isConnected = false;
                    MessageLoggerManager.AddMessage(null, this.connectionID, LogState.ConnectionClose);
                    try
                    {
                        if ((this.dataSocket != null) && this.dataSocket.Connected)
                        {
                            this.dataSocket.Shutdown(SocketShutdown.Both);
                            this.dataSocket.Close();
                        }
                    }
                    catch
                    {
                    }
                    this.dataSocket.Dispose();
                    this.parser.Dispose();
                    try
                    {
                        if (this.connectionChanged != null)
                        {
                            this.connectionChanged(this, ConnectionState.closed);
                        }
                    }
                    catch
                    {
                    }
                    this.connectionChanged = null;
                }
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            if (this.isConnected)
            {
                if (ButterflyEnvironment.DisconectLog)
                {
                    Console.WriteLine("Disconect on 191 ConnectionInformation.cs");
                }
                this.disconnect();
            }
        }

        public int getConnectionID()
        {
            return this.connectionID;
        }

        public string getIp()
        {
            return this.ip;
        }

        private void handlePacketData(byte[] packet)
        {
            if (this.parser != null)
            {
                this.parser.handlePacketData(packet);
            }
        }

        private void incomingDataPacket(IAsyncResult iAr)
        {
            int num;
            try
            {
                num = this.dataSocket.EndReceive(iAr);
            }
            catch
            {
                if (ButterflyEnvironment.DisconectLog)
                {
                    Console.WriteLine("Disconect on 217 ConnectionInformation.cs");
                }
                this.disconnect();
                return;
            }
            if (num == 0)
            {
                if (ButterflyEnvironment.DisconectLog)
                {
                    Console.WriteLine("Disconect on 222 ConnectionInformation.cs");
                }
                this.disconnect();
            }
            else
            {
                try
                {
                    if (!disableReceive)
                    {
                        byte[] destinationArray = new byte[num];
                        Array.Copy(this.buffer, destinationArray, num);
                        MessageLoggerManager.AddMessage(destinationArray, this.connectionID, LogState.Normal);
                        this.handlePacketData(destinationArray);
                    }
                }
                catch
                {
                    if (ButterflyEnvironment.DisconectLog)
                    {
                        Console.WriteLine("Disconect on 250 ConnectionInformation.cs");
                    }
                    this.disconnect();
                }
                finally
                {
                    try
                    {
                        this.dataSocket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(this.incomingDataPacket), this.dataSocket);
                    }
                    catch
                    {
                        if (ButterflyEnvironment.DisconectLog)
                        {
                            Console.WriteLine("Disconect on 265 ConnectionInformation.cs");
                        }
                        this.disconnect();
                    }
                }
            }
        }

        private void LogMessage(string message)
        {
            try
            {
                FileStream stream = new FileStream("packetlog.txt", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(Environment.NewLine + message);
                stream.Write(bytes, 0, bytes.Length);
                stream.Dispose();
            }
            catch
            {
                Console.WriteLine("UNABLE TO WRITE TO LOGFILE");
            }
        }

        public void sendbData(byte[] packet)
        {
            this.sendData(packet);
        }

        private void sendData(byte[] packet)
        {
            try
            {

                this.SendUnsafeData(packet);
            }
            catch
            {
                if (ButterflyEnvironment.DisconectLog)
                {
                    Console.WriteLine("Disconect on 312 ConnectionInformation.cs");
                }
                this.disconnect();
            }
        }

        public void SendData(byte[] packet)
        {
            this.sendData(packet);
        }

        public void SendMuchData(byte[] packet)
        {
            this.sendData(packet);
        }

        public void SendUnsafeData(byte[] packet)
        {
            if (this.isConnected && !disableSend)
            {
                this.dataSocket.BeginSend(packet, 0, packet.Length, SocketFlags.None, this.sendCallback, null);
            }
        }

        private void sentData(IAsyncResult iAr)
        {
            try
            {
                this.dataSocket.EndSend(iAr);
            }
            catch
            {
                if (ButterflyEnvironment.DisconectLog)
                {
                    Console.WriteLine("Disconect on 337 ConnectionInformation.cs");
                }
                this.disconnect();
            }
        }

        public void startPacketProcessing()
        {
            if (!this.isConnected)
            {
                this.isConnected = true;
                try
                {
                    this.dataSocket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(this.incomingDataPacket), this.dataSocket);
                }
                catch
                {
                    if (ButterflyEnvironment.DisconectLog)
                    {
                        Console.WriteLine("Disconect on 110 ConnectionInformation.cs");
                    }
                    this.disconnect();
                }
            }
        }

        public IDataParser parser { get; set; }

        public delegate void ConnectionChange(ConnectionInformation information, ConnectionState state);
    }
}

