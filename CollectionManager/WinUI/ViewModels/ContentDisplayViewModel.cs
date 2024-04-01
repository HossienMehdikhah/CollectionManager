using CollectionManager.Core;
using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;

namespace CollectionManager.WinUI.ViewModels;

public partial class ContentDisplayViewModel(SiteManager siteManager) : ObservableObject
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
    [RelayCommand]
    private async Task ShowImageAsBiggerSize(string selectedImageUri)
    {
        await ShowImageAsBiggerSizeAction.Invoke(new Uri(selectedImageUri));
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
    private async Task DownloadLinkSelectionConfirm(TreeView item)
    {
        var temp = item.SelectedItems.Where(x => x is DownloadURIDTO).Cast<DownloadURIDTO>().Select(x=>x.Uri).ToList();
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
        IsMarkedButtonChecked = isChecked;
        IsSeenButtonChecked = isChecked;
        IsEralyAccesButtonChecked = isChecked;
    }
}
