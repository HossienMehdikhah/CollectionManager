using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
namespace CollectionManager.WinUI.ViewModels;

public partial class DisplayGameViewModel(SiteManager siteManager) : ObservableObject, INavigationAware
{
    public void OnNavigatedFrom()
    {
    }
    public async void OnNavigatedTo(object parameter)
    {
        WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(true));
        var currentPage = await siteManager.GetSpecificationPageAsync(((GamePageDTO)parameter).URL!);
        WeakReferenceMessenger.Default.Send(new CurrentPageSourceMessage(currentPage));
        WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(false));
    }
}
