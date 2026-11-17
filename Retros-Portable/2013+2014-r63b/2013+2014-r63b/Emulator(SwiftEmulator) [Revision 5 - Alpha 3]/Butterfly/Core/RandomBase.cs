namespace Butterfly.Core
{
    using System;

    internal abstract class RandomBase : Random
    {
        internal RandomBase()
        {
        }

        internal RandomBase(int seed) : base(seed)
        {
        }

        protected static int ConvertToInt32(ulong value)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes((ulong) (value & ((ulong) 0x7fffffffL))), 0);
        }

        protected int GetBaseNextInt32()
        {
            return base.Next();
        }

        public abstract override int Next();
        public override int Next(int maxValue)
        {
            return this.Next(0, maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            return Convert.ToInt32((double) (((maxValue - minValue) * this.Sample()) + minValue));
        }

        public override void NextBytes(byte[] buffer)
        {
            if (buffer.Length > 0)
            {
                int num3;
                int index = 0;
                while (index < (buffer.Length - 4))
                {
                    num3 = this.Next();
                    buffer[index] = Convert.ToByte((int) (num3 & 0xff));
                    buffer[index + 1] = Convert.ToByte((int) ((num3 & 0xff00) >> 8));
                    buffer[index + 2] = Convert.ToByte((int) ((num3 & 0xff0000) >> 0x10));
                    buffer[index + 3] = Convert.ToByte((long) ((num3 & 0xff000000L) >> 0x18));
                    index += 4;
                }
                num3 = this.Next();
                for (int i = 0; i < (buffer.Length % 4); i++)
                {
                    buffer[index + i] = Convert.ToByte((int) ((num3 & (0xff << ((8 * i) & 0x1f))) >> (8 * i)));
                }
            }
        }

        public override double NextDouble()
        {
            return this.Sample();
        }

        protected override double Sample()
        {
            return (Convert.ToDouble(this.Next()) / 2147483648);
        }
    }
}

