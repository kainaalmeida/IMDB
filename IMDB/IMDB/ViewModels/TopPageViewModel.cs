using Prism.Navigation;

namespace IMDB.ViewModels
{
    public class TopPageViewModel : ViewModelBase
    {
        public TopPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        protected override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();
        }
    }
}
