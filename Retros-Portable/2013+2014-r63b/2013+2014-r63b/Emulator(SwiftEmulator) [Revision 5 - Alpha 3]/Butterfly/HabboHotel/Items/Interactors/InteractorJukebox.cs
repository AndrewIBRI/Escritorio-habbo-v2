namespace Butterfly.HabboHotel.Items.Interactors
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Habbo.SoundMachine;
    using Butterfly.HabboHotel.Items;
    using System;

    internal class InteractorJukebox : FurniInteractor
    {
        internal override void OnPlace(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnRemove(GameClient Session, RoomItem Item)
        {
        }

        internal override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if ((UserHasRights && (Session != null)) && (Item != null))
            {
                RoomMusicController roomMusicController = ButterflyEnvironment.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).GetRoomMusicController();
                if (roomMusicController.IsPlaying)
                {
                    roomMusicController.Stop();
                }
                else
                {
                    roomMusicController.Start(Request);
                }
            }
        }
    }
}

