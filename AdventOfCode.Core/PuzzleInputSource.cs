namespace AdventOfCode.Core
{
    public interface IPuzzleInputSource
    {
        public Task<string[]> GetInputLinesAsync(int dayNumber, CancellationToken cancellationToken = default);
    }

    public class PuzzleInputSource : IPuzzleInputSource
    {
        private readonly string _inputPath = "D:\\Dev\\adventofcode\\Input";
        public async Task<string[]> GetInputLinesAsync(int dayNumber, CancellationToken cancellationToken = default)
        {
            var file = Path.Combine(_inputPath, DateTime.Today.Year.ToString(), $"{dayNumber}.txt");
            if (!File.Exists(file))
                throw new SolutionFailedException("Input file does not exist");
            return await File.ReadAllLinesAsync(file, cancellationToken);
        }
    }
}
