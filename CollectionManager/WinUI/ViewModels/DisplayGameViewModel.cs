using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
namespace CollectionManager.WinUI.ViewModels;

public partial class DisplayGameViewModel(SiteManager siteManager) : ObservableObject
{
    [ObservableProperty]
    private GamePageDTO currentPage = new();
    [ObservableProperty]
    private bool progressRingIsActive = false;
    [ObservableProperty]
    private Visibility progressRingVisibility = Visibility.Collapsed;

    public GamePageDTO SelectedItem { get; set; } = new();

    [RelayCommand]
    public async Task Loading()
    {
        ActivateLoading();
        CurrentPage = await siteManager.GetSpecificationPageAsync(SelectedItem.URL!);
        DeactivateLoading();
    }

    private void ActivateLoading()
    {
        ProgressRingIsActive = true;
        ProgressRingVisibility = Visibility.Visible;
    }
    private void DeactivateLoading()
    {
        ProgressRingIsActive = false;
        ProgressRingVisibility = Visibility.Collapsed;
    }
}
