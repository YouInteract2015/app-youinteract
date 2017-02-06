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
using YouInteractV1.Themes;


namespace YouInteractV1
{
    /// <summary>
    /// Interaction logic for YouMenu.xaml
    /// </summary>
    public partial class YouMenu : Page, YouPlugin
    {
        public static readonly DependencyProperty PageLeftEnabledProperty = DependencyProperty.Register(
            "PageLeftEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public static readonly DependencyProperty PageRightEnabledProperty = DependencyProperty.Register(
            "PageRightEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        private const double ScrollErrorMargin = 0.001;
        private const int PixelScrollByAmount = 15;
        private double w, h;

        private string[] visibleApps;
        private int countApps;
        private Boolean appRows;
        private Boolean firstMargin;

        public YouMenu()
        {
            InitializeComponent();
            
            ThemeManager.StoreCurrentActiveTheme();
            KinectApi.bindRegion(YouKinectRegion);
            MainBackGrImageBrush.ImageSource = new BitmapImage(new Uri(ThemeManager.GetThemeOrDefaultPath("/Background/background.jpg"), UriKind.Absolute));
            visibleApps = YouInteractV1.LoaderData.ManageStructs.GetActiveApps().ToArray();

            firstMargin = true;
            countApps = 10;
            setAppRows(countApps);

            setWindow();
        }

        private void setWindow()
        {
            // Get Window Measures
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();

            // Set Logo
            YouWindow.initLogo(Logo, 0.29, 0.38);

            // Set User Viewer
            YouWindow.setUserViewer(Viewer, 0.3, 0.4, 0, 0);

            // Set Scroll Arrows
            YouWindow.setScrollArrows(ScrollLeft, ScrollRight, 0.24, 0.05, 0.2, 0.01, 0.2, 0.93);
            setScroll();

            // Portal sem aplicações selecionadas
            if (countApps == 0)
            {
                noApplications();
            }
                // Percorrer as aplicações visiveis fornecidas pelo portal
            else
            {
                int i;
                int count = 0;
                Console.WriteLine("Number of applications: " + visibleApps.Length);
                for (i = 0; i < visibleApps.Length; i++)
                {
                    if (count == 2)
                    {
                        firstMargin = false;
                    }
                    if (visibleApps[i] == "You_Tutorial")
                    {
                        BitmapImage youBitMap = new BitmapImage();
                        Image youImage = new Image();
                        Console.WriteLine(ThemeManager.GetThemeOrDefaultPath("/Apps/" + visibleApps[i] + ".png"));
                        YouWindow.bitmapSource(youBitMap, ThemeManager.GetThemeOrDefaultPath("/Apps/" + visibleApps[i] + ".png"), UriKind.Absolute);
                        var button = new YouButton { };
                        button.Width = w * 0.12;
                        button.Height = h * 0.22;
                        button.Label = visibleApps[i];
                        button.Name = visibleApps[i];
                        button.BorderBrush = null;
                        button.Foreground = null;
                        button.LabelBackground = null;
                        button.Click += new RoutedEventHandler(Button_Click);
                        button.EnterEvent += new onHandEnterHandler(Button_Hover_Event);
                        button.LeaveEvent += new onhandLeaveHandler(Button_Leave_Event);
                        youImage.Stretch = Stretch.Fill;
                        youImage.Source = youBitMap;
                        button.Content = youImage;
                        button.Background = new ImageBrush(youBitMap);
                        YouMenuCanvas.Children.Add(button);
                        Canvas.SetTop(button, h * 0.68);
                        Canvas.SetLeft(button, w * 0.05);
                    }
                    else
                    {
                        // Inicializar cada aplicação
                        BitmapImage youBitMap = new BitmapImage();
                        Image youImage = new Image();
                        Console.WriteLine(ThemeManager.GetThemeOrDefaultPath("/Apps/" + visibleApps[i] + ".png"));
                        YouWindow.bitmapSource(youBitMap, ThemeManager.GetThemeOrDefaultPath("/Apps/" + visibleApps[i] + ".png"), UriKind.Absolute);
                        var button = initButton(visibleApps[i], youImage, youBitMap);
                        this.WrapScrollPanel.Children.Add(button);
                        count++;
                    }
                }
            }
        }

        private YouButton initButton(string app, Image img, BitmapImage bitmap)
        {
            var button = new YouButton { };
            button.Width = w * 0.14;
            button.Height = h * 0.25;
            button.Label = app;
            button.Name = app;
            button.BorderBrush = null;
            button.Foreground = null;
            button.LabelBackground = null;
            button.Click += new RoutedEventHandler(Button_Click);
            button.EnterEvent += new onHandEnterHandler(Button_Hover_Event);
            button.LeaveEvent += new onhandLeaveHandler(Button_Leave_Event);
            img.Stretch = Stretch.Fill;
            img.Source = bitmap;
            button.Content = img;
            button.Background = new ImageBrush(bitmap);
            if (!firstMargin)
            {
                button.Margin = new Thickness(w * 0.03, h * 0.025, w * 0.03, h * 0.025);
            }
            else
            {
                button.Margin = new Thickness(0, h * 0.025, w * 0.03, h * 0.025);
            }
            return button;
        }

        private void noApplications()
        {
            BitmapImage bit = new BitmapImage();
            Image imgnoapps = new Image();
            YouWindow.bitmapSource(bit, ThemeManager.GetThemeOrDefaultPath("/Apps/sad.png"), UriKind.Absolute);
            imgnoapps.Stretch = Stretch.Fill;
            imgnoapps.Source = bit;
            imgnoapps.Height = h * 0.3;
            imgnoapps.Width = w * 0.3;
            YouMenuCanvas.Children.Add(imgnoapps);
            Canvas.SetTop(imgnoapps, h * 0.2);
            Canvas.SetLeft(imgnoapps, w / 2 - imgnoapps.Width / 2);

            PanelNoapps.Width = w * 0.3;
            PanelNoapps.Height = h * 0.3;
            Noapps.Text = "No apps. Contact an Admin";
            Canvas.SetTop(PanelNoapps, h * 0.2);
            Canvas.SetRight(PanelNoapps, w * 0.025);

            PanelError.Width = w * 0.3;
            PanelError.Height = h * 0.3;
            Error.Text = "There was an error";
            Canvas.SetTop(PanelError, h * 0.2);
            Canvas.SetLeft(PanelError, w * 0.025);
        }

        private void alteraPath(String a)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(a, UriKind.Relative);
            bi3.EndInit();
            Logo.Stretch = Stretch.Fill;
            Logo.Source = bi3;
        }

        #region Scroll Configurations

        private void setScroll()
        {
            if (appRows)
            {
                // Scroll Panel
                WrapScrollPanel.Height = h * 0.63;
                Canvas.SetTop(WrapScrollPanel, h * 0.02);
                // scrollViewer
                ScrollViewer.Height = h * 0.63;
                Canvas.SetTop(ScrollViewer, h * 0.02);
            }
            else
            {
                // Scroll Panel
                WrapScrollPanel.Height = h * 0.31;
                Canvas.SetTop(WrapScrollPanel, h * 0.15);
                // scrollViewer
                ScrollViewer.Height = h * 0.31;
                Canvas.SetTop(ScrollViewer, h * 0.15);
            }
            ScrollViewer.Width = w;
            ScrollViewer.HoverBackground = Brushes.Transparent;
            Canvas.SetLeft(WrapScrollPanel, 0);
            Canvas.SetLeft(ScrollViewer, 0);
        }

        private void setAppRows(int countMyApps)
        {
            if (countMyApps > 8)
            {
                appRows = true;
            }
            else
            {
                appRows = false;
            }
        }
        #endregion

        #region YourPlugin Interface Methods
        public string getAppName()
        {
            return "YouMainMenu";
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
            return this.YouKinectRegion;
        }
        public bool getIsFirstPage()
        {
            return true;
        }
        #endregion

        #region YouButtonEventHandlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            MainWindow.Reference.MainMenuChange(b.Name);
        }

        private void Button_Leave_Event(object sender, HandPointerEventArgs e)
        {
            alteraPath("/YouInteractV1;component/Images/Logo/tvlogo.png");
        }

        private void Button_Hover_Event(object sender, HandPointerEventArgs e)
        {
            var b = (YouButton) e.OriginalSource;
            alteraPath("/YouInteractV1;component/Images/Logo/" + b.Label.ToString() + ".png");
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

    }
}
