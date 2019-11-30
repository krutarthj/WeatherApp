using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace WeatherApp.Core
{
    public class AppStart<TViewModel> : MvxAppStart<TViewModel> where TViewModel : IMvxViewModel
    {
        public AppStart(IMvxApplication application, IMvxNavigationService navigationService) : base(application, navigationService)
        {
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            await NavigationService.Navigate<TViewModel>();
        }
    }
}