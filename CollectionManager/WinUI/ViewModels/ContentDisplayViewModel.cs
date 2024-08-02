using CollectionManager.Core;
using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace CollectionManager.WinUI.ViewModels;

public partial class ContentDisplayUserControlViewModel : ObservableObject
{
    private GamePageDTO currentPage = new();
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
                case MarkedType.Intresting:
                    {
                        IsIntrestingButtonChecked = true;
                        break;
                    }
                case MarkedType.Unintresting:
                    {
                        IsUnintrestingButtonChecked = true;
                        break;
                    }
                case MarkedType.EarlyAccess:
                    {
                        IsEralyAccesButtonChecked = true;
                        break;
                    }
                case MarkedType.Tested:
                    {
                        IsTestedButtonChecked = true;
                        break;
                    }
            }
            SetProperty(ref currentPage, value);
        }
    }

    [ObservableProperty]
    private bool isUpdateButtonChecked;
    [ObservableProperty]
    private bool isIntrestingButtonChecked;
    [ObservableProperty]
    private bool isUnintrestingButtonChecked;
    [ObservableProperty]
    private bool isEralyAccesButtonChecked;
    [ObservableProperty]
    private bool isTestedButtonChecked;
    private readonly SiteManager _siteManager;

    public ContentDisplayUserControlViewModel(SiteManager siteManager)
    {
        _siteManager = siteManager;
        WeakReferenceMessenger.Default.Register<CurrentPageSourceMessage>(this, (r, m) =>
        {
            CurrentPage = m.Value;
        });
    }

    [RelayCommand]
    private async Task AddToUpdateCollection()
    {
        await _siteManager.AddToUpdateCollection(CurrentPage);
        IsCheckedAll(false);
        IsUpdateButtonChecked = true;
    }
    [RelayCommand]
    private async Task AddToIntrestingCollection()
    {
        await _siteManager.AddToIntrestingCollection(CurrentPage);
        IsCheckedAll(false);
        IsIntrestingButtonChecked = true;
    }
    [RelayCommand]
    private async Task AddToUnintrestingCollection()
    {
        await _siteManager.AddToUnintrestingCollection(CurrentPage);
        IsCheckedAll(false);
        IsUnintrestingButtonChecked = true;

    }
    [RelayCommand]
    private async Task AddToEarlyAccessCollection()
    {
        await _siteManager.AddToEarlyAccessCollection(CurrentPage);
        IsCheckedAll(false);
        IsEralyAccesButtonChecked = true;
    }
    [RelayCommand]
    private async Task AddToTestedCollection()
    {
        await _siteManager.AddToTesedCollection(CurrentPage);
        IsCheckedAll(false);
        IsTestedButtonChecked = true;
    }
    [RelayCommand]
    private async Task ShowImageAsBiggerSize(string selectedImageUri)
    {
        var uri = new Uri(selectedImageUri);
        await ShowImageAsBiggerSizeAction.Invoke(uri);
    }
    [RelayCommand]
    private async Task ShowDownloadSelectorDialog()
    {
        await ShowDownloadSelectorAction.Invoke();
    }
    [RelayCommand]
    private void TreeViewSelectionChange(TreeViewSelectionChangedEventArgs args)
    {
        TreeViewSelectionChangeAction.Invoke(args);
    }
    [RelayCommand]
    private void DownloadLinkSelectionConfirm(TreeView item)
    {
        var temp = item.SelectedItems.Where(x => x is DownloadURIDTO).Cast<DownloadURIDTO>().Select(x => x.Uri).ToList();
        Broker.AddToIDMDownLoadList(temp);
    }


    public Func<Uri, Task> ShowImageAsBiggerSizeAction { get; set; } = async (uri) => await Task.CompletedTask;
    public Func<Task> ShowDownloadSelectorAction { get; set; }
    public Action DownloadLinkSelectionConfirmAction { get; set; }
    public Action<TreeViewSelectionChangedEventArgs> TreeViewSelectionChangeAction { get; set; }
    public void ShowDownloadLink_SelectionChanged(TreeView sender, TreeViewSelectionChangedEventArgs args)
    {
        if (args.RemovedItems.Any())
            sender.RootNodes.ToList().ForEach(x => x.IsExpanded = false);
        else
        {
            var temp = sender.RootNodes.Where(x => !args.AddedItems.Where(y => y is EncoderTeamDto).Cast<EncoderTeamDto>().Any(y => ((EncoderTeamDto)x.Content).EncoderName.Equals(y.EncoderName)))
                .ToList();
            temp.ForEach(x => x.IsExpanded = false);
            if (sender.SelectedItems.Where(x => x is EncoderTeamDto).Count() >= 2)
                sender.SelectedNodes.RemoveAt(0);
            var temp1 = sender.RootNodes
                .First(x => ((EncoderTeamDto)x.Content).EncoderName.Equals(((EncoderTeamDto)args.AddedItems.First(y => y is EncoderTeamDto)).EncoderName));
            temp1.IsExpanded = true;
        }
    }
    private void IsCheckedAll(bool isChecked)
    {
        IsUpdateButtonChecked = isChecked;
        IsIntrestingButtonChecked = isChecked;
        IsUnintrestingButtonChecked = isChecked;
        IsEralyAccesButtonChecked = isChecked;
        IsTestedButtonChecked = isChecked;
    }
}
