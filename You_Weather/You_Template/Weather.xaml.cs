using System;
using System.Globalization;
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
    ///     Interaction logic for Template.xaml
    /// </summary>
    public partial class Weather : YouPlugin
    {
        public delegate void EventHandler();

        private static readonly double ScreenWidth = SystemParameters.PrimaryScreenWidth;
/*
        private static readonly double ScreenHeight = SystemParameters.PrimaryScreenHeight;
*/

        public static readonly DependencyProperty ButtonHeightProperty = DependencyProperty.Register("ButtonHeight",
            typeof (float), typeof (Weather), new PropertyMetadata((float) ScreenWidth*(11f/96)));

        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register("ButtonWidth",
            typeof (float), typeof (Weather), new PropertyMetadata((float) ScreenWidth*(11f/96)));

        public static readonly DependencyProperty ButtonWithNamesHeightProperty =
            DependencyProperty.Register("ButtonWithNamesHeight", typeof (float), typeof (Weather),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty ButtonWithNamesWidthProperty =
            DependencyProperty.Register("ButtonWithNamesWidth", typeof (float), typeof (Weather),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty HomeButtonOffsetProperty =
            DependencyProperty.Register("HomeButtonOffset", typeof (float), typeof (Weather),
                new PropertyMetadata((float) ScreenWidth*(37f/96)));

        public static readonly DependencyProperty HourlyButtonOffsetProperty =
            DependencyProperty.Register("HourlyButtonOffset", typeof (float), typeof (Weather),
                new PropertyMetadata((float) ScreenWidth*(1f/2)));

        public static readonly DependencyProperty LocationFontSizeProperty =
            DependencyProperty.Register("LocationFontSize", typeof (float), typeof (Weather),
                new PropertyMetadata((float) ScreenWidth*(1f/32)));

        public static readonly DependencyProperty ServiceFontSizeProperty =
            DependencyProperty.Register("ServiceFontSize", typeof (float), typeof (Weather),
                new PropertyMetadata(((float) ScreenWidth*(5f/192))/2));

        public static readonly DependencyProperty LocationCityProperty = DependencyProperty.Register("LocationCity",
            typeof (string), typeof (Weather), new PropertyMetadata(""));

        public static readonly DependencyProperty LocationCountryProperty =
            DependencyProperty.Register("LocationCountry", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty LUpdateDateProperty = DependencyProperty.Register("LUpdateDate",
            typeof (DateTime), typeof (Weather), new PropertyMetadata(new DateTime(1, 1, 1)));

        public static readonly DependencyProperty ServiceProperty = DependencyProperty.Register("Service",
            typeof (string), typeof (Weather), new PropertyMetadata(""));

        public static readonly DependencyProperty GridMarginProperty = DependencyProperty.Register("GridMargin",
            typeof (Thickness), typeof (Weather),
            new PropertyMetadata(new Thickness((float) ScreenWidth*(5f/96), 0, (float) ScreenWidth*(5f/96), 0)));

        public static readonly DependencyProperty GridWidthProperty = DependencyProperty.Register("GridWidth",
            typeof (float), typeof (Weather), new PropertyMetadata((float) ScreenWidth*(25f/96)));

        public static readonly DependencyProperty WindowBigHeightProperty =
            DependencyProperty.Register("WindowBigHeight", typeof (float), typeof (Weather),
                new PropertyMetadata((float) ScreenWidth*(67f/192)));

        public static readonly DependencyProperty WindowBigWidthProperty = DependencyProperty.Register(
            "WindowBigWidth", typeof (float), typeof (Weather), new PropertyMetadata((float) ScreenWidth*(43f/192)));

        public static readonly DependencyProperty BigMarginProperty = DependencyProperty.Register("BigMargin",
            typeof (Thickness), typeof (Weather), new PropertyMetadata(new Thickness(0, 0, 0, ScreenWidth*(5f/54))));

        public static readonly DependencyProperty BigCurrentTemperatureUnitsProperty =
            DependencyProperty.Register("BigCurrentTemperatureUnits", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BigCurrentTemperatureProperty =
            DependencyProperty.Register("BigCurrentTemperature", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BigCurrentWeatherTextProperty =
            DependencyProperty.Register("BigCurrentWeatherText", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BigCurrentHumidityProperty =
            DependencyProperty.Register("BigCurrentHumidity", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BigCurrentHumidityUnitsProperty =
            DependencyProperty.Register("BigCurrentHumidityUnits", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BigCurrentPressureProperty =
            DependencyProperty.Register("BigCurrentPressure", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BigCurrentPressureUnitsProperty =
            DependencyProperty.Register("BigCurrentPressureUnits", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BigCurrentWindSpeedTextProperty =
            DependencyProperty.Register("BigCurrentWindSpeedText", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BigCurrentWindDirectionTextProperty =
            DependencyProperty.Register("BigCurrentWindDirectionText", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BigCurrentCloudsTextProperty =
            DependencyProperty.Register("BigCurrentCloudsText", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BigCurrentPrecipitationTextProperty =
            DependencyProperty.Register("BigCurrentPrecipitationText", typeof (string), typeof (Weather),
                new PropertyMetadata(""));

        public static string Units;

        public static readonly DependencyProperty MainBackgroundProperty = DependencyProperty.Register(
            "MainBackground", typeof (ImageSource), typeof (Weather), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty LUpdateForegroundProperty =
            DependencyProperty.Register("LUpdateForeground", typeof (Brush), typeof (Weather),
                new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty LUpdateWeightProperty = DependencyProperty.Register("LUpdateWeight",
            typeof (FontWeight), typeof (Weather), new PropertyMetadata(FontWeights.Normal));

        public static readonly DependencyProperty LErrorTextProperty = DependencyProperty.Register("LErrorText",
            typeof (string), typeof (Weather), new PropertyMetadata(""));

        public static readonly DependencyProperty LErrorForegroundProperty =
            DependencyProperty.Register("LErrorForeground", typeof (Brush), typeof (Weather),
                new PropertyMetadata(Brushes.White));

        //private KinectSensorChooser sensor;
        //public void KeepSensor(KinectSensorChooser _sensor)
        //{
        //    sensor = _sensor;
        //    sensorChooserUi.KinectSensorChooser = sensor;
        //    var regionSensorBinding = new Binding("Kinect") { Source = sensor };
        //    BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
        //    sensor.Start();
        //}

        public static bool From = false;

        public static readonly DependencyProperty HoverButton1MarginProperty =
            DependencyProperty.Register("HoverButton1Margin", typeof (Thickness), typeof (Weather),
                new PropertyMetadata(new Thickness((float) ScreenWidth*(5f/128), 0, (float) ScreenWidth*(1f/384), 0)));

        public static readonly DependencyProperty HoverButtonSizeProperty =
            DependencyProperty.Register("HoverButtonSize", typeof (float), typeof (Weather),
                new PropertyMetadata((float) ScreenWidth*(5f/64)));

        public static readonly DependencyProperty HoverButton2MarginProperty =
            DependencyProperty.Register("HoverButton2Margin", typeof (Thickness), typeof (Weather),
                new PropertyMetadata(new Thickness((float) ScreenWidth*(1f/384), 0, (float) ScreenWidth*(1f/384), 0)));

        public static readonly DependencyProperty DownElementsMargin1Property =
            DependencyProperty.Register("DownElementsMargin1", typeof (Thickness), typeof (Weather),
                new PropertyMetadata(new Thickness((float) ScreenWidth*(7f/192), 0, 0, 0)));

        public static readonly DependencyProperty DownElementsMargin2Property =
            DependencyProperty.Register("DownElementsMargin2", typeof (Thickness), typeof (Weather),
                new PropertyMetadata(new Thickness((float) ScreenWidth*(-7f/192), 0, 0, (float) ScreenWidth*(83f/1920))));

        public static readonly DependencyProperty DownElementsMargin3Property =
            DependencyProperty.Register("DownElementsMargin3", typeof (Thickness), typeof (Weather),
                new PropertyMetadata(new Thickness(0, 0, (float) ScreenWidth*(1f/128), 0)));

        public static readonly DependencyProperty WindowBigMarginProperty =
            DependencyProperty.Register("WindowBigMargin", typeof (Thickness), typeof (Weather),
                new PropertyMetadata(new Thickness(0, 0, 0, (float) ScreenWidth*(5f/96))));

        public static CurrentToHourlyCorrectInfo CurrentToHourlyCorrectInfo = new CurrentToHourlyCorrectInfo();

        private DispatcherTimer _timer;
        private DispatcherTimer _timer2;
        private DispatcherTimer _timer3;

        public Weather()
        {
            InitializeComponent();
            //if (!Directory.Exists(Directory.GetCurrentDirectory() + "/WeatherData"))
            //    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/WeatherData");
            //if (!File.Exists(Directory.GetCurrentDirectory() + "/WeatherData/GoogleMaps.html"))
            //    File.Create(Directory.GetCurrentDirectory() + "/WeatherData/GoogleMaps.html");

            KinectApi.bindRegion(KinectRegion);
            WeatherLocation.GuiWeatherListener += WeatherLocation_GuiWeatherListener;
            Loaded += Weather_Loaded;
            Unloaded += Weather_Unloaded;
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

        public float LocationFontSize
        {
            get { return (float) GetValue(LocationFontSizeProperty); }
            set { SetValue(LocationFontSizeProperty, value); }
        }

        public float ServiceFontSize
        {
            get { return (float) GetValue(ServiceFontSizeProperty); }
            set { SetValue(ServiceFontSizeProperty, value); }
        }

        public string LocationCity
        {
            get { return (string) GetValue(LocationCityProperty); }
            set { SetValue(LocationCityProperty, value); }
        }

        public string LocationCountry
        {
            get { return (string) GetValue(LocationCountryProperty); }
            set { SetValue(LocationCountryProperty, value); }
        }

        public DateTime LUpdateDate
        {
            get { return (DateTime) GetValue(LUpdateDateProperty); }
            set { SetValue(LUpdateDateProperty, value); }
        }

        public string Service
        {
            get { return (string) GetValue(ServiceProperty); }
            set { SetValue(ServiceProperty, value); }
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

        public float WindowBigHeight
        {
            get { return (float) GetValue(WindowBigHeightProperty); }
            set { SetValue(WindowBigHeightProperty, value); }
        }

        public float WindowBigWidth
        {
            get { return (float) GetValue(WindowBigWidthProperty); }
            set { SetValue(WindowBigWidthProperty, value); }
        }

        public string BigMargin
        {
            get { return (string) GetValue(BigMarginProperty); }
            set { SetValue(BigMarginProperty, value); }
        }

        public string BigCurrentTemperatureUnits
        {
            get { return (string) GetValue(BigCurrentTemperatureUnitsProperty); }
            set { SetValue(BigCurrentTemperatureUnitsProperty, value); }
        }

        public string BigCurrentTemperature
        {
            get { return (string) GetValue(BigCurrentTemperatureProperty); }
            set { SetValue(BigCurrentTemperatureProperty, value); }
        }

        public string BigCurrentWeatherText
        {
            get { return (string) GetValue(BigCurrentWeatherTextProperty); }
            set { SetValue(BigCurrentWeatherTextProperty, value); }
        }

        public string BigCurrentHumidity
        {
            get { return (string) GetValue(BigCurrentHumidityProperty); }
            set { SetValue(BigCurrentHumidityProperty, value); }
        }

        public string BigCurrentHumidityUnits
        {
            get { return (string) GetValue(BigCurrentHumidityUnitsProperty); }
            set { SetValue(BigCurrentHumidityUnitsProperty, value); }
        }

        public string BigCurrentPressure
        {
            get { return (string) GetValue(BigCurrentPressureProperty); }
            set { SetValue(BigCurrentPressureProperty, value); }
        }

        public string BigCurrentPressureUnits
        {
            get { return (string) GetValue(BigCurrentPressureUnitsProperty); }
            set { SetValue(BigCurrentPressureUnitsProperty, value); }
        }

        public string BigCurrentWindSpeedText
        {
            get { return (string) GetValue(BigCurrentWindSpeedTextProperty); }
            set { SetValue(BigCurrentWindSpeedTextProperty, value); }
        }

        public string BigCurrentWindDirectionText
        {
            get { return (string) GetValue(BigCurrentWindDirectionTextProperty); }
            set { SetValue(BigCurrentWindDirectionTextProperty, value); }
        }

        public string BigCurrentCloudsText
        {
            get { return (string) GetValue(BigCurrentCloudsTextProperty); }
            set { SetValue(BigCurrentCloudsTextProperty, value); }
        }

        public string BigCurrentPrecipitationText
        {
            get { return (string) GetValue(BigCurrentPrecipitationTextProperty); }
            set { SetValue(BigCurrentPrecipitationTextProperty, value); }
        }

        public ImageSource MainBackground
        {
            get { return (ImageSource) GetValue(MainBackgroundProperty); }
            set { SetValue(MainBackgroundProperty, value); }
        }

        public Brush LUpdateForeground
        {
            get { return (Brush) GetValue(LUpdateForegroundProperty); }
            set { SetValue(LUpdateForegroundProperty, value); }
        }

        public FontWeight LUpdateWeight
        {
            get { return (FontWeight) GetValue(LUpdateWeightProperty); }
            set { SetValue(LUpdateWeightProperty, value); }
        }

        public string LErrorText
        {
            get { return (string) GetValue(LErrorTextProperty); }
            set { SetValue(LErrorTextProperty, value); }
        }

        public Brush LErrorForeground
        {
            get { return (Brush) GetValue(LErrorForegroundProperty); }
            set { SetValue(LErrorForegroundProperty, value); }
        }

        public Thickness HoverButton1Margin
        {
            get { return (Thickness) GetValue(HoverButton1MarginProperty); }
            set { SetValue(HoverButton1MarginProperty, value); }
        }

        public float HoverButtonSize
        {
            get { return (float) GetValue(HoverButtonSizeProperty); }
            set { SetValue(HoverButtonSizeProperty, value); }
        }

        public Thickness HoverButton2Margin
        {
            get { return (Thickness) GetValue(HoverButton2MarginProperty); }
            set { SetValue(HoverButton2MarginProperty, value); }
        }

        public Thickness DownElementsMargin1
        {
            get { return (Thickness) GetValue(DownElementsMargin1Property); }
            set { SetValue(DownElementsMargin1Property, value); }
        }

        public Thickness DownElementsMargin2
        {
            get { return (Thickness) GetValue(DownElementsMargin2Property); }
            set { SetValue(DownElementsMargin2Property, value); }
        }

        public Thickness DownElementsMargin3
        {
            get { return (Thickness) GetValue(DownElementsMargin3Property); }
            set { SetValue(DownElementsMargin3Property, value); }
        }

        public Thickness WindowBigMargin
        {
            get { return (Thickness) GetValue(WindowBigMarginProperty); }
            set { SetValue(WindowBigMarginProperty, value); }
        }

        private void _timer3_Tick(object sender, EventArgs e)
        {
            WeatherGetter.WeatherLocationSetDefault();
        }

        private void Weather_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _timer3.Stop();

            _timer3.Tick -= _timer3_Tick;
            _timer3 = null;

            _timer2.Tick -= RunTask2;
            _timer2 = null;

            _timer.Tick -= RunTask1;
            _timer = null;
        }

        private void RunTask1(object sender, EventArgs e)
        {
            RunTask();
        }

        private void RunTask2(object sender, EventArgs e)
        {
            _timer2.Stop();

            SetUpGui();
            GuiWeatherHourlyListener();
            GuiWeatherLocationListener();

            Rectangle.Opacity = 0d;
            RectangleLabel.Opacity = 0d;
        }

        private void RunTask()
        {
            Rectangle.Opacity = 0.8d;
            RectangleLabel.Opacity = 1d;
            _timer2.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timer2.Start();
        }

        private void Weather_Loaded(object sender, RoutedEventArgs e)
        {
            _timer3 = new DispatcherTimer {IsEnabled = false, Interval = new TimeSpan(0, 0, 4, 0, 0)};
            _timer3.Tick += _timer3_Tick;

            _timer2 = new DispatcherTimer {IsEnabled = false};
            _timer2.Tick += RunTask2;

            _timer = new DispatcherTimer {IsEnabled = false, Interval = new TimeSpan(0, 0, 3, 0, 0)};
            _timer.Tick += RunTask1;

            if (!WeatherGetter.WasAllTheWeatherLoadedCorrectly ||
                WeatherGetter.LUpdateDate.Equals(new DateTime(1, 1, 1)))
                _timer.Interval = new TimeSpan(0, 0, 0, 10, 0);

            _timer.Start();
            _timer3.Start();

            if (!From)
            {
                WeatherGetter.WeatherLocationSetDefault();
                RunTask();
            }

            From = false;

            try
            {
                ScrollViewer.ScrollToRightEnd();
                KinectTileButton currItem =
                    ((ForecastButtonControl) VisualTreeHelper.GetChild(ScrollPanel, 0)).TheForecastButton;

                currItem.BringIntoView();
            }
            catch (Exception)
            {
                ScrollViewer.ScrollToLeftEnd();
            }
        }

        private void WeatherLocation_GuiWeatherListener()
        {
            SetUpGui();
        }

        public void SetUpGui()
        {
            //ButtonHeight = (float) ScreenWidth*(11f/96); /*(float) ScreenHeight*(11f/54);*/
            //ButtonWidth = (float) ScreenWidth*(11f/96);
            //HomeButtonOffset = (float) ScreenWidth*(1f/3);
            //HourlyButtonOffset = (float) ScreenWidth*(43f/96);
            //HistoryButtonOffset = (float) ScreenWidth*(9f/16);
            //LocationFontSize = (float) ScreenWidth*(5f/192);
            //ServiceFontSize = LocationFontSize/2;
            //GridMargin = (float) ScreenWidth*(5f/96) + ", 0, " + (float) ScreenWidth*(5f/96) + ", 0";
            //GridWidth = (float) ScreenWidth*(25f/96);
            //WindowBigHeight = (float) ScreenWidth*(67f/192);
            //WindowBigWidth = (float) ScreenWidth*(43f/192);
            //BigMargin = "0, 0, 0, " + (float) ScreenHeight*(5f/54);

            LErrorText = "";
            LErrorForeground = Brushes.White;
            LUpdateForeground = Brushes.White;
            LUpdateWeight = FontWeights.Normal;

            WeatherGetter.GetAllWeather(Units);

            LocationCity = WeatherGetter.CurrentLocation.City;
            LocationCountry = WeatherGetter.CurrentLocation.Country;
            Service = "OpenWeatherMap.Org";
            LUpdateDate = WeatherGetter.LUpdateDate;

            if (!WeatherGetter.WasAllTheWeatherLoadedCorrectly ||
                WeatherGetter.LUpdateDate.Equals(new DateTime(1, 1, 1)))
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not get Weather information from OpenWeatherMap.Org!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
                Console.WriteLine(@"(WEATHER) ERROR! Weather could not be retrieved with success!");
                if (WeatherGetter.LUpdateDate.Equals(new DateTime(1, 1, 1)))
                {
                    CurrentWeatherMyButton.Visibility = Visibility.Hidden;
                    MyStackPanel.Visibility = Visibility.Hidden;
                }
                if (_timer == null) return;
                _timer.Interval = new TimeSpan(0, 0, 0, 10, 0);
                _timer.Start();
                return;
            }

            CurrentWeatherMyButton.Visibility = Visibility.Visible;
            MyStackPanel.Visibility = Visibility.Visible;

            if (_timer != null)
            {
                _timer.Interval = new TimeSpan(0, 0, 3, 0, 0);
                _timer.Start();
            }

            var buttonStyle = new Style
            {
                TargetType = typeof (ForecastButtonControl)
            };
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.WindowHeightProperty, (float) ScreenWidth*(91f/384)));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.WindowWidthProperty, (float) ScreenWidth*(77f/384)));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.DaySizeProperty, 37f));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.WeatherTextSizeProperty, 30f));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.TemperatureSizeProperty, 25f));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.TemperatureMarginProperty,
                new Thickness(0, 35, 0, 0)));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.TemperatureMinSizeProperty, 15f));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.TemperatureMinValueSizeProperty, 40f));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.TemperatureMinValueMarginProperty,
                new Thickness(0, 15, 0, 0)));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.WindTextSizeProperty, 20f));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.WindTextMarginProperty, new Thickness(5, 35, 0, 0)));
            buttonStyle.Setters.Add(new Setter(ForecastButtonControl.WindText2MarginProperty, new Thickness(5, 60, 0, 0)));

            /* XML READING STARTS */

            BigCurrentTemperatureUnits = WeatherGetter.BigCurrentTemperatureUnits.Equals("celsius")
                ? "ºC"
                : String.Empty;
            BigCurrentTemperature = Math.Round(WeatherGetter.BigCurrentTemperature).ToString(CultureInfo.CurrentCulture);
            BigCurrentWeatherText = WeatherGetter.BigCurrentWeatherText;
            BigCurrentHumidity = WeatherGetter.BigCurrentHumidity.ToString(CultureInfo.CurrentCulture);
            BigCurrentHumidityUnits = WeatherGetter.BigCurrentHumidityUnits;
            BigCurrentPressure = Math.Round(WeatherGetter.BigCurrentPressure).ToString(CultureInfo.CurrentCulture);
            BigCurrentPressureUnits = WeatherGetter.BigCurrentPressureUnits;
            BigCurrentWindSpeedText = WeatherGetter.BigCurrentWindSpeedText;
            BigCurrentWindDirectionText = WeatherGetter.BigCurrentWindDirectionText;
            BigCurrentCloudsText = WeatherGetter.BigCurrentCloudsText;
            BigCurrentPrecipitationText = WeatherGetter.BigCurrentPrecipitationText;
            MainBackground =
                new BitmapImage(new Uri("pack://application:,,," + WeatherGetter.MainBackground, UriKind.Absolute));

            ScrollPanel.Children.Clear();

            short nDays = WeatherGetter.NumberOfDaysForecast;

            for (short currentDay = 0; currentDay < nDays; currentDay++)
            {
                var button1Style = new Style
                {
                    TargetType = typeof (ForecastButtonControl),
                    BasedOn = buttonStyle
                };

                DateTime dayTemp = WeatherGetter.SmallForecastButtons[currentDay].DayDate.Date;
                string realDayName = dayTemp.DayOfWeek.ToString();
                if (dayTemp.Equals(DateTime.Now.Date))
                    realDayName = "TODAY";
                else if (dayTemp.Equals(DateTime.Now.AddDays(-1).Date))
                    realDayName = "Yesterday";
                else if (dayTemp.Equals(DateTime.Now.AddDays(1).Date))
                    realDayName = "TOMORROW";
                button1Style.Setters.Add(new Setter(ForecastButtonControl.DayNameProperty, realDayName));
                button1Style.Setters.Add(new Setter(ForecastButtonControl.WeatherIconPathProperty,
                    new BitmapImage(
                        new Uri("http://openweathermap.org/img/w/" +
                                WeatherGetter.SmallForecastButtons[currentDay].WeatherIconPath +
                                ".png"))));
                button1Style.Setters.Add(new Setter(ForecastButtonControl.WeatherTextProperty,
                    WeatherGetter.SmallForecastButtons[currentDay].WeatherText));
                if (WeatherGetter.SmallForecastButtons[currentDay].TemperatureUnit.Equals("celsius"))
                {
                    button1Style.Setters.Add(new Setter(ForecastButtonControl.TemperatureMinValueProperty,
                        Math.Round(WeatherGetter.SmallForecastButtons[currentDay].TemperatureMinValue) + " º"));
                    button1Style.Setters.Add(new Setter(ForecastButtonControl.TemperatureMaxValueProperty,
                        Math.Round(WeatherGetter.SmallForecastButtons[currentDay].TemperatureMaxValue) + " º"));
                }
                else
                {
                    button1Style.Setters.Add(new Setter(ForecastButtonControl.TemperatureMinValueProperty,
                        Math.Round(WeatherGetter.SmallForecastButtons[currentDay].TemperatureMinValue)
                            .ToString(CultureInfo.CurrentCulture)));
                    button1Style.Setters.Add(new Setter(ForecastButtonControl.TemperatureMaxValueProperty,
                        Math.Round(WeatherGetter.SmallForecastButtons[currentDay].TemperatureMaxValue)
                            .ToString(CultureInfo.CurrentCulture)));
                }
                button1Style.Setters.Add(new Setter(ForecastButtonControl.WindTextProperty,
                    WeatherGetter.SmallForecastButtons[currentDay].WindText));
                button1Style.Setters.Add(new Setter(ForecastButtonControl.WindText2Property,
                    WeatherGetter.SmallForecastButtons[currentDay].WindText2));
                button1Style.Setters.Add(new Setter(ForecastButtonControl.CloudsTextProperty,
                    WeatherGetter.SmallForecastButtons[currentDay].CloudsText));
                button1Style.Setters.Add(new Setter(ForecastButtonControl.PrecipitationTextProperty,
                    WeatherGetter.SmallForecastButtons[currentDay].PrecipitationText));
                button1Style.Setters.Add(new Setter(ForecastButtonControl.DateProperty, dayTemp));

                var forecast = new ForecastButtonControl {Style = button1Style};
                forecast.TheForecastButton.Click += TheForecastButton_Click;
                forecast.TheForecastButton.Tag = new CurrentToHourlyCorrectInfo
                {
                    CurrentDateToHourlyCorrectInfo = WeatherGetter.SmallForecastButtons[currentDay].DayDate.Date,
                    CurrentIndexToHourlyCorrectInfo = currentDay
                };
                ScrollPanel.Children.Add(forecast);
            }
        }

        public static event EventHandler GuiWeatherHourlyListener = delegate { };
        public static event EventHandler GuiWeatherLocationListener = delegate { };

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            YouNavigation.navigateToMainMenu(this);
        }

        private void ButtonHelpOnClick(object sender, RoutedEventArgs e)
        {
            YouNavigation.requestFrameChange(this, "YouWeatherHelp");
        }

        private void ButtonMapOnClick(object sender, RoutedEventArgs e)
        {
            YouNavigation.requestFrameChange(this, "YouWeatherLocation");
        }

        private void ButtonHomeOnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonHourlyOnClick(object sender, RoutedEventArgs e)
        {
            CurrentToHourlyCorrectInfo = new CurrentToHourlyCorrectInfo();
            GuiWeatherHourlyListener();
            //MainWindow.window.WeatherHourly.SetUpGui();
            YouNavigation.requestFrameChange(this, "YouWeatherHourly");
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentToHourlyCorrectInfo = new CurrentToHourlyCorrectInfo();
            GuiWeatherHourlyListener();
            //MainWindow.window.WeatherHourly.SetUpGui();
            YouNavigation.requestFrameChange(this, "YouWeatherHourly");
        }

        private void TheForecastButton_Click(object sender, RoutedEventArgs e)
        {
            var buttonName = sender as KinectTileButton;
            if (buttonName != null) CurrentToHourlyCorrectInfo = (CurrentToHourlyCorrectInfo) buttonName.Tag;
            GuiWeatherHourlyListener();
            //MainWindow.window.WeatherHourly.SetUpGui();
            YouNavigation.requestFrameChange(this, "YouWeatherHourly");
        }

        private void PageLeftButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - 5);
        }

        private void PageRightButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + 5);
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

    public class CurrentToHourlyCorrectInfo
    {
        public CurrentToHourlyCorrectInfo()
        {
            CurrentDateToHourlyCorrectInfo = new DateTime(1, 1, 1);
            CurrentIndexToHourlyCorrectInfo = -10;
        }

        public DateTime CurrentDateToHourlyCorrectInfo { get; set; }
        public short CurrentIndexToHourlyCorrectInfo { get; set; }
    }
}