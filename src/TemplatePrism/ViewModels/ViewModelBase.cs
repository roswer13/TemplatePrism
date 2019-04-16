using System;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism;
using Prism.AppModel;
using Prism.Navigation;
using Prism.Services;
using TemplatePrism.Exceptions;
using TemplatePrism.Resources;
using System.Diagnostics;

namespace TemplatePrism.ViewModels
{
    public class ViewModelBase : BindableBase, IActiveAware, INavigationAware, IDestructible, IConfirmNavigation, IConfirmNavigationAsync, IApplicationLifecycleAware, IPageLifecycleAware
    {
        protected IPageDialogService _pageDialogService { get; }

        protected IDeviceService _deviceService { get; }

        protected INavigationService _navigationService { get; }

        public ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService,
                             IDeviceService deviceService)
        {
            _pageDialogService = pageDialogService;
            _deviceService = deviceService;
            _navigationService = navigationService;
        }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Icon { get; set; }

        public bool IsBusy { get; set; }

        public bool IsNotBusy { get; set; }

        public bool CanLoadMore { get; set; }

        public string Header { get; set; }

        public string Footer { get; set; }

        private void OnIsBusyChanged() => IsNotBusy = !IsBusy;

        private void OnIsNotBusyChanged() => IsBusy = !IsNotBusy;

        #region IActiveAware

        public bool IsActive { get; set; }

        public event EventHandler IsActiveChanged;

        private void OnIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);

            if (IsActive)
            {
                OnIsActive();
            }
            else
            {
                OnIsNotActive();
            }
        }

        protected virtual void OnIsActive() { }

        protected virtual void OnIsNotActive() { }

        #endregion IActiveAware

        #region INavigationAware

        public virtual void OnNavigatingTo(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        #endregion INavigationAware

        #region IDestructible

        public virtual void Destroy() { }

        #endregion IDestructible

        #region IConfirmNavigation

        public virtual bool CanNavigate(INavigationParameters parameters) => true;

        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) =>
            Task.FromResult(CanNavigate(parameters));

        #endregion IConfirmNavigation

        #region IApplicationLifecycleAware

        public virtual void OnResume() { }

        public virtual void OnSleep() { }

        #endregion IApplicationLifecycleAware

        #region IPageLifecycleAware

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        #endregion IPageLifecycleAware

        public async Task RunSafe(Task task, bool ShowLoading = true, string loadingMessage = null)
        {
            try
            {
                IsBusy = ShowLoading;
                await task;
            }
            catch (ConnectivityException e)
            {
                //_userDialogs.HideLoading();
                Debug.WriteLine(e.ToString());
                //await _pageDialogService.DisplayAlertAsync(AppResources.AppName, AppResources.NoNetworkConnection, AppResources.Ok);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                //_userDialogs.HideLoading();
                Debug.WriteLine(ex.ToString());

                // Do not display error when task is canceled.
                // Ex.: Cancel get case detail task when user navigates back
                //      to a related case.
                //if (ex is TaskCanceledException)
                //    await _pageDialogService.DisplayAlertAsync(AppResources.TryAgain, AppResources.TimeoutReached, AppResources.Ok);

                //if (!(ex is OperationCanceledException))
                    //await _pageDialogService.DisplayAlertAsync(AppResources.AppName, $"{AppResources.AnErrorHasOcurred}: {ex.Message}", AppResources.Ok);

                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}