using Xamarin.Forms;

namespace IMDB.Views
{
    public partial class WebviewPage : ContentPage
    {
        public WebviewPage()
        {
            InitializeComponent();
        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Loading...");
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
        }
    }
}
