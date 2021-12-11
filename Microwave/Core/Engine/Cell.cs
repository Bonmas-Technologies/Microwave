using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave.Core.Engine
{
    public class Cell
    {
        public const int maxValue    = 945;
        public const int multiplier  = 15;
        public const int maxLiveTime = 128;

        private int energy   = maxValue / 2;
        private int water    = maxValue / 2;
        private int livetime = 0;
        
        private Genome genome;

        public Cell()
        {
            genome = new Genome();
        }

        public Cell(Cell other)
        {
            genome = new Genome(other.genome.GetContainer());
        }

        public void Step()
        {
            var container = genome.ReturnCommand();

            // world.GetEnergy();

            switch (container.command)
            {
                case GenomeStates.none:
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
                    break;
                case GenomeStates.currentWaterIncome:
                    break;

                case GenomeStates.verticalPosition:
                    break;
                case GenomeStates.haveConnections:
                    break;

                case GenomeStates.go:
                    break;
                case GenomeStates.look:
                    break;
                case GenomeStates.mutate:
                    break;

                case GenomeStates.photosynthesis:
                    break;
                case GenomeStates.eatAnything:
                    break;
                case GenomeStates.sendEnergy:
                    break;
            }

            livetime++;
            energy--;
        }
    }
}
