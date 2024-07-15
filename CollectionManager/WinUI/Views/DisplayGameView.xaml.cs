using CollectionManager.Core.Models;
using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUI;
namespace CollectionManager.WinUI.Views;


public sealed partial class DisplayGameView : Page
{
    public DisplayGameViewModel ViewModel {  get; set; }
    public DisplayGameView()
    {
        ViewModel = App.GetService<DisplayGameViewModel>();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        ViewModel.SelectedItem = (GamePageDTO)e.Parameter;
        InitializeComponent();
    }
}
