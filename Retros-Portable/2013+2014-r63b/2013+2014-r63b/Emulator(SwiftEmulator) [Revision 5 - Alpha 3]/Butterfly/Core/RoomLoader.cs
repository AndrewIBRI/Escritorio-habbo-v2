namespace Butterfly.Core
{
    using Butterfly;
    using System;
    using System.Threading;

    internal class RoomLoader
    {
        private uint roomsToLoad;

        public RoomLoader(uint count)
        {
            this.roomsToLoad = count + 0x183c0;
            new Thread(new ThreadStart(this.Handle)).Start();
        }

        private void Handle()
        {
            for (uint i = 0x183c0; i < this.roomsToLoad; i++)
            {
                try
                {
                    ButterflyEnvironment.GetGame().GetRoomManager().LoadRoom(i);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                }
            }
            Console.WriteLine(this.roomsToLoad + " rooms loaded");
            Thread.Sleep(0x32c8);
            this.Handle();
        }
    }
}

