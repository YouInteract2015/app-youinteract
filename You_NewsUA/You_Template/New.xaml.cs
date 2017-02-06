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

namespace You_NewsUA
{
    /// <summary>
    /// Interaction logic for New.xaml
    /// </summary>
    /// 

    /*
     * @author: Tomás Rodrigues 68129
     *          Pedro Abade 59385
     */
    public partial class New : Page , YouPlugin
    {
        // Width and Height from app window
        private double h, w;
        // .xml that contains the News
        XmlDocument noticias = new XmlDocument();
        // List of all News
        Noticia[] allNoticias;
        // List of News Categories
        private static XmlNodeList lista_categorias;
        // Error margin when scrolling
        private const double ScrollErrorMargin = 0.001;
        // Amount of moved pixels when scrolling
        private const int PixelScrollByAmount = 15;
        // Auxiliar variable to save new selected category
        private int changed_cat = -1;
        // Auxiliar variables to sinalyze available News or not
        private bool hasCat = true, hasNew = true;

        // Left Button Scroll properties
        public static readonly DependencyProperty PageLeftEnabledProperty = DependencyProperty.Register(
            "PageLeftEnabled", typeof(bool), typeof(MainMenu), new PropertyMetadata(false));
        // Right Button Scroll properties
        public static readonly DependencyProperty PageRightEnabledProperty = DependencyProperty.Register(
            "PageRightEnabled", typeof(bool), typeof(MainMenu), new PropertyMetadata(false));
        // Up Button Scroll properties
        public static readonly DependencyProperty PageUpEnabledProperty = DependencyProperty.Register(
            "PageUpEnabled", typeof(bool), typeof(MainMenu), new PropertyMetadata(false));
        // Down Button Scroll properties
        public static readonly DependencyProperty PageDownEnabledProperty = DependencyProperty.Register(
            "PageDownEnabled", typeof(bool), typeof(MainMenu), new PropertyMetadata(false));

        /**
         * Initialize New Page
         */
        public New()
        {
            // Initialize component
            InitializeComponent();
            // Binding kinect interactable region
            KinectApi.bindRegion(YouNewRegion);
            // Set Window properties
            setWindow();
            // Actiavte when new Categorie selected
            AllNews.newActivation += new_activateInfo;
        }

        /**
         * When selected a new Category
         */
        void new_activateInfo(int cat)
        {
            // Set TopMenu properties
            try
            {
                lista_categorias = AllNews.getCategories();

                RowDefinition row1 = new RowDefinition();
                row1.Height = new GridLength(h * 0.12);
                H_Menu.RowDefinitions.Add(row1);

                RowDefinition row2 = new RowDefinition();
                row2.Height = new GridLength(h * 0.12);
                H_Menu.RowDefinitions.Add(row2);

                H_Menu.Width = (w * 0.25) * (lista_categorias.Count / 2);
            }
            catch (Exception e) { Console.WriteLine("Can't load list of News categories!"); hasCat = false; }

            // Fetch Categories
            fetchAndSet_Cat_News(cat);
            // Show New
            showNews();
        }

        /**
         * Fetch list of Categories
         */
        private void fetchAndSet_Cat_News(int cat)
        {
            changed_cat = cat;

            int linha = 0;
            int coluna = 0;

            // Define a new Button for each Category
            try
            {
                foreach (XmlElement categ in lista_categorias)
                {
                    defineNewButton(categ.ChildNodes[1].InnerText, linha, coluna, Convert.ToInt32(categ.ChildNodes[0].InnerText));

                    if (linha == 1)
                    {
                        linha = 0;
                        coluna++;
                    }
                    else
                        linha++;
                }
            }
            catch (Exception e) { Console.WriteLine("Can't produce horizontal Menu!"); }

            // Load News by selected Category
            try
            {
                switch (cat)
                {
                    case 1000:
                        {
                            presentTitle("Destaques");

                            noticias.Load("http://services.sapo.pt/UA/Online/contents_xml?dt=1");
                            break;
                        }
                    case 1001:
                        {
                            presentTitle("Todas");

                            noticias.Load("http://services.sapo.pt/UA/Online/contents_xml?dt=0");
                            break;
                        }
                    case 1002:
                        {
                            H_Menu.Visibility = Visibility.Visible;
                            ScrollLeft.Visibility = Visibility.Visible;
                            ScrollRight.Visibility = Visibility.Visible;
                            WrapScrollPanelHorizontal.Visibility = Visibility.Visible;
                            ScrollViewerHorizontal.Visibility = Visibility.Visible;

                            noticias.Load("http://services.sapo.pt/UA/Online/contents_xml?dt=0&i=" + lista_categorias[0].ChildNodes[0].InnerText);
                            break;
                        }
                    default:
                        {
                            H_Menu.Visibility = Visibility.Visible;
                            ScrollLeft.Visibility = Visibility.Visible;
                            ScrollRight.Visibility = Visibility.Visible;
                            WrapScrollPanelHorizontal.Visibility = Visibility.Visible;
                            ScrollViewerHorizontal.Visibility = Visibility.Visible;

                            noticias.Load("http://services.sapo.pt/UA/Online/contents_xml?dt=0&i=" + cat);
                            break;
                        }
                }

                // Instantate News
                allNoticias = new Noticia[noticias.ChildNodes[1].ChildNodes[0].ChildNodes.Count - 8];

                // Save News
                int contador = 0;
                foreach (XmlElement item in noticias.ChildNodes[1].ChildNodes[0].ChildNodes)
                {
                    if (item.Name == "item")
                    {
                        try
                        {
                            allNoticias[contador] = new Noticia(item.ChildNodes[0].InnerText, item.ChildNodes[1].InnerText, item.ChildNodes[2].InnerText, item.ChildNodes[3].InnerText.Split('>')[1], item.ChildNodes[4].InnerText);
                        }
                        catch (Exception ex) { allNoticias[contador] = new Noticia(item.ChildNodes[0].InnerText, item.ChildNodes[1].InnerText, item.ChildNodes[2].InnerText, item.ChildNodes[3].InnerText, item.ChildNodes[4].InnerText); }

                        contador++;
                    }
                }

                AllNews.instantate(allNoticias.Length);
                AllNews.setNews(allNoticias);
            }
            catch (Exception e) { Console.WriteLine("Can't retrieve News .xml!"); hasNew = false; }
        }

        /**
         * Present page Title
         */
        private void presentTitle(string texto)
        {
            Viewbox news_cat = new Viewbox();
            news_cat.HorizontalAlignment = HorizontalAlignment.Center;
            news_cat.VerticalAlignment = VerticalAlignment.Center;
            System.Windows.Media.Effects.DropShadowEffect dropShadowEffect = new System.Windows.Media.Effects.DropShadowEffect();
            news_cat.Effect = dropShadowEffect;
            news_cat.Stretch = Stretch.Uniform;

            TextBlock text_cat = new TextBlock();
            text_cat.HorizontalAlignment = HorizontalAlignment.Center;
            text_cat.VerticalAlignment = VerticalAlignment.Center;
            text_cat.TextWrapping = TextWrapping.Wrap;
            text_cat.TextTrimming = TextTrimming.CharacterEllipsis;
            text_cat.FontSize = 30;
            text_cat.Foreground = new SolidColorBrush(Colors.Black);
            text_cat.Text = texto;

            news_cat.Child = text_cat;
            News_cat.Child = news_cat;
            News_cat.Width = w * 0.4;
            News_cat.Height = h * 0.12;
            News_cat.Visibility = Visibility.Visible;
            Canvas.SetTop(News_cat, h * 0.08);
            Canvas.SetLeft(News_cat, w * 0.29);
        }

        /**
         * Present News
         */
        private void showNews()
        {
            // If there are any New available
            if (hasCat && hasNew)
            {
                Noticias.Visibility = Visibility.Visible;

                int linha = 0;

                foreach (Noticia noticia in AllNews.getNews())
                {
                    // New day
                    RowDefinition dia = new RowDefinition();
                    dia.Height = new GridLength(h * 0.035);
                    Noticias.RowDefinitions.Add(dia);

                    Border border_date = new Border();
                    border_date.HorizontalAlignment = HorizontalAlignment.Left;
                    border_date.VerticalAlignment = VerticalAlignment.Center;
                    border_date.Background = new SolidColorBrush(Colors.LemonChiffon);
                    border_date.Width = Noticias.Width;
                    border_date.BorderBrush = null;

                    TextBlock text_date = new TextBlock();
                    text_date.HorizontalAlignment = HorizontalAlignment.Left;
                    text_date.TextWrapping = TextWrapping.Wrap;
                    text_date.TextTrimming = TextTrimming.CharacterEllipsis;
                    text_date.FontSize = 25;
                    text_date.Foreground = new SolidColorBrush(Colors.Blue);
                    text_date.Text = noticia.getDate();

                    border_date.Child = text_date;
                    Noticias.Children.Add(border_date);
                    Grid.SetRow(border_date, linha);
                    linha++;

                    //New Title
                    RowDefinition title = new RowDefinition();
                    Noticias.RowDefinitions.Add(title);

                    Border border_title = new Border();
                    border_title.HorizontalAlignment = HorizontalAlignment.Left;
                    border_title.VerticalAlignment = VerticalAlignment.Center;
                    border_title.Background = new SolidColorBrush(Colors.LemonChiffon);
                    border_title.Width = Noticias.Width;
                    border_title.BorderBrush = null;

                    TextBlock text_title = new TextBlock();
                    text_title.HorizontalAlignment = HorizontalAlignment.Center;
                    text_title.TextWrapping = TextWrapping.Wrap;
                    text_title.TextTrimming = TextTrimming.CharacterEllipsis;
                    text_title.FontSize = 40;
                    text_title.Foreground = new SolidColorBrush(Colors.Black);
                    System.Windows.Media.Effects.DropShadowEffect dropShadowEffect = new System.Windows.Media.Effects.DropShadowEffect();
                    text_title.Effect = dropShadowEffect;
                    text_title.Text = noticia.getTitle();

                    border_title.Child = text_title;
                    Noticias.Children.Add(border_title);
                    Grid.SetRow(border_title, linha);
                    linha++;

                    // Blanck row
                    RowDefinition blanck = new RowDefinition();
                    blanck.Height = new GridLength(h * 0.01);
                    Noticias.RowDefinitions.Add(blanck);

                    linha++;

                    //New Description
                    RowDefinition desc = new RowDefinition();
                    Noticias.RowDefinitions.Add(desc);

                    TextBox text_desc = new TextBox();
                    text_desc.MinLines = 1;
                    text_desc.AcceptsReturn = true;
                    text_desc.HorizontalAlignment = HorizontalAlignment.Left;
                    text_desc.TextWrapping = TextWrapping.Wrap;
                    text_desc.BorderBrush = null;
                    text_desc.FontSize = 25;
                    text_desc.Foreground = new SolidColorBrush(Colors.Black);
                    text_desc.Text = noticia.getDescription();
                    text_desc.Width = Noticias.Width;
                    text_desc.IsReadOnly = true;

                    Noticias.Children.Add(text_desc);
                    Grid.SetRow(text_desc, linha);
                    linha++;

                    // blanck row
                    RowDefinition blanck2 = new RowDefinition();
                    blanck2.Height = new GridLength(h * 0.09);
                    Noticias.RowDefinitions.Add(blanck2);

                    linha++;
                }
            }
            // if there are no New available
            else
            {
                // Hide MenuTop and Scroll Buttons
                News_cat.Visibility = Visibility.Hidden;

                ScrollUp.Visibility = Visibility.Hidden;
                ScrollDown.Visibility = Visibility.Hidden;
                ScrollLeft.Visibility = Visibility.Hidden;
                ScrollRight.Visibility = Visibility.Hidden;

                // Load error image
                BitmapImage bit = new BitmapImage();
                Image imgnoapps = new Image();
                YouWindow.bitmapSource(bit, AppDomain.CurrentDomain.BaseDirectory + "/App/You_NewsUA/usedApp/no_New.png", UriKind.Absolute);
                imgnoapps.Stretch = Stretch.Fill;
                imgnoapps.Source = bit;
                imgnoapps.Height = h * 0.8;
                imgnoapps.Width = w * 0.7;
                YouNewCanvas.Children.Add(imgnoapps);
                Canvas.SetTop(imgnoapps, h * 0.2);
                Canvas.SetLeft(imgnoapps, w * 0.2);

                // Reset auxiliar variables
                this.hasCat = true;
                this.hasNew = true;
            }
        }

        /**
         * Set Window properties
         */
        public void setWindow()
        {
            lista_categorias = AllNews.getCategories();

            // Get Window Measures
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();

            // Set Back Button
            backButton.Width = w * 0.11;
            backButton.Height = h * 0.22;
            Canvas.SetTop(backButton, h * 0.01);
            Canvas.SetLeft(backButton, w * 0.01);

            ScrollUp.Visibility = Visibility.Visible;
            ScrollDown.Visibility = Visibility.Visible;

            YouWindow.setScrollArrows(ScrollUp, ScrollDown, 0.07, 0.8, 0.25, 0.1, 0.91, 0.1);
            YouWindow.setScrollArrows(ScrollLeft, ScrollRight, 0.25, 0.04, 0.01, 0.12, 0.01, 0.95);

            // Set Scroll Panel (data)
            WrapScrollPanelVertical.Width = w * 0.9;
            Canvas.SetTop(WrapScrollPanelVertical, h * 0.33);
            Canvas.SetLeft(WrapScrollPanelVertical, w * 0.04);

            // Set Scroll Viewer (area)
            ScrollViewerVertical.HoverBackground = Brushes.Transparent;
            ScrollViewerVertical.Height = h * 0.6;
            ScrollViewerVertical.Width = w * 0.9;
            Canvas.SetBottom(ScrollViewerVertical, h * 0.08);
            Canvas.SetLeft(ScrollViewerVertical, w * 0.04);

            // Set Scroll Panel Horizontal (data)
            WrapScrollPanelHorizontal.Height = h * 0.25;
            Canvas.SetTop(WrapScrollPanelHorizontal, 0);
            Canvas.SetLeft(WrapScrollPanelHorizontal, w * 0.16);

            // Set Scroll Viewer (area)
            ScrollViewerHorizontal.HoverBackground = Brushes.Transparent;
            ScrollViewerHorizontal.Height = h * 0.25;
            ScrollViewerHorizontal.Width = w * 0.79;
            Canvas.SetBottom(ScrollViewerHorizontal, h * 0.75);
            Canvas.SetLeft(ScrollViewerHorizontal, w * 0.16);

            Noticias.Width = w * 0.9;
        }

        /**
         * Define new Button for TopMenu
         */
        private void defineNewButton(string texto, int linha, int coluna, int id)
        {
            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(w * 0.25);
            H_Menu.ColumnDefinitions.Add(col);

            var button = new YouButton() { };

            if (changed_cat == 1002 && linha == 0 && coluna == 0)
                button.Background = new SolidColorBrush(Colors.LightSteelBlue);
            else if (changed_cat == id)
                button.Background = new SolidColorBrush(Colors.LightSteelBlue);
            else
                button.Background = new SolidColorBrush(Colors.White);

            button.Width = (w * 0.25) - (w * 0.005);
            button.Height = w * 0.06;
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
            text.FontSize = 30;
            text.Foreground = new SolidColorBrush(Colors.Blue);
            text.Text = texto;

            button.Content = text;

            H_Menu.Children.Add(button);

            Grid.SetRow(button, linha);
            Grid.SetColumn(button, coluna);
        }

        #region YouButtonEventHandlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            if (b.Name == "backButton")
            {
                Noticias.Children.Clear();
                Noticias.RowDefinitions.Clear();
                Noticias.Visibility = Visibility.Hidden;

                News_cat.Visibility = Visibility.Hidden;

                H_Menu.Visibility = Visibility.Hidden;

                ScrollLeft.Visibility = Visibility.Hidden;
                ScrollRight.Visibility = Visibility.Hidden;

                YouNavigation.requestFrameChange(this, "YouMainMenu");
            }
            else if(b.Name.Contains("cat"))
            {
                Noticias.Children.Clear();
                Noticias.RowDefinitions.Clear();
                Noticias.Visibility = Visibility.Hidden;

                News_cat.Visibility = Visibility.Hidden;

                H_Menu.Visibility = Visibility.Hidden;

                ScrollLeft.Visibility = Visibility.Hidden;
                ScrollRight.Visibility = Visibility.Hidden;


                fetchAndSet_Cat_News(Convert.ToInt32(b.Name.Substring(3)));
                showNews();
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
            return YouNewRegion;
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

        public bool PageUpEnabled
        {
            get
            {
                return (bool)GetValue(PageUpEnabledProperty);
            }

            set
            {
                this.SetValue(PageUpEnabledProperty, value);
            }
        }
        public bool PageDownEnabled
        {
            get
            {
                return (bool)GetValue(PageDownEnabledProperty);
            }

            set
            {
                this.SetValue(PageDownEnabledProperty, value);
            }
        }

        private void UpdatePagingButtonState()
        {
            this.PageLeftEnabled = ScrollViewerHorizontal.HorizontalOffset > ScrollErrorMargin;
            this.PageRightEnabled = ScrollViewerHorizontal.HorizontalOffset < ScrollViewerHorizontal.ScrollableWidth - ScrollErrorMargin;
            this.PageUpEnabled = ScrollViewerVertical.VerticalOffset > ScrollErrorMargin;
            this.PageDownEnabled = ScrollViewerVertical.VerticalOffset < ScrollViewerVertical.ScrollableHeight - ScrollErrorMargin;
        }
        private void PageRightButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewerHorizontal.ScrollToHorizontalOffset(ScrollViewerHorizontal.HorizontalOffset + PixelScrollByAmount);
        }
        private void PageLeftButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewerHorizontal.ScrollToHorizontalOffset(ScrollViewerHorizontal.HorizontalOffset - PixelScrollByAmount);
        }
        private void PageUpButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewerVertical.ScrollToVerticalOffset(ScrollViewerVertical.VerticalOffset - PixelScrollByAmount);
        }
        private void PageDownButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewerVertical.ScrollToVerticalOffset(ScrollViewerVertical.VerticalOffset + PixelScrollByAmount);
        }
        #endregion

        private void Button_GripEvent(object sender, HandPointerEventArgs e)
        {

            var b = (YouButton)sender;
            if (b.Name == "backButton")
            {
                YouNavigation.requestFrameChange(this, "YouMainMenu");
            }
        }
    }
}
