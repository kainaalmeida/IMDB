using Acr.UserDialogs;
using Flurl.Http;
using IMDB.Extensions;
using IMDB.Models;
using IMDB.Services.Contracts;
using IMDB.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace IMDB.ViewModels
{
    public class TrendingPageViewModel : ViewModelBase
    {
        private readonly Lazy<ITrendingRepository> _trendingRepository;
        private readonly Lazy<IPageDialogService> _pageDialogService;

        public ObservableCollection<Result> ListMovies { get; } = new ObservableCollection<Result>();

        private Movies _movie;
        public Movies Movie
        {
            get { return _movie; }
            set { SetProperty(ref _movie, value); }
        }

        private Result _selectedItem;
        public Result SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
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

        private DelegateCommand<Result> _selectedItemCommand;
        public DelegateCommand<Result> SelectedItemCommand =>
            _selectedItemCommand ?? (_selectedItemCommand = new DelegateCommand<Result>(async (movie) => await ExecuteSelectedItemCommand(movie)));

        public TrendingPageViewModel
            (
                INavigationService navigationService,
                Lazy<ITrendingRepository> trendingRepository,
                Lazy<IPageDialogService> pageDialogService
            ) : base(navigationService)
        {
            _trendingRepository = trendingRepository;
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
                    var result = await _trendingRepository.Value.GetTrendingMoviesAsync(CurrentPage);

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

        private async Task ExecuteSelectedItemCommand(Result movie)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (movie is null) return;

                var navParam = new NavigationParameters();
                navParam.Add("Movie", movie);
                await NavigationService.NavigateAsync($"{nameof(DetailMoviePage)}", parameters: navParam);

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

        private async Task GetTrendingMoviesAsync()
        {
            try
            {
                using (var Dialog = UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    var result = await _trendingRepository.Value.GetTrendingMoviesAsync();
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                await GetTrendingMoviesAsync();
            }
        }
    }
}
