using System;

namespace AbstractTileGame
{
    public static class HexAxisHelpers
    {
        private const float Sqrt3 = 1.7320508075688772f;
        public static (float X, float Y, float Z) CubeRound(in (float x, float y, float z) cube)
        {
            var rx = (float)Math.Round(cube.x);
            var ry = (float)Math.Round(cube.y);
            var rz = (float)Math.Round(cube.z);

            float dx = Math.Abs(rx - cube.x);
            float dy = Math.Abs(ry - cube.y);
            float dz = Math.Abs(rz - cube.z);

            if (dx > dy && dx > dz)
                return (-ry - rz, ry, rz);
            return dy > dz ? (rx, -rx - rz, rz) : (rx, ry, -rx - ry);
        }

        public static (float X, float Y, float Z) CartesianToCube(in (float x, float y) cartesian)
        {      
            return AxialToCube(CartesianToAxial(cartesian));
        }

        public static (float Q, float R) CartesianToAxial(in (float x, float y) cartesian)
        {
            float q = (Sqrt3 * cartesian.x + cartesian.y) / 3;
            float r = (2 * cartesian.y) / 3;
            return (q, r);
        }

        public static (float X, float Y) RoundAxial(in (float Q, float R) axial)
        {
            return CubeToAxial(CubeRound(AxialToCube(axial)));
        }

        public static (float Q, float R) CubeToAxial(in (float x, float y, float z) cube)
        {
            return (cube.x, cube.z);
        }

        public static (float X, float Y, float Z) AxialToCube(in (float Q, float R) axial)
        {
            return (axial.Q, -axial.Q - axial.R, axial.R);
        }
    }
}
