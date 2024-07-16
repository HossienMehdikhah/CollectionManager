using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using WinUI;
namespace CollectionManager.WinUI.UserControls;

public sealed partial class LoadingUserControl : UserControl
{
    private LoadingUserControlViewModel ViewModel { get => (LoadingUserControlViewModel)DataContext; }
    public LoadingUserControl()
    {
        DataContext = App.GetService<LoadingUserControlViewModel>();
        InitializeComponent();
    }
}
