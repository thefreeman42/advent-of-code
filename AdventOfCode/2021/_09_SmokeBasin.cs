using AdventOfCode.Core;
using Microsoft.Extensions.Logging;

namespace AdventOfCode._2021
{
    public class _09_SmokeBasin : PuzzleBase
    {
        private int[][]? _grid;
        private int _rowCount;
        private int _colCount;
        private int _lastRowIndex;
        private int _lastColIndex;

        public _09_SmokeBasin(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource)
            : base(logger, inputSource, 9)
        {
        }

        public override void Setup(string[] input)
        {
            _grid = input
                .Select(line => line.Select(c => c - '0').ToArray())
                .ToArray();
            _rowCount = _grid!.Length;
            _colCount = _grid[0].Length;
            _lastRowIndex = _rowCount - 1;
            _lastColIndex = _colCount - 1;
        }

        public override long SolvePartOne(string[] input)
        {
            var riskValues = new List<int>();

            foreach (var x in Enumerable.Range(0, _rowCount))
            {
                foreach (var y in Enumerable.Range(0, _colCount))
                {
                    if (GridPointIsMinimum(x, y))
                    {
                        riskValues.Add(_grid![x][y] + 1);
                        _minima.Add((x, y));
                        _unexploredGridPoints.Enqueue((x, y));
                    }
                }
            }

            return riskValues.Sum();
        }

        private readonly HashSet<(int, int)> _minima = new HashSet<(int, int)>();
        private readonly HashSet<(int, int)> _exploredGridPoints = new HashSet<(int, int)>();
        private readonly Queue<(int, int)> _unexploredGridPoints = new Queue<(int, int)>();

        public override long SolvePartTwo(string[] input)
        {
            var basinCoordinateLookup = new Dictionary<(int, int), (int, int)>();
            var basinSizeLookup = new Dictionary<(int, int), int>();

            while (_unexploredGridPoints.Count > 0)
            {
                var coord = _unexploredGridPoints.Dequeue();

                if (!_exploredGridPoints.Add(coord))
                    continue;

                if (_grid![coord.Item1][coord.Item2] == 9)
                    continue;

                (int, int) basinCoord; // coordinate of basin minimum;

                if (_minima.Contains(coord))
                {
                    basinCoord = coord;
                    basinSizeLookup.Add(coord, 1);
                }
                else
                {
                    basinCoord = basinCoordinateLookup[coord];
                    basinSizeLookup[basinCoord]++;
                }

                if (coord.Item1 != 0)
                {
                    var neighbour = (coord.Item1 - 1, coord.Item2);
                    _unexploredGridPoints.Enqueue(neighbour);
                    basinCoordinateLookup.TryAdd(neighbour, basinCoord);
                }
                if (coord.Item2 != 0)
                {
                    var neighbour = (coord.Item1, coord.Item2 - 1);
                    _unexploredGridPoints.Enqueue(neighbour);
                    basinCoordinateLookup.TryAdd(neighbour, basinCoord);
                }
                if (coord.Item1 != _lastRowIndex)
                {
                    var neighbour = (coord.Item1 + 1, coord.Item2);
                    _unexploredGridPoints.Enqueue(neighbour);
                    basinCoordinateLookup.TryAdd(neighbour, basinCoord);
                }
                if (coord.Item2 != _lastColIndex)
                {
                    var neighbour = (coord.Item1, coord.Item2 + 1);
                    _unexploredGridPoints.Enqueue(neighbour);
                    basinCoordinateLookup.TryAdd(neighbour, basinCoord);
                }
            }

            return basinSizeLookup.Values
                .OrderByDescending(size => size)
                .Take(3)
                .Aggregate(1, (x, y) => x * y);
        }

        private bool GridPointIsMinimum(int x, int y)
        {
            var value = _grid![x][y];
            if (value == 9) return false;

            if (x != 0 && _grid[x - 1][y] <= value) return false;
            if (y != 0 && _grid[x][y - 1] <= value) return false;
            if (x != _lastRowIndex && _grid[x + 1][y] <= value) return false;
            if (y != _lastColIndex && _grid[x][y + 1] <= value) return false;

            return true;
        }
    }
}
