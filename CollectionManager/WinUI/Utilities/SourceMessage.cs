using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
namespace CollectionManager.WinUI.Utilities;

public class IncrementalSourceMessage(ObservableCollection<PostDTO> incrementalSource)
    : ValueChangedMessage<ObservableCollection<PostDTO>>(incrementalSource)
{
}

public class CurrentPageSourceMessage(GamePageDTO currentPage)
    : ValueChangedMessage<GamePageDTO>(currentPage)
{
}

public class IsLoadingSourceMessage(bool Isloading)
    : ValueChangedMessage<bool>(Isloading)
{
}