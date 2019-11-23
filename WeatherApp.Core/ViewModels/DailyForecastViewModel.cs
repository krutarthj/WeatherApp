using System;

namespace WeatherApp.Core.ViewModels
{
    public class DailyForecastViewModel : BaseViewModel
    {
        private double _maximumTemperature;
        public double MaximumTemperature
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
        
        private double _minimumTemperature;
        public double MinimumTemperature
        {
            get => _minimumTemperature;
            set
            {
                if (_minimumTemperature == value)
                    return;

                _minimumTemperature = value;
                RaisePropertyChanged(nameof(MaximumTemperature));
                RaisePropertyChanged(nameof(DailyForecastLabel));
            }
        }
        public string DailyForecastLabel => Convert.ToInt32(MaximumTemperature) + "/" + Convert.ToInt32(MinimumTemperature);
    }
}