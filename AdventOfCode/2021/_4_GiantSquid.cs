using AdventOfCode.Core;
using Microsoft.Extensions.Logging;

namespace AdventOfCode._2021
{
    public class _4_GiantSquid : PuzzleBase
    {
        private int[]? _drawnNumbers;
        private List<BingoBoard>? _boards;

        public _4_GiantSquid(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource)
            : base(logger, inputSource, 4)
        {
        }

        public override void Setup(string[] input)
        {
            _drawnNumbers = input[0]
                .Split(',')
                .Select(n => int.Parse(n))
                .ToArray();

            _boards = input
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Chunk(5)
                .Select(c => new BingoBoard(c))
                .ToList();
        }

        public override long SolvePartOne(string[] input)
        {
            foreach (var number in _drawnNumbers!)
            {
                _boards!.ForEach(b => b.DrawNumber(number));

                var winningBoard = _boards.FirstOrDefault(b => b.BingoAchieved());

                if (winningBoard != null)
                    return winningBoard.GetScore(number);
            }

            throw new SolutionFailedException("Bingo was not achieved");
        }

        public override long SolvePartTwo(string[] input)
        {
            BingoBoard? lastBoard = default;
            foreach (var number in _drawnNumbers!)
            {
                var remainingBoards = _boards!.Where(b => !b.BingoAchieved());

                if (lastBoard == default && remainingBoards.Count() == 1)
                {
                    lastBoard = remainingBoards.Single();
                }

                _boards!.ForEach(b => b.DrawNumber(number));

                if (lastBoard != default && lastBoard.BingoAchieved())
                    return lastBoard.GetScore(number);
            }

            throw new SolutionFailedException("Bingo was not achieved");
        }

        private class BingoBoard
        {
            public BingoBoard(string[] input)
            {
                if (input.Length != 5)
                    throw new SolutionFailedException("Unexpected number of bingo board rows");

                numbers = input
                    .SelectMany(r => r
                        .Trim()
                        .Replace("  ", " ")
                        .Split(' ')
                        .Select(n => new BingoNumber(int.Parse(n))))
                    .ToList();

                rows = numbers
                    .Chunk(5)
                    .Select(r => new BingoLine(r))
                    .ToList();
                cols = numbers
                    .Select((value, ix) => new { Number = value, Index = ix })
                    .GroupBy(v => v.Index % 5)
                    .Select(g => new BingoLine(g.Select(v => v.Number).ToArray()))
                    .ToList();
            }

            private IList<BingoNumber> numbers;
            private ICollection<BingoLine> rows;
            private ICollection<BingoLine> cols;

            public void DrawNumber(int number)
            {
                numbers
                    .FirstOrDefault(n => n.Equals(number))
                    ?.Draw();
            }

            public bool BingoAchieved()
                => rows.Any(l => l.AllDrawn) || cols.Any(l => l.AllDrawn);

            public long GetScore(int lastDrawn)
                => numbers.Where(n => !n.Drawn).Select(n => n.Value).Sum() * lastDrawn;

            private struct BingoLine
            {
                public BingoLine(ICollection<BingoNumber> numbers)
                {
                    if (numbers.Count != 5)
                        throw new SolutionFailedException("Unexpected amount of numbers for bingo line");
                    this.numbers = numbers;
                }

                private readonly ICollection<BingoNumber> numbers;

                public bool AllDrawn => numbers.All(n => n.Drawn);
            }
        }

        private class BingoNumber : IEquatable<int>
        {
            public BingoNumber(int value)
            {
                Value = value;
            }

            public int Value { get; }

            private bool _drawn = false;
            public bool Drawn => _drawn;

            public void Draw()
                => _drawn = true;

            public bool Equals(int other)
            {
                return Value == other;
            }
        }
    }
}
