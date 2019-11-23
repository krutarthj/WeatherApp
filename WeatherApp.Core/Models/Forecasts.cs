using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.Core.Models
{
    public class Forecasts
    {
        [JsonProperty("list")]
        public List<Forecast> ForecastList { get; set; }
        public Sys City { get; set; }
    }
}