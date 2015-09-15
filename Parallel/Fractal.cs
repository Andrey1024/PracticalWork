using System;    
using System.Numerics;
using System.Drawing;

namespace Fractal
{
    public interface IFractal
    {
        Complex zMin  { get; set; }
        Complex zMax  { get; set; }

        int StepsX { get; set; }
        int StepsY { get; set; }

        int MaxIteraion  { get; set; }
        int MaxConstalnt { get; set; }

        Color C1 { get; set; }
        Func<int, Color>  C2 { get; set; }

        Func<Complex, Complex> F { get; set; }
        Func<Complex, Complex, Complex> G { get; set; }

        int[,] Process(int steps);
    }
}
