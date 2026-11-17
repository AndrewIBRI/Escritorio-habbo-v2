namespace Butterfly.Net
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Data;
    using System.Net.Sockets;
    using System.Text;
    using HabboEvents;

    internal class MusConnection
    {
        private byte[] buffer = new byte[0x400];
        private Socket socket;

        internal MusConnection(Socket _socket)
        {
            this.socket = _socket;
            try
            {
                this.socket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(this.OnEvent_RecieveData), this.socket);
            }
            catch
            {
                this.tryClose();
            }
        }

        internal void OnEvent_RecieveData(IAsyncResult iAr)
        {
            try
            {
                int count = 0;
                try
                {
                    count = this.socket.EndReceive(iAr);
                }
                catch
                {
                    this.tryClose();
                    return;
                }
                string data = Encoding.Default.GetString(this.buffer, 0, count);
                if (data.Length > 0)
                {
                    this.processCommand(data);
                }
            }
            catch
            {
            }
            this.tryClose();
        }

        internal void processCommand(string data)
        {
            string str = data.Split(new char[] { Convert.ToChar(1) })[0];
            string s = data.Split(new char[] { Convert.ToChar(1) })[1];

            Console.Title = str + " " + s;

            GameClient clientByUserID = null;
            switch (str.ToLower())
            {
                case "settoken":
                {
                    string str3 = ButterflyEnvironment.globalCrypto.Prime + ":" + ButterflyEnvironment.globalCrypto.Generator;
                    this.socket.Send(ButterflyEnvironment.GetDefaultEncoding().GetBytes(str3));
                    return;
                }
                case "updatecredits":
                    if (s != "ALL")
                    {
                        clientByUserID = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(uint.Parse(s));
                        if (clientByUserID != null)
                        {
                            DataRow row;
                            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                adapter.setQuery("SELECT credits FROM users WHERE id = @userid");
                                adapter.addParameter("userid", (int) clientByUserID.GetHabbo().Id);
                                row = adapter.getRow();
                            }
                            clientByUserID.GetHabbo().Credits = (int) row["credits"];
                            clientByUserID.GetHabbo().UpdateCreditsBalance();
                        }
                    }
                    return;

                case "signout":
                    ButterflyEnvironment.GetGame().GetClientManager().GetClientByUserID(uint.Parse(s)).Disconnect();
                    return;

                case "ha":
                {
                    ServerMessage message = new ServerMessage(Outgoing.BroadcastMessage);
                    message.AppendString(LanguageLocale.GetValue("hotelallert.notice") + "\r\n" + str + "\r\n- Staff");
                    ButterflyEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(message);
                    
                    return;
                }
                case "useralert":
                {
                    string str4 = data.Split(new char[] { Convert.ToChar(1) })[2];
                    string str5 = str4.Split(new char[] { Convert.ToChar(1) })[0];
                    GameClient clientByUsername = null;
                    clientByUsername = ButterflyEnvironment.GetGame().GetClientManager().GetClientByUsername(s);
                    if (clientByUsername != null)
                    {
                        if (str5.Contains("://"))
                        {
                            str4 = str4 + Convert.ToChar(2) + str5;
                        }
                        clientByUsername.SendNotif(str4);
                    }
                    return;
                }
            }
            Logging.WriteLine("Unrecognized MUS packet: " + str + "//" + data);
        }

        internal void tryClose()
        {
            try
            {
                this.socket.Shutdown(SocketShutdown.Both);
                this.socket.Close();
                this.socket.Dispose();
            }
            catch
            {
            }
            this.socket = null;
            this.buffer = null;
        }
    }
}

