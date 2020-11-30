using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Ai
{
    public static class StupidLogic
    {
        public static SnakeKeys CalculateNextMove(Settings settings, Circle food, Snake snake, ScoreSetup scoreSetup)
        {
            var depth = (int)(snake.Body.Count * scoreSetup.AIDepthScaling + 1);
            if (depth > scoreSetup.AIMaxDepth)
                depth = scoreSetup.AIMaxDepth;
            if (depth < scoreSetup.AIMinDepth)
                depth = scoreSetup.AIMinDepth;

            var route = CalculateRoute(new List<Move>(), settings, food, snake.Clone(), 0, depth, scoreSetup);
            if (route?.Moves.Any() != true)
                return MoveLogic.GetKey(snake.Direction);

            var nextMove = route.Moves[0];
            var key = MoveLogic.GetKey(nextMove.Direction);
            return key;
        }

        private static Route CalculateRoute(List<Move> moves, Settings settings, Circle food, Snake snake, int level, int depth, ScoreSetup scoreSetup)
        {
            if (level == depth)
                return new Route(scoreSetup) { Moves = moves };
            level++;

            var possibleMoves = GetPossibleMoves(snake);

            var routes = new List<Route>();
            foreach (var move in possibleMoves)
            {
                var newSnake = snake.Clone();
                var head = snake.Body[0];
                var lastBody = newSnake.Body.Last();
                newSnake.Body.Insert(0, move.Location);
                if (move.Location.Equals(food))
                {
                    move.HasFood = true;
                    food = new Circle(0, 0);
                }
                if (moves.LastOrDefault()?.HasFood != true)
                    newSnake.Body.Remove(lastBody);

                newSnake.Direction = MoveLogic.CalculateDirection(head, move.Location);
                var newMoves = moves.ToList();
                newMoves.Add(move);

                if (HasCrashed(newSnake, settings))
                {
                    routes.Add(new Route(scoreSetup) { WillCrash = true, Moves = newMoves, DistanceToFood = GetFoodDistance(move.Location, food) });
                    continue;
                }

                var route = CalculateRoute(newMoves, settings, food, newSnake, level, depth, scoreSetup);
                if (route is null)
                    continue;

                route.DistanceToFood = GetFoodDistance(move.Location, food);
                routes.Add(route);
            }

            if (!routes.Any())
                return new Route(scoreSetup) { WillCrash = true };

            var bestMoveFoodDistance = routes.Max(x => x.CalculateScore());
            var bestRoute = routes.FirstOrDefault(x => Math.Abs(x.CalculateScore() - bestMoveFoodDistance) < 0.00001);
            return bestRoute;
        }

        private static bool HasCrashed(Snake snake, Settings setting)
        {
            var head = snake.Body[0];
            var maxXPos = setting.Width;
            var maxYPos = setting.Height;

            if (head.X < 0 || head.Y < 0 || head.X >= maxXPos || head.Y >= maxYPos)
                return true;
            
            // detect collision with body.
            for (var j = 1; j < snake.Body.Count; ++j)
                if (head.X == snake.Body[j].X && head.Y == snake.Body[j].Y)
                    return true;
            
            return false;
        }

        private static List<Move> GetPossibleMoves(Snake snake)
        {
            var head = snake.Body[0];
            var leftDirection = MoveLogic.GetLeftDirection(snake.Direction);
            var left = new Move(MoveLogic.GetNextLocation(head, leftDirection), leftDirection);
            var forward = new Move(MoveLogic.GetNextLocation(head, snake.Direction), snake.Direction);
            var rightDirection = MoveLogic.GetRightDirection(snake.Direction);
            var right = new Move(MoveLogic.GetNextLocation(head, rightDirection), rightDirection);

            var moves = new List<Move> { left, forward, right };
            return moves;
        }

        private static double GetFoodDistance(Circle head, Circle food)
        {
            var xDistance = Math.Abs(head.X - food.X);
            var yDistance = Math.Abs(head.Y - food.Y);
            return xDistance + yDistance;
        }


        
    }
}