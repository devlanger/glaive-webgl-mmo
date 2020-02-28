using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GlaiveServer
{
    public static class Utils
    {
        public static double DistanceBetween(Vector2UInt16 c1, Vector2UInt16 c2)
        {
            return Math.Sqrt(Math.Pow(c1.X - c2.X, 2) + Math.Pow(c2.Y- c1.Y, 2));
        }
    }

    public struct Vector2UInt16
    {
        public ushort X;
        public ushort Y;

        public Vector2UInt16(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }

        public static Vector2UInt16 Zero
        {
            get
            {
                return new Vector2UInt16(0, 0);
            }
        }

        public static bool operator== (Vector2UInt16 b, Vector2UInt16 c)
        {
            return (b.X == c.X && b.Y == c.Y);
        }

        public static bool operator !=(Vector2UInt16 b, Vector2UInt16 c)
        {
            return (b.X != c.X && b.Y != c.Y);
        }

        public static Vector2UInt16 operator +(Vector2UInt16 b, Vector2UInt16 c)
        {
            b.X += c.X;
            b.Y += c.Y;
            return b;
        }

        public static Vector2UInt16 operator -(Vector2UInt16 b, Vector2UInt16 c)
        {
            b.X -= c.X;
            b.Y -= c.Y;
            return b;
        }
    }
}
