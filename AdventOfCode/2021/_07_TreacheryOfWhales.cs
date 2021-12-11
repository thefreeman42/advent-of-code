using AdventOfCode.Core;
using Microsoft.Extensions.Logging;

namespace AdventOfCode._2021
{
    public class _07_TreacheryOfWhales : PuzzleBase
    {
        private int[]? _initialCrabPositions;
        private int _minimumPosition;
        private int _maximumPosition;

        public _07_TreacheryOfWhales(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource)
            : base(logger, inputSource, 7)
        {
        }

        public override void Setup(string[] input)
        {
            _initialCrabPositions = input
                .Single()
                .Trim()
                .Split(',')
                .Select(p => int.Parse(p))
                .ToArray();
            _minimumPosition = _initialCrabPositions.Min();
            _maximumPosition = _initialCrabPositions.Max();
        }

        public override long SolvePartOne(string[] input)
        {
            return FindOptimalPosition(pos =>
                (p => Math.Abs(pos - p)));
        }

        public override long SolvePartTwo(string[] input)
        {
            return FindOptimalPosition(pos =>
                (p => GetTriangleNumber(Math.Abs(pos - p))));
        }

        private int FindOptimalPosition(Func<int, Func<int, int>> transformDelegate)
        {
            var minUsage = int.MaxValue;
            foreach (var pos in Enumerable.Range(_minimumPosition, (_maximumPosition - _minimumPosition + 1)))
            {
                var fuelUsage = _initialCrabPositions!.Select(transformDelegate.Invoke(pos)).Sum();
                if (fuelUsage < minUsage) minUsage = fuelUsage;
            }

            return minUsage;
        }

        private int GetTriangleNumber(int value)
            => (value * (value + 1)) / 2;
    }
}
