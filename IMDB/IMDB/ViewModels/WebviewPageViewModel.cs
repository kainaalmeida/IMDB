using Prism.Navigation;
using Prism.Services;
using System;

namespace IMDB.ViewModels
{
    public class WebviewPageViewModel : ViewModelBase
    {
        private string _url;

        public string URL
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }
        private readonly Lazy<IPageDialogService> _pageDialogService;
        public WebviewPageViewModel
            (
                INavigationService navigationService,
                Lazy<IPageDialogService> pageDialogService
            ) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                URL = parameters.GetValue<string>(nameof(URL));

                if (string.IsNullOrEmpty(URL))
                {
                    await _pageDialogService.Value.DisplayAlertAsync("IMDb", "URL is empty", "OK");
                    await NavigationService.GoBackAsync();
                }
            }
        }
    }
}
