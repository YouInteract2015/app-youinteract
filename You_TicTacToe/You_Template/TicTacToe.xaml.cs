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

namespace You_TicTacToe
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>
    public partial class TicTacToe : Page, YouPlugin
    {
       // private YouWindow youWindow;
        private double w, h;

        public TicTacToe()
        {
            InitializeComponent();
          //youWindow = new YouWindow(this.Height, this.Width);
            KinectApi.bindRegion(YouTicTacToeRegion);
            setWindow();
           
            
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

            // TicTacToe 2 Screens Button
            TicTacToe2Screens.Width = w * 0.35;
            TicTacToe2Screens.Height = h * 0.25;
            Canvas.SetTop(TicTacToe2Screens, h * 0.60);
            Canvas.SetLeft(TicTacToe2Screens, w * 0.33);

            // TicTacToe 1 Player Button
            TicTacToe1Player.Width = w * 0.35;
            TicTacToe1Player.Height = h * 0.25;
            Canvas.SetTop(TicTacToe1Player, h * 0.30);
            Canvas.SetLeft(TicTacToe1Player, w * 0.33);

            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            if (b.Name == "Main")
            {
                YouNavigation.navigateToMainMenu(this);
            }
            else if (b.Name == "TicTacToe2Screens")
            {
                //FrameUtils.requestRestart("2s");

                YouNavigation.requestFrameChange(this, "YouTicTacToeLobby");
            }
            else if (b.Name == "TicTacToe1Player")
            {
                
                YouNavigation.requestFrameChange(this, "YouTicTacToe1Player");
            }
            
        }

        private void Button_GripEvent(object sender, HandPointerEventArgs handPointerEventArgs)
        {
            var b = (YouButton)sender;
            if (b.Name == "Main")
            {
                YouNavigation.navigateToMainMenu(this);
            }
            else if (b.Name == "TicTacToe2Screens")
            {
                FrameUtils.requestRestart("2s");
                YouNavigation.requestFrameChange(this, "YouTicTacToeLobby");
            }
            else if (b.Name == "TicTacToe1Player")
            {
                
                YouNavigation.requestFrameChange(this, "YouTicTacToe1Player");
            }

        }

        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_TicTacToe";
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
            return YouTicTacToeRegion;
        }

        #endregion

        
    }
}
