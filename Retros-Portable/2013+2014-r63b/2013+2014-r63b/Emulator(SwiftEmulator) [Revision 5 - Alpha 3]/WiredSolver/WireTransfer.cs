namespace WiredSolver
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class WireTransfer
    {
        private WireCurrentTransfer currentTransfer;

        public WireTransfer(int x, int y, WireCurrentTransfer currentTransfer)
        {
            this.location = new Point(x, y);
            this.currentTransfer = currentTransfer;
            this.Current = CurrentType.OFF;
        }

        internal bool acceptsCurrentFrom(WireCurrentTransfer wireCurrentTransfer)
        {
            return ((this.currentTransfer == WireCurrentTransfer.NONE) || (((wireCurrentTransfer == WireCurrentTransfer.DOWN) && this.transfersCurrentTo(WireCurrentTransfer.DOWN)) || (((wireCurrentTransfer == WireCurrentTransfer.UP) && this.transfersCurrentTo(WireCurrentTransfer.UP)) || (((wireCurrentTransfer == WireCurrentTransfer.LEFT) && this.transfersCurrentTo(WireCurrentTransfer.LEFT)) || ((wireCurrentTransfer == WireCurrentTransfer.RIGHT) && this.transfersCurrentTo(WireCurrentTransfer.RIGHT))))));
        }

        public WireCurrentTransfer getTransfer()
        {
            return this.currentTransfer;
        }

        public bool isPowered()
        {
            return ((this.Current == CurrentType.ON) || (this.Current == CurrentType.SENDER));
        }

        internal void setCurrent(CurrentType currentType)
        {
            this.Current = currentType;
        }

        internal void setCurrentTransfer(WireCurrentTransfer wireCurrentTransfer)
        {
            this.currentTransfer = wireCurrentTransfer;
        }

        internal bool setPower(bool powerIsOn)
        {
            if (powerIsOn && (this.Current == CurrentType.OFF))
            {
                this.Current = CurrentType.ON;
                return true;
            }
            if (!(powerIsOn || (this.Current != CurrentType.ON)))
            {
                this.Current = CurrentType.OFF;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("Location x:{0} y:{1}", this.location.X, this.location.Y);
        }

        internal bool transfersCurrentTo(WireCurrentTransfer current)
        {
            return ((current & this.currentTransfer) == current);
        }

        public CurrentType Current { get; private set; }

        public Point location { get; private set; }
    }
}

