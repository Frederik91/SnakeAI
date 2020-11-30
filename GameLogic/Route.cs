using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Ai
{
    public class Route
    {
        private readonly ScoreSetup _scoreSetup;
        public List<Move> Moves { get; set; } = new List<Move>();
        public double DistanceToFood { get; set; } = int.MaxValue;
        public bool WillCrash { get; set; }

        public Route(ScoreSetup scoreSetup)
        {
            _scoreSetup = scoreSetup;
        }

        public double CalculateScore()
        {
            var score = 0.0;
            if (WillCrash)
                score -= _scoreSetup.CrashCost;

            foreach (var move in Moves)
            {
                var foodScore = move.HasFood ? _scoreSetup.FoodReward - Moves.IndexOf(move) : 0;
                score += foodScore;
                score -= _scoreSetup.MoveCost;
            }

            score += _scoreSetup.DistanceToFoodReward / DistanceToFood;

            return score;
        }
    }
}