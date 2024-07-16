using CollectionManager.Core.Models;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Utilities;
using CollectionManager.WinUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
namespace CollectionManager.WinUI.ViewModels;

public partial class GamelistedUserControlViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public GamelistedUserControlViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        WeakReferenceMessenger.Default.Register<IncrementalSourceMessage>(this, (r, m) =>
        {
            ((GamelistedUserControlViewModel)r).GamePages = m.Value;
            OnPropertyChanged(nameof(GamePages));
        });
    }

    public ObservableCollection<GamePageDTO>? GamePages { get; set; }

    [RelayCommand]
    public void ItemClick(ItemClickEventArgs e)
    {
        _navigationService.NavigateTo(typeof(DisplayGamePage).FullName!, (GamePageDTO)e.ClickedItem);
    }
}
