using System.Numerics;
using System.Drawing;
using System;

namespace Fractal
{
    /// <summary>
    /// Interface IFractal
    /// </summary>
    public interface IFractal
    {
        byte[,] Process(double MaxConstant, byte MaxIteration, Func<Complex, Complex> F, Func<Complex, Complex, Complex> G,
               Complex zMin, Complex zMax, int Steps, Color C1, Func<int, Color> C2);
    }
}
