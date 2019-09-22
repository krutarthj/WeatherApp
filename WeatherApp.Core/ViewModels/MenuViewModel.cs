using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace WeatherApp.Core.ViewModels
{
    public class MenuViewModel : MvxViewModel
    {
        public IMvxCommand ShowFirstViewModelCommand => new MvxCommand(ShowFirstViewModel);
        public IMvxCommand ShowSecondViewModelCommand => new MvxCommand(ShowSecondViewModel);
        
        private void ShowFirstViewModel()
        {
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<HomeViewModel>();
        }

        private void ShowSecondViewModel()
        {
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<SettingsViewModel>();
        }
    }
}