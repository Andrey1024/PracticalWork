using System;
using System.Numerics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Fractal
{

    public class Serial : IFractal
    {
        public int MaxConstalnt { get; set; }
        public int MaxIteraion { get; set; }

        public Func<Complex, Complex> F { get; set; }
        public Func<Complex, Complex, Complex> G { get; set; }

        public Complex zMin { get; set; }
        public Complex zMax { get; set; }

        public int StepsX { get; set; }
        public int StepsY { get; set; }

        public Color C1 { get; set; }
        public Func<int, Color> C2 { get; set; }

        private double StepX
        {
            get { return (zMax.Real - zMin.Real) / StepsX; }
        }
        private double StepY
        {
            get { return (zMax.Imaginary - zMin.Imaginary) / StepsY; }
        }

        public Serial()
            : this(2, 150, z => z, (z1, z2) => z1 * z1 + z2, new Complex(-2, -2), new Complex(2, 2), Color.Red, k => Color.FromArgb(k))
        { }

        public Serial(int MaxConstalnt, int MaxIteraion, Func<Complex, Complex> F, Func<Complex, Complex, Complex> G,
               Complex zMin, Complex zMax, Color C1, Func<int, Color> C2)
        {
            this.MaxConstalnt = MaxConstalnt;
            this.MaxIteraion = MaxIteraion;
            this.F = F;
            this.G = G;
            this.zMin = zMin;
            this.zMax = zMax;
            this.C1 = C1;
            this.C2 = C2;
            this.StepsX = this.StepsY = 200;
        }

        public int[,] Process(int steps = 200)
        {
            this.StepsX = this.StepsY = steps;
            int[,] result = new int[StepsX, StepsY];
            
             for (int i = 0; i < StepsX; i++) {
                for (int j = 0; j < StepsY; j++)
                {
                    int Iteration = 0;
                    double Module = 0;
                    Complex zNext;
                    Complex z = zMin + new Complex(i * StepX, j * StepY);

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
