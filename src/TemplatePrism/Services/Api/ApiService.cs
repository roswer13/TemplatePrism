using System;
using System.Net.Http;
using Fusillade;
using ModernHttpClient;
using Refit;

namespace TemplatePrism.Services.Api
{
    public class ApiService<T> : IApiService<T>
    {
        Func<HttpMessageHandler, T> createClient;

        public ApiService(string apiBaseAddress)
        {
            createClient = messageHandler =>
            {
                HttpClient client;

                client = new HttpClient(new HttpLoggingHandler(messageHandler))
                {
                    BaseAddress = new Uri(apiBaseAddress)
                };

                return RestService.For<T>(client);
            };
        }

        T Background => new Lazy<T>(() => createClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Background))).Value;
        T UserInitiated => new Lazy<T>(() => createClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.UserInitiated))).Value;
        T Speculative => new Lazy<T>(() => createClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Speculative))).Value;

        public T GetApi(Priority priority)
        {
            switch (priority)
            {
                case Priority.Background:
                    return Background;
                case Priority.UserInitiated:
                    return UserInitiated;
                case Priority.Speculative:
                    return Speculative;
                default:
                    return UserInitiated;
            }
        }
    }
}

