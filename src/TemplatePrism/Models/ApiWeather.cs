using System;
using Newtonsoft.Json;

namespace TemplatePrism.Models
{
    public class ApiWeather
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("current")]
        public Current Current { get; set; }
    }
}
