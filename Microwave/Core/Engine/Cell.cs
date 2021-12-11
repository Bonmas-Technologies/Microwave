using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave.Core.Engine
{
    internal class Cell
    {
        public const int maxValue    = 945;
        public const int multiplier  = 15;
        public const int maxLiveTime = 128;

        public Coord curCoord;

        private int energy   = maxValue / 2;
        private int water    = maxValue;
        private int livetime = 0;
        
        private Genome genome;

        public Cell(Coord coord)
        {
            curCoord = coord;

            genome = new Genome();
        }

        public Cell(Coord coord, Cell other)
        {
            curCoord = coord;

            genome = new Genome(other.genome.GetContainer());
        }

        public void Step(World world)
        {
            var container = genome.ReturnCommand();

            int energyIncome = world.GetEnergy(curCoord);
            int waterIncome = world.GetWater(curCoord);

            var offset = ArgumentToCoord(container.argument);
            
            var block = world.GetBlock(curCoord + offset);


            switch (container.command)
            {
                case GenomeStates.none:
                    genome.OffsetGenome(container.argument);
                    break;

                case GenomeStates.currentEnergy:
                    if ((container.argument * multiplier) < energy)
                        genome.OffsetGenome(container.genomeArguments[0]);
                    else
                        genome.OffsetGenome(container.genomeArguments[1]);
                    break;

                case GenomeStates.currentWater:
                    if ((container.argument * multiplier) < water)
                        genome.OffsetGenome(container.genomeArguments[0]);
                    else
                        genome.OffsetGenome(container.genomeArguments[1]);
                    break;

                case GenomeStates.currentEnergyIncome:
                    if (container.argument < energyIncome * multiplier)
                        genome.OffsetGenome(container.genomeArguments[0]);
                    else
                        genome.OffsetGenome(container.genomeArguments[1]);
                    break;
                case GenomeStates.currentWaterIncome:
                    if (container.argument < waterIncome * multiplier)
                        genome.OffsetGenome(container.genomeArguments[0]);
                    else
                        genome.OffsetGenome(container.genomeArguments[1]);
                    break;

                case GenomeStates.eatAnything:

                    
                    break;
                case GenomeStates.go:
                    if (block == Blocks.None)
                    {
                        curCoord += offset;
                    }
                    genome.OffsetGenome(container.genomeArguments[(int)block]);
                    break;

                case GenomeStates.look:
                    genome.OffsetGenome(container.genomeArguments[(int)block]);
                    break;

                case GenomeStates.photosynthesis:
                    energy += energyIncome;
                    break;
                case GenomeStates.mutate:
                    genome.Mutate();
                    break;
                case GenomeStates.sendEnergy:
                    break;
            }

            if (energy >= maxValue)
            {
                bool canBeCreated = false;
                
                for (int i = 0; i < 8; i++)
                {
                    var coord = ArgumentToCoord(i);

                    canBeCreated = world.GetBlock(coord) == Blocks.None;

                    if (canBeCreated)
                    {
                        energy = maxValue / 2;

                        // TODO: Create new Cell
                        break;
                    }
                }

                if (!canBeCreated)
                {

                }

            }


            livetime++;
            energy--;

            water += waterIncome - 1;
        }


        private Coord ArgumentToCoord(int arg)
        {
            arg = arg % 8;

            switch (arg)
            {
                case 0:
                    return new Coord(-1, 1);
                case 1:
                    return new Coord(0, 1);
                case 2:
                    return new Coord(1, 1);
                case 3:
                    return new Coord(1, 0);
                case 4:
                    return new Coord(1, -1);
                case 5:
                    return new Coord(0, -1);
                case 6:
                    return new Coord(-1, -1);
                case 7:
                    return new Coord(-1, 0);
                default:
                    return new Coord(0, 0);
            }

        }
    }
}
