using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using WeatherApp.Core.Models;
using WeatherApp.Core.Services;
using WeatherApp.Core.Services.Interfaces;
using WeatherApp.Core.Utilities;

namespace WeatherApp.Core.ViewModels
{
    public class LocationsViewModel : MvxViewModelResult<NavigationParameters>
    {
        private ILocationService _locationService;
        
        private string _citySearch;
        public string CitySearch
        {
            get => _citySearch;
            set
            {
                if (_citySearch == value)
                    return;
                
                _citySearch = value;
                RaisePropertyChanged(nameof(CitySearch));
                Task.Run(FilterCity);
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading == value)
                    return;

                _isLoading = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        private bool _isAlertVisible = true;
        public bool IsAlertVisible
        {
            get => _isAlertVisible;
            set
            {
                if (_isAlertVisible == value)
                    return;

                _isAlertVisible = value;
                RaisePropertyChanged(nameof(IsAlertVisible));
            }
        }

        public MvxObservableCollection<LocationViewModel> Locations { get; set; }

        public IMvxCommand SelectLocationCommand => new MvxAsyncCommand<LocationViewModel>(SelectLocation);
        public IMvxCommand CloseCommand => new MvxAsyncCommand(CloseAsync);
        
        public LocationsViewModel()
        {
            _locationService = new LocationService();
            
            Locations = new MvxObservableCollection<LocationViewModel>();
        }
        
        private async Task FilterCity()
        {
            IsLoading = true;
            
            if (CitySearch.Length == 0)
            {
                IsAlertVisible = true;
                Locations.Clear();
            }
            
            if (CitySearch.Length > 0)
            {
                IsAlertVisible = false;
                
                var result = await _locationService.GetLocationAsync(CitySearch);

                if (result.Value != null && result.Value.Embedded.CitySearchResults.Count > 0)
                {
                    List<LocationViewModel> locationsList = new List<LocationViewModel>(); 
                    foreach (var embeddedCitySearchResult in result.Value.Embedded.CitySearchResults)
                    {
                        var citySearchResult = new LocationViewModel
                        {
                            CityFullName = embeddedCitySearchResult.MatchingFullName
                        };
                    
                        locationsList.Add(citySearchResult);
                    }
                
                    Locations.Refresh(locationsList);
                }
            }

            IsLoading = false;
        }

        private async Task SelectLocation(LocationViewModel locationViewModel)
        {
            var fullName = locationViewModel.CityFullName.Split(',').ToList();
            var cityName = fullName[0];
            var countryName = fullName[2].TrimStart();

            if (countryName.Contains('('))
            {
                var splitName = countryName.Split('(').ToList();
                countryName = splitName[0].TrimEnd();
            }

            var result = await _locationService.GetCountryAsync(countryName);

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var country in result.Value)
                {
                    if (country.NativeName == countryName || country.Name == countryName)
                    {
                        var countryCode = country.Alpha2Code;
                        var cityNameAndCountryCode = cityName + "," + countryCode.ToLower();

                        var returnParameters = new NavigationParameters
                        {
                            CityName = cityName,
                            CountryName = countryName,
                            CityAndCountryName = cityNameAndCountryCode
                        };
                        
                        await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(this, returnParameters);
                        break;
                    }
                }
            }
        }

        private async Task CloseAsync()
        {
            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(this);
        }
    }
}
