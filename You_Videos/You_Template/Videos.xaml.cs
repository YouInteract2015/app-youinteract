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
using System.Xml;
using System.Xml.Linq;
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;


namespace You_Videos
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>

    /*
     * @author: Vasco Santos, 64191 - 2014
     * @author:  Rui Oliveira 68779 - 2015
     * 
     */

    public partial class Videos : Page, YouPlugin
    {
        public static readonly DependencyProperty PageLeftEnabledProperty = DependencyProperty.Register(
            "PageLeftEnabled", typeof(bool), typeof(Videos), new PropertyMetadata(false));

        public static readonly DependencyProperty PageRightEnabledProperty = DependencyProperty.Register(
            "PageRightEnabled", typeof(bool), typeof(Videos), new PropertyMetadata(false));

        private const double ScrollErrorMargin = 0.001;
        private const int PixelScrollByAmount = 15;
        private double w, h;
        private int i;

        private Video[] myVideos;
        private string[] videoConfigura;
        private string[] conf;

        private Viewbox PanelVideoName;
        private Viewbox PanelVideos;
        private TextBlock VideosName;
        private TextBlock VideosDescription;
        private XDocument doc;
        private List<string> videosConfig;

        public Videos()
        {
            InitializeComponent();

            KinectApi.bindRegion(YouVideosRegion);
            // Loader
            InitializeMyVideos();
            setWindow();
        }

        public void setWindow()
        {
            // Get Window Measures
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();

            // Set Title
            setTitle();

            // Back Button
            MainMenuButton.Width = w * 0.11;
            MainMenuButton.Height = h * 0.22;
            Canvas.SetTop(MainMenuButton, h * 0.01);
            Canvas.SetLeft(MainMenuButton, w * 0.01);

            if (myVideos.Length == 0)
            {
                ScrollLeft.Visibility = Visibility.Hidden;
                ScrollRight.Visibility = Visibility.Hidden;

                BitmapImage bit = new BitmapImage();
                Image imgnoapps = new Image();
                YouWindow.bitmapSource(bit, "/You_Videos;component/Images/sad.png", UriKind.Relative);
                imgnoapps.Stretch = Stretch.Fill;
                imgnoapps.Source = bit;
                imgnoapps.Height = h * 0.3;
                imgnoapps.Width = w * 0.3;
                YouVideosCanvas.Children.Add(imgnoapps);
                Canvas.SetTop(imgnoapps, h * 0.35);
                Canvas.SetLeft(imgnoapps, w / 2 - imgnoapps.Width / 2);

                PanelNoapps.Width = w * 0.3;
                PanelNoapps.Height = h * 0.3;
                NoVideos.Text = "No videos!";
                Canvas.SetTop(PanelNoapps, h * 0.35);
                Canvas.SetRight(PanelNoapps, w * 0.025);

                PanelError.Width = w * 0.3;
                PanelError.Height = h * 0.3;
                Error.Text = "Try Again Later";
                Canvas.SetTop(PanelError, h * 0.35);
                Canvas.SetLeft(PanelError, w * 0.025);
            }
            // Se existem videos
            else
            {
                // Set Scroll Arrows
                YouWindow.setScrollArrows(ScrollLeft, ScrollRight, 0.65, 0.1, 0.23, 0, 0.23, 0.9);

                // Scroll Panel
                WrapScrollPanel.Height = h * 0.4;
                Canvas.SetTop(WrapScrollPanel, h * 0.27);
                Canvas.SetLeft(WrapScrollPanel, 0);

                // scrollViewer
                ScrollViewer.HoverBackground = Brushes.Transparent;
                ScrollViewer.Height = h * 0.4;
                ScrollViewer.Width = w;
                Canvas.SetTop(ScrollViewer, h * 0.27);
                Canvas.SetLeft(ScrollViewer, 0);

                // Add in display content
                for (i = 0; i < myVideos.Length; i++)
                {
                    // Video Button
                    var button = new YouButton() { };
                    button.Background = new SolidColorBrush(Colors.LightSeaGreen);
                    button.Width = w * 0.315;
                    button.Height = h * 0.4;
                    button.Name = "VideoToPlay" + myVideos[i].getId();
                    button.Click += new RoutedEventHandler(Button_Click);
                    button.EnterEvent += new onHandEnterHandler(Button_Hover_Event);
                    button.LeaveEvent += new onhandLeaveHandler(Button_Leave_Event);

                    // Image Button
                    BitmapImage bitmap = new BitmapImage();
                    Image img = new Image();
                    var real = AppDomain.CurrentDomain.BaseDirectory + "/images/" + myVideos[i].getpathpreview();
                    YouWindow.bitmapSource(bitmap, real, UriKind.Absolute);
                    img.Stretch = Stretch.Fill;
                    img.Source = bitmap;
                    button.Content = img;
                    button.Background = new ImageBrush(bitmap);

                    // Add to Scroller
                    this.WrapScrollPanel.Children.Add(button);
                }
                // Hover Panel
                PanelVideoName = new Viewbox();
                VideosName = new TextBlock();

                PanelVideos = new Viewbox();
                VideosDescription = new TextBlock();

                PanelVideoName.Stretch = Stretch.Uniform;
                PanelVideoName.Child = VideosName;
                VideosName.TextWrapping = TextWrapping.Wrap;
                VideosName.TextTrimming = TextTrimming.CharacterEllipsis;
                VideosName.Text = "";

                PanelVideoName.Width = w * 0.6;
                PanelVideoName.Height = h * 0.1;

                this.YouVideosCanvas.Children.Add((PanelVideoName));

                Canvas.SetTop(PanelVideoName, h * 0.65);
                Canvas.SetLeft(PanelVideoName, w * 0.2);

                PanelVideos.Stretch = Stretch.Uniform;
                PanelVideos.Child = VideosDescription;


                VideosDescription.TextWrapping = TextWrapping.Wrap;
                VideosDescription.TextTrimming = TextTrimming.CharacterEllipsis;

                VideosDescription.Text = "Passe a mão por cima do video para ver detalhes";

                PanelVideos.Width = w * 0.75;
                PanelVideos.Height = h * 0.25;

                this.YouVideosCanvas.Children.Add(PanelVideos);

                Canvas.SetTop(PanelVideos, h * 0.75);
                Canvas.SetLeft(PanelVideos, w * 0.125);
                
            }
            
            
        }

        // Initialize Video Array by Portal Information
        private void InitializeMyVideos()
        {
            doc = null;
            try
            {
                doc = YouXMLFetcher.getAppXml(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            ;

            myVideos = new Video[doc.Descendants("video").Count()];
            int contador = 0;
            foreach (var child in doc.Descendants("video"))
            {
                Console.WriteLine("Video:");
                string name = child.Element("title").Value;
                Console.WriteLine(name);
                string video = child.Element("path").Value;
                Console.WriteLine(video);
                string prev = child.Element("preview").Value;
                Console.WriteLine(prev);
                string desc = child.Element("description").Value;
                Console.WriteLine(desc);
                
                Console.WriteLine(contador);

                myVideos[contador] = new Video(++contador, name,desc, video, prev);

            }
            AllVideos.instantate(myVideos.Length);
            AllVideos.setVideos(myVideos);

        }

        // Get Video Name by ID
        public string getVideoName(int i)
        {
            foreach (var v in myVideos)
            {
                if (v.getId() == i)
                    return v.getName();
            }
            return "";
        }

        // Get Video Name by path
        public string getVideoName(string path)
        {
            foreach (var v in myVideos)
            {
                if (v.getpathvideo().Equals(path))
                    return v.getName();
            }
            return "";
        }

        // Get Video path by ID
        public string getVideoPath(int i)
        {
            foreach (var v in myVideos)
            {
                if (v.getId() == i)
                    return v.getpathvideo();
            }
            return "";
        }

        // Get Video Description by ID
        public string getVideoDescription(int i)
        {
            foreach (var v in myVideos)
            {
                if (v.getId() == i)
                    return v.getDescription();
            }
            return "";
        }

        private void setTitle()
        {
            BitmapImage bitmapT = new BitmapImage();
            Image imgT = new Image();
            bitmapT.BeginInit();
            bitmapT.UriSource = new Uri("/You_Videos;component/Images/Themes/Theme1/Videos/videos.png", UriKind.Relative);
            bitmapT.EndInit();
            imgT.Stretch = Stretch.Fill;
            imgT.Source = bitmapT;
            Titulo.Stretch = Stretch.Fill;
            Titulo.Source = bitmapT;

            Titulo.Width = w * 0.6;
            Titulo.Height = h * 0.25;
            Canvas.SetTop(Titulo, 0);
            Canvas.SetLeft(Titulo, w * 0.5 - Titulo.Width * 0.5);
        }

        // Hover Handler
        public void myHoverHandler(int id)
        {
            if (id == -1)
            {
                VideosDescription.Text = "Mão em cima do video para ver os seus detalhes";
                VideosName.Text = "";
            }
            else
            {
                string description = getVideoDescription(id);
                string finalDescription = "";

                if (description.Length > 60)
                {
                    int countLinhas = description.Length / 60;
                    int count = 0;
                    char c = ' ';
                    for (int i = 0; (i < countLinhas + 1) && count < description.Length; i++)
                    {
                        while (((count < (i + 1) * 60) || (c != ' ')) && count < description.Length)
                        {
                            c = description[count];
                            finalDescription += c.ToString();
                            count++;
                        }
                        finalDescription += "\n";
                    }
                }
                else
                {
                    finalDescription = description;
                }
                VideosName.Text = getVideoName(id);
                VideosDescription.Text = finalDescription;
            }
        }

        #region YouButtonEventHandlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            if (b.Name == "MainMenuButton")
            {
                YouNavigation.navigateToMainMenu(this);
            }
            else if (b.Name.Contains("VideoToPlay"))
            {
                string videoNumber = "";
                char[] myChar = b.Name.ToCharArray();

                foreach (char ch in myChar)
                {
                    if (char.IsDigit(ch))
                    {
                        videoNumber += ch.ToString();
                    }
                }
                int videoID = Convert.ToInt32(videoNumber);
                string pathVideo = getVideoPath(videoID);
                Player.setActiveVideo(pathVideo);
                YouNavigation.requestFrameChange(this, "YouVideoPlayer");
            }
        }


        private void Button_Leave_Event(object sender, HandPointerEventArgs e)
        {
            myHoverHandler(-1);
        }

        private void Button_Hover_Event(object sender, HandPointerEventArgs e)
        {

            var b = (YouButton)e.OriginalSource;
            if (b.Name.Contains("VideoToPlay"))
            {
                string videoNumber = "";
                char[] myChar = b.Name.ToCharArray();

                foreach (char ch in myChar)
                {
                    if (char.IsDigit(ch))
                    {
                        videoNumber += ch.ToString();
                    }
                }
                int videoID = Convert.ToInt32(videoNumber);
                myHoverHandler(videoID);
            }
        }

        #endregion

        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_Videos";
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
            return YouVideosRegion;
        }

        #endregion

        #region Scroller Controllers
        public bool PageLeftEnabled
        {
            get
            {
                return (bool)GetValue(PageLeftEnabledProperty);
            }

            set
            {
                this.SetValue(PageLeftEnabledProperty, value);
            }
        }
        public bool PageRightEnabled
        {
            get
            {
                return (bool)GetValue(PageRightEnabledProperty);
            }

            set
            {
                this.SetValue(PageRightEnabledProperty, value);
            }
        }

        private void UpdatePagingButtonState()
        {
            this.PageLeftEnabled = ScrollViewer.HorizontalOffset > ScrollErrorMargin;
            this.PageRightEnabled = ScrollViewer.HorizontalOffset < ScrollViewer.ScrollableWidth - ScrollErrorMargin;
        }
        private void PageRightButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + PixelScrollByAmount);
        }
        private void PageLeftButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - PixelScrollByAmount);
        }
        #endregion

        private void Button_GripEvent(object sender, HandPointerEventArgs e)
        {
           
            var b = (YouButton)sender;
            if (b.Name == "MainMenuButton")
            {
                YouNavigation.navigateToMainMenu(this);
            }
        }

    }
}
