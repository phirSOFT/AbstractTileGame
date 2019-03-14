using System;

namespace AbstractTileGame
{
    public struct TilePosition : ITilePosition, IComparable<ITilePosition>, IEquatable<ITilePosition>
    {
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is ITilePosition && Equals((TilePosition) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public TilePosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }


        public static bool operator ==(TilePosition left, TilePosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TilePosition left, TilePosition right)
        {
            return !(left == right);
        }

        public static bool operator <(TilePosition left, TilePosition right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(TilePosition left, TilePosition right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(TilePosition left, TilePosition right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(TilePosition left, TilePosition right)
        {
            return left.CompareTo(right) >= 0;
        }

        public int CompareTo(ITilePosition other)
        {
            if (X == other.X)
                return Y - other.Y;
            return X - other.X;
        }

        public bool Equals(ITilePosition other)
        {
            return CompareTo(other) == 0;
        }

        public static implicit operator TilePosition((int x, int y) position)
        {
            return new TilePosition(position.x, position.y);
        }
    }
}