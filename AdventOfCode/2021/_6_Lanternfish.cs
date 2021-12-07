using AdventOfCode.Core;
using Microsoft.Extensions.Logging;

namespace AdventOfCode._2021
{
    public class _6_Lanternfish : PuzzleBase
    {
        private int[]? _initialLanternfish;

        public _6_Lanternfish(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource)
            : base(logger, inputSource, 6)
        {
        }

        public override void Setup(string[] input)
        {
            _initialLanternfish = input
                .Single()
                .Trim()
                .Split(',')
                .Select(c => int.Parse(c))
                .ToArray();
        }

        public override long SolvePartOne(string[] input)
        {
            var cycleContext = new LanternfishCycleContext(_initialLanternfish!, 7);

            cycleContext.RunCycleForDays(80);

            return cycleContext.FishCount;
        }

        public override long SolvePartTwo(string[] input)
        {
            var cycleContext = new LanternfishCycleContext(_initialLanternfish!, 7);

            cycleContext.RunCycleForDays(256);

            return cycleContext.FishCount;
        }

        private class LanternfishCycleContext
        {
            private readonly int _cycleLength;
            private readonly int _additionalDaysForChildren;
            private readonly Dictionary<int, long> _newFishPerCycleDay;
            private readonly Dictionary<int, long> _childrenAwaitingNormalCycle;

            public LanternfishCycleContext(int[] initialState, int normalCycle, int additionalDaysUntilNormalCycleForChildren = 2)
            {
                _cycleLength = normalCycle;
                _additionalDaysForChildren = additionalDaysUntilNormalCycleForChildren;
                _newFishPerCycleDay = Enumerable.Range(0, normalCycle)
                    .ToDictionary(n => n, _ => (long)0);
                _childrenAwaitingNormalCycle = Enumerable.Range(0, normalCycle)
                    .ToDictionary(n => n, _ => (long)0);

                foreach (var initialFishCycle in initialState)
                {
                    _adultFishCount++;
                    _newFishPerCycleDay[initialFishCycle + 1]++;
                }
            }

            private long _adultFishCount = 0;
            public long FishCount => 
                _adultFishCount + _childrenAwaitingNormalCycle.Sum(kvp => kvp.Value);

            public void RunCycleForDays(int dayCount)
            {
                foreach (var cycle in Enumerable.Range(1, dayCount))
                {
                    var cycleDay = cycle % _cycleLength;
                    _childrenAwaitingNormalCycle[(cycle + _additionalDaysForChildren) % _cycleLength] += _newFishPerCycleDay[cycleDay];

                    _adultFishCount += _childrenAwaitingNormalCycle[cycleDay];
                    _newFishPerCycleDay[cycleDay] += _childrenAwaitingNormalCycle[cycleDay];
                    _childrenAwaitingNormalCycle[cycleDay] = 0;
                }
            }
        }
    }
}
