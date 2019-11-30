using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using WeatherApp.Core.Models;

namespace WeatherApp.Core.ViewModels
{
    public class SettingsViewModel : MvxViewModel<NavigationParameters, NavigationParameters>
    {
        private bool _isCelsius;
        public bool IsCelsius
        {
            get => _isCelsius;
            set
            {
                if (_isCelsius == value)
                    return;
                
                _isCelsius = value;
                RaisePropertyChanged(nameof(IsCelsius));
            }
        }
        
        private bool _isCurrentLocationRequested;
        public bool IsCurrentLocationRequested
        {
            get => _isCurrentLocationRequested;
            set
            {
                if (_isCurrentLocationRequested == value)
                    return;
                
                _isCurrentLocationRequested = value;
                RaisePropertyChanged(nameof(IsCurrentLocationRequested));
            }
        }
        
        private bool _isCurrentLocation;
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
        
        public IMvxCommand SaveSettingsCommand => new MvxAsyncCommand(SaveSettings);
        public IMvxCommand CloseCommand => new MvxAsyncCommand(CloseAsync);
        public override void Prepare(NavigationParameters parameter)
        {
            IsCelsius = parameter.IsCelsius;
            IsCurrentLocationRequested = parameter.IsCurrentLocationRequest;
            IsCurrentLocation = parameter.IsCurrentLocationRequest;
        }

        private async Task SaveSettings()
        {
            var returnParameters = new NavigationParameters
            {
                IsCelsius = IsCelsius,
                IsCurrentLocationRequest = IsCurrentLocationRequested
            };

            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(this, returnParameters);
        }

        private async Task CloseAsync()
        {
            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(this);
        }
    }
}
