namespace Butterfly.HabboHotel.Habbo.Guilds
{
    using Butterfly;
    using Butterfly.HabboHotel;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Butterfly.Core;

    internal class Guilds
    {
        public Queue deleteGuild = new Queue();
        public Dictionary<int, MyGuild> loadedGuilds = new Dictionary<int, MyGuild>();
        public Queue loadGuild = new Queue();

        public MyGuild AddGuild(int id, string name, int ownerid, string ownername, string description, int roomid, string image, int customcolor1, int customcolor2, int guildbase, int guildbasecolor, List<int> guildstates, string htmlcolor1, string htmlcolor2, string datecreated, Dictionary<int,string> members, List<int> petitions, int type, int rightstype)
        {
            Game game = ButterflyEnvironment.GetGame();
            game.LastGuildId++;
            id = ButterflyEnvironment.GetGame().LastGuildId;
            MyGuild g = new MyGuild(id, ButterflyEnvironment.FilterInjectionChars(name), ownerid, ownername, ButterflyEnvironment.FilterInjectionChars(description), roomid, image, customcolor1, customcolor2, guildbase, guildbasecolor, guildstates, htmlcolor1, htmlcolor2, datecreated, members, petitions, type, rightstype, new Dictionary<int, int>());
            string str = "";
            foreach (int num in guildstates)
            {
                str = str + num + ";";
            }
            str = str.Substring(0, str.Length - 1);
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("INSERT INTO groups (Id, Name, OwnerId, OwnerName, Description, RoomId, Image, CustomColor1, CustomColor2, GuildBase, GuildBaseColor, GuildStates, HtmlColor1, HtmlColor2, DateCreated) VALUES (@id, @name, @ownerid, @ownername, @description, @roomid, @image, @color1, @color2, @guildbase, @guildbasecolor, @guildstates, @htmlcolor1, @htmlcolor2, @datecreated)");
                adapter.addParameter("id", id);
                adapter.addParameter("name", ButterflyEnvironment.FilterInjectionChars(name));
                adapter.addParameter("ownerid", ownerid);
                adapter.addParameter("ownername", ownername);
                adapter.addParameter("description", ButterflyEnvironment.FilterInjectionChars(description));
                adapter.addParameter("roomid", roomid);
                adapter.addParameter("image", image);
                adapter.addParameter("color1", customcolor1);
                adapter.addParameter("color2", customcolor2);
                adapter.addParameter("guildbase", guildbase);
                adapter.addParameter("guildbasecolor", guildbasecolor);
                adapter.addParameter("guildstates", str);
                adapter.addParameter("htmlcolor1", htmlcolor1);
                adapter.addParameter("htmlcolor2", htmlcolor2);
                adapter.addParameter("datecreated", datecreated);
                adapter.runQuery();

                adapter.runFastQuery(string.Concat(new object[] { "UPDATE rooms SET GroupId = '", id, "' WHERE Id = '", roomid, "'" }));
            }
            this.LoadGuild(id,false);
            return g;
        }

        public string GenerateGuildImage(int GuildBase, int GuildBaseColor, List<int> GStates)
        {
            List<int> list = GStates;
            string str = "";
            int num = 0;
            string str2 = "b";
            if (GuildBase.ToString().Length >= 2)
            {
                str2 = str2 + GuildBase;
            }
            else
            {
                str2 = str2 + "0" + GuildBase;
            }
            str = GuildBaseColor.ToString();
            if (str.Length >= 2)
            {
                str2 = str2 + str;
            }
            else if (str.Length <= 1)
            {
                str2 = str2 + "0" + str;
            }
            int num2 = 0;
            if (list[9] != 0)
            {
                num2 = 4;
            }
            else if (list[6] != 0)
            {
                num2 = 3;
            }
            else if (list[3] != 0)
            {
                num2 = 2;
            }
            else if (list[0] != 0)
            {
                num2 = 1;
            }
            int num3 = 0;
            for (int i = 0; i < num2; i++)
            {
                str2 = str2 + "s";
                num = list[num3] - 20;
                if (num.ToString().Length >= 2)
                {
                    str2 = str2 + num;
                }
                else
                {
                    str2 = str2 + "0" + num;
                }
                int num5 = list[1 + num3];
                str = num5.ToString();
                if (str.Length >= 2)
                {
                    str2 = str2 + str;
                }
                else if (str.Length <= 1)
                {
                    str2 = str2 + "0" + str;
                }
                str2 = str2 + list[2 + num3].ToString();
                switch (num3)
                {
                    case 0:
                        num3 = 3;
                        break;

                    case 3:
                        num3 = 6;
                        break;

                    case 6:
                        num3 = 9;
                        break;
                }
            }
            return str2;
        }

        public MyGuild GetGuild(int GuildId)
        {
            MyGuild guild;
            if(loadedGuilds.TryGetValue(GuildId, out guild))
            {
                return guild;
            }
            else
            {
                return this.LoadGuild(GuildId,false);
            }
        }

        public string GetHtmlColor(int Color)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT ExtraData1 FROM groups_elements WHERE Id = '" + Color + "' AND Type = 'Color3'");
                return adapter.getString();
            }
        }

        public MyGuild ReloadGuild(int guildId)
        {
            return this.LoadGuild(guildId, true);
        }

        public MyGuild LoadGuild(int GuildId, bool isReloading)
        {

            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.setQuery("SELECT * FROM groups WHERE Id = '" + GuildId + "' LIMIT 1");
                    DataRow row = adapter.getRow();
                    if (row == null)
                    {
                        return null;
                    }
                    List<int> guildstates = new List<int>();
                    foreach (string str in row["GuildStates"].ToString().Split(new char[] { ';' }))
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(str))
                            {
                                guildstates.Add(int.Parse(str));
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Failed to add guild states for guild ID: " + GuildId);
                        }
                    }
                    List<int> petitions = new List<int>();
                    foreach (string str2 in row["Petitions"].ToString().Split(new char[] { ';' }))
                    {
                        try
                        {
                            int pet = int.Parse(str2);
                            if (!String.IsNullOrEmpty(str2) && !petitions.Contains(pet))
                            {
                                petitions.Add(pet);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Failed to add guild petitions for guild ID: " + GuildId);
                        }
                    }
                    Dictionary<int, int> guildranks = new Dictionary<int, int>();
                    Dictionary<int, string> members = new Dictionary<int, string>();
                    adapter.setQuery("SELECT * FROM user_groups WHERE GroupId = '" + GuildId + "'");
                    DataTable table = adapter.getTable();
                    foreach (DataRow row2 in table.Rows)
                    {
                        members.Add((int)row2["UserId"],(string)row2["GroupDate"]);
                        guildranks.Add((int)row2["UserId"], (int)row2["Rank"]);
                    }
                    MyGuild g = new MyGuild(GuildId, ButterflyEnvironment.FilterInjectionChars(row["Name"].ToString()), (int)row["OwnerId"], row["OwnerName"].ToString(), ButterflyEnvironment.FilterInjectionChars(row["Description"].ToString()), (int)row["RoomId"], row["Image"].ToString(), (int)row["CustomColor1"], (int)row["CustomColor2"], (int)row["GuildBase"], (int)row["GuildBaseColor"], guildstates, row["HtmlColor1"].ToString(), row["HtmlColor2"].ToString(), row["DateCreated"].ToString(), members, petitions, (int)row["Typee"], (int)row["RightsType"], guildranks);
                    this.QueueAddGuild(g);
                    return g;
            }
        }

        public void OnCycle_Remove()
        {
            try
            {
                foreach (int i in loadedGuilds.Keys)
                {
                    this.CyclicGuildDisposer(i);
                }
                this.WorkGuildRemoveQueue();
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "Guilds.OnCycle_GuildRemove Exception --> Not inclusive");
            }
        }

        public void OnCycle_GuildAdd()
        {
            try
            {
                this.WorkAddGuildQueue();       
                ButterflyEnvironment.GetGame().GuildsCycleAdd_ended = true;
            }
            catch (Exception exception)
            {
                Logging.LogThreadException(exception.ToString(), "Guilds.OnCycle_GuildAdd Exception --> Not inclusive");
            }
        }

        public void QueueAddGuild(MyGuild G)
        {
                this.loadGuild.Enqueue(G);
        }

        public void QueueDeleteGuild(MyGuild G)
        {
                this.deleteGuild.Enqueue(G);
        }

        private void CyclicGuildDisposer(int guildId)
        {
            if (!this.loadedGuilds.ContainsKey(guildId))
                return;
            else
            {
                int headQuarter,headQuarterLoaded;
                MyGuild group = GetGuild(guildId);

                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {

                    headQuarter = group.RoomId;
                    adapter.setQuery("SELECT active_users FROM room_active WHERE roomid = " + headQuarter);
                    try
                    {
                        int loadedFactor = adapter.getInteger();
                        if (loadedFactor == 0)
                            headQuarterLoaded = 0;
                        else if (loadedFactor > 0)
                            headQuarterLoaded = 1;
                        else
                        {
                            headQuarterLoaded = 0;
                            Console.WriteLine("Error while trying to flush allocated memory for group " + guildId);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error while trying to flush allocated memory for group " + guildId);
                        headQuarterLoaded = 0;
                    }
                }

                if ((headQuarterLoaded == 0))
                    this.UnloadGuild(guildId);
            }
        }


        public void UnloadGuild(int GuildId)
        {
            MyGuild g = this.GetGuild(GuildId);
            if (g != null)
            {
                this.QueueDeleteGuild(g);
            }
        }

        private void WorkAddGuildQueue()
        {
            try
            {
                lock (loadedGuilds)
                {
                    if (this.loadGuild.Count > 0)
                    {
                        while (this.loadGuild.Count > 0)
                        {
                            MyGuild guild = (MyGuild)this.loadGuild.Dequeue();
                            if (!this.loadedGuilds.ContainsKey(guild.Id))
                            {
                                this.loadedGuilds.Add(guild.Id, guild);
                                //Console.WriteLine("Loaded guild: " + guild.Id + " - " + guild.Name);
                            }
                            else
                            {
                                this.loadedGuilds[guild.Id] = guild;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to load a guild: " + ex.ToString());
            }
        }

        private void WorkGuildRemoveQueue()
        {
            lock (loadedGuilds)
            {
                if (this.deleteGuild.Count > 0)
                {
                    while (this.deleteGuild.Count > 0)
                    {
                        MyGuild guild = (MyGuild)this.deleteGuild.Dequeue();
                        this.loadedGuilds.Remove(guild.Id);
                        //Console.WriteLine("Unloaded guild: " + guild.Id + " - " + guild.Name + ".");
                    }
                }
            }
        }
    }
}
