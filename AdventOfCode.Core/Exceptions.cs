namespace AdventOfCode.Core
{
    public class SolutionFailedException : Exception
    {
        public SolutionFailedException(string? message) : base(message)
        {
        }
    }

    public class SolutionNotRunException : Exception
    {
        public SolutionNotRunException() : base()
        {
        }
    }
}
