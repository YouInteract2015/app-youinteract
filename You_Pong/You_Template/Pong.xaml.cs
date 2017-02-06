using System;
using System.CodeDom;
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
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;

namespace You_Pong
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>
    public partial class Pong : Page, YouPlugin
    {
       // private YouWindow youWindow;
        private double w, h;

        public Pong()
        {
            InitializeComponent();
          //youWindow = new YouWindow(this.Height, this.Width);
            KinectApi.bindRegion(YouPongRegion);
            setWindow();
            PongHighscores.IsEnabled = false;
        }

        public void setWindow()
        {
            // Get Window Measures

            //YouWindow = this.getHeight;
            //YouWindow = this.getWidth;
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();
            // Set Title

            BitmapImage bitmapT = new BitmapImage();
            Image imgT = new Image();
            bitmapT.BeginInit();
            bitmapT.UriSource = new Uri("", UriKind.Relative);
            bitmapT.EndInit();
            imgT.Stretch = Stretch.Fill;
            imgT.Source = bitmapT;
            titulo.Stretch = Stretch.Fill;
            titulo.Source = bitmapT;

            titulo.Width = w * 0.6;
            titulo.Height = h * 0.25;
            Canvas.SetTop(titulo, h * 0);
            Canvas.SetLeft(titulo, w * 0.5 - titulo.Width * 0.5);

            // Back Button
            Main.Width = w * 0.11;
            Main.Height = h * 0.22;
            Canvas.SetTop(Main, h * 0.01);
            Canvas.SetLeft(Main, w * 0.01);

            // Welcome msg
            welcomemsg.Height = h*0.1;
            welcomemsg.Width = w*0.6;
            Canvas.SetTop(welcomemsg,h*0.1);
            Canvas.SetLeft(welcomemsg,w*0.22);

            // Pong One Player Button
            PongOnePlayer.Width = w * 0.35;
            PongOnePlayer.Height = h * 0.25;
            Canvas.SetTop(PongOnePlayer, h * 0.25);
            Canvas.SetLeft(PongOnePlayer, w * 0.33);

            // Pong Two Players Button
            PongTwoPlayers.Width = w * 0.35;
            PongTwoPlayers.Height = h * 0.25;
            Canvas.SetTop(PongTwoPlayers, h * 0.45);
            Canvas.SetLeft(PongTwoPlayers, w * 0.325);

            //Highscores button
            PongHighscores.Width = w*0.35;
            PongHighscores.Height = h*0.25;
            Canvas.SetTop(PongHighscores,h*0.65);
            Canvas.SetLeft(PongHighscores,w*0.3);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            if (b.Name == "Main")
            {
                YouNavigation.navigateToMainMenu(this);
            }
            else if (b.Name == "PongTwoPlayers")
            {
                FrameUtils.requestRestart("2p"); 

                YouNavigation.requestFrameChange(this,"YouPong2Players");             
            }
            else if (b.Name == "PongOnePlayer")
            {
                FrameUtils.requestRestart("1p");

                YouNavigation.requestFrameChange(this, "YouPong1Player");
            }
            else if (b.Name == "PongHighscores")
            {
                YouNavigation.requestFrameChange(this, "YouPongViewHighscores");
            }
        }

        private void Button_GripEvent(object sender, HandPointerEventArgs handPointerEventArgs)
        {
            var b = (YouButton)sender;
            if (b.Name == "Main")
            {
                YouNavigation.navigateToMainMenu(this);
            }
            else if (b.Name == "PongTwoPlayers")
            {
                FrameUtils.requestRestart("2p"); 
                YouNavigation.requestFrameChange(this, "YouPong2Players");
            }
            else if (b.Name == "PongOnePlayer")
            {
                FrameUtils.requestRestart("1p");
                YouNavigation.requestFrameChange(this, "YouPong1Player");
            }
            else if (b.Name == "PongHighscores")
            {
                YouNavigation.requestFrameChange(this, "YouPongViewHighscores");
            }
        }

        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_Pong";
        }
        // To identify the main page of the Plugin
        public bool getIsFirstPage()
        {
            return true;
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
            return YouPongRegion;
        }

        #endregion

        
    }
}
