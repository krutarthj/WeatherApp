using MvvmCross.IoC;
using MvvmCross.ViewModels;
using WeatherApp.Core.ViewModels;

namespace WeatherApp.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            
            RegisterAppStart<MainViewModel>();
        }
    }
}
