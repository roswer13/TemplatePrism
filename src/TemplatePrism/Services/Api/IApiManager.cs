using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TemplatePrism.Services.Api
{
    public interface IApiManager
    {
        Task<HttpResponseMessage> GetWeatherAsync(string token);
    }
}
