using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using GameLogic.Ai;

namespace GameLogic
{
    public delegate void GameTick();

    public class Game
    {
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        public Timer _timer;

        public ScoreSetup ScoreSetup { get; }

        public Settings Settings { get; }
        public Snake Snake = new Snake();
        private int _lastEatDuration;

        public Circle Food { get; set; }

        public event GameTick Tick;


        public Game(ScoreSetup scoreSetup, Settings settings)
        {
            ScoreSetup = scoreSetup;
            Settings = settings;
            var head = new Circle { X = Settings.Width / 2, Y = Settings.Height / 2 };
            Snake.Body.Add(head);
            GenerateFood();
            _timer = new Timer(1000.0 / Settings.Speed);
            _timer.Elapsed += (s, e) => UpdateScreen();
        }

        public void Play()
        {
            if (Settings.DontSleep)
            {
                RunWithoutSleep();
            }
            else
            {
                _timer.Start();
            }
        }

        private void RunWithoutSleep()
        {
            _timer = null;
            while (!Settings.GameOver)
            {
                UpdateScreen();
            }
        }

        private void GenerateFood()
        {
            while (true)
            {
                Food = new Circle()
                {
                    X = _random.Next(1, Settings.Width - 1),
                    Y = _random.Next(1, Settings.Height - 1)
                };
                if (Snake.Body.TrueForAll(x => !x.Equals(Food)))
                    return;
            }

        }

        private void MoveSnake()
        {
            if (Settings.Duration - _lastEatDuration > Snake.Body.Count * 20)
            {
                Die();
                return;
            }

            for (var i = Snake.Body.Count - 1; i >= 0; i--)
            {
                // move the head.
                if (i == 0)
                {
                    switch (Snake.Direction)
                    {
                        case Direction.Right:
                            Snake.Body[i].X++;
                            break;
                        case Direction.Left:
                            Snake.Body[i].X--;
                            break;
                        case Direction.Up:
                            Snake.Body[i].Y--;
                            break;
                        case Direction.Down:
                            Snake.Body[i].Y++;
                            break;
                    }
                    // get max X and Y positions.
                    var maxXPos = Settings.Width;
                    var maxYPos = Settings.Height;

                    // detect collision with game boundaries.
                    if (Snake.Body[i].X < 0 || Snake.Body[i].Y < 0 || Snake.Body[i].X >= maxXPos || Snake.Body[i].Y >= maxYPos)
                    {
                        Die();
                    }
                    // detect collision with body.
                    for (var j = 1; j < Snake.Body.Count; ++j)
                    {
                        if (Snake.Body[i].X == Snake.Body[j].X && Snake.Body[i].Y == Snake.Body[j].Y)
                        {
                            Die();
                        }
                    }
                    // detect collision with food.
                    if (Snake.Body[i].X == Food.X && Snake.Body[i].Y == Food.Y)
                    {
                        Eat();
                    }
                }
                // move the rest of the body.
                else
                {
                    // set current node to previous node's position.
                    Snake.Body[i].X = Snake.Body[i - 1].X;
                    Snake.Body[i].Y = Snake.Body[i - 1].Y;
                }
            }
        }

        private void UpdateScreen()
        {
            _timer?.Stop();
            // check for game over.
            if (Settings.GameOver)
                return;

            Settings.Duration += 1;

            var key = AIPlayer.GetNextMove(Settings, Food, Snake, ScoreSetup);

            if (key == SnakeKeys.D && Snake.Direction != Direction.Left)
                Snake.Direction = Direction.Right;
            else if (key == SnakeKeys.A && Snake.Direction != Direction.Right)
                Snake.Direction = Direction.Left;
            else if (key == SnakeKeys.W && Snake.Direction != Direction.Down)
                Snake.Direction = Direction.Up;
            else if (key == SnakeKeys.S && Snake.Direction != Direction.Up)
                Snake.Direction = Direction.Down;

            MoveSnake();
            Tick?.Invoke();
            _timer?.Start();
        }

        private void Eat()
        {
            // add circle to the body.
            var food = new Circle { X = Snake.Body[Snake.Body.Count - 1].X, Y = Snake.Body[Snake.Body.Count - 1].Y };
            Snake.Body.Add(food);
            // update score.
            Settings.Score += Settings.Points;
            // create a new food object.
            GenerateFood();
            _lastEatDuration = Settings.Duration;
        }

        private void Die()
        {
            Settings.GameOver = true;
            _timer?.Stop();
        }
    }
}