using System;
using Newtonsoft.Json;

namespace TemplatePrism.Models
{
    public class Current
    {
        [JsonProperty("last_updated_epoch")]
        public int LastUpdatedEpoch { get; set; }

        [JsonProperty("last_updated")]
        public string LastUpdated { get; set; }

        [JsonProperty("temp_c")]
        public int TempC { get; set; }

        [JsonProperty("temp_f")]
        public int TempF { get; set; }

        [JsonProperty("is_day")]
        public int IsDay { get; set; }

        [JsonProperty("condition")]
        public Condition Condition { get; set; }

        [JsonProperty("wind_mph")]
        public double WindMph { get; set; }

        [JsonProperty("wind_kph")]
        public int WindKph { get; set; }

        [JsonProperty("wind_degree")]
        public int WindDegree { get; set; }

        [JsonProperty("wind_dir")]
        public string WindDir { get; set; }

        [JsonProperty("pressure_mb")]
        public int PressureMb { get; set; }

        [JsonProperty("pressure_in")]
        public double PressureIn { get; set; }

        [JsonProperty("precip_mm")]
        public int PrecipMm { get; set; }

        [JsonProperty("precip_in")]
        public int PrecipIn { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("cloud")]
        public int Cloud { get; set; }

        [JsonProperty("feelslike_c")]
        public double FeelslikeC { get; set; }

        [JsonProperty("feelslike_f")]
        public double FeelslikeF { get; set; }

        [JsonProperty("vis_km")]
        public int VisKm { get; set; }

        [JsonProperty("vis_miles")]
        public int VisMiles { get; set; }

        [JsonProperty("uv")]
        public int Uv { get; set; }

        [JsonProperty("gust_mph")]
        public double GustMph { get; set; }

        [JsonProperty("gust_kph")]
        public double GustKph { get; set; }
    }
}
