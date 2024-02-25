using CollectionManager.WinUI.ViewModels;
using CollectionManager.WinUI.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using WinUIEx;
namespace WinUI;


public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();
        Content = null;
    }
}
