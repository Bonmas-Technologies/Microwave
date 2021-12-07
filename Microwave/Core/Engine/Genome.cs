using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave.Core.Engine
{
    public class Genome
    {
        public static Random random;

        public const int GenomeLength = 64;

        private int index;
        private int[] genome;

        public Genome()
        {
            index = 0;
            genome = new int[GenomeLength];
        }

        static Genome()
        {
            random = new Random();
        }

        public void EcexuteCommand(Cell cell)
        {
            var command = (GenomeStates)genome[index];

            switch (command)
            {
                case GenomeStates.currentEnergy:
                    break;
                case GenomeStates.currentEnergyIncome:
                    break;
                case GenomeStates.currentWater:
                    break;
                case GenomeStates.currentWaterIncome:
                    break;
                case GenomeStates.worldPosition:
                    break;
                case GenomeStates.haveConnections:
                    break;
                case GenomeStates.photosynthesis:
                    break;
                case GenomeStates.eatAnything:
                    break;
                case GenomeStates.sendEnergy:
                    break;
                case GenomeStates.look:
                    //int output = cell.LookAt(genome[index + 1] % 8);
                    break;
                case GenomeStates.go:
                    //int output = cell.GoTo(genome[index + 1] % 8);
                    break;
                default:
                    index += (int)command;

                    if (index >= GenomeLength) 
                        index -= GenomeLength;
                    break;
            }

        }
    }

    public enum GenomeStates
    {
        currentEnergy,
        currentEnergyIncome,
        
        currentWater,
        currentWaterIncome,
        
        worldPosition,
        haveConnections,

        photosynthesis,
        eatAnything,
        sendEnergy,

        look,
        turn,
        go
    }
}
