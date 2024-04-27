using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CollectionManager.WinUI.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private NavigationViewBackButtonVisible canGoBack = NavigationViewBackButtonVisible.Collapsed;
    [ObservableProperty]
    private object? selected;
    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }
    public INavigationService NavigationService
    {
        get;
    }
    public INavigationViewService NavigationViewService
    {
        get;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        //if (e.SourcePageType == typeof(SettingsPage))
        //{
        //    Selected = NavigationViewService.SettingsItem;
        //    return;
        //}
        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {            
            Selected = selectedItem;
        }
        CanGoBack = NavigationService.CanGoBack ? NavigationViewBackButtonVisible.Visible :
                    NavigationViewBackButtonVisible.Collapsed;
    }
}
