namespace Butterfly.HabboHotel.Pets
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct PetCommand
    {
        internal readonly int commandID;
        internal readonly string commandInput;
        public PetCommand(int commandID, string commandInput)
        {
            this.commandID = commandID;
            this.commandInput = commandInput;
        }
    }
}

