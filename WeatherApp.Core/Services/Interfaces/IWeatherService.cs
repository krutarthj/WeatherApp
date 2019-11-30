using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<Result<CurrentWeatherWithForecast>> RetrieveWeatherAndForecast(bool isCelsius, string cityName);
    }
}