using System;
using System.Windows;
using System.Windows.Media;

namespace You_Weather.WeatherData
{
    /// <summary>
    ///     Interaction logic for ForecastButtonControl.xaml
    /// </summary>
    public partial class ForecastButtonControl
    {
        public static readonly DependencyProperty WindowWidthProperty = DependencyProperty.Register("WindowWidth",
            typeof (float), typeof (ForecastButtonControl), new PropertyMetadata(1f));

        public static readonly DependencyProperty WindowHeightProperty = DependencyProperty.Register("WindowHeight",
            typeof (float), typeof (ForecastButtonControl), new PropertyMetadata(1f));

        public static readonly DependencyProperty DaySizeProperty = DependencyProperty.Register("DaySize",
            typeof (float), typeof (ForecastButtonControl), new PropertyMetadata(1f));

        public static readonly DependencyProperty DayNameProperty = DependencyProperty.Register("DayName",
            typeof (string), typeof (ForecastButtonControl), new PropertyMetadata(" "));

        public static readonly DependencyProperty WeatherIconPathProperty =
            DependencyProperty.Register("WeatherIconPath", typeof (ImageSource), typeof (ForecastButtonControl),
                new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty WeatherTextSizeProperty =
            DependencyProperty.Register("WeatherTextSize", typeof (float), typeof (ForecastButtonControl),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty WeatherTextProperty = DependencyProperty.Register("WeatherText",
            typeof (string), typeof (ForecastButtonControl), new PropertyMetadata(""));

        public static readonly DependencyProperty TemperatureSizeProperty =
            DependencyProperty.Register("TemperatureSize", typeof (float), typeof (ForecastButtonControl),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty TemperatureMinSizeProperty =
            DependencyProperty.Register("TemperatureMinSize", typeof (float), typeof (ForecastButtonControl),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty TemperatureMinValueSizeProperty =
            DependencyProperty.Register("TemperatureMinValueSize", typeof (float), typeof (ForecastButtonControl),
                new PropertyMetadata(1f));

        public static readonly DependencyProperty TemperatureMinValueMarginProperty =
            DependencyProperty.Register("TemperatureMinValueMargin", typeof(Thickness), typeof(ForecastButtonControl),
                new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty TemperatureMinValueProperty =
            DependencyProperty.Register("TemperatureMinValue", typeof (string), typeof (ForecastButtonControl),
                new PropertyMetadata(""));

        public static readonly DependencyProperty TemperatureMaxValueProperty =
            DependencyProperty.Register("TemperatureMaxValue", typeof (string), typeof (ForecastButtonControl),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WindTextSizeProperty = DependencyProperty.Register("WindTextSize",
            typeof (float), typeof (ForecastButtonControl), new PropertyMetadata(1f));

        public static readonly DependencyProperty WindTextMarginProperty = DependencyProperty.Register(
            "WindTextMargin", typeof(Thickness), typeof(ForecastButtonControl), new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty WindTextProperty = DependencyProperty.Register("WindText",
            typeof (string), typeof (ForecastButtonControl), new PropertyMetadata(""));

        public static readonly DependencyProperty WindText2MarginProperty =
            DependencyProperty.Register("WindText2Margin", typeof (Thickness), typeof (ForecastButtonControl),
                new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty WindText2Property = DependencyProperty.Register("WindText2",
            typeof (string), typeof (ForecastButtonControl), new PropertyMetadata(""));

        public static readonly DependencyProperty CloudsTextProperty = DependencyProperty.Register("CloudsText",
            typeof (string), typeof (ForecastButtonControl), new PropertyMetadata(""));

        public static readonly DependencyProperty PrecipitationTextProperty =
            DependencyProperty.Register("PrecipitationText", typeof (string), typeof (ForecastButtonControl),
                new PropertyMetadata(""));

        public static readonly DependencyProperty TemperatureMarginProperty =
            DependencyProperty.Register("TemperatureMargin", typeof (Thickness), typeof (ForecastButtonControl),
                new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register("Date", typeof (DateTime),
            typeof (ForecastButtonControl), new PropertyMetadata(new DateTime(1, 1, 1)));

        public ForecastButtonControl()
        {
            InitializeComponent();
        }

        public float WindowWidth
        {
            get { return (float) GetValue(WindowWidthProperty); }
            set { SetValue(WindowWidthProperty, value); }
        }

        public float WindowHeight
        {
            get { return (float) GetValue(WindowHeightProperty); }
            set { SetValue(WindowHeightProperty, value); }
        }

        public float DaySize
        {
            get { return (float) GetValue(DaySizeProperty); }
            set { SetValue(DaySizeProperty, value); }
        }

        public string DayName
        {
            get { return (string) GetValue(DayNameProperty); }
            set { SetValue(DayNameProperty, value); }
        }

        public ImageSource WeatherIconPath
        {
            get { return (ImageSource)GetValue(WeatherIconPathProperty); }
            set { SetValue(WeatherIconPathProperty, value); }
        }

        public float WeatherTextSize
        {
            get { return (float) GetValue(WeatherTextSizeProperty); }
            set { SetValue(WeatherTextSizeProperty, value); }
        }

        public string WeatherText
        {
            get { return (string) GetValue(WeatherTextProperty); }
            set { SetValue(WeatherTextProperty, value); }
        }

        public float TemperatureSize
        {
            get { return (float) GetValue(TemperatureSizeProperty); }
            set { SetValue(TemperatureSizeProperty, value); }
        }

        public float TemperatureMinSize
        {
            get { return (float) GetValue(TemperatureMinSizeProperty); }
            set { SetValue(TemperatureMinSizeProperty, value); }
        }

        public float TemperatureMinValueSize
        {
            get { return (float) GetValue(TemperatureMinValueSizeProperty); }
            set { SetValue(TemperatureMinValueSizeProperty, value); }
        }

        public Thickness TemperatureMinValueMargin
        {
            get { return (Thickness) GetValue(TemperatureMinValueMarginProperty); }
            set { SetValue(TemperatureMinValueMarginProperty, value); }
        }

        public string TemperatureMinValue
        {
            get { return (string) GetValue(TemperatureMinValueProperty); }
            set { SetValue(TemperatureMinValueProperty, value); }
        }

        public string TemperatureMaxValue
        {
            get { return (string) GetValue(TemperatureMaxValueProperty); }
            set { SetValue(TemperatureMaxValueProperty, value); }
        }

        public float WindTextSize
        {
            get { return (float) GetValue(WindTextSizeProperty); }
            set { SetValue(WindTextSizeProperty, value); }
        }

        public Thickness WindTextMargin
        {
            get { return (Thickness)GetValue(WindTextMarginProperty); }
            set { SetValue(WindTextMarginProperty, value); }
        }

        public string WindText
        {
            get { return (string) GetValue(WindTextProperty); }
            set { SetValue(WindTextProperty, value); }
        }

        public Thickness WindText2Margin
        {
            get { return (Thickness) GetValue(WindText2MarginProperty); }
            set { SetValue(WindText2MarginProperty, value); }
        }

        public string WindText2
        {
            get { return (string) GetValue(WindText2Property); }
            set { SetValue(WindText2Property, value); }
        }

        public string CloudsText
        {
            get { return (string) GetValue(CloudsTextProperty); }
            set { SetValue(CloudsTextProperty, value); }
        }

        public string PrecipitationText
        {
            get { return (string) GetValue(PrecipitationTextProperty); }
            set { SetValue(PrecipitationTextProperty, value); }
        }

        public Thickness TemperatureMargin
        {
            get { return (Thickness) GetValue(TemperatureMarginProperty); }
            set { SetValue(TemperatureMarginProperty, value); }
        }

        public DateTime Date
        {
            get { return (DateTime) GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
    }
}
