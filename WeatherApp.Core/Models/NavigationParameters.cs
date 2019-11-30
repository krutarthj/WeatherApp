namespace WeatherApp.Core.Models
{
    public class NavigationParameters
    {
        public bool IsCelsius { get; set; }
        public bool IsCurrentLocationRequest { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string CityAndCountryName { get; set; }
    }
}