using System;
using System.Threading.Tasks;
using TemplatePrism.Models;

namespace TemplatePrism.Services.Weather
{
    public interface IWeatherService
    {
        Task<ApiWeather> GetWeather(string token);
    }
}
