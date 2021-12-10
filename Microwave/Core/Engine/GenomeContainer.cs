using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave.Core.Engine
{
    internal class GenomeContainer
    {
        public const int GenomeLength   = 64;
        public const int MaxGenomeGeneratedValue = 64;
        public const int incrementSpeed = 10;

        public int CurrentLength { get { return GenomeLength; } }
        public int CurrentIndex  { get { return index; } }

        private int index;
        private int[] genome;

        private static Random random;
        private static int errorInCreation = 0;

        static GenomeContainer()
        {
            random = new Random();
        }

        public GenomeContainer(int length = GenomeLength)
        {
            index = 0;
            genome = new int[length];

            for (int i = 0; i < length; i++)
            {
                genome[i] = (int)GenomeStates.photosynthesis;
            }
        }

        public GenomeContainer(GenomeContainer other)
        {
            genome = new int[CurrentLength];

            for (int i = 0; i < genome.Length; i++)
            {
                genome[i] = other.genome[i]; 
            }

            if ((errorInCreation / 100f) > random.NextDouble())
            {
                errorInCreation = 0;

                int pos   = random.Next(GenomeLength);
                int value = random.Next(MaxGenomeGeneratedValue);

                genome[pos] = value;
            }
            else
            {
                errorInCreation += incrementSpeed;
            }
        }

        public GenomeStates GetCommand()
        {
            int i = GetInt();

            return IntToGenomeState(i);
        }

        public int GetInt()
        {
            int command = genome[index];

            Increment();

            return command;
        }

        private void Increment()
        {
            index++;

            if (index >= GenomeLength)
                index = 0;
        }

        public void Offset(int i)
        {
            index += i;

            if (index >= GenomeLength)
                index -= GenomeLength;
        }

        public static GenomeStates IntToGenomeState(int command)
        {
            if (Enum.IsDefined(typeof(GenomeStates), command))
            {
                return (GenomeStates)command;
            }
            else
            {
                return GenomeStates.none;
            }
        }
    }
}
