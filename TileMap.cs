using System;
using System.Collections.Generic;

namespace AbstractTileGame
{
    /// <summary>
    ///     Represents a map, which contains of titles. The tile map does not handle the actual layout of the titles and can be
    ///     used for any two dimensional (in terms of each tile has distinct two dimensional integer coordiates) tile array.
    /// </summary>
    /// <typeparam name="T">The type of the tiles in this map</typeparam>
    /// <remarks>
    ///     The tile map will grow when needed. If it grows it will create a new chunk of n x n titles, where n is the
    ///     chucksize given in the constructor. The default size is 127.
    /// </remarks>
    public class TileMap<T>
    {
        private readonly HashSet<TilePosition> _placedTiles = new HashSet<TilePosition>();
        private IRecursiveTileMap<T> _map;
        private ChunckSize _size;

        /// <summary>
        ///     Creates a new Tilemap with a chunksize of 127.
        /// </summary>
        public TileMap() : this(chunksize: 127)
        {
        }


        /// <summary>
        ///     Creates a nee tile map
        /// </summary>
        /// <param name="chunksize">The lenght of each side of a chunk. Must be greater than two.</param>
        public TileMap(int chunksize)
        {
            if (chunksize <= 2)
                throw new ArgumentException("Chunksize must be greater than 2.", nameof(chunksize));

            _map = new TileChunk<T>(chunksize);
            _size = new ChunckSize(chunksize);
        }

        /// <summary>
        ///     Gets the tile at a specific postion.
        /// </summary>
        /// <param name="position">The position of a tile</param>
        /// <returns>The tile at the specified position.</returns>
        public T this[ITilePosition position]
        {
            get
            {
                if (position == null) throw new ArgumentNullException(nameof(position));
                return this[position.X, position.Y];
            }
            set
            {
                if (position == null) throw new ArgumentNullException(nameof(position));
                this[position.X, position.Y] = value;
            }
        }

        public T this[int x, int y]
        {
            get => _size.Contains(x, y) ? _map[x, y] : default;
            set
            {
                while (!_size.Contains(x, y))
                {
                    _map = new RecursiveTileMap<T>(_map, _size.ChunkSize);
                    _size++;
                }

                _map[x, y] = value;
                OnTilePlaced(x, y, value);
            }
        }

        public IEnumerable<TilePosition> PlacedTilesPositions => _placedTiles;

        protected virtual void OnTilePlaced(int x, int y, T tile)
        {
            if (Equals(tile, default(T)))
                _placedTiles.Remove((x,y));
            else
                _placedTiles.Add((x,y));
        }

        private struct ChunckSize
        {
            public int ChunkSize { get; }
            private readonly int _min;
            private readonly int _max;
            private readonly int _upperFactor;
            private readonly int _lowerFactor;

            public ChunckSize(int chunkSize) : this(chunkSize, min: 0, max: chunkSize, upperFactor: (chunkSize - 1) / 2,
                lowerFactor: chunkSize / 2)
            {
            }

            private ChunckSize(int chunkSize, int min, int max, int upperFactor, int lowerFactor)
            {
                ChunkSize = chunkSize;
                _min = min;
                _max = max;
                _upperFactor = upperFactor;
                _lowerFactor = lowerFactor;
            }

            public bool Contains(int x, int y)
            {
                return _min <= x && x < _max && _min <= y && y < _max;
            }

            public static ChunckSize operator ++(ChunckSize other)
            {
                return new ChunckSize(other.ChunkSize, other._min - other.ChunkSize * other._lowerFactor,
                    other._max + other.ChunkSize * other._upperFactor, other._upperFactor, other._lowerFactor);
            }
        }
    }
}