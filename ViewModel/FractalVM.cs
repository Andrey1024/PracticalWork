using System;
using Fractal;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ViewModel
{
    public class FractalVM: ViewModelBase
    {
        private IFractal fractal;
        private Object thisLock = new Object();

        private bool isMandelbrot = true, isJulia;

        private BitmapSource image;

        private bool isRendering = false, waitRender = false;
        private double juliaReal = 0.3, juliaImaginary = 0.6;

        private RelayCommand zoomIn;
        private RelayCommand zoomOut;

        private double xmin = -2, xmax = 2, ymin = -2, ymax = 2, maxConstant = 2;
        private byte maxIteration = 150;
        private int steps = 500;


        public ICommand ZoomIn
        {
            get
            {
                if (zoomIn == null)
                {
                    zoomIn = new RelayCommand(x => ZoomInProcedure(x));
                }
                return zoomIn;
            }
        }

        public ICommand ZoomOut
        {
            get
            {
                if (zoomOut == null)
                {
                    zoomOut = new RelayCommand(x => ZoomOutProcedure(x));
                }
                return zoomOut;
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
            get { return xmin; }
            set {
                lock(thisLock)
                    xmin = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public double xMax
        {
            get { return xmax; }
            set {
                lock(thisLock)
                    xmax = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public double yMin
        {
            get { return ymin; }
            set
            {
                lock(thisLock)
                    ymin = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public double yMax
        {
            get { return ymax; }
            set
            {
                lock(thisLock)
                    ymax = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public int Steps
        {
            get { return steps; }
            set
            {
                lock(thisLock)
                    steps = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }
        
        public byte MaxIteration
        {
            get { return maxIteration; }
            set {
                lock(thisLock)
                    maxIteration = value;
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        public double MaxConstant
        {
            get { return maxConstant; }
            set { maxConstant = value; }
        }

        private System.Drawing.Color C1
        {
            get; set;
        }

        private Func<int, System.Drawing.Color> C2
        {
            get; set;
        }

        private Func<Complex, Complex> F 
        {
            get; set;
        }
        private Func<Complex, Complex, Complex> G 
        {
            get; set;
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
                lock(thisLock)
                    G = (z1, z2) => z1 * z1 + new Complex(juliaReal, juliaImaginary);
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
                    G = (z1, z2) => z1 * z1 + new Complex(juliaReal, juliaImaginary);
                RenderImage();
                NotifyPropertyChanged();
            }
        }

        private void setMandelbrot()
        {
            lock(thisLock)
            {
                xMin = yMin = -2;
                xMax = yMax =  2;
                G = (z1, z2) => z1 * z1 + z2;
                F = x => x;
            }

        }

        private void setJulia()
        {
            lock (fractal)
            {
                xMin = yMin = -2;
                xMax = yMax = 2;
                G = (z1, z2) => z1 * z1 + new Complex(juliaReal, juliaImaginary);
            }

        }
        
        private Task<BitmapSource> Render ()
        {
            return Task.Run<BitmapSource>(() =>
            {
                BitmapSource result;
                int X, Y;
                lock (thisLock)
                    X = Y = steps;
                byte[,] source = fractal.Process(MaxConstant, MaxIteration, F, G, new Complex(xmin, ymin), new Complex(xmax, ymax), steps, C1, C2);

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

        private void ZoomProcedure (object e, double multiply)
        {
            System.Windows.Controls.Image img = e as System.Windows.Controls.Image;
            Point mousePos = Mouse.GetPosition((IInputElement)e);

            double CurrentDx = xMax - xMin;
            double newDxHalf = CurrentDx * multiply / 2;
            double PosX = xMin + (mousePos.X / img.ActualWidth) * CurrentDx;
            double PosY = yMin + (mousePos.Y / img.ActualHeight) * CurrentDx;
            lock (thisLock)
            {
                xmin = PosX - newDxHalf;
                xmax = PosX + newDxHalf;
                ymin = PosY - newDxHalf;
                ymax = PosY + newDxHalf;
            }
            NotifyPropertyChanged("xMin");
            NotifyPropertyChanged("xMax");
            NotifyPropertyChanged("yMin");
            NotifyPropertyChanged("yMax");

            RenderImage();
        }

        private void ZoomInProcedure(object e)
        {
            ZoomProcedure(e, 0.7);
        }

        private void ZoomOutProcedure(object e)
        {
            ZoomProcedure(e, 1.3);
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
            C1 = System.Drawing.Color.Green;
            C2 = k => System.Drawing.Color.FromArgb(k, (int)(k * 0.5) % 255, (int)(k * 1.2) % 255 );
            IsMandelbrot = true;
        }
    }
}
