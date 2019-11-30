using MvvmCross.Plugin.Messenger;

namespace WeatherApp.Core.Messages
{
    public class WeatherDegreeMessage : MvxMessage
    {
        public bool IsCelsius { get; set; }
        
        public WeatherDegreeMessage(object sender, bool isCelsius) : base(sender)
        {
            IsCelsius = isCelsius;
        }
    }
}