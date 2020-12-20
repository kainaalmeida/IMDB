using Acr.UserDialogs;
using Flurl.Http;
using IMDB.Models;
using IMDB.Services.Contracts;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;

namespace IMDB.ViewModels
{
    public class TrendingPageViewModel : ViewModelBase
    {
        private readonly Lazy<ITrendingRepository> _trendingRepository;
        private readonly Lazy<IPageDialogService> _pageDialogService;

        private Movies _movies;
        public Movies Movies
        {
            get { return _movies; }
            set { SetProperty(ref _movies, value); }
        }


        public TrendingPageViewModel
            (
                INavigationService navigationService,
                Lazy<ITrendingRepository> trendingRepository,
                Lazy<IPageDialogService> pageDialogService
            ) : base(navigationService)
        {
            _trendingRepository = trendingRepository;
            _pageDialogService = pageDialogService;
        }

        private async Task GetTrendingMoviesAsync()
        {
            try
            {
                using (var Dialog = UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    Movies = await _trendingRepository.Value.GetTrendingMoviesAsync();
                }
            }
            catch (FlurlHttpTimeoutException ex)
            {
                await _pageDialogService.Value.DisplayAlertAsync("IMDb", "Desculpe, mas o servidor não está respondendo.", "OK");
            }
            catch (Exception ex)
            {
                await _pageDialogService.Value.DisplayAlertAsync("IMDb", "Um erro aconteceu enquanto processava sua requisição", "OK");
            }
            finally
            {

            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                await GetTrendingMoviesAsync();
            }
        }
    }
}
