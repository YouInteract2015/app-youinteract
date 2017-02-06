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
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;

namespace You_Videos
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class YouPlayer : Page, YouPlugin
    {
        private double w, h;
        private string name;

        private Viewbox PanelVideos;
        private TextBlock VideosName;

        public YouPlayer()
        {
            InitializeComponent();
            KinectApi.bindRegion(YouVideoPlayerRegion);
            setWindow();

            Player.videoActivation += Player_videoActivation;
        }

        void Player_videoActivation(string pathVideo)
        {
            //VideosName.Text =  pathVideo; 
            VideoControl.ScrubbingEnabled = true;
            playVideo(pathVideo);
        }

        public void setWindow()
        {

            h = YouWindow.getHeight();
            w = YouWindow.getWidth();

            // Back Button
            MenuVideos.Width = w * 0.11;
            MenuVideos.Height = h * 0.22;
            Canvas.SetTop(MenuVideos, h * 0.01);
            Canvas.SetLeft(MenuVideos, w * 0.01);

            // Media Element
            VideoControl.Volume = 100;
            VideoControl.Width = w * 0.70; // 0.8
            VideoControl.Height = h * 0.70;   // 
            Canvas.SetBottom(VideoControl, h * 0.1);  //0.025
            Canvas.SetRight(VideoControl, w * 0.1);

            // Play Button
            Play.Width = w * 0.10;
            Play.Height = h * 0.20;
            Canvas.SetTop(Play, h * 0.25);
            Canvas.SetLeft(Play, w * 0.04);

            // Pause Button
            Pause.Width = w * 0.10;
            Pause.Height = h * 0.20;
            Canvas.SetTop(Pause, (h * 0.25) + (h * 0.2) + (h * 0.05));
            Canvas.SetLeft(Pause, w * 0.04);

            // Stop Button
            Stop.Width = w * 0.10;
            Stop.Height = h * 0.20;
            Canvas.SetTop(Stop, (h * 0.25) + (h * 0.2) * 2 + (h * 0.05) * 2);
            Canvas.SetLeft(Stop, w * 0.04);

            // Title
            PanelVideos = new Viewbox();
            VideosName = new TextBlock();

            PanelVideos.Stretch = Stretch.Uniform;
            PanelVideos.Child = VideosName;
            VideosName.TextWrapping = TextWrapping.Wrap;
            VideosName.TextTrimming = TextTrimming.CharacterEllipsis;
            VideosName.Foreground = Brushes.Black;

            PanelVideos.Width = w * 0.6;
            PanelVideos.Height = h * 0.15;

            this.YouVideoPlayerCanvas.Children.Add(PanelVideos);

            Canvas.SetTop(PanelVideos, h * 0);
            Canvas.SetLeft(PanelVideos, w * 0.55 - PanelVideos.Width * 0.5);
        }

        public void playVideo(string pathvideo)
        {
                VideoControl.Source = new Uri("Videos/" + pathvideo, UriKind.RelativeOrAbsolute);
                
                VideoControl.Play();
                VideoControl.Pause();
        }

        #region YourPlugin Interface Methods
        public string getAppName()
        {
            return "You_Videos";
        }

        public bool getIsFirstPage()
        {
            return false;
        }

        public KinectRequirements getKinectRequirements()
        {
            return new KinectRequirements(true, false, false);
        }

        public string getName()
        {
            return this.Name;
        }

        public Page getPage()
        {
            
            return this;
        }

        public KinectRegion getRegion()
        {
            return YouVideoPlayerRegion;
        }
        #endregion

        #region YouButtonEventHandlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            click(b.Name);
        }

        private void MenuVideos_OnGripEvent(object sender, HandPointerEventArgs e)
        {
            var b = (YouButton)sender;
            click(b.Name);
        }

        private void click(string name)
        {
            switch (name)
            {
                case "MenuVideos":
                    YouNavigation.requestFrameChange(this, "YouVideos");
                    break;
                case "Play":
                    VideoControl.Play();
                    break;
                case "Pause":
                    VideoControl.Pause();
                    break;
                case "Stop":
                    VideoControl.Stop();
                    break;
            }
        }

        #endregion


    }
}
