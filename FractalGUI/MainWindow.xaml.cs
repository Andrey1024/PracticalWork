using System.Windows;
using ViewModel;

namespace FractalGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new FractalVM();
        }

        private void Image_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

        }
    }
}
