using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using WeatherApp.Core.ViewModels;

namespace WeatherApp.Android.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("weatherapp.android.fragments.DetailsFragment")]
    public class DetailsFragment : BaseFragment<DetailsViewModel>
    {
        protected override int FragmentId => Resource.Layout.detailsView;
    }
}
