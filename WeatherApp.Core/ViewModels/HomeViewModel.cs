using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services.Interfaces;
using WeatherApp.Core.Utilities;

namespace WeatherApp.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IWeatherService _weatherService;

        private string _currentCondition;
        public string CurrentCondition
        {
            get => _currentCondition;
            set
            {
                if (_currentCondition == value)
                    return;
                
                _currentCondition = value;
                RaisePropertyChanged(nameof(CurrentCondition));
            }
        }

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
                RaisePropertyChanged(nameof(MinimumTemperature));
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
            }
        }

        private string _currentTemperatureDescription;
        public string CurrentTemperatureDescription
        {
            get => _currentTemperatureDescription;
            set
            {
                if (_currentTemperatureDescription == value)
                    return;

                _currentTemperatureDescription = value;
                RaisePropertyChanged(nameof(CurrentTemperatureDescription));
            }
        }

        private int _pressure;
        public int Pressure
        {
            get => _pressure;
            set
            {
                if (_pressure == value)
                    return;
                
                _pressure = value;
                RaisePropertyChanged(nameof(Pressure));
                RaisePropertyChanged(nameof(PressureLabel));
            }
        }
        
        private int _humidity;
        public int Humidity
        {
            get => _humidity;
            set
            {
                if (_humidity == value)
                    return;
                
                _humidity = value;
                RaisePropertyChanged(nameof(Humidity));
                RaisePropertyChanged(nameof(HumidityLabel));
            }
        }

        private int _cloud;
        public int Cloud
        {
            get => _cloud;
            set
            {
                if (_cloud == value)
                    return;
                
                _cloud = value;
                RaisePropertyChanged(nameof(Cloud));
                RaisePropertyChanged(nameof(CloudinessLabel));
            }
        }

        private double _windSpeed;
        public double WindSpeed
        {
            get => _windSpeed;
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_windSpeed == value)
                    return;
                
                _windSpeed = value;
                RaisePropertyChanged(nameof(WindSpeed));
                RaisePropertyChanged(nameof(WindSpeedLabel));
            }
        }

        public string CurrentDateLabel => DateTime.Now.ToString("dddd");
        
        public string CurrentWeatherForecast => MaximumTemperature + "/" + MinimumTemperature;

        public string SunriseLabel => Sunrise.ToString("t");
        public string SunsetLabel => Sunset.ToString("t");

        public string HumidityLabel => Humidity + "%";
        
        public string CloudinessLabel => Cloud + "%";

        public string PressureLabel => Pressure + " hPa";

        public string WindSpeedLabel
        {
            get
            {
                if (IsSettingCelsius)
                    return WindSpeed + " m/s";

                return WindSpeed + " mph";
            }
        }
        
        public string CurrentTemperatureLabel
        {
            get
            {
                if (IsSettingCelsius)
                    return Convert.ToInt32(CurrentTemperature) + "°C";
                
                return Convert.ToInt32(CurrentTemperature) + "°F";
            }
        }
        
        public MvxObservableCollection<DailyForecastViewModel> FiveDaysForecast { get; set; }
        
        public IMvxCommand GetWeatherCommand => new MvxAsyncCommand(GetWeather);
        public IMvxCommand GetLocationCommand => new MvxAsyncCommand(GetLocation);
        public IMvxCommand OpenSettingsCommand => new MvxAsyncCommand(OpenSettings);
        
        public HomeViewModel()
        {
            FiveDaysForecast = new MvxObservableCollection<DailyForecastViewModel>();
            
            _weatherService = Mvx.IoCProvider.Resolve<IWeatherService>();
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            await GetWeather();
        }

        private async Task GetWeather()
        {
            IsRefreshing = true;

            if (CityName != null)
            {
                IsCurrentLocation = false;
            }
            
            var result = await _weatherService.RetrieveWeatherAndForecast(IsSettingCelsius, CityName);

            if (result.Value == null)
            {
                if (result.ErrorMessage != null)
                {
                    await UserDialogs.Instance.AlertAsync(result.ErrorMessage);
                    IsRefreshing = false;
                    return;
                }

                await UserDialogs.Instance.AlertAsync("Something went wrong. Please try again.");
                IsRefreshing = false;
                return;
            }
            
            if (result.Value.CurrentWeather != null)
            {
                CurrentCityName = result.Value.CurrentWeather.Name;
                CountryName = result.Value.CurrentWeather.Sys.Country;
                CurrentTemperature = result.Value.CurrentWeather.Main.Temp;
                CurrentTemperatureDescription = result.Value.CurrentWeather.Weather[0].Main;
                MaximumTemperature = Convert.ToInt32(result.Value.CurrentWeather.Main.TempMax);
                MinimumTemperature = Convert.ToInt32(result.Value.CurrentWeather.Main.TempMin);
                CurrentCondition = result.Value.CurrentWeather.Weather[0].Main;
                
                WindSpeed = result.Value.CurrentWeather.Wind.Speed;
                Pressure = result.Value.CurrentWeather.Main.Pressure;
                Humidity = result.Value.CurrentWeather.Main.Humidity;
                Cloud = result.Value.CurrentWeather.Clouds.All;

                var timezone = result.Value.CurrentWeather.Timezone;
                var sunriseUnixTimestamp = result.Value.CurrentWeather.Sys.Sunrise + timezone;
                var sunsetUnixTimestamp = result.Value.CurrentWeather.Sys.Sunset + timezone;

                Sunrise = UnixTimeStampToDateTime(sunriseUnixTimestamp);
                Sunset = UnixTimeStampToDateTime(sunsetUnixTimestamp);
            }

            if (result.Value.Forecast != null)
            {
                List<DailyForecastViewModel> dailyForecastViewModels = ConvertDailyForecastsToViewModel(result.Value.Forecast.ForecastList);
                FiveDaysForecast.Refresh(dailyForecastViewModels);
            }

            IsRefreshing = false;
        }

        private async Task GetLocation()
        {
            var result = await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<LocationsViewModel, NavigationParameters>();

            if (result != null)
            {
                CityName = result.CityAndCountryName;
                await GetWeather();
            }
        }

        private async Task OpenSettings()
        {
            var parameters = new NavigationParameters
            {
                IsCelsius = IsSettingCelsius,
                IsCurrentLocationRequest = IsCurrentLocation
            };
            
            var result = await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<SettingsViewModel, NavigationParameters, NavigationParameters>(parameters);

            if (result != null)
            {
                IsSettingCelsius = result.IsCelsius;

                if (CityName != null && result.IsCurrentLocationRequest)
                {
                    IsCurrentLocation = result.IsCurrentLocationRequest;

                    CityName = null;
                }
                
                await GetWeather();
            }
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
                MaximumTemperature = Convert.ToInt32(forecast.Main.TempMax),
                MinimumTemperature = Convert.ToInt32(forecast.Main.TempMin),
                DayOfWeek = forecast.Date
            };

            return dailyForecast;
        }
        
        private static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }
    }
}
