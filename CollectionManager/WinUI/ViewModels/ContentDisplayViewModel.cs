using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CollectionManager.WinUI.ViewModels;

public partial class ContentDisplayViewModel(SiteManager siteManager) : ObservableObject
{
    private GamePageDTO currentPage;
    public GamePageDTO CurrentPage
    {
        get
        {
            return currentPage;
        }
        set
        {
            IsCheckedAll(false);
            switch (value.MarkedType)
            {
                case MarkedType.Update:
                    {
                        IsUpdateButtonChecked = true;
                        break;
                    }
                case MarkedType.Marked:
                    {
                        IsMarkedButtonChecked = true;
                        break;
                    }
                case MarkedType.Seen:
                    {
                        IsSeenButtonChecked = true;
                        break;
                    }
                case MarkedType.EarlyAccess:
                    {
                        IsEralyAccesButtonChecked = true;
                        break;
                    }
            }
            SetProperty(ref currentPage, value);
        }
    }
    [ObservableProperty]
    private bool isUpdateButtonChecked;
    [ObservableProperty]
    private bool isMarkedButtonChecked;
    [ObservableProperty]
    private bool isSeenButtonChecked;
    [ObservableProperty]
    private bool isEralyAccesButtonChecked;


    [RelayCommand]
    private async Task AddToUpdateCollection()
    {
        await siteManager.AddToUpdateCollection(CurrentPage);
        IsCheckedAll(false);
        IsUpdateButtonChecked = true;
    }
    [RelayCommand]
    private async Task AddToMarkedCollection()
    {
        await siteManager.AddToMarkCollection(CurrentPage);
        IsCheckedAll(false);
        IsMarkedButtonChecked = true;
    }
    [RelayCommand]
    private async Task AddToSeenCollection()
    {
        await siteManager.AddToSeenCollection(CurrentPage);
        IsCheckedAll(false);
        IsSeenButtonChecked = true;

    }
    [RelayCommand]
    private async Task AddToEarlyAccessCollection()
    {
        await siteManager.AddToEarlyAccessCollection(CurrentPage);
        IsCheckedAll(false);
        IsEralyAccesButtonChecked = true;
    }



    private void IsCheckedAll(bool isChecked)
    {
        IsUpdateButtonChecked = isChecked;
        IsMarkedButtonChecked = isChecked;
        IsSeenButtonChecked = isChecked;
        IsEralyAccesButtonChecked = isChecked;
    }
}
