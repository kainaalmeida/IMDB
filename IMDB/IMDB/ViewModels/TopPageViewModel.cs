using Acr.UserDialogs;
using Flurl.Http;
using IMDB.Extensions;
using IMDB.Models;
using IMDB.Services.Contracts;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace IMDB.ViewModels
{
    public class TopPageViewModel : ViewModelBase
    {
        private readonly Lazy<ITopRatingRepository> _topRatingRepository;
        private readonly Lazy<IPageDialogService> _pageDialogService;

        public ObservableCollection<Result> ListMovies { get; } = new ObservableCollection<Result>();

        private Movies _movie;
        public Movies Movie
        {
            get { return _movie; }
            set { SetProperty(ref _movie, value); }
        }


        private int _itemTreshold;
        public int ItemTreshold
        {
            get { return _itemTreshold; }
            set { SetProperty(ref _itemTreshold, value); }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty(ref _currentPage, value); }
        }

        private DelegateCommand _itemTresholdReachedCommand;
        public DelegateCommand ItemTresholdReachedCommand =>
            _itemTresholdReachedCommand ?? (_itemTresholdReachedCommand = new DelegateCommand(async () => await ExecuteItemTresholdReachedCommand()));
        public TopPageViewModel
            (
                INavigationService navigationService,
                Lazy<ITopRatingRepository> topRatingRepository,
                Lazy<IPageDialogService> pageDialogService

            ) : base(navigationService)
        {
            _topRatingRepository = topRatingRepository;
            _pageDialogService = pageDialogService;
            ItemTreshold = 1;
        }

        private async Task ExecuteItemTresholdReachedCommand()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                using (var Dialog = UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    CurrentPage = Movie.page += 1;
                    var result = await _topRatingRepository.Value.GetTopRatingMoviesAsync(CurrentPage);

                    if (result.results != null)
                    {
                        ListMovies.AddRange(result.results);
                        Movie = result;
                    }

                    if (Movie.page == Movie.total_pages)
                    {
                        ItemTreshold = -1;
                        return;
                    }
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
                IsBusy = false;
            }
        }

        private async Task GetTopRatingMoviesAsync()
        {
            try
            {
                using (var Dialog = UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    var result = await _topRatingRepository.Value.GetTopRatingMoviesAsync();
                    if (result.results != null)
                    {
                        ListMovies.AddRange(result.results);
                        Movie = result;
                    }
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
        protected override async void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();

            if (IsActive)
                await GetTopRatingMoviesAsync();
        }
    }
}
