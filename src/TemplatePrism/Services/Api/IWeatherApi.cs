using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace TemplatePrism.Services.Api
{
    public interface IWeatherApi
    {
        [Get("/v1/current.json?key={token}&q=Zaragoza")]
        Task<HttpResponseMessage> GetWeather(string token);
    }
}
