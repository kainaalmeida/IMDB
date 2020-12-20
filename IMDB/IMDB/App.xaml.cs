using IMDB.Services.Contracts;
using IMDB.Services.Implementation;
using IMDB.ViewModels;
using IMDB.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace IMDB
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(ImdbTabbedPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<TrendingPage, TrendingPageViewModel>();
            containerRegistry.RegisterForNavigation<TopPage, TopPageViewModel>();
            containerRegistry.RegisterForNavigation<IncomingPage, IncomingPageViewModel>();
            containerRegistry.RegisterForNavigation<ImdbTabbedPage, ImdbTabbedPageViewModel>();

            //Services
            containerRegistry.Register<ITrendingRepository, TrendingRepository>();
        }
    }
}
