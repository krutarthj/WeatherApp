using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using WeatherApp.Android.Activities;

namespace WeatherApp.Android.Fragments
{
    public abstract class BaseDialogFragment<TViewModel> : MvxDialogFragment where TViewModel : class, IMvxViewModel
    {
        protected Toolbar Toolbar { get; private set; }
        
        protected abstract int FragmentId { get; }

        public new TViewModel ViewModel
        {
            get => base.ViewModel as TViewModel;
            set => base.ViewModel = value;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(FragmentId, null);

            Toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (Toolbar == null)
                return view;

            if (!(Activity is MainView mainView))
                return view;

            mainView.Title = "";
            mainView.SetSupportActionBar(Toolbar);

            return view;
        }
    }
}