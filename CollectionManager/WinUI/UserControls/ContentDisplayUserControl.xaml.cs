using Microsoft.UI.Xaml.Controls;
using CollectionManager.WinUI.ViewModels;
using WinUI;
using CollectionManager.Core.Models;
using Microsoft.UI.Xaml.Media.Imaging;
namespace CollectionManager.WinUI.UserControls;

public sealed partial class ContentDisplayUserControl : UserControl
{
    public ContentDisplayUserControl()
    {
        DataContext = App.GetService<ContentDisplayUserControlViewModel>();
        ViewModel.ShowImageAsBiggerSizeAction = async (Uri) =>
        {
            BiggerImageDialogimge.Source = new BitmapImage(Uri);
            await BiggerImageDialog.ShowAsync();
        };
        ViewModel.ShowDownloadSelectorAction = async () =>
        {
            showDownloadLink.ItemsSource = ViewModel.CurrentPage.DownloadLink;
            await DownloadEncoderSelectorDialog.ShowAsync();
        };
        ViewModel.TreeViewSelectionChangeAction = (args) =>
        {
            ViewModel.ShowDownloadLink_SelectionChanged(showDownloadLink, args);
        };
        ViewModel.DownloadLinkSelectionConfirmAction = () =>
        {
            showDownloadLink.SelectedItems.Where(x => x is DownloadURIDTO);
        };
        InitializeComponent();
    }
    public ContentDisplayUserControlViewModel ViewModel { get => (ContentDisplayUserControlViewModel)DataContext; }
}
