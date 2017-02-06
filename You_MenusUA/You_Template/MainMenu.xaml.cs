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

namespace You_MenusUA
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>

    /*
     * @author: Pedro Abade 59385
     *          Tomás Rodrigues 68129
     */

    public partial class MainMenu : Page, YouPlugin
    {
        // Define property for Scroll Left Button
        public static readonly DependencyProperty PageLeftEnabledProperty = DependencyProperty.Register(
            "PageLeftEnabled", typeof(bool), typeof(MainMenu), new PropertyMetadata(false));

        // Define property for Scroll Right Button
        public static readonly DependencyProperty PageRightEnabledProperty = DependencyProperty.Register(
            "PageRightEnabled", typeof(bool), typeof(MainMenu), new PropertyMetadata(false));

        // XDocument with app config received from Portal
        private XDocument config;
        // List of available canteens
        private Cantin[] myCantins;
        // Error margin when scrolling
        private const double ScrollErrorMargin = 0.001;
        // Amount of moved pixels when scrolling
        private const int PixelScrollByAmount = 15;
        // With and Height of screen window
        private double w, h;
        // Auxiliar variable used in cycles
        private int i;
        // Box were cantin name appears
        private Viewbox PanelPhotoName;
        // Panel for canteen photos
        private Viewbox PanelPhotos;
        // Text area for canteen name
        private TextBlock PhotosName;
        // Text area for canteen description
        private TextBlock PhotosDescription;
        
        /**
         * Initialize canteens
         */
        public MainMenu()
        {  
            // Inicialize component
            InitializeComponent();
            // Bind to kinect interactable area
            KinectApi.bindRegion(YouMenusRegion);
            
            // Loade canteens
            InitializeMainMenu();
            // Set window atrributes
            setWindow();
        }

        /**
         * Set window properties
         */
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
            
            // If there are no available canteen
            if (myCantins.Length==0)
            {
                // Hide scroll Buttons
                ScrollLeft.Visibility = Visibility.Hidden;
                ScrollRight.Visibility = Visibility.Hidden;

                // Present error image
                BitmapImage bit = new BitmapImage();
                Image imgnoapps = new Image();
                YouWindow.bitmapSource(bit, AppDomain.CurrentDomain.BaseDirectory + "/App/You_MenusUA/usedApp/sad.png", UriKind.Absolute);
                imgnoapps.Stretch = Stretch.Fill;
                imgnoapps.Source = bit;
                imgnoapps.Height = h * 0.4;
                imgnoapps.Width = w * 0.2;
                YouMenusCanvas.Children.Add(imgnoapps);
                Canvas.SetTop(imgnoapps, h * 0.35);
                Canvas.SetLeft(imgnoapps, w / 2 - imgnoapps.Width / 2);

                PanelNoapps.Width = w * 0.2;
                PanelNoapps.Height = h * 0.2;
                Canvas.SetTop(PanelNoapps, h * 0.35);
                Canvas.SetRight(PanelNoapps, w * 0.025);

                // Present error text
                PanelError.Width = w * 0.2;
                PanelError.Height = h * 0.2;
                Error.Text = "Try Again Later";
                Canvas.SetTop(PanelError, h * 0.45);
                Canvas.SetLeft(PanelError, w * 0.035);
            }
            // if there are at least an available canteen
            else
            {
                // Set Scroll Arrows
                YouWindow.setScrollArrows(ScrollLeft, ScrollRight, 0.65, 0.1, 0.23, 0, 0.23, 0.9);

                // Set Scroll Panel (data)
                WrapScrollPanel.Height = h * 0.5;
                Canvas.SetTop(WrapScrollPanel, h * 0.9);
                Canvas.SetLeft(WrapScrollPanel, 0);

                // Set Scroll Viewer (area)
                ScrollViewer.HoverBackground = Brushes.Transparent;
                ScrollViewer.Height = h * 0.5;
                ScrollViewer.Width = w;
                Canvas.SetBottom(ScrollViewer, h * 0.1);
                Canvas.SetLeft(ScrollViewer, 0);

                // Present available canteens
                for (i = 0; i < myCantins.Length; i++)
                {
                    // New Button for each available canteen
                    var button = new YouButton() { };
                    button.Background = new SolidColorBrush(Colors.LightSeaGreen);
                    button.Width = w * 0.3;
                    button.Height = h * 0.4;
                    button.Name = "cantin" + myCantins[i].getId();
                    button.Click += new RoutedEventHandler(Button_Click);
                    button.EnterEvent += new onHandEnterHandler(Button_Hover_Event);
                    button.LeaveEvent += new onhandLeaveHandler(Button_Leave_Event);

                    // Image Button
                    BitmapImage bitmap = new BitmapImage();
                    Image img = new Image();
                    var realPath = AppDomain.CurrentDomain.BaseDirectory + "/Images/" + myCantins[i].getPhoto(); 
                    YouWindow.bitmapSource(bitmap, realPath, UriKind.Absolute);
                    img.Stretch = Stretch.Fill;
                    img.Source = bitmap;
                    button.Content = img;
                    button.Background = new ImageBrush(bitmap);

                    // Add to Scroller
                    this.WrapScrollPanel.Children.Add(button);
                }
                // Hover Panel (information about each canteen)
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

                this.YouMenusCanvas.Children.Add((PanelPhotoName));

                Canvas.SetTop(PanelPhotoName, h * 0.65);
                Canvas.SetLeft(PanelPhotoName, w * 0.2);

                PanelPhotos.Stretch = Stretch.Uniform;
                PanelPhotos.Child = PhotosDescription;
                PhotosDescription.TextWrapping = TextWrapping.Wrap;
                PhotosDescription.TextTrimming = TextTrimming.CharacterEllipsis;
                //PhotosDescription.Text = "Passe a mão por cima do video para ver detalhes";

                PanelPhotos.Width = w * 0.75;
                PanelPhotos.Height = h * 0.25;

                this.YouMenusCanvas.Children.Add(PanelPhotos);

                Canvas.SetTop(PanelPhotos, h * 0.75);
                Canvas.SetLeft(PanelPhotos, w * 0.125);
            }
        }

        // Initialize Cantins Array by .xml Information
        private void InitializeMainMenu()
        {
            config = null;
            try
            {
                config = YouXMLFetcher.getAppXml(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            ;

            // INitialize canteens
            myCantins = new Cantin[config.Descendants("image").Count()];
            int contador = 0;
            foreach (var child in config.Descendants("image"))
            {
                string name = child.Element("title").Value;
                string photo = child.Element("path").Value;
                string desc = child.Element("description").Value;
                
                myCantins[contador] = new Cantin(++contador, name, photo, desc);
            }

            AllCantins.instantate(myCantins.Length);
            AllCantins.setCantins(myCantins);
        }

        // Get Canteen Name by ID
        public string getPhotoName(int i)
        {
            foreach (var v in myCantins)
            {
                if (v.getId() == i)
                    return v.getName();
            }
            return "";
        }

        // Get Canteen Description by ID
        public string getPhotoDescription(int i)
        {
            foreach (var v in myCantins)
            {
                if (v.getId() == i)
                    return v.getLink();
            }
            return "";
        }

        /**
         * Put title of the app visible
         */
        private void setTitle()
        {
            // Get title image
            BitmapImage bitmapT = new BitmapImage();
            Image imgT = new Image();
            bitmapT.BeginInit();
            bitmapT.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/App/You_MenusUA/usedApp/title.png", UriKind.Absolute);
            bitmapT.EndInit();
            imgT.Stretch = Stretch.Fill;
            imgT.Source = bitmapT;
            Titulo.Stretch = Stretch.Fill;
            Titulo.Source = bitmapT;

            // Size and set title
            Titulo.Width = w * 0.8;
            Titulo.Height = h * 0.5;
            Canvas.SetTop(Titulo, 0);
            Canvas.SetLeft(Titulo, w * 0.5 - Titulo.Width * 0.5);
        }

        // Hover Handler
        public void myHoverHandler(int id)
        {
            if (id == -1)
            {
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
                PhotosName.Text = getPhotoName(id);
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
            else
            {
                AllCantins.setActiveCantin(Convert.ToInt32(b.Name.Substring(6)));

                YouNavigation.requestFrameChange(this, "YouEmentas");
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
            return "You_MenusUA";
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
            return YouMenusRegion;
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
