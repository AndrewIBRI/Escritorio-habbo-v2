namespace Butterfly.Messages.StaticMessageHandlers
{
    using Butterfly.Messages;
    using System;

    internal class SharedPacketLib
    {
        public static void AcceptMember(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("AcceptMember");
            Handler.AcceptMember();
        }
        internal static void GetOwnBots(GameClientMessageHandler handler)
        {
            handler.GetOwnBots();
        }
        internal static void SaveMannequin(GameClientMessageHandler handler)
        {
            handler.SaveMannequin();
        }
        internal static void SaveMannequinName(GameClientMessageHandler handler)
        {
            handler.SaveMannequinName();
        }
        internal static void Dev(GameClientMessageHandler handler)
        {
            handler.GetUserInfo();
        }
        internal static void PlaceBot(GameClientMessageHandler handler)
        {
            handler.PlaceBot();
        }
        internal static void AcceptRequest(GameClientMessageHandler handler)
        {
            //Console.WriteLine("AcceptRequest");
            handler.AcceptRequest();
        }
        internal static void BotEditRequest(GameClientMessageHandler handler)
        {
            handler.BotEditRequest();
        }
        internal static void SetRelationshipsStatus(GameClientMessageHandler handler)
        {
            handler.SetRelationshipsStatus();
        }
        internal static void GetRelationshipsProfile(GameClientMessageHandler handler)
        {
            handler.GetRelationshipsProfile();
        }
        internal static void PickBot(GameClientMessageHandler handler)
        {
            handler.PickBot();
        }
        internal static void HandleBotSpeechList(GameClientMessageHandler handler)
        {
            handler.HandleBotSpeechList();
        }
        internal static void GetRoomAdsRooms(GameClientMessageHandler handler)
        {
            handler.GetRoomAdsRooms();
        }

        internal static void AcceptTrade(GameClientMessageHandler handler)
        {
            handler.AcceptTrade();
        }

        internal static void RecycleAgain(GameClientMessageHandler handler)
        {
            handler.RecycleAgain();
        }

        internal static void AddFavorite(GameClientMessageHandler handler)
        {
            handler.AddFavorite();
        }

        internal static void SaveBranding(GameClientMessageHandler handler)
        {
            handler.SaveBranding();
        }

        public static void AddNewCdToJuke(GameClientMessageHandler Handler)
        {
            Handler.AddNewCdToJuke();
        }

        internal static void AddSaddle(GameClientMessageHandler handler)
        {
            handler.AddSaddle();
        }

        internal static void AnswerDoorbell(GameClientMessageHandler handler)
        {
            handler.AnswerDoorbell();
        }

        internal static void AnswerQuestion(GameClientMessageHandler handler)
        {
            handler.AnswerInfobusPoll();
        }

        internal static void ApplyEffect(GameClientMessageHandler handler)
        {
            handler.ApplyEffect();
        }

        internal static void ApplyRoomEffect(GameClientMessageHandler handler)
        {
            handler.ApplyRoomEffect();
        }
        
        internal static void BotsInventary(GameClientMessageHandler handler)
        {
            handler.BotsInventary();
        }

        internal static void BanUser(GameClientMessageHandler handler)
        {
            handler.BanUser();
        }

        public static void BuyGroup(GameClientMessageHandler Handler)
        {
            Handler.BuyGroup();
        }

        internal static void CanCreateRoom(GameClientMessageHandler handler)
        {
            handler.CanCreateRoom();
        }

        internal static void CanGift(GameClientMessageHandler handler)
        {
            handler.CanGift();
        }

        internal static void ChangeLook(GameClientMessageHandler handler)
        {
            handler.ChangeLook();
        }

        internal static void ChangeMotto(GameClientMessageHandler handler)
        {
            handler.ChangeMotto();
        }

        internal static void CheckHabboRelease(GameClientMessageHandler handler)
        {
            handler.CheckRelease();
        }

        internal static void CheckNameChange(GameClientMessageHandler handler)
        {
            handler.CheckNameToChange();
        }

        internal static void CheckPetName(GameClientMessageHandler handler)
        {
            handler.CheckPetName();
        }

        internal static void ClearRoomLoading(GameClientMessageHandler handler)
        {
            handler.ClearRoomLoading();
        }

        internal static void CommandsPet(GameClientMessageHandler handler)
        {
            handler.CommandsPet();
        }

        internal static void CompleteTrade(GameClientMessageHandler handler)
        {
            handler.CompleteTrade();
        }

        internal static void CreateRoom(GameClientMessageHandler handler)
        {
            handler.CreateRoom();
        }

        internal static void CreateStream(GameClientMessageHandler handler)
        {
            handler.CreateStream();
        }

        internal static void Dance(GameClientMessageHandler handler)
        {
            handler.Dance();
        }

        internal static void DeclineRequest(GameClientMessageHandler handler)
        {
            handler.DeclineRequest();
        }

        internal static void DeletePendingCFH(GameClientMessageHandler handler)
        {
            handler.DeletePendingCFH();
        }

        internal static void DeletePostit(GameClientMessageHandler handler)
        {
            handler.DeletePostit();
        }

        internal static void DeleteRoom(GameClientMessageHandler handler)
        {
            handler.DeleteRoom();
        }

        public static void EditAjustes(GameClientMessageHandler Handler)
        {
            Handler.EditAjustes();
        }

        public static void EditColores(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("EditColors");
            
            Handler.EditColores();
        }

        public static void EditGuild(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("Editguild");
            Handler.EditGuild();
        }

        public static void EditIdentidad(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("EditIdentidad");
            Handler.EditIdentidad();
        }

        public static void EditPlaca(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("EditPlaca");
            Handler.EditPlaca();
        }

        internal static void EnableEffect(GameClientMessageHandler handler)
        {
            handler.EnableEffect();
        }

        public static void EndConfirmBuy(GameClientMessageHandler Handler)
        {
            Handler.EndConfirmBuy();
        }

        public static void EndQuiz(GameClientMessageHandler Handler)
        {
            Handler.EndQuizQuestions();
        }

        internal static void EnterInfobus(GameClientMessageHandler handler)
        {
            handler.EnterInfobus();
        }

        internal static void EnterInquiredRoom(GameClientMessageHandler handler)
        {
            handler.EnterInquiredRoom();
        }

        internal static void enterOnRoom(GameClientMessageHandler handler)
        {
            handler.enterOnRoom();
        }

        public static void ExitGuild(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("ExitGuild");
            Handler.ExitGuild();
        }

        internal static void ExtraVipWindow(GameClientMessageHandler handler)
        {
            handler.ExtraVipWindow();
        }

        internal static void FollowBuddy(GameClientMessageHandler handler)
        {
            handler.FollowBuddy();
        }

        internal static void FriendsListUpdate(GameClientMessageHandler handler)
        {
            handler.FriendsListUpdate();
        }

        internal static void GetAchievements(GameClientMessageHandler handler)
        {
            handler.GetAchievements();
        }

        internal static void GetBadges(GameClientMessageHandler handler)
        {
            handler.GetBadges();
        }

        internal static void GetBalance(GameClientMessageHandler handler)
        {
            handler.GetBalance();
        }

        internal static void GetCataData1(GameClientMessageHandler handler)
        {
            handler.GetCataData1();
        }

        internal static void GetCataData2(GameClientMessageHandler handler)
        {
            handler.GetCataData2();
        }

        internal static void GetCatalogIndex(GameClientMessageHandler handler)
        {
            handler.GetCatalogIndex();
        }

        internal static void GetCatalogPage(GameClientMessageHandler handler)
        {
            handler.GetCatalogPage();
        }

        internal static void GetEvents(GameClientMessageHandler handler)
        {
            handler.GetEvents();
        }

        internal static void GetFavoriteRooms(GameClientMessageHandler handler)
        {
            handler.GetFavoriteRooms();
        }

        internal static void GetFlatCats(GameClientMessageHandler handler)
        {
            handler.GetFlatCats();
        }

        internal static void GetFriendsRooms(GameClientMessageHandler handler)
        {
            handler.GetFriendsRooms();
        }

        public static void GetGame(GameClientMessageHandler Handler)
        {
            Handler.GetGame();
        }

        public static void GetGames(GameClientMessageHandler Handler)
        {
            Handler.GetGames();
        }

        public static void GetGuildFavorite(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("GetGuildFavorite");
            Handler.GetGuildFavorite();
        }

        internal static void GetHelpCategories(GameClientMessageHandler handler)
        {
            handler.GetHelpCategories();
        }

        internal static void GetHighRatedRooms(GameClientMessageHandler handler)
        {
            handler.GetHighRatedRooms();
        }

        internal static void GetInventory(GameClientMessageHandler handler)
        {
            handler.GetInventory();
        }

        internal static void GetMoodlight(GameClientMessageHandler handler)
        {
            handler.GetMoodlight();
        }

        public static void GetMusicData(GameClientMessageHandler Handler)
        {
            Handler.GetMusicData();
        }

        internal static void GetOwnRooms(GameClientMessageHandler handler)
        {
            handler.GetOwnRooms();
        }

        internal static void GetPetInfo(GameClientMessageHandler handler)
        {
            handler.GetPetInfo();
        }

        internal static void GetPetsInventory(GameClientMessageHandler handler)
        {
            handler.GetPetsInventory();
        }

        internal static void GetPopularRooms(GameClientMessageHandler handler)
        {
            handler.GetPopularRooms();
        }

        internal static void GetPopularTags(GameClientMessageHandler handler)
        {
            handler.GetPopularTags();
        }

        internal static void GetPubs(GameClientMessageHandler handler)
        {
            handler.GetPubs();
        }

        internal static void GetRecentRooms(GameClientMessageHandler handler)
        {
            handler.GetRecentRooms();
        }


        internal static void GetRoomData2(GameClientMessageHandler handler)
        {
            handler.GetRoomData2();
        }

        internal static void GetRoomData3(GameClientMessageHandler handler)
        {
            handler.GetRoomData3();
        }

        internal static void GetRoomEditData(GameClientMessageHandler handler)
        {
            handler.GetRoomEditData();
        }

        internal static void GetRoomsWithFriends(GameClientMessageHandler handler)
        {
            handler.GetRoomsWithFriends();
        }

        internal static void GetSubscriptionData(GameClientMessageHandler handler)
        {
            handler.GetSubscriptionData();
        }

        internal static void GetTopicsInCategory(GameClientMessageHandler handler)
        {
            handler.GetTopicsInCategory();
        }

        internal static void GetTrainerPanel(GameClientMessageHandler handler)
        {
            handler.GetTrainerPanel();
        }

        internal static void GetUserBadges(GameClientMessageHandler handler)
        {
            handler.GetUserBadges();
        }

        internal static void GetUserInfo(GameClientMessageHandler handler)
        {
            handler.GetUserInfo();
        }

        internal static void GetUserTags(GameClientMessageHandler handler)
        {
            handler.GetUserTags();
        }

        internal static void GetWardrobe(GameClientMessageHandler handler)
        {
            handler.GetWardrobe();
        }

        internal static void GiveHanditem(GameClientMessageHandler handler)
        {
            handler.GiveHanditem();
        }

        internal static void GiveRespect(GameClientMessageHandler handler)
        {
            handler.GiveRespect();
        }

        internal static void GiveRights(GameClientMessageHandler handler)
        {
            handler.GiveRights();
        }

        internal static void GoToHotelView(GameClientMessageHandler handler)
        {
            handler.GoToHotelView();
        }

        internal static void HandlePurchase(GameClientMessageHandler handler)
        {
            handler.HandlePurchase();
        }

        internal static void IgnoreUser(GameClientMessageHandler handler)
        {
            handler.IgnoreUser();
        }

        internal static void InitCrypto(GameClientMessageHandler handler)
        {
            handler.InitCrypto();
        }

        internal static void InitHelpTool(GameClientMessageHandler handler)
        {
            handler.InitHelpTool();
        }

        internal static void InitTrade(GameClientMessageHandler handler)
        {
            handler.InitTrade();
        }

        internal static void KickBot(GameClientMessageHandler handler)
        {
            handler.KickBot();
        }

        internal static void KickUser(GameClientMessageHandler handler)
        {
            handler.KickUser();
        }

        public static void LeaveGame(GameClientMessageHandler Handler)
        {
            Handler.LeaveGame();
        }

        public static void LoadInvSongs(GameClientMessageHandler Handler)
        {
            Handler.LoadInvSongs();
        }

        public static void LoadJukeSongs(GameClientMessageHandler Handler)
        {
            Handler.LoadJukeSongs();
        }

        public static void LoadMembersPetitions(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("LoadMembersPetition");
            Handler.LoadMembersPetitions();
        }

        public static void RejectGuildMember(GameClientMessageHandler handler)
        {
            //Console.WriteLine("RejectGuildMember");
            handler.RejectGuildMember();
        }

        internal static void LoadProfile(GameClientMessageHandler handler)
        {
            //Console.WriteLine("LoadProfile");
            handler.LoadProfile();
        }

        internal static void LoadTalents(GameClientMessageHandler handler)
        {
            handler.LoadTalents();
        }

        internal static void LoadUser2(GameClientMessageHandler Handler)
        {
            Handler.LoadUser2();
        }

        internal static void LookAt(GameClientMessageHandler handler)
        {
            handler.LookAt();
        }

        internal static void MarketplaceCanSell(GameClientMessageHandler handler)
        {
            handler.MarketplaceCanSell();
        }

        internal static void MarketplaceClaimCredits(GameClientMessageHandler handler)
        {
            handler.MarketplaceClaimCredits();
        }

        internal static void MarketplaceGetOffers(GameClientMessageHandler handler)
        {
            handler.MarketplaceGetOffers();
        }

        internal static void MarketplaceGetOwnOffers(GameClientMessageHandler handler)
        {
            handler.MarketplaceGetOwnOffers();
        }

        internal static void MarketplacePostItem(GameClientMessageHandler handler)
        {
            handler.MarketplacePostItem();
        }

        internal static void MarketplacePurchase(GameClientMessageHandler handler)
        {
            handler.MarketplacePurchase();
        }

        internal static void MarketplaceTakeBack(GameClientMessageHandler handler)
        {
            handler.MarketplaceTakeBack();
        }

        internal static void ModBanUser(GameClientMessageHandler handler)
        {
            handler.ModBanUser();
        }

        internal static void ModCloseTicket(GameClientMessageHandler handler)
        {
            handler.ModCloseTicket();
        }

        internal static void ModGetRoomChatlog(GameClientMessageHandler handler)
        {
            handler.ModGetRoomChatlog();
        }

        internal static void ModGetRoomTool(GameClientMessageHandler handler)
        {
            handler.ModGetRoomTool();
        }

        internal static void ModGetRoomVisits(GameClientMessageHandler handler)
        {
            handler.ModGetRoomVisits();
        }

        internal static void ModGetTicketChatlog(GameClientMessageHandler handler)
        {
            handler.ModGetTicketChatlog();
        }

        internal static void ModGetUserChatlog(GameClientMessageHandler handler)
        {
            handler.ModGetUserChatlog();
        }

        internal static void ModGetUserInfo(GameClientMessageHandler handler)
        {
            handler.ModGetUserInfo();
        }

        internal static void ModKickUser(GameClientMessageHandler handler)
        {
            handler.ModKickUser();
        }

        internal static void ModPerformRoomAction(GameClientMessageHandler handler)
        {
            handler.ModPerformRoomAction();
        }

        internal static void ModPickTicket(GameClientMessageHandler handler)
        {
            handler.ModPickTicket();
        }

        internal static void ModReleaseTicket(GameClientMessageHandler handler)
        {
            handler.ModReleaseTicket();
        }

        internal static void ModSendRoomAlert(GameClientMessageHandler handler)
        {
            handler.ModSendRoomAlert();
        }

        internal static void ModSendUserCaution(GameClientMessageHandler handler)
        {
            handler.ModSendUserCaution();
        }

        internal static void ModSendUserMessage(GameClientMessageHandler handler)
        {
            handler.ModSendUserMessage();
        }

        internal static void Move(GameClientMessageHandler handler)
        {
            handler.Move();
        }

        internal static void MoveItem(GameClientMessageHandler handler)
        {
            handler.MoveItem();
        }

        internal static void MoveWallItem(GameClientMessageHandler handler)
        {
            handler.MoveWallItem();
        }

        internal static void OfferTradeItem(GameClientMessageHandler handler)
        {
            handler.OfferTradeItem();
        }

        internal static void OnRoomUserAdd(GameClientMessageHandler handler)
        {
            handler.OnRoomUserAdd();
        }

        internal static void OpenFlat(GameClientMessageHandler handler)
        {
            handler.OpenFlat();
        }

        public static void OpenGuildPage(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("OpenGuildPage");
            Handler.OpenGuildPage();
        }

        internal static void OpenPostit(GameClientMessageHandler handler)
        {
            handler.OpenPostit();
        }

        internal static void OpenPresent(GameClientMessageHandler handler)
        {
            handler.OpenPresent();
        }

        internal static void OpenQuests(GameClientMessageHandler handler)
        {
            handler.OpenQuests();
        }

        internal static void PerformSearch(GameClientMessageHandler handler)
        {
            handler.PerformSearch();
        }

        internal static void PerformSearch2(GameClientMessageHandler handler)
        {
            handler.PerformSearch2();
        }

        internal static void PetRaces(GameClientMessageHandler handler)
        {
            handler.PetRaces();
        }

        internal static void PickUpPet(GameClientMessageHandler handler)
        {
            handler.PickUpPet();
        }

        internal static void Ping(GameClientMessageHandler handler)
        {
            handler.Ping();
        }

        internal static void PlaceItem(GameClientMessageHandler handler)
        {
            handler.PlaceItem();
        }

        internal static void PlacePet(GameClientMessageHandler handler)
        {
            handler.PlacePet();
        }

        internal static void PlacePostIt(GameClientMessageHandler handler)
        {
            handler.PlacePostIt();
        }

        internal static void Pong(GameClientMessageHandler handler)
        {
            handler.Pong();
        }

        public static void PopularGuilds(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("PopularGuilds");
            Handler.PopularGuilds();
        }

        internal static void PurchaseGift(GameClientMessageHandler handler)
        {
            handler.PurchaseGift();
        }

        internal static void RateRoom(GameClientMessageHandler handler)
        {
            handler.RateRoom();
        }

        internal static void RecycleItems(GameClientMessageHandler handler)
        {
            handler.RecycleItems();
        }

        internal static void RedeemExchangeFurni(GameClientMessageHandler handler)
        {
            handler.RedeemExchangeFurni();
        }

        internal static void RedeemVoucher(GameClientMessageHandler handler)
        {
            handler.RedeemVoucher();
        }

        internal static void RemoveBuddy(GameClientMessageHandler handler)
        {
            handler.RemoveBuddy();
        }

        public static void RemoveCdToJuke(GameClientMessageHandler Handler)
        {
            Handler.RemoveCdToJuke();
        }

        internal static void RemoveFavorite(GameClientMessageHandler handler)
        {
            handler.RemoveFavorite();
        }

        public static void RemoveGuildFavorite(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("RemoveGuildFavorite");
            Handler.RemoveGuildFavorite();
        }

        internal static void RemoveHanditem(GameClientMessageHandler handler)
        {
            handler.RemoveHanditem();
        }

        internal static void RemoveSaddle(GameClientMessageHandler handler)
        {
            handler.RemoveSaddle();
        }

        internal static void ReqLoadRoomForUser(GameClientMessageHandler handler)
        {
            handler.ReqLoadRoomForUser();
        }

        internal static void RequestBuddy(GameClientMessageHandler handler)
        {
            handler.RequestBuddy();
        }

        internal static void RespectPet(GameClientMessageHandler handler)
        {
            handler.RespectPet();
        }

        internal static void Ride(GameClientMessageHandler handler)
        {
            handler.MountOnPet();
        }

        internal static void SavePostit(GameClientMessageHandler handler)
        {
            handler.SavePostit();
        }

        internal static void SaveRoomData(GameClientMessageHandler handler)
        {
            handler.SaveRoomData();
        }

        internal static void SaveWardrobe(GameClientMessageHandler handler)
        {
            handler.SaveWardrobe();
        }

        internal static void SaveWired(GameClientMessageHandler handler)
        {
            handler.SaveWired();
        }

        internal static void SaveWiredCondition(GameClientMessageHandler handler)
        {
            handler.SaveWiredConditions();
        }

        internal static void SearchHabbo(GameClientMessageHandler handler)
        {
            handler.SearchHabbo();
        }

        internal static void SearchHelpTopics(GameClientMessageHandler handler)
        {
            handler.SearchHelpTopics();
        }

        internal static void SecretKey(GameClientMessageHandler handler)
        {
            handler.InitSecretKey();
        }

        public static void SendFurniGuild(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("SendFurniGuild");
            Handler.SendFurniGuild();
        }

        internal static void SendInstantInvite(GameClientMessageHandler handler)
        {
            handler.SendInstantInvite();
        }

        internal static void SendInstantMessenger(GameClientMessageHandler handler)
        {
            handler.SendInstantMessenger();
        }

        public static void SendRequestGuild(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("SendRequestGuild");
            Handler.SendRequestGuild();
        }

        internal static void SetHomeRoom(GameClientMessageHandler handler)
        {
            handler.SetHomeRoom();
        }

        internal static void SetLookTransfer(GameClientMessageHandler handler)
        {
            handler.SetLookTransfer();
        }

        internal static void setUniqueId(GameClientMessageHandler handler)
        {
            handler.setUniqueIDToClient();
        }

        internal static void setVars(GameClientMessageHandler handler)
        {
            handler.setClientVars();
        }

        internal static void Shout(GameClientMessageHandler handler)
        {
            handler.Shout();
        }

        internal static void Sign(GameClientMessageHandler handler)
        {
            handler.Sign();
        }

        internal static void Sit(GameClientMessageHandler handler)
        {
            handler.Sit();
        }

        internal static void SSOLogin(GameClientMessageHandler handler)
        {
            handler.SSOLogin();
        }

        internal static void StartEndedQuest(GameClientMessageHandler handler)
        {
            handler.ActiveEndedQuest();
        }

        public static void StartPanel(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("StartPanel");
            Handler.StartPanel();
        }

        public static void stageReadyMessage(GameClientMessageHandler handler)
        {
            handler.stageReadyMessage();
        }

        internal static void StartQuest(GameClientMessageHandler handler)
        {
            handler.StartQuest();
        }

        public static void StartQuiz(GameClientMessageHandler Handler)
        {
            Handler.StartQuiz();
        }

        internal static void StartTyping(GameClientMessageHandler handler)
        {
            handler.StartTyping();
        }

        internal static void StopQuest(GameClientMessageHandler handler)
        {
            handler.StopQuest();
        }

        internal static void StopTrade(GameClientMessageHandler handler)
        {
            handler.StopTrade();
        }

        internal static void StopTyping(GameClientMessageHandler handler)
        {
            handler.StopTyping();
        }

        internal static void Stream(GameClientMessageHandler handler)
        {
            handler.InitStream();
        }

        internal static void StreamLike(GameClientMessageHandler handler)
        {
            handler.StreamLike();
        }

        internal static void SubmitHelpTicket(GameClientMessageHandler handler)
        {
            handler.SubmitHelpTicket();
        }

        internal static void SwitchMoodlightStatus(GameClientMessageHandler handler)
        {
            handler.SwitchMoodlightStatus();
        }

        internal static void TakeAllRights(GameClientMessageHandler handler)
        {
            handler.TakeAllRights();
        }

        internal static void TakeBackTradeItem(GameClientMessageHandler handler)
        {
            handler.TakeBackTradeItem();
        }

        internal static void TakeItem(GameClientMessageHandler handler)
        {
            handler.TakeItem();
        }

        internal static void TakeRights(GameClientMessageHandler handler)
        {
            handler.TakeRights();
        }

        internal static void Talk(GameClientMessageHandler handler)
        {
            handler.Talk();
        }

        public static void TalkGame(GameClientMessageHandler Handler)
        {
            Handler.TalkGame();
        }

        internal static void TriggerItem(GameClientMessageHandler handler)
        {
            handler.TriggerItem();
        }

        internal static void TriggerItemDiceSpecial(GameClientMessageHandler handler)
        {
            handler.TriggerItemDiceSpecial();
        }

        internal static void UnacceptTrade(GameClientMessageHandler handler)
        {
            handler.UnacceptTrade();
        }

        internal static void UnignoreUser(GameClientMessageHandler handler)
        {
            handler.UnignoreUser();
        }

        internal static void Unride(GameClientMessageHandler handler)
        {
            handler.CancelMountOnPet();
        }

        internal static void UpdateAllMount(GameClientMessageHandler handler)
        {
            handler.UpdateAllMount();
        }

        internal static void UpdateBadges(GameClientMessageHandler handler)
        {
            handler.UpdateBadges();
        }

        internal static void UpdateMoodlight(GameClientMessageHandler handler)
        {
            handler.UpdateMoodlight();
        }

        public static void UpdateUserToRankGuild(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("UpdateUserToRankGuild");
            Handler.UpdateUserToRankGuild();
        }

        public static void UpdateUserFromRankGuild(GameClientMessageHandler Handler)
        {
            //Console.WriteLine("UpdateUserFromRank");
            Handler.UpdateUserFromRankGuild();
        }

        internal static void ViewHelpTopic(GameClientMessageHandler handler)
        {
            handler.ViewHelpTopic();
        }

        public static void WalkGame(GameClientMessageHandler Handler)
        {
            Handler.WalkGame();
        }

        internal static void Wave(GameClientMessageHandler handler)
        {
            handler.Wave();
        }

        internal static void Whisper(GameClientMessageHandler handler)
        {
            handler.Whisper();
        }
        
        public static void EjectFurni(GameClientMessageHandler handler)
        {
            handler.EjectFurni();
        }
        
        public static void volumeControl(GameClientMessageHandler handler)
        {
            handler.volumeControl();
        }

        public static void EventTracker(GameClientMessageHandler handler)
        {
            handler.EventTracker();
        }
    }
}

