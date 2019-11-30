using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services.Interfaces;
using Xamarin.Essentials;
using Location = WeatherApp.Core.Models.Location;

namespace WeatherApp.Core.Services
{
    public class LocationService : ILocationService
    {
        public async Task<Result<Coordinates>> GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Coordinates coordinates = new Coordinates
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };

                    return new Result<Coordinates>(true, coordinates, null, null);
                }

                return new Result<Coordinates>(false, null, null, "Something went wrong. Please try again.");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                return new Result<Coordinates>(false, null, null, fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                return new Result<Coordinates>(false, null, null, fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                return new Result<Coordinates>(false, null, null, pEx.Message);
            }
            catch (Exception ex)
            {
                return new Result<Coordinates>(true, null, null, ex.Message);
            }
        }

        public async Task<Result<Location>> GetLocationAsync(string search)
        {
            var url = "https://api.teleport.org/api/cities/?search=" + WebUtility.UrlEncode(search);

            ApiResult<Location> apiResult = await RequestManager.Instance.GetApiAsync<Location>(url);

            if (apiResult != null)
            {
                return new Result<Location>(apiResult.IsSuccess, apiResult.SuccessResult, apiResult.ErrorResult?.Code, apiResult.ErrorResult?.Message);
            }
            
            return new Result<Location>();
        }

        public async Task<Result<List<Country>>> GetCountryAsync(string countrySearch)
        {
            var url = "https://restcountries.eu/rest/v2/name/" + countrySearch;

            ApiResult<List<Country>> apiResult = await RequestManager.Instance.GetApiAsync<List<Country>>(url);

            if (apiResult != null)
            {
                return new Result<List<Country>>(apiResult.IsSuccess, apiResult.SuccessResult, apiResult.ErrorResult?.Code, apiResult.ErrorResult?.Message);
            }

            return new Result<List<Country>>();
        }
    }
}