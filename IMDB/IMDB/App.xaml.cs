using IMDB.Services.Contracts;
using IMDB.Services.Implementation;
using IMDB.ViewModels;
using IMDB.Views;
using MonkeyCache.LiteDB;
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

            Barrel.ApplicationId = "IMDb";

            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(ImdbTabbedPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<TrendingPage, TrendingPageViewModel>();
            containerRegistry.RegisterForNavigation<TopPage, TopPageViewModel>();
            containerRegistry.RegisterForNavigation<ImdbTabbedPage, ImdbTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<UpcomingPage, UpcomingPageViewModel>();
            containerRegistry.RegisterForNavigation<DetailMoviePage, DetailMoviePageViewModel>();

            //Services
            containerRegistry.Register<ITrendingRepository, TrendingRepository>();
            containerRegistry.Register<ITopRatingRepository, TopRatingRepository>();
            containerRegistry.Register<IIncomingRepository, IncomingRepository>();
            containerRegistry.Register<IDetailMovieRepository, DetailMovieRepository>();
            containerRegistry.RegisterForNavigation<WebviewPage, WebviewPageViewModel>();
        }
    }
}
