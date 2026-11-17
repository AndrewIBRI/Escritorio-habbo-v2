namespace Butterfly.HabboHotel.Habbo.SoundMachine
{
    using System;

    internal class SongInstance
    {
        private SongItem mDiskItem;
        private Butterfly.HabboHotel.Habbo.SoundMachine.SongData mSongData;

        public SongInstance(SongItem Item, Butterfly.HabboHotel.Habbo.SoundMachine.SongData SongData)
        {
            this.mDiskItem = Item;
            this.mSongData = SongData;
        }

        public SongItem DiskItem
        {
            get
            {
                return this.mDiskItem;
            }
        }

        public Butterfly.HabboHotel.Habbo.SoundMachine.SongData SongData
        {
            get
            {
                return this.mSongData;
            }
        }
    }
}

