using System;
using Fractal;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ViewModel
{
    public class FractalVM: ViewModelBase
    {
        private FractalP fractal;

        private bool isMandelbrot, isJulia;

        private BitmapSource image;

        private bool isRendering, waitRender;

        public Nullable<bool> IsMandelbrot
        {
            get { return isMandelbrot; }
            set {
                if (value == true) {
                    isMandelbrot = true;
                    fractal.G = (z1, z2) => z1 * z1 + z2;
                    RenderImage();
                }
                else
                    isMandelbrot = false;
            }
        }

        public Nullable<bool> IsJulia
        {
            get { return isJulia; }
            set
            {
                if (value == true) {
                    isJulia = true;
                    fractal.G = (z1, z2) => z1 * z1 + new Complex(0.3, 0.6);
                    RenderImage();
                }
                else
                    isJulia = false;
            }
        }

        public Complex zMin {
            get { return fractal.zMin; }
            set {
                fractal.zMin = value;
                NotifyPropertyChanged();
            }
        }

        public Complex zMax
        {
            get { return fractal.zMax; }
            set {
                fractal.zMax = value;
                NotifyPropertyChanged();
            }
        }

        public int Steps
        {
            get { return fractal.StepsX; }
            set
            {
                    fractal.StepsX = fractal.StepsY = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public int StepsX {
            get { return fractal.StepsX; }
            set {
                fractal.StepsX = value;
                NotifyPropertyChanged();
            }
        }

        public int StepsY
        {
            get { return fractal.StepsY; }
            set {
                fractal.StepsY = value;
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

        private byte[,] getArray()
        {
            byte[,] source;
            lock (fractal)
            {
                source = fractal.Process();
            }
            return source;
        }
        
        private Task<BitmapSource> Render ()
        {
            return Task.Run<BitmapSource>(() =>
            {
                BitmapSource result;
                byte[,] source = null;

                int X = StepsX;
                int Y = StepsY;
                source = getArray();

                int stride = X * 3 + X % 4;
                byte[] pixels = new byte[Y * stride];

                for (int i = 0; i < X; i++)
                {
                    for (int j = 0; j < Y; j++)
                    {
                        var current = source[i, j];
                        pixels[i * 3 + j * stride + 0] = (current == MaxIteration ? C1 : C2(current)).R;
                        pixels[i * 3 + j * stride + 1] = (current == MaxIteration ? C1 : C2(current)).G;
                        pixels[i * 3 + j * stride + 2] = (current == MaxIteration ? C1 : C2(current)).B;
                    }
                }
                result =  BitmapSource.Create(X, Y, 96, 96, PixelFormats.Rgb24, BitmapPalettes.WebPalette, pixels, stride);
                result.Freeze();
                return result;
            });
        }

        private async void RenderImageAsync()
        {
            isRendering = true;
            image = await Render();
            image.Freeze();
            lock(this)
            {
                NotifyPropertyChanged("Image");
            }
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
            IsMandelbrot = true;
            isJulia = false;
            isRendering = waitRender = false;
        }
    }
}
