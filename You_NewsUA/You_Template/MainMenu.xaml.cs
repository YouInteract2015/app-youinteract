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
using System.Globalization;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;

namespace You_NewsUA
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>

    /*
     * @author: Tomás Rodrigues 68129
     *          Pedro Abade 59385
     */

    public partial class MainMenu : Page, YouPlugin
    {
        // Xml that contains News Ctegories
        private XmlDocument categories = new XmlDocument();
        // List of categories
        private XmlNodeList lista_categorias;
        // Width and Height from app window
        private double w, h;

        /**
         * Instantiate app
         */
        public MainMenu()
        {
            // Initialize component
            InitializeComponent();
            // Bind to kinect interactable area
            KinectApi.bindRegion(YouNoticiasRegion);
            // Set window properties
            setWindow();
            // Inicializar MainMenu Buttons and Text
            InitializeMainMenu();
        }

        /**
         * Set MainWindow properties
         */
        public void setWindow()
        {
            // Get Window Measures
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();

            // Set Title
            setTitle();

            // Set Back Button
            MainMenuButton.Width = w * 0.11;
            MainMenuButton.Height = h * 0.22;
            Canvas.SetTop(MainMenuButton, h * 0.01);
            Canvas.SetLeft(MainMenuButton, w * 0.01);

            Categorias.Width = w * 0.34;
            Categorias.Visibility = Visibility.Visible;
            Canvas.SetTop(Categorias, h * 0.5);
            Canvas.SetLeft(Categorias, w * 0.325);
        }

        /**
         * Initialize MainMenu components
         */
        private void InitializeMainMenu()
        {
            // Load News Categories
            try
            {
                categories.Load("http://services.sapo.pt/UA/Online/categs_xml");
                lista_categorias = categories.ChildNodes[1].ChildNodes;

            }
            catch (Exception e) { Console.WriteLine("Can't load News categories!"); }

            // Present MainMenu Buttons
            defineNewButton("Destaques", 0, 1000);
            defineNewButton("Todas", 1, 1001);
            defineNewButton("Categorias", 2, 1002);
        }

        /**
         * MainMenu Button
         */
        private void defineNewButton(string texto, int linha, int id)
        {
            // New row in MainMenu table
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(h * 0.15);
            Categorias.RowDefinitions.Add(row);

            var button = new YouButton() { };
            button.Background = new SolidColorBrush(Colors.LemonChiffon);
            button.Width = (w * 0.34) - (w * 0.005);
            button.Height = h * 0.13;
            button.BorderBrush = null;
            button.Name = "cat" + id;
            button.Click += Button_Click;
            button.GripEvent += new onGripHandler(Button_GripEvent);
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalAlignment = HorizontalAlignment.Center;

            TextBlock text = new TextBlock();
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.TextWrapping = TextWrapping.Wrap;
            text.TextTrimming = TextTrimming.CharacterEllipsis;
            text.FontSize = 40;
            text.Foreground = new SolidColorBrush(Colors.Blue);
            text.Text = texto;

            button.Content = text;

            Categorias.Children.Add(button);

            Grid.SetRow(button, linha);
        }

        /**
         * Set app Title
         */
        private void setTitle()
        {
            // Get Title image
            BitmapImage bitmapT = new BitmapImage();
            Image imgT = new Image();
            bitmapT.BeginInit();
            bitmapT.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/App/You_NewsUA/usedApp/title.png", UriKind.Absolute);
            bitmapT.EndInit();
            imgT.Stretch = Stretch.Fill;
            imgT.Source = bitmapT;
            Titulo.Stretch = Stretch.Fill;
            Titulo.Source = bitmapT;

            // Size the title
            Titulo.Width = w * 0.8;
            Titulo.Height = h * 0.5;
            Canvas.SetTop(Titulo, 0);
            Canvas.SetLeft(Titulo, w * 0.5 - Titulo.Width * 0.5);
        }

        #region YouButtonEventHandlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            if (b.Name == "MainMenuButton")
            {
                YouNavigation.navigateToMainMenu(this);
            }
            else if (b.Name.Contains("cat"))
            {
                AllNews.setCategories(lista_categorias, Convert.ToInt32(b.Name.Substring(3)));
                YouNavigation.requestFrameChange(this, "YouNew");
            }
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
            }
        }

        #endregion

        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_NewsUA";
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
            return YouNoticiasRegion;
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

