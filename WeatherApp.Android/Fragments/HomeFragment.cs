using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using WeatherApp.Core.ViewModels;

namespace WeatherApp.Android.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("weatherapp.android.fragments.HomeFragment")]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        protected override int FragmentId => Resource.Layout.homeView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.forecast);
            recyclerView.NestedScrollingEnabled = false;

            return view;
        }
    }
}
