namespace Butterfly.HabboHotel.GameClients
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Habbo.Users;
    using Butterfly.HabboHotel.Misc;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using Butterfly.Messages;
    using Butterfly.Net;
    using Butterfly.Util;
    using ConnectionManager;
    using HabboEvents;
    using System;
    using System.Drawing;
    using System.Threading.Tasks;

    public class GameClient
    {
        internal DateTime bannertimmer;
        private ConnectionInformation Connection;
        internal bool CryptoInitialized = false;
        internal int CurrentRoomUserID;
        internal int DesignedHandler = 1;
        private bool Disconnected = false;
        internal bool gotTheThing = false;
        private Butterfly.HabboHotel.Habbo.Users.Habbo Habbo;
        internal int i = 0;
        private uint Id;
        internal int j = 0;
        internal TaskFactory listaTareas = new TaskFactory();
        internal string MachineId;
        private GameClientMessageHandler MessageHandler;
        internal Point newDoorPos;
        internal GamePacketParser packetParser;
        internal bool SetDoorPos;
        internal int[] table = new int[0x100];
        internal DateTime TimePingedReceived;

        internal GameClient(uint ClientId, ConnectionInformation pConnection)
        {
            this.Id = ClientId;
            this.Connection = pConnection;
            this.SetDoorPos = false;
            this.CurrentRoomUserID = -1;
            this.packetParser = new GamePacketParser(this);
        }

        internal void Disconnect()
        {
            if ((this.GetHabbo() != null) && (this.GetHabbo().GetInventoryComponent() != null))
            {
                this.GetHabbo().GetInventoryComponent().RunDBUpdate();
            }
            if (!this.Disconnected)
            {
                if (this.Connection != null)
                {
                    this.Connection.Dispose();
                }
                this.Disconnected = true;
            }
        }

        internal ConnectionInformation GetConnection()
        {
            return this.Connection;
        }

        internal Butterfly.HabboHotel.Habbo.Users.Habbo GetHabbo()
        {
            return this.Habbo;
        }

        internal GameClientMessageHandler GetMessageHandler()
        {
            return this.MessageHandler;
        }

        internal void HandleConnectionData(ref byte[] data)
        {
            if (data[0] == 0x40)
            {
                int index = 0;
                while (index < data.Length)
                {
                    try
                    {
                        byte[] buffer2 = new byte[3];
                        index++;
                        buffer2[0] = data[index];
                        index++;
                        buffer2[1] = data[index];
                        index++;
                        buffer2[2] = data[index];
                        int num2 = Base64Encoding.DecodeInt32(buffer2);
                        buffer2 = new byte[2];
                        index++;
                        buffer2[0] = data[index];
                        index++;
                        buffer2[1] = data[index];
                        int messageID = Base64Encoding.DecodeInt32(buffer2);
                        byte[] body = new byte[num2 - 2];
                        for (int i = 0; i < body.Length; i++)
                        {
                            index++;
                            body[i] = data[index];
                        }
                        ClientMessage message = new ClientMessage(messageID, body);
                        if (this.MessageHandler == null)
                        {
                            this.InitHandler();
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.HandleException(exception, "packet handling");
                        this.Disconnect();
                    }
                }
            }
            else
            {
                this.Connection.SendData(ButterflyEnvironment.GetDefaultEncoding().GetBytes(CrossdomainPolicy.GetXmlPolicy()));
            }
        }

        internal void InitHandler()
        {
            this.MessageHandler = new GameClientMessageHandler(this);
        }

        private void parser_onNewPacket(ClientMessage Message)
        {
            try
            {
                this.MessageHandler.HandleRequest(Message);
            }
            catch (Exception exception)
            {
                Logging.LogPacketException(Message.ToString(), exception.ToString());
            }
        }

        private void PolicyRequest()
        {
            this.Connection.SendData(ButterflyEnvironment.GetDefaultEncoding().GetBytes(CrossdomainPolicy.GetXmlPolicy()));
        }

        internal void SendBanMessage(string Message)
        {
            ServerMessage message = new ServerMessage(0x23);
            message.AppendString(LanguageLocale.GetValue("moderation.banmessage"), 13);
            message.AppendString(Message);
            this.GetConnection().SendData(message.GetBytes());
        }

        internal void SendBroadcastMessage(string Message)
        {
            ServerMessage message = new ServerMessage(Outgoing.BroadcastMessage);
            message.AppendString(Message);
            message.AppendString("");
            this.SendMessage(message);
        }

        internal void SendMessage(ServerMessage Message)
        {
            if ((Message != null) && (this.GetConnection() != null))
            {
                this.GetConnection().SendData(Message.GetBytes());
                //Console.WriteLine("Inviato -> " + Message.Id);
            }
        }

        internal void SendNotif(string Message)
        {
            this.SendNotif(Message, false);
        }

        internal void SendNotif(string Message, bool FromHotelManager)
        {
            ServerMessage message = new ServerMessage(Outgoing.SetCommandsView);
            message.AppendInt32(1);
            message.AppendString(Message);
            this.SendMessage(message);
        }

        internal void SendNotifWithScroll(string message)
        {
            ServerMessage message2 = new ServerMessage(Outgoing.SetCommandsView);
            message2.AppendInt32(1);
            message2.AppendString("");
            this.SendMessage(message2);
        }

        internal void StartConnection()
        {
            if (this.Connection != null)
            {
                this.TimePingedReceived = DateTime.Now;
                (this.Connection.parser as InitialPacketParser).PolicyRequest += new InitialPacketParser.NoParamDelegate(this.PolicyRequest);
                (this.Connection.parser as InitialPacketParser).SwitchParserRequest += new InitialPacketParser.NoParamDelegate(this.SwitchParserRequest);
                this.Connection.startPacketProcessing();
            }
        }

        internal void Stop()
        {
            if (this.GetMessageHandler() != null)
            {
                this.MessageHandler.Destroy();
            }
            if (this.GetHabbo() != null)
            {
                this.Habbo.OnDisconnect();
            }
            this.CurrentRoomUserID = -1;
            this.MessageHandler = null;
            this.Habbo = null;
            this.Connection = null;
        }

        private void SwitchParserRequest()
        {
            if (this.MessageHandler == null)
            {
                this.InitHandler();
            }
            this.packetParser.SetConnection(this.Connection);
            this.packetParser.onNewPacket += new GamePacketParser.HandlePacket(this.parser_onNewPacket);
            byte[] currentData = (this.Connection.parser as InitialPacketParser).currentData;
            this.Connection.parser.Dispose();
            this.Connection.parser = this.packetParser;
            this.Connection.parser.handlePacketData(currentData);
        }

        internal bool tryLogin(string AuthTicket)
        {
            try
            {
                string ip = this.GetConnection().getIp();
                byte errorCode = 0;
                UserData data = UserDataFactory.GetUserData(AuthTicket, ip, out errorCode);
                switch (errorCode)
                {
                    case 1:
                        this.SendNotifWithScroll(LanguageLocale.GetValue("login.invalidsso"));
                        return false;

                    case 2:
                        this.SendNotifWithScroll(LanguageLocale.GetValue("login.loggedin"));
                        return false;
                }
                ButterflyEnvironment.GetGame().GetClientManager().RegisterClient(this, data.userID, data.user.Username);
                this.Habbo = data.user;
                data.user.LoadData(data);
                if (data.user.Username == null || data.user.Username == "" || data.user.Username == " ")
                {
                    this.SendBanMessage("You have no username.");
                    return false;
                }
                string banReason = ButterflyEnvironment.GetGame().GetBanManager().GetBanReason(data.user.Username, ip);
                if (!string.IsNullOrEmpty(banReason))
                {
                    this.SendBanMessage(banReason);
                    return false;
                }
                data.user.Init(this, data);
                QueuedServerMessage serverMessages = new QueuedServerMessage(this.Connection);
                ServerMessage message = new ServerMessage(Outgoing.UniqueID);
                message.AppendString(this.MachineId);
                serverMessages.appendResponse(message);
                ServerMessage message3 = new ServerMessage(Outgoing.AuthenticationOK);
                serverMessages.appendResponse(message3);
                ServerMessage message4 = new ServerMessage(Outgoing.HomeRoom);
                message4.AppendInt32(this.GetHabbo().HomeRoom);
                message4.AppendInt32(0);
                serverMessages.appendResponse(message4);
                ServerMessage message5 = new ServerMessage(Outgoing.FavouriteRooms);
                message5.AppendInt32(30);
                message5.AppendInt32(data.user.FavoriteRooms.Count);
                foreach (uint num2 in data.user.FavoriteRooms.ToArray())
                {
                    message5.AppendInt32(num2);
                }
                //serverMessages.appendResponse(message5);
                ServerMessage message6 = new ServerMessage(Outgoing.Fuserights);
                if (this.GetHabbo().GetSubscriptionManager().HasSubscription("club_habbo"))
                {
                    message6.AppendInt32(2);
                }
                else if (this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                {
                    message6.AppendInt32(1);
                }
                else
                {
                    message6.AppendInt32(0);
                }
                message6.AppendInt32(this.GetHabbo().Rank);
                serverMessages.appendResponse(message6);
                ServerMessage message7 = new ServerMessage(Outgoing.bools1);
                message7.AppendBoolean(true);
                message7.AppendBoolean(false);
                serverMessages.appendResponse(message7);
                serverMessages.sendResponse();
                if (ButterflyEnvironment.GetGame().GetClientManager().pixelsOnLogin > 0)
                {
                    PixelManager.GivePixels(this, ButterflyEnvironment.GetGame().GetClientManager().pixelsOnLogin);
                }
                if (ButterflyEnvironment.GetGame().GetClientManager().creditsOnLogin > 0)
                {
                    data.user.Credits += ButterflyEnvironment.GetGame().GetClientManager().creditsOnLogin;
                    data.user.UpdateCreditsBalance();
                }
                Logging.WriteLine("[" + this.Habbo.Username + "] logged in");
                ButterflyEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(this, "ACH_EmailVerification", 1);
                if (data.user.HasFuse("fuse_mod"))
                {
                    this.SendMessage(ButterflyEnvironment.GetGame().GetModerationTool().SerializeTool());
                    ButterflyEnvironment.GetGame().GetModerationTool().SerializeOpenTickets(ref serverMessages, data.userID);
                }
                if (LanguageLocale.welcomeAlertEnabled)
                {
                    this.SendBroadcastMessage(LanguageLocale.welcomeAlert);
                    
                }
                return true;
            }
            catch (UserDataNotFoundException exception)
            {
                this.SendNotifWithScroll(LanguageLocale.GetValue("login.invalidsso") + "extra data: " + exception.ToString());
            }
            catch (Exception exception2)
            {
                Logging.LogCriticalException("Invalid Dario bug duing user login: " + exception2.ToString());
                this.SendNotifWithScroll("Login error: " + exception2.ToString());
            }
            return false;
        }

        internal void UnsafeSendMessage(ServerMessage Message)
        {
            if ((Message != null) && (this.GetConnection() != null))
            {
                this.GetConnection().SendUnsafeData(Message.GetBytes());
            }
        }

        internal uint ConnectionID
        {
            get
            {
                return this.Id;
            }
        }
    }
}

