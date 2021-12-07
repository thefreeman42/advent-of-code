using AdventOfCode.Core;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AdventOfCode._2021
{
    public class _3_BinaryDiagnostic : PuzzleBase
    {
        private const char ZERO = '0';
        private const char ONE = '1';

        private List<BinaryNumber>? _diagnosticNumbers;

        public _3_BinaryDiagnostic(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource)
            : base(logger, inputSource, 3)
        {
        }

        public override void Setup(string[] input)
        {
            _diagnosticNumbers = input
                .Select(i => new BinaryNumber(i))
                .ToList();
        }

        public override long SolvePartOne(string[] input)
        {
            var gammaBuilder = new StringBuilder();
            var epsilonBuilder = new StringBuilder();

            var digitCount = input.Select(i => i.Length).Distinct().Single();

            foreach (var pos in Enumerable.Range(0, digitCount))
            {
                var digits = _diagnosticNumbers!.Select(n => n[pos]);
                var gammaDigit = GetMostCommonDigit(digits, BinaryDigit.One);
                var epsilonDigit = GetLeastCommonDigit(digits, BinaryDigit.Zero);

                gammaBuilder.Append(gammaDigit.Value);
                epsilonBuilder.Append(epsilonDigit.Value);
            }

            var gammaValue = new BinaryNumber(gammaBuilder.ToString());
            var epsilonValue = new BinaryNumber(epsilonBuilder.ToString());

            return gammaValue.DecimalValue * epsilonValue.DecimalValue;
        }

        public override long SolvePartTwo(string[] input)
        {
            var oxygenGenerationRating = FilterDiagnosticNumbers(
                _diagnosticNumbers!,
                (digits) => GetMostCommonDigit(digits, BinaryDigit.One));

            var co2ScrubberRating = FilterDiagnosticNumbers(
                _diagnosticNumbers!,
                (digits) => GetLeastCommonDigit(digits, BinaryDigit.Zero));

            return oxygenGenerationRating.DecimalValue * co2ScrubberRating.DecimalValue;
        }

        private BinaryNumber FilterDiagnosticNumbers(
            IEnumerable<BinaryNumber> numbers,
            Func<IEnumerable<BinaryDigit>, BinaryDigit> filterDelegate,
            int position = 0)
        {
            if (numbers.Count() == 1)
                return numbers.First();

            var digits = numbers.Select(n => n[position]);
            var filterDigit = filterDelegate.Invoke(digits);

            return FilterDiagnosticNumbers(
                numbers.Where(n => n[position] == filterDigit),
                filterDelegate,
                position + 1);
        }

        private BinaryDigit GetMostCommonDigit(IEnumerable<BinaryDigit> digits, BinaryDigit tiebreaker)
        {
            var zeros = digits.Count(d => d == BinaryDigit.Zero);
            var ones = digits.Count(d => d == BinaryDigit.One);

            if (zeros == ones) return tiebreaker;
            else if (zeros > ones) return BinaryDigit.Zero;
            else return BinaryDigit.One;
        }

        private BinaryDigit GetLeastCommonDigit(IEnumerable<BinaryDigit> digits, BinaryDigit tiebreaker)
        {
            var zeros = digits.Count(d => d == BinaryDigit.Zero);
            var ones = digits.Count(d => d == BinaryDigit.One);

            if (zeros == ones) return tiebreaker;
            else if (zeros < ones) return BinaryDigit.Zero;
            else return BinaryDigit.One;
        }

        private struct BinaryNumber
        {
            public BinaryNumber(string value)
            {
                Value = value;
                _digits = value.Select(x => x switch
                {
                    ZERO => BinaryDigit.Zero,
                    ONE => BinaryDigit.One,
                    _ => throw new SolutionFailedException($"Unexpected character in binary value: {x}")
                }).ToArray();
            }

            public string Value { get; }

            public int DecimalValue
                => Convert.ToInt32(Value, 2);

            private readonly BinaryDigit[] _digits;

            public BinaryDigit this[int key]
                => _digits[key];
        }

        private struct BinaryDigit
        {
            private BinaryDigit(char value)
            {
                Value = value;
            }

            public char Value { get; }

            public static BinaryDigit Zero
                => new(ZERO);
            public static BinaryDigit One
                => new(ONE);

            public static bool operator ==(BinaryDigit lhs, BinaryDigit rhs)
                => lhs.Value == rhs.Value;
            public static bool operator !=(BinaryDigit lhs, BinaryDigit rhs)
                => lhs.Value != rhs.Value;

            public override bool Equals(object? obj)
            {
                if (obj is BinaryDigit d) return this == d;
                else return false;
            }

            public override int GetHashCode()
                => Value.GetHashCode();
        }
    }
}
