﻿using System;
using System.Numerics;
using System.Drawing;


namespace Fractal
{
    /// <summary>
    /// Class for Parallel processing of fractal.
    /// </summary>
    public class Serial : IFractal
    {
        public int MaxConstant { get; set; }
        public byte MaxIteration { get; set; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="FractalP"/> class.
        /// </summary>
        public Serial()
            : this(2, 150, z => z, (z1, z2) => z1 * z1 + z2, new Complex(-2, -2), new Complex(2, 2), Color.Red, k => Color.FromArgb(k, k * 2, k *3))
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
        public Serial(int MaxConstant, byte MaxIteration, Func<Complex, Complex> F, Func<Complex, Complex, Complex> G,
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
            this.StepsX = StepsY = 200;
        }

        public byte[,] Process()
        {
            byte[,] result = new byte[StepsX, StepsY];

            for(int i = 0; i < StepsX;  i++)
            {
                for (int j = 0; j < StepsY; j++)
                {
                    byte Iteration = 0;
                    double Module = 0;
                    Complex zNext;
                    Complex z = zMin + new Complex(i * StepX, j * StepY);

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
