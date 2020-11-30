using System;

namespace GameLogic.Ai
{
    public class Move
    {
        public bool HasFood;
        public Circle Location { get; }
        public Direction Direction { get; }

        public Move(Circle location, Direction direction)
        {
            Location = location;
            Direction = direction;
        }
    }
}