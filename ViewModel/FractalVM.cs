using System;
using Fractal;
using System.Threading;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ViewModel
{
    public class FractalVM: ViewModelBase
    {
        private FractalP fractal;

        private bool isMandelbrot, isJulia;

        public Nullable<bool> IsMandelbrot
        {
            get { return isMandelbrot; }
            set {
                if (value == true) {
                    isMandelbrot = true;
                    fractal.G = (z1, z2) => z1 * z1 + z2;
                    NotifyPropertyChanged("Image");
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
                    NotifyPropertyChanged("Image");
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
            get { return RenderImage(); }
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

        private BitmapSource RenderImage()
        {
            byte[,] source = null;
            Thread getData = new Thread(() => source = getArray());
            getData.Start();
            getData.Join();

            var stride = StepsX * 3 + StepsX % 4;
            byte[] pixels = new byte[StepsY * stride];

            for (int i = 0; i < StepsX; i++)
            {
                for (int j = 0; j < StepsY; j++)
                {
                    var current = source[i, j];
                    pixels[i * 3 + j * stride + 0] = (current == MaxIteration ? C1 : C2(current)).R;
                    pixels[i * 3 + j * stride + 1] = (current == MaxIteration ? C1 : C2(current)).G;
                    pixels[i * 3 + j * stride + 2] = (current == MaxIteration ? C1 : C2(current)).B;
                }
            }
            return BitmapSource.Create(StepsX, StepsY, 96, 96, PixelFormats.Rgb24, BitmapPalettes.WebPalette, pixels, stride);
        }

        public FractalVM()
        {
            fractal = new FractalP();
            isMandelbrot = true;
            isJulia = false;
        }
    }
}
