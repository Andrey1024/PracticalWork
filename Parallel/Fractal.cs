using System;    
using System.Numerics;

namespace Parallel
{
    interface IFractal
    {
        Complex zMin  { get; set; }
        Complex zMax  { get; set; }

        int StepX { get; set; }
        int StepY { get; set; }

        int MaxIteraion  { get; set; }
        int MaxConstalnt { get; set; }

        Func<Complex, Complex> F { get; set; }
        Func<Complex, Complex, Complex> G { get; set; }

        int[,] Process();
    }
}
