using Microsoft.Extensions.Logging;

namespace AdventOfCode.Core
{
    public abstract class PuzzleBase : IPuzzle
    {
        protected readonly ILogger<PuzzleBase> _logger;
        private readonly IPuzzleInputSource _inputSource;
        private readonly int _dayNumber;

        public PuzzleBase(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource,
            int dayNumber)
        {
            _logger = logger;
            _inputSource = inputSource;
            _dayNumber = dayNumber;
        }

        private string[]? _input;

        private async Task<string[]> GetInputAsync(CancellationToken cancellationToken = default)
        {
            return await _inputSource.GetInputLinesAsync(_dayNumber, cancellationToken);
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Initializing solution for day {n}", _dayNumber);
            _input = await GetInputAsync(cancellationToken);
            Setup(_input);
        }

        public async Task<string> GetPartOneResult(CancellationToken cancellationToken = default)
        {
            try
            {
                return SolvePartOne(_input!).ToString();
            }
            catch (SolutionFailedException ex)
            {
                _logger.LogError(ex, "Solution failed");
                return "Failed";
            }
            catch (SolutionNotRunException)
            {
                return "Not run";
            }
        }

        public async Task<string> GetPartTwoResult(CancellationToken cancellationToken = default)
        {
            try
            {
                return SolvePartTwo(_input!).ToString();
            }
            catch (SolutionFailedException ex)
            {
                _logger.LogError(ex, "Solution failed");
                return "Failed";
            }
            catch (SolutionNotRunException)
            {
                return "Not run";
            }
        }

        public abstract void Setup(string[] input);
        public abstract long SolvePartOne(string[] input);
        public abstract long SolvePartTwo(string[] input);
    }
}
