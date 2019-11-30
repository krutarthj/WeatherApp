namespace WeatherApp.Core.Models
{
    public class CurrentWeatherWithForecast
    {
        public CurrentWeather CurrentWeather { get; set; }
        public Forecasts Forecast { get; set; }
    }
}
