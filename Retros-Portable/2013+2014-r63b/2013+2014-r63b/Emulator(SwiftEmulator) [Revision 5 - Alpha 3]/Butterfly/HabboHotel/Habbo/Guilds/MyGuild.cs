namespace Butterfly.HabboHotel.Habbo.Guilds
{
    using System;
    using System.Collections.Generic;

    internal class MyGuild
    {
        public int CustomColor1;
        public int CustomColor2;
        public string DateCreated;
        public string Description;
        public int GuildBase;
        public int GuildBaseColor;
        public Dictionary<int, int> GuildRanks = new Dictionary<int, int>();
        public List<int> GuildStates = new List<int>();
        public string HtmlColor1;
        public string HtmlColor2;
        public int Id;
        public string Image;
        public Dictionary<int,string> Members = new Dictionary<int,string>();
        public string Name;
        public int OwnerId;
        public string OwnerName;
        public List<int> Petitions;
        public int RightsType;
        public int RoomId;
        public int Type;

        public MyGuild(int id, string name, int ownerid, string ownername, string description, int roomid, string image, int customcolor1, int customcolor2, int guildbase, int guildbasecolor, List<int> guildstates, string htmlcolor1, string htmlcolor2, string datecreated, Dictionary<int,string> members, List<int> petitions, int type, int rightstype, Dictionary<int, int> guildranks)
        {
            this.Id = id;
            this.Name = name;
            this.OwnerId = ownerid;
            this.OwnerName = ownername;
            this.Description = description;
            this.RoomId = roomid;
            this.Image = image;
            this.CustomColor1 = customcolor1;
            this.CustomColor2 = customcolor2;
            this.GuildBase = guildbase;
            this.GuildBaseColor = guildbasecolor;
            this.GuildStates = guildstates;
            this.HtmlColor1 = htmlcolor1;
            this.HtmlColor2 = htmlcolor2;
            this.DateCreated = datecreated;
            this.Members = members;
            this.Petitions = petitions;
            this.Type = type;
            this.RightsType = rightstype;
            this.GuildRanks = guildranks;
        }
    }
}

