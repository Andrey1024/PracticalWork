using System;
using Fractal;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ViewModel
{
    public class FractalVM: ViewModelBase, IFractal
    {
        private FractalP fractal;

        private bool isMandelbrot, isJulia, isCustom;

        private BitmapSource image;

        private bool isRendering, waitRender;
        private double juliaReal, juliaImaginary;

        public bool IsCustom
        {
            get { return isCustom; }
            set
            {
                if (value)
                {
                    isCustom = true;
                    RenderImage();
                }
                else
                    isCustom = false;
                NotifyPropertyChanged();
            }
        }

        public bool IsMandelbrot
        {
            get { return isMandelbrot; }
            set {
                if (value) {
                    isMandelbrot = true;
                    setMandelbrot();
                    RenderImage();
                }
                else
                    isMandelbrot = false;
                NotifyPropertyChanged();
            }
        }

        public bool IsJulia
        {
            get { return isJulia; }
            set
            {
                if (value == true) {
                    isJulia = true;
                    setJulia();
                    RenderImage();
                }
                else
                    isJulia = false;
                NotifyPropertyChanged();
            }
        }

        public double xMin {
            get { return fractal.xMin; }
            set {
                lock(fractal)
                    fractal.xMin = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public double xMax
        {
            get { return fractal.xMax; }
            set {
                lock(fractal)
                    fractal.xMax = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public double yMin
        {
            get { return fractal.yMin; }
            set
            {
                lock(fractal)
                    fractal.yMin = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public double yMax
        {
            get { return fractal.yMax; }
            set
            {
                lock(fractal)
                    fractal.yMax = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public int Steps
        {
            get { return fractal.Steps; }
            set
            {
                lock(fractal)
                    fractal.Steps = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }
        
        public byte MaxIteration
        {
            get { return fractal.MaxIteration; }
            set { fractal.MaxIteration = value; }
        }

        public int MaxConstant
        {
            get { return fractal.MaxConstant; }
            set { fractal.MaxConstant = value; }
        }

        public System.Drawing.Color C1
        {
            get { return fractal.C1; }
            set { fractal.C1 = value; }
        }

        public Func<int, System.Drawing.Color> C2
        {
            get { return fractal.C2; }
            set { fractal.C2 = value; }
        }

        public Func<Complex, Complex> F 
        {
            get { return fractal.F; }
            set { fractal.F = value; }
        }
        public Func<Complex, Complex, Complex> G 
        {
            get { return fractal.G; }
            set { fractal.G = value; }
        }

        public BitmapSource Image
        {
            get { return image; }
            set {
                image = value;
                NotifyPropertyChanged();
            }
        }

        public double JuliaReal
        {
            get { return juliaReal; }
            set
            {
                juliaReal = value;
                lock(fractal)
                    fractal.G = (z1, z2) => z1 * z1 + new Complex(juliaReal, juliaImaginary);
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public double JuliaImaginary
        {
            get { return juliaImaginary; }
            set
            {
                juliaImaginary = value;
                lock (fractal)
                    fractal.G = (z1, z2) => z1 * z1 + new Complex(juliaReal, juliaImaginary);
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        private void setMandelbrot()
        {
            lock(fractal)
            {
                xMin = yMin = -2;
                xMax = yMax =  2;
                fractal.G = (z1, z2) => z1 * z1 + z2;
            }

        }

        private void setJulia()
        {
            lock (fractal)
            {
                xMin = yMin = -2;
                xMax = yMax = 2;
                fractal.G = (z1, z2) => z1 * z1 + new Complex(juliaReal, juliaImaginary);
            }

        }
        
        private Task<BitmapSource> Render ()
        {
            return Task.Run<BitmapSource>(() =>
            {
                BitmapSource result;
                int X, Y;
                lock (fractal)
                    X = Y = fractal.Steps;
                byte[,] source = fractal.Process();

                int stride = X * 3;
                byte[] pixels = new byte[Y * stride];
                int p = 0;

                for (int j = 0; j < Y; j++)
                {
                    for (int i = 0; i < X; i++)
                    {
                        var current = source[i, j];
                        pixels[p++] = (current == MaxIteration ? C1 : C2(current)).R;
                        pixels[p++] = (current == MaxIteration ? C1 : C2(current)).G;
                        pixels[p++] = (current == MaxIteration ? C1 : C2(current)).B;
                    }
                }
                result =  BitmapSource.Create(X, Y, 96, 96, PixelFormats.Rgb24, null, pixels, stride);
                result.Freeze();
                return result;
            });
        }

        private async void RenderImageAsync()
        {
            isRendering = true;
            Image = await Render();
            isRendering = false;
            if (waitRender)
            {
                RenderImageAsync();
                waitRender = false;
            }
        }

        private void RenderImage()
        {
            if (isRendering)
            {
                waitRender = true;
            }
            else
            {
                RenderImageAsync();
            }
        }

        public FractalVM()
        {
            fractal = new FractalP();
            IsJulia = IsCustom = false;
            isRendering = waitRender = false;
            JuliaReal = 0.3;
            JuliaImaginary = 0.6;
            IsMandelbrot = true;
        }
    }
}
