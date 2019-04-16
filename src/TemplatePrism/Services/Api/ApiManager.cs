using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fusillade;
using Polly;
using Refit;
using TemplatePrism.Exceptions;
using Xamarin.Essentials;

namespace TemplatePrism.Services.Api
{
    public class ApiManager : IApiManager
    {
        public NetworkAccess NetworkAccess { get; set; }

        readonly IApiService<IWeatherApi> weatherApi;
        public bool IsConnected { get; set; }
        public bool IsReachable { get; set; }

        Dictionary<int, CancellationTokenSource> runningTasks = new Dictionary<int, CancellationTokenSource>();
        Dictionary<string, Task<HttpResponseMessage>> taskContainer = new Dictionary<string, Task<HttpResponseMessage>>();

        public ApiManager(IApiService<IWeatherApi> _weatherApi)
        {
            weatherApi = _weatherApi;
            NetworkAccess = Connectivity.NetworkAccess;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            NetworkAccess = e.NetworkAccess;

            if (e.NetworkAccess == NetworkAccess.None)
            {
                // Cancel all running tasks
                var items = runningTasks.ToList();
                foreach (var item in items)
                {
                    item.Value.Cancel();
                    runningTasks.Remove(item.Key);
                }
            }
        }

        protected async Task<TData> RemoteRequestAsync<TData>(Task<TData> task)
            where TData : HttpResponseMessage,
            new()
        {
            var data = new TData();

            NetworkAccess = Connectivity.NetworkAccess;
            if (NetworkAccess == NetworkAccess.None)
            {
                /*
                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(AppResources.NoNetworkConnection);
                return data;
                */
                throw new ConnectivityException();
            }

            data = await Policy
            .Handle<WebException>()
            .Or<ApiException>()
            .Or<TaskCanceledException>()
            .OrResult<TData>(r => r.StatusCode == HttpStatusCode.Unauthorized)
            .WaitAndRetryAsync
            (
                retryCount: 0,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            )
            .ExecuteAsync(async () =>
            {
                var result = await task;

                runningTasks.Remove(task.Id);

                return result;
            });

            return data;
        }

        public async Task<HttpResponseMessage> GetWeatherAsync(string token)
        {
            var cts = new CancellationTokenSource();
            var task = RemoteRequestAsync<HttpResponseMessage>(weatherApi.GetApi(Priority.UserInitiated).GetWeather(token));
            runningTasks.Add(task.Id, cts);

            return await task;
        }
    }
}

