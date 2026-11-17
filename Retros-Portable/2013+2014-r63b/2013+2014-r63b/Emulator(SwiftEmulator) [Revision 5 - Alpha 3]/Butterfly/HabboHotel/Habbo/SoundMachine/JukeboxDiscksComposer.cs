namespace Butterfly.HabboHotel.Habbo.SoundMachine
{
    using Butterfly;
    using Butterfly.HabboHotel.Items;
    using Butterfly.Messages;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class JukeboxDiscksComposer
    {
        public static ServerMessage Compose(List<SongData> Songs)
        {
            ServerMessage message = new ServerMessage(Outgoing.ListenPreviewSong);
            message.AppendInt32(Songs.Count);
            foreach (SongData data in Songs)
            {
                message.AppendInt32(data.Id);
                message.AppendString(data.Name);
                message.AppendString(data.Name.Replace("_", " "));
                message.AppendString(data.Sound);
                message.AppendInt32(data.CreatorId);
                message.AppendString(data.Artist);
            }
            return message;
        }

        public static ServerMessage Compose(int PlaylistCapacity, List<SongInstance> Playlist)
        {
            ServerMessage message = new ServerMessage(Outgoing.SerializeJukeSongs);
            message.AppendInt32(PlaylistCapacity);
            message.AppendInt32(Playlist.Count);
            foreach (SongInstance instance in Playlist)
            {
                message.AppendInt32(instance.DiskItem.itemID);
                message.AppendInt32(instance.SongData.Id);
            }
            return message;
        }

        public static ServerMessage ComposePlayingComposer(int SongId, int PlaylistItemNumber, int SyncTimestampMs)
        {
            ServerMessage message = new ServerMessage(Outgoing.PlayStopMusic);
            if (SongId == 0)
            {
                message.AppendInt32(-1);
                message.AppendInt32(-1);
                message.AppendInt32(-1);
                message.AppendInt32(-1);
                message.AppendInt32(0);
                return message;
            }
            message.AppendInt32(SongId);
            message.AppendInt32(PlaylistItemNumber);
            message.AppendInt32(SongId);
            message.AppendInt32(0);
            message.AppendInt32(SyncTimestampMs);
            return message;
        }

        public static ServerMessage SerializeSongInventory(Hashtable songs)
        {
            ServerMessage message = new ServerMessage(Outgoing.SerializeInvSongs);
            message.AppendInt32(songs.Count);
            foreach (UserItem item in songs.Values)
            {
                int i = TextHandling.Parse2(item.ExtraData);
                message.AppendInt32(item.Id);
                message.AppendInt32(i);
            }
            return message;
        }
    }
}

