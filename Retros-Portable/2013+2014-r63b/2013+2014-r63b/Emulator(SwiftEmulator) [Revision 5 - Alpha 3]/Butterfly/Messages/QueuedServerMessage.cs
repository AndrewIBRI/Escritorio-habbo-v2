namespace Butterfly.Messages
{
    using ConnectionManager;
    using System;
    using System.Collections.Generic;

    public class QueuedServerMessage
    {
        private List<byte> packet;
        private ConnectionInformation userConnection;

        public QueuedServerMessage(ConnectionInformation connection)
        {
            this.userConnection = connection;
            this.packet = new List<byte>();
        }

        internal void addBytes(byte[] bytes)
        {
            this.appendBytes(bytes);
        }

        private void appendBytes(byte[] bytes)
        {
            this.packet.AddRange(bytes);
        }

        internal void appendResponse(ServerMessage message)
        {
            this.appendBytes(message.GetBytes());
            //Console.WriteLine("Inviato -> " + message.Id);
        }

        internal void Dispose()
        {
            this.packet.Clear();
            this.userConnection = null;
        }

        internal void sendResponse()
        {
            this.userConnection.SendMuchData(this.packet.ToArray());
            this.Dispose();
        }

        internal byte[] getPacket
        {
            get
            {
                return this.packet.ToArray();
            }
        }
    }
}

