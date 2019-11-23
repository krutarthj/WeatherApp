using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services.Interfaces;
using WeatherApp.Core.Utilities;

namespace WeatherApp.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IWeatherService _weatherService;

        private DateTime _sunrise;
        public DateTime Sunrise
        {
            get => _sunrise;
            set
            {
                if (_sunrise == value)
                    return;

                _sunrise = value;
                RaisePropertyChanged(nameof(Sunrise));
                RaisePropertyChanged(nameof(SunriseLabel));
            }
        }
        
        private DateTime _sunset;
        public DateTime Sunset
        {
            get => _sunset;
            set
            {
                if (_sunset == value)
                    return;

                _sunset = value;
                RaisePropertyChanged(nameof(Sunset));
                RaisePropertyChanged(nameof(SunsetLabel));
            }
        }
        
        private bool _isSettingCelsius;
        public bool IsSettingCelsius
        {
            get => _isSettingCelsius;
            set
            {
                if (_isSettingCelsius == value)
                    return;

                _isSettingCelsius = value;
                RaisePropertyChanged(nameof(IsSettingCelsius));
            }
        }
        
        private double _currentTemperature;
        public double CurrentTemperature
        {
            get => _currentTemperature;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_currentTemperature == value)
                    return;

                _currentTemperature = value;
                RaisePropertyChanged(nameof(CurrentTemperature));
                RaisePropertyChanged(nameof(CurrentTemperatureLabel));
            }
        }

        private bool _isCurrentLocation = true;
        public bool IsCurrentLocation
        {
            get => _isCurrentLocation;
            set
            {
                if (_isCurrentLocation == value)
                    return;

                _isCurrentLocation = value;
                RaisePropertyChanged(nameof(IsCurrentLocation));
            }
        }

        private string _cityName;
        public string CityName
        {
            get => _cityName;
            set
            {
                if (_cityName == value)
                    return;

                _cityName = value;
                RaisePropertyChanged(nameof(CityName));
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing == value)
                    return;

                _isRefreshing = value;
                RaisePropertyChanged(nameof(IsRefreshing));
            }
        }

        private int _maximumTemperature;
        public int MaximumTemperature
        {
            get => _maximumTemperature;
            set
            {
                if (_maximumTemperature == value)
                    return;

                _maximumTemperature = value;
                RaisePropertyChanged(nameof(MaximumTemperature));
            }
        }
        
        private int _minimumTemperature;
        public int MinimumTemperature
        {
            get => _minimumTemperature;
            set
            {
                if (_minimumTemperature == value)
                    return;

                _minimumTemperature = value;
                RaisePropertyChanged(nameof(MaximumTemperature));
                RaisePropertyChanged(nameof(CurrentWeatherForecast));
            }
        }

        private string _currentCityName;
        public string CurrentCityName
        {
            get => _currentCityName;
            set
            {
                if (_currentCityName == value)
                    return;

                _currentCityName = value;
                RaisePropertyChanged(nameof(CurrentCityName));
                RaisePropertyChanged(nameof(CityCountryLabel));
            }
        }
        
        private string _countryName;
        public string CountryName
        {
            get => _countryName;
            set
            {
                if (_countryName == value)
                    return;

                _countryName = value;
                RaisePropertyChanged(nameof(CountryName));
                RaisePropertyChanged(nameof(CityCountryLabel));
            }
        }

        public string CurrentDateLabel => DateTime.Now.ToString("dddd");
        
        public string CityCountryLabel => CurrentCityName + ", " + CountryName;

        public string CurrentWeatherForecast => MaximumTemperature + "/" + MinimumTemperature;

        public string SunriseLabel => Sunrise.ToString("t");
        public string SunsetLabel => Sunset.ToString("t");
        
        public string CurrentTemperatureLabel
        {
            get
            {
                if(IsSettingCelsius)
                    return Convert.ToInt32(CurrentTemperature) + "°C";
                
                return Convert.ToInt32(CurrentTemperature) + "°F";
            }
        }
        
        public MvxObservableCollection<DailyForecastViewModel> FiveDaysForecast { get; set; }
        
        public IMvxCommand GetWeatherCommand => new MvxAsyncCommand(GetWeather);
        
        public HomeViewModel()
        {
            FiveDaysForecast = new MvxObservableCollection<DailyForecastViewModel>();
            
            _weatherService = Mvx.IoCProvider.Resolve<IWeatherService>();
        }

        public override async Task Initialize()
        {
            await GetWeather();
        }

        private async Task GetWeather()
        {
            IsRefreshing = true;

            string city = null;
            
            if (CityName != null)
            {
                city = CityName;
                if (!city.Contains(","))
                {
                    await UserDialogs.Instance.AlertAsync("Please search in format: CityName, CountryCode. For example, Newark, US");
                    IsRefreshing = false;
                    return;
                }

                if (city.Contains(",USA") || city.Contains(",usa"))
                {
                    List<string> words = city.Split(',').ToList();
                    city = words[0] + ",us";
                }
            }

            var result = await _weatherService.RetrieveCurrentWeatherAsync(IsSettingCelsius, city);
            var forecast = await _weatherService.RetrieveFiveDaysForecastsAsync(IsSettingCelsius, city);

            if (result == null)
            {
                await UserDialogs.Instance.AlertAsync("Please search in format: \n CityName, CountryCode. \n For example, Newark, US");
                IsRefreshing = false;
                return;
            }
            
            if (result.Value != null)
            {
                CurrentCityName = result.Value.Name;
                CountryName = result.Value.Sys.Country;
                CurrentTemperature = result.Value.Main.Temp;
                MaximumTemperature = Convert.ToInt32(result.Value.Main.TempMax);
                MinimumTemperature = Convert.ToInt32(result.Value.Main.TempMin);

                Sunrise = UnixTimeStampToDateTime(result.Value.Sys.Sunrise);
                Sunset = UnixTimeStampToDateTime(result.Value.Sys.Sunset);
            }

            if (forecast.Value != null)
            {
                List<DailyForecastViewModel> dailyForecastViewModels = ConvertDailyForecastsToViewModel(forecast.Value.ForecastList);
                FiveDaysForecast.Refresh(dailyForecastViewModels);
            }
            
            IsRefreshing = false;
        }

        private static List<DailyForecastViewModel> ConvertDailyForecastsToViewModel(List<Forecast> forecasts)
        {
            if (forecasts == null)
                return null;
            
            List<DailyForecastViewModel> dailyForecastViewModels = new List<DailyForecastViewModel>();
            
            foreach (var forecast in forecasts)
            {
                DailyForecastViewModel dailyForecastViewModel = ConvertDailyForecastToViewModel(forecast);
                dailyForecastViewModels.Add(dailyForecastViewModel);
            }

            return dailyForecastViewModels;
        }

        private static DailyForecastViewModel ConvertDailyForecastToViewModel(Forecast forecast)
        {
            if (forecast == null)
                return null;

            var dailyForecast = new DailyForecastViewModel
            {
                MaximumTemperature = forecast.Main.TempMax,
                MinimumTemperature = forecast.Main.TempMin
            };

            return dailyForecast;
        }
        
        private static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            return dtDateTime;
        }
    }
}
