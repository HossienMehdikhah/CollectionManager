using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
namespace CollectionManager.WinUI.ViewModels;

public partial class DisplayGameViewModel(SiteManager siteManager) : ObservableObject, INavigationAware
{
    [ObservableProperty]
    private bool progressRingIsActive = false;
    [ObservableProperty]
    private Visibility progressRingVisibility = Visibility.Collapsed;

    public void OnNavigatedFrom()
    {
    }
    public async void OnNavigatedTo(object parameter)
    {
        ActivateLoading();
        var currentPage = await siteManager.GetSpecificationPageAsync(((GamePageDTO)parameter).URL!);
        WeakReferenceMessenger.Default.Send(new CurrentPageSourceMessage(currentPage));
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
