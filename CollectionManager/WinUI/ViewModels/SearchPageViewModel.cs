using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.WinUI.ViewModels
{
    public class SearchPageViewModel(SiteManager siteManager) : ObservableObject
    {
        #region Property_Field
        private GamePageDTO currentPage = new();
        private bool progressRingIsActive = false;
        private Visibility progressRingVisibility = Visibility.Collapsed;
        #endregion

        public GamePageDTO CurrentPage
        {
            get => currentPage;
            set
            {
                SetProperty(ref currentPage, value);
            }
        }
        public bool ProgressRingIsActive
        {
            get => progressRingIsActive;
            private set
            {
                SetProperty(ref progressRingIsActive, value);
            }
        }
        public Visibility ProgressRingVisibility
        {
            get => progressRingVisibility;
            private set
            {
                SetProperty(ref progressRingVisibility, value);
            }
        }

        public async Task<IEnumerable<GamePageDTO>> GetSearchSuggestion(string searchTerm)
        {
            return await siteManager.GetSearchSuggestion(searchTerm);
        }

        public async Task Search(Uri uri)
        {
            var page = await siteManager.GetSpecificationPageAsync(uri);
            CurrentPage = page;
            ProgressRingIsActive = false;
            ProgressRingVisibility = Visibility.Collapsed;
        }
    }
}
