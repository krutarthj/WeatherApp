using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using WeatherApp.Core.ViewModels;

namespace WeatherApp.Android.Fragments
{
    [MvxDialogFragmentPresentation]
    [Register("weatherapp.android.fragments.SettingsFragment")]
    public class SettingsFragment : BaseDialogFragment<SettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.settingsView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetStyle(StyleNormal, Resource.Style.FullScreenDialogTheme);
        }
        
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            
            Dialog.Window.SetWindowAnimations(Resource.Style.DialogAnimation);
        }
        
        public override void OnStart()
        {
            base.OnStart();
            
            Dialog?.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
        }
    }
}
