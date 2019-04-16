using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using TemplatePrism.Models;
using TemplatePrism.Resources;
using TemplatePrism.Services.Weather;

namespace TemplatePrism.ViewModels
{
    public class NewsPageViewModel : ViewModelBase
    {
        #region Constant Fields
        readonly IWeatherService _weatherService;
        #endregion

        #region Constructors
        public NewsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,
                                 IDeviceService deviceService, IWeatherService weatherService)
            : base(navigationService, pageDialogService, deviceService)
        {
            Title = AppResources.MainPageTitle;
            _weatherService = weatherService;
        }
        #endregion

        #region Lifecycle
        public async override void OnNavigatingTo(INavigationParameters parameters)
        {
            // TODO: Implement your initialization logic
            await RunSafe(RetrieveCasesData());
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            // TODO: Handle any final tasks before you navigate away
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    // TODO: Handle any tasks that should occur only when navigated back to
                    break;
                case NavigationMode.New:
                    // TODO: Handle any tasks that should occur only when navigated to for the first time
                    break;
            }

            // TODO: Handle any tasks that should be done every time OnNavigatedTo is triggered
        }
        #endregion

        #region Propertie
        public ApiWeather WeatherInfo { get; set; }
        public string Name { get; set; }
        public string TempC { get; set; }
        #endregion

        async Task RetrieveCasesData()
        {
            var weatherInfo = await _weatherService.GetWeather("c457cf88999b40f8b2772318191204");
            if (weatherInfo != null)
            {
                Name = weatherInfo.Location.Name;
                TempC = weatherInfo.Current.TempC + " C";
            }
            else
                await _pageDialogService.DisplayAlertAsync("Error", "Bad conection.", "OK");
        }
    }
}