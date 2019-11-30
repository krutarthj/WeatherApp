using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services.Interfaces;
using Xamarin.Essentials;
using Location = WeatherApp.Core.Models.Location;

namespace WeatherApp.Core.Services
{
    public class LocationService : ILocationService
    {
        public async Task GetLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
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