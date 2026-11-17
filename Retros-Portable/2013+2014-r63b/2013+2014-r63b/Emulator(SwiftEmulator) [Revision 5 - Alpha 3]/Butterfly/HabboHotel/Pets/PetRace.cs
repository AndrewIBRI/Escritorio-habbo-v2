namespace Butterfly.HabboHotel.Pets
{
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class PetRace
    {
        public int Color1;
        public int Color2;
        public bool Has1Color;
        public bool Has2Color;
        public int RaceId;
        public static List<PetRace> Races;

        public static List<PetRace> GetRacesForRaceId(int sRaceId)
        {
            List<PetRace> list = new List<PetRace>();
            foreach (PetRace race in Races)
            {
                if (race.RaceId == sRaceId)
                {
                    list.Add(race);
                }
            }
            return list;
        }

        public static void Init(IQueryAdapter dbClient)
        {
            dbClient.setQuery("SELECT * FROM pets_racesoncatalogue");
            DataTable table = dbClient.getTable();
            Races = new List<PetRace>();
            foreach (DataRow row in table.Rows)
            {
                PetRace item = new PetRace {
                    RaceId = (int) row["raceid"],
                    Color1 = (int) row["color1"],
                    Color2 = (int) row["color2"],
                    Has1Color = ((string) row["has1color"]) == "1",
                    Has2Color = ((string) row["has2color"]) == "1"
                };
                Races.Add(item);
            }
        }

        public static bool RaceGotRaces(int sRaceId)
        {
            return (GetRacesForRaceId(sRaceId).Count > 0);
        }
    }
}

