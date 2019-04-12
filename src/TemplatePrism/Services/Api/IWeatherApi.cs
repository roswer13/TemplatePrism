using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace TemplatePrism.Services.Api
{
    [Headers("Content-Type: application/json")]
    public interface IWeatherApi
    {
        [Get("/v1/current.json?key=c457cf88999b40f8b2772318191204&q=Zaragoza")]
        Task<HttpResponseMessage> GetMakeUps(string brand);
    }
}
