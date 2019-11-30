using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.Core.Models
{
    public class Embedded
    {
        [JsonProperty("city:search-results")]
        public List<CitySearchResult> CitySearchResults { get; set; }
    }
}