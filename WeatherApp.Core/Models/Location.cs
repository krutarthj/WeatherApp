using Newtonsoft.Json;

namespace WeatherApp.Core.Models
{
    public class Location
    {
        [JsonProperty("_embedded")]
        public Embedded Embedded { get; set; }
        public int Count { get; set; }
    }
}