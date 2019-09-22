using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;
using WeatherApp.Core;

namespace WeatherApp.Android
{
    public class Setup : MvxAppCompatSetup<App>
    {
        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            //Register Custom Bindings Here
        }

        protected override void InitializeLastChance()
        {
            //Register Platform-Specific Services Here

            base.InitializeLastChance();
        }

        public override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.Console;
    }
}