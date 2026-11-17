namespace WiredSolver
{
    using System;

    [Flags]
    public enum WireCurrentTransfer
    {
        DOWN = 4,
        LEFT = 8,
        NONE = 0,
        RIGHT = 2,
        UP = 1
    }
}

