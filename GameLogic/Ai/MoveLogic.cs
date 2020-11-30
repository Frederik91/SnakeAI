using System;

namespace GameLogic.Ai
{
    public static class MoveLogic
    {
        public static Direction CalculateDirection(Circle head, Circle nextLocation)
        {
            if (head.X < nextLocation.X)
                return Direction.Left;
            if (head.X > nextLocation.X)
                return Direction.Right;
            if (head.Y < nextLocation.Y)
                return Direction.Up;
            return Direction.Down;

        }

        public static Direction GetRightDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Right;
                case Direction.Down:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Up;
                case Direction.Right:
                    return Direction.Down;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public static Direction GetLeftDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Left;
                case Direction.Down:
                    return Direction.Right;
                case Direction.Left:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Up;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public static Circle GetNextLocation(Circle head, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Circle(head.X, head.Y - 1);
                case Direction.Left:
                    return new Circle(head.X - 1, head.Y);
                case Direction.Right:
                    return new Circle(head.X + 1, head.Y);
                case Direction.Down:
                    return new Circle(head.X, head.Y + 1);
                default:
                    return null;
            }
        }

        public static SnakeKeys GetKey(Direction newDirection)
        {
            switch (newDirection)
            {
                case Direction.Up:
                    return SnakeKeys.W;
                case Direction.Down:
                    return SnakeKeys.S;
                case Direction.Left:
                    return SnakeKeys.A;
                case Direction.Right:
                    return SnakeKeys.D;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newDirection), newDirection, null);
            }
        }
    }
}