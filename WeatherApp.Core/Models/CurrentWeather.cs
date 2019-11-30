using System.Collections.Generic;

namespace WeatherApp.Core.Models
{
    public class CurrentWeather
    {
        public List<Weather> Weather { get; set; }
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public Clouds Clouds { get; set; }
        public Sys Sys { get; set; }
        public string Name { get; set; }
        public int Timezone { get; set; }
    }
}