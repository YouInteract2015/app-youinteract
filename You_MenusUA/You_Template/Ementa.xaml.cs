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
    /// Interaction logic for Ementa.xaml
    /// </summary>
    /// 


    /*
     * @author: Pedro Abade 59385
     *          Tomás Rodrigues 68129
     */
    public partial class Ementa : Page , YouPlugin
    {
        // Define property for Scroll Up Button
        public static readonly DependencyProperty PageUpEnabledProperty = DependencyProperty.Register(
            "PageUpEnabled", typeof(bool), typeof(MainMenu), new PropertyMetadata(false));

        // Define property for Scroll Down Button
        public static readonly DependencyProperty PageDownEnabledProperty = DependencyProperty.Register(
            "PageDownEnabled", typeof(bool), typeof(MainMenu), new PropertyMetadata(false));

        // Error margin when scrolling
        private const double ScrollErrorMargin = 0.001;
        // Amount of moved pixels when scrolling
        private const int PixelScrollByAmount = 15;
        // .xml document that contains menus information for a canteen
        private XmlDocument infoCantin = new XmlDocument();
        // With and Height of screen window
        private double w, h;
        // active canteen
        private Cantin activeCantin;
        // list of menu of a canteen
        private XmlNodeList cantinMenu = null;
        // list of original menus of a canteen
        private XmlNodeList originalCantinMenu = null;
        // date of a menu
        private string date;
        // Flags that indicates if there are any menu available
        private bool hasMenu1 = true, hasMenu2 = true;
        // saving actual day when changing for new day
        private string changed_day = null;

        /**
         * Initialize menus
         */
        public Ementa()
        {
            // initialize kinect component
            InitializeComponent();
            // bind to kinect interactable area
            KinectApi.bindRegion(YouEmentaRegion);
            // specify sizes and variables for the aplication
            setWindow();
            // New canteen selected
            AllCantins.cantinActivation += cantin_activateInfo;
        }

        /**
         * Functions activated when selecting a new canteen
         */
        void cantin_activateInfo(int cantin)
        {
            // get active canteen
            activeCantin = AllCantins.getCantin(AllCantins.getActiveCantin());

            // try to fetch menus for that canteen
            try
            {
                // fetch menu for actual selected date
                switch (System.DateTime.Now.DayOfWeek.ToString())
                {
                    case "Monday": { fetchMenu("Segunda"); break; }
                    case "Tuesday": { fetchMenu("Terça"); break; }
                    case "Wednesday": { fetchMenu("Quarta"); break; }
                    case "Thursday": { fetchMenu("Quinta"); break; }
                    case "Friday": { fetchMenu("Sexta"); break; }
                    case "Saturday": { fetchMenu("Sábado"); break; }
                    case "Sunday": { fetchMenu("Domingo"); break; }
                    default: { fetchMenu(System.DateTime.Now.DayOfWeek.ToString()); break; }
                }
              }
              catch (Exception e) { hasMenu1 = false; };

            // show fetched menu
            showMenu();
        }

        /**
         * Specify sizes for the application
         */
        public void setWindow()
        {
            // Get Window Measures
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();

            // Set Back Button
            backButton.Width = w * 0.11;
            backButton.Height = h * 0.22;
            Canvas.SetTop(backButton, h * 0.01);
            Canvas.SetLeft(backButton, w * 0.01);
        }

        /**
         * Fetch canteen menu
         */
        public void fetchMenu(string day)
        {
            // day of required menu
            string dia;
            // auxiliar variable for couting how many times .xml's were successfully readed
            int contador = 0;

            // Convert days from portuguese to english
            switch (day)
            {
                case "Segunda": { dia = "Monday"; break; }
                case "Terça": { dia = "Tuesday"; break; }
                case "Quarta": { dia = "Wednesday"; break; }
                case "Quinta": { dia = "Thursday"; break; }
                case "Sexta": { dia = "Friday"; break; }
                case "Sábado": { dia = "Saturday"; break; }
                case "Domingo": { dia = "Sunday"; break; }
                default: { dia = day; break; }
            }

            // Load menu information
            try
            {
                this.infoCantin.Load(activeCantin.getLink());

                if (activeCantin.getName().Contains("Santiago") || activeCantin.getName().Contains("santiago"))
                {
                    this.originalCantinMenu = this.infoCantin.SelectNodes("result/menus/menu[contains(@canteen,'Santiago')]");
                    this.cantinMenu = this.infoCantin.SelectNodes("result/menus/menu[contains(@canteen,'Santiago') and contains(@weekday,'" + dia + "')]");

                    this.date = this.infoCantin.SelectSingleNode("result/menus/menu[contains(@canteen,'Santiago') and contains(@weekday,'" + dia + "')]").Attributes["date"].Value;
                }
                else if (activeCantin.getName().Contains("Crasto") || activeCantin.getName().Contains("crasto"))
                {
                    this.originalCantinMenu = this.infoCantin.SelectNodes("result/menus/menu[contains(@canteen,'Crasto')]");
                    cantinMenu = this.infoCantin.SelectNodes("result/menus/menu[contains(@canteen,'Crasto') and contains(@weekday,'" + dia + "')]");

                    this.date = this.infoCantin.SelectSingleNode("result/menus/menu[contains(@canteen,'Crasto') and contains(@weekday,'" + dia + "')]").Attributes["date"].Value;
                }
                else if (activeCantin.getName().Contains("Self") || activeCantin.getName().Contains("self") || activeCantin.getName().Contains("Snack") || activeCantin.getName().Contains("snack"))
                {
                    this.originalCantinMenu = this.infoCantin.SelectNodes("result/menus/menu[contains(@canteen,'Self')]");
                    cantinMenu = this.infoCantin.SelectNodes("result/menus/menu[contains(@canteen,'Self') and contains(@weekday,'" + dia + "')]");

                    this.date = this.infoCantin.SelectSingleNode("result/menus/menu[contains(@canteen,'Self') and contains(@weekday,'" + dia + "')]").Attributes["date"].Value;
                }
                else if (activeCantin.getName() != null)
                {
                    this.originalCantinMenu = this.infoCantin.SelectNodes("result/menus/menu[contains(@date,'0')]");
                    cantinMenu = this.infoCantin.SelectNodes("result/menus/menu[contains(@weekday,'" + dia + "')]");

                    this.date = this.infoCantin.SelectSingleNode("result/menus/menu[contains(@weekday,'" + dia + "')]").Attributes["date"].Value;
                }
                else
                {
                    this.hasMenu1 = false;
                }
            }
            catch (Exception e) { Console.WriteLine("Can't retreive menu from cantin!"); this.hasMenu1 = false; }

            changed_day = day;

            // saving menu date
            try
            {
                string[] date_split = date.Split(' ');

                switch (date_split[2])
                {
                    case "Jan":
                        {
                            date_split[2] = "Janeiro";
                            break;
                        }
                    case "Feb":
                        {
                            date_split[2] = "Fevereiro";
                            break;
                        }
                    case "Mar":
                        {
                            date_split[2] = "Março";
                            break;
                        }
                    case "Apr":
                        {
                            date_split[2] = "Abril";
                            break;
                        }
                    case "May":
                        {
                            date_split[2] = "Maio";
                            break;
                        }
                    case "Jun":
                        {
                            date_split[2] = "Junho";
                            break;
                        }
                    case "Jul":
                        {
                            date_split[2] = "Julho";
                            break;
                        }
                    case "Aug":
                        {
                            date_split[2] = "Agosto";
                            break;
                        }
                    case "Sep":
                        {
                            date_split[2] = "Setembro";
                            break;
                        }
                    case "Oct":
                        {
                            date_split[2] = "Outubro";
                            break;
                        }
                    case "Nov":
                        {
                            date_split[2] = "Novembro";
                            break;
                        }
                    case "Dec":
                        {
                            date_split[2] = "Dezembro";
                            break;
                        }
                }

                date = date_split[1] + " " + date_split[2] + " " + date_split[3];

            }
            catch (Exception e) { Console.WriteLine("Can't read menu date!"); }

            // auxiliar variable for counting amount of Grid rows used presenting Lunch
            int howMany = 1;

            // present Lunch if available
            try
            {
                if (cantinMenu[0].Attributes["disabled"].Value == "0" && cantinMenu[0].Attributes["meal"].Value == "Almoço")
                {
                    // Present menu date
                    RowDefinition date_row = new RowDefinition();
                    date_row.Height = new GridLength(h * 0.03);
                    EmentaAlmoco.RowDefinitions.Add(date_row);

                    Viewbox box_date = new Viewbox();
                    box_date.Stretch = Stretch.Uniform;
                    box_date.HorizontalAlignment = HorizontalAlignment.Left;
                    box_date.VerticalAlignment = VerticalAlignment.Center;

                    TextBlock text_date = new TextBlock();
                    text_date.HorizontalAlignment = HorizontalAlignment.Left;
                    text_date.VerticalAlignment = VerticalAlignment.Center;
                    text_date.TextWrapping = TextWrapping.Wrap;
                    text_date.TextTrimming = TextTrimming.CharacterEllipsis;
                    text_date.FontSize = 20;
                    text_date.Foreground = new SolidColorBrush(Colors.Blue);
                    text_date.Text = date;

                    box_date.Child = text_date;
                    EmentaAlmoco.Children.Add(box_date);
                    Grid.SetColumnSpan(box_date, 2);
                    Grid.SetRow(box_date, 0);

                    // Present menu
                    howMany = presentLunch(cantinMenu);
                    contador++;
                }

            }
            catch (Exception e) { Console.WriteLine("Can't present Lunch!"); }

            // Present Dinner if available
            try
            {
                if (cantinMenu[1].Attributes["meal"].Value == "Jantar" && cantinMenu[1].Attributes["disabled"].Value == "0")
                {
                    // Present Dinner after presenting Lunch
                    presentDinner(cantinMenu, howMany);
                    contador++;
                }

                // Present only Dinner
                if (cantinMenu[0].Attributes["meal"].Value == "Jantar" && cantinMenu[0].Attributes["disabled"].Value == "0")
                {
                    // Present menu date
                    RowDefinition date_row = new RowDefinition();
                    date_row.Height = new GridLength(h * 0.03);
                    EmentaAlmoco.RowDefinitions.Add(date_row);

                    Viewbox box_date = new Viewbox();
                    box_date.Stretch = Stretch.Uniform;
                    box_date.HorizontalAlignment = HorizontalAlignment.Left;
                    box_date.VerticalAlignment = VerticalAlignment.Center;

                    TextBlock text_date = new TextBlock();
                    text_date.HorizontalAlignment = HorizontalAlignment.Left;
                    text_date.VerticalAlignment = VerticalAlignment.Center;
                    text_date.TextWrapping = TextWrapping.Wrap;
                    text_date.TextTrimming = TextTrimming.CharacterEllipsis;
                    text_date.FontSize = 20;
                    text_date.Foreground = new SolidColorBrush(Colors.Blue);
                    text_date.Text = date;

                    box_date.Child = text_date;
                    EmentaAlmoco.Children.Add(box_date);
                    Grid.SetColumnSpan(box_date, 2);
                    Grid.SetRow(box_date, 0);

                    // Present Dinner
                    presentDinner(cantinMenu, howMany);
                    contador++;
                }
            }
            catch (Exception e) { Console.WriteLine("Can't present Dinner!"); }

            // Present Days lateral menu
            try
            {
                switch (originalCantinMenu[0].Attributes["weekday"].Value)
                {
                    case "Monday":
                        {
                            newDay("Segunda", 0);
                            newDay("Terça", 1);
                            newDay("Quarta", 2);
                            newDay("Quinta", 3);
                            newDay("Sexta", 4);
                            newDay("Sábado", 5);
                            newDay("Domingo", 6);
                            break;
                        }
                    case "Tuesday":
                        {
                            newDay("Terça", 0);
                            newDay("Quarta", 1);
                            newDay("Quinta", 2);
                            newDay("Sexta", 3);
                            newDay("Sábado", 4);
                            newDay("Domingo", 5);
                            break;
                        }
                    case "Wednesday":
                        {
                            newDay("Quarta", 0);
                            newDay("Quinta", 1);
                            newDay("Sexta", 2);
                            newDay("Sábado", 3);
                            newDay("Domingo", 4);
                            break;
                        }
                    case "Thursday":
                        {
                            newDay("Quinta", 0);
                            newDay("Sexta", 1);
                            newDay("Sábado", 2);
                            newDay("Domingo", 3);
                            break;
                        }
                    case "Friday":
                        {
                            newDay("Sexta", 0);
                            newDay("Sábado", 1);
                            newDay("Domingo", 2);
                            break;
                        }
                    case "Saturday":
                        {
                            newDay("Sábado", 0);
                            newDay("Domingo", 1);
                            break;
                        }
                    case "Sunday":
                        {
                            newDay("Domingo", 0);
                            break;
                        }
                }

                // checks how many .xmls were successfully retrieved (at least one need to be for presenting information)
                if (contador < 1)
                    this.hasMenu2 = false;
            }
            catch (Exception e) { Console.WriteLine("Can't read week day!"); }
        }

        /**
         * Present a new day in Days lateral menu
         */
        private void newDay(string day, int linha)
        {
            // New grid row with a day button
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(h * 0.1);
            Days.RowDefinitions.Add(row);

            var button = new YouButton() { };

            if (changed_day == day)
                button.Background = new SolidColorBrush(Colors.LightSteelBlue);
            else
                button.Background = new SolidColorBrush(Colors.White);
            
            button.Width = Days.Width - h * 0.005;
            button.Height = h * 0.09;
            button.BorderBrush = null;
            button.Name = "dia"+day;
            button.Click += Button_Click;
            button.VerticalAlignment = VerticalAlignment.Center;

            TextBlock text = new TextBlock();
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.TextWrapping = TextWrapping.Wrap;
            text.TextTrimming = TextTrimming.CharacterEllipsis;
            text.FontSize = 40;
            text.Foreground = new SolidColorBrush(Colors.Blue);
            text.Text = day;

            button.Content = text;

            Viewbox box = new Viewbox();
            box.Stretch = Stretch.Uniform;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.Width = w * 0.14;

            Days.Children.Add(button);
            Grid.SetRow(button, linha);
        }

        /**
         * Show fetched menu
         */
        private void showMenu()
        {
            // Present canteen name
            Viewbox cant_title = new Viewbox();
            cant_title.HorizontalAlignment = HorizontalAlignment.Center;
            cant_title.VerticalAlignment = VerticalAlignment.Center;
            System.Windows.Media.Effects.DropShadowEffect dropShadowEffect = new System.Windows.Media.Effects.DropShadowEffect();
            cant_title.Effect = dropShadowEffect;
            cant_title.Stretch = Stretch.Uniform;

            TextBlock text_cant_title = new TextBlock();
            text_cant_title.HorizontalAlignment = HorizontalAlignment.Center;
            text_cant_title.VerticalAlignment = VerticalAlignment.Center;
            text_cant_title.TextWrapping = TextWrapping.Wrap;
            text_cant_title.TextTrimming = TextTrimming.CharacterEllipsis;
            text_cant_title.FontSize = 30;
            text_cant_title.Foreground = new SolidColorBrush(Colors.Black);
            text_cant_title.Text = activeCantin.getName();

            cant_title.Child = text_cant_title;
            Canteen_title.Child = cant_title;
            Canteen_title.Width = w * 0.4;
            Canteen_title.Height = h * 0.07;
            Canteen_title.Visibility = Visibility.Visible;
            Canvas.SetTop(Canteen_title, h * 0.08);
            Canvas.SetLeft(Canteen_title, w * 0.29);

            // Present fetched menu if available
            if (this.hasMenu1 && this.hasMenu2)
            {
                // Set scroll Up and Down Buttons
                ScrollUp.Visibility = Visibility.Visible;
                ScrollDown.Visibility = Visibility.Visible;
                YouWindow.setScrollArrows(ScrollUp, ScrollDown, 0.1, 0.65, 0, 0.17, 0.88, 0.17);

                // Set Scroll Panel (data)
                WrapScrollPanel.Width = w * 0.65;
                Canvas.SetTop(WrapScrollPanel, h * 0.1);
                Canvas.SetLeft(WrapScrollPanel, w * 0.17);

                // Set Scroll Viewer (area)
                ScrollViewer.HoverBackground = Brushes.Transparent;
                ScrollViewer.Height = h * 0.725;
                ScrollViewer.Width = w * 0.65;
                Canvas.SetBottom(ScrollViewer, h * 0.12);
                Canvas.SetLeft(ScrollViewer, w * 0.17);

                // Set Ementa table that contains menu information
                ColumnDefinition col_esq = new ColumnDefinition();
                col_esq.Width = new GridLength(w * 0.18);
                EmentaAlmoco.ColumnDefinitions.Add(col_esq);

                ColumnDefinition col_dir = new ColumnDefinition();
                col_dir.Width = new GridLength(w * 0.42);
                EmentaAlmoco.ColumnDefinitions.Add(col_dir);

                Canvas.SetLeft(EmentaAlmoco, 0);
                Canvas.SetTop(EmentaAlmoco, 0);
                EmentaAlmoco.Width = w * 0.65;
                EmentaAlmoco.Visibility = Visibility.Visible;

                Days.Width = w * 0.14;
                Days.Visibility = Visibility.Visible;
                Canvas.SetLeft(Days, w * 0.015);
                Canvas.SetTop(Days, h * 0.27);
            }
            else
            {
                // Hide scroll buttons
                ScrollUp.Visibility = Visibility.Hidden;
                ScrollDown.Visibility = Visibility.Hidden;

                // Present only Days lateral menu
                Canvas.SetLeft(Days, w * 0.015);
                Canvas.SetTop(Days, h * 0.27);

                // Set Scroll Viewer (area)
                ScrollViewer.HoverBackground = Brushes.Transparent;
                ScrollViewer.Height = h * 1.4;
                ScrollViewer.Width = w;
                Canvas.SetBottom(ScrollViewer, h * -0.01);
                Canvas.SetLeft(ScrollViewer, h * 0.2);

                // Present a error message
                BitmapImage bit = new BitmapImage();
                Image imgnoapps = new Image();
                YouWindow.bitmapSource(bit, AppDomain.CurrentDomain.BaseDirectory + "/App/You_MenusUA/usedApp/no_Menu.png", UriKind.Absolute);
                imgnoapps.Stretch = Stretch.Fill;
                imgnoapps.Source = bit;
                imgnoapps.Height = h * 0.8;
                imgnoapps.Width = w * 0.7;
                YouEmentaCanvas.Children.Add(imgnoapps);
                Canvas.SetTop(imgnoapps, h * 0.2);
                Canvas.SetLeft(imgnoapps, w * 0.2);

                Days.Width = w * 0.14;
                Days.Visibility = Visibility.Visible;
                Canvas.SetLeft(Days, w * 0.015);
                Canvas.SetTop(Days, h * 0.27);

                // Reset flags
                this.hasMenu1 = true;
                this.hasMenu2 = true;
            }
        }

        /**
         * Present Lunch menu
         * @return Amount of Grid spent lines in presenting menu
         */
        private int presentLunch(XmlNodeList cantinMenu)
        {
            // Set title ("Lunch")
            RowDefinition titleRow_lunch = new RowDefinition();
            titleRow_lunch.Height = new GridLength(h * 0.07);
            EmentaAlmoco.RowDefinitions.Add(titleRow_lunch);

            Viewbox box_lunch = new Viewbox();
            box_lunch.Stretch = Stretch.Uniform;
            box_lunch.HorizontalAlignment = HorizontalAlignment.Center;
            box_lunch.VerticalAlignment = VerticalAlignment.Center;
            System.Windows.Media.Effects.DropShadowEffect dropShadowEffect = new System.Windows.Media.Effects.DropShadowEffect();
            box_lunch.Effect = dropShadowEffect;

            TextBlock text_lunch = new TextBlock();
            text_lunch.HorizontalAlignment = HorizontalAlignment.Center;
            text_lunch.TextWrapping = TextWrapping.Wrap;
            text_lunch.TextTrimming = TextTrimming.CharacterEllipsis;
            text_lunch.FontSize = 44;
            text_lunch.Foreground = new SolidColorBrush(Colors.Black);
            text_lunch.Text = "Almoço";

            box_lunch.Child = text_lunch;
            EmentaAlmoco.Children.Add(box_lunch);
            Grid.SetColumnSpan(box_lunch, 2);
            Grid.SetRow(box_lunch, 1);

            // Present menu
            int i = 2;
            foreach (XmlElement item in cantinMenu[0].ChildNodes[0].ChildNodes)
            {
                // New row for each plate
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(h * 0.0388);
                EmentaAlmoco.RowDefinitions.Add(row);

                Viewbox box_esq = new Viewbox();
                box_esq.Stretch = Stretch.Uniform;
                box_esq.HorizontalAlignment = HorizontalAlignment.Right;

                TextBlock text_esq = new TextBlock();
                text_esq.HorizontalAlignment = HorizontalAlignment.Center;
                text_esq.TextWrapping = TextWrapping.Wrap;
                text_esq.TextTrimming = TextTrimming.CharacterEllipsis;
                text_esq.FontSize = 50;
                text_esq.Foreground = new SolidColorBrush(Colors.Blue);
                text_esq.Text = item.Attributes["name"].Value + ":   ";

                Viewbox box = new Viewbox();
                box.Stretch = Stretch.Uniform;
                box.HorizontalAlignment = HorizontalAlignment.Left;

                TextBlock text = new TextBlock();
                text.HorizontalAlignment = HorizontalAlignment.Center;
                text.TextWrapping = TextWrapping.Wrap;
                text.TextTrimming = TextTrimming.CharacterEllipsis;
                text.FontSize = 50;
                text.Text = item.InnerText;

                box.Child = text;
                EmentaAlmoco.Children.Add(box);
                Grid.SetColumn(box, 1);
                Grid.SetRow(box, i);

                box_esq.Child = text_esq;
                EmentaAlmoco.Children.Add(box_esq);
                Grid.SetColumn(box_esq, 0);
                Grid.SetRow(box_esq, i);

                i++;
            }
            return i;
        }

        /**
         * Present Dinner menu
         */
        private void presentDinner(XmlNodeList cantinMenu, int i)
        {
            // Set title ("Dinner")
            RowDefinition titleRow_dinner = new RowDefinition();
            titleRow_dinner.Height = new GridLength(h * 0.07);
            EmentaAlmoco.RowDefinitions.Add(titleRow_dinner);

            Viewbox box_dinner = new Viewbox();
            box_dinner.Stretch = Stretch.Uniform;
            box_dinner.HorizontalAlignment = HorizontalAlignment.Center;
            box_dinner.VerticalAlignment = VerticalAlignment.Center;
            System.Windows.Media.Effects.DropShadowEffect dropShadowEffe = new System.Windows.Media.Effects.DropShadowEffect();
            box_dinner.Effect = dropShadowEffe;

            TextBlock text_dinner = new TextBlock();
            text_dinner.HorizontalAlignment = HorizontalAlignment.Center;
            text_dinner.TextWrapping = TextWrapping.Wrap;
            text_dinner.TextTrimming = TextTrimming.CharacterEllipsis;
            text_dinner.FontSize = 44;
            text_dinner.Foreground = new SolidColorBrush(Colors.Black);
            text_dinner.Text = "Jantar";

            box_dinner.Child = text_dinner;
            EmentaAlmoco.Children.Add(box_dinner);
            Grid.SetColumnSpan(box_dinner, 2);
            Grid.SetRow(box_dinner, i);

            i++;
            foreach (XmlElement item in cantinMenu[1].ChildNodes[0].ChildNodes)
            {
                // New row for each plate
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(h * 0.0388);
                EmentaAlmoco.RowDefinitions.Add(row);

                Viewbox box_esq = new Viewbox();
                box_esq.Stretch = Stretch.Uniform;
                box_esq.HorizontalAlignment = HorizontalAlignment.Right;

                TextBlock text_esq = new TextBlock();
                text_esq.HorizontalAlignment = HorizontalAlignment.Center;
                text_esq.TextWrapping = TextWrapping.Wrap;
                text_esq.TextTrimming = TextTrimming.CharacterEllipsis;
                text_esq.FontSize = 50;
                text_esq.Foreground = new SolidColorBrush(Colors.Blue);
                text_esq.Text = item.Attributes["name"].Value + ":   ";

                Viewbox box = new Viewbox();
                box.Stretch = Stretch.Uniform;
                box.HorizontalAlignment = HorizontalAlignment.Left;

                TextBlock text = new TextBlock();
                text.HorizontalAlignment = HorizontalAlignment.Center;
                text.TextWrapping = TextWrapping.Wrap;
                text.TextTrimming = TextTrimming.CharacterEllipsis;
                text.FontSize = 50;
                text.Text = item.InnerText;

                box.Child = text;
                EmentaAlmoco.Children.Add(box);
                Grid.SetColumn(box, 1);
                Grid.SetRow(box, i);

                box_esq.Child = text_esq;
                EmentaAlmoco.Children.Add(box_esq);
                Grid.SetColumn(box_esq, 0);
                Grid.SetRow(box_esq, i);

                i++;
            }
        }

        #region Button Events

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Originate event button
            var b = (YouButton)e.OriginalSource;

            // Back Button
            if (b.Name == "backButton")
            {
                // Clear menus table
                EmentaAlmoco.Children.Clear();
                EmentaAlmoco.RowDefinitions.Clear();
                EmentaAlmoco.Visibility = Visibility.Hidden;

                // Clear Days lateral menu
                Days.Children.Clear();
                Days.RowDefinitions.Clear();
                Days.Visibility = Visibility.Hidden;

                // Clear any error messages
                int c;
                for (c = 7; c < YouEmentaCanvas.Children.Count; c++)
                    YouEmentaCanvas.Children[c].Visibility = Visibility.Hidden;

                // Navigate to MainMenu
                YouNavigation.requestFrameChange(this, "YouMenusUA");
            }

            // Days lateral menu Button
            if (b.Name.Contains("dia"))
            {
                // Clear menus table
                EmentaAlmoco.Children.Clear();
                EmentaAlmoco.RowDefinitions.Clear();
                EmentaAlmoco.Visibility = Visibility.Hidden;

                // Clear Days lateral menu
                Days.Children.Clear();
                Days.RowDefinitions.Clear();
                Days.Visibility = Visibility.Hidden;

                // Clear any error messages
                int c;
                for (c = 6; c < YouEmentaCanvas.Children.Count; c++)
                    YouEmentaCanvas.Children[c].Visibility = Visibility.Hidden;

                // Fetch menu for new selected day
                fetchMenu(b.Name.Substring(3));
                // Show menu of new selected day
                showMenu();
            }
        }

        private void Button_GripEvent(object sender, HandPointerEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;

            if (b.Name == "backButton")
            {
                EmentaAlmoco.Children.Clear();
                EmentaAlmoco.RowDefinitions.Clear();
                EmentaAlmoco.Visibility = Visibility.Hidden;

                Days.Children.Clear();
                Days.RowDefinitions.Clear();
                Days.Visibility = Visibility.Hidden;

                int c;
                for (c = 7; c < YouEmentaCanvas.Children.Count; c++)
                    YouEmentaCanvas.Children[c].Visibility = Visibility.Hidden;

                YouNavigation.requestFrameChange(this, "YouMenusUA");
            }
        }

        #endregion

        #region scroll definitions region

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
            this.PageUpEnabled = ScrollViewer.VerticalOffset > ScrollErrorMargin;
            this.PageDownEnabled = ScrollViewer.VerticalOffset < ScrollViewer.ScrollableHeight - ScrollErrorMargin;
        }
        private void PageUpButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - PixelScrollByAmount);
        }
        private void PageDownButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + PixelScrollByAmount);
        }

        #endregion

        #region App events

        public string getAppName()
        {
            return "You_MenusUA";
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
            return YouEmentaRegion;
        }

        #endregion
    }
}
