using CollectionManager.WinUI.ViewModels;
using CollectionManager.WinUI.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
namespace WinUI;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private MainWindowViewModel viewModel;
    public MainWindow()
    {
        viewModel = App.GetServices<MainWindowViewModel>();
        this.InitializeComponent();
    }

    private void navView_Loaded(object sender, RoutedEventArgs e)
    {
        NavView.SelectedItem = NavView.MenuItems[0];
    }
    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer != null)
        {
            var namePageSelected = args.SelectedItemContainer.Tag.ToString();
            NavView_Navigate(namePageSelected);
        }
    }

    private void ContentFrame_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        NavView.SelectedItem = NavView.MenuItems
                        .OfType<NavigationViewItem>()
                        .First(i => i.Tag.Equals(ContentFrame.SourcePageType.Name));
    }

    private void NavView_Navigate(string selectedPage)
    {
        if (ContentFrame.CurrentSourcePageType is not null && ContentFrame.CurrentSourcePageType.Name.Equals(selectedPage))
            return;

        NavigationTransitionInfo navigationTransition = new EntranceNavigationTransitionInfo();
        if (ContentFrame.CurrentSourcePageType is not null)
        {
            var currentPage = NavView.MenuItems.Cast<NavigationViewItem>().First(x => x.Tag.Equals(ContentFrame.CurrentSourcePageType.Name));
            var currentPageIndex = NavView.MenuItems.IndexOf(currentPage);
            var nextPage = NavView.MenuItems.Cast<NavigationViewItem>().First(x => x.Tag.Equals(selectedPage));
            var nextPageIndex = NavView.MenuItems.IndexOf(nextPage);
            if (currentPageIndex < nextPageIndex)
                navigationTransition = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft };
            else navigationTransition = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight };
        }

        switch (selectedPage)
        {
            case nameof(MainPage):
                {
                    ContentFrame.Navigate(typeof(MainPage), null, navigationTransition);
                    break;
                }
            case nameof(SearchPage):
                {
                    ContentFrame.Navigate(typeof(SearchPage), null, navigationTransition);
                    break;
                }
        }
    }
}
