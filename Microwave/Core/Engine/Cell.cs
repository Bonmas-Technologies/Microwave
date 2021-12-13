using System;

namespace Microwave.Core.Engine
{
    internal class Cell
    {
        public const int maxValue    = 945;
        public const int multiplier  = 15;
        public const int maxLiveTime = 128;

        public Coord curCoord;

        public Action<Cell> CreateCell;
        public Action<Cell> KillCell;
        
        private int energy   = maxValue / 2;
        private int water    = maxValue;
        private int livetime = 0;
        
        private Genome genome;

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
                    if (block == Blocks.Organics)
                    {
                        world.EatOrganics(curCoord + offset);
                    }
                    else if (block == Blocks.Cell)
                    {
                        world.KillCell(curCoord + offset);
                    }
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

                        CreateCell?.Invoke(new Cell(coord, this));

                        break;
                    }
                }

                if (!canBeCreated)
                {
                    KillCell?.Invoke(this);
                }

            }

            if (energy <= 0 || water <= 0)
            {
                KillCell?.Invoke(this);
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
