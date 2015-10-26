using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Drawing;


namespace Fractal
{
    /// <summary>
    /// Class for Parallel processing of fractal.
    /// </summary>
    public class FractalP : IFractal
    {
        public int MaxConstant { get; set; }
        public byte MaxIteration { get; set; }

        public Func<Complex, Complex> F { get; set; }
        public Func<Complex, Complex, Complex> G { get; set; }



        private Complex zMin {
            get { return new Complex(xMin, yMin); }
            set { xMin = value.Real; yMin = value.Imaginary; }
        }
        private Complex zMax {
            get { return new Complex(xMax, yMax); }
            set { xMax = value.Real; yMax = value.Imaginary; }
        }
        

        public Color C1 { get; set; }
        public Func<int, Color> C2 { get; set; }

        private double StepX
        {
            get { return (zMax.Real - zMin.Real) / Steps; }
        }
        private double StepY
        {
            get { return (zMax.Imaginary - zMin.Imaginary) / Steps; }
        }

        public int Steps { get; set; }


        public double xMin { get; set; }
        public double xMax { get; set; }
        public double yMin { get; set; }
        public double yMax { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FractalP"/> class.
        /// </summary>
        public FractalP()
            : this(2, 150, z => z, (z1, z2) => z1*z1 + z2, new Complex(-2, -2), new Complex(2, 2), Color.Green, k => Color.FromArgb(k, (k * 2)%255 , (k * 3)%255))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FractalP"/> class.
        /// </summary>
        /// <param name="MaxConstant">The maximum constant.</param>
        /// <param name="MaxIteration">The maximum iteration.</param>
        /// <param name="F">The f.</param>
        /// <param name="G">The g.</param>
        /// <param name="zMin">The z minimum.</param>
        /// <param name="zMax">The z maximum.</param>
        /// <param name="C1">The c1.</param>
        /// <param name="C2">The c2.</param>
        public FractalP(int MaxConstant, byte MaxIteration, Func<Complex, Complex> F, Func<Complex, Complex, Complex> G,
               Complex zMin, Complex zMax, Color C1, Func<int, Color> C2)
        {
            this.MaxConstant = MaxConstant;
            this.MaxIteration = MaxIteration;
            this.F = F;
            this.G = G;
            this.zMin = zMin;
            this.zMax = zMax;
            this.C1 = C1;
            this.C2 = C2;
            this.Steps = 500;
        }
    
        public byte[,] Process()
        {
            int X, Y;
            double dX, dY;
            lock(this)
            {
                X = Y = Steps;
                dX = StepX;
                dY = StepY;
            }
            byte[,] result = new byte[X, Y];

            Parallel.For(0, X, i => {
                for (int j = 0; j < Y; j++)
                {
                    byte Iteration = 0;
                    double Module = 0;
                    Complex zNext;
                    Complex z = zMin + new Complex(i * dX, j * dY);

                    zNext = F(z);

                    while (Iteration < MaxIteration && Module < MaxConstant)
                    {
                        zNext = G(zNext, z);
                        Module = zNext.Magnitude;
                        Iteration++;
                    }

                    result[i, j] = Iteration;
                }
            });

            return result;
        }
    }
}
