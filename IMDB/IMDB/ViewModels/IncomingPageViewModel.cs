using Prism.Navigation;

namespace IMDB.ViewModels
{
    public class IncomingPageViewModel : ViewModelBase
    {
        public IncomingPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        protected override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();
        }
    }
}
