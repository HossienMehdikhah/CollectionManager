using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;

namespace CollectionManager.WinUI.ViewModels;

public class ContentDisplayViewModel: ObservableObject
{
    private GamePageDTO currentPage;
    public GamePageDTO CurrentPage
    {
        get => currentPage;
        set
        {
            SetProperty(ref currentPage, value);
        }
    }
}
