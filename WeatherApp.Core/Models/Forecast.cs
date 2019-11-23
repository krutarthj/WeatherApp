using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.Core.Models
{
    public class Forecast
    {
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
        [JsonProperty("dt_txt")]
        public string Date { get; set; }
    }
}