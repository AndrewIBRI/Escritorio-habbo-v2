namespace Butterfly.HabboHotel.Habbo.SoundMachine
{
    using Butterfly;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class SongManager
    {
        private const int CACHE_LIFETIME = 180;
        private static Dictionary<int, double> cacheTimer;
        private static Dictionary<int, SongData> songs;

        public static SongData GetSong(int SongId)
        {
            return songs[SongId];
        }

        public static SongData GetSongFromDataRow(DataRow dRow)
        {
            return new SongData(Convert.ToInt32(dRow["id"]), (string) dRow["name"], (string) dRow["song_data"], Convert.ToInt32(dRow["creator_id"]), (string) dRow["artist"]);
        }

        public static void Initialize()
        {
            songs = new Dictionary<int, SongData>();
            cacheTimer = new Dictionary<int, double>();
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT * FROM songs");
                DataTable table = adapter.getTable();
                foreach (DataRow row in table.Rows)
                {
                    SongData songFromDataRow = GetSongFromDataRow(row);
                    songs.Add(songFromDataRow.Id, songFromDataRow);
                }
            }
        }
    }
}

