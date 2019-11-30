using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MvvmCross;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services.Interfaces;

namespace WeatherApp.Core.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly ILocationService _locationService;
        private readonly Key _key = new Key();

        public WeatherService()
        {
            _locationService = Mvx.IoCProvider.Resolve<ILocationService>();
        }

        public async Task<Result<CurrentWeatherWithForecast>> RetrieveWeatherAndForecast(bool isCelsius, string cityName)
        {
            double? latitude = null;
            double? longitude = null;
            
            if (string.IsNullOrWhiteSpace(cityName))
            {
                var getLocation = await _locationService.GetCurrentLocationAsync();

                if (!getLocation.IsSuccess || getLocation.Value == null)
                {
                    return new Result<CurrentWeatherWithForecast>(false, null, null, getLocation.ErrorMessage);
                }

                latitude = getLocation.Value.Latitude;
                longitude = getLocation.Value.Longitude;
            }

            var currentWeather = await RetrieveCurrentWeatherAsync(isCelsius, cityName, latitude, longitude);
            var forecast = await RetrieveFiveDaysForecastsAsync(isCelsius, cityName, latitude, longitude);

            var weatherAndForecast = new CurrentWeatherWithForecast();

            if(currentWeather.Value != null && forecast.Value != null)
            {
                weatherAndForecast.CurrentWeather = currentWeather.Value;
                weatherAndForecast.Forecast = forecast.Value;
            }

            if (currentWeather.ErrorMessage != null && forecast.ErrorMessage != null)
            {
                var errorMessage = currentWeather.ErrorMessage;
                return new Result<CurrentWeatherWithForecast>(false, null, null, errorMessage);
            }
            if (currentWeather.ErrorMessage != null)
            {
                var errorMessage = currentWeather.ErrorMessage;
                return new Result<CurrentWeatherWithForecast>(false, null, null, errorMessage);
            }
            if (forecast.ErrorMessage != null)
            {
                var errorMessage = forecast.ErrorMessage;
                return new Result<CurrentWeatherWithForecast>(false, null, null, errorMessage);
            }

            return new Result<CurrentWeatherWithForecast>(true, weatherAndForecast, null, null);
        }
        
        private async Task<Result<CurrentWeather>> RetrieveCurrentWeatherAsync(bool isCelsius, string cityName, double? latitude = null, double? longitude = null)
        {
            string url;

            if (string.IsNullOrWhiteSpace(cityName))
            {
                if (isCelsius)
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

            if (apiResult != null)
            {
                if (apiResult.SuccessResult == null)
                {
                    return new Result<CurrentWeather>(false, null, apiResult.ErrorResult?.Code, apiResult.ErrorResult?.Message);
                }
                
                return new Result<CurrentWeather>(apiResult.IsSuccess, apiResult.SuccessResult, apiResult.ErrorResult?.Code, apiResult.ErrorResult?.Message);
            }

            return new Result<CurrentWeather>();
        }

        private async Task<Result<Forecasts>> RetrieveFiveDaysForecastsAsync(bool isCelsius, string cityName, double? latitude = null, double? longitude = null)
        {
            string url;

            if (string.IsNullOrWhiteSpace(cityName))
            {
                if (isCelsius)
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

            if (apiResult != null)
            {
                if (apiResult.SuccessResult == null)
                {
                    return new Result<Forecasts>(false, null, apiResult.ErrorResult?.Code, apiResult.ErrorResult?.Message);
                }

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