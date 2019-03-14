namespace AbstractTileGame
{
    internal class TileChunk<T> : IRecursiveTileMap<T>
    {
        private readonly T[,] _tiles;

        public TileChunk(int size)
        {
            _tiles = new T[size, size];
        }

        public T this[int x, int y]
        {
            get => _tiles[x, y];
            set => _tiles[x, y] = value;
        }
    }
}