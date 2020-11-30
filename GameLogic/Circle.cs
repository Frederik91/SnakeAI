using System;

namespace GameLogic
{
    public class Circle : IEquatable<Circle>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Circle()
        {
            X = 0;
            Y = 0;
        }

        public Circle(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Circle other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Circle) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public double DistanceTo(Circle food)
        {
            return Math.Sqrt((Math.Pow((food.X - X), 2) + Math.Pow((food.Y - Y), 2)));
        }
    }
}
