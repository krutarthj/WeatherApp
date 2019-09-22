using Android.App;
using Android.Content.PM;
using MvvmCross.Platforms.Android.Views;

namespace WeatherApp.Android.Activities
{
    [Activity(MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true, Theme = "@style/AppTheme")]
    public class SplashView : MvxSplashScreenActivity
    {
        public SplashView() : base(Resource.Layout.splashView)
        {
            
        }
    }
}
