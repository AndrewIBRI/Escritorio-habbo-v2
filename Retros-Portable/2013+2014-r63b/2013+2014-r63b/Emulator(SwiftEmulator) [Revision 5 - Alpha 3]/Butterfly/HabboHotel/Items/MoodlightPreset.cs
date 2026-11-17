namespace Butterfly.HabboHotel.Items
{
    using System;

    internal class MoodlightPreset
    {
        internal bool BackgroundOnly;
        internal string ColorCode;
        internal int ColorIntensity;

        internal MoodlightPreset(string ColorCode, int ColorIntensity, bool BackgroundOnly)
        {
            this.ColorCode = ColorCode;
            this.ColorIntensity = ColorIntensity;
            this.BackgroundOnly = BackgroundOnly;
        }
    }
}

