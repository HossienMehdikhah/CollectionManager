using CollectionManager.Core.Models;
using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUI;
namespace CollectionManager.WinUI.Views;


public sealed partial class DisplayGameView : Page
{
    private readonly DisplayGameViewModel viewModel;
    public DisplayGameView()
    {
        viewModel = App.GetService<DisplayGameViewModel>();
        
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        viewModel.SelectedItem = (GamePageDTO)e.Parameter;
        this.InitializeComponent();
    }
}
