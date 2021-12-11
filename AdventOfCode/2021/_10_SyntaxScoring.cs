using AdventOfCode.Core;
using Microsoft.Extensions.Logging;

namespace AdventOfCode._2021
{
    public class _10_SyntaxScoring : PuzzleBase
    {
        private NavSystemLine[]? _lines;

        public _10_SyntaxScoring(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource)
            : base(logger, inputSource, 10)
        {
        }

        public override void Setup(string[] input)
        {
            _lines = input
                .Select(line => new NavSystemLine(
                    line.Select(c => new NavSystemChunkDelimiter(c))))
                .ToArray();
        }

        public override long SolvePartOne(string[] input)
        {
            return _lines!
                .Where(line => line.IsCorrupted)
                .Sum(line => line.ErrorScore);
        }

        public override long SolvePartTwo(string[] input)
        {
            var completionScores = new List<long>();
            foreach (var line in _lines!.Where(l => l.IsIncomplete))
            {
                long score = 0;
                while (line.OpenChunkStack.Count > 0)
                {
                    var chunkType = line.OpenChunkStack.Pop();
                    score = score * 5 + (int)chunkType;
                }

                completionScores.Add(score);
            }

            // this will be a whole number by definition of the puzzle
            var midX = (completionScores.Count - 1) / 2;
            return completionScores
                .OrderBy(s => s)
                .ToList()
                .ElementAt(midX);
        }

        private class NavSystemLine : List<NavSystemChunkDelimiter>
        {
            public NavSystemLine(IEnumerable<NavSystemChunkDelimiter> collection)
                : base(collection)
            {
                OpenChunkStack = new Stack<NavSystemChunkType>();
                foreach (var delimiter in collection)
                {
                    if (delimiter.IsOpener)
                        OpenChunkStack.Push(delimiter.Type);
                    else if (OpenChunkStack.Pop() != delimiter.Type)
                    {
                        ErrorScore += GetErrorScore(delimiter.Type);
                        break;
                    }
                }
            }

            private static int GetErrorScore(NavSystemChunkType type)
                => type switch
                {
                    NavSystemChunkType.Parentheses => 3,
                    NavSystemChunkType.SquareBrackets => 57,
                    NavSystemChunkType.CurlyBrackets => 1197,
                    NavSystemChunkType.AngleBrackets => 25137,
                    _ => 0
                };

            public int ErrorScore { get; } = 0;
            public bool IsCorrupted => ErrorScore > 0;

            public Stack<NavSystemChunkType> OpenChunkStack { get; }
            public bool IsIncomplete => !IsCorrupted && OpenChunkStack.Count > 0;
        }

        private struct NavSystemChunkDelimiter
        {
            public NavSystemChunkDelimiter(char input)
            {
                switch (input)
                {
                    case '(':
                        Type = NavSystemChunkType.Parentheses;
                        IsOpener = true;
                        break;
                    case ')':
                        Type = NavSystemChunkType.Parentheses;
                        IsOpener = false;
                        break;
                    case '[':
                        Type = NavSystemChunkType.SquareBrackets;
                        IsOpener = true;
                        break;
                    case ']':
                        Type = NavSystemChunkType.SquareBrackets;
                        IsOpener = false;
                        break;
                    case '{':
                        Type = NavSystemChunkType.CurlyBrackets;
                        IsOpener = true;
                        break;
                    case '}':
                        Type = NavSystemChunkType.CurlyBrackets;
                        IsOpener = false;
                        break;
                    case '<':
                        Type = NavSystemChunkType.AngleBrackets;
                        IsOpener = true;
                        break;
                    case '>':
                        Type = NavSystemChunkType.AngleBrackets;
                        IsOpener = false;
                        break;
                    default: throw new SolutionFailedException("Unexpected chunk delimiter");
                }
            }

            public NavSystemChunkType Type { get; }
            public bool IsOpener { get; }
        }

        private enum NavSystemChunkType
        {
            Parentheses = 1,
            SquareBrackets = 2,
            CurlyBrackets = 3,
            AngleBrackets = 4
        }
    }
}
