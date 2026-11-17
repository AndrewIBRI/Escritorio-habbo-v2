namespace Butterfly.HabboHotel.Habbo.SoundMachine
{
    using Butterfly;
    using Butterfly.HabboHotel.Items;
    using Butterfly.HabboHotel.Rooms;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class RoomMusicController
    {
        private static bool mBroadcastNeeded;
        private bool mIsPlaying;
        private Dictionary<int, SongItem> mLoadedDisks = new Dictionary<int, SongItem>();
        private SortedDictionary<int, SongInstance> mPlaylist = new SortedDictionary<int, SongInstance>();
        private RoomItem mRoomOutputItem;
        private SongInstance mSong;
        private int mSongQueuePosition;
        private double mStartedPlayingTimestamp;

        public int AddDisk(SongItem DiskItem)
        {
            int songID = DiskItem.songID;
            if (songID == 0)
            {
                return -1;
            }
            SongData song = SongManager.GetSong(songID);
            if (song == null)
            {
                return -1;
            }
            if (this.mLoadedDisks.ContainsKey(DiskItem.itemID))
            {
                return -1;
            }
            this.mLoadedDisks.Add(DiskItem.itemID, DiskItem);
            int count = this.mPlaylist.Count;
            lock (this.mPlaylist)
            {
                this.mPlaylist.Add(count, new SongInstance(DiskItem, song));
            }
            return count;
        }

        internal void BroadcastCurrentSongData(Room Instance)
        {
            if (this.mSong != null)
            {
                Instance.SendMessage(JukeboxDiscksComposer.ComposePlayingComposer(this.mSong.SongData.Id, this.mSongQueuePosition, 0));
            }
            else
            {
                Instance.SendMessage(JukeboxDiscksComposer.ComposePlayingComposer(0, 0, 0));
            }
        }

        internal void Destroy()
        {
            if (this.mLoadedDisks != null)
            {
                this.mLoadedDisks.Clear();
            }
            if (this.mPlaylist != null)
            {
                this.mPlaylist.Clear();
            }
            this.mPlaylist = null;
            this.mLoadedDisks = null;
            this.mSong = null;
            this.mRoomOutputItem = null;
        }

        public bool IsLoaded(int ItemId)
        {
            return this.mLoadedDisks.ContainsKey(ItemId);
        }

        public void LinkRoomOutputItem(RoomItem Item)
        {
            this.mRoomOutputItem = Item;
        }

        internal void OnNewUserEnter(RoomUser user)
        {
            if ((user.GetClient() != null) && (this.mSong != null))
            {
                this.mSongQueuePosition++;
                user.GetClient().SendMessage(JukeboxDiscksComposer.ComposePlayingComposer(this.mSong.SongData.Id, this.mSongQueuePosition, this.SongSyncTimestamp));
            }
        }

        public void PlaySong()
        {
            if (this.mSongQueuePosition >= this.mPlaylist.Count)
            {
                this.mSongQueuePosition = 0;
            }
            if (this.mPlaylist.Count == 0)
            {
                this.Stop();
            }
            else
            {
                if (!this.mPlaylist.ContainsKey(this.mSongQueuePosition))
                {
                    this.mSongQueuePosition = 0;
                }
                this.mSong = this.mPlaylist[this.mSongQueuePosition];
                this.mStartedPlayingTimestamp = ButterflyEnvironment.GetUnixTimestamp();
                mBroadcastNeeded = true;
            }
        }

        public SongItem RemoveDisk(int PlaylistIndex)
        {
            SongInstance instance = null;
            lock (this.mPlaylist)
            {
                if (!this.mPlaylist.ContainsKey(PlaylistIndex))
                {
                    return null;
                }
                instance = this.mPlaylist[PlaylistIndex];
                this.mPlaylist.Remove(PlaylistIndex);
            }
            lock (this.mLoadedDisks)
            {
                this.mLoadedDisks.Remove(instance.DiskItem.itemID);
            }
            this.RepairPlaylist();
            if (PlaylistIndex == this.mSongQueuePosition)
            {
                this.PlaySong();
            }
            return instance.DiskItem;
        }

        public void RepairPlaylist()
        {
            List<SongItem> list = null;
            lock (this.mLoadedDisks)
            {
                list = this.mLoadedDisks.Values.ToList<SongItem>();
                this.mLoadedDisks.Clear();
            }
            lock (this.mPlaylist)
            {
                this.mPlaylist.Clear();
            }
            foreach (SongItem item in list)
            {
                this.AddDisk(item);
            }
        }

        public void Reset()
        {
            lock (this.mLoadedDisks)
            {
                this.mLoadedDisks.Clear();
            }
            lock (this.mPlaylist)
            {
                this.mPlaylist.Clear();
            }
            this.mRoomOutputItem = null;
            this.mSongQueuePosition = -1;
            this.mStartedPlayingTimestamp = 0.0;
        }

        public void SetNextSong()
        {
            this.mSongQueuePosition++;
            this.PlaySong();
        }

        public void Start(int SongRequest)
        {
            this.mIsPlaying = true;
            this.mSongQueuePosition = SongRequest - 1;
            this.SetNextSong();
        }

        public void Stop()
        {
            this.mSong = null;
            this.mIsPlaying = false;
            this.mSongQueuePosition = -1;
            mBroadcastNeeded = true;
        }

        public void Update(Room Instance)
        {
            if (this.mIsPlaying && ((this.mSong == null) || (this.TimePlaying > 60.0)))
            {
                if (this.mPlaylist.Count == 0)
                {
                    this.Stop();
                    this.mRoomOutputItem.ExtraData = "0";
                    this.mRoomOutputItem.UpdateState();
                }
                else
                {
                    this.SetNextSong();
                }
                mBroadcastNeeded = true;
            }
            if (mBroadcastNeeded)
            {
                this.BroadcastCurrentSongData(Instance);
                mBroadcastNeeded = false;
            }
        }

        public SongInstance CurrentSong
        {
            get
            {
                return this.mSong;
            }
        }

        public bool HasLinkedItem
        {
            get
            {
                return (this.mRoomOutputItem != null);
            }
        }

        public bool IsPlaying
        {
            get
            {
                return this.mIsPlaying;
            }
        }

        public int LinkedItemId
        {
            get
            {
                return ((this.mRoomOutputItem != null) ? ((int) this.mRoomOutputItem.RoomId) : 0);
            }
        }

        public SortedDictionary<int, SongInstance> Playlist
        {
            get
            {
                SortedDictionary<int, SongInstance> dictionary = new SortedDictionary<int, SongInstance>();
                lock (this.mPlaylist)
                {
                    foreach (KeyValuePair<int, SongInstance> pair in this.mPlaylist)
                    {
                        dictionary.Add(pair.Key, pair.Value);
                    }
                }
                return dictionary;
            }
        }

        public int PlaylistCapacity
        {
            get
            {
                return 20;
            }
        }

        public int PlaylistSize
        {
            get
            {
                return this.mPlaylist.Count;
            }
        }

        public int SongQueuePosition
        {
            get
            {
                return this.mSongQueuePosition;
            }
        }

        public int SongSyncTimestamp
        {
            get
            {
                if (!(this.mIsPlaying && (this.mSong != null)))
                {
                    return 0;
                }
                if (this.TimePlaying >= this.mSong.SongData.CreatorId)
                {
                    return this.mSong.SongData.CreatorId;
                }
                return (int) (this.TimePlaying * 1000.0);
            }
        }

        public double TimePlaying
        {
            get
            {
                return (ButterflyEnvironment.GetUnixTimestamp() - this.mStartedPlayingTimestamp);
            }
        }
    }
}

