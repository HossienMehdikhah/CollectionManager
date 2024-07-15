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
    [ObservableProperty]
    private GamePageDTO currentPage = new();
    [ObservableProperty]
    private bool progressRingIsActive = false;
    [ObservableProperty]
    private Visibility progressRingVisibility = Visibility.Collapsed;

   
    [RelayCommand]
    private async Task Search(string searchQuery)
    {
        //if (!string.IsNullOrWhiteSpace(searchQuery)
        //    && !searchQuery.Equals("notfound"))
        //{
        //    ProgressRingIsActive = true;
        //    ProgressRingVisibility = Visibility.Visible;
        //    GamePageDTO? itemfounded = null;
        //    if (ComboSuggestion.Any())
        //        itemfounded = ComboSuggestion.FirstOrDefault(x => x.Name.Equals(searchQuery));
        //    else
        //    {
        //        CancellationToken cancellationToken = CancellationToken.None;
        //        await foreach (var item in siteManager.GetSearchSuggestion(AutoSuggestionSelectedText,
        //            cancellationToken))
        //        {
        //            itemfounded = item;
        //            cancellationToken = new CancellationToken(true);
        //            break;
        //        }
        //    }

        //    if (itemfounded != null)
        //    {
        //        var page = await siteManager.GetSpecificationPageAsync(itemfounded.URL!);
        //        CurrentPage = page;
        //    }

        //    ProgressRingIsActive = false;
        //    ProgressRingVisibility = Visibility.Collapsed;
        //}
        await Task.CompletedTask;
    }
}
