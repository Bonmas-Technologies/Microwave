using Microwave.Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave.Core.Render
{
    public class ConsoleRenderer : Renderer
    {
        private World world;

        public ConsoleRenderer(World world)
        {
            Console.CursorVisible = false;

            this.world = world;
        }

        public override void Render()
        {
            StringBuilder panel = new StringBuilder();

            for (int x = -1; x <= world.Width; x++)
            {
                for (int y = -1; y <= world.Height; y++)
                {
                    var block = world.GetBlock(new Coord(x, y));

                    switch (block)
                    {
                        case Blocks.None:
                            panel.Append(' ');
                            break;
                        case Blocks.Wall:
                            panel.Append('#');
                            break;
                        case Blocks.Organics:
                            panel.Append('.');
                            break;
                        case Blocks.Cell:
                            panel.Append('*');
                            break;
                    }
                }
                panel.Append(Environment.NewLine);
            }

            Console.CursorLeft = 0;
            Console.CursorTop  = 0;

            Console.Write(panel.ToString());
        }
    }
}
