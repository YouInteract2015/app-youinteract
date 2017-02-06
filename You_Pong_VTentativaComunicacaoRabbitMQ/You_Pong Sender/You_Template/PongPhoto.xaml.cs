using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using YouInteract.YouPlugin_Developing;


namespace You_Pong
{
    /// <summary>
    /// Interaction logic for PongPhoto.xaml
    /// </summary>
    public partial class PongPhoto : Page, YouPlugin
    {
        double w,h;
        DispatcherTimer t = new DispatcherTimer();
        int countdown = 3;
        int pos = 0;

        public PongPhoto()
        {
            InitializeComponent();
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();
            this.Loaded += PongPhoto_Loaded;
            this.Unloaded += PongPhoto_Unloaded;
            FrameUtils.NewHigh += FrameUtils_NewHigh;
            t.Interval = new TimeSpan(0, 0, 1);
            t.Tick += t_Tick;
            setWindow();
        }

        void PongPhoto_Unloaded(object sender, RoutedEventArgs e)
        {
            KinectApi.ColorStreamEvent -= KinectApi_ColorStreamEvent;
        }

        void FrameUtils_NewHigh(int t)
        {
            pos = t;
        }
        
        void t_Tick(object sender, EventArgs e)
        {
            if (countdown == 0)
            {
                t.Stop();
                saveImage(KinectVideo);
                YouNavigation.requestFrameChange(this, "YouPongViewHighscores");
            }
            cd.Text = countdown.ToString();
            countdown--;

           
        }

        private void setWindow()
        {
            KinectVideo.Width = w * 0.8;
            KinectVideo.Height = h * 0.8;
            Canvas.SetTop(KinectVideo, h*0.1);
            Canvas.SetLeft(KinectVideo, w * 0.1);
            Botao.Width = 0.3 *w;
            Botao.Height = 0.3 * h;
            Canvas.SetLeft(Botao, w * 0.4);
            Canvas.SetTop(Botao, h * 0.4);
            cd.Width = 0.2 * w;
            cd.Height = 0.2 * h;
            Canvas.SetLeft(cd, w * 0.4);
            Canvas.SetTop(cd, h * 0.4);
            cd.Visibility = Visibility.Hidden;
        }

        private void KinectApi_ColorStreamEvent(BitmapSource e)
        {
            Console.WriteLine("Color Stream - Pong Photo");
            KinectVideo.Source = e;
        }

        void PongPhoto_Loaded(object sender, RoutedEventArgs e)
        {
            KinectApi.ColorStreamEvent += KinectApi_ColorStreamEvent;
            
            countdown = 3;
            cd.Visibility = Visibility.Hidden;
            KinectApi.setColor(true);
        }

        private void saveImage(Image PaintImage)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)PaintImage.Source));
            using (FileStream stream = new FileStream("Pong"+ pos +".png", FileMode.Create))
                encoder.Save(stream);
        }


        //YouPlugin
        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_Pong";
        }
        // To identify the main page of the Plugin
        public bool getIsFirstPage()
        {
            return false;
        }
        // To identify which Kinect Requirements need to be active
        // Kinect Region; Skeleton Stream; Interaction Stream
        public KinectRequirements getKinectRequirements()
        {
            return new KinectRequirements(true, false, false);
        }
        // To identify the page name
        public string getName()
        {
            return this.Name;
        }
        // This Page
        public Page getPage()
        {
            return this;
        }
        // To identigy the kinect Region
        // Return your Kinect Region if it is active
        // else return Null
        public KinectRegion getRegion()
        {
            return KinectRegion;
        }

        #endregion

        private void Botao_Click(object sender, RoutedEventArgs e)
        {
            cd.Visibility = Visibility.Visible;
            Botao.Visibility = Visibility.Hidden;
            t.Start();
        }

        private void Botao_OnGripEvent(object sender, HandPointerEventArgs e)
        {
            cd.Visibility = Visibility.Visible;
            Botao.Visibility = Visibility.Hidden;
            t.Start();
        }
    }
}
