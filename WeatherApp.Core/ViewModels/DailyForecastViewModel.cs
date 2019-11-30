namespace WeatherApp.Core.ViewModels
{
    public class DailyForecastViewModel : BaseViewModel
    {
        private int _maximumTemperature;
        public int MaximumTemperature
        {
            get => _maximumTemperature;
            set
            {
                if (_maximumTemperature == value)
                    return;

                _maximumTemperature = value;
                RaisePropertyChanged(nameof(MaximumTemperature));
            }
        }
        
        private int _minimumTemperature;
        public int MinimumTemperature
        {
            get => _minimumTemperature;
            set
            {
                if (_minimumTemperature == value)
                    return;

                _minimumTemperature = value;
                RaisePropertyChanged(nameof(MaximumTemperature));
            }
        }
        
        private string _dayOfWeek;
        public string DayOfWeek
        {
            get => _dayOfWeek;
            set
            {
                if (_dayOfWeek == value)
                    return;

                _dayOfWeek = value;
                RaisePropertyChanged(nameof(DayOfWeek));
            }
        }
    }
}