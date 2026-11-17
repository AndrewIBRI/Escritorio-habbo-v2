namespace Butterfly.HabboHotel.Items
{
    using Butterfly;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    internal class MoodlightData
    {
        internal int CurrentPreset;
        internal bool Enabled;
        internal uint ItemId;
        internal List<MoodlightPreset> Presets;

        internal MoodlightData(uint ItemId)
        {
            DataRow row;
            this.ItemId = ItemId;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT enabled,current_preset,preset_one,preset_two,preset_three FROM items_moodlight WHERE item_id = " + ItemId);
                row = adapter.getRow();
            }
            if (row == null)
            {
                throw new NullReferenceException("No moodlightdata found in the database");
            }
            this.Enabled = ButterflyEnvironment.EnumToBool(row["enabled"].ToString());
            this.CurrentPreset = (int) row["current_preset"];
            this.Presets = new List<MoodlightPreset>();
            this.Presets.Add(GeneratePreset((string) row["preset_one"]));
            this.Presets.Add(GeneratePreset((string) row["preset_two"]));
            this.Presets.Add(GeneratePreset((string) row["preset_three"]));
        }

        internal void Disable()
        {
            this.Enabled = false;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE items_moodlight SET enabled = 0 WHERE item_id = " + this.ItemId);
            }
        }

        internal void Enable()
        {
            this.Enabled = true;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery("UPDATE items_moodlight SET enabled = 1 WHERE item_id = " + this.ItemId);
            }
        }

        internal string GenerateExtraData()
        {
            MoodlightPreset preset = this.GetPreset(this.CurrentPreset);
            StringBuilder builder = new StringBuilder();
            if (this.Enabled)
            {
                builder.Append(2);
            }
            else
            {
                builder.Append(1);
            }
            builder.Append(",");
            builder.Append(this.CurrentPreset);
            builder.Append(",");
            if (preset.BackgroundOnly)
            {
                builder.Append(2);
            }
            else
            {
                builder.Append(1);
            }
            builder.Append(",");
            builder.Append(preset.ColorCode);
            builder.Append(",");
            builder.Append(preset.ColorIntensity);
            return builder.ToString();
        }

        internal static MoodlightPreset GeneratePreset(string Data)
        {
            string[] strArray = Data.Split(new char[] { ',' });
            if (!IsValidColor(strArray[0]))
            {
                strArray[0] = "#000000";
            }
            return new MoodlightPreset(strArray[0], int.Parse(strArray[1]), ButterflyEnvironment.EnumToBool(strArray[2]));
        }

        internal MoodlightPreset GetPreset(int i)
        {
            i--;
            if (this.Presets[i] != null)
            {
                return this.Presets[i];
            }
            return new MoodlightPreset("#000000", 0xff, false);
        }

        internal static bool IsValidColor(string ColorCode)
        {
            switch (ColorCode)
            {
                case "#000000":
                case "#0053F7":
                case "#EA4532":
                case "#82F349":
                case "#74F5F5":
                case "#E759DE":
                case "#F2F851":
                    return true;
            }
            return false;
        }

        internal static bool IsValidIntensity(int Intensity)
        {
            if ((Intensity < 0) || (Intensity > 0xff))
            {
                return false;
            }
            return true;
        }

        internal void UpdatePreset(int Preset, string Color, int Intensity, bool BgOnly)
        {
            if (IsValidColor(Color) && IsValidIntensity(Intensity))
            {
                string str;
                switch (Preset)
                {
                    case 2:
                        str = "two";
                        break;

                    case 3:
                        str = "three";
                        break;

                    default:
                        str = "one";
                        break;
                }
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery(string.Concat(new object[] { "UPDATE items_moodlight SET preset_", str, " = '@color,", Intensity, ",", ButterflyEnvironment.BoolToEnum(BgOnly), "' WHERE item_id = ", this.ItemId }));
                    adapter.addParameter("color", Color);
                    adapter.runQuery();
                }
                this.GetPreset(Preset).ColorCode = Color;
                this.GetPreset(Preset).ColorIntensity = Intensity;
                this.GetPreset(Preset).BackgroundOnly = BgOnly;
            }
        }
    }
}

