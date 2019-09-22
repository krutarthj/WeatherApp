using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace WeatherApp.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public void ShowMenu()
        {
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<MenuViewModel>();
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<HomeViewModel>();
        }
    }
}