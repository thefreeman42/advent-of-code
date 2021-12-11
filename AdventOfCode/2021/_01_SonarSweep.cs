using AdventOfCode.Core;
using Microsoft.Extensions.Logging;

namespace AdventOfCode._2021
{
    public class _01_SonarSweep : PuzzleBase
    {
        private IEnumerable<int>? _measurements;

        public _01_SonarSweep(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource)
            : base(logger, inputSource, 1)
        {
        }

        public override void Setup(string[] input)
        {
            _measurements = input.Select(m => int.Parse(m));
        }

        public override long SolvePartOne(string[] input)
        {
            long largerThanPreviousCount = 0;
            var previousMeasurement = (int?)null;
            foreach (var measurement in _measurements!)
            {
                if (previousMeasurement.HasValue && measurement > previousMeasurement)
                    largerThanPreviousCount++;
                previousMeasurement = measurement;
            }

            return largerThanPreviousCount;
        }

        public override long SolvePartTwo(string[] input)
        {
            long largerThanPreviousCount = 0;
            var previousRollingSum = (int?)null;
            var calc = new RollingSum(3);

            foreach (var measurement in _measurements!)
            {
                var rollingSum = calc.AddNewSample(measurement);
                
                if (!rollingSum.HasValue)
                    continue;

                if (previousRollingSum.HasValue && rollingSum > previousRollingSum)
                    largerThanPreviousCount++;

                previousRollingSum = rollingSum;
            }

            return largerThanPreviousCount;
        }

        private class RollingSum
        {
            private readonly Queue<int> _samples = new Queue<int>();
            private readonly int _rollingWindowSize;
            public RollingSum(int windowSize)
            {
                _rollingWindowSize = windowSize;
            }

            public int? AddNewSample(int sample)
            {
                _samples.Enqueue(sample);
                if (_samples.Count < _rollingWindowSize)
                    return null;
                if (_samples.Count > _rollingWindowSize)
                    _samples.Dequeue();
                return _samples.Sum();
            }
        }
    }
}
