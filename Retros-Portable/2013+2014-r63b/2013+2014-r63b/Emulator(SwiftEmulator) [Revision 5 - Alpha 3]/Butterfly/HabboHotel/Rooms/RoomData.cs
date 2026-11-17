namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly;
    using Butterfly.Core;
    using Butterfly.HabboHotel.Habbo.Guilds;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    internal class RoomData
    {
        internal bool AllowPets;
        internal bool AllowPetsEating;
        internal bool AllowRightsOverride;
        internal bool AllowWalkthrough;
        internal int Category;
        internal string CCTs;
        internal string Description;
        internal RoomEvent Event;
        internal string Floor;
        internal int FloorThickness;
        internal int GroupId;
        internal bool Hidewall;
        internal uint Id;
        internal string Landscape;
        private RoomModel mModel;
        internal string ModelName;
        private RoomIcon myIcon;
        internal string Name;
        internal string Owner;
        internal int OwnerId;
        internal string Password;
        internal int Score;
        internal int State;
        internal List<string> Tags;
        internal string Type;
        internal int UsersMax;
        internal int UsersNow;
        internal string Wallpaper;
        internal int WallThickness;

        internal RoomData()
        {
        }

        internal void DeadFill(DataRow Row)
        {
            this.Id = Convert.ToUInt32(Row["id"]);
            this.Name = (string) Row["caption"];
            this.Description = (string) Row["description"];
            this.Type = (string) Row["roomtype"];
            this.Owner = (string) Row["owner"];
            string str3 = Row["state"].ToString().ToLower();
            if (str3 != null)
            {
                if (!(str3 == "open"))
                {
                    if (str3 == "password")
                    {
                        this.State = 2;
                        goto Label_00D1;
                    }
                    if (str3 == "locked")
                    {
                    }
                }
                else
                {
                    this.State = 0;
                    goto Label_00D1;
                }
            }
            this.State = 1;
        Label_00D1:
            this.Category = (int) Row["category"];
            if (!string.IsNullOrEmpty(Row["active_users"].ToString()))
            {
                this.UsersNow = (int) Row["active_users"];
            }
            else
            {
                this.UsersNow = 0;
            }
            this.UsersMax = (int) Row["users_max"];
            this.ModelName = (string) Row["model_name"];
            this.CCTs = (string) Row["public_ccts"];
            this.Score = (int) Row["score"];
            this.Tags = new List<string>();
            this.AllowPets = ButterflyEnvironment.EnumToBool(Row["allow_pets"].ToString());
            this.AllowPetsEating = ButterflyEnvironment.EnumToBool(Row["allow_pets_eat"].ToString());
            this.AllowWalkthrough = ButterflyEnvironment.EnumToBool(Row["allow_walkthrough"].ToString());
            this.AllowRightsOverride = ButterflyEnvironment.EnumToBool(Row["allow_rightsoverride"].ToString());
            this.Hidewall = ButterflyEnvironment.EnumToBool(Row["allow_hidewall"].ToString());
            this.Password = (string) Row["password"];
            this.Wallpaper = (string) Row["wallpaper"];
            this.Floor = (string) Row["floor"];
            this.Landscape = (string) Row["landscape"];
            this.Landscape = (string) Row["landscape"];
            this.FloorThickness = (int) Row["floorthickness"];
            Dictionary<int, int> items = new Dictionary<int, int>();
            if (!string.IsNullOrEmpty(Row["icon_items"].ToString()))
            {
                foreach (string str in Row["icon_items"].ToString().Split(new char[] { '|' }))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] strArray = str.Replace('.', ',').Split(new char[] { ',' });
                        int result = 0;
                        int num2 = 0;
                        if (strArray.Length > 1)
                        {
                            int.TryParse(strArray[1], out num2);
                        }
                        else
                        {
                            int.TryParse(strArray[0], out result);
                        }
                        try
                        {
                            if (!items.ContainsKey(result))
                            {
                                items.Add(result, num2);
                            }
                        }
                        catch (Exception exception)
                        {
                            Logging.LogException("Exception: " + exception.ToString() + "[" + str + "]");
                        }
                    }
                }
            }
            this.myIcon = new RoomIcon((int) Row["icon_bg"], (int) Row["icon_fg"], items);
            foreach (string str2 in Row["tags"].ToString().Split(new char[] { ',' }))
            {
                this.Tags.Add(str2);
            }
        }

        internal void Fill(Room Room)
        {
            this.Id = Room.RoomId;
            this.Name = Room.Name;
            this.Description = Room.Description;
            this.Type = Room.Type;
            this.Owner = Room.Owner;
            this.Category = Room.Category;
            this.State = Room.State;
            this.UsersNow = Room.UsersNow;
            this.UsersMax = Room.UsersMax;
            this.ModelName = Room.ModelName;
            this.CCTs = Room.CCTs;
            this.Score = Room.Score;
            this.Tags = new List<string>();
            foreach (string str in Room.Tags.ToArray())
            {
                this.Tags.Add(str);
            }
            this.AllowPets = Room.AllowPets;
            this.AllowPetsEating = Room.AllowPetsEating;
            this.AllowWalkthrough = Room.AllowWalkthrough;
            this.Hidewall = Room.Hidewall;
            this.myIcon = Room.Icon;
            this.Password = Room.Password;
            this.Event = Room.Event;
            this.Wallpaper = Room.Wallpaper;
            this.Floor = Room.Floor;
            this.Landscape = Room.Landscape;
            this.FloorThickness = Room.FloorThickness;
            this.WallThickness = Room.WallThickness;
            this.GroupId = Room.GroupId;
            this.mModel = ButterflyEnvironment.GetGame().GetRoomManager().GetModel(this.ModelName, this.Id);
        }

        internal void Fill(DataRow Row)
        {
            this.Id = Convert.ToUInt32(Row["id"]);
            this.Name = (string) Row["caption"];
            this.Description = (string) Row["description"];
            this.Type = (string) Row["roomtype"];
            this.Owner = (string) Row["owner"];
            this.OwnerId = 0;
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.setQuery("SELECT id FROM users WHERE username = '" + this.Owner + "'");
                int num = adapter.getInteger();
                if (num > 0)
                {
                    this.OwnerId = num;
                }
            }
            string str3 = Row["state"].ToString().ToLower();
            if (str3 != null)
            {
                if (!(str3 == "open"))
                {
                    if (str3 == "password")
                    {
                        this.State = 2;
                        goto Label_0131;
                    }
                    if (str3 == "locked")
                    {
                    }
                }
                else
                {
                    this.State = 0;
                    goto Label_0131;
                }
            }
            this.State = 1;
        Label_0131:
            this.Category = (int) Row["category"];
            if (!string.IsNullOrEmpty(Row["active_users"].ToString()))
            {
                this.UsersNow = (int) Row["active_users"];
            }
            else
            {
                this.UsersNow = 0;
            }
            this.UsersMax = (int) Row["users_max"];
            this.ModelName = (string) Row["model_name"];
            this.CCTs = (string) Row["public_ccts"];
            this.Score = (int) Row["score"];
            this.Tags = new List<string>();
            this.AllowPets = ButterflyEnvironment.EnumToBool(Row["allow_pets"].ToString());
            this.AllowPetsEating = ButterflyEnvironment.EnumToBool(Row["allow_pets_eat"].ToString());
            this.AllowWalkthrough = ButterflyEnvironment.EnumToBool(Row["allow_walkthrough"].ToString());
            this.AllowRightsOverride = ButterflyEnvironment.EnumToBool(Row["allow_rightsoverride"].ToString());
            this.Hidewall = ButterflyEnvironment.EnumToBool(Row["allow_hidewall"].ToString());
            this.Password = (string) Row["password"];
            this.Wallpaper = (string) Row["wallpaper"];
            this.Floor = (string) Row["floor"];
            this.Landscape = (string) Row["landscape"];
            this.FloorThickness = (int) Row["floorthickness"];
            this.WallThickness = (int) Row["wallthickness"];
            this.GroupId = (int) Row["GroupId"];
            Dictionary<int, int> items = new Dictionary<int, int>();
            if (!string.IsNullOrEmpty(Row["icon_items"].ToString()))
            {
                foreach (string str in Row["icon_items"].ToString().Split(new char[] { '|' }))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] strArray = str.Replace('.', ',').Split(new char[] { ',' });
                        int result = 0;
                        int num3 = 0;
                        int.TryParse(strArray[0], out result);
                        if (strArray.Length > 1)
                        {
                            int.TryParse(strArray[1], out num3);
                        }
                        try
                        {
                            if (!items.ContainsKey(result))
                            {
                                items.Add(result, num3);
                            }
                        }
                        catch (Exception exception)
                        {
                            Logging.LogException("Exception: " + exception.ToString() + "[" + str + "]");
                        }
                    }
                }
            }
            this.myIcon = new RoomIcon((int) Row["icon_bg"], (int) Row["icon_fg"], items);
            foreach (string str2 in Row["tags"].ToString().Split(new char[] { ',' }))
            {
                if (!(str2 == ""))
                {
                    this.Tags.Add(str2);
                }
            }
            this.mModel = ButterflyEnvironment.GetGame().GetRoomManager().GetModel(this.ModelName, this.Id);
        }

        internal void FillNull(uint pId)
        {
            this.Id = pId;
            this.Name = "Unknown Room";
            this.Description = "-";
            this.Type = "private";
            this.Owner = "-";
            this.Category = 0;
            this.UsersNow = 0;
            this.UsersMax = 0;
            this.ModelName = "NO_MODEL";
            this.CCTs = "";
            this.Score = 0;
            this.Tags = new List<string>();
            this.AllowPets = true;
            this.AllowPetsEating = false;
            this.AllowWalkthrough = true;
            this.Hidewall = false;
            this.Password = "";
            this.Wallpaper = "0.0";
            this.Floor = "0.0";
            this.Landscape = "0.0";
            this.WallThickness = 0;
            this.FloorThickness = 0;
            this.AllowRightsOverride = false;
            this.myIcon = new RoomIcon(1, 1, new Dictionary<int, int>());
            this.GroupId = 0;
            this.mModel = ButterflyEnvironment.GetGame().GetRoomManager().GetModel(this.ModelName, pId);
        }

        internal void Serialize(ServerMessage Message, bool ShowEvents)
        {
            Message.AppendInt32(this.Id);
            if ((this.Event == null) || !ShowEvents)
            {
                Message.AppendString(this.Name);
                Message.AppendBoolean(true);
                Message.AppendInt32(this.OwnerId);
                Message.AppendString(this.Owner);
                Message.AppendInt32(this.State);
                Message.AppendInt32(this.UsersNow);
                Message.AppendInt32(this.UsersMax);
                Message.AppendString(this.Description);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(this.Score);
                Message.AppendInt32(0);
                Message.AppendInt32(this.Category);
                if (this.GroupId == 0)
                {
                    Message.AppendInt32(0);
                    Message.AppendInt32(0);
                }
                else
                {
                    MyGuild guild = ButterflyEnvironment.GetGame().GetGuilds().GetGuild(this.GroupId);
                    Message.AppendInt32(guild.Id);
                    Message.AppendString(guild.Name);
                    Message.AppendString(guild.Image);
                }
                Message.AppendString("");
                Message.AppendInt32(this.TagCount);
                foreach (string str in this.Tags)
                {
                    Message.AppendString(str);
                }
            }
            else
            {
                Message.AppendBoolean(true);
                Message.AppendString(this.Event.Name);
                Message.AppendString(this.Owner);
                Message.AppendInt32(this.State);
                Message.AppendInt32(this.UsersNow);
                Message.AppendInt32(this.UsersMax);
                Message.AppendString(this.Event.Description);
                Message.AppendBoolean(true);
                Message.AppendBoolean(true);
                Message.AppendInt32(this.Score);
                Message.AppendInt32(this.Event.Category);
                Message.AppendString(this.Event.StartTime);
                Message.AppendInt32(this.Event.Tags.Count);
                foreach (string str in this.Event.Tags.ToArray())
                {
                    Message.AppendString(str);
                }
            }
            Message.AppendInt32(0);
            Message.AppendInt32(0);
            Message.AppendInt32(0);
            Message.AppendBoolean(false);
            Message.AppendBoolean(true);
            Message.AppendInt32(0);
            Message.AppendInt32(0);
            
        }

        internal RoomIcon Icon
        {
            get
            {
                return this.myIcon;
            }
        }

        internal bool IsPublicRoom
        {
            get
            {
                return (this.Type.ToLower() == "public");
            }
        }

        internal RoomModel Model
        {
            get
            {
                if (this.mModel == null)
                {
                    this.mModel = ButterflyEnvironment.GetGame().GetRoomManager().GetModel(this.ModelName, this.Id);
                }
                return this.mModel;
            }
        }

        internal int TagCount
        {
            get
            {
                return this.Tags.Count;
            }
        }
    }
}

