using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave.Core.Engine
{
    internal struct Coord
    {
        public readonly int X;
        public readonly int Y;

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord(a.X+b.X, a.Y+b.Y);
        }
        public static Coord operator -(Coord a, Coord b)
        {
            return new Coord(a.X-b.X, a.Y-b.Y);
        }
    }
}
