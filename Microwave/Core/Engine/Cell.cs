using System;
using System.Collections.Generic;

namespace Microwave.Core.Engine
{
    internal class Cell
    {
        public const int maxValue    = 945;
        public const int multiplier  = 15;
        public const int maxLiveTime = 945;

        public Coord curCoord;

        public Action<Cell> CreateCell;
        public Action<Cell> KillCell;
        
        private int energy   = maxValue / 2;
        private int minerals = 0;
        private int livetime = 0;
        
        private Genome genome;
        private static Random random = new Random();

        public Cell(Coord coord, Action<Cell> createCell, Action<Cell> killCell)
        {
            curCoord = coord;

            CreateCell = createCell;
            KillCell   = killCell;

            genome = new Genome();
        }

        public Cell(Coord coord, Cell parent)
        {
            curCoord = coord;

            CreateCell = parent.CreateCell;
            KillCell = parent.KillCell;

            genome = new Genome(parent.genome.GetContainer());
        }

        public void Step(World world)
        {
            if (world.GetBlock(curCoord) != Blocks.Cell)
            {
                KillCell?.Invoke(this);
                return;
            }

            var container = genome.ReturnCommand();

            int energyIncome = world.GetEnergy(curCoord);
            int mineralsIncome = world.GetMinerals(curCoord);

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

                case GenomeStates.currentMinerals:
                    if ((container.argument * multiplier) < minerals)
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
                case GenomeStates.currentMineralsIncome:
                    if (container.argument < mineralsIncome * multiplier)
                        genome.OffsetGenome(container.genomeArguments[0]);
                    else
                        genome.OffsetGenome(container.genomeArguments[1]);
                    break;

                case GenomeStates.eatAnything:
                    if (block == Blocks.Organics)
                    {
                        if (world.EatOrganics(curCoord + offset))
                        {
                            energy += 10;
                        }
                    }
                    else if (block == Blocks.Cell)
                    {
                        //world.KillCell(curCoord + offset);
                    }

                    genome.OffsetGenome(container.genomeArguments[(int)block]);
                    break;

                case GenomeStates.go:
                    if (block == Blocks.None)
                    {
                        world.GoTo(curCoord, offset);
                        
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
                case GenomeStates.mineralsConvert:
                    energy += minerals;
                    minerals = 0;
                    break;
                case GenomeStates.mutate:
                    genome.Mutate();
                    genome.Mutate();
                    break;
            }

            if (energy >= maxValue)
            {

                List<Coord> WhereCanCreated = new List<Coord>(8);

                for (int i = 0; i < 8; i++)
                {
                    var coord = ArgumentToCoord(i);
                    
                    bool canBeCreated = world.GetBlock(coord + curCoord) == Blocks.None;

                    if (canBeCreated)
                    {
                        WhereCanCreated.Add(coord);
                    }
                }

                if (WhereCanCreated.Count > 0)
                {
                    energy = maxValue / 2;

                    CreateCell?.Invoke(new Cell(WhereCanCreated[random.Next(WhereCanCreated.Count)] + curCoord, this));
                }
                else
                {
                    KillCell?.Invoke(this);
                }
            }

            if (energy <= 0 || livetime >= maxLiveTime)
            {
                KillCell?.Invoke(this);
            }


            livetime++;
            energy--;

            minerals += mineralsIncome;
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
