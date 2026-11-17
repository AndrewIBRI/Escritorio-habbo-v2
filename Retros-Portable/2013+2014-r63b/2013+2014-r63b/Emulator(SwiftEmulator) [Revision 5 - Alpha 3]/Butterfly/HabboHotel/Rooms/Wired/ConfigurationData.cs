namespace Butterfly.HabboHotel.Rooms.Wired
{
    using System;

    internal class ConfigurationData
    {
        internal bool Boolean;
        internal int Duration;
        internal int VariableInt;

        public ConfigurationData(int duration, bool boolean, int variableInt)
        {
            this.Duration = duration;
            this.Boolean = boolean;
            this.VariableInt = variableInt;
        }
    }
}

