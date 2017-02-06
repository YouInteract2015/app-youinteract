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

namespace You_Gallery
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>

    /*
     * @author: Rui Oliveira 68779 2015
     */

    public partial class Photos : Page, YouPlugin
    {
        public static readonly DependencyProperty PageLeftEnabledProperty = DependencyProperty.Register(
            "PageLeftEnabled", typeof(bool), typeof(Photos), new PropertyMetadata(false));

        public static readonly DependencyProperty PageRightEnabledProperty = DependencyProperty.Register(
            "PageRightEnabled", typeof(bool), typeof(Photos), new PropertyMetadata(false));

        private const double ScrollErrorMargin = 0.001;
        private const int PixelScrollByAmount = 15;
        private double w, h;
        private int i;

        private Photo[] myPhotos;

        private Viewbox PanelPhotoName;
        private Viewbox PanelPhotos;
        private TextBlock PhotosName;
        private TextBlock PhotosDescription;
        private XDocument doc;


        public Photos()
        {
            InitializeComponent();

            KinectApi.bindRegion(YouVideosRegion);
            // Loader
            InitializeMyPhotos();
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

            if (myPhotos.Length == 0)
            {
                ScrollLeft.Visibility = Visibility.Hidden;
                ScrollRight.Visibility = Visibility.Hidden;

                BitmapImage bit = new BitmapImage();
                Image imgnoapps = new Image();
                YouWindow.bitmapSource(bit, AppDomain.CurrentDomain.BaseDirectory + "/App/You_Gallery/usedApp/sad.png", UriKind.Absolute);
                imgnoapps.Stretch = Stretch.Fill;
                imgnoapps.Source = bit;
                imgnoapps.Height = h * 0.4;
                imgnoapps.Width = w * 0.2;
                YouVideosCanvas.Children.Add(imgnoapps);
                Canvas.SetTop(imgnoapps, h * 0.35);
                Canvas.SetLeft(imgnoapps, w / 2 - imgnoapps.Width / 2);

                PanelError.Width = w * 0.2;
                PanelError.Height = h * 0.2;
                Error.Text = "Try Again Later";
                Canvas.SetTop(PanelError, h * 0.45);
                Canvas.SetLeft(PanelError, w * 0.035);
            }
            // Se existem videos
            else
            {
                // Set Scroll Arrows
                YouWindow.setScrollArrows(ScrollLeft, ScrollRight, 0.65, 0.1, 0.23, 0, 0.23, 0.9);

                // Scroll Panel
                WrapScrollPanel.Height = h * 0.5;
                Canvas.SetTop(WrapScrollPanel, h * 0.27);
                Canvas.SetLeft(WrapScrollPanel, 0);

                // scrollViewer
                ScrollViewer.HoverBackground = Brushes.Transparent;
                ScrollViewer.Height = h * 0.5;
                ScrollViewer.Width = w;
                Canvas.SetTop(ScrollViewer, h * 0.27);
                Canvas.SetLeft(ScrollViewer, 0);

                // Add in display content
                for (i = 0; i < myPhotos.Length; i++)
                {
                    // Video Button
                    var button = new YouButton() { };
                    button.Background = new SolidColorBrush(Colors.LightSeaGreen);
                    button.Width = w * 0.415;
                    button.Height = h * 0.5;
                    button.Name = "PhotoToPlay" + myPhotos[i].getId();
                    //button.Click += new RoutedEventHandler(Button_Click);
                    button.EnterEvent += new onHandEnterHandler(Button_Hover_Event);
                    button.LeaveEvent += new onhandLeaveHandler(Button_Leave_Event);

                    // Image Button
                    BitmapImage bitmap = new BitmapImage();
                    Image img = new Image();

                    var realPath = AppDomain.CurrentDomain.BaseDirectory + "/images/" + myPhotos[i].getPath();  

                    YouWindow.bitmapSource(bitmap, realPath, UriKind.Absolute);
                    img.Stretch = Stretch.Fill;
                    img.Source = bitmap;
                    
                    button.Content = img;
                    button.Background = new ImageBrush(bitmap);
                    button.IsEnabled = false;
                    // Add to Scroller
                    this.WrapScrollPanel.Children.Add(button);
                }
                // Hover Panel
                PanelPhotoName = new Viewbox();
                PhotosName = new TextBlock();

                PanelPhotos = new Viewbox();
                PhotosDescription = new TextBlock();

                PanelPhotoName.Stretch = Stretch.Uniform;
                PanelPhotoName.Child = PhotosName;
                PhotosName.TextWrapping = TextWrapping.Wrap;
                PhotosName.TextTrimming = TextTrimming.CharacterEllipsis;
                PhotosName.Text = "";

                PanelPhotoName.Width = w * 0.6;
                PanelPhotoName.Height = h * 0.1;

                this.YouVideosCanvas.Children.Add((PanelPhotoName));

                Canvas.SetTop(PanelPhotoName, h * 0.65);
                Canvas.SetLeft(PanelPhotoName, w * 0.2);

                PanelPhotos.Stretch = Stretch.Uniform;
                PanelPhotos.Child = PhotosDescription;
                PhotosDescription.TextWrapping = TextWrapping.Wrap;
                PhotosDescription.TextTrimming = TextTrimming.CharacterEllipsis;
                //PhotosDescription.Text = "Passe a mão por cima do video para ver detalhes";

                PanelPhotos.Width = w * 0.75;
                PanelPhotos.Height = h * 0.25;

                this.YouVideosCanvas.Children.Add(PanelPhotos);

                Canvas.SetTop(PanelPhotos, h * 0.75);
                Canvas.SetLeft(PanelPhotos, w * 0.125);
            }
            
            
        }

        // Initialize Video Array by Portal Information
        private void InitializeMyPhotos()
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

            myPhotos = new Photo[doc.Descendants("image").Count()];
            int contador = 0;
            foreach (var child in doc.Descendants("image"))
            {
                //Console.WriteLine("Photo:");
                string name = child.Element("title").Value;
                //Console.WriteLine(name);
                string photo = child.Element("path").Value;
                //Console.WriteLine(photo);
                string desc = child.Element("description").Value;
                //Console.WriteLine(desc);
                //Console.WriteLine(contador);

                myPhotos[contador] = new Photo(++contador, name, photo, desc);
            }

            AllPhotos.instantate(myPhotos.Length);
            AllPhotos.setCantins(myPhotos);

        }

        // Get Video Name by ID
        public string getPhotoName(int i)
        {
            foreach (var v in myPhotos)
            {
                if (v.getId() == i)
                    return v.getName();
            }
            return "";
        }

        // Get Video Description by ID
        public string getPhotoDescription(int i)
        {
            foreach (var v in myPhotos)
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
            bitmapT.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/App/You_Gallery/usedApp/title.png", UriKind.Absolute);
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
                PhotosDescription.Text = ""; 
                PhotosName.Text = "";
            }
            else
            {
                string description = getPhotoDescription(id);
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
                PhotosName.Text = ""; 
                PhotosDescription.Text = finalDescription;
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
        }


        private void Button_Leave_Event(object sender, HandPointerEventArgs e)
        {
            myHoverHandler(-1);
        }

        private void Button_Hover_Event(object sender, HandPointerEventArgs e)
        {

            var b = (YouButton)e.OriginalSource;
            if (b.Name.Contains("PhotoToPlay"))
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
            return "You_Gallery";
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
