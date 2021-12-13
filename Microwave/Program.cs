using Microwave.Core.Engine;
using Microwave.Core.Render;
using System;

namespace Microwave
{
    internal class Program
    {
        static void Main(string[] args)
        {
            World world = new World(27, 117); // 27 117

            Renderer renderer = new ConsoleRenderer(world);

            while (true)
            {
                for (int i = 0; i < 1; i++)
                {
                    world.Step();
                }
                renderer.Render();
            }
        }
    }
}
