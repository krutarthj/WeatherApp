using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MvvmCross;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services.Interfaces;
using Xamarin.Essentials;

namespace WeatherApp.Core.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly ILocationService _locationService;

        public WeatherService()
        {
            _locationService = Mvx.IoCProvider.Resolve<ILocationService>();
        }
        
        private readonly Key _key = new Key();

        private async Task<Tuple<double, double>> GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Lowest, TimeSpan.FromSeconds(10));
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                    return null;

                var latitude = location.Latitude;
                var longitude = location.Longitude;

                return new Tuple<double, double>(latitude, longitude);
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<Result<CurrentWeather>> RetrieveCurrentWeatherAsync(bool isCelsius, string cityName)
        {
            string url;

            if (string.IsNullOrWhiteSpace(cityName))
            {
                var test = _locationService.GetLocationAsync();

                var getLocation = GetCurrentLocation();
                
                if (getLocation.Result == null)
                {
                    return new Result<CurrentWeather>(false, null, null, "Please enable device location or give location permission to our app");
                }
                
                var (latitude, longitude) = getLocation.Result;

                //var latitude = getLocation.Result.Latitude;
                //var longitude = getLocation.Result.Longitude;
                
                if(isCelsius)
                    url = "https://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&units=metric&appid=" + _key.ApiKey;
                else
                {
                    url = "https://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&units=imperial&appid=" + _key.ApiKey;
                }
            }
            else
            {
                if(isCelsius)
                    url = "http://api.openweathermap.org/data/2.5/weather?q=" + WebUtility.UrlEncode(cityName) + "&units=metric&appid=" + _key.ApiKey;
                else
                    url = "http://api.openweathermap.org/data/2.5/weather?q=" + WebUtility.UrlEncode(cityName) + "&units=imperial&appid=" + _key.ApiKey;
            }
            
            ApiResult<CurrentWeather> apiResult = await RequestManager.Instance.GetApiAsync<CurrentWeather>(url);

            if (apiResult.SuccessResult != null)
            {
                return new Result<CurrentWeather>(apiResult.IsSuccess, apiResult.SuccessResult, apiResult.ErrorResult?.Code, apiResult.ErrorResult?.Message);
            }
            
            return new Result<CurrentWeather>();
        }

        public async Task<Result<Forecasts>> RetrieveFiveDaysForecastsAsync(bool isCelsius, string cityName)
        {
            string url;

            if (string.IsNullOrWhiteSpace(cityName))
            {
                var getLocation = GetCurrentLocation();
                
                //var getLocation = _locationService.GetCurrentPosition();
                
                if (getLocation.Result == null)
                {
                    return new Result<Forecasts>(false, null, null, "Please enable device location or give location permission to our app");
                }
                
                var (latitude, longitude) = getLocation.Result;

                //var latitude = getLocation.Result.Latitude;
                //var longitude = getLocation.Result.Longitude;
                
                if(isCelsius)
                    url = "https://api.openweathermap.org/data/2.5/forecast?lat=" + latitude + "&lon=" + longitude + "&units=metric&appid=" + _key.ApiKey;
                else
                {
                    url = "https://api.openweathermap.org/data/2.5/forecast?lat=" + latitude + "&lon=" + longitude + "&units=imperial&appid=" + _key.ApiKey;
                }
            }
            else
            {
                if(isCelsius)
                    url = "https://api.openweathermap.org/data/2.5/forecast?q=" + WebUtility.UrlEncode(cityName) + "&units=metric&appid=" + _key.ApiKey;
                else
                    url = "https://api.openweathermap.org/data/2.5/forecast?q=" + WebUtility.UrlEncode(cityName) + "&units=imperial&appid=" + _key.ApiKey;
            }
            
            ApiResult<Forecasts> apiResult = await RequestManager.Instance.GetApiAsync<Forecasts>(url);

            if (apiResult.SuccessResult != null)
            {
                Dictionary<DateTime, List<Forecast>> fiveDayThreeHourForecastDic = new Dictionary<DateTime, List<Forecast>>(5);
                foreach (var forecast in apiResult.SuccessResult.ForecastList)
                {
                    DateTime dateAndTime = DateTime.Parse(forecast.Date);

                    var date = dateAndTime.Date;
                    
                    if (fiveDayThreeHourForecastDic.TryGetValue(date, out var forecasts))
                    {
                        fiveDayThreeHourForecastDic[date].Add(forecast);
                    }
                    else
                    {
                        forecasts = new List<Forecast> {forecast};
                        fiveDayThreeHourForecastDic.Add(date, forecasts);
                    }
                }

                Dictionary<DateTime, Forecast> fiveDayForecasts = new Dictionary<DateTime, Forecast>(5);

                foreach (var key in fiveDayThreeHourForecastDic.Keys)
                {
                    List<Forecast> forecasts = fiveDayThreeHourForecastDic[key];

                    double tempMin = 1000;
                    double tempMax = -1000;
                    
                    foreach (var forecast in forecasts)
                    {
                        var forecastTempMin = forecast.Main.TempMin;
                        var forecastTempMax = forecast.Main.TempMax;

                        if (forecastTempMin < tempMin)
                        {
                            tempMin = forecastTempMin;
                        }

                        if (forecastTempMax > tempMax)
                        {
                            tempMax = forecastTempMax;
                        }
                    }

                    Forecast newForecast = new Forecast
                    {
                        Main = new Main
                        {
                            TempMin = tempMin, 
                            TempMax = tempMax
                        }, 
                        Date = key.DayOfWeek.ToString(), 
                        Weather = null
                    };
                    
                    fiveDayForecasts.Add(key, newForecast);
                }

                Forecasts newForecasts = new Forecasts();
                List<Forecast> newForecastList = new List<Forecast>();
                
                foreach (var key in fiveDayForecasts.Keys)
                {
                    newForecastList.Add(fiveDayForecasts[key]);
                }

                if (newForecastList[0].Date == DateTime.Now.DayOfWeek.ToString())
                {
                    newForecastList.RemoveAt(0);
                }
                
                newForecasts.ForecastList = newForecastList;
                
                apiResult.SuccessResult = newForecasts;

                return new Result<Forecasts>(apiResult.IsSuccess, apiResult.SuccessResult, apiResult.ErrorResult?.Code, apiResult.ErrorResult?.Message);
            }
            
            return new Result<Forecasts>();
        }
    }
}