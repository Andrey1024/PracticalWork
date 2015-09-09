using System;
using System.Numerics;

namespace Parallel
{
    public class Parallel : IFractal
    {
        public int MaxConstalnt { get; set; }
        public int MaxIteraion { get; set; }

        public Func<Complex, Complex> F { get; set; }
        public Func<Complex, Complex, Complex> G { get; set; }

        public Complex zMin { get; set; }
        public Complex zMax { get; set; }

        public int StepX { get; set; }
        public int StepY { get; set; }

        public Parallel() { }
        
        public int[,] Process()
        {
            int[,] result = new int[StepX, StepY];

            for (int i = 0; i < StepX; i++)
            {
                for (int j = 0; j < StepY; j++)
                {
                    int Iteration = 0;
                    double Module = 0;
                    Complex zNext;
                    Complex z = new Complex();

                    zNext = F(z);

                    while (Iteration < MaxIteraion && Module < MaxConstalnt)
                    {
                        zNext = G(zNext, z);
                        Module = zNext.Magnitude;
                        Iteration++;
                    }

                    result[i, j] = Iteration;
                }

            }

            return result;
        }
    }
}
