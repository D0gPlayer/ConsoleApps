using ConsoleApps.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.MathGame
{

    public class MathGame
    {
        private enum Operation
        {
            Add = 1,
            Subtract,
            Multiply,
            Divide,
            Random
        }
        private enum Difficulty
        {
            Easy = 1,
            Medium,
            Hard
        }

        private static readonly Dictionary<Operation, string> operations =
            new Dictionary<Operation, string>{{Operation.Add,"+"}, {Operation.Subtract,"-"}, {Operation.Multiply,"*"},
                {Operation.Divide,"/"}, {Operation.Random,"?"}, };

        private static readonly Dictionary<Difficulty, List<int>> difficultyLimits =
            new Dictionary<Difficulty, List<int>>{ {Difficulty.Easy, new List<int>{ 1, 25} },
                                                    {Difficulty.Medium, new List<int>{ 1, 75} },
                                                    {Difficulty.Hard, new List<int>{ 1, 150} } };

        private static readonly SortedDictionary<int, int> scoreTable = new SortedDictionary<int, int>();
        private readonly Random random;
        private int Lives { get; set; }
        private int Score { get; set; }
        private Difficulty DifficultyLevel { get; set; }
        private Stopwatch timer = new Stopwatch();

        public MathGame()
        {
            random = new Random();
            while (true) StartGame();
        }

        private void StartGame()
        {
            ConsoleClr.WriteLine("MathGame", ConsoleColor.Green);
            DisplayScoreTable();
            Lives = 3;
            Score = 0;
            var operation = SelectOperation();
            DifficultyLevel = SelectDifficulty();
            timer.Start();
            while (Lives > 0)
            {
                DisplayGameState();
                if (GenerateQuestion(operation, DifficultyLevel)) Score++;
                else Lives--;
            }

            var gameTime = timer.Elapsed.Seconds;
            if (!scoreTable.TryGetValue(Score, out int seconds)) scoreTable.Add(Score, gameTime);
            else if (gameTime < seconds) scoreTable[Score] = gameTime;

            timer.Reset();
        }

        private void DisplayGameState()
        {
            Console.Clear();
            DisplayTimer();
            var color = ConsoleColor.Green;
            switch (DifficultyLevel)
            {
                case Difficulty.Easy:
                    color = ConsoleColor.Green;
                    break;
                case Difficulty.Medium:
                    color = ConsoleColor.Yellow;
                    break;
                case Difficulty.Hard:
                    color = ConsoleColor.Red;
                    break;
                default:
                    break;
            }
            ConsoleClr.WriteLine($"Difficulty: {DifficultyLevel}", color);
            ConsoleClr.WriteLine($"Lives: {Lives}", ConsoleColor.Green);
            ConsoleClr.WriteLine($"Score: {Score}", ConsoleColor.Yellow);
            Console.WriteLine();
        }

        private void DisplayScoreTable()
        {
            Console.Clear();
            ConsoleClr.WriteLine($"Records:", ConsoleColor.Blue);
            foreach (var score in scoreTable)
            {
                Console.WriteLine($"Score: {score.Key}, Time: {score.Value}");
            }
            Console.WriteLine();
            Console.WriteLine("Press enter to start!");
            Console.ReadLine();
        }

        private void DisplayTimer()
        {
            ConsoleClr.WriteLine($"Time: {timer.Elapsed.Seconds}", ConsoleColor.Blue);
        }

        private Operation SelectOperation()
        {
            Console.WriteLine();
            Console.WriteLine("Select operation:");
            var index = 1;
            foreach (var operand in operations)
            {
                Console.WriteLine($"{index}) {operand.Value}");
                index++;
            }
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int number) || number < 1 || number > operations.Count)
            {
                ConsoleClr.WriteLine($"{input} - Bad input", ConsoleColor.Red);
                Console.WriteLine();
                SelectOperation();
            }
            return (Operation)number;
        }

        private Difficulty SelectDifficulty()
        {
            Console.WriteLine();
            Console.WriteLine("Select difficulty:");
            var index = 0;
            for (index = 1; index < 4; index++)
            {
                Console.WriteLine($"{index}) {(Difficulty)index}");
            }
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int number) || number < 1 || number > 3)
            {
                ConsoleClr.WriteLine($"{input} - Bad input", ConsoleColor.Red);
                Console.WriteLine();
                SelectDifficulty();
            }
            return (Difficulty)number;
        }

        ///<returns>Answered correctly</returns>
        private bool GenerateQuestion(Operation operation, Difficulty difficulty)
        {
            if (operation == Operation.Random)
            {
                operation = (Operation)random.Next(1, operations.Count - 1);
            }
            var operands = GenerateOperands(operation, difficulty);
            if (operation == Operation.Divide)
            {
                while (operands.Item1 % operands.Item2 != 0)
                {
                    operands = GenerateOperands(operation, difficulty);
                }
            }

            Console.WriteLine($"{operands.Item1} {operations[operation]} {operands.Item2}?");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int answer)) return false;
            else
            {
                int actualAnswer = 0;
                switch (operation)
                {
                    case Operation.Add:
                        actualAnswer = operands.Item1 + operands.Item2;
                        break;
                    case Operation.Subtract:
                        actualAnswer = operands.Item1 - operands.Item2;
                        break;
                    case Operation.Multiply:
                        actualAnswer = operands.Item1 * operands.Item2;
                        break;
                    case Operation.Divide:
                        actualAnswer = operands.Item1 / operands.Item2;
                        break;
                    default:
                        break;
                }

                return actualAnswer == answer;
            }

        }

        private Tuple<int, int> GenerateOperands(Operation operation, Difficulty difficulty)
        {
            var limits = difficultyLimits[difficulty];
            var operand1 = random.Next(limits[0], limits[1]);
            var operand2 = random.Next(limits[0], operation == Operation.Multiply ? 11 : limits[1]);
            var maxOperand = Math.Max(operand1, operand2);
            var minOperand = Math.Min(operand1, operand2);
            return Tuple.Create(maxOperand, minOperand);
        }


    }
}
