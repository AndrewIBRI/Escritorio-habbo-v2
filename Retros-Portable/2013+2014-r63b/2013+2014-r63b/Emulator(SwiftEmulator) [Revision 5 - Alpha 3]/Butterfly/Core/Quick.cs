namespace Butterfly.Core
{
    using System;

    internal class Quick : RandomBase
    {
        private const uint a = 0x4182bed5;
        private ulong i;

        internal Quick() : this(Convert.ToInt32((long) (DateTime.Now.Ticks & 0x7fffffffL)))
        {
        }

        internal Quick(int seed) : base(seed)
        {
            this.i = Convert.ToUInt64(base.GetBaseNextInt32());
        }

        public override int Next()
        {
            this.i = ((ulong) 0x4182bed5L) * this.i;
            return RandomBase.ConvertToInt32(this.i);
        }
    }
}

