using AdventOfCode.Core;
using Microsoft.Extensions.Logging;

namespace AdventOfCode._2021
{
    public class _2_Dive : PuzzleBase
    {
        private SubmarineMovement[]? _movements;

        public _2_Dive(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource)
            : base(logger, inputSource, 2)
        {
        }

        public override void Setup(string[] input)
        {
            _movements = input.Select(line => new SubmarineMovement(line)).ToArray();
        }

        public override long SolvePartOne(string[] input)
        {
            var horizontalPos = 0;
            var depth = 0;

            foreach (var movement in _movements!)
                switch (movement.Direction)
                {
                    case SubmarineMovementDirection.Forward:
                        horizontalPos += movement.Distance;
                        break;
                    case SubmarineMovementDirection.Up:
                        depth -= movement.Distance;
                        break;
                    case SubmarineMovementDirection.Down:
                        depth += movement.Distance;
                        break;
                };

            return horizontalPos * depth;
        }

        public override long SolvePartTwo(string[] input)
        {
            var horizontalPos = 0;
            var depth = 0;
            var aim = 0;

            foreach (var movement in _movements!)
                switch (movement.Direction)
                {
                    case SubmarineMovementDirection.Forward:
                        horizontalPos += movement.Distance;
                        depth += (aim * movement.Distance);
                        break;
                    case SubmarineMovementDirection.Up:
                        aim -= movement.Distance;
                        break;
                    case SubmarineMovementDirection.Down:
                        aim += movement.Distance;
                        break;
                };

            return horizontalPos * depth;
        }

        private struct SubmarineMovement
        {
            private const string ForwardString = "forward";
            private const string UpString = "up";
            private const string DownString = "down";

            public SubmarineMovement(string input)
            {
                var split = input.Trim().Split(' ');
                Direction = split[0] switch
                {
                    ForwardString => SubmarineMovementDirection.Forward,
                    UpString => SubmarineMovementDirection.Up,
                    DownString => SubmarineMovementDirection.Down,
                    _ => throw new SolutionFailedException("Unexpected submarine movement direction received")
                };
                Distance = int.Parse(split[1]);
            }

            public SubmarineMovementDirection Direction { get; }
            public int Distance { get; }
        }

        private enum SubmarineMovementDirection
        {
            Forward,
            Up,
            Down
        }
    }
}
