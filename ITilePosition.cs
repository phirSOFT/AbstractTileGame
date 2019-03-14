namespace AbstractTileGame
{
    public interface ITilePosition
    {
        /// <summary>
        ///     Gets the x coordinate of the tile in the tile map coordinate system.
        /// </summary>
        int X { get; }

        /// <summary>
        ///     Gets the y coordinate of the tile in the tile map coordinate system.
        /// </summary>
        int Y { get; }
    }
}