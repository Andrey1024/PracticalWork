using System;    
using System.Numerics;
using System.Drawing;

namespace Fractal
{
    /// <summary>
    /// Interface IFractal
    /// </summary>
    public interface IFractal
    {
        /// <summary>
        /// Gets or sets the z minimum.
        /// </summary>
        /// <value>The z minimum.</value>
        Complex zMin  { get; set; }
        /// <summary>
        /// Gets or sets the z maximum.
        /// </summary>
        /// <value>The z maximum.</value>
        Complex zMax  { get; set; }

        /// <summary>
        /// Gets or sets the steps x.
        /// </summary>
        /// <value>The steps x.</value>
        int StepsX { get; set; }
        /// <summary>
        /// Gets or sets the steps y.
        /// </summary>
        /// <value>The steps y.</value>
        int StepsY { get; set; }

        /// <summary>
        /// Gets or sets the maximum iteraion.
        /// </summary>
        /// <value>The maximum iteraion.</value>
        int MaxIteration  { get; set; }
        /// <summary>
        /// Gets or sets the maximum constalnt.
        /// </summary>
        /// <value>The maximum constalnt.</value>
        int MaxConstant { get; set; }

        /// <summary>
        /// Gets or sets the first color.
        /// </summary>
        /// <value>The first color.</value>
        Color C1 { get; set; }
        /// <summary>
        /// Gets or sets the second color.
        /// </summary>
        /// <value>The second color.</value>
        Func<int, Color>  C2 { get; set; }

        /// <summary>
        /// Gets or sets the f.
        /// </summary>
        /// <value>The f.</value>
        Func<Complex, Complex> F { get; set; }
        /// <summary>
        /// Gets or sets the g.
        /// </summary>
        /// <value>The g.</value>
        Func<Complex, Complex, Complex> G { get; set; }

        /// <summary>
        /// Processes the specified steps.
        /// </summary>
        /// <returns>System.Int32[].</returns>
        int[,] Process();
    }
}
