using System.Threading.Tasks;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<Result<Weather>> RetrieveCurrentWeatherAsync(bool isCurrentLocation, string cityName = null);
        Task<Result<CitySearch>> RetrieveListOfCitiesAsync(string citySearchText);
    }
}