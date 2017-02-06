using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using YouInteract.YouBasic;

namespace YouInteractV1
{
    /// <summary>
    /// Interaction logic for Screensaver.xaml
    /// </summary>
    public partial class Screensaver : Page
    {
        private double h, w;
        private DispatcherTimer timer;
        private Random r = new Random();

        public Screensaver()
        {
            InitializeComponent();

            w = MainWindow.Reference.Width;
            h = MainWindow.Reference.Height;

            ScreensaverImage.Width = h * 0.2;
            ScreensaverImage.Height = h * 0.2;
            Canvas.SetTop(ScreensaverImage,h*0.5);
            Canvas.SetRight(ScreensaverImage,w*0.5);
            timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 2)};
            timer.Tick += timer_Tick;
        }

        public void activateTimer()
        {
            timer.Start();
        }

        public void stopTimer()
        {
            timer.Stop();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            moveImg();
        }

        private void moveImg()
        {
            double max = 1;
            double min = 0;

            Canvas.SetRight(ScreensaverImage, (w - Canvas.GetRight(ScreensaverImage)) * r.NextDouble());
            Canvas.SetTop(ScreensaverImage, (h - Canvas.GetTop(ScreensaverImage)) * r.NextDouble());
            Console.WriteLine(Canvas.GetRight(ScreensaverImage) + "," + Canvas.GetTop(ScreensaverImage));
        }     
    }
}
