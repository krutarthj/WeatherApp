using Newtonsoft.Json;

namespace WeatherApp.Core.Models
{
    public class CitySearchResult
    {
        [JsonProperty("matching_full_name")]
        public string MatchingFullName { get; set; }
    }
}