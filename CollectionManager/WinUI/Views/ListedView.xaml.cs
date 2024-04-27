using CollectionManager.Core.Models;
using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUI;
namespace CollectionManager.WinUI.Views;


public sealed partial class ListedView : Page
{
    public ListedViewModel ViewModel { get; private set; }
    public ListedView()
    {
        ViewModel = App.GetService<ListedViewModel>();
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        ViewModel.MarkedType = Enum.Parse<MarkedType>((string)e.Parameter);
        test.ItemsSource = ViewModel.GamePages;
    }
}
