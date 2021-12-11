using AdventOfCode.Core;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AdventOfCode._2021
{
    public class _8_SevenSegmentSearch : PuzzleBase
    {
        private DisplayState[]? _states;

        public _8_SevenSegmentSearch(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource)
            : base(logger, inputSource, 8)
        {
        }

        public override void Setup(string[] input)
        {
            _states = input
                .Select(line => new DisplayState(line))
                .ToArray();
        }

        public override long SolvePartOne(string[] input)
        {
            var uniqueDigitLengths = new List<int> { 2, 3, 4, 7 };
            return _states!
                .SelectMany(state => state.Output)
                .Where(outputDigit => uniqueDigitLengths.Contains(outputDigit.Length))
                .Count();
        }

        public override long SolvePartTwo(string[] input)
        {
            var sum = 0;
            foreach (var display in _states!)
            {
                sum += display.CalculateOutputValue();
            }
            return sum;
        }

        private class DisplayState
        {
            public DisplayState(string input)
            {
                var split = input.Split('|');
                Input = split[0]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
                Output = split[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
            }

            public int CalculateOutputValue()
            {
                var lookupDict = new Dictionary<char, DisplayDigitSegment>();

                var one = Input.First(i => i.Length == 2).ToCharArray().OrderBy(c => c);
                var seven = Input.First(i => i.Length == 3).ToCharArray().OrderBy(c => c);
                var topSegmentChar = seven.Except(one).Single();
                lookupDict.Add(topSegmentChar, DisplayDigitSegment.Top);

                var charCounts = Input
                    .SelectMany(c => c)
                    .GroupBy(c => c)
                    .ToDictionary(c => c.Key, c => c.Count());

                var topLeftSegmentChar = charCounts.First(kvp => kvp.Value == 6).Key;
                var topRightSegmentChar = charCounts.First(kvp => kvp.Key != topSegmentChar && kvp.Value == 8).Key;
                var bottomLeftSegmentChar = charCounts.First(kvp => kvp.Value == 4).Key;
                var bottomRightSegmentChar = charCounts.First(kvp => kvp.Value == 9).Key;
                lookupDict.Add(topLeftSegmentChar, DisplayDigitSegment.TopLeft);
                lookupDict.Add(topRightSegmentChar, DisplayDigitSegment.TopRight);
                lookupDict.Add(bottomLeftSegmentChar, DisplayDigitSegment.BottomLeft);
                lookupDict.Add(bottomRightSegmentChar, DisplayDigitSegment.BottomRight);

                var middleSegmentChar = Input.First(i => i.Length == 4).First(c => !lookupDict.Keys.Contains(c));
                lookupDict.Add(middleSegmentChar, DisplayDigitSegment.Middle);
                var bottomSegmentChar = Input.First(i => i.Length == 7).First(c => !lookupDict.Keys.Contains(c));
                lookupDict.Add(bottomSegmentChar, DisplayDigitSegment.Bottom);

                var outputString = new StringBuilder();
                foreach (var digitString in Output)
                {
                    DisplayDigitSegment segments = default;
                    foreach (var segmentChar in digitString)
                        segments |= lookupDict[segmentChar];
                    var value = GetDisplayDigitValue((int)segments);
                    outputString.Append(value.ToString());
                }

                return int.Parse(outputString.ToString());
            }

            private int GetDisplayDigitValue(int value)
                => value switch
                {
                    (int)DisplayDigitValue.Zero => 0,
                    (int)DisplayDigitValue.One => 1,
                    (int)DisplayDigitValue.Two => 2,
                    (int)DisplayDigitValue.Three => 3,
                    (int)DisplayDigitValue.Four => 4,
                    (int)DisplayDigitValue.Five => 5,
                    (int)DisplayDigitValue.Six => 6,
                    (int)DisplayDigitValue.Seven => 7,
                    (int)DisplayDigitValue.Eight => 8,
                    (int)DisplayDigitValue.Nine => 9,
                    _ => throw new SolutionFailedException("Unexpected display digit value")
                };

            public string[] Input { get; set; }
            public string[] Output { get; set; }
        }

        [Flags]
        private enum DisplayDigitSegment
        {
            Top = 1,
            TopLeft = 2,
            TopRight = 4,
            Middle = 8,
            BottomLeft = 16,
            BottomRight = 32,
            Bottom = 64,
        }

        private enum DisplayDigitValue
        {
            Zero = DisplayDigitSegment.Top |
                   DisplayDigitSegment.TopLeft |
                   DisplayDigitSegment.TopRight |
                   DisplayDigitSegment.BottomLeft |
                   DisplayDigitSegment.BottomRight |
                   DisplayDigitSegment.Bottom,
            One = DisplayDigitSegment.TopRight | DisplayDigitSegment.BottomRight,
            Two = DisplayDigitSegment.Top |
                  DisplayDigitSegment.TopRight |
                  DisplayDigitSegment.Middle |
                  DisplayDigitSegment.BottomLeft |
                  DisplayDigitSegment.Bottom,
            Three = DisplayDigitSegment.Top |
                DisplayDigitSegment.TopRight |
                DisplayDigitSegment.Middle |
                DisplayDigitSegment.BottomRight |
                DisplayDigitSegment.Bottom,
            Four = DisplayDigitSegment.TopLeft |
                DisplayDigitSegment.TopRight |
                DisplayDigitSegment.Middle |
                DisplayDigitSegment.BottomRight,
            Five = DisplayDigitSegment.Top |
                  DisplayDigitSegment.TopLeft |
                  DisplayDigitSegment.Middle |
                  DisplayDigitSegment.BottomRight |
                  DisplayDigitSegment.Bottom,
            Six = DisplayDigitSegment.Top |
                  DisplayDigitSegment.TopLeft |
                  DisplayDigitSegment.Middle |
                  DisplayDigitSegment.BottomLeft |
                  DisplayDigitSegment.BottomRight |
                  DisplayDigitSegment.Bottom,
            Seven = DisplayDigitSegment.Top | DisplayDigitSegment.TopRight | DisplayDigitSegment.BottomRight,
            Eight = DisplayDigitSegment.Top |
                  DisplayDigitSegment.TopLeft |
                  DisplayDigitSegment.TopRight |
                  DisplayDigitSegment.Middle |
                  DisplayDigitSegment.BottomLeft |
                  DisplayDigitSegment.BottomRight |
                  DisplayDigitSegment.Bottom,
            Nine = DisplayDigitSegment.Top |
                  DisplayDigitSegment.TopLeft |
                  DisplayDigitSegment.TopRight |
                  DisplayDigitSegment.Middle |
                  DisplayDigitSegment.BottomRight |
                  DisplayDigitSegment.Bottom,
        }
    }
}
