using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<Result<CurrentWeather>> RetrieveCurrentWeatherAsync(bool isCelsius, string cityName);
        Task<Result<Forecasts>> RetrieveFiveDaysForecastsAsync(bool isCelsius, string cityName);
        //Task<Result<List<Weather>>> RetrieveCurrentWeatherAsync(bool isCurrentLocation, string cityName = null);
        //Task<Result<CitySearch>> RetrieveListOfCitiesAsync(string citySearchText);
        //Task<Result<Forcast>> RetrieveFiveDaysWeatherAsync(bool isCurrentLocation, string cityName = null);
        //Task<Result<Forcast>> RetrieveCurrentForecastAsync(bool isCurrentLocation, string cityName = null);
    }
}