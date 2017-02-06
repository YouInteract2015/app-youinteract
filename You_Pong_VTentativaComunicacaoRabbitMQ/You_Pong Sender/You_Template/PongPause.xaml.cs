using System;
using System.Collections.Generic;
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
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit.Interaction;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;

namespace You_Pong
{
    /// <summary>
    /// Interaction logic for PongPause.xaml
    /// </summary>
    /// 

    // Para guardar estado do jogo
    public partial class PongPause : Page, YouPlugin
    {
        private double w, h;
        private Ellipse pball;
        private double y1, y2;
        private Ball b1;
        private Player p1, p2;

        public PongPause()
        {
            InitializeComponent();
            w = YouWindow.getWidth();
            h = YouWindow.getHeight();
            KinectApi.bindRegion(YouPongPauseRegion);
            
            //p1 = new Player();
            //p2 = new Player();
            // Resume Button
            Resume.Width = w * 0.20;
            Resume.Height = h * 0.18;
            Canvas.SetTop(Resume, h * 0.40);
            Canvas.SetLeft(Resume, w * 0.10);
            P2.Width = P1.Width = w * 0.03;
            P2.Height = P1.Height = h*0.2;
            // Quit Element
            Quit.Width = w * 0.20;
            Quit.Height = h * 0.18;
            Canvas.SetTop(Quit, h * 0.40);
            Canvas.SetLeft(Quit, w * 0.70);

            // Restart Button
            Restart.Width = w * 0.20;
            Restart.Height = h * 0.18;
            Canvas.SetTop(Restart, h * 0.40);
            Canvas.SetLeft(Restart, w * 0.40);

            // pauseMsg
            Canvas.SetTop(pauseblock, h * 0.1);
            Canvas.SetLeft(pauseblock, w * 0.35);

            FrameUtils.Pause += FrameUtils_Pause;
        }

        void FrameUtils_Pause(Player player1, double yi1, Player player2, double yi2, Ellipse ibola, Ball b)
        {
            this.p1 = player1;
            this.p2 = player2;
            PBall = ibola;
            PBall.Width = ibola.Width;
            PBall.Height = ibola.Height;
            Canvas.SetTop(P1, yi1);
            Canvas.SetTop(P2, yi2);
            Canvas.SetLeft(P1, w * 0.05);
            Canvas.SetRight(P2, w * 0.05);
            Canvas.SetTop(PBall, b.y);
            Canvas.SetLeft(PBall, b.x);
            b1 = b;
        }
    
        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            var b = (KinectTileButton) e.OriginalSource;
            if (b.Name.Equals("Restart"))
            {
                FrameUtils.requestRestart(FrameUtils.GetMode());
                YouNavigation.requestFrameChange(this,
                    FrameUtils.GetMode() == "1p" ? "YouPong1Player" : "YouPong2Players");
            }
            else if (b.Name.Contains("Resume"))
            {
                FrameUtils.requestResume(FrameUtils.GetMode(), p1, Canvas.GetTop(P1), p2, Canvas.GetTop(P2), PBall, b1);
                YouNavigation.requestFrameChange(this,
                    FrameUtils.GetMode() == "1p" ? "YouPong1Player" : "YouPong2Players");
            }
            else
            {
                FrameUtils.requestRestart(FrameUtils.GetMode());
                YouNavigation.requestFrameChange(this, "YouPong");
            }

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
            return YouPongPauseRegion;
        }

        #endregion
    }
}
