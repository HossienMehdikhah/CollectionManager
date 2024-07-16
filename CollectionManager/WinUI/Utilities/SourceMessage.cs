using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
namespace CollectionManager.WinUI.Utilities;

public class IncrementalSourceMessage(ObservableCollection<GamePageDTO> incrementalSource)
    : ValueChangedMessage<ObservableCollection<GamePageDTO>>(incrementalSource)
{
}

public class CurrentPageSourceMessage(GamePageDTO currentPage)
    : ValueChangedMessage<GamePageDTO>(currentPage)
{
}