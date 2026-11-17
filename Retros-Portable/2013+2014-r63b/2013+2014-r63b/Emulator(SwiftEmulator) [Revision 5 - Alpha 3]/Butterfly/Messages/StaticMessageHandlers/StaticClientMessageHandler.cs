namespace Butterfly.Messages.StaticMessageHandlers
{
    using Butterfly.Core;
    using Butterfly.Messages;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    internal class StaticClientMessageHandler
    {
        private static Hashtable handlers;

        internal static void HandlePacket(GameClientMessageHandler handler, ClientMessage message)
        {
            if (handlers.ContainsKey(message.Id))
            {
                StaticRequestHandler handler2 = (StaticRequestHandler) handlers[message.Id];
                handler2(handler);
            }
        }

        internal static void Initialize()
        {
            handlers = new Hashtable(300);
            RegisterPacketLibary();
        }

        internal static void RegisterPacketLibary()
        {
            handlers.Add(Incoming.CheckReleaseMessageEvent, new StaticRequestHandler(SharedPacketLib.CheckHabboRelease));
            handlers.Add(Incoming.InitCrypto, new StaticRequestHandler(SharedPacketLib.InitCrypto));
            handlers.Add(Incoming.SecretKey, new StaticRequestHandler(SharedPacketLib.SecretKey));
            handlers.Add(Incoming.ClientVars, new StaticRequestHandler(SharedPacketLib.setVars));
            handlers.Add(Incoming.UniqueMachineID, new StaticRequestHandler(SharedPacketLib.setUniqueId));
            handlers.Add(Incoming.OpenCatalog, new StaticRequestHandler(SharedPacketLib.GetCatalogIndex));
            handlers.Add(Incoming.OpenCatalogPage, new StaticRequestHandler(SharedPacketLib.GetCatalogPage));
            handlers.Add(Incoming.RedeemVoucher, new StaticRequestHandler(SharedPacketLib.RedeemVoucher));
            handlers.Add(Incoming.PurchaseCatalogItem, new StaticRequestHandler(SharedPacketLib.HandlePurchase));
            handlers.Add(Incoming.PurchaseGift, new StaticRequestHandler(SharedPacketLib.PurchaseGift));
            handlers.Add(Incoming.StartQuiz, new StaticRequestHandler(SharedPacketLib.StartQuiz));
            handlers.Add(Incoming.EndQuiz, new StaticRequestHandler(SharedPacketLib.EndQuiz));
            handlers.Add(Incoming.LoadTalents, new StaticRequestHandler(SharedPacketLib.LoadTalents));
            handlers.Add(Incoming.LoadUser2, new StaticRequestHandler(SharedPacketLib.LoadUser2));
            handlers.Add(Incoming.ExtraVipWindow, new StaticRequestHandler(SharedPacketLib.ExtraVipWindow));
            handlers.Add(Incoming.AnswerQuestion, new StaticRequestHandler(SharedPacketLib.AnswerQuestion));
            handlers.Add(Incoming.CatalogData1, new StaticRequestHandler(SharedPacketLib.GetCataData1));
            handlers.Add(Incoming.CatalogData2, new StaticRequestHandler(SharedPacketLib.GetCataData2));
            handlers.Add(Incoming.BotsInventary, new StaticRequestHandler(SharedPacketLib.GetOwnBots));
            handlers.Add(Incoming.BotComposer, new StaticRequestHandler(SharedPacketLib.PlaceBot));
            handlers.Add(Incoming.SerializeBotInformation, new StaticRequestHandler(SharedPacketLib.BotEditRequest));
            handlers.Add(Incoming.BotDisposer, new StaticRequestHandler(SharedPacketLib.PickBot));
            handlers.Add(Incoming.CheckPetName, new StaticRequestHandler(SharedPacketLib.CheckPetName));
            handlers.Add(Incoming.UpdateAllMount, new StaticRequestHandler(SharedPacketLib.UpdateAllMount));
            handlers.Add(Incoming.CatalogGetRace, new StaticRequestHandler(SharedPacketLib.PetRaces));
            handlers.Add(Incoming.GetRoomForEvents, new StaticRequestHandler(SharedPacketLib.GetRoomAdsRooms));
            handlers.Add(Incoming.Pong, new StaticRequestHandler(SharedPacketLib.Pong));
            handlers.Add(Incoming.Ping, new StaticRequestHandler(SharedPacketLib.Ping));
            handlers.Add(Incoming.SSOTicket, new StaticRequestHandler(SharedPacketLib.SSOLogin));
            handlers.Add(Incoming.OpenHelpTool, new StaticRequestHandler(SharedPacketLib.InitHelpTool));
            handlers.Add(Incoming.CreateTicket, new StaticRequestHandler(SharedPacketLib.SubmitHelpTicket));
            handlers.Add(Incoming.SubmitHelpTicket, new StaticRequestHandler(SharedPacketLib.SubmitHelpTicket));
            handlers.Add(Incoming.SendRoomAlert, new StaticRequestHandler(SharedPacketLib.ModSendRoomAlert));
            handlers.Add(Incoming.PickIssue, new StaticRequestHandler(SharedPacketLib.ModPickTicket));
            handlers.Add(Incoming.BotSpeechList, new StaticRequestHandler(SharedPacketLib.HandleBotSpeechList));
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
            handlers.Add(Incoming.UpdateFriendsState, new StaticRequestHandler(SharedPacketLib.FriendsListUpdate));
            handlers.Add(Incoming.DeleteFriend, new StaticRequestHandler(SharedPacketLib.RemoveBuddy));
            handlers.Add(Incoming.SearchFriend, new StaticRequestHandler(SharedPacketLib.SearchHabbo));
            handlers.Add(Incoming.SendInstantMessenger, new StaticRequestHandler(SharedPacketLib.SendInstantMessenger));
            handlers.Add(Incoming.AcceptRequest, new StaticRequestHandler(SharedPacketLib.AcceptRequest));
            handlers.Add(Incoming.DeclineRequest, new StaticRequestHandler(SharedPacketLib.DeclineRequest));
            handlers.Add(Incoming.FriendRequest, new StaticRequestHandler(SharedPacketLib.RequestBuddy));
            handlers.Add(Incoming.FollowFriend, new StaticRequestHandler(SharedPacketLib.FollowBuddy));
            handlers.Add(Incoming.InviteFriendsToMyRoom, new StaticRequestHandler(SharedPacketLib.SendInstantInvite));
            handlers.Add(Incoming.AddFavourite, new StaticRequestHandler(SharedPacketLib.AddFavorite));
            handlers.Add(Incoming.RemoveFavourite, new StaticRequestHandler(SharedPacketLib.RemoveFavorite));
            handlers.Add(Incoming.GoToHotelView, new StaticRequestHandler(SharedPacketLib.GoToHotelView));
            handlers.Add(Incoming.LoadCategorys, new StaticRequestHandler(SharedPacketLib.GetFlatCats));
            handlers.Add(Incoming.LoadFeaturedRooms, new StaticRequestHandler(SharedPacketLib.GetPubs));
            handlers.Add(Incoming.LoadAllRooms, new StaticRequestHandler(SharedPacketLib.GetPopularRooms));
            handlers.Add(Incoming.HighRatedRooms, new StaticRequestHandler(SharedPacketLib.GetHighRatedRooms));
            handlers.Add(Incoming.RoomsOfMyFriends, new StaticRequestHandler(SharedPacketLib.GetFriendsRooms));
            handlers.Add(Incoming.RoomsWhereMyFriends, new StaticRequestHandler(SharedPacketLib.GetRoomsWithFriends));
            handlers.Add(Incoming.LoadMyRooms, new StaticRequestHandler(SharedPacketLib.GetOwnRooms));
            handlers.Add(Incoming.MyFavs, new StaticRequestHandler(SharedPacketLib.GetFavoriteRooms));
            handlers.Add(Incoming.RecentRooms, new StaticRequestHandler(SharedPacketLib.GetRecentRooms));
            handlers.Add(Incoming.LoadPopularTags, new StaticRequestHandler(SharedPacketLib.GetPopularTags));
            handlers.Add(Incoming.SearchRoomByName, new StaticRequestHandler(SharedPacketLib.PerformSearch));
            handlers.Add(Incoming.LoadHeightMap, new StaticRequestHandler(SharedPacketLib.GetRoomData2));
            handlers.Add(Incoming.AddUserToRoom, new StaticRequestHandler(SharedPacketLib.GetRoomData3));
            handlers.Add(Incoming.AddUserToRoom2, new StaticRequestHandler(SharedPacketLib.GetRoomData3));
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
            handlers.Add(Incoming.ApplyAction, new StaticRequestHandler(SharedPacketLib.Wave));
            handlers.Add(Incoming.ApplySign, new StaticRequestHandler(SharedPacketLib.Sign));
            handlers.Add(Incoming.Sit, new StaticRequestHandler(SharedPacketLib.Sit));
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
            handlers.Add(Incoming.EjectFurni, new StaticRequestHandler(SharedPacketLib.TakeItem));
            handlers.Add(Incoming.MoveOrRotate, new StaticRequestHandler(SharedPacketLib.MoveItem));
            handlers.Add(Incoming.MoveWall, new StaticRequestHandler(SharedPacketLib.MoveWallItem));
            handlers.Add(Incoming.HandleItem, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(Incoming.OneWayGate, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(Incoming.OpenDice, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(Incoming.SaveBranding, new StaticRequestHandler(SharedPacketLib.SaveBranding));
            handlers.Add(Incoming.RunDice, new StaticRequestHandler(SharedPacketLib.TriggerItemDiceSpecial));
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
            handlers.Add(Incoming.AcceptTrade, new StaticRequestHandler(SharedPacketLib.AcceptTrade));
            handlers.Add(Incoming.UnacceptTrade, new StaticRequestHandler(SharedPacketLib.UnacceptTrade));
            handlers.Add(Incoming.ConfirmTrade, new StaticRequestHandler(SharedPacketLib.CompleteTrade));
            handlers.Add(Incoming.SendRespects, new StaticRequestHandler(SharedPacketLib.GiveRespect));
            handlers.Add(Incoming.StartEffect, new StaticRequestHandler(SharedPacketLib.ApplyEffect));
            handlers.Add(Incoming.EnableEffect, new StaticRequestHandler(SharedPacketLib.EnableEffect));
            handlers.Add(Incoming.RedeemExchangeFurni, new StaticRequestHandler(SharedPacketLib.RedeemExchangeFurni));
            handlers.Add(Incoming.PlacePet, new StaticRequestHandler(SharedPacketLib.PlacePet));
            handlers.Add(Incoming.PetInfo, new StaticRequestHandler(SharedPacketLib.GetPetInfo));
            handlers.Add(Incoming.PickupPet, new StaticRequestHandler(SharedPacketLib.PickUpPet));
            handlers.Add(Incoming.RespetPet, new StaticRequestHandler(SharedPacketLib.RespectPet));
            handlers.Add(Incoming.AddPostIt, new StaticRequestHandler(SharedPacketLib.PlacePostIt));
            handlers.Add(Incoming.AddSaddleToPet, new StaticRequestHandler(SharedPacketLib.AddSaddle));
            handlers.Add(Incoming.RemoveSaddle, new StaticRequestHandler(SharedPacketLib.RemoveSaddle));
            handlers.Add(Incoming.MountOnPet, new StaticRequestHandler(SharedPacketLib.Ride));
            handlers.Add(Incoming.SaveWiredEffect, new StaticRequestHandler(SharedPacketLib.SaveWired));
            handlers.Add(Incoming.SaveWiredTrigger, new StaticRequestHandler(SharedPacketLib.SaveWired));
            handlers.Add(Incoming.UserInformation, new StaticRequestHandler(SharedPacketLib.GetUserInfo));
            handlers.Add(Incoming.RecycleAgain, new StaticRequestHandler(SharedPacketLib.RecycleAgain));
            handlers.Add(Incoming.RecycleItem, new StaticRequestHandler(SharedPacketLib.RecycleItems));
            handlers.Add(Incoming.LoadProfile, new StaticRequestHandler(SharedPacketLib.LoadProfile));
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
            handlers.Add(Incoming.TrainPet, new StaticRequestHandler(SharedPacketLib.GetTrainerPanel));
            handlers.Add(Incoming.SaveConditional, new StaticRequestHandler(SharedPacketLib.SaveWiredCondition));
            handlers.Add(Incoming.OpenQuests, new StaticRequestHandler(SharedPacketLib.OpenQuests));
            handlers.Add(Incoming.ActiveQuests, new StaticRequestHandler(SharedPacketLib.StartQuest));
            handlers.Add(Incoming.CancelQuests, new StaticRequestHandler(SharedPacketLib.StopQuest));
            handlers.Add(Incoming.ActiveEndedQuest, new StaticRequestHandler(SharedPacketLib.StartEndedQuest));
            handlers.Add(Incoming.CheckNameChange, new StaticRequestHandler(SharedPacketLib.CheckNameChange));
            handlers.Add(Incoming.SaveNameChange, new StaticRequestHandler(SharedPacketLib.SaveMannequinName));
            handlers.Add(Incoming.MarketplaceCanSell, new StaticRequestHandler(SharedPacketLib.MarketplaceCanSell));
            handlers.Add(Incoming.MarketplacePostItem, new StaticRequestHandler(SharedPacketLib.MarketplacePostItem));
            handlers.Add(Incoming.MarketplaceGetOwnOffers, new StaticRequestHandler(SharedPacketLib.MarketplaceGetOwnOffers));
            handlers.Add(Incoming.MarketplaceTakeBack, new StaticRequestHandler(SharedPacketLib.MarketplaceTakeBack));
            handlers.Add(Incoming.MarketplaceClaimCredits, new StaticRequestHandler(SharedPacketLib.MarketplaceClaimCredits));
            handlers.Add(Incoming.MarketplaceGetOffers, new StaticRequestHandler(SharedPacketLib.MarketplaceGetOffers));
            handlers.Add(Incoming.MarketplacePurchase, new StaticRequestHandler(SharedPacketLib.MarketplacePurchase));
            handlers.Add(Incoming.ListenPreview, new StaticRequestHandler(SharedPacketLib.GetMusicData));
            handlers.Add(Incoming.LoadInvSongs, new StaticRequestHandler(SharedPacketLib.LoadInvSongs));
            handlers.Add(Incoming.LoadJukeSongs, new StaticRequestHandler(SharedPacketLib.LoadJukeSongs));
            handlers.Add(Incoming.AddNewCdToJuke, new StaticRequestHandler(SharedPacketLib.AddNewCdToJuke));
            handlers.Add(Incoming.RemoveCdToJuke, new StaticRequestHandler(SharedPacketLib.RemoveCdToJuke));
            handlers.Add(Incoming.InitStream, new StaticRequestHandler(SharedPacketLib.Stream));
            handlers.Add(Incoming.CreateStream, new StaticRequestHandler(SharedPacketLib.CreateStream));
            handlers.Add(Incoming.StreamLike, new StaticRequestHandler(SharedPacketLib.StreamLike));
            handlers.Add(Incoming.SearchStream, new StaticRequestHandler(SharedPacketLib.Stream));
            handlers.Add(Incoming.OpenGuildPage, new StaticRequestHandler(SharedPacketLib.OpenGuildPage));
            handlers.Add(Incoming.BuyGroup, new StaticRequestHandler(SharedPacketLib.BuyGroup));
            handlers.Add(Incoming.EndConfirmBuy, new StaticRequestHandler(SharedPacketLib.EndConfirmBuy));
            handlers.Add(Incoming.EditGuild, new StaticRequestHandler(SharedPacketLib.EditGuild));
            handlers.Add(Incoming.EditIdentidad, new StaticRequestHandler(SharedPacketLib.EditIdentidad));
            handlers.Add(Incoming.EditPlaca, new StaticRequestHandler(SharedPacketLib.EditPlaca));
            handlers.Add(Incoming.EditColores, new StaticRequestHandler(SharedPacketLib.EditColores));
            handlers.Add(Incoming.EditAjustes, new StaticRequestHandler(SharedPacketLib.EditAjustes));
            handlers.Add(Incoming.PopularGuilds, new StaticRequestHandler(SharedPacketLib.PopularGuilds));
            handlers.Add(Incoming.ExitGuild, new StaticRequestHandler(SharedPacketLib.ExitGuild));
            handlers.Add(Incoming.SendRequestGuild, new StaticRequestHandler(SharedPacketLib.SendRequestGuild));
            handlers.Add(Incoming.LoadMembersPetitions, new StaticRequestHandler(SharedPacketLib.LoadMembersPetitions));
            handlers.Add(Incoming.RejectGuildMember, new StaticRequestHandler(SharedPacketLib.RejectGuildMember));
            handlers.Add(Incoming.AcceptMember, new StaticRequestHandler(SharedPacketLib.AcceptMember));
            handlers.Add(Incoming.UpdateUserToRankGuild, new StaticRequestHandler(SharedPacketLib.UpdateUserToRankGuild));
            handlers.Add(Incoming.UpdateUserFromRankGuild, new StaticRequestHandler(SharedPacketLib.UpdateUserToRankGuild));
            handlers.Add(Incoming.SendFurniGuild, new StaticRequestHandler(SharedPacketLib.SendFurniGuild));
            handlers.Add(Incoming.GetGuildFavorite, new StaticRequestHandler(SharedPacketLib.GetGuildFavorite));
            handlers.Add(Incoming.RemoveGuildFavorite, new StaticRequestHandler(SharedPacketLib.RemoveGuildFavorite));
            handlers.Add(Incoming.GetGames, new StaticRequestHandler(SharedPacketLib.GetGames));
            handlers.Add(Incoming.GetGame, new StaticRequestHandler(SharedPacketLib.GetGame));
            handlers.Add(Incoming.StartPanel, new StaticRequestHandler(SharedPacketLib.StartPanel));
            handlers.Add(Incoming.Game2LeaveGameMessageComposer, new StaticRequestHandler(SharedPacketLib.LeaveGame));
            handlers.Add(Incoming.Game2GameChatMessageComposer, new StaticRequestHandler(SharedPacketLib.TalkGame));
            handlers.Add(Incoming.Game2LoadStageReadyMessageComposer, new StaticRequestHandler(SharedPacketLib.stageReadyMessage));
            handlers.Add(Incoming.Game2SetUserMoveTargetMessageComposer, new StaticRequestHandler(SharedPacketLib.WalkGame));
            handlers.Add(Incoming.Mannequinshit, new StaticRequestHandler(SharedPacketLib.SaveMannequin)); 
            //handlers.Add(Incoming.EjectFurni, new StaticRequestHandler(SharedPacketLib.EjectFurni));
            handlers.Add(Incoming.volumeControl, new StaticRequestHandler(SharedPacketLib.volumeControl));
            handlers.Add(Incoming.EventTracker, new StaticRequestHandler(SharedPacketLib.EventTracker));
            handlers.Add(Incoming.SetRelationshipStatus, new StaticRequestHandler(SharedPacketLib.SetRelationshipsStatus));
            handlers.Add(Incoming.GetRelationshipsProfile, new StaticRequestHandler(SharedPacketLib.GetRelationshipsProfile));

            Logging.WriteLine("Logged " + handlers.Count + " packet handler(s)!");
        }

        private delegate void StaticRequestHandler(GameClientMessageHandler handler);
    }
}

