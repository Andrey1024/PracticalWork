using System;
using System.Numerics;
using System.Drawing;
using System.Threading.Tasks;


namespace Fractal
{
    /// <summary>
    /// Class for Parallel processing of fractal.
    /// </summary>
    public class FractalP : IFractal
    {
        int IFractal.MaxConstant { get; set; }
        int IFractal.MaxIteration { get; set; }

        Func<Complex, Complex> IFractal.F { get; set; }
        Func<Complex, Complex, Complex> IFractal.G { get; set; }

        Complex IFractal.zMin { get; set; }
        Complex IFractal.zMax { get; set; }

        int IFractal.StepsX { get; set; }
        int IFractal.StepsY { get; set; }
        
        Color IFractal.C1 { get; set; }
        Func<int, Color> IFractal.C2 { get; set; }

        private double StepX
        {
            get { return (((IFractal)this).zMax.Real - ((IFractal)this).zMin.Real) / ((IFractal)this).StepsX; }
        }
        private double StepY
        {
            get { return (((IFractal)this).zMax.Imaginary - ((IFractal)this).zMin.Imaginary) / ((IFractal)this).StepsY; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FractalP"/> class.
        /// </summary>
        public FractalP()
            : this(2, 150, z => z, (z1, z2) => z1*z1 + z2, new Complex(-2, -2), new Complex(2, 2), Color.Red, k => Color.FromArgb(k))
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
        public FractalP(int MaxConstant, int MaxIteration, Func<Complex, Complex> F, Func<Complex, Complex, Complex> G,
               Complex zMin, Complex zMax, Color C1, Func<int, Color> C2)
        {
            ((IFractal)this).MaxConstant = MaxConstant;
            ((IFractal)this).MaxIteration = MaxIteration;
            ((IFractal)this).F = F;
            ((IFractal)this).G = G;
            ((IFractal)this).zMin = zMin;
            ((IFractal)this).zMax = zMax;
            ((IFractal)this).C1 = C1;
            ((IFractal)this).C2 = C2;
            ((IFractal)this).StepsX = ((IFractal)this).StepsY = 200;
        }
    
        int[,] IFractal.Process()
        {
            int[,] result = new int[((IFractal)this).StepsX, ((IFractal)this).StepsY];

            Parallel.For(0, ((IFractal)this).StepsX, i => {
                for (int j = 0; j < ((IFractal)this).StepsY; j++)
                {
                    int Iteration = 0;
                    double Module = 0;
                    Complex zNext;
                    Complex z = ((IFractal)this).zMin + new Complex(i * StepX, j * StepY);

                    zNext = ((IFractal)this).F(z);

                    while (Iteration < ((IFractal)this).MaxIteration && Module < ((IFractal)this).MaxConstant)
                    {
                        zNext = ((IFractal)this).G(zNext, z);
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
