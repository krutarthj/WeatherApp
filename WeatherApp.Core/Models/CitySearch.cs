using Newtonsoft.Json;

namespace WeatherApp.Core.Models
{
    public class CitySearch
    {
        [JsonProperty("_embedded")]
        public Embedded Embedded { get; set; }
        public int Count { get; set; }
    }
}