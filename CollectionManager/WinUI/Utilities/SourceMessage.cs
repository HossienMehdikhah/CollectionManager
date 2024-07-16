using CollectionManager.Core.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.WinUI.Collections;
namespace CollectionManager.WinUI.Utilities;

public class IncrementalSourceMessage(IIncrementalSource<GamePageDTO> incrementalSource)
    : ValueChangedMessage<IIncrementalSource<GamePageDTO>>(incrementalSource)
{
}

public class CurrentPageSourceMessage(GamePageDTO currentPage)
    : ValueChangedMessage<GamePageDTO>(currentPage)
{
}