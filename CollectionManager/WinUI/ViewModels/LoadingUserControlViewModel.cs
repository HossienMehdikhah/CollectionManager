using CollectionManager.WinUI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;

namespace CollectionManager.WinUI.ViewModels;

public partial class LoadingUserControlViewModel : ObservableObject
{
    [ObservableProperty]
    private bool progressRingIsActive = false;
    [ObservableProperty]
    private Visibility progressRingVisibility = Visibility.Visible;
    public LoadingUserControlViewModel()
    {
        WeakReferenceMessenger.Default.Register<IsLoadingSourceMessage>(this, (r, m) =>
        {
            ((LoadingUserControlViewModel)r).ProgressRingIsActive = m.Value;
            ((LoadingUserControlViewModel)r).ProgressRingVisibility = m.Value ? Visibility.Visible : Visibility.Collapsed;
        });
    }
}
