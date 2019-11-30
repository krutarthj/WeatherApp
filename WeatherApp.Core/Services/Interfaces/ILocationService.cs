using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.Services.Interfaces
{
    public interface ILocationService
    {
        Task<Result<Location>> GetLocationAsync(string search);
        Task<Result<List<Country>>> GetCountryAsync(string countrySearch);
        Task<Result<Coordinates>> GetCurrentLocationAsync();
    }
}