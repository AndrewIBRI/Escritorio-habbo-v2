namespace Butterfly.HabboHotel.Rooms
{
    using Butterfly.Messages;
    using HabboEvents;
    using System;
    using System.Text;

    internal class DynamicRoomModel
    {
        internal bool ClubOnly;
        internal int DoorOrientation;
        internal int DoorX;
        internal int DoorY;
        internal double DoorZ;
        internal string Heightmap;
        private bool HeightmapSerialized;
        internal int MapSizeX;
        internal int MapSizeY;
        private bool RelativeSerialized;
        private ServerMessage SerializedHeightmap;
        private ServerMessage SerializedRelativeHeightmap;
        internal short[,] SqFloorHeight;
        internal byte[,] SqSeatRot;
        internal SquareState[,] SqState;
        private RoomModel staticModel;

        internal DynamicRoomModel(RoomModel pModel)
        {
            this.staticModel = pModel;
            this.DoorX = this.staticModel.DoorX;
            this.DoorY = this.staticModel.DoorY;
            this.DoorZ = this.staticModel.DoorZ;
            this.DoorOrientation = this.staticModel.DoorOrientation;
            this.Heightmap = this.staticModel.Heightmap;
            this.MapSizeX = this.staticModel.MapSizeX;
            this.MapSizeY = this.staticModel.MapSizeY;
            this.ClubOnly = this.staticModel.ClubOnly;
            this.Generate();
        }

        internal void AddX()
        {
            this.MapSizeX++;
            this.refreshArrays();
        }

        internal void AddY()
        {
            this.MapSizeY++;
            this.refreshArrays();
        }

        internal void Destroy()
        {
            Array.Clear(this.SqState, 0, this.SqState.Length);
            Array.Clear(this.SqFloorHeight, 0, this.SqFloorHeight.Length);
            Array.Clear(this.SqSeatRot, 0, this.SqSeatRot.Length);
            this.staticModel = null;
            this.Heightmap = null;
            this.SqState = null;
            this.SqFloorHeight = null;
            this.SqSeatRot = null;
        }

        internal void Generate()
        {
            this.SqState = new SquareState[this.MapSizeX, this.MapSizeY];
            this.SqFloorHeight = new short[this.MapSizeX, this.MapSizeY];
            this.SqSeatRot = new byte[this.MapSizeX, this.MapSizeY];
            for (int i = 0; i < this.MapSizeY; i++)
            {
                for (int j = 0; j < this.MapSizeX; j++)
                {
                    if ((j > (this.staticModel.MapSizeX - 1)) || (i > (this.staticModel.MapSizeY - 1)))
                    {
                        this.SqState[j, i] = SquareState.BLOCKED;
                    }
                    else
                    {
                        this.SqState[j, i] = this.staticModel.SqState[j, i];
                        this.SqFloorHeight[j, i] = this.staticModel.SqFloorHeight[j, i];
                        this.SqSeatRot[j, i] = this.staticModel.SqSeatRot[j, i];
                    }
                }
            }
            this.RelativeSerialized = false;
            this.HeightmapSerialized = false;
        }

        internal ServerMessage GetHeightmap()
        {
            if (!this.HeightmapSerialized)
            {
                this.SerializedHeightmap = this.SerializeHeightmap();
                this.HeightmapSerialized = true;
            }
            return this.SerializedHeightmap;
        }

        private ServerMessage GetRelativeHeightmap()
        {
            ServerMessage message = new ServerMessage(Outgoing.RelativeMap);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.MapSizeY; i++)
            {
                for (int j = 0; j < this.MapSizeX; j++)
                {
                    if ((j == this.DoorX) && (i == this.DoorY))
                    {
                        builder.Append(this.DoorZ.ToString());
                    }
                    else if (this.SqState[j, i] == SquareState.BLOCKED)
                    {
                        builder.Append("x");
                    }
                    else
                    {
                        builder.Append(this.SqFloorHeight[j, i].ToString());
                    }
                }
                builder.Append(Convert.ToChar(13).ToString());
            }
            message.AppendString(builder.ToString());
            return message;
        }

        internal void OpenSquare(int x, int y, double z)
        {
            if (z > 9.0)
            {
                z = 9.0;
            }
            if (z < 0.0)
            {
                z = 0.0;
            }
            this.SqFloorHeight[x, y] = (short) z;
            this.SqState[x, y] = SquareState.OPEN;
        }

        internal void refreshArrays()
        {
            SquareState[,] stateArray = new SquareState[this.MapSizeX + 1, this.MapSizeY + 1];
            short[,] numArray = new short[this.MapSizeX + 1, this.MapSizeY + 1];
            byte[,] buffer = new byte[this.MapSizeX + 1, this.MapSizeY + 1];
            for (int i = 0; i < this.MapSizeY; i++)
            {
                for (int j = 0; j < this.MapSizeX; j++)
                {
                    if ((j > (this.staticModel.MapSizeX - 1)) || (i > (this.staticModel.MapSizeY - 1)))
                    {
                        stateArray[j, i] = SquareState.BLOCKED;
                    }
                    else
                    {
                        stateArray[j, i] = this.SqState[j, i];
                        numArray[j, i] = this.SqFloorHeight[j, i];
                        buffer[j, i] = this.SqSeatRot[j, i];
                    }
                }
            }
            this.SqState = stateArray;
            this.SqFloorHeight = numArray;
            this.SqSeatRot = buffer;
            this.RelativeSerialized = false;
            this.HeightmapSerialized = false;
        }

        internal ServerMessage SerializeHeightmap()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.MapSizeY; i++)
            {
                for (int j = 0; j < this.MapSizeX; j++)
                {
                    if (this.SqState[j, i] == SquareState.BLOCKED)
                    {
                        builder.Append("x");
                    }
                    else if ((j == this.DoorX) && (i == this.DoorY))
                    {
                        builder.Append(this.DoorZ.ToString());
                    }
                    else
                    {
                        builder.Append(this.SqFloorHeight[j, i]);
                    }
                }
                builder.Append(Convert.ToChar(13));
            }
            ServerMessage message = new ServerMessage(Outgoing.HeightMap);
            message.AppendString(builder.ToString());
            return message;
        }

        internal ServerMessage SerializeRelativeHeightmap()
        {
            if (!this.RelativeSerialized)
            {
                this.SerializedRelativeHeightmap = this.GetRelativeHeightmap();
                this.RelativeSerialized = true;
            }
            return this.SerializedRelativeHeightmap;
        }

        internal void SetMapsize(int x, int y)
        {
            this.MapSizeX = x;
            this.MapSizeY = y;
            this.refreshArrays();
        }

        internal void SetUpdateState()
        {
            this.RelativeSerialized = false;
            this.HeightmapSerialized = false;
        }
    }
}

