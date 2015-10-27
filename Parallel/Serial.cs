using System;
using System.Numerics;
using System.Drawing;


namespace Fractal
{
    /// <summary>
    /// Class for Serial processing of fractal.
    /// </summary>
    public class Serial : IFractal
    {
        public byte[,] Process(double MaxConstant, byte MaxIteration, Func<Complex, Complex> F, Func<Complex, Complex, Complex> G,
               Complex zMin, Complex zMax, int Steps, Color C1, Func<int, Color> C2)
        {
            double dX, dY;
            dX = (zMax.Real - zMin.Real) / Steps;
            dY = (zMax.Imaginary - zMin.Imaginary) / Steps;
            byte[,] result = new byte[Steps, Steps];

            for( int i = 0; i < Steps; i++)
            {
                for (int j = 0; j < Steps; j++)
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
            }

            return result;
        }
    }
}
