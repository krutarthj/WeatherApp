using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Views.InputMethods;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using WeatherApp.Core.ViewModels;

namespace WeatherApp.Android.Activities
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainView : MvxAppCompatActivity<MainViewModel>
    {
        public DrawerLayout DrawerLayout;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.main);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            
            if(bundle == null)
                ViewModel.ShowMenu();
            
            Xamarin.Essentials.Platform.Init(this, bundle);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    if (DrawerLayout.GetDrawerLockMode(GravityCompat.Start) == DrawerLayout.LockModeLockedClosed)
                    {
                        HideSoftKeyboard();
                    }
                    else
                    {
                        DrawerLayout.OpenDrawer(GravityCompat.Start);
                    }

                    return true;
            }
            
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            if(DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
                DrawerLayout.CloseDrawers();
            else
                base.OnBackPressed();
        }

        public void HideSoftKeyboard()
        {
            if (CurrentFocus == null)
                return;

            var inputMethodManager = (InputMethodManager) GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            
            CurrentFocus.ClearFocus();
        }
    }
}
