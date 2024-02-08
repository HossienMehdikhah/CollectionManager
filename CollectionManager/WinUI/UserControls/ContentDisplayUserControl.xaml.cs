using Microsoft.UI.Xaml.Controls;
using CollectionManager.WinUI.ViewModels;
using WinUI;
using CollectionManager.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CollectionManager.WinUI.UserControls
{
    public sealed partial class ContentDisplayUserControl : UserControl
    {
        private ContentDisplayViewModel viewModel;
        public ContentDisplayUserControl()
        {
            viewModel = App.GetServices<ContentDisplayViewModel>();
            this.InitializeComponent();
        }

        public GamePageDTO CurrentPage
        {
            get => viewModel.CurrentPage;
            set => viewModel.CurrentPage = value;
        }
    }
}
