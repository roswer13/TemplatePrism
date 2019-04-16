using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TemplatePrism.Models;
using TemplatePrism.Services.Api;

namespace TemplatePrism.Services.Weather
{
    public class WeatherService : IWeatherService
    {
        readonly IApiManager _apiManager;

        public WeatherService(IApiManager apiManager)
        {
            _apiManager = apiManager;
        }

        public async Task<ApiWeather> GetWeather(string token)
        {
            var httpResponse = await _apiManager.GetWeatherAsync(token);
            if (httpResponse.IsSuccessStatusCode)
            {
                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                var dataResponse = JsonConvert.DeserializeObject<ApiWeather>(jsonString);

                if (dataResponse != null)
                    return dataResponse;
            }

            return null;
        }
    }
}
