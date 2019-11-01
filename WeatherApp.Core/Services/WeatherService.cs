using System.Net;
using System.Threading.Tasks;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services.Interfaces;
using Xamarin.Essentials;
using Location = WeatherApp.Core.Models.Location;

namespace WeatherApp.Core.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly Key _key = new Key();

        private async Task<ApiResult<Location>> RetrieveLocationKeyAsync(bool isCurrentLocation, string cityName)
        {
            string url = null;
            
            if (isCurrentLocation)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location == null)
                    return null;
                
                var latitude = location.Latitude;
                var longitude = location.Longitude;
                
                url = "http://dataservice.accuweather.com/locations/v1/cities/geoposition/search?apikey=" + _key.ApiKey + "&q=" + latitude + "," + longitude;

            }

            if (!string.IsNullOrWhiteSpace(cityName) && !isCurrentLocation)
            {
                url = "http://dataservice.accuweather.com/locations/v1/cities/geoposition/search?apikey=" + _key.ApiKey + "&q=" + WebUtility.UrlEncode(cityName);
            }
            
            var apiResult = await RequestManager.Instance.GetApiAsync<Location>(url).ConfigureAwait(false);

            return apiResult;
        }
        
        public async Task<Result<Weather>> RetrieveCurrentWeatherAsync(bool isCurrentLocation, string  cityName = null)
        {
            var location = RetrieveLocationKeyAsync(isCurrentLocation, cityName);
            var locationKey = location.Result.SuccessResult.Key;
            
            var url = "http://dataservice.accuweather.com/currentconditions/v1/" + locationKey + ".json?apikey=" + _key.ApiKey;

            var apiResult = await RequestManager.Instance.GetApiAsync<Weather>(url).ConfigureAwait(false);

            if (apiResult != null)
            {
                return new Result<Weather>(apiResult.IsSuccess, apiResult.SuccessResult, apiResult.ErrorResult?.Code, apiResult.ErrorResult?.Message);
            }
            
            return new Result<Weather>();
        }

        public async Task<Result<CitySearch>> RetrieveListOfCitiesAsync(string citySearchText)
        {
            var url = "https://api.teleport.org/api/cities/?search=" + WebUtility.UrlEncode(citySearchText);

            var apiResult = await RequestManager.Instance.GetApiAsync<CitySearch>(url).ConfigureAwait(false);

            if (apiResult != null)
            {
                return new Result<CitySearch>(apiResult.IsSuccess, apiResult.SuccessResult, apiResult.ErrorResult?.Code, apiResult.ErrorResult?.Message);
            }

            return new Result<CitySearch>();
        }
    }
}