namespace Butterfly.HabboHotel.Users.Inventory
{
    using Butterfly;
    using Butterfly.HabboHotel.GameClients;
    using Butterfly.HabboHotel.Rooms;
    using Butterfly.HabboHotel.Users.UserDataManagement;
    using Butterfly.Messages;
    using Database_Manager.Database.Session_Details.Interfaces;
    using HabboEvents;
    using System;
    using System.Collections;
    using System.Text;

    internal class AvatarEffectsInventoryComponent
    {
        internal int CurrentEffect;
        private int EffectCount;
        private ArrayList Effects;
        private GameClient mClient;
        private uint UserId;

        internal AvatarEffectsInventoryComponent(uint UserId, GameClient pClient, UserData data)
        {
            this.mClient = pClient;
            this.Effects = new ArrayList();
            this.UserId = UserId;
            this.CurrentEffect = -1;
            this.Effects.Clear();
            StringBuilder builder = new StringBuilder();
            foreach (AvatarEffect effect in data.effects)
            {
                if (!effect.HasExpired)
                {
                    this.Effects.Add(effect);
                    this.EffectCount++;
                }
                else
                {
                    builder.Append(string.Concat(new object[] { "DELETE FROM user_effects WHERE user_id = ", UserId, " AND effect_id = ", effect.EffectId, "; " }));
                }
            }
            if (builder.Length > 0)
            {
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(builder.ToString());
                }
            }
        }

        internal void AddEffect(int EffectId, int Duration)
        {
            using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                adapter.runFastQuery(string.Concat(new object[] { "INSERT INTO user_effects (user_id,effect_id,total_duration,is_activated,activated_stamp) VALUES (", this.UserId, ",", EffectId, ",", Duration, ",0,0)" }));
            }
            this.EffectCount++;
            this.Effects.Add(new AvatarEffect(EffectId, Duration, false, 0.0));
            this.GetClient().GetMessageHandler().GetResponse().Init(Outgoing.AddEffectToInventary);
            this.GetClient().GetMessageHandler().GetResponse().AppendInt32(1);
            this.GetClient().GetMessageHandler().GetResponse().AppendInt32(EffectId);
            this.GetClient().GetMessageHandler().GetResponse().AppendInt32(0);
            this.GetClient().GetMessageHandler().SendResponse();
        }

        internal void ApplyCustomEffect(int EffectId)
        {
            Room userRoom = this.GetUserRoom();
            if (userRoom != null)
            {
                RoomUser roomUserByHabbo = userRoom.GetRoomUserManager().GetRoomUserByHabbo(this.GetClient().GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    this.CurrentEffect = EffectId;
                    ServerMessage message = new ServerMessage(Outgoing.ApplyEffects);
                    message.AppendInt32(roomUserByHabbo.VirtualId);
                    message.AppendInt32(EffectId);
                    message.AppendInt32(0);
                    userRoom.SendMessage(message);
                }
            }
        }

        internal void ApplyEffect(int EffectId)
        {
            if (this.HasEffect(EffectId, true))
            {
                Room userRoom = this.GetUserRoom();
                if (userRoom != null)
                {
                    RoomUser roomUserByHabbo = userRoom.GetRoomUserManager().GetRoomUserByHabbo(this.GetClient().GetHabbo().Id);
                    if (roomUserByHabbo != null)
                    {
                        this.CurrentEffect = EffectId;
                        ServerMessage message = new ServerMessage(Outgoing.ApplyEffects);
                        message.AppendInt32(roomUserByHabbo.VirtualId);
                        message.AppendInt32(EffectId);
                        message.AppendInt32(0);
                        userRoom.SendMessage(message);
                    }
                }
            }
        }

        internal void CheckExpired()
        {
            if (this.Effects.Count > 0)
            {
                foreach (AvatarEffect effect in this.Effects.ToArray())
                {
                    if (effect.HasExpired)
                    {
                        this.StopEffect(effect.EffectId);
                    }
                }
            }
        }

        internal void Dispose()
        {
            this.EffectCount = 0;
            this.Effects.Clear();
            this.mClient = null;
        }

        internal void EnableEffect(int EffectId)
        {
            AvatarEffect effect = this.GetEffect(EffectId, false);
            if (((effect != null) && !effect.HasExpired) && !effect.Activated)
            {
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "UPDATE user_effects SET is_activated = 1, activated_stamp = ", ButterflyEnvironment.GetUnixTimestamp(), " WHERE user_id = ", this.UserId, " AND effect_id = ", EffectId }));
                }
                effect.Activate();
                this.GetClient().GetMessageHandler().GetResponse().Init(Outgoing.EnableEffect);
                this.GetClient().GetMessageHandler().GetResponse().AppendInt32(effect.EffectId);
                this.GetClient().GetMessageHandler().GetResponse().AppendInt32(effect.TotalDuration);
                this.GetClient().GetMessageHandler().SendResponse();
            }
        }

        private GameClient GetClient()
        {
            return this.mClient;
        }

        internal AvatarEffect GetEffect(int EffectId, bool IfEnabledOnly)
        {
            foreach (AvatarEffect effect in this.Effects.ToArray())
            {
                if ((!IfEnabledOnly || effect.Activated) && (effect.EffectId == EffectId))
                {
                    return effect;
                }
            }
            return null;
        }

        private Room GetUserRoom()
        {
            return this.mClient.GetHabbo().CurrentRoom;
        }

        internal bool HasEffect(int EffectId, bool IfEnabledOnly)
        {
            if (((EffectId == -1) || (EffectId == 0x1c)) || (EffectId == 0x1d))
            {
                return true;
            }
            foreach (AvatarEffect effect in this.Effects.ToArray())
            {
                if (((!IfEnabledOnly || effect.Activated) && !effect.HasExpired) && (effect.EffectId == EffectId))
                {
                    return true;
                }
            }
            return false;
        }

        internal ServerMessage Serialize()
        {
            ServerMessage message = new ServerMessage(Outgoing.EffectsInventary);
            message.AppendInt32(this.Count);
            foreach (AvatarEffect effect in this.Effects.ToArray())
            {
                message.AppendInt32(effect.EffectId);
                message.AppendInt32(effect.TotalDuration);
                message.AppendInt32(!effect.Activated ? 1 : 0);
                message.AppendInt32(effect.TimeLeft);
            }
            return message;
        }

        internal void StopEffect(int EffectId)
        {
            AvatarEffect effect = this.GetEffect(EffectId, true);
            if ((effect != null) && effect.HasExpired)
            {
                using (IQueryAdapter adapter = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    adapter.runFastQuery(string.Concat(new object[] { "DELETE FROM user_effects WHERE user_id = ", this.UserId, " AND effect_id = ", EffectId, " AND is_activated = 1" }));
                }
                this.Effects.Remove(effect);
                this.EffectCount--;
                this.GetClient().GetMessageHandler().GetResponse().Init(Outgoing.StopEffect);
                this.GetClient().GetMessageHandler().GetResponse().AppendInt32(EffectId);
                this.GetClient().GetMessageHandler().SendResponse();
                if (this.CurrentEffect >= 0)
                {
                    this.ApplyEffect(-1);
                }
            }
        }

        internal int Count
        {
            get
            {
                return this.Effects.Count;
            }
        }
    }
}

