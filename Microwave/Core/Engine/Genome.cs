using System.Collections.Generic;

namespace Microwave.Core.Engine
{
    internal class Genome
    {
        GenomeContainer genome;
        
        public Genome()
        {
            genome = new GenomeContainer();
        }

        public Genome(GenomeContainer other)
        {
            genome = other;
        }

        public GenomeContainer GetContainer()
        {
            return new GenomeContainer(genome);
        }

        public void OffsetGenome(int i)
        {
            genome.Offset(i);
        }

        public CommandContainer ReturnCommand()
        {
            CommandContainer container = new CommandContainer();

            int mainCommandIndex = genome.CurrentIndex;
            int notConvertCommant = genome.GetInt();

            container.command = GenomeContainer.IntToGenomeState(notConvertCommant);

            List<int> states = new List<int>(5);

            switch (container.command)
            {
                case GenomeStates.haveConnections:
                case GenomeStates.none:
                    container.argument = notConvertCommant;
                    break;

                case GenomeStates.currentEnergy:
                case GenomeStates.currentEnergyIncome:
                case GenomeStates.currentWater:
                case GenomeStates.currentWaterIncome:
                case GenomeStates.verticalPosition:
                    container.argument = genome.GetInt();

                    for (int i = 0; i < 2; i++)
                        states.Add(genome.GetInt());
                    break;

                case GenomeStates.look:
                case GenomeStates.go:
                case GenomeStates.eatAnything:
                    container.argument = genome.GetInt();

                    for (int i = 0; i < 5; i++)
                        states.Add(genome.GetInt());
                    break;
                case GenomeStates.mutate:
                case GenomeStates.sendEnergy:
                case GenomeStates.photosynthesis:
                    break;
            }

            for (int i = 0; i < states.Count; i++)
            {
                states[i] += mainCommandIndex - genome.CurrentIndex;
            }

            container.genomeArguments = states.ToArray();

            return container;
        }

    }

    public struct CommandContainer
    {
        public GenomeStates command;
        
        public int argument;

        public int[] genomeArguments;
    }

    public enum GenomeStates
    {
        none,

        // mind commands
        currentEnergy,
        currentEnergyIncome, 
        
        currentWater, 
        currentWaterIncome, 

        verticalPosition, 
        haveConnections, // nc
        
        look,

        // work commands
        mutate,
        photosynthesis, 
        eatAnything,
        sendEnergy,
        go
    }
}
