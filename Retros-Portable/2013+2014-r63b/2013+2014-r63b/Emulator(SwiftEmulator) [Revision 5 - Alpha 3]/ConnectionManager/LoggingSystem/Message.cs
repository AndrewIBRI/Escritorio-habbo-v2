namespace ConnectionManager.LoggingSystem
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct Message
    {
        private readonly int connectionID;
        private readonly int timeStamp;
        private readonly string data;
        public Message(int connectionID, int timeStamp, string data)
        {
            this.connectionID = connectionID;
            this.timeStamp = timeStamp;
            this.data = data;
        }

        internal int ConnectionID
        {
            get
            {
                return this.connectionID;
            }
        }
        internal int GetTimestamp
        {
            get
            {
                return this.timeStamp;
            }
        }
        internal string GetData
        {
            get
            {
                return this.data;
            }
        }
    }
}

