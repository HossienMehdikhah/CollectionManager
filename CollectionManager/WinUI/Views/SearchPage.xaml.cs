using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using CollectionManager.WinUI.ViewModels;
using WinUI;
using CollectionManager.Core.Models;

namespace CollectionManager.WinUI.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private readonly SearchPageViewModel viewModel;
        public SearchPage()
        {
            viewModel = App.GetServices<SearchPageViewModel>();
            this.InitializeComponent();
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && !string.IsNullOrWhiteSpace(sender.Text))
            {
                var result = await viewModel.GetSearchSuggestion(sender.Text);
                sender.ItemsSource = result.ToList();
            }
        }

        private async void SearchComboBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
             await viewModel.Search(((GamePageDTO)args.ChosenSuggestion).URL);
        }
    }
}
