using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave.Core.Engine
{
    public class World
    {
        public int Width  { get; set; }
        
        public int Height { get; set; }


        private Blocks[,] map;
        private Blocks[,] cellMap;

        private List<Cell> cells;

        public World(int width, int height)
        {
            Width   = width;
            Height  = height;

            map     = new Blocks[width, height];
            cellMap = new Blocks[width, height];

            cells   = new List<Cell>(2 << 12)
            {
                new Cell(new Coord(width / 2, height / 2), OnCellCreate, OnCellKill)
            };
        }

        public void Step()
        {
            Queue<Cell> updatableCells = (Queue<Cell>)new Queue<Cell>().Union(cells);

            cellMap = new Blocks[Width, Height];
            
            foreach (Cell cell in cells)
            {
                var x = cell.curCoord.X;
                var y = cell.curCoord.Y;

                cellMap[x, y] = Blocks.Cell;
            }

            while (updatableCells.Count > 0)
            {
                var cell = updatableCells.Dequeue();

                cell.Step(this);
            }


            foreach (Cell cell in cells)
            {
                var x = cell.curCoord.X;
                var y = cell.curCoord.Y;

                if (cellMap[x, y] == Blocks.None)
                {
                    cell.Step(this);
                }
            }
        }

        public bool EatOrganics(Coord coord)
        {
            if (map[coord.X, coord.Y] == Blocks.Organics)
                map[coord.X, coord.Y] = Blocks.None;
            else
                return false;

            return true;
        }

        public void KillCell(Coord coord)
        {
            cellMap[coord.X, coord.Y] = Blocks.None;
        }


        private void OnCellCreate(Cell current)
        {
            cellMap[current.curCoord.X, current.curCoord.Y] = Blocks.Wall;

            cells.Add(current);
        }

        private void OnCellKill(Cell current)
        {
            cellMap[current.curCoord.X, current.curCoord.Y] = Blocks.None;
            map[current.curCoord.X, current.curCoord.Y] = Blocks.Organics;

            cells.Remove(current);
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
                if (cellMap[x, y] == Blocks.None)
                    return map[x, y];
                else
                    return cellMap[x, y];
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

    public enum Blocks
    {
        None,
        Wall,
        Organics,
        Cell,
    }
}
