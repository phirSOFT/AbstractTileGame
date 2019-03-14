namespace AbstractTileGame
{
    internal interface IRecursiveTileMap<T>
    {
        T this[int x, int y] { get; set; }
    }
}