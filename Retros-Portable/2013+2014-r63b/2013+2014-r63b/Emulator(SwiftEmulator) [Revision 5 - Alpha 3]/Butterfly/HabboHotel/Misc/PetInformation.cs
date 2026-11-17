namespace Butterfly.HabboHotel.Misc
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct PetInformation
    {
        internal string roomName;
        internal string roomOwner;
        internal uint petID;
        internal string petName;
        public PetInformation(string pRoomName, string pRoomOwner, uint pPetID, string pPetName)
        {
            this.roomName = pRoomName;
            this.roomOwner = pRoomOwner;
            this.petID = pPetID;
            this.petName = pPetName;
        }
    }
}

