using CollectionManager.Core.Models;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Collections;
using Microsoft.UI.Xaml.Controls;
namespace CollectionManager.WinUI.ViewModels;

public partial class GamelistedViewUserControlViewModel(INavigationService navigationService) : ObservableObject
{
    public IncrementalLoadingCollection<IIncrementalSource<GamePageDTO>, GamePageDTO>? GamePages { get; set; }

    [RelayCommand]
    public void ItemClick(ItemClickEventArgs e)
    {
        navigationService.NavigateTo(typeof(DisplayGameView).FullName!, (GamePageDTO)e.ClickedItem);
    }
}
