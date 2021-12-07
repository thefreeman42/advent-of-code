using AdventOfCode.Core;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode._2021
{
    public class _5_HyrothermalVenture : PuzzleBase
    {
        private List<VentLine>? _lines;

        public _5_HyrothermalVenture(
            ILogger<PuzzleBase> logger,
            IPuzzleInputSource inputSource
            ) : base(logger, inputSource, 5)
        {
        }

        public override void Setup(string[] input)
        {
            _lines = input
                .Select(line => new VentLine(line))
                .ToList();
        }

        public override long SolvePartOne(string[] input)
        {
            var coordDict = new Dictionary<VentCoordinate, int>();

            foreach (var ventLine in _lines!.Where(l => l.IsCardinal))
            {
                var coords = ventLine.GetAllCoveredCoordinates();
                foreach (var coord in coords)
                {
                    if (!coordDict.TryAdd(coord, 1))
                        coordDict[coord] += 1;
                }
            }

            return coordDict.Where(kvp => kvp.Value >= 2).Count();
        }

        public override long SolvePartTwo(string[] input)
        {
            var coordDict = new Dictionary<VentCoordinate, int>();

            foreach (var ventLine in _lines!)
            {
                var coords = ventLine.GetAllCoveredCoordinates();
                foreach (var coord in coords)
                {
                    if (!coordDict.TryAdd(coord, 1))
                        coordDict[coord] += 1;
                }
            }

            return coordDict.Where(kvp => kvp.Value >= 2).Count();
        }

        private struct VentLine
        {
            public VentLine(string input)
            {
                var coords = input
                    .Replace(" -> ", ",")
                    .Split(',')
                    .Select(c => int.Parse(c))
                    .ToArray();

                Start = new VentCoordinate(coords[0], coords[1]);
                End = new VentCoordinate(coords[2], coords[3]);
            }

            public VentCoordinate Start { get; }
            public VentCoordinate End { get; }

            public VentCoordinate[] GetAllCoveredCoordinates()
            {
                var xDirection = Start.GetXDirectionTo(End);
                var yDirection = Start.GetYDirectionTo(End);

                var coords = new List<VentCoordinate> { Start };
                var current = Start;
                do
                {
                    current = new VentCoordinate(current.X + xDirection, current.Y + yDirection);
                    coords.Add(current);
                } while (current != End);

                return coords.ToArray();
            }

            public bool IsCardinal
                => Start.X == End.X || Start.Y == End.Y;
        }

        private struct VentCoordinate
        {
            public VentCoordinate(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }

            public int GetXDirectionTo([NotNull] VentCoordinate other)
            {
                if (other.X > X) return 1;
                if (other.X < X) return -1;
                else return 0;
            }

            public int GetYDirectionTo([NotNull] VentCoordinate other)
            {
                if (other.Y > Y) return 1;
                if (other.Y < Y) return -1;
                else return 0;
            }

            public static bool operator ==(VentCoordinate lhs, VentCoordinate rhs)
                => lhs.X == rhs.X && lhs.Y == rhs.Y;

            public static bool operator !=(VentCoordinate lhs, VentCoordinate rhs)
                => lhs.X != rhs.X || lhs.Y != rhs.Y;

            public override bool Equals(object? obj)
            {
                if (obj == null) return false;
                if (obj is VentCoordinate vc) return X == vc.X && Y == vc.Y;
                return false;
            }

            public override int GetHashCode()
                => HashCode.Combine(X, Y);
        }
    }
}
