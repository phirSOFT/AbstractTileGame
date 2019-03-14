namespace AbstractTileGame
{
    internal class RecursiveTileMap<T> : IRecursiveTileMap<T>
    {
        private readonly TileChunk<IRecursiveTileMap<T>> _submap;
        private readonly int _submapSize;

        public RecursiveTileMap(byte level, int submapSize)
        {
            Level = level;
            _submapSize = submapSize;
            _submap = new TileChunk<IRecursiveTileMap<T>>(submapSize);
        }

        public RecursiveTileMap(IRecursiveTileMap<T> center, int submapSize) : this((byte) ((center as RecursiveTileMap<T>)?.Level +1 ?? 1), submapSize)
        {
            _submap[_submapSize / 2, _submapSize / 2] = center;
        }

        public byte Level { get; }

        public T this[int x, int y]
        {
            get
            {
                (int xMajor, int xminor) = TranslateCoordinate(x);
                (int yMajor, int yminor) = TranslateCoordinate(y);

                IRecursiveTileMap<T> submap = _submap[xMajor, yMajor];
                return submap == null ? default : submap[xminor, yminor];
            }
            set
            {
                (int xMajor, int xminor) = TranslateCoordinate(x);
                (int yMajor, int yminor) = TranslateCoordinate(y);

                IRecursiveTileMap<T> submap = _submap[xMajor, yMajor] ?? (_submap[xMajor, yMajor] = GenerateSubmap());

                submap[xminor, yminor] = value;
            }
        }

        private IRecursiveTileMap<T> GenerateSubmap()
        {
            return Level == 1
                ? (IRecursiveTileMap<T>) new TileChunk<T>(_submapSize)
                : new RecursiveTileMap<T>((byte) (Level - 1), _submapSize);
        }

        private (int Major, int Minor) TranslateCoordinate(int coodinate)
        {
            int major = coodinate / _submapSize + _submapSize / 2;
            int minor = coodinate % _submapSize;

            if (coodinate < 0)
            {
                major--;
                minor += _submapSize;
            }

            return (major, minor);
        }
    }
}