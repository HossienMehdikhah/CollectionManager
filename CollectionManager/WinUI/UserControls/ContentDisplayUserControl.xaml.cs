using Microsoft.UI.Xaml.Controls;
using CollectionManager.WinUI.ViewModels;
using WinUI;
using CollectionManager.Core.Models;
namespace CollectionManager.WinUI.UserControls;

public sealed partial class ContentDisplayUserControl : UserControl
{
    private ContentDisplayViewModel ViewModel { get => (ContentDisplayViewModel)DataContext; }
    public ContentDisplayUserControl()
    {
        DataContext = App.GetServices<ContentDisplayViewModel>();
        this.InitializeComponent();
    }

    public GamePageDTO CurrentPage
    {
        get => ViewModel.CurrentPage;
        set => ViewModel.CurrentPage = value;
    }
}
