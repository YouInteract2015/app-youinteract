using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    ///     Interaction logic for WeatherHourly.xaml
    /// </summary>
    public partial class WeatherHourly : YouPlugin
    {
        private static readonly double ScreenWidth = SystemParameters.PrimaryScreenWidth;
/*
        private static readonly double ScreenHeight = SystemParameters.PrimaryScreenHeight;
*/

        public static readonly DependencyProperty ButtonHeightProperty = DependencyProperty.Register("ButtonHeight",
            typeof (float), typeof (WeatherHourly), new PropertyMetadata((float) ScreenWidth*(11f/96)));

        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register("ButtonWidth",
            typeof (float), typeof (WeatherHourly), new PropertyMetadata((float) ScreenWidth*(11f/96)));

        public static readonly DependencyProperty ButtonWithNamesHeightProperty =
            DependencyProperty.Register("ButtonWithNamesHeight", typeof (float), typeof (WeatherHourly),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty ButtonWithNamesWidthProperty =
            DependencyProperty.Register("ButtonWithNamesWidth", typeof (float), typeof (WeatherHourly),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty HomeButtonOffsetProperty =
            DependencyProperty.Register("HomeButtonOffset", typeof (float), typeof (WeatherHourly),
                new PropertyMetadata((float) ScreenWidth*(37f/96)));

        public static readonly DependencyProperty HourlyButtonOffsetProperty =
            DependencyProperty.Register("HourlyButtonOffset", typeof (float), typeof (WeatherHourly),
                new PropertyMetadata((float) ScreenWidth*(1f/2)));

        public static readonly DependencyProperty LocationFontSizeProperty =
            DependencyProperty.Register("LocationFontSize", typeof (float), typeof (WeatherHourly),
                new PropertyMetadata((float) ScreenWidth*(1f/32)));

        public static readonly DependencyProperty ServiceFontSizeProperty =
            DependencyProperty.Register("ServiceFontSize", typeof (float), typeof (WeatherHourly),
                new PropertyMetadata(((float) ScreenWidth*(5f/192))/2));

        public static readonly DependencyProperty LocationCityProperty = DependencyProperty.Register("LocationCity",
            typeof (string), typeof (WeatherHourly), new PropertyMetadata(""));

        public static readonly DependencyProperty LocationCountryProperty =
            DependencyProperty.Register("LocationCountry", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty LUpdateDateProperty = DependencyProperty.Register("LUpdateDate",
            typeof (DateTime), typeof (WeatherHourly), new PropertyMetadata(new DateTime(1, 1, 1)));

        public static readonly DependencyProperty ServiceProperty = DependencyProperty.Register("Service",
            typeof (string), typeof (WeatherHourly), new PropertyMetadata(""));

        public static readonly DependencyProperty GridMarginProperty = DependencyProperty.Register("GridMargin",
            typeof (Thickness), typeof (WeatherHourly),
            new PropertyMetadata(new Thickness(ScreenWidth*(5f/96), 0, ScreenWidth*(5f/96), 0)));

        public static readonly DependencyProperty GridWidthProperty = DependencyProperty.Register("GridWidth",
            typeof (float), typeof (WeatherHourly), new PropertyMetadata((float) ScreenWidth*(25f/96)));

        public static string Units;

        public static readonly DependencyProperty WindowBigHeightProperty =
            DependencyProperty.Register("WindowBigHeight", typeof (float), typeof (WeatherHourly),
                new PropertyMetadata((float) ScreenWidth*(67f/192)));

        public static readonly DependencyProperty WindowBigWidthProperty = DependencyProperty.Register(
            "WindowBigWidth", typeof (float), typeof (WeatherHourly),
            new PropertyMetadata((float) ScreenWidth*(95f/384)));

        public static readonly DependencyProperty DayNameProperty = DependencyProperty.Register("DayName",
            typeof (string), typeof (WeatherHourly), new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherTextProperty = DependencyProperty.Register("WeatherText",
            typeof (string), typeof (WeatherHourly), new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherTemperatureProperty =
            DependencyProperty.Register("WeatherTemperature", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherHumidityProperty =
            DependencyProperty.Register("WeatherHumidity", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherPressureProperty =
            DependencyProperty.Register("WeatherPressure", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherWindSpeedValueProperty =
            DependencyProperty.Register("WeatherWindSpeedValue", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherWindSpeedTextProperty =
            DependencyProperty.Register("WeatherWindSpeedText", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherWindDirectionValueProperty =
            DependencyProperty.Register("WeatherWindDirectionValue", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherWindDirectionTextProperty =
            DependencyProperty.Register("WeatherWindDirectionText", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherCloudsValueProperty =
            DependencyProperty.Register("WeatherCloudsValue", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherCloudsTextProperty =
            DependencyProperty.Register("WeatherCloudsText", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherPrecipitationValueProperty =
            DependencyProperty.Register("WeatherPrecipitationValue", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WeatherPrecipitationTextProperty =
            DependencyProperty.Register("WeatherPrecipitationText", typeof (string), typeof (WeatherHourly),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WindowSmallHeightProperty =
            DependencyProperty.Register("WindowSmallHeight", typeof (float), typeof (WeatherHourly),
                new PropertyMetadata((float) ScreenWidth*(65f/192)));

        public static readonly DependencyProperty WindowSmallWidthProperty =
            DependencyProperty.Register("WindowSmallWidth", typeof (float), typeof (WeatherHourly),
                new PropertyMetadata((float) ScreenWidth*(5f/12)));

        public static readonly DependencyProperty WeatherIconPathProperty =
            DependencyProperty.Register("WeatherIconPath", typeof (ImageSource), typeof (WeatherHourly),
                new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty MainBackgroundProperty = DependencyProperty.Register(
            "MainBackground", typeof (ImageSource), typeof (WeatherHourly), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty LUpdateForegroundProperty =
            DependencyProperty.Register("LUpdateForeground", typeof (Brush), typeof (WeatherHourly),
                new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty LUpdateWeightProperty = DependencyProperty.Register("LUpdateWeight",
            typeof (FontWeight), typeof (WeatherHourly), new PropertyMetadata(FontWeights.Normal));

        public static readonly DependencyProperty LErrorTextProperty = DependencyProperty.Register("LErrorText",
            typeof (string), typeof (WeatherHourly), new PropertyMetadata(""));

        public static readonly DependencyProperty LErrorForegroundProperty =
            DependencyProperty.Register("LErrorForeground", typeof (Brush), typeof (WeatherHourly),
                new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty BorderMarginProperty = DependencyProperty.Register("BorderMargin",
            typeof (Thickness), typeof (WeatherHourly),
            new PropertyMetadata(new Thickness(ScreenWidth*(1f/384), ScreenWidth*(1f/384), ScreenWidth*(1f/384),
                ScreenWidth*(1f/384))));

        public static readonly DependencyProperty ScrollViewerMarginProperty =
            DependencyProperty.Register("ScrollViewerMargin", typeof (Thickness), typeof (WeatherHourly),
                new PropertyMetadata(new Thickness(ScreenWidth*(1f/384), ScreenWidth*(11f/192), 0, ScreenWidth*(11f/192))));

        public static readonly DependencyProperty HoverBtnSizeProperty = DependencyProperty.Register("HoverBtnSize",
            typeof (float), typeof (WeatherHourly), new PropertyMetadata((float) ScreenWidth*(5f/64)));

        public static readonly DependencyProperty HoverMarginProperty = DependencyProperty.Register("HoverMargin",
            typeof (Thickness), typeof (WeatherHourly),
            new PropertyMetadata(new Thickness(ScreenWidth*(5f/128), ScreenWidth*(11f/192), 0, ScreenWidth*(11f/192))));

        private Dictionary<DateTime, Label> _controls;
        private DispatcherTimer _timer3;

        public WeatherHourly()
        {
            InitializeComponent();
            KinectApi.bindRegion(KinectRegion);
            Weather.GuiWeatherHourlyListener += Weather_GuiWeatherHourlyListener;
            WeatherLocation.GuiWeatherHourlyListener += Weather_GuiWeatherHourlyListener;
            WeatherHelp.GuiWeatherHourlyListener += Weather_GuiWeatherHourlyListener;

            Loaded += WeatherHourly_Loaded;
            Unloaded += WeatherHourly_Unloaded;
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

        public string DayName
        {
            get { return (string) GetValue(DayNameProperty); }
            set { SetValue(DayNameProperty, value); }
        }

        public string WeatherText
        {
            get { return (string) GetValue(WeatherTextProperty); }
            set { SetValue(WeatherTextProperty, value); }
        }

        public string WeatherTemperature
        {
            get { return (string) GetValue(WeatherTemperatureProperty); }
            set { SetValue(WeatherTemperatureProperty, value); }
        }

        public string WeatherHumidity
        {
            get { return (string) GetValue(WeatherHumidityProperty); }
            set { SetValue(WeatherHumidityProperty, value); }
        }

        public string WeatherPressure
        {
            get { return (string) GetValue(WeatherPressureProperty); }
            set { SetValue(WeatherPressureProperty, value); }
        }

        public string WeatherWindSpeedValue
        {
            get { return (string) GetValue(WeatherWindSpeedValueProperty); }
            set { SetValue(WeatherWindSpeedValueProperty, value); }
        }

        public string WeatherWindSpeedText
        {
            get { return (string) GetValue(WeatherWindSpeedTextProperty); }
            set { SetValue(WeatherWindSpeedTextProperty, value); }
        }

        public string WeatherWindDirectionValue
        {
            get { return (string) GetValue(WeatherWindDirectionValueProperty); }
            set { SetValue(WeatherWindDirectionValueProperty, value); }
        }

        public string WeatherWindDirectionText
        {
            get { return (string) GetValue(WeatherWindDirectionTextProperty); }
            set { SetValue(WeatherWindDirectionTextProperty, value); }
        }

        public string WeatherCloudsValue
        {
            get { return (string) GetValue(WeatherCloudsValueProperty); }
            set { SetValue(WeatherCloudsValueProperty, value); }
        }

        public string WeatherCloudsText
        {
            get { return (string) GetValue(WeatherCloudsTextProperty); }
            set { SetValue(WeatherCloudsTextProperty, value); }
        }

        public string WeatherPrecipitationValue
        {
            get { return (string) GetValue(WeatherPrecipitationValueProperty); }
            set { SetValue(WeatherPrecipitationValueProperty, value); }
        }

        public string WeatherPrecipitationText
        {
            get { return (string) GetValue(WeatherPrecipitationTextProperty); }
            set { SetValue(WeatherPrecipitationTextProperty, value); }
        }

        public float WindowSmallHeight
        {
            get { return (float) GetValue(WindowSmallHeightProperty); }
            set { SetValue(WindowSmallHeightProperty, value); }
        }

        public float WindowSmallWidth
        {
            get { return (float) GetValue(WindowSmallWidthProperty); }
            set { SetValue(WindowSmallWidthProperty, value); }
        }

        public ImageSource WeatherIconPath
        {
            get { return (ImageSource) GetValue(WeatherIconPathProperty); }
            set { SetValue(WeatherIconPathProperty, value); }
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

        public Thickness BorderMargin
        {
            get { return (Thickness) GetValue(BorderMarginProperty); }
            set { SetValue(BorderMarginProperty, value); }
        }

        public Thickness ScrollViewerMargin
        {
            get { return (Thickness) GetValue(ScrollViewerMarginProperty); }
            set { SetValue(ScrollViewerMarginProperty, value); }
        }

        public float HoverBtnSize
        {
            get { return (float) GetValue(HoverBtnSizeProperty); }
            set { SetValue(HoverBtnSizeProperty, value); }
        }

        public Thickness HoverMargin
        {
            get { return (Thickness) GetValue(HoverMarginProperty); }
            set { SetValue(HoverMarginProperty, value); }
        }

        private void WeatherHourly_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer3.Stop();

            _timer3.Tick -= _timer3_Tick;
            _timer3 = null;
        }

        private void _timer3_Tick(object sender, EventArgs e)
        {
            Weather.From = true;
            YouNavigation.requestFrameChange(this, "YouWeather");
        }

        private void WeatherHourly_Loaded(object sender, RoutedEventArgs e)
        {
            _timer3 = new DispatcherTimer {IsEnabled = false, Interval = new TimeSpan(0, 0, 5, 0, 0)};
            _timer3.Tick += _timer3_Tick;

            _timer3.Start();

            DateTime date = Weather.CurrentToHourlyCorrectInfo.CurrentDateToHourlyCorrectInfo;
            try
            {
                ScrollViewer.ScrollToBottom();
                Label row = _controls[date];

                row.BringIntoView();
            }
            catch (Exception)
            {
                if (date.CompareTo(_controls.Keys.Max()) > 0)
                    ScrollViewer.ScrollToBottom();
                else
                    _controls[_controls.Keys.Min()].BringIntoView();
            }
        }

        private void Weather_GuiWeatherHourlyListener()
        {
            SetUpGui();
        }

        public void SetUpGui()
        {
            LErrorText = "";
            LErrorForeground = Brushes.White;
            LUpdateForeground = Brushes.White;
            LUpdateWeight = FontWeights.Normal;

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
            ////BigMargin = "0, 0, 0, " + (float)ScreenHeight * (5f / 54);

            /* XML READING STARTS */

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
                if (!WeatherGetter.LUpdateDate.Equals(new DateTime(1, 1, 1))) return;
                HourlyBorder.Visibility = Visibility.Hidden;
                HourlyStackPanel.Visibility = Visibility.Hidden;
                return;
            }

            HourlyBorder.Visibility = Visibility.Visible;
            HourlyStackPanel.Visibility = Visibility.Visible;

            //BigCurrentTemperatureUnits = WeatherGetter.BigCurrentTemperatureUnits.Equals("celsius") ? "ºC" : String.Empty;
            //BigCurrentTemperature = (float)Math.Round(WeatherGetter.BigCurrentTemperature);
            //BigCurrentWeatherText = WeatherGetter.BigCurrentWeatherText;
            //BigCurrentHumidity = WeatherGetter.BigCurrentHumidity;
            //BigCurrentHumidityUnits = WeatherGetter.BigCurrentHumidityUnits;
            //BigCurrentPressure = WeatherGetter.BigCurrentPressure;
            //BigCurrentPressureUnits = WeatherGetter.BigCurrentPressureUnits;
            //BigCurrentWindSpeedText = WeatherGetter.BigCurrentWindSpeedText;
            //BigCurrentWindDirectionText = WeatherGetter.BigCurrentWindDirectionText;
            //BigCurrentCloudsText = WeatherGetter.BigCurrentCloudsText;
            //BigCurrentPrecipitationText = WeatherGetter.BigCurrentPrecipitationText;

            if (WeatherGetter.SmallForecastButtons.Any())
            {
                short index = Weather.CurrentToHourlyCorrectInfo.CurrentIndexToHourlyCorrectInfo;
                if (index == -10)
                {
                    DayName = "NOW";
                    MainBackground = new BitmapImage(new Uri("pack://application:,,," + WeatherGetter.MainBackground));

                    WeatherIconPath =
                        new BitmapImage(
                            new Uri("http://openweathermap.org/img/w/" + WeatherGetter.BigCurrentWeatherIconPath +
                                    ".png"));
                    WeatherText = WeatherGetter.BigCurrentWeatherText;
                    WeatherTemperature = WeatherGetter.BigCurrentTemperature.ToString(CultureInfo.CurrentCulture);
                    if (WeatherGetter.BigCurrentTemperatureUnits.Equals("celsius"))
                        WeatherTemperature += " ºC";
                    WeatherHumidity = WeatherGetter.BigCurrentHumidity + " " + WeatherGetter.BigCurrentHumidityUnits;
                    WeatherPressure = WeatherGetter.BigCurrentPressure + " " + WeatherGetter.BigCurrentPressureUnits;
                    WeatherWindSpeedValue = WeatherGetter.BigCurrentWindSpeedValue + " " +
                                            WeatherGetter.BigCurrentWindSpeedValueUnits;
                    WeatherWindSpeedText = WeatherGetter.BigCurrentWindSpeedText;
                    WeatherWindDirectionValue = WeatherGetter.BigCurrentWindDirectionValue + " " +
                                                WeatherGetter.BigCurrentWindDirectionValueUnits;
                    WeatherWindDirectionText = WeatherGetter.BigCurrentWindDirectionText;
                    WeatherCloudsValue = WeatherGetter.BigCurrentCloudsValue + " " +
                                         WeatherGetter.BigCurrentCloudsValueUnits;
                    WeatherCloudsText = WeatherGetter.BigCurrentCloudsText;
                    WeatherPrecipitationValue = WeatherGetter.BigCurrentPrecipitationValue + " " +
                                                WeatherGetter.BigCurrentPrecipitationValueUnits;
                    WeatherPrecipitationText = WeatherGetter.BigCurrentPrecipitationText;
                }
                else
                {
                    DayName = WeatherGetter.SmallForecastButtons[index].DayName;
                    DateTime dayDate = WeatherGetter.SmallForecastButtons[index].DayDate;
                    if (dayDate.Date.Equals(DateTime.Now.Date))
                        DayName = "TODAY";
                    else if (dayDate.Date.Equals(DateTime.Now.AddDays(-1).Date))
                        DayName = "Yesterday";
                    else if (dayDate.Date.Equals(DateTime.Now.AddDays(1).Date))
                        DayName = "TOMORROW";
                    MainBackground =
                        new BitmapImage(
                            new Uri("pack://application:,,," + WeatherGetter.SmallForecastButtons[index].MainBackground));

                    WeatherIconPath = new BitmapImage(new Uri("http://openweathermap.org/img/w/" +
                                                              WeatherGetter.SmallForecastButtons[index].WeatherIconPath +
                                                              ".png"));
                    WeatherText = WeatherGetter.SmallForecastButtons[index].WeatherText;
                    WeatherTemperature =
                        WeatherGetter.SmallForecastButtons[index].TemperatureMinValue.ToString(
                            CultureInfo.CurrentCulture);
                    WeatherTemperature += " / ";
                    WeatherTemperature +=
                        WeatherGetter.SmallForecastButtons[index].TemperatureMaxValue.ToString(
                            CultureInfo.CurrentCulture);
                    if (WeatherGetter.SmallForecastButtons[index].TemperatureUnit.Equals("celsius"))
                        WeatherTemperature += " ºC";
                    else
                        WeatherTemperature += " ";
                    WeatherHumidity = WeatherGetter.SmallForecastButtons[index].HumidityValue + " " +
                                      WeatherGetter.SmallForecastButtons[index].HumidityValueUnits;
                    WeatherPressure = WeatherGetter.SmallForecastButtons[index].PressureValue + " " +
                                      WeatherGetter.SmallForecastButtons[index].PressureValueUnits;
                    WeatherWindSpeedValue = WeatherGetter.SmallForecastButtons[index].WindSpeedValue + " " +
                                            WeatherGetter.SmallForecastButtons[index].WindSpeedValueUnits;
                    WeatherWindSpeedText = WeatherGetter.SmallForecastButtons[index].WindText;
                    WeatherWindDirectionValue = WeatherGetter.SmallForecastButtons[index].WindDirectionValue + " " +
                                                WeatherGetter.SmallForecastButtons[index].WindDirectionValueUnits;
                    WeatherWindDirectionText = WeatherGetter.SmallForecastButtons[index].WindText2;
                    WeatherCloudsValue = WeatherGetter.SmallForecastButtons[index].CloudsValue + " " +
                                         WeatherGetter.SmallForecastButtons[index].CloudsValueUnits;
                    WeatherCloudsText = WeatherGetter.SmallForecastButtons[index].CloudsText;
                    WeatherPrecipitationValue = WeatherGetter.SmallForecastButtons[index].PrecipitationValue + " " +
                                                WeatherGetter.SmallForecastButtons[index].PrecipitationValueUnits;
                    WeatherPrecipitationText = WeatherGetter.SmallForecastButtons[index].PrecipitationText;
                }
            }

            HourlyForecastListGrid.Children.Clear();

            //HourlyForecastListGrid.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(100)});
            //HourlyForecastListGrid.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(75)});
            //HourlyForecastListGrid.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(1, GridUnitType.Star)});
            //HourlyForecastListGrid.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(1, GridUnitType.Auto)});

            short nHours = WeatherGetter.NumberOfHoursHourly;
            short nDays = WeatherGetter.HourlyWeatherDayNumber(nHours);

            for (short i = 0; i < nHours + nDays; i++)
            {
                HourlyForecastListGrid.RowDefinitions.Add(new RowDefinition());
                if (i%2 == 0) continue;
                for (short j = 0; j < 4; j++)
                {
                    var gridRowMarker = new Grid
                    {
                        Background = new SolidColorBrush(Colors.Green),
                        Opacity = 0.2,
                    };

                    Grid.SetRow(gridRowMarker, i);
                    Grid.SetColumn(gridRowMarker, j);

                    HourlyForecastListGrid.Children.Add(gridRowMarker);
                }
            }

            if (!WeatherGetter.HourlyForecastButtons.Any())
                return;

            //var firstDay = new Label
            //{
            //    VerticalAlignment = VerticalAlignment.Center,
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    Foreground = new SolidColorBrush(Colors.White),
            //    FontSize = 15,
            //    FontWeight = FontWeights.Bold,
            //    FontFamily = new FontFamily("Calibri"),
            //};
            //if (now.Date.Equals(currentDay))
            //{
            //    firstDay.Content = "Today";
            //}
            //else if (now.Date.Equals(currentDay.AddDays(1)))
            //{
            //    firstDay.Content = "Tomorrow";
            //}
            //else if (now.Date.Equals(currentDay.AddDays(-1)))
            //{
            //    firstDay.Content = "Yesterday";
            //}
            //else
            //{
            //    firstDay.Content = currentDay.Day + "/" + currentDay.Month;
            //}

            //Grid.SetRow(firstDay, 0);
            //Grid.SetColumn(firstDay, 0);

            //HourlyForecastListGrid.Children.Add(firstDay);

            _controls = new Dictionary<DateTime, Label>();
            var currentDay = new DateTime(1, 1, 1);
            DateTime now = DateTime.Now;
            for (short currentHour = 0, currentRow = 0, counter = 0; currentHour < nHours; currentHour++, currentRow++)
            {
                DateTime tempDate = WeatherGetter.HourlyForecastButtons[currentHour].FromDateTime.Date;
                if (!tempDate.Equals(currentDay))
                {
                    currentDay = tempDate;

                    var day = new Label
                    {
                        Name = "L" + counter.ToString(CultureInfo.CurrentCulture),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Foreground = new SolidColorBrush(Colors.White),
                        FontSize = 15,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new FontFamily("Calibri"),
                    };
                    counter++;
                    if (now.Date.Equals(currentDay))
                    {
                        day.Content = "TODAY";
                    }
                    else if (now.Date.Equals(currentDay.AddDays(-1)))
                    {
                        day.Content = "TOMORROW";
                    }
                    else if (now.Date.Equals(currentDay.AddDays(1)))
                    {
                        day.Content = "Yesterday";
                    }
                    else
                    {
                        day.Content = currentDay.DayOfWeek;
                    }

                    _controls.Add(currentDay, day);

                    Grid.SetRow(day, currentRow);
                    Grid.SetColumn(day, 0);

                    HourlyForecastListGrid.Children.Add(day);
                    currentRow++;
                }

                DateTime currentFromDateTime = WeatherGetter.HourlyForecastButtons[currentHour].FromDateTime;
                DateTime currentToDateTime = WeatherGetter.HourlyForecastButtons[currentHour].ToDateTime;
                var hour = new Label
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Foreground = new SolidColorBrush(Colors.LightGray),
                    FontSize = 15,
                    Margin = new Thickness(15, 0, 25, 0),
                    FontFamily = new FontFamily("Calibri"),
                    Content = currentFromDateTime.Hour + "h-" + currentToDateTime.Hour + "h"
                };

                Grid.SetRow(hour, currentRow);
                Grid.SetColumn(hour, 0);

                HourlyForecastListGrid.Children.Add(hour);

                var temperature = new TextBlock
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 18,
                    FontFamily = new FontFamily("Verdana"),
                    Margin = new Thickness(0, 0, 5, 0),
                };
                if (WeatherGetter.HourlyForecastButtons[currentHour].WeatherTemperatureUnits.Equals("celsius"))
                    temperature.Text = Math.Round(WeatherGetter.HourlyForecastButtons[currentHour].WeatherTemperature) +
                                       " º";
                else
                    temperature.Text =
                        WeatherGetter.HourlyForecastButtons[currentHour].WeatherTemperature.ToString(
                            CultureInfo.CurrentCulture);

                Grid.SetRow(temperature, currentRow);
                Grid.SetColumn(temperature, 1);

                HourlyForecastListGrid.Children.Add(temperature);

                var state = new TextBlock
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 20,
                    FontFamily = new FontFamily("Calibri"),
                    Margin = new Thickness(0, 0, 5, 0),
                    Text = WeatherGetter.HourlyForecastButtons[currentHour].WeatherText
                };

                Grid.SetRow(state, currentRow);
                Grid.SetColumn(state, 2);

                HourlyForecastListGrid.Children.Add(state);

                var icon = new Image
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Width = 25,
                    Height = 25,
                    Stretch = Stretch.Uniform,
                    Source =
                        new BitmapImage(
                            new Uri("http://openweathermap.org/img/w/" +
                                    WeatherGetter.HourlyForecastButtons[currentHour].WeatherIconPath + ".png"))
                };

                Grid.SetRow(icon, currentRow);
                Grid.SetColumn(icon, 3);

                HourlyForecastListGrid.Children.Add(icon);
            }
        }

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
            Weather.From = true;
            YouNavigation.requestFrameChange(this, "YouWeather");
        }

        private void ButtonHourlyOnClick(object sender, RoutedEventArgs e)
        {
        }

        private void PageUpButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - 5);
        }

        private void PageDownButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + 5);
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