using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace CollectionManager.WinUI.ViewModels;

public partial class SearchPageViewModel(SiteManager siteManager) : ObservableObject
{
    private IEnumerable<GamePageDTO> _suggestedGames = [];
    [ObservableProperty]
    private GamePageDTO currentPage = new();
    [ObservableProperty]
    private bool progressRingIsActive = false;
    [ObservableProperty]
    private Visibility progressRingVisibility = Visibility.Collapsed;

    public string AutoSuggestionSelectedText { get; set; } = string.Empty;
    public ObservableCollection<string> ComboSuggestion = [];


    [RelayCommand]
    private async Task OnTextChanged(AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && !string.IsNullOrWhiteSpace(AutoSuggestionSelectedText))
        {
            ComboSuggestion.Clear();
            _suggestedGames = await siteManager.GetSearchSuggestion(AutoSuggestionSelectedText);

            if (_suggestedGames.Any())
                foreach (var item in _suggestedGames)
                    ComboSuggestion.Add(item.Name);
            else
                ComboSuggestion.Add("notfound");
        }
    }

    [RelayCommand]
    private async Task Search(AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (!string.IsNullOrWhiteSpace(args.QueryText) && !args.QueryText.Equals("notfound"))
        {
            ProgressRingIsActive = true;
            ProgressRingVisibility = Visibility.Visible;
            await Task.Delay(4000);
            var uri = _suggestedGames.FirstOrDefault(x => x.Name.Equals(args.QueryText)).URL;
            var page = await siteManager.GetSpecificationPageAsync(uri);
            CurrentPage = page;
            ProgressRingIsActive = false;
            ProgressRingVisibility = Visibility.Collapsed;
        }
    }
}
