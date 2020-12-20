using IMDB.Models;
using Prism.Navigation;

namespace IMDB.ViewModels
{
    public class DetailMoviePageViewModel : ViewModelBase
    {

        private Result _movie;
        public Result Movie
        {
            get { return _movie; }
            set { SetProperty(ref _movie, value); }
        }


        public DetailMoviePageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                Movie = parameters.GetValue<Result>(nameof(Movie));
            }

        }
    }
}
