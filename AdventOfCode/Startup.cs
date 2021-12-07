using AdventOfCode.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AdventOfCode
{
    public class Startup : BackgroundService
    {
        private readonly ILogger<Startup> _logger;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IPuzzle _puzzle;

        public Startup(
            ILogger<Startup> logger,
            IHostApplicationLifetime lifetime,
            IPuzzle puzzle)
        {
            _logger = logger;
            _lifetime = lifetime;
            _puzzle = puzzle;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _puzzle.InitializeAsync(cancellationToken);
                _logger.LogInformation("Part 1: {result}", await _puzzle.GetPartOneResult(cancellationToken));
                _logger.LogInformation("Part 2: {result}", await _puzzle.GetPartTwoResult(cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
            }

            _lifetime.StopApplication();
        }
    }
}
