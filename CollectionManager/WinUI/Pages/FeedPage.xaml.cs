using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CollectionManager.WinUI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedPage : Page
    {
        private readonly FeedPageViewModel viewModel;
        public FeedPage()
        {
            this.Unloaded += MainPage_Unloaded;
            viewModel = App.GetService<FeedPageViewModel>();
            this.InitializeComponent();
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
           viewModel.CancellationToken = new CancellationToken(true);
        }
    }
}
