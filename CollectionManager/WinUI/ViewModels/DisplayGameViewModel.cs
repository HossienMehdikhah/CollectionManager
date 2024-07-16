using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Singleton;
using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
namespace CollectionManager.WinUI.ViewModels;

public partial class DisplayGameViewModel(SiteManager siteManager, DisplayGamePageViewModelSingleton singleton) 
    : ObservableObject, INavigationAware
{
    public void OnNavigatedFrom()
    {
    }
    public async void OnNavigatedTo(object parameter)
    {
        var url = ((GamePageDTO)parameter).URL!;
        WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(true));
        GamePageDTO? currentPage = singleton.GetPage(url);
        if (currentPage is null)
        {
            currentPage = await siteManager.GetSpecificationPageAsync(url);
            singleton.SetPage(currentPage);
        }
        WeakReferenceMessenger.Default.Send(new CurrentPageSourceMessage(currentPage));
        WeakReferenceMessenger.Default.Send(new IsLoadingSourceMessage(false));
    }
}
