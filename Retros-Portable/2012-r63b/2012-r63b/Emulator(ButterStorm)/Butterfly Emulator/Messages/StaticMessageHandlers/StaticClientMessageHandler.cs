using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using HabboEvents;

namespace Butterfly.Messages.StaticMessageHandlers
{
    class StaticClientMessageHandler
    {
        private delegate void StaticRequestHandler(GameClientMessageHandler handler);
        private static Hashtable handlers;

        internal static void Initialize()
        {
            handlers = new Hashtable(177);
            RegisterPacketLibary();
        }

        internal static void HandlePacket(GameClientMessageHandler handler, ClientMessage message)
        {
            //Console.WriteLine("Handle Packet => " + message.Id);

            if (handlers.ContainsKey(message.Id))
            {
                StaticRequestHandler currentHandler = (StaticRequestHandler)handlers[message.Id];
                currentHandler.Invoke(handler);
            }
        }

        #region Register
        internal static void RegisterPacketLibary()
        {
            handlers.Add(Incoming.CheckReleaseMessageEvent, new StaticRequestHandler(SharedPacketLib.CheckHabboRelease));
            handlers.Add(Incoming.InitCrypto, new StaticRequestHandler(SharedPacketLib.InitCrypto));
            handlers.Add(Incoming.SecretKey, new StaticRequestHandler(SharedPacketLib.SecretKey));
            handlers.Add(Incoming.ClientVars, new StaticRequestHandler(SharedPacketLib.setVars));
            handlers.Add(Incoming.UniqueMachineID, new StaticRequestHandler(SharedPacketLib.setUniqueId));
            handlers.Add(Incoming.PrepareCampaing, new StaticRequestHandler(SharedPacketLib.PrepareCampaing));
            handlers.Add(Incoming.SendCampaingData, new StaticRequestHandler(SharedPacketLib.SendCampaingData));
            handlers.Add(Incoming.OpenCatalog, new StaticRequestHandler(SharedPacketLib.GetCatalogIndex));
            handlers.Add(Incoming.OpenCatalogPage, new StaticRequestHandler(SharedPacketLib.GetCatalogPage));
            handlers.Add(Incoming.RedeemVoucher, new StaticRequestHandler(SharedPacketLib.RedeemVoucher));
            handlers.Add(Incoming.PurchaseCatalogItem, new StaticRequestHandler(SharedPacketLib.HandlePurchase));
            handlers.Add(Incoming.PurchaseGift, new StaticRequestHandler(SharedPacketLib.PurchaseGift));
            /*handlers.Add(412, new StaticRequestHandler(SharedPacketLib.GetRecyclerRewards));
            handlers.Add(3030, new StaticRequestHandler(SharedPacketLib.CanGift));*/
            handlers.Add(Incoming.CatalogData1, new StaticRequestHandler(SharedPacketLib.GetCataData1));
            handlers.Add(Incoming.CatalogData2, new StaticRequestHandler(SharedPacketLib.GetCataData2));
            /*handlers.Add(3012, new StaticRequestHandler(SharedPacketLib.MarketplaceCanSell));
            handlers.Add(3010, new StaticRequestHandler(SharedPacketLib.MarketplacePostItem));
            handlers.Add(3019, new StaticRequestHandler(SharedPacketLib.MarketplaceGetOwnOffers));
            handlers.Add(3015, new StaticRequestHandler(SharedPacketLib.MarketplaceTakeBack));
            handlers.Add(3016, new StaticRequestHandler(SharedPacketLib.MarketplaceClaimCredits));
            handlers.Add(3018, new StaticRequestHandler(SharedPacketLib.MarketplaceGetOffers));
            handlers.Add(3014, new StaticRequestHandler(SharedPacketLib.MarketplacePurchase));*/
            handlers.Add(Incoming.CheckPetName, new StaticRequestHandler(SharedPacketLib.CheckPetName));
            handlers.Add(Incoming.CatalogGetRace, new StaticRequestHandler(SharedPacketLib.PetRaces));
            handlers.Add(Incoming.Pong, new StaticRequestHandler(SharedPacketLib.Pong));
            //handlers.Add(231, new StaticRequestHandler(SharedPacketLib.GetGroupdetails));
            handlers.Add(Incoming.SSOTicket, new StaticRequestHandler(SharedPacketLib.SSOLogin));
            handlers.Add(Incoming.OpenHelpTool, new StaticRequestHandler(SharedPacketLib.InitHelpTool));
            //handlers.Add(417, new StaticRequestHandler(SharedPacketLib.GetHelpCategories));
            //handlers.Add(418, new StaticRequestHandler(SharedPacketLib.ViewHelpTopic));
            //handlers.Add(419, new StaticRequestHandler(SharedPacketLib.SearchHelpTopics));
            //handlers.Add(420, new StaticRequestHandler(SharedPacketLib.GetTopicsInCategory));
            handlers.Add(Incoming.CreateTicket, new StaticRequestHandler(SharedPacketLib.SubmitHelpTicket));
            //handlers.Add(238, new StaticRequestHandler(SharedPacketLib.DeletePendingCFH));
            //handlers.Add(440, new StaticRequestHandler(SharedPacketLib.CallGuideBot));
            handlers.Add(Incoming.SendRoomAlert, new StaticRequestHandler(SharedPacketLib.ModSendRoomAlert));
            handlers.Add(Incoming.PickIssue, new StaticRequestHandler(SharedPacketLib.ModPickTicket));
            handlers.Add(Incoming.ReleaseIssue, new StaticRequestHandler(SharedPacketLib.ModReleaseTicket));
            handlers.Add(Incoming.CloseIssue, new StaticRequestHandler(SharedPacketLib.ModCloseTicket));
            handlers.Add(Incoming.ToolForUser, new StaticRequestHandler(SharedPacketLib.ModGetUserInfo));
            handlers.Add(Incoming.UserChatlog, new StaticRequestHandler(SharedPacketLib.ModGetUserChatlog));
            handlers.Add(Incoming.OpenRoomChatlog, new StaticRequestHandler(SharedPacketLib.ModGetRoomChatlog));
            handlers.Add(Incoming.IssueChatlog, new StaticRequestHandler(SharedPacketLib.ModGetTicketChatlog));
            handlers.Add(Incoming.GetRoomVisits, new StaticRequestHandler(SharedPacketLib.ModGetRoomVisits));
            handlers.Add(Incoming.ToolForThisRoom, new StaticRequestHandler(SharedPacketLib.ModGetRoomTool));
            handlers.Add(Incoming.PerformRoomAction, new StaticRequestHandler(SharedPacketLib.ModPerformRoomAction));
            handlers.Add(Incoming.SendUserMessage, new StaticRequestHandler(SharedPacketLib.ModSendUserCaution));
            handlers.Add(Incoming.SendMessageByTemplate, new StaticRequestHandler(SharedPacketLib.ModSendUserMessage));
            handlers.Add(Incoming.ModActionKickUser, new StaticRequestHandler(SharedPacketLib.ModKickUser));
            handlers.Add(Incoming.ModActionMuteUser, new StaticRequestHandler(SharedPacketLib.ModKickUser));
            handlers.Add(Incoming.ModActionBanUser, new StaticRequestHandler(SharedPacketLib.ModBanUser));
            //handlers.Add(Incoming.GetFriends, new StaticRequestHandler(SharedPacketLib.InitMessenger));
            handlers.Add(Incoming.UpdateFriendsState, new StaticRequestHandler(SharedPacketLib.FriendsListUpdate));
            handlers.Add(Incoming.DeleteFriend, new StaticRequestHandler(SharedPacketLib.RemoveBuddy));
            handlers.Add(Incoming.SearchFriend, new StaticRequestHandler(SharedPacketLib.SearchHabbo));
            handlers.Add(Incoming.SendInstantMessenger, new StaticRequestHandler(SharedPacketLib.SendInstantMessenger));
            handlers.Add(Incoming.AcceptRequest, new StaticRequestHandler(SharedPacketLib.AcceptRequest));
            handlers.Add(Incoming.DeclineRequest, new StaticRequestHandler(SharedPacketLib.DeclineRequest));
            handlers.Add(Incoming.FriendRequest, new StaticRequestHandler(SharedPacketLib.RequestBuddy));
            handlers.Add(Incoming.FollowFriend, new StaticRequestHandler(SharedPacketLib.FollowBuddy));
            handlers.Add(Incoming.InviteFriendsToMyRoom, new StaticRequestHandler(SharedPacketLib.SendInstantInvite));
            //*handlers.Add(391, new StaticRequestHandler(SharedPacketLib.OpenFlat));
            handlers.Add(Incoming.AddFavourite, new StaticRequestHandler(SharedPacketLib.AddFavorite));
            handlers.Add(Incoming.RemoveFavourite, new StaticRequestHandler(SharedPacketLib.RemoveFavorite));
            handlers.Add(Incoming.GoToHotelView, new StaticRequestHandler(SharedPacketLib.GoToHotelView));
            handlers.Add(Incoming.LoadCategorys, new StaticRequestHandler(SharedPacketLib.GetFlatCats));
            //handlers.Add(233, new StaticRequestHandler(SharedPacketLib.EnterInquiredRoom));
            handlers.Add(Incoming.LoadFeaturedRooms, new StaticRequestHandler(SharedPacketLib.GetPubs));
            //handlers.Add(385, new StaticRequestHandler(SharedPacketLib.GetRoomInfo));*/
            handlers.Add(Incoming.LoadAllRooms, new StaticRequestHandler(SharedPacketLib.GetPopularRooms));
            handlers.Add(Incoming.HighRatedRooms, new StaticRequestHandler(SharedPacketLib.GetHighRatedRooms));
            handlers.Add(Incoming.RoomsOfMyFriends, new StaticRequestHandler(SharedPacketLib.GetFriendsRooms));
            handlers.Add(Incoming.RoomsWhereMyFriends, new StaticRequestHandler(SharedPacketLib.GetRoomsWithFriends));
            handlers.Add(Incoming.LoadMyRooms, new StaticRequestHandler(SharedPacketLib.GetOwnRooms));
            handlers.Add(Incoming.MyFavs, new StaticRequestHandler(SharedPacketLib.GetFavoriteRooms));
            handlers.Add(Incoming.RecentRooms, new StaticRequestHandler(SharedPacketLib.GetRecentRooms));
            //handlers.Add(439, new StaticRequestHandler(SharedPacketLib.GetEvents));*/
            handlers.Add(Incoming.LoadPopularTags, new StaticRequestHandler(SharedPacketLib.GetPopularTags));
            handlers.Add(Incoming.SearchRoomByName, new StaticRequestHandler(SharedPacketLib.PerformSearch));
            /*handlers.Add(438, new StaticRequestHandler(SharedPacketLib.PerformSearch2));
            handlers.Add(3101, new StaticRequestHandler(SharedPacketLib.OpenQuests));
            handlers.Add(3102, new StaticRequestHandler(SharedPacketLib.StartQuest));
            handlers.Add(3106, new StaticRequestHandler(SharedPacketLib.StopQuest));
            handlers.Add(3107, new StaticRequestHandler(SharedPacketLib.GetCurrentQuest));
            handlers.Add(182, new StaticRequestHandler(SharedPacketLib.GetAdvertisement));
            handlers.Add(388, new StaticRequestHandler(SharedPacketLib.GetPub));
            handlers.Add(2, new StaticRequestHandler(SharedPacketLib.OpenPub));
            handlers.Add(230, new StaticRequestHandler(SharedPacketLib.GetGroupBadges));*/
            //handlers.Add(215, new StaticRequestHandler(SharedPacketLib.GetRoomData1));
            handlers.Add(Incoming.LoadHeightMap, new StaticRequestHandler(SharedPacketLib.GetRoomData2));
            handlers.Add(Incoming.AddUserToRoom, new StaticRequestHandler(SharedPacketLib.GetRoomData3));
            handlers.Add(Incoming.Talk, new StaticRequestHandler(SharedPacketLib.Talk));
            handlers.Add(Incoming.Shout, new StaticRequestHandler(SharedPacketLib.Shout));
            handlers.Add(Incoming.Whisp, new StaticRequestHandler(SharedPacketLib.Whisper));
            handlers.Add(Incoming.Move, new StaticRequestHandler(SharedPacketLib.Move));
            handlers.Add(Incoming.CanCreateRoom, new StaticRequestHandler(SharedPacketLib.CanCreateRoom));
            handlers.Add(Incoming.CreateRoom, new StaticRequestHandler(SharedPacketLib.CreateRoom));
            handlers.Add(Incoming.LoadFirstRoomData, new StaticRequestHandler(SharedPacketLib.enterOnRoom));
            handlers.Add(Incoming.GetRoomData, new StaticRequestHandler(SharedPacketLib.GetRoomEditData));
            handlers.Add(Incoming.SaveRoomData, new StaticRequestHandler(SharedPacketLib.SaveRoomData));
            handlers.Add(Incoming.GiveRights, new StaticRequestHandler(SharedPacketLib.GiveRights));
            handlers.Add(Incoming.RemoveRightsFrom, new StaticRequestHandler(SharedPacketLib.TakeRights));
            handlers.Add(Incoming.RemoveAllRights, new StaticRequestHandler(SharedPacketLib.TakeAllRights));
            handlers.Add(Incoming.KickUserOfRoom, new StaticRequestHandler(SharedPacketLib.KickUser));
            handlers.Add(Incoming.BanUserOfRoom, new StaticRequestHandler(SharedPacketLib.BanUser));
            handlers.Add(Incoming.StartTrade, new StaticRequestHandler(SharedPacketLib.InitTrade));
            handlers.Add(Incoming.SetHome, new StaticRequestHandler(SharedPacketLib.SetHomeRoom));
            handlers.Add(Incoming.RemoveRoom, new StaticRequestHandler(SharedPacketLib.DeleteRoom));
            handlers.Add(Incoming.LookTo, new StaticRequestHandler(SharedPacketLib.LookAt));
            handlers.Add(Incoming.StartTyping, new StaticRequestHandler(SharedPacketLib.StartTyping));
            handlers.Add(Incoming.StopTyping, new StaticRequestHandler(SharedPacketLib.StopTyping));
            handlers.Add(Incoming.IgnoreUser, new StaticRequestHandler(SharedPacketLib.IgnoreUser));
            handlers.Add(Incoming.UnignoreUser, new StaticRequestHandler(SharedPacketLib.UnignoreUser));
            handlers.Add(Incoming.CanCreateRoomEvent, new StaticRequestHandler(SharedPacketLib.CanCreateRoomEvent));
            handlers.Add(Incoming.CreateRoomEvent, new StaticRequestHandler(SharedPacketLib.StartEvent));
            handlers.Add(Incoming.StopEvent, new StaticRequestHandler(SharedPacketLib.StopEvent));
            handlers.Add(Incoming.EditEvent, new StaticRequestHandler(SharedPacketLib.EditEvent));
            handlers.Add(Incoming.ApplyAction, new StaticRequestHandler(SharedPacketLib.Wave));
            handlers.Add(Incoming.ApplySign, new StaticRequestHandler(SharedPacketLib.Sign));
            handlers.Add(Incoming.GetUserTags, new StaticRequestHandler(SharedPacketLib.GetUserTags));
            handlers.Add(Incoming.GetUserBadges, new StaticRequestHandler(SharedPacketLib.GetUserBadges));
            handlers.Add(Incoming.GiveRoomScore, new StaticRequestHandler(SharedPacketLib.RateRoom));
            handlers.Add(Incoming.ApplyDance, new StaticRequestHandler(SharedPacketLib.Dance));
            handlers.Add(Incoming.RemoveHanditem, new StaticRequestHandler(SharedPacketLib.RemoveHanditem));
            handlers.Add(Incoming.GiveObject, new StaticRequestHandler(SharedPacketLib.GiveHanditem));
            handlers.Add(Incoming.AnswerDoorBell, new StaticRequestHandler(SharedPacketLib.AnswerDoorbell));
            handlers.Add(Incoming.ReqLoadByDoorBell, new StaticRequestHandler(SharedPacketLib.ReqLoadRoomForUser));
            handlers.Add(Incoming.ApplySpace, new StaticRequestHandler(SharedPacketLib.ApplyRoomEffect));
            handlers.Add(Incoming.AddFloorItem, new StaticRequestHandler(SharedPacketLib.PlaceItem));
            handlers.Add(Incoming.PickupItem, new StaticRequestHandler(SharedPacketLib.TakeItem));
            handlers.Add(Incoming.MoveOrRotate, new StaticRequestHandler(SharedPacketLib.MoveItem));
            handlers.Add(Incoming.MoveWall, new StaticRequestHandler(SharedPacketLib.MoveWallItem));
            handlers.Add(Incoming.HandleItem, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(Incoming.HandleWallItem, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(Incoming.OpenPostIt, new StaticRequestHandler(SharedPacketLib.OpenPostit));
            handlers.Add(Incoming.SavePostIt, new StaticRequestHandler(SharedPacketLib.SavePostit));
            handlers.Add(Incoming.DeletePostIt, new StaticRequestHandler(SharedPacketLib.DeletePostit));
            handlers.Add(Incoming.OpenGift, new StaticRequestHandler(SharedPacketLib.OpenPresent));
            handlers.Add(Incoming.StartMoodlight, new StaticRequestHandler(SharedPacketLib.GetMoodlight));
            handlers.Add(Incoming.ApplyMoodlightChanges, new StaticRequestHandler(SharedPacketLib.UpdateMoodlight));
            handlers.Add(Incoming.TurnOnMoodlight, new StaticRequestHandler(SharedPacketLib.SwitchMoodlightStatus));
            handlers.Add(Incoming.SendOffer, new StaticRequestHandler(SharedPacketLib.OfferTradeItem));
            handlers.Add(Incoming.CancelOffer, new StaticRequestHandler(SharedPacketLib.TakeBackTradeItem));
            handlers.Add(Incoming.CancelTrade, new StaticRequestHandler(SharedPacketLib.StopTrade));
            //handlers.Add(403, new StaticRequestHandler(SharedPacketLib.StopTrade));
            handlers.Add(Incoming.AcceptTrade, new StaticRequestHandler(SharedPacketLib.AcceptTrade));
            handlers.Add(Incoming.UnacceptTrade, new StaticRequestHandler(SharedPacketLib.UnacceptTrade));
            handlers.Add(Incoming.ConfirmTrade, new StaticRequestHandler(SharedPacketLib.CompleteTrade));
            handlers.Add(Incoming.SendRespects, new StaticRequestHandler(SharedPacketLib.GiveRespect));
            handlers.Add(Incoming.StartEffect, new StaticRequestHandler(SharedPacketLib.ApplyEffect));
            handlers.Add(Incoming.EnableEffect, new StaticRequestHandler(SharedPacketLib.EnableEffect));
            /*handlers.Add(232, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(314, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(247, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(76, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(77, new StaticRequestHandler(SharedPacketLib.TriggerItemDiceSpecial));
            handlers.Add(414, new StaticRequestHandler(SharedPacketLib.RecycleItems));*/
            handlers.Add(Incoming.RedeemExchangeFurni, new StaticRequestHandler(SharedPacketLib.RedeemExchangeFurni));
            /*handlers.Add(113, new StaticRequestHandler(SharedPacketLib.EnterInfobus));
            handlers.Add(441, new StaticRequestHandler(SharedPacketLib.KickBot));*/
            handlers.Add(Incoming.PlacePet, new StaticRequestHandler(SharedPacketLib.PlacePet));
            handlers.Add(Incoming.PetInfo, new StaticRequestHandler(SharedPacketLib.GetPetInfo));
            handlers.Add(Incoming.PickupPet, new StaticRequestHandler(SharedPacketLib.PickUpPet));
            //handlers.Add(3004, new StaticRequestHandler(SharedPacketLib.CommandsPet));
            handlers.Add(Incoming.RespetPet, new StaticRequestHandler(SharedPacketLib.RespectPet));
            handlers.Add(Incoming.AddPostIt, new StaticRequestHandler(SharedPacketLib.PlacePostIt));
            handlers.Add(Incoming.AddSaddleToPet, new StaticRequestHandler(SharedPacketLib.AddSaddle));
            handlers.Add(Incoming.RemoveSaddle, new StaticRequestHandler(SharedPacketLib.RemoveSaddle));
            handlers.Add(Incoming.MountOnPet, new StaticRequestHandler(SharedPacketLib.Ride));
            //handlers.Add(480, new StaticRequestHandler(SharedPacketLib.SetLookTransfer));
            //handlers.Add(3052, new StaticRequestHandler(SharedPacketLib.SaveWiredCondition));
            handlers.Add(Incoming.SaveWiredEffect, new StaticRequestHandler(SharedPacketLib.SaveWired));
            handlers.Add(Incoming.SaveWiredTrigger, new StaticRequestHandler(SharedPacketLib.SaveWired));
            /*handlers.Add(221, new StaticRequestHandler(SharedPacketLib.GetMusicData));
            handlers.Add(255, new StaticRequestHandler(SharedPacketLib.AddPlaylistItem));
            handlers.Add(256, new StaticRequestHandler(SharedPacketLib.RemovePlaylistItem));
            handlers.Add(259, new StaticRequestHandler(SharedPacketLib.GetDisks));
            handlers.Add(258, new StaticRequestHandler(SharedPacketLib.GetPlaylists));*/
            handlers.Add(Incoming.UserInformation, new StaticRequestHandler(SharedPacketLib.GetUserInfo));
            handlers.Add(Incoming.LoadProfile, new StaticRequestHandler(SharedPacketLib.LoadProfile)); //--> TEMPORARILY DISABLED

            //handlers.Add(8, new StaticRequestHandler(SharedPacketLib.GetBalance));
            handlers.Add(Incoming.SerializeClub, new StaticRequestHandler(SharedPacketLib.GetSubscriptionData));
            handlers.Add(Incoming.BadgesInventary, new StaticRequestHandler(SharedPacketLib.GetBadges));
            handlers.Add(Incoming.ApplyBadge, new StaticRequestHandler(SharedPacketLib.UpdateBadges));
            handlers.Add(Incoming.OpenAchievements, new StaticRequestHandler(SharedPacketLib.GetAchievements));
            handlers.Add(Incoming.ChangeLook, new StaticRequestHandler(SharedPacketLib.ChangeLook));
            handlers.Add(Incoming.ChangeMotto, new StaticRequestHandler(SharedPacketLib.ChangeMotto));
            handlers.Add(Incoming.GetWardrobe, new StaticRequestHandler(SharedPacketLib.GetWardrobe));
            handlers.Add(Incoming.SaveWardrobe, new StaticRequestHandler(SharedPacketLib.SaveWardrobe));
            handlers.Add(Incoming.OpenInventory, new StaticRequestHandler(SharedPacketLib.GetInventory));
            handlers.Add(Incoming.PetInventary, new StaticRequestHandler(SharedPacketLib.GetPetsInventory));
            Console.WriteLine("Logged " + handlers.Count + " packet handler(s)!");
        }
        #endregion
    }
}
