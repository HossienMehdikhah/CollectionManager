using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    public MainViewModel()
    {
    }
    public IList<Uri> GaleryLinkList
    {
        get ;
    }
}
