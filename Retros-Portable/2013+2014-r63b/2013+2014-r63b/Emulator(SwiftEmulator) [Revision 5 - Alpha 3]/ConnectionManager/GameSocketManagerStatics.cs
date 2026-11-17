namespace ConnectionManager
{
    using System;

    public class GameSocketManagerStatics
    {
        public static readonly int BUFFER_SIZE = 0x400;
        public static readonly int MAX_PACKET_SIZE = (BUFFER_SIZE - 4);
    }
}

