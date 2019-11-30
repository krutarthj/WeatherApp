using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.ViewModels;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Android.Support.V7.Widget;
using WeatherApp.Android.Activities;

namespace WeatherApp.Android.Fragments
{
    public abstract class BaseFragment<TViewModel> : MvxFragment where TViewModel : class, IMvxViewModel
    {
        protected Toolbar Toolbar { get; private set; }
        
        protected abstract int FragmentId { get; }

        public new TViewModel ViewModel
        {
            get { return (TViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }
        
        protected BaseFragment()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(FragmentId, null);

            Toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (Toolbar == null)
                return view;
            
            if (!(Activity is MainView mainActivity))
                return view;

            mainActivity.Title = "";
            mainActivity.SetSupportActionBar(Toolbar);

            return view;
        }
    }
}
