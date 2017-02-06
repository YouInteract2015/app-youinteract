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
using System.Xml.XPath;
using System.IO;
using System.Security;

namespace You_SGAUA
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>

    /*
     * @author: Tomás Rodrigues 68129
     *          Pedro Abade 59385
     */

    public partial class Senhas : Page, YouPlugin
    {
        // With and Height of the Window
        private double w, h;
        // Auxiliar variable to save selected button
        private string changed_letter;
        // XmlDocument containing Servicos Academicos information
        XmlDocument doc = new XmlDocument();
        // NodeList containing all items available in Servicos Academicos information
        XmlNodeList items;
        // Auxiliar variable to indicate if any information is available
        private bool hasService = true;

        public Senhas()
        {
            // Initialize component
            InitializeComponent();
            // Bind kinect to interactable region
            KinectApi.bindRegion(YouSenhasRegion);
            // Set Window properties
            setWindow();
            // Fetch Servicos Academicos information
            fetchInfo();
            // Show fetched information
            showInfo();
        }

        /**
         * Show Servicos Academicos fetched information
         */
        private void showInfo()
        {
            // if any information available
            if (hasService)
            {
                Services.Visibility = Visibility.Visible;
                Tickets.Visibility = Visibility.Visible;
            }
            // if there are no information available
            else
            {
                // Hide Menu and Info tables
                Services.Visibility = Visibility.Hidden;
                Tickets.Visibility = Visibility.Hidden;

                // Load error image
                BitmapImage bit = new BitmapImage();
                Image imgnoapps = new Image();
                YouWindow.bitmapSource(bit, AppDomain.CurrentDomain.BaseDirectory + "/App/You_SGAUA/usedApp/no_Service.png", UriKind.Absolute);
                imgnoapps.Stretch = Stretch.Fill;
                imgnoapps.Source = bit;
                imgnoapps.Height = h * 0.8;
                imgnoapps.Width = w * 0.7;
                YouSenhasCanvas.Children.Add(imgnoapps);
                Canvas.SetTop(imgnoapps, h * 0.2);
                Canvas.SetLeft(imgnoapps, w * 0.2);

                // Reset auxiliar variable
                this.hasService = true;
            }
        }

        /**
         * Fetch Servicos Academicos information
         */
        private void fetchInfo()
        {
            try
            {
                doc.Load("http://services.web.ua.pt/sac/senhas");
                items = doc.SelectNodes("//item");

                // Verifies if there are any information available
                if (items.Count == 0)
                {
                    hasService = false;
                }
                else
                {
                    Tickets.Visibility = Visibility.Visible;

                    // For each item retrieved create a new button and present some information
                    int aux = 0;
                    int linha = 2;
                    foreach (XmlNode item in items)
                    {
                        // New button
                        newService((item.SelectSingleNode("letter") as XmlElement).InnerText, aux, ((item.SelectSingleNode("letter") as XmlElement).InnerText + " - " + (item.SelectSingleNode("desc") as XmlElement).InnerText));

                        // Create right table with information
                        RowDefinition row = new RowDefinition();
                        row.Height = new GridLength(h * 0.075);
                        Tickets.RowDefinitions.Add(row);

                        TextBlock text_esq = new TextBlock();
                        text_esq.HorizontalAlignment = HorizontalAlignment.Center;
                        text_esq.VerticalAlignment = VerticalAlignment.Center;
                        text_esq.TextWrapping = TextWrapping.Wrap;
                        text_esq.TextTrimming = TextTrimming.CharacterEllipsis;
                        text_esq.FontSize = 30;
                        text_esq.Foreground = new SolidColorBrush(Colors.Black);
                        text_esq.Text = item.SelectSingleNode("latest").InnerText;

                        TextBlock text_dir = new TextBlock();
                        text_dir.HorizontalAlignment = HorizontalAlignment.Center;
                        text_dir.VerticalAlignment = VerticalAlignment.Center;
                        text_dir.TextWrapping = TextWrapping.Wrap;
                        text_dir.TextTrimming = TextTrimming.CharacterEllipsis;
                        text_dir.FontSize = 30;
                        text_dir.Foreground = new SolidColorBrush(Colors.Black);
                        text_dir.Text = item.SelectSingleNode("wc").InnerText;

                        Border bord_esq = new Border();
                        bord_esq.Width = w * 0.22;
                        bord_esq.BorderBrush = null;
                        bord_esq.Height = h * 0.075;
                        bord_esq.Background = null;// new SolidColorBrush(Colors.White);
                        bord_esq.HorizontalAlignment = HorizontalAlignment.Center;
                        bord_esq.VerticalAlignment = VerticalAlignment.Center;

                        Border bord_dir = new Border();
                        bord_dir.Width = w * 0.22;
                        bord_dir.BorderBrush = null;
                        bord_dir.Height = h * 0.075;
                        bord_dir.Background = null;// new SolidColorBrush(Colors.White);
                        bord_dir.HorizontalAlignment = HorizontalAlignment.Center;
                        bord_dir.VerticalAlignment = VerticalAlignment.Center;

                        bord_esq.Child = text_esq;
                        Tickets.Children.Add(bord_esq);
                        Grid.SetRow(bord_esq, linha);
                        Grid.SetColumn(bord_esq, 0);

                        bord_dir.Child = text_dir;
                        Tickets.Children.Add(bord_dir);
                        Grid.SetRow(bord_dir, linha);
                        Grid.SetColumn(bord_dir, 1);

                        linha++;

                        RowDefinition blank_row = new RowDefinition();
                        blank_row.Height = new GridLength(h * 0.022);
                        Tickets.RowDefinitions.Add(blank_row);

                        linha++;
                        aux++;
                    }
                }
            }
            catch (Exception e) { hasService = false; }
        }

        /**
         * Create a new button in left menu
         */
        private void newService(string letter, int linha, string texto)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength( h * 0.097);
            Services.RowDefinitions.Add(row);

            var button = new YouButton();
            
            if (changed_letter == letter)
                button.Background = new SolidColorBrush(Colors.LightSteelBlue);
            else
                button.Background = new SolidColorBrush(Colors.LemonChiffon);

            button.Width = Services.Width - (w * 0.005);
            button.Height = h * 0.09;
            button.BorderBrush = null;
            button.Name = "letra" + letter;
            button.Click += Button_Click;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.Visibility = Visibility.Visible;

            TextBlock text = new TextBlock();
            text.HorizontalAlignment = HorizontalAlignment.Left;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.TextWrapping = TextWrapping.Wrap;
            text.TextTrimming = TextTrimming.CharacterEllipsis;
            text.FontSize = 30;
            text.Foreground = new SolidColorBrush(Colors.Black);
            text.Text = texto;

            button.Content = text;

            Viewbox box = new Viewbox();
            box.Stretch = Stretch.Uniform;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.Width = w * 0.5;

            Services.Children.Add(button);
            Grid.SetRow(button, linha);
        }

        /**
         * Set Window properties
         */
        public void setWindow()
        {
            // Get Window Measures
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();

            // Set Title
            setTitle();

            // Set tables measures
            Services.Width = w * 0.5;
            Tickets.Width = w * 0.45;
            selected.Width = w * 0.4;
            selected.Height = h * 0.3;
            Canvas.SetLeft(Services, 0);
            Canvas.SetTop(Services, h * 0.28);
            Canvas.SetLeft(Tickets, w * 0.5);
            Canvas.SetTop(Tickets, h * 0.208);
            Canvas.SetTop(selected, h * 0.47);
            Canvas.SetLeft(selected, w * 0.5);

            // Set Back Button
            MainMenuButton.Width = w * 0.11;
            MainMenuButton.Height = h * 0.22;
            Canvas.SetTop(MainMenuButton, h * 0.01);
            Canvas.SetLeft(MainMenuButton, w * 0.01);

            // Create right table that is visible only when selecting a button
            ColumnDefinition col_inicial_esq = new ColumnDefinition();
            col_inicial_esq.Width = new GridLength(w * 0.2);
            selected.ColumnDefinitions.Add(col_inicial_esq);

            ColumnDefinition col_inicial_dir = new ColumnDefinition();
            col_inicial_dir.Width = new GridLength(w * 0.2);
            selected.ColumnDefinitions.Add(col_inicial_dir);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(h * 0.075);
            selected.RowDefinitions.Add(row1);

            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(h * 0.075);
            selected.RowDefinitions.Add(row2);

            RowDefinition row3 = new RowDefinition();
            row3.Height = new GridLength(h * 0.075);
            selected.RowDefinitions.Add(row3);

            RowDefinition row4 = new RowDefinition();
            row4.Height = new GridLength(h * 0.075);
            selected.RowDefinitions.Add(row4);

            TextBlock text_1 = new TextBlock();
            text_1.HorizontalAlignment = HorizontalAlignment.Right;
            text_1.VerticalAlignment = VerticalAlignment.Center;
            text_1.TextWrapping = TextWrapping.Wrap;
            text_1.TextTrimming = TextTrimming.CharacterEllipsis;
            text_1.FontSize = 20;
            text_1.Foreground = new SolidColorBrush(Colors.Black);
            text_1.Text = "Nº Senha atual:   ";

            TextBlock text_2 = new TextBlock();
            text_2.HorizontalAlignment = HorizontalAlignment.Right;
            text_2.VerticalAlignment = VerticalAlignment.Center;
            text_2.TextWrapping = TextWrapping.Wrap;
            text_2.TextTrimming = TextTrimming.CharacterEllipsis;
            text_2.FontSize = 20;
            text_2.Foreground = new SolidColorBrush(Colors.Black);
            text_2.Text = "Pessoas em espera:   ";

            TextBlock text_3 = new TextBlock();
            text_3.HorizontalAlignment = HorizontalAlignment.Right;
            text_3.VerticalAlignment = VerticalAlignment.Center;
            text_3.TextWrapping = TextWrapping.Wrap;
            text_3.TextTrimming = TextTrimming.CharacterEllipsis;
            text_3.FontSize = 20;
            text_3.Foreground = new SolidColorBrush(Colors.Black);
            text_3.Text = "Tempo médio de espera:   ";

            TextBlock text_4 = new TextBlock();
            text_4.HorizontalAlignment = HorizontalAlignment.Right;
            text_4.VerticalAlignment = VerticalAlignment.Center;
            text_4.TextWrapping = TextWrapping.Wrap;
            text_4.TextTrimming = TextTrimming.CharacterEllipsis;
            text_4.FontSize = 20;
            text_4.Foreground = new SolidColorBrush(Colors.Black);
            text_4.Text = "Atualização:   ";

            selected.Children.Add(text_1);
            selected.Children.Add(text_2);
            selected.Children.Add(text_3);
            selected.Children.Add(text_4);
            
            Grid.SetRow(text_1, 0);
            Grid.SetColumn(text_1, 0);
            Grid.SetRow(text_2, 1);
            Grid.SetColumn(text_2, 0);
            Grid.SetRow(text_3, 2);
            Grid.SetColumn(text_3, 0);
            Grid.SetRow(text_4, 3);
            Grid.SetColumn(text_4, 0);

            // Create right table that is visible when user dont select any button
            RowDefinition titles = new RowDefinition();
            titles.Height = new GridLength(h * 0.075);
            Tickets.RowDefinitions.Add(titles);

            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(w * 0.22);
            Tickets.ColumnDefinitions.Add(col);

            ColumnDefinition col_dir = new ColumnDefinition();
            col_dir.Width = new GridLength(w * 0.22);
            Tickets.ColumnDefinitions.Add(col_dir);

            Border bord_esq = new Border();
            bord_esq.Width = w * 0.22;
            bord_esq.BorderBrush = null;
            bord_esq.Height = h * 0.075;
            bord_esq.Background = new SolidColorBrush(Colors.White);

            Border bord_dir = new Border();
            bord_dir.Width = w * 0.22;
            bord_dir.BorderBrush = null;
            bord_dir.Height = h * 0.075;
            bord_dir.Background = new SolidColorBrush(Colors.White);

            TextBlock text_dir = new TextBlock();
            text_dir.HorizontalAlignment = HorizontalAlignment.Center;
            text_dir.VerticalAlignment = VerticalAlignment.Center;
            text_dir.TextWrapping = TextWrapping.Wrap;
            text_dir.TextTrimming = TextTrimming.CharacterEllipsis;
            text_dir.FontSize = 30;
            text_dir.Foreground = new SolidColorBrush(Colors.Black);
            text_dir.Text = "Pessoas em espera";

            TextBlock text_esq = new TextBlock();
            text_esq.HorizontalAlignment = HorizontalAlignment.Center;
            text_esq.VerticalAlignment = VerticalAlignment.Center;
            text_esq.TextWrapping = TextWrapping.Wrap;
            text_esq.TextTrimming = TextTrimming.CharacterEllipsis;
            text_esq.FontSize = 30;
            text_esq.Foreground = new SolidColorBrush(Colors.Black);
            text_esq.Text = "Nº Senha Atual";

            bord_esq.Child = text_esq;

            Tickets.Children.Add(bord_esq);
            Grid.SetRow(bord_esq, 0);
            Grid.SetColumn(bord_esq, 0);

            bord_dir.Child = text_dir;

            Tickets.Children.Add(bord_dir);
            Grid.SetRow(bord_dir, 0);
            Grid.SetColumn(bord_dir, 1);

            RowDefinition blank_row = new RowDefinition();
            blank_row.Height = new GridLength(h * 0.008);
            Tickets.RowDefinitions.Add(blank_row);
        }

        /**
         * Set App title
         */
        private void setTitle()
        {
            BitmapImage bitmapT = new BitmapImage();
            Image imgT = new Image();
            bitmapT.BeginInit();
            bitmapT.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/App/You_SGAUA/usedApp/YOU_SGAUA.png", UriKind.Absolute);
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

        /**
         * Fetch information and put it on left table
         */
        private void showServiceInfo(XmlNode item)
        {
            TextBlock text_s = new TextBlock();
            text_s.HorizontalAlignment = HorizontalAlignment.Left;
            text_s.VerticalAlignment = VerticalAlignment.Center;
            text_s.TextWrapping = TextWrapping.Wrap;
            text_s.TextTrimming = TextTrimming.CharacterEllipsis;
            text_s.FontSize = 20;
            text_s.Foreground = new SolidColorBrush(Colors.Blue);
            text_s.Text = item.SelectSingleNode("latest").InnerText;

            TextBlock text_e = new TextBlock();
            text_e.HorizontalAlignment = HorizontalAlignment.Left;
            text_e.VerticalAlignment = VerticalAlignment.Center;
            text_e.TextWrapping = TextWrapping.Wrap;
            text_e.TextTrimming = TextTrimming.CharacterEllipsis;
            text_e.FontSize = 20;
            text_e.Foreground = new SolidColorBrush(Colors.Blue);
            text_e.Text = item.SelectSingleNode("wc").InnerText;

            TextBlock text_me = new TextBlock();
            text_me.HorizontalAlignment = HorizontalAlignment.Left;
            text_me.VerticalAlignment = VerticalAlignment.Center;
            text_me.TextWrapping = TextWrapping.Wrap;
            text_me.TextTrimming = TextTrimming.CharacterEllipsis;
            text_me.FontSize = 20;
            text_me.Foreground = new SolidColorBrush(Colors.Blue);

            int segundos = Convert.ToInt32(item.SelectSingleNode("awt").InnerText);
            text_me.Text = string.Format("{0:00}:{1:00}:{2:00}", segundos / 3600, (segundos / 60) % 60, segundos % 60); ;

            TextBlock text_a = new TextBlock();
            text_a.HorizontalAlignment = HorizontalAlignment.Left;
            text_a.VerticalAlignment = VerticalAlignment.Center;
            text_a.TextWrapping = TextWrapping.Wrap;
            text_a.TextTrimming = TextTrimming.CharacterEllipsis;
            text_a.FontSize = 20;
            text_a.Foreground = new SolidColorBrush(Colors.Blue);
            text_a.Text = item.SelectSingleNode("date").InnerText;

            selected.Children.Add(text_a);
            selected.Children.Add(text_e);
            selected.Children.Add(text_me);
            selected.Children.Add(text_s);

            Grid.SetRow(text_s, 0);
            Grid.SetColumn(text_s, 1);
            Grid.SetRow(text_e, 1);
            Grid.SetColumn(text_e, 1);
            Grid.SetRow(text_me, 2);
            Grid.SetColumn(text_me, 1);
            Grid.SetRow(text_a, 3);
            Grid.SetColumn(text_a, 1);
        }

        /**
         * ButtonClick events
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;

            if (b.Name == "MainMenuButton")
            {
                changed_letter = "";
                Services.Children.Clear();

                setWindow();
                fetchInfo();

                Services.Visibility = Visibility.Visible;
                selected.Visibility = Visibility.Hidden;

                YouNavigation.navigateToMainMenu(this);
            }
            else if(b.Name.Contains("letra")){

                changed_letter = b.Name.Substring(5);

                selected.Children.Clear();
                Tickets.Children.Clear();

                setWindow();
                fetchInfo();

                Tickets.Visibility = Visibility.Hidden;
                selected.Visibility = Visibility.Visible;

                switch(b.Name){
                    case "letraA":
                        foreach (XmlNode item in items)
                        {
                            if((item.SelectSingleNode("letter") as XmlElement).InnerText.Equals("A")){
                                showServiceInfo(item);
                            }
                        }
                    break;

                    case "letraD":
                        foreach (XmlNode item in items)
                        {
                            if ((item.SelectSingleNode("letter") as XmlElement).InnerText.Equals("D"))
                            {
                                showServiceInfo(item);
                            }
                        }
                    break;

                    case "letraH":
                        foreach (XmlNode item in items)
                        {
                            if ((item.SelectSingleNode("letter") as XmlElement).InnerText.Equals("H"))
                            {
                                showServiceInfo(item);
                            }
                        }
                    break;

                    case "letraG":
                        foreach (XmlNode item in items)
                        {

                            if ((item.SelectSingleNode("letter") as XmlElement).InnerText.Equals("G"))
                            {
                                showServiceInfo(item);
                            }
                        }
                    break;

                    case "letraC":
                        foreach (XmlNode item in items)
                        {
                            if ((item.SelectSingleNode("letter") as XmlElement).InnerText.Equals("C"))
                            {
                                showServiceInfo(item);
                            }
                        }
                    break;

                    case "letraV":
                        foreach (XmlNode item in items)
                        {
                            if ((item.SelectSingleNode("letter") as XmlElement).InnerText.Equals("V"))
                            {
                                showServiceInfo(item);
                            }
                        }
                    break;

                    case "letraU":
                        foreach (XmlNode item in items)
                        {
                            if ((item.SelectSingleNode("letter") as XmlElement).InnerText.Equals("U"))
                            {
                                showServiceInfo(item);
                            }
                        }
                        break;
                 }
            }
        }

        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_SGAUA";
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
            return YouSenhasRegion;
        }

        #endregion

    }
}
