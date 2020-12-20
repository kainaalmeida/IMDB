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
    public class DetailMoviePageViewModel : ViewModelBase
    {

        private readonly Lazy<IDetailMovieRepository> _detailMovieRepository;
        private readonly Lazy<IPageDialogService> _pageDialogService;

        private Result _movie;
        public Result Movie
        {
            get { return _movie; }
            set { SetProperty(ref _movie, value); }
        }

        private MovieDetail _movieDetail;
        public MovieDetail MovieDetail
        {
            get { return _movieDetail; }
            set { SetProperty(ref _movieDetail, value); }
        }

        public DetailMoviePageViewModel
            (
                INavigationService navigationService,
                Lazy<IDetailMovieRepository> detailMovieRepository,
                Lazy<IPageDialogService> pageDialogService
            ) : base(navigationService)
        {
            _detailMovieRepository = detailMovieRepository;
            _pageDialogService = pageDialogService;
        }

        private async Task GetMovieDetailAsync(int id)
        {
            try
            {
                using (var Dialog = UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    MovieDetail = await _detailMovieRepository.Value.GetMovieDetailAsync(id);
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
            base.OnNavigatedTo(parameters);
            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                Movie = parameters.GetValue<Result>(nameof(Movie));

                if (!(Movie is null))
                    await GetMovieDetailAsync(Movie.id);
            }
        }
    }
}
