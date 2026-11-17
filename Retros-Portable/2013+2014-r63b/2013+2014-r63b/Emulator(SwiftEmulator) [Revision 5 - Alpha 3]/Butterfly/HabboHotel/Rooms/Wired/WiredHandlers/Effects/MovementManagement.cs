namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects
{
    using Butterfly.HabboHotel.Rooms.Wired;
    using System;
    using System.Drawing;

    internal class MovementManagement
    {
        private Random rnd = new Random();

        private static void HandleClockwiseRotation(ref int rotation)
        {
            rotation += 2;
            if (rotation > 6)
            {
                rotation = 0;
            }
        }

        private static void HandleCounterClockwiseRotation(ref int rotation)
        {
            rotation -= 2;
            if (rotation < 0)
            {
                rotation = 6;
            }
        }

        private static void HandleMovement(ref Point coordinate, MovementState state)
        {
            switch (state)
            {
                case MovementState.up:
                    coordinate.Y--;
                    break;

                case MovementState.right:
                    coordinate.X++;
                    break;

                case MovementState.down:
                    coordinate.Y++;
                    break;

                case MovementState.left:
                    coordinate.X--;
                    break;
            }
        }

        protected Point HandleMovement(Point newCoordinate, MovementState state, int newRotation)
        {
            Point coordinate = new Point(newCoordinate.X, newCoordinate.Y);
            switch (state)
            {
                case MovementState.random:
                    switch (this.rnd.Next(1, 5))
                    {
                        case 1:
                            HandleMovement(ref coordinate, MovementState.up);
                            return coordinate;

                        case 2:
                            HandleMovement(ref coordinate, MovementState.down);
                            return coordinate;

                        case 3:
                            HandleMovement(ref coordinate, MovementState.left);
                            return coordinate;

                        case 4:
                            HandleMovement(ref coordinate, MovementState.right);
                            return coordinate;
                    }
                    return coordinate;

                case MovementState.leftright:
                    if (this.rnd.Next(0, 2) != 1)
                    {
                        HandleMovement(ref coordinate, MovementState.right);
                        return coordinate;
                    }
                    HandleMovement(ref coordinate, MovementState.left);
                    return coordinate;

                case MovementState.updown:
                    if (this.rnd.Next(0, 2) != 1)
                    {
                        HandleMovement(ref coordinate, MovementState.down);
                        return coordinate;
                    }
                    HandleMovement(ref coordinate, MovementState.up);
                    return coordinate;

                case MovementState.up:
                case MovementState.right:
                case MovementState.down:
                case MovementState.left:
                    HandleMovement(ref coordinate, state);
                    return coordinate;
            }
            return coordinate;
        }

        protected int HandleRotation(int oldRotation, RotationState state)
        {
            int rotation = oldRotation;
            switch (state)
            {
                case RotationState.clocwise:
                    HandleClockwiseRotation(ref rotation);
                    return rotation;

                case RotationState.counterClockwise:
                    HandleCounterClockwiseRotation(ref rotation);
                    return rotation;

                case RotationState.random:
                    if (this.rnd.Next(0, 3) != 1)
                    {
                        HandleCounterClockwiseRotation(ref rotation);
                        return rotation;
                    }
                    HandleClockwiseRotation(ref rotation);
                    return rotation;
            }
            return rotation;
        }
    }
}

