namespace Butterfly.HabboHotel.Habbo.SoundMachine
{
    using System;

    internal class SongData
    {
        private string mArtist;
        private int mCreatorId;
        private int mId;
        private string mName;
        private string mSound;

        public SongData(int _Id, string _Name, string _Sound, int _CreatorId, string _Artist)
        {
            this.mId = _Id;
            this.mName = _Name;
            this.mSound = _Sound;
            this.mCreatorId = _CreatorId;
            this.mArtist = _Artist;
        }

        public string Artist
        {
            get
            {
                return this.mArtist;
            }
        }

        public int CreatorId
        {
            get
            {
                return this.mCreatorId;
            }
        }

        public int Id
        {
            get
            {
                return this.mId;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public string Sound
        {
            get
            {
                return this.mSound;
            }
        }
    }
}

