using CollectionManager.Core.Models;
using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUI;
namespace CollectionManager.WinUI.Views;


public sealed partial class ListedView : Page
{
    private readonly ListedViewModel viewModel;
    public ListedView()
    {
        viewModel = App.GetService<ListedViewModel>();
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        viewModel.MarkedType = Enum.Parse<MarkedType>((string)e.Parameter);
    }
}
