using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
