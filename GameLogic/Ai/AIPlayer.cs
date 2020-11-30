using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace GameLogic.Ai
{
    public static class AIPlayer
    {
        public static SnakeKeys GetNextMove(Settings settings, Circle food, Snake snake, ScoreSetup scoreSetup)
        {
            return NeuralPlayer.CalculateNextMove(settings, food, snake);
            return StupidLogic.CalculateNextMove(settings, food, snake, scoreSetup);
        }
    }
}