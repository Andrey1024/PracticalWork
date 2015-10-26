using System.Numerics;

namespace Fractal
{
    /// <summary>
    /// Interface IFractal
    /// </summary>
    public interface IFractal
    {
        int Steps { get; set; }

        double xMin { get; set; }
        double xMax { get; set; }
        double yMin { get; set; }
        double yMax { get; set; }

    }
}
