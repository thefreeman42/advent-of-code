namespace AdventOfCode.Core
{
    public interface IPuzzle
    {
        public Task InitializeAsync(CancellationToken cancellationToken = default);
        public Task<string> GetPartOneResult(CancellationToken cancellationToken = default);
        public Task<string> GetPartTwoResult(CancellationToken cancellationToken = default);
    }
}
