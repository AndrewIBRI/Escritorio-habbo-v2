namespace Butterfly.ServerManager
{
    using Butterfly.Core;
    using System;
    using System.Net.Sockets;
    using System.Text;

    internal class Session
    {
        private bool mClosed;
        private byte[] mDataBuffer;
        private int mDisconnectionErrors;
        private int mDisconnections;
        private string mIP;
        private string mLongIP;
        private AsyncCallback mReceivedCallback;
        private Socket mSock;

        public Session(Socket pSock)
        {
            this.mSock = pSock;
            this.mDataBuffer = new byte[0x400];
            this.mReceivedCallback = new AsyncCallback(this.BytesReceived);
            this.mClosed = false;
            Logging.WriteLine("Received connection");
            this.mIP = this.mSock.RemoteEndPoint.ToString().Split(new char[] { ':' })[0];
            this.mLongIP = pSock.RemoteEndPoint.ToString();
            this.SendData("authreq");
            this.ContinueListening();
        }

        private void BytesReceived(IAsyncResult pIar)
        {
            try
            {
                int length = this.mSock.EndReceive(pIar);
                try
                {
                    byte[] destinationArray = new byte[length];
                    Array.Copy(this.mDataBuffer, destinationArray, length);
                    string[] strArray = Encoding.Default.GetString(this.mDataBuffer, 0, length).Split(new char[] { '|' });
                    foreach (string str in strArray)
                    {
                        if (!string.IsNullOrEmpty(str) && (str.Split(new char[] { ':' })[0].Length == 0))
                        {
                            this.Close();
                            return;
                        }
                    }
                    this.ContinueListening();
                }
                catch
                {
                    this.Close();
                }
            }
            catch
            {
                this.Close();
            }
        }

        internal void Close()
        {
            if (!this.mClosed)
            {
                this.mClosed = true;
                try
                {
                    this.mSock.Close();
                }
                catch
                {
                }
                SessionManagement.RemoveSession(this);
                Logging.WriteLine("Reached end of session");
            }
        }

        private void ContinueListening()
        {
            try
            {
                this.mSock.BeginReceive(this.mDataBuffer, 0, this.mDataBuffer.Length, SocketFlags.None, this.mReceivedCallback, this);
            }
            catch
            {
                this.Close();
            }
        }

        internal bool GetState()
        {
            try
            {
                return this.mSock.Connected;
            }
            catch
            {
                return false;
            }
        }

        private void SendData(string pData)
        {
            try
            {
                byte[] bytes = Encoding.Default.GetBytes(pData + "|");
                this.mSock.Send(bytes);
                Console.WriteLine(pData);
            }
            catch
            {
                this.Close();
            }
        }

        internal void SendMessage(string data)
        {
            
            this.SendData(data);
        }

        internal int Disconnection
        {
            get
            {
                return this.mDisconnections;
            }
            set
            {
                this.mDisconnections = value;
            }
        }

        internal int DisconnectionError
        {
            get
            {
                return this.mDisconnectionErrors;
            }
            set
            {
                this.mDisconnectionErrors = value;
            }
        }

        internal string GetLongIP
        {
            get
            {
                return this.mSock.RemoteEndPoint.ToString();
            }
        }
    }
}

