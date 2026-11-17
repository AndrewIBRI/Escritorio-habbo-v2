using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.HabboHotel.Pets;
namespace GoldTree.Communication.Messages.Rooms.Pets
{
	internal sealed class GetPetCommandsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			RoomUser class2 = @class.method_48(num);
			if (class2 != null && class2.PetData != null)
			{
                Pet pet = class2.PetData;
				
				Session.SendMessage(pet.SerializePetCommands());
			}
		}
	}
}
