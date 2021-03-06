﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameLogic;
using GameLogic.Ai;

namespace RunConsole
{
    class Program
    {
        private static readonly Random _rnd = new Random(DateTime.Now.Millisecond);

        static void Main(string[] args)
        {
            var population = CreatePopulation(5000);

            ScoreSetup bestEver = null;
            var bestEverScore = double.MinValue;
            for (var i = 1; i <= 20; i++)
            {
                var result = RunPopulation(population);
                var lines = result.Select(x => string.Join(";", x.Item1.AIDepthScaling, x.Item1.AIMaxDepth, x.Item1.AIMinDepth, x.Item1.DistanceToFoodReward, x.Item1.CrashCost, x.Item1.FoodReward, x.Item1.MoveCost, x.Item3));
                File.AppendAllLines("data.txt", lines);
                var bestScore = result.Max(x => x.Item2);
                if (bestScore > bestEverScore)
                {
                    bestEverScore = bestScore;
                    bestEver = result.First(x => x.Item2 == bestScore).Item1;
                }
                Console.WriteLine($"Generation {i}: Min: {result.Min(x => x.Item2)} - Avg: {result.Average(x => x.Item2)} - Max: {bestScore}");
                population = CreateNextGeneration(result);
            }

            Console.WriteLine("");
            Console.WriteLine("--------------------------");
            Console.WriteLine("");
            Console.WriteLine("Best setup:");
            Console.WriteLine($"Distance to food reward: {bestEver.DistanceToFoodReward}");
            Console.WriteLine($"Crash cost: {bestEver.CrashCost}");
            Console.WriteLine($"Food reward: {bestEver.FoodReward}");
            Console.WriteLine($"Move cost: {bestEver.MoveCost}");
            Console.WriteLine($"AI depth scaling: {bestEver.AIDepthScaling}");
            Console.WriteLine($"AI max depth: {bestEver.AIMaxDepth}");
            Console.WriteLine($"AI min depth: {bestEver.AIMinDepth}");
        }

        private static List<ScoreSetup> CreateNextGeneration(List<(ScoreSetup, double, int)> population)
        {
            var newPopulation = new List<ScoreSetup>();
            var topChromosomes = new List<ScoreSetup>();
            foreach (var chromosome in population.OrderByDescending(x => x.Item2))
            {
                if (newPopulation.Count > population.Count / 4)
                {
                    var newChromosome = CreateChromosome();
                    var otherChromosome = topChromosomes[_rnd.Next(0, topChromosomes.Count - 1)];
                    Crossover(otherChromosome, newChromosome);

                    newPopulation.Add(newChromosome);
                }
                else
                {
                    newPopulation.Add(chromosome.Item1);
                    topChromosomes.Add(chromosome.Item1);
                    Mutate(chromosome.Item1);
                }
            }

            return newPopulation;
        }


        private static void Crossover(ScoreSetup chromosome, ScoreSetup newChromosome)
        {
            var crossoverStart = _rnd.Next(0, 6);
            var crossoverEnd = _rnd.Next(crossoverStart, 6);
            for (var i = crossoverStart; i <= crossoverEnd; i++)
            {
                switch (i)
                {
                    case 0:
                        newChromosome.CrashCost = chromosome.CrashCost;
                        break;
                    case 1:
                        newChromosome.FoodReward = chromosome.FoodReward;
                        break;
                    case 2:
                        newChromosome.MoveCost = chromosome.MoveCost;
                        break;
                    case 3:
                        newChromosome.DistanceToFoodReward = chromosome.DistanceToFoodReward;
                        break;
                    case 4:
                        newChromosome.AIDepthScaling = chromosome.AIDepthScaling;
                        break;
                    case 5:
                        newChromosome.AIMaxDepth = chromosome.AIMaxDepth;
                        break;
                    case 6:
                        newChromosome.AIMinDepth = chromosome.AIMinDepth;
                        break;
                }
            }

        }


        private static void Mutate(ScoreSetup newChromosome)
        {
            if (_rnd.Next(0, 100) < 30)
                return;

            var gene = _rnd.Next(0, 3);
            switch (gene)
            {
                case 0:
                    newChromosome.CrashCost = GetRandomCrashCost();
                    break;
                case 1:
                    newChromosome.FoodReward = GetRandomFoodReward();
                    break;
                case 2:
                    newChromosome.MoveCost = GetRandomMoveCost();
                    break;
                case 3:
                    newChromosome.DistanceToFoodReward = GetRandomDistanceToFoodReward();
                    break;
                case 4:
                    newChromosome.AIDepthScaling = GetRandomAIDepthScaling();
                    break;
                case 5:
                    newChromosome.AIMaxDepth = GetRandomAIMaxDepth();
                    break;
                case 6:
                    newChromosome.AIMinDepth = GetRandomAIMinDepth();
                    break;
            }
        }

        private static List<(ScoreSetup, double, int)> RunPopulation(IReadOnlyList<ScoreSetup> population)
        {
            var games = new List<Game>();
            foreach (var chromosome in population)
            {
                var game = new Game(chromosome, new Settings { DontSleep = true });
                games.Add(game);
            }

            Parallel.ForEach(games, x => x.Play());

            return games.Select(x =>
            {
                var index = games.IndexOf(x);
                var chromosome = population[index];
                return (chromosome, CalculateFitness(x), x.Settings.Score);
            }).ToList();
        }

        private static double CalculateFitness(Game game)
        {
            return game.Settings.Score;
        }

        private static List<ScoreSetup> CreatePopulation(int size)
        {
            var population = new List<ScoreSetup>();
            for (var i = 0; i < size; i++)
            {
                population.Add(CreateChromosome());
            }

            return population;
        }

        private static ScoreSetup CreateChromosome()
        {
            return new ScoreSetup
            {
                DistanceToFoodReward = GetRandomDistanceToFoodReward(),
                CrashCost = GetRandomCrashCost(),
                FoodReward = GetRandomFoodReward(),
                MoveCost = GetRandomMoveCost(),
                AIDepthScaling = GetRandomAIDepthScaling(),
                AIMaxDepth = GetRandomAIMaxDepth(),
                AIMinDepth = GetRandomAIMinDepth()
            };
        }

        private static int GetRandomMoveCost()
        {
            return _rnd.Next(-1000, 1000);
        }

        private static int GetRandomFoodReward()
        {
            return _rnd.Next(-1000, 1000);
        }

        private static int GetRandomCrashCost()
        {
            return _rnd.Next(-10000, 10000);
        }

        private static int GetRandomDistanceToFoodReward()
        {
            return _rnd.Next(-1000, 1000);
        }

        private static double GetRandomAIDepthScaling()
        {
            return _rnd.Next(0, 100) / 10.0;
        }

        private static int GetRandomAIMaxDepth()
        {
            return _rnd.Next(0, 10);
        }

        private static int GetRandomAIMinDepth()
        {
            return _rnd.Next(0, 5);
        }
    }
}
