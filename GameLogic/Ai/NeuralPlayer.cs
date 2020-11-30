using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Ai
{
    public static class NeuralPlayer
    {
        public static SnakeKeys CalculateNextMove(Settings settings, Circle food, Snake snake)
        {
            var distances = new List<(double, double, double)>();
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    
                    var bodyDistance = GetBodyDistance(i, j, snake, settings);
                    var foodDistance = GetFoodDistance(i, j, snake, food, settings);
                    var wallDistance = GetWallDistance(i, j, snake, settings);
                    distances.Add((bodyDistance, foodDistance, wallDistance));
                }
            }



            return SnakeKeys.A;
        }

        private static double GetBodyDistance(int x, int y, Snake snake, Settings settings)
        {
            if (snake.Body.Count == 1)
                return int.MaxValue;
            var head = snake.Body[0];
            var body = snake.Body.GetRange(1, snake.Body.Count - 1);
            for (var i = 0; i < settings.Width; i++)
            {
                var newX = head.X + i * x;
                var newY = head.Y + i * y;
                var circle = new Circle(newX, newY);
                foreach (var part in body)
                {
                    if (circle.Equals(part))
                        return head.DistanceTo(part);
                }

            }


            return 1;
        }

        private static double GetFoodDistance(int x, int y, Snake snake, Circle food, Settings settings)
        {
            var head = snake.Body[0];
            for (var i = 0; i <= settings.Width; i++)
            {
                var newX = head.X + i * x;
                var newY = head.Y + i * y;
                var circle = new Circle(newX, newY);
                if (circle.Equals(food))
                    return head.DistanceTo(food);
            }

            return int.MaxValue;
        }

        private static double GetWallDistance(int x, int y, Snake snake, Settings settings)
        {
            var head = snake.Body[0];

            var x1 = 0;
            if (x == -1)
                x1 = -head.X;
            else if (x == 1)
                x1 = settings.Width - head.X;

            var y1 = 0;
            if (y == -1)
                y1 = -head.Y;
            else if (y == 1)
                y1 = settings.Height - head.Y;

            return Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(y1, 2));
        }
    }
}