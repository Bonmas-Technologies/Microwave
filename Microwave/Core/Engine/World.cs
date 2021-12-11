using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave.Core.Engine
{
    internal class World
    {
        public int Width  { get; set; }
        
        public int Height { get; set; }


        private Blocks[,] map;

        private List<Cell> cells;

        public World(int width, int height)
        {
            Width  = width;
            Height = height;

            map = new Blocks[width, height];

            cells = new List<Cell>(2 << 12);

            cells.Add(new Cell(new Coord(width / 2, height / 2)));
        }

        public void Step()
        {

        }




        public Blocks GetBlock(Coord from)
        {
            int x = from.X;
            int y = from.Y;

            if (x == -1 || y == -1 || x == Width || y == Height)
            {
                return Blocks.Wall;
            }
            else
            {
                return map[x, y];
            }

        }

        internal int GetEnergy(Coord curCoord)
        {
            return (Height - curCoord.Y) / 2 / (Height / 8); // 0..4
        }
        internal int GetWater(Coord curCoord)
        {
            return curCoord.Y / 2 / (Height / 8);
        }
    }

    internal enum Blocks
    {
        None,
        Wall,
        Organics,
        Cell,
    }
}
