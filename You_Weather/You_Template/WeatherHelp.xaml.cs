using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using YouInteract.YouPlugin_Developing;
using You_Weather.WeatherData;

namespace You_Weather
{
    /// <summary>
    ///     Interaction logic for WeatherLocation.xaml
    /// </summary>
    public partial class WeatherHelp : YouPlugin
    {
        public delegate void EventHandler();

        private static readonly double ScreenWidth = SystemParameters.PrimaryScreenWidth;
        /*
                private static readonly double ScreenHeight = SystemParameters.PrimaryScreenHeight;
        */

        public static readonly DependencyProperty ButtonHeightProperty = DependencyProperty.Register("ButtonHeight",
            typeof (float), typeof (WeatherHelp), new PropertyMetadata((float) ScreenWidth*(11f/96)));

        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register("ButtonWidth",
            typeof (float), typeof (WeatherHelp), new PropertyMetadata((float) ScreenWidth*(11f/96)));

        public static readonly DependencyProperty ButtonWithNamesHeightProperty =
            DependencyProperty.Register("ButtonWithNamesHeight", typeof (float), typeof (WeatherHelp),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty ButtonWithNamesWidthProperty =
            DependencyProperty.Register("ButtonWithNamesWidth", typeof (float), typeof (WeatherHelp),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty HomeButtonOffsetProperty =
            DependencyProperty.Register("HomeButtonOffset", typeof (float), typeof (WeatherHelp),
                new PropertyMetadata((float) ScreenWidth*(37f/96)));

        public static readonly DependencyProperty HourlyButtonOffsetProperty =
            DependencyProperty.Register("HourlyButtonOffset", typeof (float), typeof (WeatherHelp),
                new PropertyMetadata((float) ScreenWidth*(1f/2)));

        public static readonly DependencyProperty GridMarginProperty = DependencyProperty.Register("GridMargin",
            typeof (Thickness), typeof (WeatherHelp),
            new PropertyMetadata(new Thickness(ScreenWidth*(5f/96), 0, ScreenWidth*(5f/96), 0)));

        public static readonly DependencyProperty GridWidthProperty = DependencyProperty.Register("GridWidth",
            typeof (float), typeof (WeatherHelp), new PropertyMetadata((float) ScreenWidth*(25f/96)));

        public static readonly DependencyProperty MainBackgroundProperty = DependencyProperty.Register(
            "MainBackground", typeof (ImageSource), typeof (WeatherHelp), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty TextHelpSizeProperty = DependencyProperty.Register("TextHelpSize",
            typeof (float), typeof (WeatherHelp), new PropertyMetadata((float) ScreenWidth*(1f/48)));

        private DispatcherTimer _timer3;

        public WeatherHelp()
        {
            InitializeComponent();
            KinectApi.bindRegion(KinectRegion);

            Loaded += WeatherLocation_Loaded;
            Unloaded += WeatherLocation_Unloaded;
            SetUpGui();
        }

        public float ButtonHeight
        {
            get { return (float) GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        public float ButtonWidth
        {
            get { return (float) GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }

        public float ButtonWithNamesHeight
        {
            get { return (float) GetValue(ButtonWithNamesHeightProperty); }
            set { SetValue(ButtonWithNamesHeightProperty, value); }
        }

        public float ButtonWithNamesWidth
        {
            get { return (float) GetValue(ButtonWithNamesWidthProperty); }
            set { SetValue(ButtonWithNamesWidthProperty, value); }
        }

        public float HomeButtonOffset
        {
            get { return (float) GetValue(HomeButtonOffsetProperty); }
            set { SetValue(HomeButtonOffsetProperty, value); }
        }

        public float HourlyButtonOffset
        {
            get { return (float) GetValue(HourlyButtonOffsetProperty); }
            set { SetValue(HourlyButtonOffsetProperty, value); }
        }

        public Thickness GridMargin
        {
            get { return (Thickness) GetValue(GridMarginProperty); }
            set { SetValue(GridMarginProperty, value); }
        }

        public float GridWidth
        {
            get { return (float) GetValue(GridWidthProperty); }
            set { SetValue(GridWidthProperty, value); }
        }

        public ImageSource MainBackground
        {
            get { return (ImageSource) GetValue(MainBackgroundProperty); }
            set { SetValue(MainBackgroundProperty, value); }
        }

        public float TextHelpSize
        {
            get { return (float) GetValue(TextHelpSizeProperty); }
            set { SetValue(TextHelpSizeProperty, value); }
        }

        private void WeatherLocation_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer3.Stop();

            _timer3.Tick -= _timer3_Tick;
            _timer3 = null;
        }

        private void WeatherLocation_Loaded(object sender, RoutedEventArgs e)
        {
            _timer3 = new DispatcherTimer {IsEnabled = false, Interval = new TimeSpan(0, 0, 5, 0, 0)};
            _timer3.Tick += _timer3_Tick;

            _timer3.Start();
        }

        private void _timer3_Tick(object sender, EventArgs e)
        {
            Weather.From = true;
            YouNavigation.requestFrameChange(this, "YouWeather");
        }

        private void SetUpGui()
        {
            if (WeatherGetter.LUpdateDate.Equals(new DateTime(1, 1, 1)))
            {
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
                return;
            }

            MainBackground = new BitmapImage(new Uri("pack://application:,,," + WeatherGetter.MainBackground));
        }

        public static event EventHandler GuiWeatherHourlyListener = delegate { };

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            YouNavigation.navigateToMainMenu(this);
        }

        private void ButtonHelpOnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonMapOnClick(object sender, RoutedEventArgs e)
        {
            YouNavigation.requestFrameChange(this, "YouWeatherLocation");
        }

        private void ButtonHomeOnClick(object sender, RoutedEventArgs e)
        {
            Weather.From = true;
            YouNavigation.requestFrameChange(this, "YouWeather");
        }

        private void ButtonHourlyOnClick(object sender, RoutedEventArgs e)
        {
            Weather.CurrentToHourlyCorrectInfo = new CurrentToHourlyCorrectInfo();
            GuiWeatherHourlyListener();
            //MainWindow.window.WeatherHourly.SetUpGui();
            YouNavigation.requestFrameChange(this, "YouWeatherHourly");
        }

        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_Weather";
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
            return Name;
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
            return KinectRegion;
        }

        #endregion
    }
}