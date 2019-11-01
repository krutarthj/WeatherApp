using System;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using WeatherApp.Core.Services.Interfaces;
using Xamarin.Essentials;

namespace WeatherApp.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IWeatherService _weatherService;

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

        public string CurrentTemperatureLabel
        {
            get
            {
                if(IsSettingCelsius)
                    return Convert.ToInt32(CurrentTemperature).ToString() + "°C";
                
                return Convert.ToInt32(CurrentTemperature).ToString() + "°F";
            }
        }
        
        public IMvxCommand GetWeatherCommand => new MvxAsyncCommand(GetWeather);
        
        public HomeViewModel()
        {
            _weatherService = Mvx.IoCProvider.Resolve<IWeatherService>();
        }

        private async Task GetWeather()
        {
            var result = await _weatherService.RetrieveCurrentWeatherAsync(IsCurrentLocation, CityName);

            if (IsSettingCelsius)
                CurrentTemperature = result.Value.Temperature.Metric.Value;
            else
                CurrentTemperature = result.Value.Temperature.Imperial.Value;
        }
    }
}
