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
        DataContext = App.GetServices<ContentDisplayViewModel>();
        ViewModel.ShowBiggerImage = async (Uri) =>
        {
            BiggerImageDialogimge.Source = new BitmapImage(Uri);
            await BiggerImageDialog.ShowAsync();
        };
        this.InitializeComponent();
    }
    public ContentDisplayViewModel ViewModel { get => (ContentDisplayViewModel)DataContext; }

    public GamePageDTO CurrentPage
    {
        get => ViewModel.CurrentPage;
        set => ViewModel.CurrentPage = value;
    }
}
