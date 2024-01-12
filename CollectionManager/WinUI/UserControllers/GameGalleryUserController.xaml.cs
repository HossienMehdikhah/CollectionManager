using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI.UserControllers
{
    public sealed partial class GameGalleryUserController : UserControl
    {
        public GameGalleryUserController()
        {
            this.InitializeComponent();
        }
        public IList<Uri> GameLinkList
        {
            get => (IList<Uri>)GetValue(GameLinkListProperty);
            set => SetValue(GameLinkListProperty, value ?? new List<Uri>());
        }

        public static readonly DependencyProperty GameLinkListProperty =
            DependencyProperty.Register("GameLinkList",
                typeof(IList<Uri>),
                typeof(GameGalleryUserController),
                new PropertyMetadata(new List<Uri>(), OnGameLinkListChanged));

        private static void OnGameLinkListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dp = (GameGalleryUserController)d;
            for (var i = 0; i < dp.GameLinkList.Count; i++)
            {
                var uri = dp.GameLinkList.ElementAt(i);
                if (uri.ToString().Contains("mp4"))
                {
                    uri = new Uri("/Resources/PlayIcon.png", UriKind.Relative);
                }
            }
        }
    }
}
