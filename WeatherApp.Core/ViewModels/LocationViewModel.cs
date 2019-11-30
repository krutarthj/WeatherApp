namespace WeatherApp.Core.ViewModels
{
    public class LocationViewModel : BaseViewModel
    {
        private string _cityFullName;
        public string CityFullName
        {
            get => _cityFullName;
            set
            {
                if (_cityFullName == value)
                    return;

                _cityFullName = value;
                RaisePropertyChanged(nameof(CityFullName));
            }
        }
    }
}