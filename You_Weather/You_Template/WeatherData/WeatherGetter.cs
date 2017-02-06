using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace You_Weather.WeatherData
{
    public static class WeatherGetter
    {
        public const string AppId = "6d51491fbb244f6ff0d6e763827eea07";
        private static Location _currentLocation;
        private static double[] _currentCoords;
        private static DateTime _lUpdateDate = new DateTime(1, 1, 1);
        private static string _bigCurrentTemperatureUnits = "";
        private static float _bigCurrentTemperature;
        private static string _bigCurrentWeatherText = " ";
        private static float _bigCurrentHumidity;
        private static string _bigCurrentHumidityUnits = " ";
        private static float _bigCurrentPressure;
        private static string _bigCurrentPressureUnits = " ";
        private static string _bigCurrentWindSpeedText = " ";
        private static float _bigCurrentWindSpeedValue;
        private static string _bigCurrentWindSpeedValueUnits = "Km/h";
        private static string _bigCurrentWindDirectionText = " ";
        private static float _bigCurrentWindDirectionValue;
        private static string _bigCurrentWindDirectionValueUnits = "º";
        private static string _bigCurrentCloudsText = " ";
        private static float _bigCurrentCloudsValue;
        private static string _bigCurrentCloudsValueUnits = "%";
        private static string _bigCurrentPrecipitationText = " ";
        private static float _bigCurrentPrecipitationValue;
        private static string _bigCurrentPrecipitationValueUnits = "mm/3h";
        private static string _bigCurrentWeatherIconPath = " ";

        private static string _mainBackground = "/You_Weather;component/Images/WeatherBack/Weather.jpg";
        public static List<SmallForecast> SmallForecastButtons;

        public static List<HourlyForecast> HourlyForecastButtons;

        public static XDocument Document { get; set; }

        public static string BigCurrentTemperatureUnits
        {
            get { return _bigCurrentTemperatureUnits; }
            set { _bigCurrentTemperatureUnits = value; }
        }

        public static Location CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; }
        }

        public static DateTime LUpdateDate
        {
            get { return _lUpdateDate; }
            set { _lUpdateDate = value; }
        }

        public static float BigCurrentTemperature
        {
            get { return _bigCurrentTemperature; }
            set { _bigCurrentTemperature = value; }
        }

        public static string BigCurrentWeatherText
        {
            get { return _bigCurrentWeatherText; }
            set { _bigCurrentWeatherText = value; }
        }

        public static float BigCurrentHumidity
        {
            get { return _bigCurrentHumidity; }
            set { _bigCurrentHumidity = value; }
        }

        public static string BigCurrentHumidityUnits
        {
            get { return _bigCurrentHumidityUnits; }
            set { _bigCurrentHumidityUnits = value; }
        }

        public static float BigCurrentPressure
        {
            get { return _bigCurrentPressure; }
            set { _bigCurrentPressure = value; }
        }

        public static string BigCurrentPressureUnits
        {
            get { return _bigCurrentPressureUnits; }
            set { _bigCurrentPressureUnits = value; }
        }

        public static string BigCurrentWindSpeedText
        {
            get { return _bigCurrentWindSpeedText; }
            set { _bigCurrentWindSpeedText = value; }
        }

        public static string BigCurrentWindDirectionText
        {
            get { return _bigCurrentWindDirectionText; }
            set { _bigCurrentWindDirectionText = value; }
        }

        public static string BigCurrentCloudsText
        {
            get { return _bigCurrentCloudsText; }
            set { _bigCurrentCloudsText = value; }
        }

        public static string BigCurrentPrecipitationText
        {
            get { return _bigCurrentPrecipitationText; }
            set { _bigCurrentPrecipitationText = value; }
        }

        public static string MainBackground
        {
            get { return _mainBackground; }
            set { _mainBackground = value; }
        }

        public static float BigCurrentWindSpeedValue
        {
            get { return _bigCurrentWindSpeedValue; }
            set { _bigCurrentWindSpeedValue = value; }
        }

        public static float BigCurrentWindDirectionValue
        {
            get { return _bigCurrentWindDirectionValue; }
            set { _bigCurrentWindDirectionValue = value; }
        }

        public static float BigCurrentCloudsValue
        {
            get { return _bigCurrentCloudsValue; }
            set { _bigCurrentCloudsValue = value; }
        }

        public static float BigCurrentPrecipitationValue
        {
            get { return _bigCurrentPrecipitationValue; }
            set { _bigCurrentPrecipitationValue = value; }
        }

        public static string BigCurrentWeatherIconPath
        {
            get { return _bigCurrentWeatherIconPath; }
            set { _bigCurrentWeatherIconPath = value; }
        }

        public static string BigCurrentWindSpeedValueUnits
        {
            get { return _bigCurrentWindSpeedValueUnits; }
            set { _bigCurrentWindSpeedValueUnits = value; }
        }

        public static string BigCurrentWindDirectionValueUnits
        {
            get { return _bigCurrentWindDirectionValueUnits; }
            set { _bigCurrentWindDirectionValueUnits = value; }
        }

        public static string BigCurrentCloudsValueUnits
        {
            get { return _bigCurrentCloudsValueUnits; }
            set { _bigCurrentCloudsValueUnits = value; }
        }

        public static string BigCurrentPrecipitationValueUnits
        {
            get { return _bigCurrentPrecipitationValueUnits; }
            set { _bigCurrentPrecipitationValueUnits = value; }
        }

        public static bool WasAllTheWeatherLoadedCorrectly { get; set; }
        public static short NumberOfDaysForecast { get; set; }
        public static short NumberOfHoursHourly { get; set; }

        public static double[] CurrentCoords
        {
            get { return _currentCoords; }
            set { _currentCoords = value; }
        }

        public static void WeatherLocationDefault()
        {
            if (_currentLocation != null) return;
            _currentLocation = new Location("Aveiro", "PT");
            _currentCoords = new[] {40.63939168501634, -8.650317420501777};
        }

        public static void WeatherLocationSetDefault()
        {
            _currentLocation = new Location("Aveiro", "PT");
            _currentCoords = new[] {40.63939168501634, -8.650317420501777};
        }

        public static bool ParseCurrentWeather(string units)
        {
            if (String.IsNullOrEmpty(units) || units.Contains(" "))
                units = "metric";
            short retry = 0;
            bool pass = false;

            while (retry < 1 && !pass)
            {
                retry++;
                pass = true;
                try
                {
                    Document =
                        XDocument.Load("http://api.openweathermap.org/data/2.5/weather?q=" + _currentLocation.City.Replace(" ", "%20") + "," +
                                       _currentLocation.Country.Replace(" ", "%20") + "&mode=xml&units=" + units + "&APPID=" + AppId);
                }
                catch (Exception)
                {
                    pass = false;
                }
            }

            if (retry >= 1 && !pass)
                return false;

            SmallForecastButtons = new List<SmallForecast>();

            if (Document.Root == null) return false;

            XElement city = Document.Root.Element("city");
            if (city == null) return false;
            _currentLocation.City = (string) city.Attribute("name");
            _currentLocation.Country = (string) city.Element("country");

            XElement temperature = Document.Root.Element("temperature");
            if (temperature == null) return false;
            _bigCurrentTemperatureUnits = (string) temperature.Attribute("unit");
            _bigCurrentTemperature = (float) temperature.Attribute("value");

            XElement humidity = Document.Root.Element("humidity");
            if (humidity == null) return false;
            _bigCurrentHumidityUnits = (string) humidity.Attribute("unit");
            _bigCurrentHumidity = (float) humidity.Attribute("value");

            XElement pressure = Document.Root.Element("pressure");
            if (pressure == null) return false;
            _bigCurrentPressureUnits = (string) pressure.Attribute("unit");
            _bigCurrentPressure = (float) pressure.Attribute("value");

            XElement wind = Document.Root.Element("wind");
            if (wind == null) return false;

            XElement speed = wind.Element("speed");
            if (speed == null) return false;
            _bigCurrentWindSpeedText = (string) speed.Attribute("name");
            _bigCurrentWindSpeedValue = ((float) speed.Attribute("value")*3.6f);

            XElement direction = wind.Element("direction");
            if (direction == null) return false;
            _bigCurrentWindDirectionText = (string) direction.Attribute("name");
            _bigCurrentWindDirectionValue = (float) direction.Attribute("value");

            XElement clouds = Document.Root.Element("clouds");
            if (clouds == null) return false;
            _bigCurrentCloudsText = (string) clouds.Attribute("name");
            _bigCurrentCloudsValue = (float) clouds.Attribute("value");

            XElement precipitation = Document.Root.Element("precipitation");
            if (precipitation == null) return false;
            _bigCurrentPrecipitationText = (string) precipitation.Attribute("mode");
            if (precipitation.Attribute("value") == null)
            {
                _bigCurrentPrecipitationValue = 0f;
                _bigCurrentPrecipitationValueUnits = "mm/3h";
            }
            else
            {
                _bigCurrentPrecipitationValue = (float) precipitation.Attribute("value");
                _bigCurrentPrecipitationValueUnits = "mm/" + (string) precipitation.Attribute("unit");
            }

            XElement weather = Document.Root.Element("weather");
            if (weather == null) return false;
            _bigCurrentWeatherText = (string) weather.Attribute("value");
            _bigCurrentWeatherIconPath = (string) weather.Attribute("icon");

            _mainBackground = GetWeatherBackground((string) weather.Attribute("icon"));

            XElement lastupdate = Document.Root.Element("lastupdate");
            if (lastupdate == null) return false;
            _lUpdateDate = (DateTime) lastupdate.Attribute("value");

            return true;
        }

        public static short ParseForecastWeather(string units)
        {
            if (String.IsNullOrEmpty(units) || units.Contains(" "))
                units = "metric";
            short retry = 0;
            bool pass = false;

            while (retry < 1 && !pass)
            {
                retry++;
                pass = true;
                try
                {
                    Document =
                        XDocument.Load("http://api.openweathermap.org/data/2.5/forecast/daily?q=" +
                                       _currentLocation.City.Replace(" ", "%20") + "," +
                                       _currentLocation.Country.Replace(" ", "%20") + "&mode=xml&units=" + units + "&APPID=" + AppId);
                }
                catch (Exception)
                {
                    pass = false;
                }
            }

            if (retry >= 1 && !pass)
                return -1;

            if (Document.Root == null) return -1;

            XElement forecast = Document.Root.Element("forecast");
            if (forecast == null) return -1;

            IEnumerable<XElement> times = forecast.Elements("time");
            XElement[] timesArray = times as XElement[] ?? times.ToArray();
            if (!timesArray.Any()) return -1;

            short day = 0;

            foreach (XElement time in timesArray)
            {
                var smallForecast = new SmallForecast();

                var dayName = (DateTime) time.Attribute("day");
                smallForecast.DayName = dayName.DayOfWeek.ToString();
                smallForecast.DayDate = dayName; /* dayName is actually a date */

                XElement symbol = time.Element("symbol");
                if (symbol == null) return -1;
                smallForecast.WeatherIconPath = (string) symbol.Attribute("var");
                smallForecast.WeatherText = (string) symbol.Attribute("name");

                smallForecast.MainBackground = GetWeatherBackground(smallForecast.WeatherIconPath);

                XElement precipitation = time.Element("precipitation");
                if (precipitation == null) return -1;
                var precTemp = (string) precipitation.Attribute("type");
                smallForecast.PrecipitationText = String.IsNullOrEmpty(precTemp) ? "no" : precTemp;
                if (precipitation.Attribute("value") == null)
                {
                    smallForecast.PrecipitationValue = 0f;
                }
                else
                {
                    smallForecast.PrecipitationValue = (float) precipitation.Attribute("value");
                }
                smallForecast.PrecipitationValueUnits = "mm/3h";

                XElement windDirection = time.Element("windDirection");
                if (windDirection == null) return -1;
                smallForecast.WindText2 = (string) windDirection.Attribute("name");
                smallForecast.WindDirectionValue = (float) windDirection.Attribute("deg");
                smallForecast.WindDirectionValueUnits = "º";

                XElement windSpeed = time.Element("windSpeed");
                if (windSpeed == null) return -1;
                smallForecast.WindText = (string) windSpeed.Attribute("name");
                smallForecast.WindSpeedValue = (float) windSpeed.Attribute("mps")*3.6f;
                smallForecast.WindSpeedValueUnits = "Km/h";

                XElement temperature = time.Element("temperature");
                if (temperature == null) return -1;
                smallForecast.TemperatureMinValue = (float) temperature.Attribute("min");
                smallForecast.TemperatureMaxValue = (float) temperature.Attribute("max");

                XElement clouds = time.Element("clouds");
                if (clouds == null) return -1;
                smallForecast.CloudsText = (string) clouds.Attribute("value");
                smallForecast.CloudsValue = (float) clouds.Attribute("all");
                smallForecast.CloudsValueUnits = (string) clouds.Attribute("unit");

                XElement humidity = time.Element("humidity");
                if (humidity == null) return -1;
                smallForecast.HumidityValue = (float) humidity.Attribute("value");
                smallForecast.HumidityValueUnits = (string) humidity.Attribute("unit");

                XElement pressure = time.Element("pressure");
                if (pressure == null) return -1;
                smallForecast.PressureValue = (float) pressure.Attribute("value");
                smallForecast.PressureValueUnits = (string) pressure.Attribute("unit");

                SmallForecastButtons.Add(smallForecast);
                day++;
            }

            return day;
        }

        public static short ParseHourlyWeather(string units)
        {
            if (String.IsNullOrEmpty(units) || units.Contains(" "))
                units = "metric";
            short retry = 0;
            bool pass = false;

            while (retry < 1 && !pass)
            {
                retry++;
                pass = true;
                try
                {
                    Document =
                        XDocument.Load("http://api.openweathermap.org/data/2.5/forecast?q=" + _currentLocation.City.Replace(" ", "%20") +
                                       "," +
                                       _currentLocation.Country.Replace(" ", "%20") + "&mode=xml&units=" + units + "&APPID=" + AppId);
                }
                catch (Exception)
                {
                    pass = false;
                }
            }

            if (retry >= 1 && !pass)
                return -1;

            HourlyForecastButtons = new List<HourlyForecast>();

            if (Document.Root == null) return -1;

            XElement forecast = Document.Root.Element("forecast");
            if (forecast == null) return -1;

            IEnumerable<XElement> times = forecast.Elements("time");
            XElement[] timesArray = times as XElement[] ?? times.ToArray();
            if (!timesArray.Any()) return -1;

            short hour = 0;

            foreach (XElement time in timesArray)
            {
                var hourlyForecast = new HourlyForecast();

                var fromDate = (DateTime) time.Attribute("from");
                hourlyForecast.FromDateTime = fromDate;

                var toDate = (DateTime) time.Attribute("to");
                hourlyForecast.ToDateTime = toDate;

                XElement symbol = time.Element("symbol");
                if (symbol == null) return -1;
                hourlyForecast.WeatherIconPath = (string) symbol.Attribute("var");
                hourlyForecast.WeatherText = (string) symbol.Attribute("name");

                XElement temperature = time.Element("temperature");
                if (temperature == null) return -1;
                hourlyForecast.WeatherTemperature = (float) temperature.Attribute("value");
                hourlyForecast.WeatherTemperatureUnits = (string) temperature.Attribute("unit");

                HourlyForecastButtons.Add(hourlyForecast);
                hour++;
            }

            return hour;
        }

        public static short HourlyWeatherDayNumber(short nHours)
        {
            if (HourlyForecastButtons == null)
                return -1;

            if (!HourlyForecastButtons.Any())
                return 0;

            short nDays = 0;
            DateTime currentDay = HourlyForecastButtons[0].FromDateTime.Date;
            foreach (HourlyForecast hourlyForecast in HourlyForecastButtons)
            {
                DateTime date = hourlyForecast.FromDateTime.Date;
                if (date.Equals(currentDay))
                    nDays++;
                else
                    currentDay = hourlyForecast.FromDateTime.Date;
            }
            return nDays;
        }

        public static void GetAllWeather(string units)
        {
            Console.WriteLine(@"	(WEATHER GETTER) Setting default location if there isn't any...");
            WeatherLocationDefault();

            _currentLocation.City = _currentLocation.City.Replace("N/A", "");
            _currentLocation.Country = _currentLocation.Country.Replace("N/A", "");
            if (_currentLocation.City.Equals("") || _currentLocation.Country.Equals(""))
                return;

            Console.WriteLine(@"	(WEATHER GETTER) Getting current weather...");
            bool valid = ParseCurrentWeather(units);
            Console.WriteLine(@"	(WEATHER GETTER) Success? " + valid);
            if (!valid)
            {
                WasAllTheWeatherLoadedCorrectly = false;
                NumberOfDaysForecast = -1;
                NumberOfHoursHourly = -1;
                return;
            }
            Console.WriteLine(@"	(WEATHER GETTER) Getting forecast weather...");
            short nDays = ParseForecastWeather(units);
            Console.WriteLine(@"	(WEATHER GETTER) Number of days: " + nDays);
            if (nDays == -1)
            {
                WasAllTheWeatherLoadedCorrectly = false;
                NumberOfDaysForecast = -1;
                NumberOfHoursHourly = -1;
                return;
            }
            Console.WriteLine(@"	(WEATHER GETTER) Getting hourly weather...");
            short nHours = ParseHourlyWeather(units);
            Console.WriteLine(@"	(WEATHER GETTER) Number of hours: " + nHours);

            if (nDays > 0 && nHours > 0)
                WasAllTheWeatherLoadedCorrectly = valid;
            else
                WasAllTheWeatherLoadedCorrectly = false;
            NumberOfDaysForecast = nDays;
            NumberOfHoursHourly = nHours;
        }

        public static string GetWeatherBackground(string icon)
        {
            switch (icon)
            {
                case "01d":
                case "01n":
                    return "/You_Weather;component/Images/WeatherBack/SkyIsClear.jpg";
                case "02d":
                case "02n":
                    return "/You_Weather;component/Images/WeatherBack/FewClouds.jpg";
                case "03d":
                case "03n":
                    return "/You_Weather;component/Images/WeatherBack/ScatteredClouds.jpg";
                case "04d":
                case "04n":
                    return "/You_Weather;component/Images/WeatherBack/BrokenClouds.jpg";
                case "09d":
                case "09n":
                    return "/You_Weather;component/Images/WeatherBack/ShowerRain.jpg";
                case "10d":
                case "10n":
                    return "/You_Weather;component/Images/WeatherBack/Rain.jpg";
                case "11d":
                case "11n":
                    return "/You_Weather;component/Images/WeatherBack/Thunderstorm.jpg";
                case "13d":
                case "13n":
                    return "/You_Weather;component/Images/WeatherBack/Snow.jpg";
                case "50d":
                case "50n":
                    return "/You_Weather;component/Images/WeatherBack/Mist.jpg";
                default:
                    return "/You_Weather;component/Images/WeatherBack/Weather.jpg";
            }
        }

        public static List<string> GetLocation(string location)
        {
            short retry = 0;
            bool pass = false;
            string[] split = location.Split(',');
            string locCity = split[0].Trim();

            if (locCity.Equals("N/A"))
                locCity = "";
            while (retry < 5 && !pass)
            {
                retry++;
                pass = true;
                try
                {
                    Document =
                        XDocument.Load("http://api.openweathermap.org/data/2.5/find?q=" + location.Replace(", ", ",").Replace(" ", "%20") +
                                       "&mode=xml&APPID=" + AppId);
                }
                catch (Exception)
                {
                    pass = false;
                }
            }

            if (retry >= 5 && !pass)
                return null;

            if (Document.Root == null) return null;

            XElement list = Document.Root.Element("list");
            if (list == null) return null;

            XElement item = list.Element("item");
            if (item == null) return null;

            XElement city = item.Element("city");
            if (city == null) return null;
            var cityName = (string) city.Attribute("name");
            var countryName = (string) city.Element("country");
            if (String.IsNullOrEmpty(cityName) && String.IsNullOrEmpty(cityName))
                return new List<string> { cityName, countryName, "ERROR" };
            return new List<string> {cityName, countryName, !cityName.Equals(locCity) ? "ALERT" : "OK"};
        }
    }

    public class HourlyForecast
    {
        public HourlyForecast()
        {
            FromDateTime = new DateTime(1, 1, 1);
            ToDateTime = new DateTime(1, 1, 1);
            WeatherText = " ";
            WeatherIconPath = "";
            WeatherTemperature = 0f;
            WeatherTemperatureUnits = "celsius";
        }

        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public string WeatherText { get; set; }
        public string WeatherIconPath { get; set; }
        public float WeatherTemperature { get; set; }
        public string WeatherTemperatureUnits { get; set; }
    }

    public class SmallForecast
    {
        public SmallForecast()
        {
            DayDate = new DateTime(1, 1, 1);
            DayName = " ";
            WeatherIconPath = "";
            WeatherText = " ";
            TemperatureMinValue = 0f;
            TemperatureUnit = "celsius";
            TemperatureMaxValue = 0f;
            WindText = " ";
            WindText2 = " ";
            CloudsText = " ";
            PrecipitationText = " ";
            MainBackground = "/You_Weather;component/Images/WeatherBack/Weather.jpg";
            HumidityValue = 0f;
            HumidityValueUnits = "%";
            PressureValue = 0f;
            PressureValueUnits = "hPa";
            WindSpeedValue = 0f;
            WindSpeedValueUnits = "Km/h";
            WindDirectionValue = 0f;
            WindDirectionValueUnits = "º";
            CloudsValue = 0f;
            CloudsValueUnits = "%";
            PrecipitationValue = 0f;
            PrecipitationValueUnits = "mm/3h";
        }

        public DateTime DayDate { get; set; }
        public string DayName { get; set; }
        public string WeatherIconPath { get; set; }
        public string WeatherText { get; set; }
        public float TemperatureMinValue { get; set; }
        public string TemperatureUnit { get; set; }
        public float TemperatureMaxValue { get; set; }
        public string WindText { get; set; }
        public string WindText2 { get; set; }
        public string CloudsText { get; set; }
        public string PrecipitationText { get; set; }
        public string MainBackground { get; set; }
        public float HumidityValue { get; set; }
        public string HumidityValueUnits { get; set; }
        public float PressureValue { get; set; }
        public string PressureValueUnits { get; set; }
        public float WindSpeedValue { get; set; }
        public string WindSpeedValueUnits { get; set; }
        public float WindDirectionValue { get; set; }
        public string WindDirectionValueUnits { get; set; }
        public float CloudsValue { get; set; }
        public string CloudsValueUnits { get; set; }
        public float PrecipitationValue { get; set; }
        public string PrecipitationValueUnits { get; set; }
    }

    public class Location
    {
        public Location(string city, string country)
        {
            City = city;
            Country = country;
        }

        public string City { get; set; }
        public string Country { get; set; }
    }
}