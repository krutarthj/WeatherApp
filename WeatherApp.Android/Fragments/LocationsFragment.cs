using System;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using WeatherApp.Core.ViewModels;

namespace WeatherApp.Android.Fragments
{
    [MvxDialogFragmentPresentation]
    [Register("weatherapp.android.fragments.LocationsFragment")]
    public class LocationsFragment : BaseDialogFragment<LocationsViewModel>
    {
        private SearchView _searchView;
        private ImageView _closeButton;
        private MvxRecyclerView _locationsRecyclerView;
        private LinearLayout _rootView;
        
        protected override int FragmentId => Resource.Layout.locationsView;

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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _rootView = view.FindViewById<LinearLayout>(Resource.Id.rootView);
            _searchView = view.FindViewById<SearchView>(Resource.Id.searchLocation);
            _closeButton = view.FindViewById<ImageView>(Resource.Id.closeButton);
            _locationsRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.locations);

            SetSearchViewColors();
            
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            
            Dialog?.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
        }
        
        private void SetSearchViewColors()
        {
            int textViewId = _searchView.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
            var textView = _searchView.FindViewById<TextView>(textViewId);
            
            textView.SetTextColor(Color.White);
            
            textView.SetHintTextColor(Color.Gray);

            int searchPlateId = Context.Resources.GetIdentifier("android:id/search_plate", null, null);
            var searchPlate = _searchView.FindViewById<ViewGroup>(searchPlateId);
            
            searchPlate.Background.SetColorFilter(Color.Gray, PorterDuff.Mode.Multiply);

            int closeSearchButtonId = Context.Resources.GetIdentifier("android:id/search_close_btn", null, null);
            var closeSearchButton = _searchView.FindViewById<ImageView>(closeSearchButtonId);
            
            closeSearchButton.SetColorFilter(Color.Gray, PorterDuff.Mode.Multiply);
        }
    }
}
