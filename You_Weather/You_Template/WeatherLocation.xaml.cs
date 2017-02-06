using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Awesomium.Core;
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using YouInteract.YouPlugin_Developing;
using You_Weather.WeatherData;

namespace You_Weather
{
    /// <summary>
    ///     Interaction logic for WeatherLocation.xaml
    /// </summary>
    public partial class WeatherLocation : YouPlugin
    {
        public delegate void EventHandler();

        private static readonly double ScreenWidth = SystemParameters.PrimaryScreenWidth;
        /*
                private static readonly double ScreenHeight = SystemParameters.PrimaryScreenHeight;
        */

        public static readonly DependencyProperty ButtonHeightProperty = DependencyProperty.Register("ButtonHeight",
            typeof (float), typeof (WeatherLocation), new PropertyMetadata((float) ScreenWidth*(11f/96)));

        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register("ButtonWidth",
            typeof (float), typeof (WeatherLocation), new PropertyMetadata((float) ScreenWidth*(11f/96)));

        public static readonly DependencyProperty ButtonWithNamesHeightProperty =
            DependencyProperty.Register("ButtonWithNamesHeight", typeof (float), typeof (WeatherLocation),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty ButtonWithNamesWidthProperty =
            DependencyProperty.Register("ButtonWithNamesWidth", typeof (float), typeof (WeatherLocation),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty HomeButtonOffsetProperty =
            DependencyProperty.Register("HomeButtonOffset", typeof (float), typeof (WeatherLocation),
                new PropertyMetadata((float) ScreenWidth*(37f/96)));

        public static readonly DependencyProperty HourlyButtonOffsetProperty =
            DependencyProperty.Register("HourlyButtonOffset", typeof (float), typeof (WeatherLocation),
                new PropertyMetadata((float) ScreenWidth*(1f/2)));

        public static readonly DependencyProperty LocationFontSizeProperty =
            DependencyProperty.Register("LocationFontSize", typeof (float), typeof (WeatherLocation),
                new PropertyMetadata((float) ScreenWidth*(1f/32)));

        public static readonly DependencyProperty ServiceFontSizeProperty =
            DependencyProperty.Register("ServiceFontSize", typeof (float), typeof (WeatherLocation),
                new PropertyMetadata(((float) ScreenWidth*(5f/192))/2));

        public static readonly DependencyProperty LocationCityProperty = DependencyProperty.Register("LocationCity",
            typeof (string), typeof (WeatherLocation), new PropertyMetadata(" "));

        public static readonly DependencyProperty LocationCountryProperty =
            DependencyProperty.Register("LocationCountry", typeof (string), typeof (WeatherLocation),
                new PropertyMetadata(""));

        public static readonly DependencyProperty LUpdateDateProperty = DependencyProperty.Register("LUpdateDate",
            typeof (DateTime), typeof (WeatherLocation), new PropertyMetadata(new DateTime(1, 1, 1)));

        public static readonly DependencyProperty ServiceProperty = DependencyProperty.Register("Service",
            typeof (string), typeof (WeatherLocation), new PropertyMetadata(""));

        public static readonly DependencyProperty GridMarginProperty = DependencyProperty.Register("GridMargin",
            typeof (Thickness), typeof (WeatherLocation),
            new PropertyMetadata(new Thickness(ScreenWidth*(5f/96), 0, ScreenWidth*(5f/96), 0)));

        public static readonly DependencyProperty GridWidthProperty = DependencyProperty.Register("GridWidth",
            typeof (float), typeof (WeatherLocation), new PropertyMetadata((float) ScreenWidth*(35f/96)));

        public static readonly DependencyProperty WindowBigHeightProperty =
            DependencyProperty.Register("WindowBigHeight", typeof (float), typeof (WeatherLocation),
                new PropertyMetadata((float) ScreenWidth*(67f/192)));

        public static readonly DependencyProperty WindowBigWidthProperty = DependencyProperty.Register(
            "WindowBigWidth", typeof (float), typeof (WeatherLocation),
            new PropertyMetadata((float) ScreenWidth*(43f/192)));

        public static string Units;

        public static readonly DependencyProperty MainBackgroundProperty = DependencyProperty.Register(
            "MainBackground", typeof (ImageSource), typeof (WeatherLocation), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty LUpdateForegroundProperty =
            DependencyProperty.Register("LUpdateForeground", typeof (Brush), typeof (WeatherLocation),
                new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty LUpdateWeightProperty = DependencyProperty.Register("LUpdateWeight",
            typeof (FontWeight), typeof (WeatherLocation), new PropertyMetadata(FontWeights.Normal));

        //private KinectSensorChooser sensor;
        //public void KeepSensor(KinectSensorChooser _sensor)
        //{
        //    sensor = _sensor;
        //    sensorChooserUi.KinectSensorChooser = sensor;
        //    var regionSensorBinding = new Binding("Kinect") { Source = sensor };
        //    BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
        //    sensor.Start();
        //}

        public static readonly DependencyProperty LErrorTextProperty = DependencyProperty.Register("LErrorText",
            typeof (string), typeof (WeatherLocation), new PropertyMetadata(""));

        public static readonly DependencyProperty LErrorForegroundProperty =
            DependencyProperty.Register("LErrorForeground", typeof (Brush), typeof (WeatherLocation),
                new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty LocationMapProperty = DependencyProperty.Register("LocationMap",
            typeof (string), typeof (WeatherLocation), new PropertyMetadata(""));

        public static readonly DependencyProperty HoverMarginProperty = DependencyProperty.Register("HoverMargin",
            typeof (Thickness), typeof (WeatherLocation),
            new PropertyMetadata(new Thickness(ScreenWidth*(7f/1920), 0, ScreenWidth*(7f/1920), 0)));

        public static readonly DependencyProperty HoverSizeProperty = DependencyProperty.Register("HoverSize",
            typeof (float), typeof (WeatherLocation), new PropertyMetadata((float) ScreenWidth*(5f/64)));

        public static readonly DependencyProperty MarginAweProperty = DependencyProperty.Register("MarginAwe",
            typeof (Thickness), typeof (WeatherLocation), new PropertyMetadata(new Thickness(ScreenWidth*(5f/384))));

        public static readonly DependencyProperty StackHoverMarginProperty =
            DependencyProperty.Register("StackHoverMargin", typeof (Thickness), typeof (WeatherLocation),
                new PropertyMetadata(new Thickness(0, 0, 0, ScreenWidth*(1f/96))));

        public static readonly DependencyProperty HoverMargin2Property = DependencyProperty.Register("HoverMargin2",
            typeof (Thickness), typeof (WeatherLocation),
            new PropertyMetadata(new Thickness(ScreenWidth*(1f/96), ScreenWidth*(1f/96), 0, ScreenWidth*(5f/128))));

        public static readonly DependencyProperty MapHeightProperty = DependencyProperty.Register("MapHeight",
            typeof (float), typeof (WeatherLocation), new PropertyMetadata((float) ScreenWidth*(5f/16)));

        public readonly double[] ZoomValues =
        {
            0, 21282, 16355, 10064, 5540, 2909, 1485, 752, 378, 190, 95, 48, 24, 12, 6, 3,
            1.48, 0.74, 0.37, 0.19
        };

        private bool _loadReady;
        private DispatcherTimer _timer;
        private DispatcherTimer _timer3;
        private DispatcherTimer _timerMap;
        private DispatcherTimer _timerMapOkBtnDelay;

        public WeatherLocation()
        {
            InitializeComponent();
            KinectApi.bindRegion(KinectRegion);
            Weather.GuiWeatherLocationListener += WeatherLocation_GuiWeatherListener;

            MapBrowser.LoadingFrameComplete += MapBrowser_DocumentReady;

            Loaded += WeatherLocation_Loaded;
            Unloaded += WeatherLocation_Unloaded;
            SetUpGui(true);
        }

        public static short CurrentZoom { get; set; }

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

        public string LocationMap
        {
            get { return (string) GetValue(LocationMapProperty); }
            set { SetValue(LocationMapProperty, value); }
        }

        public Thickness HoverMargin
        {
            get { return (Thickness) GetValue(HoverMarginProperty); }
            set { SetValue(HoverMarginProperty, value); }
        }

        public float HoverSize
        {
            get { return (float) GetValue(HoverSizeProperty); }
            set { SetValue(HoverSizeProperty, value); }
        }

        public Thickness MarginAwe
        {
            get { return (Thickness) GetValue(MarginAweProperty); }
            set { SetValue(MarginAweProperty, value); }
        }

        public Thickness StackHoverMargin
        {
            get { return (Thickness) GetValue(StackHoverMarginProperty); }
            set { SetValue(StackHoverMarginProperty, value); }
        }

        public Thickness HoverMargin2
        {
            get { return (Thickness) GetValue(HoverMargin2Property); }
            set { SetValue(HoverMargin2Property, value); }
        }

        public float MapHeight
        {
            get { return (float) GetValue(MapHeightProperty); }
            set { SetValue(MapHeightProperty, value); }
        }

        private void WeatherLocation_Unloaded(object sender, RoutedEventArgs e)
        {
            _timerMap.Stop();
            _timer3.Stop();

            _timerMap.Tick -= PageOkButtonClick;
            _timerMap = null;

            _timer3.Tick -= _timer3_Tick;
            _timer3 = null;

            _timer.Tick -= RunTask;
            _timer = null;

            _timerMapOkBtnDelay.Tick -= OkBtnDelayAction;
            _timerMapOkBtnDelay = null;
        }

        private void WeatherLocation_Loaded(object sender, RoutedEventArgs e)
        {
            MapBrowser.Reload(true);

            _timer3 = new DispatcherTimer {IsEnabled = false, Interval = new TimeSpan(0, 0, 5, 0, 0)};
            _timer3.Tick += _timer3_Tick;

            _timer = new DispatcherTimer {IsEnabled = false};
            _timer.Tick += RunTask;

            _timerMapOkBtnDelay = new DispatcherTimer {IsEnabled = false};
            _timerMapOkBtnDelay.Tick += OkBtnDelayAction;

            _timerMap = new DispatcherTimer {IsEnabled = false, Interval = new TimeSpan(0, 0, 0, 2, 0)};
            _timerMap.Tick += PageOkButtonClick;

            _timerMap.Start();
            _timer3.Start();
        }

        private void _timer3_Tick(object sender, EventArgs e)
        {
            Weather.From = true;
            YouNavigation.requestFrameChange(this, "YouWeather");
        }

        private void WeatherLocation_GuiWeatherListener()
        {
            SetUpGui(true);
        }

        private void SetUpGui(bool normals)
        {
            if (normals)
            {
                LErrorText = "";
                LErrorForeground = Brushes.White;
                LUpdateForeground = Brushes.White;
                LUpdateWeight = FontWeights.Normal;
            }
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
                return;
            }

            HoverPanel.IsEnabled = true;
            ZoomStackPanel.IsEnabled = true;
            GoStackPanel.IsEnabled = true;
            Loc2StackPanel.Visibility = Visibility.Visible;

            MainBackground = new BitmapImage(new Uri("pack://application:,,," + WeatherGetter.MainBackground));

            try
            {
                MapBrowser.Source = new Uri("http://f4ever.byethost15.com/GoogleMaps.html");
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
            //var streamResourceInfo = Assembly.GetExecutingAssembly().GetManifestResourceStream(WeatherData.GoogleMaps.html);
            //if (streamResourceInfo == null) return;
            //var reader = new StreamReader(streamResourceInfo);
            //var theHtml = reader.ReadToEnd();
            //reader.Close();
            //const string theHtml = "<!DOCTYPE html>\n<html>\n<head>\n    <title>Google Map</title>\n    <meta name=\"viewport\" content=\"initial-scale=1.0, user-scalable=no\">\n    <meta charset=\"utf-8\">\n    <style>\n        html, body, #map-canvas {\n            height: 100%;\n            margin: 0;\n            padding: 0;\n            overflow: hidden;\n        }\n    </style>\n    <script type=\"text/javascript\" src=\"https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false\"></script>\n    <script type=\"text/javascript\">\n        var map;\n        var currentZoom = 8;\n        var geocoder;\n        var marker;\n        var infowindow = new google.maps.InfoWindow();\n        var lat = 40.5401;\n        var lng = -7.26177;\n        var marker;\n        var infoWindow;\n\n        function initialize() {\n            geocoder = new window.google.maps.Geocoder();\n\n            var mapOptions = {\n                zoom: 8,\n                center: new window.google.maps.LatLng(lat, lng),\n                disableDefaultUI: true\n            };\n\n            map = new window.google.maps.Map(window.document.getElementById(\'map-canvas\'),\n                mapOptions);\n\n            locCoords = map.getCenter();\n\n            // Add listeners to trigger checkBounds(). bounds_changed deals with zoom changes.\n            window.google.maps.event.addListener(map, \"center_changed\", function () { checkBounds(); });\n            window.google.maps.event.addListener(map, \'bounds_changed\', function () { checkBounds(); });\n        }\n\n        function Pan(x, y) {\n            try {\n                lat = map.getCenter().lat() + x;\n                lng = map.getCenter().lng() + y;\n\n                map.panTo(new window.google.maps.LatLng(lat, lng));\n            }\n            catch (ex) {\n            }\n        }\n\n        function Zoom(mode) {\n            try {\n                var currentLatLng = map.getCenter();\n\n                if (mode == \"in\") {\n                    if (currentZoom + 2 <= 18) {\n                        currentZoom += 2;\n                    }\n                }\n                else if (mode == \"out\") {\n                    if (currentZoom - 2 >= 4) {\n                        currentZoom -= 2;\n                    }\n                }\n                map.setZoom(currentZoom);\n                map.panTo(currentLatLng);\n            }\n            catch (ex) {\n            }\n        }\n\n        var city = \"\";\n        var country = \"\";\n        var locCoords;\n\n        function GetCoords0(fn) {\n            city = \"N/A\";\n            country = \"N/A\";\n            locCoords = map.getCenter();\n            geocoder.geocode({ \'latLng\': locCoords }, function (results, status) {\n                if (status == window.google.maps.GeocoderStatus.OK) {\n                    if (results[1]) {\n                        var tmp = results[1].address_components;\n                        for (var i = 0; i < tmp.length; i++) {\n                            if (contains(tmp[i].types, \"country\"))\n                                country = tmp[i].short_name;\n\n                            if (contains(tmp[i].types, \"locality\"))\n                                city = tmp[i].long_name;\n                        }\n                        fn(city + \", \" + country);\n                    }\n                }\n            });\n        }\n\n        function GetCityAndCountry() {\n            if (marker) {\n                marker.setMap(null);\n                marker = null;\n            }\n            GetCoords0(function (location) {\n                marker = new window.google.maps.Marker({\n                    position: locCoords,\n                    map: map,\n                    title: \'Location\'\n                });\n                infoWindow = new window.google.maps.InfoWindow({\n                    content: location,\n                    minWidth: 200\n                });\n                infoWindow.open(map, marker);\n            });\n        }\n\n        function GetMarkerText() {\n            if (marker)\n                return infoWindow.content;\n            return null;\n        }\n\n        function contains(a, obj) {\n            for (var i = 0; i < a.length; i++) {\n                if (a[i] === obj) {\n                    return true;\n                }\n            }\n            return false;\n        }\n\n        function GetCoords() {\n            return map.getCenter().toString();\n        }\n\n        function GetZoom() {\n            return map.getZoom();\n        }\n\n        // The allowed region which the whole map must be within\n        var southWest = new google.maps.LatLng(-85.000, -122.591);\n        var northEast = new google.maps.LatLng(85.000, -122.333);\n        var allowedBounds = new google.maps.LatLngBounds(southWest, northEast);\n        var lastValidCenter;\n        var lastValidZoom;\n\n        // If the map bounds are out of range, move it back\n        function checkBounds() {\n            // Perform the check and return if OK\n            if ((allowedBounds.getNorthEast().lat() > (map.getBounds().getNorthEast().lat())) && (allowedBounds.getSouthWest().lat() < (map.getBounds().getSouthWest().lat()))) {\n                lastValidCenter = map.getCenter();\n                lastValidZoom = map.getZoom();\n                return;\n            }\n            // not valid anymore => return to last valid position\n            map.panTo(lastValidCenter);\n            map.setZoom(lastValidZoom);\n        }\n\n        google.maps.event.addDomListener(window, \'load\', initialize);\n    </script>\n</head>\n<body>\n    <div id=\"map-canvas\"></div>\n</body>\n</html>";
            //MapBrowser.LoadHTML(theHtml);
        }

        private void MapBrowser_DocumentReady(object sender, UrlEventArgs e)
        {
            try
            {
                MapBrowser.Zoom = 180;

                _loadReady = true;
                if (WeatherGetter.CurrentCoords == null)
                {
                    MyInvokeScript("PanTo", 40.63939168501634, -8.650317420501777);
                    WeatherGetter.CurrentCoords = new[] {40.63939168501634, -8.650317420501777};
                }
                else
                {
                    MyInvokeScript("PanTo", WeatherGetter.CurrentCoords[0], WeatherGetter.CurrentCoords[1]);
                }

                short prevZoom = 8;
                if (CurrentZoom != 0)
                    prevZoom = (short) GetZoom();
                while (GetZoom() != prevZoom)
                {
                    Zoom(GetZoom() < prevZoom ? "in" : "out");
                }
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        public static event EventHandler GuiWeatherHourlyListener = delegate { };

        public static event EventHandler GuiWeatherListener = delegate { };

        private bool PageLoaded()
        {
            return _loadReady && !MapBrowser.IsCrashed && MapBrowser.IsLive;
        }

        public void Pan(double x, double y)
        {
            try
            {
                int zoomLevel = GetZoom();
                if (zoomLevel < 1 || zoomLevel > 19) return;
                double zoom = ZoomValues[zoomLevel];
                const double fact = 0.000035;
                double mult = fact*zoom;
                MyInvokeScript("Pan", x*mult, y*mult);
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        public void Zoom(string mode)
        {
            try
            {
                if (!mode.Equals("in") && !mode.Equals("out"))
                    return;
                MyInvokeScript("Zoom", mode);
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        public string GetCoords()
        {
            return MyInvokeScript("GetCoords");
        }

        public string CalculateCityAndCountry()
        {
            return MyInvokeScript("GetCityAndCountry");
        }

        private string GetCityAndCountry()
        {
            return MyInvokeScript("GetMarkerText");
        }

        public int GetZoom()
        {
            return (int) MyInvokeScript("GetZoom");
        }

        public JSValue MyInvokeScript(string name, params JSValue[] args)
        {
            if (!PageLoaded()) return -1;
            JSObject window = MapBrowser.ExecuteJavascriptWithResult("window");

            using (window)
            {
                return window.Invoke(name, args);
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

        private void PageRightButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!PageLoaded()) return;
                Pan(0, 1);
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        private void PageLeftButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!PageLoaded()) return;
                Pan(0, -1);
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        private void PageUpButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!PageLoaded()) return;
                Pan(1, 0);
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        private void PageDownButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!PageLoaded()) return;
                Pan(-1, 0);
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        private void PageZoomInButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!PageLoaded()) return;
                Zoom("in");
                CurrentZoom = (short) GetZoom();
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        private void PageZoomOutButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!PageLoaded()) return;
                Zoom("out");
                CurrentZoom = (short) GetZoom();
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        private void PageOkButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (!PageLoaded()) return;
                CalculateCityAndCountry();
                AcceptButton.IsEnabled = true;
                WeatherGetter.CurrentCoords = ConvertStringCoordsToDoubles(MyInvokeScript("GetCoords"));
                _timerMapOkBtnDelay.Interval = new TimeSpan(0, 0, 0, 0, 200);
                _timerMapOkBtnDelay.Start();
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        private void OkBtnDelayAction(object sender, EventArgs e)
        {
            try
            {
                _timerMapOkBtnDelay.Stop();

                string query = GetCityAndCountry();
                LocationMap = !String.IsNullOrEmpty(query) && !query.Equals("null") && !query.Contains("N/A")
                    ? query
                    : "Unknown";
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        private void PageAcceptButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Rectangle.Opacity = 0.8d;
                RectangleLabel.Opacity = 1d;
                _timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
                _timer.Start();
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        private void RunTask(object sender, EventArgs e)
        {
            try
            {
                _timer.Stop();

                string query = GetCityAndCountry();
                if (String.IsNullOrEmpty(query) || query.Equals("null") || query.Contains("N/A"))
                {
                    LErrorText = "ERROR: Could not find location in the map!";
                    LErrorForeground = Brushes.Red;
                    Rectangle.Opacity = 0d;
                    RectangleLabel.Opacity = 0d;
                    return;
                }
                List<string> results = WeatherGetter.GetLocation(query);
                if (results == null)
                {
                    LErrorText = "ERROR: Could not find location in OpenWeatherMap.Org servers!";
                    LErrorForeground = Brushes.Red;
                    Rectangle.Opacity = 0d;
                    RectangleLabel.Opacity = 0d;
                    return;
                }
                bool error = results[2].Equals("ALERT");
                bool error2 = results[2].Equals("ERROR");
                if (error)
                {
                    LErrorText = "WARNING: Found location in OpenWeatherMap.Org may be different!";
                    LErrorForeground = Brushes.Yellow;
                }
                else if (error2)
                {
                    LErrorText = "ERROR: Could not find location in OpenWeatherMap.Org servers!";
                    LErrorForeground = Brushes.Red;
                    Rectangle.Opacity = 0d;
                    RectangleLabel.Opacity = 0d;
                    return;
                }
                else
                {
                    LErrorText = "";
                    LErrorForeground = Brushes.White;
                    LUpdateForeground = Brushes.White;
                }
                WeatherGetter.CurrentLocation.City = results[0];
                WeatherGetter.CurrentLocation.Country = results[1];
                GuiWeatherListener();
                //MainWindow.window.Weather.SetUpGui();
                GuiWeatherHourlyListener();
                //MainWindow.window.WeatherHourly.SetUpGui();
                SetUpGui(false);
                AcceptButton.IsEnabled = false;
                //MainWindow.window.WeatherLocation.SetUpGui();

                Rectangle.Opacity = 0d;
                RectangleLabel.Opacity = 0d;

                if (results[2].Equals("ALERT") || results[2].Equals("ERROR")) return;
                Weather.From = true;
                YouNavigation.requestFrameChange(this, "YouWeather");
            }
            catch (Exception)
            {
                LErrorForeground = Brushes.Red;
                LErrorText = "ERROR: Could not access the map!";
                LUpdateForeground = Brushes.Red;
                LUpdateWeight = FontWeights.Bold;
                MainBackground =
                    new BitmapImage(
                        new Uri("pack://application:,,,/You_Weather;component/Images/WeatherBack/Weather.jpg"));
            }
        }

        public static double[] ConvertStringCoordsToDoubles(string x)
        {
            string[] y = x.Substring(1, x.Length - 2).Split(',');
            return new[]
            {
                Double.Parse(y[0].Trim(), CultureInfo.InvariantCulture),
                Double.Parse(y[1].Trim(), CultureInfo.InvariantCulture)
            };
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