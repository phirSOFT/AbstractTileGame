using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AbstractTileGame
{
    public static class TilePositionExtensions
    {
        // Todo: though this makes the code less error prone, i doubt the compiler can optimize the overhead out. 
        // There must be a better way to implement this feature without duplicate code and less overhead.
        // C# 8.0 may will have some usefull features for that purpose.

        private static readonly IReadOnlyList<Func<ITilePosition, TilePosition>> _hexangularAdjacents =
            new ReadOnlyCollection<Func<ITilePosition, TilePosition>>(new Func<ITilePosition, TilePosition>[]
            {
            North,
            East,
            SouthEast,
            South,
            West,
            NorthWest
        });

        private static readonly IReadOnlyList<Func<ITilePosition, TilePosition>> _rectangularAdjacents =
            new ReadOnlyCollection<Func<ITilePosition, TilePosition>>(new Func<ITilePosition, TilePosition>[]
            {
                North,
                East,
                South,
                West
            });

        private static readonly IReadOnlyList<Func<ITilePosition, TilePosition>> _oddTriangularAdjacents =
            new ReadOnlyCollection<Func<ITilePosition, TilePosition>>(new Func<ITilePosition, TilePosition>[]
            { 
                South,
                West,
                East
            });

        private static readonly IReadOnlyList<Func<ITilePosition, TilePosition>> _evenTriangularAdjacents =
            new ReadOnlyCollection<Func<ITilePosition, TilePosition>>(new Func<ITilePosition, TilePosition>[]
            { 
                North,
                East,
                West
            });

        /// <summary>
        ///     Enumerates all positionf of the tiles adjacent to the current tile.
        /// </summary>
        /// <param name="tile">The postion of the tile.</param>
        /// <param name="pattern">The pattern that is used.</param>
        /// <returns></returns>
        /// <remarks>
        ///     The first tile is usually the north tile. The only tiles in a triangular pattern, with one odd and one even
        ///     coordinate. In that case first title is the south located tile. The following tiltes are enumerated counter
        ///     clockwise.
        /// </remarks>
        public static IEnumerable<TilePosition> GetAdjacentTiles(this ITilePosition tile, TilePattern pattern)
        {
            int x = tile.X;
            int y = tile.Y;

            switch (pattern)
            {
                case TilePattern.Triangular:
                    // check for odd tiles
                    return ((x & 1) ^ (y & 1)) == 1 ? _oddTriangularAdjacents.Select(t => t(tile)) : _evenTriangularAdjacents.Select(t => t(tile));

                case TilePattern.Rectangular:
                    return _rectangularAdjacents.Select(t => t(tile));
                case TilePattern.Hexangular:
                    return _hexangularAdjacents.Select(t => t(tile));
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern,
                        $"Pattern {pattern} not supported.");
            }
        }

        /// <summary>
        ///     Enumerates all positionf of the tiles adjacent to the current tile.
        /// </summary>
        /// <param name="tile">The postion of the tile.</param>
        /// <param name="index"></param>
        /// <param name="pattern">The pattern that is used.</param>
        /// <returns></returns>
        /// <remarks>
        ///     The first tile is usually the north tile. The only tiles in a triangular pattern, with one odd and one even
        ///     coordinate. In that case first title is the south located tile. The following tiltes are enumerated counter
        ///     clockwise.
        /// </remarks>
        public static TilePosition GetAdjacentTile(this ITilePosition tile, int index, TilePattern pattern)
        {
            if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
            int x = tile.X;
            int y = tile.Y;

            switch (pattern)
            {
                case TilePattern.Triangular:
                    // check for odd tiles
                    return ((x & 1) ^ (y & 1)) == 1 ? _oddTriangularAdjacents[index % (int) pattern](tile) : _evenTriangularAdjacents[index % (int) pattern](tile);

                case TilePattern.Rectangular:
                    return _rectangularAdjacents[index % (int) pattern](tile);
                case TilePattern.Hexangular:
                    return _hexangularAdjacents[index % (int) pattern](tile);
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern,
                        $"Pattern {pattern} not supported.");
            }
        }

        /// <summary>
        ///     Enumerates all positionf of the tiles adjacent to the current tile.
        /// </summary>
        /// <param name="tile">The postion of the tile.</param>
        /// <param name="pattern">The pattern that is used.</param>
        /// <returns></returns>
        /// <remarks>
        ///     The first tile is usually the north tile. The only tiles in a triangular pattern, with one odd and one even
        ///     coordinate. In that case first title is the south located tile. The following tiltes are enumerated counter
        ///     clockwise.
        /// </remarks>
        public static IEnumerable<(int X, int Y)> GetAdjacentTiles(this in (int x, int y) tile, TilePattern pattern)
        {

            switch (pattern)
            {
                case TilePattern.Triangular:
                    // check for odd tiles
                    if (((tile.x & 1) ^ (tile.y & 1)) == 1)
                    {
                        return new[]
                        {
                            tile.South(),
                            tile.East(),
                            tile.West()
                        };
                    }
                    else
                    {
                        return new[]
                        {
                            tile.North(),
                            tile.West(),
                            tile.East()
                        };
                    }

                case TilePattern.Rectangular:
                    return new[]
                    {
                        tile.North(),
                        tile.East(),
                        tile.South(),
                        tile.West()
                    };
                case TilePattern.Hexangular:
                    return new[]
                    {
                        tile.North(),
                        tile.East(),
                        tile.SouthEast(),
                        tile.South(),
                        tile.West(),
                        tile.NorthWest()
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern,
                        $"Pattern {pattern} not supported.");
            }
        }

        public static IEnumerable<TilePosition> GetNearTitles(this ITilePosition tile, TilePattern pattern)
        {
            switch (pattern)
            {
                case TilePattern.Triangular:

                    break;
                case TilePattern.Rectangular:
                    yield return tile.North();
                    yield return tile.NorthEast();
                    yield return tile.East();
                    yield return tile.SouthEast();
                    yield return tile.South();
                    yield return tile.SouthWest();
                    yield return tile.West();
                    yield return tile.NorthWest();
                    break;
                case TilePattern.Hexangular:
                    yield return tile.North();
                    yield return tile.East();
                    yield return tile.SouthEast();
                    yield return tile.South();
                    yield return tile.West();
                    yield return tile.NorthWest();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern,
                        $"Pattern {pattern} not supported.");
            }
        }

        public static IEnumerable<(int X, int Y)> GetNearTitles(this in (int x, int y) tile, TilePattern pattern)
        {
            switch (pattern)
            {
                case TilePattern.Triangular:
                    return Enumerable.Empty<(int, int)>();
                case TilePattern.Rectangular:
                    return new[]
                    {
                        tile.North(),
                        tile.NorthEast(),
                        tile.East(),
                        tile.SouthEast(),
                        tile.South(),
                        tile.SouthWest(),
                        tile.West(),
                        tile.NorthWest()
                    };
                case TilePattern.Hexangular:
                    return new[]
                    {
                        tile.North(),
                        tile.East(),
                        tile.SouthEast(),
                        tile.South(),
                        tile.West(),
                        tile.NorthWest()
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern,
                        $"Pattern {pattern} not supported.");
            }
        }

        public static TilePosition North(this ITilePosition tile)
        {
            return new TilePosition(tile.X, tile.Y - 1);
        }

        public static TilePosition NorthWest(this ITilePosition tile)
        {
            return new TilePosition(tile.X - 1, tile.Y - 1);
        }

        public static TilePosition West(this ITilePosition tile)
        {
            return new TilePosition(tile.X - 1, tile.Y);
        }

        public static TilePosition SouthWest(this ITilePosition tile)
        {
            return new TilePosition(tile.X - 1, tile.Y + 1);
        }

        public static TilePosition South(this ITilePosition tile)
        {
            return new TilePosition(tile.X, tile.Y + 1);
        }

        public static TilePosition SouthEast(this ITilePosition tile)
        {
            return new TilePosition(tile.X + 1, tile.Y + 1);
        }

        public static TilePosition East(this ITilePosition tile)
        {
            return new TilePosition(tile.X + 1, tile.Y);
        }

        public static TilePosition NorthEast(this ITilePosition tile)
        {
            return new TilePosition(tile.X + 1, tile.Y - 1);
        }

        public static (int X, int Y) North(this in (int x, int y) tilePosition)
        {
            return (tilePosition.x, tilePosition.y - 1);
        }

        public static (int X, int Y) NorthWest(this in (int x, int y) tilePosition)
        {
            return (tilePosition.x - 1, tilePosition.y - 1);
        }

        public static (int X, int Y) West(this in (int x, int y) tilePosition)
        {
            return (tilePosition.x - 1, tilePosition.y);
        }

        public static (int X, int Y) SouthWest(this in (int x, int y) tilePosition)
        {
            return (tilePosition.x - 1, tilePosition.y + 1);
        }

        public static (int X, int Y) South(this in (int x, int y) tilePosition)
        {
            return (tilePosition.x, tilePosition.y + 1);
        }

        public static (int X, int Y) SouthEast(this in (int x, int y) tilePosition)
        {
            return (tilePosition.x + 1, tilePosition.y + 1);
        }

        public static (int X, int Y) East(this in (int x, int y) tilePosition)
        {
            return (tilePosition.x + 1, tilePosition.y);
        }

        public static (int X, int Y) NorthEast(this in (int x, int y) tilePosition)
        {
            return (tilePosition.x + 1, tilePosition.y - 1);
        }
    }
}