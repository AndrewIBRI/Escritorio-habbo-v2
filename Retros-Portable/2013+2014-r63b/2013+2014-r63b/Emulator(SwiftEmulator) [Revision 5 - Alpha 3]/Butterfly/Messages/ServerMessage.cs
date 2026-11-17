namespace Butterfly.Messages
{
    using Butterfly;
    using System;
    using System.Collections.Generic;

    internal class ServerMessage
    {
        private List<byte> Message;
        private int MessageId;

        public ServerMessage()
        {
            this.Message = new List<byte>();
            this.MessageId = 0;
        }

        public ServerMessage(int Header)
        {
            this.Message = new List<byte>();
            this.MessageId = 0;
            this.Init(Header);
        }

        public void AppendBoolean(bool b)
        {
            this.AppendBytes(new byte[] { b ? ((byte) 1) : ((byte) 0) }, false);
        }

        public void AppendBytes(byte[] b, bool IsInt)
        {
            if (IsInt)
            {
                for (int i = b.Length - 1; i > -1; i--)
                {
                    this.Message.Add(b[i]);
                }
            }
            else
            {
                this.Message.AddRange(b);
            }
        }

        public List<byte> AppendBytesTo(byte[] b, bool IsInt)
        {
            List<byte> list = new List<byte>();
            if (IsInt)
            {
                for (int i = b.Length - 1; i > -1; i--)
                {
                    list.Add(b[i]);
                }
                return list;
            }
            list.AddRange(b);
            return list;
        }

        public void AppendInt32(int i)
        {
            this.AppendBytes(BitConverter.GetBytes(i), true);
        }

        public void AppendInt32(uint i)
        {
            this.AppendBytes(BitConverter.GetBytes(i), true);
        }

        public void AppendShort(int i)
        {
            short num = (short) i;
            this.AppendBytes(BitConverter.GetBytes(num), true);
        }

        public void AppendString(string s)
        {
            this.AppendShort(s.Length);
            this.AppendBytes(ButterflyEnvironment.GetDefaultEncoding().GetBytes(s), false);
        }

        public void AppendString(string s, byte BreakChar)
        {
            this.AppendString(s);
        }

        public byte[] GetBytes()
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(this.Message.Count));
            list.Reverse();
            list.AddRange(this.Message);
            return list.ToArray();
        }

        public void Init(int Header)
        {
            this.Message = new List<byte>();
            this.MessageId = Header;
            this.AppendShort(Header);
        }

        public void setInt(int i, int startOn)
        {
            try
            {
                List<byte> message = new List<byte>();
                message = this.Message;
                List<byte> collection = this.AppendBytesTo(BitConverter.GetBytes(i), true);
                message.RemoveRange(startOn, collection.Count);
                message.InsertRange(startOn, collection);
                this.Message = message;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error on setInt: " + exception.ToString());
            }
        }

        public override string ToString()
        {
            return ButterflyEnvironment.GetDefaultEncoding().GetString(this.GetBytes()).Replace(Convert.ToChar(0).ToString(), "[0]").Replace(Convert.ToChar(1).ToString(), "[1]").Replace(Convert.ToChar(2).ToString(), "[2]").Replace(Convert.ToChar(3).ToString(), "[3]").Replace(Convert.ToChar(4).ToString(), "[4]").Replace(Convert.ToChar(5).ToString(), "[5]").Replace(Convert.ToChar(6).ToString(), "[6]").Replace(Convert.ToChar(7).ToString(), "[7]").Replace(Convert.ToChar(8).ToString(), "[8]").Replace(Convert.ToChar(9).ToString(), "[9]");
        }

        public int Id
        {
            get
            {
                return this.MessageId;
            }
        }
    }
}

